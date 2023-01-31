namespace ASPK_Password
{
    partial class PasswordForm
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
            this.cb_user = new System.Windows.Forms.ComboBox();
            this.lb_user = new System.Windows.Forms.Label();
            this.lb_password = new System.Windows.Forms.Label();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cb_user
            // 
            this.cb_user.FormattingEnabled = true;
            this.cb_user.Items.AddRange(new object[] {
            "BORDERBEL",
            "SYSTEM"});
            this.cb_user.Location = new System.Drawing.Point(102, 10);
            this.cb_user.Name = "cb_user";
            this.cb_user.Size = new System.Drawing.Size(121, 21);
            this.cb_user.TabIndex = 0;
            // 
            // lb_user
            // 
            this.lb_user.AutoSize = true;
            this.lb_user.Location = new System.Drawing.Point(13, 13);
            this.lb_user.Name = "lb_user";
            this.lb_user.Size = new System.Drawing.Size(83, 13);
            this.lb_user.TabIndex = 1;
            this.lb_user.Text = "Пользователь:";
            // 
            // lb_password
            // 
            this.lb_password.AutoSize = true;
            this.lb_password.Location = new System.Drawing.Point(13, 44);
            this.lb_password.Name = "lb_password";
            this.lb_password.Size = new System.Drawing.Size(48, 13);
            this.lb_password.TabIndex = 2;
            this.lb_password.Text = "Пароль:";
            // 
            // tb_password
            // 
            this.tb_password.Location = new System.Drawing.Point(102, 41);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(121, 20);
            this.tb_password.TabIndex = 3;
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(16, 77);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(207, 23);
            this.btn_ok.TabIndex = 4;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // PasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 108);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.tb_password);
            this.Controls.Add(this.lb_password);
            this.Controls.Add(this.lb_user);
            this.Controls.Add(this.cb_user);
            this.Name = "PasswordForm";
            this.Text = "Введите пароль";
            this.Load += new System.EventHandler(this.PasswordForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox cb_user;
        private System.Windows.Forms.Label lb_user;
        private System.Windows.Forms.Label lb_password;
        public System.Windows.Forms.TextBox tb_password;
        private System.Windows.Forms.Button btn_ok;
    }
}