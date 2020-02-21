using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleForm
{
    public partial class AddUser : Form
    {
        private Form1 _parent { get; set; }

        public AddUser(Form1 parent)
        {
            InitializeComponent();
            _parent = parent;
            questionBox.DataSource = QuestionModel.AllQuestions();
            questionBox.DropDownStyle = ComboBoxStyle.DropDownList;
            forumTypeBox.DataSource = new List<object>
            {
                new{
                    ID=(int) ForumType.S1,Text= ForumType.S1.ToString()
                },
                new{
                    ID =(int) ForumType.漫画补档, Text = ForumType.漫画补档.ToString()
                }
            };
            forumTypeBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            var userName = textBox1.Text;
            if (string.IsNullOrWhiteSpace(userName))
            {
                MessageBox.Show("请输入用户名");
            }
            var forumType = (ForumType)forumTypeBox.SelectedValue;
            if (_parent.IsUserExists(userName, forumType))
            {
                MessageBox.Show("用户已存在");
            }

            var password = textBox2.Text;
            int questionID = (int)questionBox.SelectedValue;
            var answer = "";
            if (questionID != 0)
            {
                answer = textBox4.Text;
            }

            _parent.AddUser(userName, password, questionID, answer, forumType);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void QuestionBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
