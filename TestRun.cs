using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace SecondForms
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
            string text2 = File.ReadAllText(path, Encoding.UTF8);

            try
            {

                // byte[] jsonUtf8Bytes = Encoding.UTF8.GetBytes(text2);

                //  var utf8Reader = new Utf8JsonReader(jsonUtf8Bytes);
                // tests = JsonSerializer.Deserialize<List<Test>>(text2);
                // ReadListTest(tests);
                //  return tests;
                return tests = JsonSerializer.Deserialize<List<Test>>(text2);

            }
            catch (System.Text.Json.JsonException e)
            {
                tests = null;
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

        //internal static List<Test> GetListTests() {

        //    var f=  ReadFile();
        // //  ReadListTest(f);
        //    return f;
        //  // Read_Write();
        //    //  byte[] jsonUtf8Bytes = new byte[1024];

        //    //  var utf8Reader = new Utf8JsonReader(jsonUtf8Bytes);
        //    //   Test t= JsonSerializer.Deserialize<Test>(ref utf8Reader);

        //    // Test restoredTest = JsonSerializer.Deserialize<Test>(json);
        //    //  Console.WriteLine(restoredTest.Qustion);
        //    //     Console.WriteLine(text);
        //}
        //       public void _Write()
        //        {
        //        //    string path = @"/home/doka/Рабочий стол/Tests";
        //            // сохранение данных
        //            List<Test> tests = new List<Test>();
        //            Test test = new Test
        //            {
        //                Qustion = "Какому из перечисленных определений соответствует понятие \"иммигрант\"?",
        //                ListsOfAnswers = new Dictionary<string, bool>()
        //            {
        //["вселенец"]= true,
        //["акклиматизированный вид"]= false,
        //["акклиматизированный подвид"]= false,
        //["акклиматизированная особь"]=false 
        //    }
        //};
        //    tests.Add(test); tests.Add(test);


        //  using (FileStream fs = new FileStream(Path, FileMode.OpenOrCreate))
        //    {


        //        string json = System.Text.Json.JsonSerializer.Serialize<List<Test>>(tests);

        //        //Console.WriteLine(json);
        //       // File.WriteAllText(Path, json, Encoding.UTF8);

        //    //    string text2 = File.ReadAllText(path + "/user.json", Encoding.UTF8);
        //  //  Console.WriteLine(text2);
        //        byte[] jsonUtf8Bytes;
        //        var options = new JsonSerializerOptions
        //        {
        //           WriteIndented = true
        //        };
        //        jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(tests, options);

        //        fs.Write(jsonUtf8Bytes, 0, jsonUtf8Bytes.Length);

        //        //byte[] bytes = Encoding.UTF8.GetBytes(jsonUtf8Bytes);
        //        //// Write the data to the file, byte by byte.
        //        //for (int i = 0; i < jsonUtf8Bytes.Length; i++)
        //        //{  
        //        //    fs.WriteByte(jsonUtf8Bytes[i]);
        //        //}


        //        //await JsonSerializer.SerializeAsync<Test>(fs, test);
        //        //Console.WriteLine("Data has been saved to file");



        //    }

        //    //using (FileStream fs = new FileStream(path + "/user.json", FileMode.Open))
        //    //{
        //    //        string text2 = File.ReadAllText(path + "/user.json", Encoding.UTF8);
        //    //      Console.WriteLine(text2);

        //    //}

        //}

    }
}
