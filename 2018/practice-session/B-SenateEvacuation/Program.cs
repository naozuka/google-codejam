using System;
using System.Collections.Generic;
using System.Linq;

namespace B_SenateEvacuation
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
            int groupsCount = helper.ReadLineAsInt();
            int[] groupsArray = helper.ReadLineAsIntArray();
            string plan = "";
            string team = "";
            bool closePlan = false;
            
            List<Senator> senators = new List<Senator>();
            for (int idx=0; idx<groupsCount; idx++)
                senators.Add(new Senator(idx, groupsArray[idx]));

            while (senators.Count > 0)
            {
                senators = senators.OrderByDescending(x => x.Count).ToList();                
                team += PullSenator(senators);

                int senatorsCount = 0;
                senators.ForEach(x => senatorsCount += x.Count);

                if (senatorsCount == 2 && team.Length == 1)
                    closePlan = true;

                if (team.Length == 2 || closePlan)
                {
                    plan += team + " ";
                    team = "";
                    closePlan = false;
                }                
            }

            helper.WriteLine(String.Format("Case #{0}: {1}", testCase, plan.TrimEnd()));
        }        
        
        static string PullSenator(List<Senator> senators)
        {
            Senator senator = senators[0];            
            if (senator.Count > 1)
                senator.Count = senator.Count-1;
            else
                senators.RemoveAt(0);

            return ((char)(65+senator.Index)).ToString();
        }
    }
    public class Senator
    {
        public int Index { get; set; }
        public int Count { get; set; }

        public Senator(int index, int count)
        {
            Index = index;
            Count = count;
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
