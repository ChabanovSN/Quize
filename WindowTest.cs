using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
namespace Quize
{
    public class WindowTest : Form
    {
        private CheckedListBox CheckedListBoxTest;     
        private Button AnswerdBtn;
        private Label question;
        private Label titleNumberTest;
        private Label numberTest;
        private Label titleScore;
        private Label score;
        private User user;
        List<Test> tests =new List<Test>();
        int index = 0;
        int rezultScore = 0;
     
        string Thema = null;
        public WindowTest(List<string> paths, User user)
        {
            if (Application.OpenForms["WindowTest"] != null) return;
        
        if (paths.Count == 1)
              Thema=  Path.GetFileNameWithoutExtension(paths[0]);
        
         

            this.user = user;
            foreach (var path in paths)
            {
                tests.AddRange(TestRun.ReadFile(path));
            }
            Random random = new Random();
            for (int i = tests.Count - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);    
                var temp = tests[j];
                tests[j] = tests[i];
                tests[i] = temp;
            }
            if (tests.Count < 20)
                tests = tests.GetRange(0, tests.Count);
            else
                tests = tests.GetRange(0, 20);

            InitializeRadioButtons();

            CheckedListBoxTest.CheckOnClick = true;
        }
        public void InitializeRadioButtons()
        {
            BackColor = Color.BlanchedAlmond;
            StartPosition = FormStartPosition.CenterScreen;
            Name = "WindowTest";
            var currentSize = Font.SizeInPoints;
            currentSize += 3;
            Font = new Font(Font.Name, currentSize,
                 Font.Style);
            this.question = new Label
            {
                Location = new Point(30, 5),
                Width = 800,
                Height = 35
        };
            titleScore = new Label
            {
                Location = new Point(200, 370),
                Size = new Size(120, 25),
                Text = "Верно "

            };
            score = new Label
            {
                Location = new Point(330, 370),
                Size = new Size(100, 25),
                Text = "0"

            };
                     
            CheckedListBoxTest = new CheckedListBox {
               Location = new Point(30, 65),
               Size = new Size(800, 300),
            
        };


            Controls.Add(question);
            titleNumberTest = new Label
            {
                Location = new Point(30, 40),
                Text = "Номер теста"
            };
            numberTest = new Label
            {
                Location = new Point(200, 40),
                Text = "1"
            };
            this.AnswerdBtn = new Button
            {
                Location = new Point(30, 370),
                Size = new Size(150, 25),
                Text = "Ответить"
            };
            AnswerdBtn.Click += Answered_Click;
            ClientSize = new Size(850, 420);
            Controls.Add(titleNumberTest);
            Controls.Add(numberTest);
            Controls.Add(CheckedListBoxTest);
            Controls.Add(AnswerdBtn);
            Controls.Add(titleScore);
            Controls.Add(score);
            NextTest();
        }
     

        void NextTest() {

            if (tests?.Count > 0 && index < tests?.Count)
            {

               
                this.question.Text = " " + tests[index].Qustion;
                numberTest.Text = (index + 1).ToString();
                CheckedListBoxTest.Items.Clear();
               
                foreach (var t in tests[index].ListsOfAnswers)
                {
                    if (t.Key.Length > 0)                      
                        CheckedListBoxTest.Items.Add("  "+t.Key);
                
                }

            }

        }


       

    
        void Answered_Click(object sender, EventArgs e)
        {
            if (AnswerdBtn.Text == "Mеню")
            {
               
                Quiz ifrm = (Quize.Quiz)Application.OpenForms[0];
                if (Thema !=null)
                {
                    try
                    {
                        user.Add(Thema, rezultScore);
                        ifrm.SetUser(user);
                        ifrm.WriteRezult();
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine(" format exception in Windwow Test");
                    }
                }
                ifrm.Show();
                this.Close();
                return;
            }

            if (CheckedListBoxTest.CheckedItems.Count==0)
            {
                MessageBox.Show(" Нужно выбрать ответ");
                return;
            }

            bool rezult = false;
            foreach (var item in CheckedListBoxTest.Items)
            {
                if (                 
                  CheckedListBoxTest.GetItemCheckState(CheckedListBoxTest.Items.IndexOf(item)) 
                                                                      == CheckState.Checked
                  & tests[index].ListsOfAnswers[item.ToString().Trim()] != true
                                                                      )
                {
                    rezult = false;
                    break;
                }else
                 if(
                    CheckedListBoxTest.GetItemCheckState(CheckedListBoxTest.Items.IndexOf(item))
                                                                      == CheckState.Unchecked
                  & tests[index].ListsOfAnswers[item.ToString().Trim()] == true
                )
                {
                    rezult = false;
                    break;
                }else
                rezult = true;
            }
         

                if (index <tests.Count) { 
                     if(rezult)                  
                        rezultScore++;                  

                    score.Text = $"{rezultScore.ToString()} из {tests.Count}";
                    index++;
                    if(index== tests.Count)
                        this.AnswerdBtn.Text = "Mеню";
                    else
                    NextTest();
                }
               
            
        }
    }

}
