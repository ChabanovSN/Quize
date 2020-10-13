﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SecondForms
{
    public class ShowRezults:Form
    {
        public ShowRezults(User user)
        {  
            this.Text = $"Результаты викторины для {user.Login}";
            ListBox listBox1 = new ListBox
            {
                Size = new System.Drawing.Size(395, 255),
                Location = new System.Drawing.Point(10, 10)
            };
            listBox1.ScrollAlwaysVisible = true;
            this.Controls.Add(listBox1); 
            listBox1.BeginUpdate();
            int x = 1;
            foreach (var item in user.Results)
            {
                listBox1.Items.Add($"{x++}: Тема {item.Key} максимальный балл {item.Value}");
            }       
            listBox1.EndUpdate();
            this.ClientSize = new System.Drawing.Size(404, 264);
        }
        public ShowRezults(List<User> users,string thema)
        {
            this.Text = $"Тема {thema}. Топ 20";
            ListBox listBox1 = new ListBox
            {
                Size = new System.Drawing.Size(395, 255),
                Location = new System.Drawing.Point(10, 10)
            };
            listBox1.ScrollAlwaysVisible = true;
            this.Controls.Add(listBox1);
            listBox1.BeginUpdate();
            int x = 1;
            foreach (var user in users)
            {
                listBox1.Items.Add($"{x++}:  {user.Login} =  {user.Results[thema]}");
            }
            listBox1.EndUpdate();
            this.ClientSize = new System.Drawing.Size(404, 264);
        }
    }
}
