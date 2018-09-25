﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core;
using System.Windows.Forms;

namespace SimpleForm
{
    public partial class Form1 : Form
    {
        private List<Refresher> refreshers = new List<Refresher>();
        private DataTable dataTable;

        private bool IsRefreshing = false;
        
        delegate void RefreshDataGrid();

        public Form1()
        {
            InitializeComponent();
            dataTable = new DataTable();
            dataTable.Columns.Add("用户名");
            dataTable.Columns.Add("当前状态");
            dataTable.Columns.Add("上次连接时间");
            userDataGridView.DataSource = dataTable;
            userDataGridView.AllowUserToAddRows = false;
            userDataGridView.AllowUserToDeleteRows = false;
            userDataGridView.AllowUserToOrderColumns = true;
            userDataGridView.ReadOnly = true;
            userDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            userDataGridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            userDataGridView.RowHeadersVisible = false;
            userDataGridView.MultiSelect = false;
            userDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            notifyIcon1.Visible = false;
            stopButton.Enabled = false;

            refreshers = S1Manager.GetUsersFromDB().Select(p => new Refresher(p.UserName, p.Password, p.QuestionID, p.Answer)).ToList();
            RefreshUserDataGridView();
        }
        
        private void RefreshUserDataGridView()
        {
            try
            {
                dataTable.Clear();
                refreshers.ForEach(p =>
                {
                    var user = p.User;
                    var row = dataTable.Rows.Add();
                    row[0] = user.UserName;
                    row[1] = user.Status;
                    row[2] = user.LastRefreshTime == DateTime.MinValue ? "尚未开始" : user.LastRefreshTime.ToString("MM/dd HH:mm:ss");
                });
            }
            catch (Exception e)
            {
                FileLogHelper.WriteLog(e);
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (IsRefreshing)
            {
                return;
            }
            if (refreshers.Any())
            {
                IsRefreshing = true;
                refreshers.ForEach(p => p.Start());
            }

            button1.Enabled = false;
            stopButton.Enabled = true;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        public void AddUser(string userName, string password, int questionID, string answer)
        {
            var refe = new Refresher(userName, password, questionID, answer);
            refreshers.Add(refe);
            S1Manager.AddUserToDB(refe.User);

            RefreshDataGridView();
        }

        public bool IsUserExists(string userName)
        {
            return refreshers.Any(p => p.User.UserName == userName);
        }

        private void addUserButton_Click(object sender, EventArgs e)
        {
            if (IsRefreshing)
            {
                MessageBox.Show("添加用户请先停止刷新");
                return;
            }
            var form = new AddUser(this);
            if (form.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!IsRefreshing)
            {
                return;
            }
            if (refreshers.Any())
            {
                IsRefreshing = false;
                refreshers.ForEach(p => p.Stop());
            }
            timer1.Stop();
            button1.Enabled = true;
            stopButton.Enabled = false;
        }

        private void delButton_Click(object sender, EventArgs e)
        {
            if (IsRefreshing)
            {
                MessageBox.Show("删除用户请先停止刷新");
                return;
            }

            if (!refreshers.Any())
            {
                return;
            }

            var selectedName = userDataGridView.SelectedRows[0].Cells[0].Value as string;
            if (!string.IsNullOrWhiteSpace(selectedName))
            {
                refreshers.RemoveAll(p => p.User.UserName == selectedName);
                S1Manager.DelUserFromDB(selectedName);

                RefreshDataGridView();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                this.Activate();
                this.ShowInTaskbar = true;
                this.Visible = true;
                notifyIcon1.Visible = false;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.Visible = false;
                notifyIcon1.Visible = true;
            }
        }

        private void RefreshDataGridView()
        {
            var fun = new RefreshDataGrid(RefreshUserDataGridView);
            userDataGridView.Invoke(fun);
        }
    }
}
