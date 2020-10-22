using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Quize
{

    class Test
    {

        public string Qustion { get; set; }
        public Dictionary<string, bool> ListsOfAnswers { get; set; }
        public Test()
        {
            ListsOfAnswers = new Dictionary<string, bool>();
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in ListsOfAnswers)
            {
                builder.Append(item.Key + " " + item.Value + "\n");
            }
            return $" {Qustion} {builder.ToString()}";
        }
    }

    public static class TestRun
    {

        internal static List<Test> ReadFile(string path)
        {

            List<Test> tests = null;          
           
                try
                {
                    string text2 = File.ReadAllText(path, Encoding.UTF8);
                    return tests = JsonSerializer.Deserialize<List<Test>>(text2);

                }
                catch (System.Text.Json.JsonException e)
                {
                   
                    Console.WriteLine("JsonException  internal List<Test> ReadFile()\n " + e);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception  internal List<Test> ReadFile()\n " + e.InnerException);

                }

            return tests;
        }

        static void ReadListTest(List<Test> tests)
        {
            foreach (var t in tests)
            {
                Console.WriteLine(t.Qustion);
                foreach (var item in t.ListsOfAnswers)
                {
                    Console.WriteLine(item.Key + " " + item.Value);
                }
            }

        }
       
    }
}
