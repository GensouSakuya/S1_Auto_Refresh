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
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            var userName = textBox1.Text;
            if (_parent.IsUserExists(userName))
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

            _parent.AddUser(userName, password, questionID, answer);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
