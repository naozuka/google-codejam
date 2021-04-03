using System;
using System.IO;

namespace A_ForegoneSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            IOHelper helper = new IOHelper(args);            
            Resolution(helper);
            helper.Dispose();            
        }

        static void Resolution(IOHelper helper)
        {
            for (int testCase = 1; testCase <= helper.TestCases; testCase++) 
            {
                int[] numbers = helper.ReadLineAsIntArray();
                int sum = 0;
                foreach (int num in numbers)
                    sum += num;

                helper.WriteLine(sum);
            }
        }
    }       

    public class IOHelper : IDisposable
    {
        private readonly TextReader readerInput;
        private readonly TextReader readerOutput;
        private readonly TextWriter writer;        
        public int TestCases { get; private set; }        
        private int CaseOutput { get; set; }

        public IOHelper(string[] args)
        {
            if (args.Length > 0)
            {                
                readerInput = new StreamReader(args[0] + "small.in");
                readerOutput = new StreamReader(args[0] + "small.out");
            }
            else
            {
                readerInput = System.Console.In;
                writer = System.Console.Out;
            }

            TestCases = ReadLineAsInt();
            CaseOutput = 0;
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
                CaseOutput++;
                string output = String.Format("Case #{0}: {1}", CaseOutput, text);
                string correctOutput = readerOutput.ReadLine();

                if (output == correctOutput)
                    output += " [PASSED]";
                else
                    output += String.Format(" [ERROR - Expected: {0}]", correctOutput);

                Console.WriteLine(output);
            }
        }        

        public void Dispose()
        {            
            readerInput.Close();
            readerOutput.Close();

            if (writer != null)
                writer.Close();
        }
    }
}
