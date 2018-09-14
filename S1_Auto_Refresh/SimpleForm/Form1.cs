using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using System.Windows.Forms;

namespace SimpleForm
{
    public partial class Form1 : Form
    {
        delegate void RefreshStatus();

        public Form1()
        {
            InitializeComponent();
        }

        private void RefreshStatusLabel()
        {
            label6.Text = refresher.User.Status + DateTime.Now.ToString("HH:mm:ss");
        }

        private Refresher refresher;
        private void button1_Click(object sender, EventArgs e)
        {
            if (refresher == null)
            {
                int questionID = 0;
                int.TryParse(textBox3.Text, out questionID);
                refresher = new Refresher(textBox1.Text, textBox2.Text, questionID, textBox4.Text);
            }

            refresher.Start();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var fun = new RefreshStatus(RefreshStatusLabel);
            label6.Invoke(fun);
        }
    }
}
