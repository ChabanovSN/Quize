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
        private TextBox question;
        private Label titleNumberTest;
        private Label numberTest;
        List<Test> tests; 
        int index = 0;
        string Path { get; set; }

        internal WindowTestEdit(List<Test> t,string path)
        {
            Path = path;
            tests = t;
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
                Location = new Point(30, 5),

            };
            titleNumberTest = new Label
            {
                Location = new Point(30,40),
                Text     = "Номер теста"
            };
            numberTest = new Label
            {
                Location = new Point(200, 40),
                Text = "1"
            };

            this.groupBox1 = new GroupBox();
             Controls.Add(this.question);
             Controls.Add(titleNumberTest);
             Controls.Add(numberTest);
            this.groupBox1.Location = new Point(30, 65);
            this.groupBox1.Size = new Size(800, 300);

           
            this.PrevTestBtn = new Button
            {
                Location = new Point(30, 370),
                Size = new Size(150, 25),
                Text = "Предыдущий",

            };
            this.PrevTestBtn.Click += PrevTest_Click;
            this.NextTestBtn = new Button
            {
                Location = new Point(200, 370),
                Size = new Size(150, 25),
                Text = "Следующий"
            };
            this.NextTestBtn.Click += NextTest_Click;

            this.SaveTestBtn = new Button
            {
                Location = new Point(370, 370),
                Size = new Size(150, 25),
                Text = "Сохранить",

            };
            this.SaveTestBtn.Click += SaveTest_Click;

            this.ClientSize = new Size(850, 420);
            this.Controls.Add(groupBox1);
            Controls.Add(PrevTestBtn);
            Controls.Add(NextTestBtn);
            Controls.Add(SaveTestBtn);

            NextTest();
        }


        void NextTest()
        {

            if (tests?.Count > 0 && index < tests?.Count)
            {

                this.question.Width = 800;
                this.question.Height = 35;
                this.question.Text = " " + tests[index].Qustion;
                this.question.Name = "Qustion";
                numberTest.Text = (index + 1).ToString();
                int x = 30, y = 40,row=1;
                this.groupBox1.Controls.Clear();
                foreach (var t in tests[index].ListsOfAnswers)
                {

                    RadioButton radio = new RadioButton
                    {
                        Size = new Size(20, 25),
                        Location = new Point(x, y),
                        Name = "radio" + row.ToString()
                    };
                                     
                    radio.Checked |= t.Value != 0;
                    TextBox answer = new TextBox
                    { Size = new Size(650, 30),
                        Location = new Point(x + 30, y),
                        Text = t.Key,
                        Name = "answer" + row.ToString()
                    };
                   
                    this.groupBox1.Controls.Add(radio);
                    this.groupBox1.Controls.Add(answer);                  
                    y += 40; row++;
                }
             

            }

        }

        void EditListTests()
        {
            TextBox newQuastion = (TextBox) Controls["Qustion"];
            RadioButton newR1 = (RadioButton)groupBox1.Controls["radio1"];
            RadioButton newR2 = (RadioButton)groupBox1.Controls["radio2"];
            RadioButton newR3 = (RadioButton)groupBox1.Controls["radio3"];
            RadioButton newR4 = (RadioButton)groupBox1.Controls["radio4"];

            TextBox newAns1 = (TextBox)groupBox1.Controls ["answer1"];
            TextBox newAns2 =(TextBox)groupBox1.Controls  ["answer2"];
            TextBox newAns3 = (TextBox)groupBox1.Controls ["answer3"];
            TextBox newAns4 = (TextBox)groupBox1.Controls ["answer4"];

            Test test = new Test
            {
                Qustion = newQuastion.Text.Trim(),
                ListsOfAnswers = new Dictionary<string, int>()
                {
                    [newAns1.Text.Trim()] = newR1.Checked ? 5 : 0,
                    [newAns2.Text.Trim()] = newR2.Checked ? 5 : 0,
                    [newAns3.Text.Trim()] = newR3.Checked ? 5 : 0,
                    [newAns4.Text.Trim()] = newR4.Checked ? 5 : 0
                }
            };
              Console.WriteLine($" tests.Count {tests.Count}");
            if (test.Qustion.Length > 0)
                if (index < tests.Count)
                    tests[index]= test;
                else 
                    tests.Add(test);
        }
        void PrevTest_Click(object sender, EventArgs e)
        {
            EditListTests();
            --index;
            if (index < 0) index = tests.Count - 1;
           
            Console.WriteLine($" index {index}");
            NextTest();
        }
            
            void NextTest_Click(object sender, EventArgs e)
        {
              EditListTests();
           
           index = (index + 1) % tests.Count;
            Console.WriteLine($" index {index}");
              NextTest();
        }

        void  SaveTest_Click(object sender, EventArgs e)
        {
            EditListTests();
            _Write();
            Form ifrm = Application.OpenForms[0];
            ifrm.Show();
            this.Close();
        }
        void _Write()
        {
            using (FileStream fs = new FileStream(Path, FileMode.Create))
            {
                byte[] jsonUtf8Bytes;
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(tests, options);
                fs.Write(jsonUtf8Bytes, 0, jsonUtf8Bytes.Length);

            }
        }

    }

}
