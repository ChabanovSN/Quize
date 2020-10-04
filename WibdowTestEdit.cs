using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace SecondForms
{
    public class WindowTestEdit : Form
    {
        private GroupBox groupBox1;
    
        private Button NextTestBtn;
        private Button PrevTestBtn;
        private Button SaveTestBtn;
        private Button AddTestBtn;
        private Button RemoveTestBtn;
        private TextBox question;
        private TextBox Thema;
        private Label ThemaLabel;
        private Button ChangeThemaName;
        private Label titleNumberTest;
        private Label numberTest;
        List<Test> tests = new List<Test>(); 
        int index = 0;
        bool addBool = false;
        string Path { get; set; }
        string ThemeFromPath="";
        public WindowTestEdit(string path)
        {
            // TestRun test = new TestRun(mapTest[selectedTheme]);
            //  Console.WriteLine("111" + mapTest[selectedTheme]);
            //test._Write();
            Path = path;
                tests.AddRange(TestRun.ReadFile(path));
                
           
          
            ThemeFromPath = Path.Remove(0, Path.LastIndexOf('/') + 1);
            ThemeFromPath = ThemeFromPath.Remove(ThemeFromPath.IndexOf('.'));           
            InitializeRadioButtons();
          

        }
        public void InitializeRadioButtons()
        {
            var currentSize = Font.SizeInPoints;
            currentSize += 3;
            Font = new Font(Font.Name, currentSize,
                 Font.Style);

            this.question = new TextBox
            {
                Location = new Point(30, 30),
               
            };
            ThemaLabel = new Label
            {
                Location = new Point(30, 5),
                Text = "Тема: ",

            };
            Thema = new TextBox
            {
                Location = new Point(100, 5),
                Width = 220,
                Text  = ThemeFromPath

            };

            ChangeThemaName = new Button {
                Location = new Point(350, 5),
                Text     = "Сменить название темы",
                 Width = 250
            };
            ChangeThemaName.Click += ChangeThemaName_Click;


        titleNumberTest = new Label
            {
                Location = new Point(30,55),
                Text     = "Номер теста"
            };
            numberTest = new Label
            {
                Location = new Point(200, 55),
                Text = "1"
            };

            this.groupBox1 = new GroupBox
            {
                Location = new Point(30, 80),
                Size = new Size(800, 280)
            };
            Controls.Add(this.question);
            Controls.Add(Thema);
            Controls.Add(ThemaLabel);
            Controls.Add(ChangeThemaName);
            Controls.Add(titleNumberTest);
            Controls.Add(numberTest);
           

           
            this.PrevTestBtn = new Button
            {
                Location = new Point(30, 370),
                Size = new Size(140, 25),
                Text = "Предыдущий",

            };
            this.PrevTestBtn.Click += PrevTest_Click;
            this.NextTestBtn = new Button
            {
                Location = new Point(185, 370),
                Size = new Size(140, 25),
                Text = "Следующий"
            };
            this.NextTestBtn.Click += NextTest_Click;

             AddTestBtn = new Button
             {
                 Location = new Point(355, 370),
                 Size = new Size(140, 25),
                 Text = "Добавить"
             };
            this.AddTestBtn.Click += Add_Test_Click;
        
           RemoveTestBtn = new Button
           {
               Location = new Point(525, 370),
               Size = new Size(140, 25),
               Text = "Удалить"
           };
            this.RemoveTestBtn.Click += Remove_Test_Click;


            this.SaveTestBtn = new Button
            {
                Location = new Point(695, 370),
                Size = new Size(140, 25),
                Text = "Сохранить",

            };
            this.SaveTestBtn.Click += SaveTest_Click;

            this.ClientSize = new Size(850, 420);
            this.Controls.Add(groupBox1);
            Controls.Add(PrevTestBtn);
            Controls.Add(NextTestBtn);
            Controls.Add(AddTestBtn);
            Controls.Add(RemoveTestBtn);
            Controls.Add(SaveTestBtn);

            NextTest();
        }

        void ChangeThemaName_Click(object sender, EventArgs e)
        {

            int ind = 0;
          
                //поиск индекса последнего слеша
                ind = Path.LastIndexOf('/');
            //переименование
            string newName = Path.Remove(ind + 1) + Thema.Text + ".json";
           //    MessageBox.Show($"{Path} {ind} {newName}");
           File.Move(Path, newName);
            Path = newName;
           // MessageBox.Show($"{Path} {ind} {newName}");

        }


        void NextTest()
        {
            this.question.Width = 800;
            this.question.Height = 35;
            this.question.Name = "Qustion";
            int x = 30, y = 40, row = 1;
            this.groupBox1.Controls.Clear();
            numberTest.Text = (index + 1).ToString();
            if (tests?.Count > 0 && index < tests?.Count)
            {

                   question.Text = " " + tests[index].Qustion;               
               foreach (var t in tests[index].ListsOfAnswers)
                {

                    CheckBox check = new CheckBox
                    {
                        Size = new Size(20, 25),
                        Location = new Point(x, y),
                        Name = "check" + row.ToString(),
                        Checked = (t.Value ? true : false)
                    };
                                    
            
                    TextBox answer = new TextBox
                    { Size = new Size(650, 30),
                        Location = new Point(x + 30, y),
                        Text = t.Key,
                        Name = "answer" + row.ToString()
                    };                   
                    this.groupBox1.Controls.Add(check);
                    this.groupBox1.Controls.Add(answer);                  
                    y += 40; row++;
                }
             

            }else if (addBool)
            {
                this.question.Text = "Вопрос";

                for (int i = 0; i < 4; i++)
                {
                    CheckBox check = new CheckBox
                    {
                        Size = new Size(20, 25),
                        Location = new Point(x, y),
                        Name = "check" + row.ToString(),
                       
                    };

                    TextBox answer = new TextBox
                    {
                        Size = new Size(650, 30),
                        Location = new Point(x + 30, y),
                        Name = "answer" + row.ToString()
                    };
                    this.groupBox1.Controls.Add(check);
                    this.groupBox1.Controls.Add(answer);
                    y += 40; row++;
                } 
            }
        }

        void EditCreateListTests()
        {
            try
            {


                Test test = new Test
                {
                    Qustion = ((TextBox)Controls["Qustion"]).Text.Trim(),

                };           
                for (int i = 0; i < groupBox1.Controls.Count/2; i++)
                {
                    CheckBox check = (CheckBox)groupBox1.Controls["check"+(i+1).ToString()];
                    TextBox newAns = (TextBox)groupBox1.Controls["answer"+ (i + 1).ToString()];
                    Console.WriteLine($" {newAns.Text.Trim()}   {check.Checked}");
                    test.ListsOfAnswers[newAns.Text.Trim()] = check.Checked ? true : false;
                }
                Console.WriteLine($" tests.Count {tests.Count}");
                if(test.ListsOfAnswers.Count != 4 || test.Qustion.Length==0)
                {
                    MessageBox.Show("Ответы одинаковые или есть пустные поля");
                  
                    return;
                }

                if (test.Qustion.Length > 0)
                    if (index < tests.Count)
                        tests[index] = test;
                    else
                    {
                        AddTestBtn.Text = "Добавить";
                        tests.Add(test);
                    }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
          

        }
        void Change_Thema_Click(object sender, EventArgs e) {
            int ind = 0;

                //поиск индекса последнего слеша
                ind = Path.LastIndexOf('\\');
            //переименование
            MessageBox.Show(Path);
              //  File.Move(Path, Path.Remove(ind + 1) + ThemeFromPath + ".json");
           

        }
        void Add_Test_Click(object sender, EventArgs e)
    {
            AddTestBtn.Text = "Потвердить";

            if (addBool)
            {
                EditCreateListTests();
                addBool = false;
            }
            else
            {
                addBool = true;
                index = tests.Count;              
                NextTest();
            }
    }
    void Remove_Test_Click(object sender, EventArgs e)
    {
            tests.RemoveAt(index);
            if (index >= tests.Count) index = tests.Count - 1;
            NextTest();
    }
    void PrevTest_Click(object sender, EventArgs e)
        {
            EditCreateListTests();
            --index;
            if (index < 0) index = tests.Count - 1;       
            NextTest();
        }
            
            void NextTest_Click(object sender, EventArgs e)
        {
              EditCreateListTests();           
              index = (index + 1) % tests.Count;           
              NextTest();
        }

        void SaveTest_Click(object sender, EventArgs e)
        {
            EditCreateListTests();
            _Write();
            Form ifrm = Application.OpenForms[0];
            if (ifrm is Quiz quiz)
            {
                quiz.ShoosFile_Click();
                quiz.Show();
                this.Close();
            }
        
        }
        void _Write()
        {
            using (FileStream fs = new FileStream(Path, FileMode.Create))
            {
                byte[] jsonUtf8Bytes;
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    
                };
                        
                jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(tests, options);
                fs.Write(jsonUtf8Bytes, 0, jsonUtf8Bytes.Length);

            }
        }

    }

}
