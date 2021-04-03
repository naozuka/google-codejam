using System;
using System.Collections.Generic;
using System.Linq;


namespace ReversortEngineering
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
            int[] array = helper.ReadLineAsArray<int>();            
            int size = array[0];
            int cost = array[1];

            // 4 6
            // 2 1
            // 7 12
            // 7 2
            // 2 1000

            

            return "";
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
