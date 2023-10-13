namespace SimpleForm
{
    partial class AddUser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            label4 = new System.Windows.Forms.Label();
            textBox4 = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            textBox2 = new System.Windows.Forms.TextBox();
            userNameLabel = new System.Windows.Forms.Label();
            userNameTextBox = new System.Windows.Forms.TextBox();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            questionModelBindingSource = new System.Windows.Forms.BindingSource(components);
            questionBox = new System.Windows.Forms.ComboBox();
            forumTypeBox = new System.Windows.Forms.ComboBox();
            forumTypeLabel = new System.Windows.Forms.Label();
            manuallyLoginCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)questionModelBindingSource).BeginInit();
            SuspendLayout();
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(22, 224);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(69, 20);
            label4.TabIndex = 16;
            label4.Text = "问题答案";
            // 
            // textBox4
            // 
            textBox4.Location = new System.Drawing.Point(103, 220);
            textBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            textBox4.Name = "textBox4";
            textBox4.PasswordChar = '*';
            textBox4.Size = new System.Drawing.Size(182, 27);
            textBox4.TabIndex = 15;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(22, 178);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(69, 20);
            label3.TabIndex = 14;
            label3.Text = "安全问题";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(22, 132);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(39, 20);
            label2.TabIndex = 12;
            label2.Text = "密码";
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(103, 128);
            textBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            textBox2.Name = "textBox2";
            textBox2.PasswordChar = '*';
            textBox2.Size = new System.Drawing.Size(182, 27);
            textBox2.TabIndex = 11;
            // 
            // userNameLabel
            // 
            userNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            userNameLabel.AutoSize = true;
            userNameLabel.Location = new System.Drawing.Point(22, 56);
            userNameLabel.Name = "userNameLabel";
            userNameLabel.Size = new System.Drawing.Size(54, 20);
            userNameLabel.TabIndex = 10;
            userNameLabel.Text = "用户名";
            // 
            // userNameTextBox
            // 
            userNameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            userNameTextBox.Location = new System.Drawing.Point(103, 53);
            userNameTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            userNameTextBox.Name = "userNameTextBox";
            userNameTextBox.Size = new System.Drawing.Size(182, 27);
            userNameTextBox.TabIndex = 9;
            // 
            // button1
            // 
            button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button1.Location = new System.Drawing.Point(169, 293);
            button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(116, 44);
            button1.TabIndex = 17;
            button1.Text = "确定";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            button2.Location = new System.Drawing.Point(12, 293);
            button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(116, 44);
            button2.TabIndex = 18;
            button2.Text = "取消";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // questionBox
            // 
            //questionBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", questionModelBindingSource, "ID", true));
            questionBox.DisplayMember = "Text";
            questionBox.FormattingEnabled = true;
            questionBox.Location = new System.Drawing.Point(103, 174);
            questionBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            questionBox.Name = "questionBox";
            questionBox.Size = new System.Drawing.Size(182, 28);
            questionBox.TabIndex = 20;
            questionBox.ValueMember = "ID";
            questionBox.SelectedIndexChanged += QuestionBox_SelectedIndexChanged;
            // 
            // forumTypeBox
            // 
            forumTypeBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            //forumTypeBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", questionModelBindingSource, "ID", true));
            forumTypeBox.DisplayMember = "Text";
            forumTypeBox.FormattingEnabled = true;
            forumTypeBox.Location = new System.Drawing.Point(103, 14);
            forumTypeBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            forumTypeBox.Name = "forumTypeBox";
            forumTypeBox.Size = new System.Drawing.Size(182, 28);
            forumTypeBox.TabIndex = 22;
            forumTypeBox.ValueMember = "ID";
            forumTypeBox.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            // 
            // forumTypeLabel
            // 
            forumTypeLabel.AutoSize = true;
            forumTypeLabel.Location = new System.Drawing.Point(22, 17);
            forumTypeLabel.Name = "forumTypeLabel";
            forumTypeLabel.Size = new System.Drawing.Size(39, 20);
            forumTypeLabel.TabIndex = 21;
            forumTypeLabel.Text = "论坛";
            forumTypeLabel.Click += Label5_Click;
            // 
            // manuallyLoginCheckBox
            // 
            manuallyLoginCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            manuallyLoginCheckBox.AutoSize = true;
            manuallyLoginCheckBox.Location = new System.Drawing.Point(22, 90);
            manuallyLoginCheckBox.Name = "manuallyLoginCheckBox";
            manuallyLoginCheckBox.Size = new System.Drawing.Size(121, 24);
            manuallyLoginCheckBox.TabIndex = 23;
            manuallyLoginCheckBox.Text = "是否手动登录";
            manuallyLoginCheckBox.UseVisualStyleBackColor = true;
            manuallyLoginCheckBox.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // AddUser
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(297, 350);
            ControlBox = false;
            Controls.Add(manuallyLoginCheckBox);
            Controls.Add(forumTypeBox);
            Controls.Add(forumTypeLabel);
            Controls.Add(questionBox);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(textBox4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(userNameLabel);
            Controls.Add(userNameTextBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddUser";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "AddUser";
            ((System.ComponentModel.ISupportInitialize)questionModelBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.BindingSource questionModelBindingSource;
        private System.Windows.Forms.ComboBox questionBox;
        private System.Windows.Forms.ComboBox forumTypeBox;
        private System.Windows.Forms.Label forumTypeLabel;
        private System.Windows.Forms.CheckBox manuallyLoginCheckBox;
    }
}
