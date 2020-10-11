using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace SecondForms
{
    public class Authorization:Form
    {
        private List<User> users = new List<User>();
        private string PathToFile { get; set; }
        
        private TextBox LoginText;
        private TextBox PaswordText;
        private DateTimePicker datePicker;
        private Button LoginBtn;
        private Button RegBtn;
        private Button SaveBtn;
        public Authorization(string path)
        {

            PathToFile = path;
            _Read();
            Init();

        }
        void Init() {
            Name = "Authorization";
            int width = this.Width / 2-50;
            LoginText = new TextBox {
                Location = new Point(width, 40),
                Size = new Size(width,25)
               };

            PaswordText = new TextBox
            {
                Location = new Point(width, 70),
                Size = new Size(width, 25),
               Text = "",
           
             PasswordChar = '*',
            
             MaxLength = 14

        };
            datePicker = new DateTimePicker
            {
                Location = new Point(width, 100),
                Size = new Size(width, 25)
            };
            datePicker.Hide();
            LoginBtn = new Button
            {
                Location = new Point(width,180),
                Size = new Size(width, 25),
                Text ="Авторизация"
            };
            LoginBtn.Click +=LoginBtn_Click;
            RegBtn = new Button
            {
                Location = new Point(width, 210),
                Size = new Size(width, 25),
                Text = "Регистрация"
            };
            RegBtn.Click +=RegBtn_Click;
            SaveBtn = new Button
            {
                Location = new Point(width, 210),
                Size = new Size(width, 25),
                Text = "Сохранить"
            };
            SaveBtn.Click +=SaveBtn_Click;
            SaveBtn.Hide();
            Controls.AddRange(new Control[] 
            {LoginText, PaswordText,datePicker, LoginBtn, RegBtn,SaveBtn });


        }

        void SaveBtn_Click(object sender, EventArgs e)
        {
            foreach (User u in users)
            {
                if (u.Login == LoginText.Text.Trim())
                {
                    MessageBox.Show("Пользователь с таким именем уже существует");
                    return;
                }

            }
            if (PaswordText.Text.Trim().Length < 5)
            {
                MessageBox.Show("Пароль не меньше 6 символов");
                return;
            }

            User user = new User
            {
                Login = LoginText.Text.Trim(),
                Password = PaswordText.Text.Trim(),
                BDay = datePicker.ToString()
            };
            users.Add(user);
            _Write();         
            if (CheckUser(user.Login, user.Password) != null)
            {
                Quiz ifrm = (SecondForms.Quiz)Application.OpenForms[0];
                ifrm.SetUser(user);
                ifrm.Show();
                this.Close();
            }
        }


        void RegBtn_Click(object sender, EventArgs e)
        {
            LoginBtn.Hide();
            SaveBtn.Show();
            datePicker.Show();
            RegBtn.Hide();
        }


        void LoginBtn_Click(object sender, EventArgs e)
        {
          User user=  CheckUser(LoginText.Text.Trim(), PaswordText.Text.Trim());
            if(user != null) {
                Quiz ifrm = (SecondForms.Quiz)Application.OpenForms[0];
                ifrm.SetUser(user);
                ifrm.Show();
                this.Hide();
            }
            else {
                MessageBox.Show("Неверный логин и пароль");
            }


        }

        public void WriteRezult(User user)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if(users[i].Login == user.Login) {
                    users[i] = user;
                }

            }
            _Write();
        }
        public User CheckUser(string login,string password) {
        
              foreach(User user in users) {
                if (user.Login == login && user.Password == password)
                    return user;
              
              }
            return null;

        }

        void _Write()
        {
            using (FileStream fs = new FileStream(PathToFile, FileMode.OpenOrCreate))
            {
                byte[] jsonUtf8Bytes;
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,

                };
                if (users.Count == 0) {
                    User user = new User
                    {
                        Login = "admin",
                        Password = "admin",
                        IsAdmin = true
                    };
                    users.Add(user);
                }
                jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(users, options);
                fs.Write(jsonUtf8Bytes, 0, jsonUtf8Bytes.Length);

            }
        }

        private void _Read() {


            try
            {   var file= File.OpenWrite(PathToFile);
                file.Close();
                string text2 = File.ReadAllText(PathToFile, Encoding.UTF8);
                if (text2.Length == 0)
                {
                    _Write();
                    text2 = File.ReadAllText(PathToFile, Encoding.UTF8);
                }
               users = JsonSerializer.Deserialize<List<User>>(text2);
            
            }
            catch (System.Text.Json.JsonException e)
            {

                Console.WriteLine("JsonException  internal List<Test> ReadFile()\n " + e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception  internal List<Test> ReadFile()\n " + e.InnerException);

            }       

        }


    }
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string BDay { get; set; } = "";
        public bool IsAdmin { get; set; } = false;
        public Dictionary<string,int> Results { get; set; }
        public User()
        {
            Results = new Dictionary<string, int>();
        }
        public void Add(string thema, int score)
        {
            if (score == 0 || IsAdmin) return;
            if (Results.ContainsKey(thema))
            {
                if (score > Results[thema])
                    Results[thema] = score;
            }
            else
                Results.Add(thema, score);
        }
    }
}
