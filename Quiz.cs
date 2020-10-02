using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SecondForms
{
    public class Quiz : Form
    {
        private ListBox mListBox;
        private ToolStrip toolStrip1;
        private ToolStripButton mCreatFiles;
        private ToolStripButton mEditFiles;
        private  bool editFileBool = false;
        readonly Dictionary<string, string> mapTest = new Dictionary<string, string>();
        public Quiz()
        {
            InitializeComponent();
            mListBox.DrawMode = DrawMode.OwnerDrawFixed;
            mListBox.DrawItem += ListBox_DrawItem;
            ShoosFile_Click();

        }

        private void ShoosFile_Click()
        {
            string path = @"/home/doka/Рабочий стол/Tests";// textBox1.Text;
                                                           // получаем все файлы
            string[] files = Directory.GetFiles(path);
            // перебор полученных файлов
            mListBox.Items.Clear();
            foreach (string file in files)
            {
                string title = file.Remove(0, file.LastIndexOf('/') + 1);
                title = title.Remove(title.IndexOf('.'));
                mapTest[title] = file;
                mListBox.Items.Add(title.PadLeft(48));


            }


        }
        void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (editFileBool == false)
            {
                string selectedTheme = mListBox.SelectedItem.ToString().Trim();
                TestRun test = new TestRun(mapTest[selectedTheme]);
                // test._Write();
                List<Test> tests = test.GetListTests();
                if (tests != null)
                {
                    WindowTest windowTest = new WindowTest(tests);
                    windowTest.Show();
                    this.Hide();
                }
                else
                    // this.Hide();
                    MessageBox.Show("Файл пуст!");
            }
            else
            {
                string selectedTheme = mListBox.SelectedItem.ToString().Trim();

                if (selectedTheme.Length > 0)
                {
                    TestRun test = new TestRun(mapTest[selectedTheme]);
                    Console.WriteLine("111" + mapTest[selectedTheme]);
                    //test._Write();
                    List<Test> tests = test.GetListTests();
                    if (tests != null)
                    {
                        WindowTestEdit windowTest = new WindowTestEdit(tests, mapTest[selectedTheme]);
                        windowTest.Show();
                        this.Hide();
                    }
                    else
                        // this.Hide();
                        MessageBox.Show("Тестов нет!");
                }
            }
        }

        void EditTest(object sender, EventArgs e)
        {
           
            editFileBool = !editFileBool;
            if (editFileBool)
            {
                mEditFiles.Text = "Отключить редактирование";
                mListBox.DrawItem -= ListBox_DrawItem;
                mListBox.DrawItem += ListBox_DrawItem_Gray;
                ShoosFile_Click();
            }
            else
            {
                mEditFiles.Text = "Редактировать";
                mListBox.DrawItem += ListBox_DrawItem;
                mListBox.DrawItem -= ListBox_DrawItem_Gray;
                ShoosFile_Click();
            }

           

        }
        void CreateTest(object sender, EventArgs e)
        {

            MessageBox.Show("Файл create!");
        }

        private void InitializeComponent()
        {
            var currentSize = Font.SizeInPoints;
            currentSize += 3;
            BackColor = Color.Aqua;
            Font = new Font(Font.Name, currentSize, Font.Style);
            System.ComponentModel.ComponentResourceManager resources =
              new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.mListBox = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            mCreatFiles = new System.Windows.Forms.ToolStripButton();
            mEditFiles = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            //
            // mListBox
            //
            this.mListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mListBox.FormattingEnabled = false;
            this.mListBox.Location = new System.Drawing.Point(50, 50);
            this.mListBox.Name = "mListBox";
            this.mListBox.Size = new System.Drawing.Size(230, 230);
            this.mListBox.TabIndex = 0;
            this.mListBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;
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
            this.mEditFiles.Click += EditTest;
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
            this.Controls.Add(this.mListBox);
            this.Controls.Add(this.toolStrip1);

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
                    e.Graphics.DrawString(mListBox.Items[e.Index].ToString(), e.Font, textBrush, e.Bounds.Location);
                }
            }
        }

        void ListBox_DrawItem_Gray(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1)
            {

                    e.Graphics.FillRectangle(Brushes.Gray, e.Bounds);

                using (Brush textBrush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(mListBox.Items[e.Index].ToString(), e.Font, textBrush, e.Bounds.Location);
                }
            }
        }
    }

}




