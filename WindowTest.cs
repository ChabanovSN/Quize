using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SecondForms
{
    public class WindowTest : Form
    {
        private GroupBox groupBox1;
        private RadioButton selectedrb;
        private Button getSelectedRB;
        private Label question;
        private Label titleScore;
        private Label score;

        List<Test> tests;
        int index = 0;
        int rezultScore = 0;
        internal WindowTest(List<Test> t)
        {
            tests = t;
            InitializeRadioButtons();
        }
        public void InitializeRadioButtons()
        {
            var currentSize = Font.SizeInPoints;
            currentSize += 3;
            Font = new Font(Font.Name, currentSize,
                 Font.Style);
            this.question = new Label
            {
                Location = new Point(30, 5),
              
            };
            titleScore = new Label
            {
                Location = new Point(200, 370),
                Size = new Size(120, 25),
                Text = "Результат(%)"

            };
            score = new Label
            {
                Location = new Point(330, 370),
                Size = new Size(40, 25),
                Text = "0"

            };

            this.groupBox1 = new GroupBox();
            Controls.Add(this.question);
            this.getSelectedRB = new Button();
            this.groupBox1.Location = new Point(30, 65);
            this.groupBox1.Size = new Size(800, 300);
            this.getSelectedRB.Location = new Point(30, 370);
            this.getSelectedRB.Size = new Size(150, 25);
            this.getSelectedRB.Text = "Ответить";
            this.getSelectedRB.Click += getSelectedRB_Click;

            this.ClientSize = new Size(850, 420);
            this.Controls.Add(this.groupBox1);
            Controls.Add(this.getSelectedRB);
            Controls.Add(titleScore);
            Controls.Add(score);
            NextTest();
        }
     

        void NextTest() {

            if (tests?.Count > 0 && index < tests?.Count)
            {

                this.question.Width = 800;
                this.question.Height = 35;
                this.question.Text = " " + tests[index].Qustion;

                // this.groupBox1.Text = "Radio Buttons";
                int x = 30, y = 40;
                this.groupBox1.Controls.Clear();
                foreach (var t in tests[index].ListsOfAnswers)
                {
                    if (t.Key.Length > 0)
                    {
                        RadioButton radio = new RadioButton
                        {
                            Location = new Point(x, y)
                        };
                        y += 40 + t.Key.Length / 300;
                        radio.Size = new Size(67, 17);
                        radio.Width = 300;
                        radio.Text = t.Key;
                        radio.CheckedChanged += radioButton_CheckedChanged;
                        this.groupBox1.Controls.Add(radio);
                    }
                }

            }

        }


        void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb == null)
            {
                MessageBox.Show("Sender is not a RadioButton");
               // return;
            }

            // Ensure that the RadioButton.Checked property
            // changed to true.
            if (rb.Checked)
            {
                // Keep track of the selected RadioButton by saving a reference
                // to it.
                selectedrb = rb;
            }
        }

        // Show the text of the selected RadioButton.
        void getSelectedRB_Click(object sender, EventArgs e)
        {
            if (selectedrb != null)
            {

                if (index <tests.Count) {                   
                        rezultScore += tests[index].ListsOfAnswers[selectedrb.Text];                  

                    score.Text = (rezultScore * 20 / tests.Count).ToString();
                    index++;
                    if(index== tests.Count)
                        this.getSelectedRB.Text = "Mеню";
                    else
                    NextTest();
                }
                else
                {
                    if (getSelectedRB.Text == "Mеню") { 
                    Form ifrm = Application.OpenForms[0];
                    ifrm.Show();
                    this.Close();
                    }
                  }

                //MessageBox.Show(selectedrb.Text);
            }
            else
                MessageBox.Show(" Нужно выбрать ответ");
        }
    }

}
