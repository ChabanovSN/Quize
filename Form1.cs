using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quize
{

    public class Form1 : Form
    {
        Button button1;
        Button deletedLabel;
        TextBox textBox1;
        TextBox textBox2;
        Label label;
        private TextBox loggerTextBox;
        public TextBox LoggerTextBox
        {
            get { return loggerTextBox; }
            set { loggerTextBox = value; }
        }
        delegate void Btn();
        event Btn OnBtn;

        private void Init()
        {
            deletedLabel = new Button {
                Text = "DELETE LABEL",
                BackColor = Color.Azure,
                Location = new Point(300, 180)

            };

           button1 = new Button
            {
                Text = "Click me",
                BackColor = Color.Azure,
                Location = new Point(300, 150)
            };
            textBox1 = new TextBox
            {
                Location = new Point(100, 50),
                Size = new Size(50,20)
            };
            textBox2 = new TextBox
            {
                Location = new Point(150, 50),
                Size = new Size(50, 20)
            };
            Controls.Add(
    new Label
    {
        Location = new Point(1, 2),
        Padding = new Padding(2, 3, 2, 5),
        BorderStyle = BorderStyle.FixedSingle,
        Text = "yourdata",
        BackColor = Color.White,
        AutoSize = true
    });



            loggerTextBox = new TextBox
            {
                Location = new Point(1, 205),
                Size = new Size(800, 250),
                // Set the Multiline property to true.
               Multiline = true,
            // Add vertical scroll bars to the TextBox control.
           ScrollBars = ScrollBars.Vertical,
            // Allow the RETURN key to be entered in the TextBox control.
          AcceptsReturn = true,
            // Allow the TAB key to be entered in the TextBox control.
         AcceptsTab = true,
            // Set WordWrap to true to allow text to wrap to the next line.
           WordWrap = true

        };
         
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(textBox2);           
            AddLabel();
            Controls.Add(deletedLabel);
           Controls.Add(LoggerTextBox);
        }

        private void AddLabel()
        {
            label = new Label
            {
                Text = Text1,// this.Controls[0].Handle.ToString(),
                Location = new Point(20, 80),
                AutoSize = true,
               
            Padding = new Padding(2,3,2,5),
               // Size = new Size(160, 30),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
            };
            Controls.Add(label);

        }

        public string Text1
        {
            get { return "My handle is " + this.Handle.ToString(); }
        }

        public Form1()
        {
            Init();

            button1.Click += Button1_Click;
            deletedLabel.Click += DELETE_Click;
            OnBtn = () =>
            {
                if (this.BackColor == Color.Brown)
                {
                    this.BackColor = Color.Red;
                    if (!label.Visible) label.Show();
                }
                else
                {
                    this.BackColor = Color.Brown;
                    if (label.Visible) label.Hide();

                }
            };
           
          
            this.Load += Form1_Load;
            this.MouseMove += Form1_MouseMove;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

            textBox1.Text = e.X.ToString();
            textBox2.Text = e.Y.ToString();

        }


        private void Button1_Click(object sender, EventArgs e)
        {
           // MessageBox.Show("Привет");
            OnBtn();
        }
        private void DELETE_Click(object sender, EventArgs e)
        {
            Controls.Remove(FromHandle(Controls[0].Handle));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Text1;  // "Рабочая поверхность";
            this.BackColor = Color.Brown;
            this.Size = new Size(650, 325);
            this.Location = new Point(300, 300);
            // this.MaximizeBox = false;
        }
   
         private void LogMessage(string message)
        {
            if (LoggerTextBox != null && DesignMode == false)
                LoggerTextBox.Text += message + "\n";
        }
        protected override void WndProc(ref Message m)
        {
            // Вывод тестового сообщения о начале работы оконной процедуры.
            LogMessage("WndProc: start");
            LogMessage(m.ToString());
            // Вызов оконной процедуры предка
            base.WndProc(ref m);
            // Вывод тестового сообщения о завершении работы оконной процедуры.
            LogMessage("WndProc: finish");
        }
        protected override void DefWndProc(ref Message m)
        {
            LogMessage("DefWndProc start");
            LogMessage(m.ToString());
            base.DefWndProc(ref m);
            LogMessage("DefWndProc: finish");
        }
        protected override void OnNotifyMessage(Message m)
        {
            const String MessageTemplate =
              "OnNotifyMessage: Message ID = {0}, LParam = {1}, WParam = {2}";
            LogMessage("OnNotifyMessage: start");
            LogMessage(String.Format(MessageTemplate, m.Msg, m.LParam, m.WParam));
            base.OnNotifyMessage(m);
            LogMessage("OnNotifyMessage: finish");
        }

    }
}
