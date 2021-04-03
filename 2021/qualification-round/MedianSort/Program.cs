using System;
using System.Collections.Generic;
using System.Linq;

namespace MedianSort
{
    class Program
    {        
        static void Main(string[] args)
        {            
            using (IOHelper helper = new IOHelper(args))
            {
                int[] array = helper.ReadLineAsArray<int>();
                int testCases = array[0];
                int elements = array[1];
                int questions = array[2];

                for (int testCase = 1; testCase <= testCases; testCase++)                
                {                    
                    Solve(helper, elements);
                }               
            }
        }

        static void Solve(IOHelper helper, int elements)
        {
            List<int> numbers = new List<int>();            
            numbers.AddRange(new int[] {1, 2});

            for (int numberToInsert=3; numberToInsert<=elements; numberToInsert++)
            {
                int startIndex = 0;
                int endIndex = numbers.Count-1;
                int pos = InsertNumberAtPos(helper, numbers, numberToInsert, startIndex, endIndex);
                numbers.Insert(pos, numberToInsert);
            }            

            helper.WriteLine(string.Join(' ', numbers));
            int result = helper.ReadLine<int>();            
        }

        static int InsertNumberAtPos(IOHelper helper, List<int> numbers, int numberToInsert, int startIndex, int endIndex)
        {
            int firstNumIndex, secondNumIndex;
            GetNumbersIndexes(startIndex, endIndex, out firstNumIndex, out secondNumIndex);

            String output = $"{numberToInsert} {numbers[firstNumIndex]} {numbers[secondNumIndex]}";
            helper.WriteLine(string.Join(' ', output));
            int median = helper.ReadLine<int>();

            if (median == numberToInsert) // Insert in middle part
            {
                if (firstNumIndex == secondNumIndex-1)
                    return secondNumIndex;
                else
                    return InsertNumberAtPos(helper, numbers, numberToInsert, firstNumIndex, secondNumIndex);                
            }
            else if (median == numbers[firstNumIndex]) // Insert in first part
            {
                if (startIndex == firstNumIndex)
                    return startIndex;
                else
                    return InsertNumberAtPos(helper, numbers, numberToInsert, startIndex, firstNumIndex);                
            }
            else // Insert in last part
            {
                if (endIndex == secondNumIndex)
                    return endIndex+1;
                else
                    return InsertNumberAtPos(helper, numbers, numberToInsert, secondNumIndex, endIndex);
            }
        }
        
        static void GetNumbersIndexes(int startIndex, int endIndex, out int firstNumIndex, out int secondNumIndex)
        {
            // Divide first part for 3 and second for 2
            double length = endIndex-startIndex+1;
            int secondIndex = (int)Math.Round((length+2)/3);            
            double rest = length-secondIndex;
            int firstIndex = (int)Math.Round((rest+1)/2);

            secondNumIndex = endIndex-secondIndex+1;
            firstNumIndex = startIndex+firstIndex-1;            
        }
    }

    public class IOHelper : IDisposable
    {
        private readonly System.IO.TextReader _readerInput;        
        private readonly System.IO.TextReader _readerOutput;
        private readonly System.IO.TextWriter _writer;
        private readonly bool _debug;
        public bool DebugOn { get; set; }

        public IOHelper(string[] args)
        {            
            DebugOn = true;
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

            RecreateLogFile();
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

        // Clear log file
        private void RecreateLogFile()
        {
            if (System.IO.File.Exists("data.log")) 
            {
                using (System.IO.TextWriter log = new System.IO.StreamWriter("data.log"))
                    log.Close();
            }
        }     

        // You can use this to "debug" in interactive problems
        public void Log(object value)
        {
            // Create this file in your root project to safe log
            if (System.IO.File.Exists("data.log") && DebugOn) 
            {
                using (System.IO.TextWriter log = new System.IO.StreamWriter("data.log", true))
                {
                    log.WriteLine(string.Format("{0}", value));
                    log.Close();
                }
            }
        }

        // You can use this as a safe way to console your variables
        public void PrintLine(object value)
        {
            if (_debug && DebugOn)
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
        }
    }
}
