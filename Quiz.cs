using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SecondForms
{
    public class Quiz : Form
    {
        private CheckedListBox CheckedListBoxTest;
        private User CurrentUser = null;
        private ToolStrip toolStrip1;
        private ToolStripButton mCreatFiles;
        private ToolStripButton mEditFiles;
        private  bool editFileBool = false;
        private Button StartBtn;
        string PathToDir { get; set; } = @"/home/doka/Рабочий стол/Store/Tests";
        private string PathToFileAuth { get; set; } = @"/home/doka/Рабочий стол/Store/auth.json";
        readonly Dictionary<string, string> mapTest = new Dictionary<string, string>();
        public Quiz()
        {
            InitializeComponent();
            CheckedListBoxTest.CheckOnClick = true;
            CheckedListBoxTest.DrawMode = DrawMode.OwnerDrawFixed;
            CheckedListBoxTest.DrawItem += ListBox_DrawItem;
            ShoosFile_Click();

        }
        public void SetUser(User user)
        {
            CurrentUser = user;
            StartBtn.Click += Start_Click;           

            if (user.IsAdmin)
            {
                toolStrip1.Show();
                this.mEditFiles.Click += EditTest;

            }


        }
        void Check_User_Click(object sender, EventArgs e)
        {  
            if(CurrentUser == null)
            {
                Authorization authorization = new Authorization(PathToFileAuth);
                authorization.Show();
                this.Hide();

            }
           

        }
        public void ShoosFile_Click()
        {
            // textBox1.Text;
                                                           // получаем все файлы
            string[] files = Directory.GetFiles(PathToDir);
            // перебор полученных файлов
            CheckedListBoxTest.Items.Clear();
            foreach (string file in files)
            {
                string title = Path.GetFileNameWithoutExtension(file);               
                mapTest[title] = file;
                CheckedListBoxTest.Items.Add(title.PadLeft(48));

            }


        }
        void Start_Click(object sender, EventArgs e)
        {
            if (editFileBool == false)
            {
                if (CheckedListBoxTest.CheckedItems.Count > 0)
                {
                    List<string> paths = new List<string>();
                    for (int i = 0; i < CheckedListBoxTest.CheckedItems.Count; i++)
                    {
                        paths.Add(mapTest[CheckedListBoxTest.CheckedItems[i].ToString().Trim()]);
                    }
                    WindowTest windowTest = new WindowTest(paths);
                    windowTest.Show();
                    this.Hide();
                }
                else
                    MessageBox.Show("Нужно выбрать тему");
            }
            else
            {

                if (CheckedListBoxTest.CheckedItems.Count > 0)
                {
                    WindowTestEdit windowTest = new WindowTestEdit(mapTest[CheckedListBoxTest.CheckedItems[0].ToString().Trim()]);
                    windowTest.Show();
                    this.Hide();
                }
                else
                    MessageBox.Show("Нужно выбрать тему для редактирования");

            }
        }

        void EditTest(object sender, EventArgs e)
        {
           
            editFileBool = !editFileBool;
            if (editFileBool)
            {
                mEditFiles.Text = "Отключить редактирование";
                CheckedListBoxTest.DrawItem -= ListBox_DrawItem;
                CheckedListBoxTest.DrawItem += ListBox_DrawItem_Gray;
                StartBtn.Hide();
                ShoosFile_Click();
                StartBtn.Click -= Start_Click;
                CheckedListBoxTest.SelectedIndexChanged += Start_Click;
            }
            else
            {
                mEditFiles.Text = "Редактировать";
                CheckedListBoxTest.DrawItem += ListBox_DrawItem;
                CheckedListBoxTest.DrawItem -= ListBox_DrawItem_Gray;
                StartBtn.Show();
                CheckedListBoxTest.SelectedIndexChanged -= Start_Click;
                StartBtn.Click += Start_Click;
                ShoosFile_Click();
            }

           

        }
        void CreateTest(object sender, EventArgs e)
        {   
            AddNewFileForm addNew = new AddNewFileForm(PathToDir);
            addNew.Show();
            this.Hide();          
        }

        private void InitializeComponent()
        {
          
            var currentSize = Font.SizeInPoints;
            currentSize += 3;
            BackColor = Color.Aqua;
            Font = new Font(Font.Name, currentSize, Font.Style);
            System.ComponentModel.ComponentResourceManager resources =
              new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            CheckedListBoxTest = new CheckedListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            mCreatFiles = new System.Windows.Forms.ToolStripButton();           
            mEditFiles = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            //
            //CheckedListBoxTest
            //
            CheckedListBoxTest.Dock = System.Windows.Forms.DockStyle.Fill;
            CheckedListBoxTest.FormattingEnabled = false;
            CheckedListBoxTest.Location = new System.Drawing.Point(50, 50);
            CheckedListBoxTest.Name = "mListBox";
            CheckedListBoxTest.Size = new System.Drawing.Size(230, 200);
            CheckedListBoxTest.TabIndex = 0;
            //
            StartBtn = new Button
            {
                Location = new Point(20, 205),
                Text = "Старт"
            };
            StartBtn.Click += Check_User_Click;
         
            Controls.Add(StartBtn);
            //
            // toolStrip1
            //
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            mEditFiles,  mCreatFiles});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(284, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            //
       

            //mEditFiles
            this.mEditFiles.ImageTransparentColor =
               System.Drawing.Color.Azure;
            this.mEditFiles.Name = "Edit";
            this.mEditFiles.Size = new System.Drawing.Size(148, 22);
            mEditFiles.Text = "Редактировать";
            this.mEditFiles.Click += Check_User_Click;
           
            //mCreatFiles
            this.mCreatFiles.ImageTransparentColor =
               System.Drawing.Color.Azure;
            this.mCreatFiles.Name = "Edit";
            this.mCreatFiles.Size = new System.Drawing.Size(148, 22);
            mCreatFiles.Text = "Добавить новый";
            this.mCreatFiles.Click += CreateTest;
            

            //////
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(CheckedListBoxTest);
            this.Controls.Add(this.toolStrip1);
            toolStrip1.Hide();
            this.Name = "Quiz";
            this.Text = "Quiz";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        void ListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1)
            {
                if ((e.Index & 1) == 1)
                    e.Graphics.FillRectangle(Brushes.Coral, e.Bounds);

                using (Brush textBrush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(CheckedListBoxTest.Items[e.Index].ToString(), e.Font, textBrush, e.Bounds.Location);
                }
            }
        }

        void StartBtn_Click(object sender, EventArgs e)
        {
        }


        void ListBox_DrawItem_Gray(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1)
            {

                    e.Graphics.FillRectangle(Brushes.Gray, e.Bounds);

                using (Brush textBrush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(CheckedListBoxTest.Items[e.Index].ToString(), e.Font, textBrush, e.Bounds.Location);
                }
            }
        }
    }

}




