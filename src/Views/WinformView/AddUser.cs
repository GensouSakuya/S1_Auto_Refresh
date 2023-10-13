using System;
using System.Data;
using System.Linq;
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

            forumTypeBox.DataSource = _parent._core.LoadedKeepers.Select(p => new
            {
                ID = p.Key,
                Text = p.Name
            }).ToList();
            forumTypeBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var userName = userNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(userName))
            {
                MessageBox.Show("请输入用户名");
                return;
            }
            var forumType = (string)forumTypeBox.SelectedValue;
            if (_parent.IsUserExists(userName, forumType))
            {
                MessageBox.Show("用户已存在");
                return;
            }

            if (!manuallyLoginCheckBox.Checked)
            {
                var password = textBox2.Text;
                int questionID = (int)questionBox.SelectedValue;
                var answer = "";
                if (questionID != 0)
                {
                    answer = textBox4.Text;
                }
                _parent.AddUser(userName, password, questionID, answer, forumType);
            }
            else
            {
                _parent.AddUser(userName, forumType);
            }

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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (manuallyLoginCheckBox.Checked)
            {
                SetLoginInfoVisible(false);
            }
            else
            {
                SetLoginInfoVisible(true);
            }
        }

        private void SetLoginInfoVisible(bool visible)
        {
            label2.Visible = visible;
            label3.Visible = visible;
            label4.Visible = visible;
            textBox2.Visible = visible;
            textBox4.Visible = visible;
            questionBox.Visible = visible;
        }
    }
}
