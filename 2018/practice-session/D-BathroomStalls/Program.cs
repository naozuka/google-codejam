using System;
using System.Collections.Generic;
using System.Linq;

namespace D_BathroomStalls
{
    class Program
    {        
        static void Main(string[] args)
        {            
            using (IOHelper helper = new IOHelper(args))
            {
                int testCases = helper.ReadLine<int>();

                for (int testCase = 1; testCase <= testCases; testCase++)                
                    Resolution(helper, testCase);                
            }
        }

        static void Resolution(IOHelper helper, int testCase)
        {            
            int[] array = helper.ReadLineAsArray<int>();
            int bathroomCount = array[0];
            int peopleCount = array[1];
            string output = "";

            // List with spaces
            //List<int> bathroomSpaces = new List<int>();
            int people = 1;
            // Spaces minSpaces = new Spaces(0, 0);
            // Spaces maxSpaces = new Spaces(0, 0);

            int oddCount = bathroomCount % 2 != 0 ? 1 : 0;
            int evenCount = bathroomCount % 2 == 0 ? 1 : 0;;

            //for (int i=1; i<peopleCount; i++)
            while (peopleCount > people)
            {   
                //bool isEven = bathroomCount % 2 == 0;
                bathroomCount = (bathroomCount)/2;

                int newEvenCount = oddCount*2 + evenCount;
                int newOddCount = evenCount;
                evenCount = newEvenCount;
                oddCount = newOddCount;

                helper.PrintLine($"Spaces: {bathroomCount} | People: {people} | Even: {evenCount} | Odd: {oddCount}");
                people *= 2;
            }

            bool isEven = bathroomCount % 2 == 0;






            
            
            helper.WriteLine(String.Format("Case #{0}: {1}", testCase, output));
        }

        static string UseBathroom(List<int> bathroomSpaces, int spaces)
        {
            int spacesLeft = (spaces-1)/2;
            int spacesRight = spacesLeft;

            if (spaces % 2 == 0) // even
                spacesRight++;

            bathroomSpaces.Add(spacesLeft);    
            bathroomSpaces.Add(spacesRight);

            return $"{spacesRight} {spacesLeft}";
        }
    }

    public class Spaces
    {
        public int Number { get; set; }
        public int Count { get; set; }
        public Spaces(int number, int count)
        {
            Number = number;
            Count = count;
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
