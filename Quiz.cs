using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Quize
{
    public class Quiz : Form
    {
        private CheckedListBox CheckedListBoxTest;
        private User CurrentUser = null;
        private ToolStrip toolStrip1;     
        private ToolStripButton mEditFiles;
        private ToolStripButton mCreatFiles;
        private ToolStripButton mDeleteFiles;
        private  bool editFileBool = false;
        private Button StartBtn;
        private Button ShowRezultBtn;
        private Button Top20Btn;
        private Button ChangeUserBtn;
        private Button SettingBtn;
        private readonly string config = "config.txt";
        private string PathToDir { get; set; } //= @"/home/doka/Рабочий стол/Store/Tests";
        private string PathToFileAuth { get; set; }// = @"/home/doka/Рабочий стол/Store/auth.json";
        readonly Dictionary<string, string> mapTest = new Dictionary<string, string>();
      
        public Quiz()
        {
            if (SetConfig())
            {
                InitializeComponent();
                CheckedListBoxTest.CheckOnClick = true;
                ShoosFile_Click();
            }
            else
            {
                MessageBox.Show("Настройте файл конфигурации");
                this.Close();
            }



        }
        public void SetUser(User user)
        {
            CurrentUser = user;         
            this.Text = $"{user.Login} участвует в Викторине";
            if (user.IsAdmin)
            {
                toolStrip1.Show();
                this.mEditFiles.Click += EditTest;

            }
            else
            {
                toolStrip1.Hide();
                this.mEditFiles.Click -= EditTest;

            }


        }
       public  void WriteRezult() {
            Authorization authorization = new Authorization(PathToFileAuth);
                authorization.WriteRezult(CurrentUser);
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
            if (CurrentUser == null)
            {
                Check_User_Click(null, null);
                return;
            }

            if (editFileBool == false)
            {
                if (CheckedListBoxTest.CheckedItems.Count > 0)
                {
                    List<string> paths = new List<string>();
                    for (int i = 0; i < CheckedListBoxTest.CheckedItems.Count; i++)
                    {
                        paths.Add(mapTest[CheckedListBoxTest.CheckedItems[i].ToString().Trim()]);
                    }
                    WindowTest windowTest = new WindowTest(paths,CurrentUser);
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
                StartBtn.Hide();
                ShoosFile_Click();
                StartBtn.Click -= Start_Click;
                CheckedListBoxTest.SelectedIndexChanged += Start_Click;
            }
            else
            {
                mEditFiles.Text = "Редактировать";               
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
         void DeleteTest(object sender, EventArgs e)
        {
            if (CheckedListBoxTest.CheckedItems.Count > 0)
            {
                List<string> paths = new List<string>();
                for (int i = 0; i < CheckedListBoxTest.CheckedItems.Count; i++)
                {
                    paths.Add(mapTest[CheckedListBoxTest.CheckedItems[i].ToString().Trim()]);
                }

                foreach (string file in paths)
                {
                    FileHelper.DeleteFile(file);
                }

            }
            else
                MessageBox.Show("Нужно выбрать тему");

            ShoosFile_Click();
        }
        void ShowRezultBtn_Click(object sender, EventArgs e)
        {
            if (CurrentUser != null)
            {
                ShowRezults rezults = new ShowRezults(CurrentUser);
                rezults.Show();
            }
            else
                Check_User_Click(null, null);
        }
        void ChangeUserBtn_Click(object sender, EventArgs e)
        {

                Authorization changeUser = new Authorization(PathToFileAuth);
                changeUser.Show();
                this.Hide();
                      
        }
        void SettingBtn_Click(object sender, EventArgs e)
        {
            if (CurrentUser != null)
            {
                Authorization setting = new Authorization(PathToFileAuth,CurrentUser);
                setting.Show();
            }
            else
                Check_User_Click(null, null);
        }

        void Top20Btn_Click(object sender, EventArgs e)
        {
            if (CurrentUser == null)
            {
                Check_User_Click(null, null);
                return;
            }
           if (CheckedListBoxTest.CheckedItems.Count != 1)
            {

                MessageBox.Show("Нужно выбрать 1 тему");
                return;
            }
            Authorization auth = new Authorization(PathToFileAuth);
            string thema = CheckedListBoxTest.CheckedItems[0].ToString().Trim();
            List<User> usersTop = auth.GetTop20(thema);
          if (usersTop.Count < 1)
            {
                MessageBox.Show("Список пуст");
                return;
            }
           ShowRezults rezults = new ShowRezults(usersTop, thema);
            rezults.Show();
        }

        private bool SetConfig() {
            if (!File.Exists(config))
            {
                using (StreamWriter sw = new StreamWriter(config))
                {
                    sw.WriteLine("***Путь к директории с темами***");
                    sw.WriteLine("PathToDir=");
                    sw.WriteLine("***Путь к файлу с пользователями***");
                    sw.WriteLine("PathToFileAuth=");

                }
            }

            using (StreamReader sr = new StreamReader(config))
            {
                while (sr.Peek() >= 0)
                {
                    string str = sr.ReadLine();
                    if (!str.StartsWith("***") && str.Length>10){
                        int pos = str.IndexOf('=');                     
                        if (pos + 1==str.Length){
                            return false;
                        }

                        if (str.StartsWith("PathToDir="))
                        {
                            PathToDir = str.Substring(pos + 1);

                        }
                        if (str.StartsWith("PathToFileAuth="))
                        {
                            PathToFileAuth = str.Substring(pos + 1);

                        }
                    }
                }

            }
            return true;

        }

        private void InitializeComponent()
        {
            StartPosition = FormStartPosition.CenterScreen;

            var currentSize = Font.SizeInPoints;
            currentSize += 3;
            BackColor = Color.Aqua;
            Font = new Font(Font.Name, currentSize, Font.Style);           
            CheckedListBoxTest = new CheckedListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            mCreatFiles = new System.Windows.Forms.ToolStripButton();           
            mEditFiles = new System.Windows.Forms.ToolStripButton();
            mDeleteFiles = new System.Windows.Forms.ToolStripButton();
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
                Location = new Point(25, 205),
                Text = "Старт"
            };
            StartBtn.Click += Start_Click;
           // StartBtn.Click += Check_User_Click;
         
            Controls.Add(StartBtn);

           ShowRezultBtn = new Button
            {
                Location = new Point(105, 205),
                Text = "Результаты"
            };
            ShowRezultBtn.Click += ShowRezultBtn_Click;
            Controls.Add(ShowRezultBtn);
            Top20Btn = new Button
            {
                Location = new Point(185, 205),
                Text = "Top 20"
            };
            Top20Btn.Click += Top20Btn_Click;
            Controls.Add(Top20Btn);
            int h = Height, w = Width;
            ChangeUserBtn = new Button
            {
                Location = new Point(2, h - 60),
                Text = "Сменить пользователя",
                Width = w -100
              
            };
            ChangeUserBtn.Click +=ChangeUserBtn_Click;
            Controls.Add(ChangeUserBtn);
            SettingBtn = new Button
            {
                Location = new Point(w - 100, h - 60),
                Text = "Настройки",
                Width =83

            };
            SettingBtn.Click += SettingBtn_Click;
            Controls.Add(SettingBtn);
            //
            // toolStrip1
            //
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            mEditFiles,  mCreatFiles, mDeleteFiles});
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


            mDeleteFiles.ImageTransparentColor =
              System.Drawing.Color.Azure;
            mDeleteFiles.Name = "Delete";
            mDeleteFiles.Size = new System.Drawing.Size(148, 22);
            mDeleteFiles.Text = "Удалить";
            mDeleteFiles.Click += DeleteTest;
           
            //////
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(CheckedListBoxTest);
            this.Controls.Add(this.toolStrip1);
            toolStrip1.Hide();
            this.Name = "Quiz";
           
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

       
        }

}




