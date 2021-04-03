using System;

namespace A_NumberGuessing
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IOHelper helper = new IOHelper(args))
            {
                int testCases = helper.ReadLineAsInt();

                for (int testCase = 1; testCase <= testCases; testCase++)                
                    Resolution(helper, testCase);                
            }
        }

        static void Resolution(IOHelper helper, int testCase)
        {
            int[] minMax = helper.ReadLineAsIntArray();
            minMax[0] = minMax[0] + 1;            

            int guesses = helper.ReadLineAsInt();
            helper.Log(String.Format("Min{0} | Max: {1} | Guesses Tries: {2}", minMax[0], minMax[1], guesses));            

            while (true)
            {
                int half = (minMax[0]+minMax[1])/2;                    
                helper.WriteLine(half);

                string result = helper.ReadLine();
                helper.Log(String.Format("Guess: {0} - {1}", half, result));

                if (result == "CORRECT") break;
                else if (result == "TOO_SMALL") minMax[0] = half+1;
                else if (result == "TOO_BIG")  minMax[1] = half-1;
            }
            
            helper.Log("---------------------------------");
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

        public int ReadLineAsInt()
        {
            return int.Parse(readerInput.ReadLine());
        }

        public string ReadLine()
        {
            return readerInput.ReadLine();
        }

        public string[] ReadLineAsStringArray()
        {
            return readerInput.ReadLine().Split(' ');
        }

        public int[] ReadLineAsIntArray()
        {
            string[] strings = readerInput.ReadLine().Split(' ');
            return Array.ConvertAll<string, int>(strings, s => int.Parse(s));
        }

        public void WriteLine(int value)
        {
            WriteLine(value.ToString());
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

        // You can use this for interactive problems
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
