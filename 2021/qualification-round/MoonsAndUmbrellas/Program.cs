using System;
using System.Collections.Generic;
using System.Linq;

namespace MoonsAndUmbrellas
{
    class Program
    {        
        static void Main(string[] args)
        {            
            using (IOHelper helper = new IOHelper(args))
            {
                int testCases = helper.ReadLine<int>();

                for (int testCase = 1; testCase <= testCases; testCase++)                
                {
                    var result = Resolution(helper);
                    helper.WriteLine($"Case #{testCase}: {result}");
                }
            }
        }

        static string Resolution(IOHelper helper)
        {
            string[] array = helper.ReadLineAsArray();
            int cjPrice = int.Parse(array[0]);
            int jcPrice = int.Parse(array[1]);
            string mural = array[2];

            char min = (cjPrice < jcPrice ? 'C' : 'J');

            int index = 0;
            bool goForward = true;
            bool changeDirection = false;
            int savedIndex = -1;
            while (index < mural.Length)
            {
                if (mural[index] == '?')
                {
                    // Situation where you can calculate "?"
                    if ((index > 0 && mural[index-1] != '?') || 
                        (index < mural.Length && mural[index+1] != '?'))
                    {
                        mural = SetBestChoice(mural, index, cjPrice, jcPrice);

                        // if it's going back and is the first "?", turn to forward and go to saved index
                        if (!goForward && (index == 0 || mural[index-1] != '?')) 
                        {
                            goForward = true;
                            index = savedIndex;
                        }

                        // save the current index to return and index will move back
                        if (changeDirection) 
                        {
                            goForward = false;
                            changeDirection = false;
                            savedIndex = index+1;
                        }
                    }
                    else // flag to come back setting previous "?"
                    {
                        changeDirection = true;
                    }
                }

                //helper.PrintLine(mural[i]);
                if (goForward)
                    index++;
                else 
                    index--;
            }

            return CalculateMoney(mural, cjPrice, jcPrice).ToString();
        }

        static string SetBestChoice(string mural, int index, int cjPrice, int jcPrice)
        {
            string bestChoice = "";
            string pre = "";
            string pos = "";
            
            if (index > 0 && mural.Substring(index-1,1) != "?")
                pre = mural.Substring(index-1,1);
            
            if (index < mural.Length-1 && mural.Substring(index+1,1) != "?")
                pos = mural.Substring(index+1,1);
            
            string text = pre + "?" + pos;
            
            if (text == "C?")            
                bestChoice = (cjPrice < 0 ? "J" : "C");
            else if (text == "J?")
                bestChoice = (jcPrice < 0 ? "C" : "J");
            else if (text == "?C")
                bestChoice = (jcPrice < 0 ? "J" : "C");
            else if (text == "?J")
                bestChoice = (cjPrice < 0 ? "C" : "J");
            else if (text == "C?C")
                bestChoice = (cjPrice+jcPrice < 0 ? "J" : "C");
            else if (text == "J?J")
                bestChoice = (cjPrice+jcPrice < 0 ? "C" : "J");
            else if (text == "C?J")
                bestChoice = "C";
            else if (text == "J?C")
                bestChoice = "C";
            else throw new Exception("Not predictaded: " + text);
            
            mural = mural.Remove(index, 1);
            mural = mural.Insert(index, bestChoice);
            return mural;
        }

        static int CalculateMoney(string mural, int cjPrice, int jcPrice)
        {
            int money = 0;
            for (int i=1; i<mural.Length; i++)
            {
                if (mural.Substring(i-1, 2) == "CJ")                
                    money += cjPrice;                
                else if (mural.Substring(i-1, 2) == "JC")
                    money += jcPrice;
            }
            return money;
        }
    }    

    public class IOHelper : IDisposable
    {
        private readonly System.IO.TextReader _readerInput;        
        private readonly System.IO.TextReader _readerOutput;
        private readonly System.IO.TextWriter _writer;
        private readonly bool _debug;
        private System.IO.TextWriter _writerLog;

        public IOHelper(string[] args)
        {            
            if (args.Length > 0)
            {                
                _debug = true;

                if (System.IO.File.Exists(args[0] + "small.in"))
                    _readerInput = new System.IO.StreamReader(args[0] + "small.in");

                if (System.IO.File.Exists(args[0] + "small.out"))
                    _readerOutput = new System.IO.StreamReader(args[0] + "small.out");                
            }
            else
            {
                _debug = false;
                _readerInput = System.Console.In;
                _writer = System.Console.Out;
            }
        }

        public string ReadLine() => _readerInput.ReadLine();        
        public string[] ReadLineAsArray() => _readerInput.ReadLine().Split(' ');        

        public T ReadLine<T>()
        {
            string value = ReadLine();
            return GenericParse<T>(value);
        }

        public T[] ReadLineAsArray<T>()
        {
            string[] strings = ReadLineAsArray();            
            return Array.ConvertAll<string, T>(strings, s => GenericParse<T>(s));
        }

        private T GenericParse<T>(string value)
        {
            Type _t = typeof(T);
            
            // Test for Nullable<T> and return the base type instead:
            Type undertype = Nullable.GetUnderlyingType(_t);
            Type basetype = undertype == null ? _t : undertype;
            return (T)Convert.ChangeType(value, basetype);
        }

        public void WriteLine(string text)
        {            
            if (_writer != null)
            {
                _writer.WriteLine(text);
                _writer.Flush();
            }
            else
            {
                string correctOutput = _readerOutput.ReadLine();

                if (text == correctOutput)
                    text += " [PASSED]";
                else
                    text += String.Format(" [ERROR - Expected: {0}]", correctOutput);

                Console.WriteLine(text);
            }
        }      

        // You can use this to "debug" in interactive problems
        public void Log(object value)
        {
            if (_debug)
            {
                if (_writerLog == null)
                    _writerLog = new System.IO.StreamWriter("data.log");

                _writerLog.WriteLine(string.Format("{0}", value));
            }
        }

        // You can use this as a safe way to console your variables
        public void PrintLine(object value)
        {
            if (_debug)
                Console.WriteLine(value);
        }

        public void Dispose()
        {
            if (_readerInput != null) 
                _readerInput.Close();

            if (_readerOutput != null) 
                _readerOutput.Close();

            if (_writer != null) 
                _writer.Close();

            if (_writerLog != null) 
                _writerLog.Close();
        }
    }
}
