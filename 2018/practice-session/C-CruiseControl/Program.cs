using System;
using System.Collections.Generic;
using System.Linq;

namespace C_CruiseControl
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
            double destination = (double)array[0];            
            double maxTime = 0;
            
            for (int i=0; i<array[1]; i++)
            {
                double[] arrHorse = helper.ReadLineAsArray<double>();
                double time = (destination-arrHorse[0])/arrHorse[1];
                if (time > maxTime)
                    maxTime = time;
            }
            
            double myTime = destination/maxTime;
            helper.WriteLine(String.Format("Case #{0}: {1}", testCase, DoubleToStr(myTime)));
        }

        static string DoubleToStr(double value)
        {
            return String.Format("{0:0.000000}", value).Replace(',', '.');
        }
    }

    public class IOHelper : IDisposable
    {
        private readonly System.IO.TextReader readerInput;        
        private readonly System.IO.TextReader readerOutput;
        private readonly System.IO.TextWriter writer;
        private System.IO.TextWriter writerLog;

        public IOHelper(string[] args)
        {            
            if (args.Length > 0)
            {                
                readerInput = new System.IO.StreamReader(args[0] + "small.in");
                readerOutput = new System.IO.StreamReader(args[0] + "small.out");
            }
            else
            {
                readerInput = System.Console.In;
                writer = System.Console.Out;
            }
        }

        public string ReadLine() => readerInput.ReadLine();        
        public string[] ReadLineAsArray() => readerInput.ReadLine().Split(' ');        

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
            if (writer != null)
            {
                writer.WriteLine(text);
                writer.Flush();
            }
            else
            {
                string correctOutput = readerOutput.ReadLine();

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
            if (writerLog == null)
                writerLog = new System.IO.StreamWriter("data.log");

            writerLog.WriteLine(string.Format("{0}", value));
        }  

        public void Dispose()
        {
            readerInput.Close();
            
            if (readerOutput != null)
                readerOutput.Close();

            if (writer != null)
                writer.Close();

            if (writerLog != null)
                writerLog.Close();
        }
    }
}
