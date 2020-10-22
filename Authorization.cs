using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Quize
{
    public class Authorization:Form
    {
        private List<User> users = new List<User>();
        private string PathToFile { get; set; }
        private Label LoginLabel;
        private TextBox LoginText;
        private TextBox PaswordText;
        private Label PaswordLabel;
        private DateTimePicker datePicker;
        private Label DateLabel;

        private Button LoginBtn;
        private Button RegBtn;
        private Button SaveBtn;
        private User correctUser;
        public Authorization(string path,User user=null)
        {
            correctUser = user;
            PathToFile = path;
            _Read();
            Init();

        }
        void Init() {
            BackColor = Color.BlanchedAlmond;
            StartPosition = FormStartPosition.CenterScreen;
            Name = "Authorization";
            int width = this.Width / 2-50;
            LoginLabel = new Label
            {
                Location = new Point(width, 40),
                Size = new Size(width, 25),
                Text = "Логин"
            };
            LoginText = new TextBox {
                Location = new Point(width, 70),
                Size = new Size(width,25)
               };
          PaswordLabel = new Label
            {
                Location = new Point(width, 100),
                Size = new Size(width, 25),
                Text = "Пароль"
            };
            PaswordText = new TextBox
            {
                Location = new Point(width, 130),
                Size = new Size(width, 25),
                 Text = "",           
                  PasswordChar = '*',            
                   MaxLength = 14

        };
            DateLabel = new Label
            {
                Location = new Point(width, 160),
                Size = new Size(width, 25),
                Text = "Дата рождения"
            };
            DateLabel.Hide();


            datePicker = new DateTimePicker
            {
                Location = new Point(width, 190),
                Size = new Size(width, 25)
            };
            datePicker.Hide();

           

            LoginBtn = new Button
            {
                Location = new Point(width,210),
                Size = new Size(width, 25),
                Text ="Авторизация"
            };
            LoginBtn.Click +=LoginBtn_Click;
            RegBtn = new Button
            {
                Location = new Point(width, 240),
                Size = new Size(width, 25),
                Text = "Регистрация"
            };
            RegBtn.Click +=RegBtn_Click;
            SaveBtn = new Button
            {
                Location = new Point(width, 240),
                Size = new Size(width, 25),
                Text = "Сохранить"
            };
            SaveBtn.Click +=SaveBtn_Click;
            SaveBtn.Hide();
            Controls.AddRange(new Control[] 
            {LoginText, PaswordText,datePicker, LoginBtn, 
            RegBtn,SaveBtn, LoginLabel,PaswordLabel,DateLabel });
            if (correctUser != null)
            {
               
                try
                {
                    LoginText.Text = correctUser.Login;
                    PaswordText.Text = correctUser.Password;
                  
                 var v = DateTime.Parse(correctUser.BDay);
                    Console.WriteLine(v.Date);
                    datePicker.Value = v.Date;
                    RegBtn_Click(null, null);
                }
                catch
                {
                    Console.WriteLine("Error Date");
                }
            }


        }

        void SaveBtn_Click(object sender, EventArgs e)
        {
         if (correctUser == null)
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
                correctUser = user;
            }
            else
            {
                if (PaswordText.Text.Trim().Length < 5)
                {
                    MessageBox.Show("Пароль не меньше 6 символов");
                    return;
                }
                for (int i = 0; i <users.Count; i++)
                {
                    if(users[i].Login == correctUser.Login)
                    {
                        correctUser.Login= users[i].Login = LoginText.Text.Trim();
                        correctUser.Password=  users[i].Password = PaswordText.Text.Trim();
                        correctUser.BDay=  users[i].BDay = datePicker.ToString();
                        break;
                    }
                }
            }

            _Write();
            if (CheckUser(correctUser.Login, correctUser.Password) != null)
            {
                Quiz ifrm = (Quize.Quiz)Application.OpenForms[0];
                ifrm.SetUser(correctUser);
                ifrm.Show();
                this.Close();
            }
            else
                Console.WriteLine("Ошибка при редактировании");
        }


        void RegBtn_Click(object sender, EventArgs e)
        {
            LoginBtn.Hide();
            SaveBtn.Show();
            datePicker.Show();
            DateLabel.Show();
            RegBtn.Hide();
        }


        void LoginBtn_Click(object sender, EventArgs e)
        {
          User user=  CheckUser(LoginText.Text.Trim(), PaswordText.Text.Trim());
            if(user != null) {
                Quiz ifrm = (Quize.Quiz)Application.OpenForms[0];
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
            using (FileStream fs = new FileStream(PathToFile, FileMode.Create))
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
                        IsAdmin = true,
                        BDay = "11 \u043E\u043A\u0442\u044F\u0431\u0440\u044F 2020 \u0433."
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
        public List<User> GetTop20(string thema)
        {
            List<User> usersTop = new List<User>();
            foreach (var user in users)
            {
                if (user.Results.ContainsKey(thema)) {
                    User u = new User
                    {
                        Login = user.Login,
                    };
                    u.Add(thema, user.Results[thema]);
                    Console.WriteLine($"{thema} {user.Results[thema]}");
                    usersTop.Add(u);
                }
            }
            usersTop.Sort();
            if(usersTop.Count<20)
                return usersTop.GetRange(0, usersTop.Count);
            return usersTop.GetRange(0, 20);
        }

    }
    public class User:IComparable<User>
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


        public int CompareTo(User u)
        {
            return -Results.Values.First().CompareTo(u.Results.Values.First());
        }
    }
}
