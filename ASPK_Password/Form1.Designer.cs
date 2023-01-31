namespace ASPK_Password
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tb_server = new System.Windows.Forms.TextBox();
            this.tb_subnet = new System.Windows.Forms.TextBox();
            this.lbl_old_password = new System.Windows.Forms.Label();
            this.lbl_new_password = new System.Windows.Forms.Label();
            this.lbl_confirm_password = new System.Windows.Forms.Label();
            this.tb_confirm_password = new System.Windows.Forms.TextBox();
            this.tb_old_password = new System.Windows.Forms.TextBox();
            this.tb_new_password = new System.Windows.Forms.TextBox();
            this.btp_ping = new System.Windows.Forms.Button();
            this.btn_change_auto = new System.Windows.Forms.Button();
            this.lbl_service_name = new System.Windows.Forms.Label();
            this.lbl_port = new System.Windows.Forms.Label();
            this.tb_service_name = new System.Windows.Forms.TextBox();
            this.tb_port = new System.Windows.Forms.TextBox();
            this.lbl_user = new System.Windows.Forms.Label();
            this.cb_user = new System.Windows.Forms.ComboBox();
            this.rb_ip = new System.Windows.Forms.RadioButton();
            this.rb_subnet = new System.Windows.Forms.RadioButton();
            this.rb_choose = new System.Windows.Forms.RadioButton();
            this.lbl_protocol = new System.Windows.Forms.Label();
            this.cb_protocol = new System.Windows.Forms.ComboBox();
            this.btn_open_old_password = new System.Windows.Forms.Button();
            this.btn_open_new_password = new System.Windows.Forms.Button();
            this.btn_open_confirm_password = new System.Windows.Forms.Button();
            this.btn_create_password = new System.Windows.Forms.Button();
            this.lbl_generate_password = new System.Windows.Forms.Label();
            this.btn_change_server = new System.Windows.Forms.Button();
            this.btn_change_local = new System.Windows.Forms.Button();
            this.btn_custom_script = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_server
            // 
            this.tb_server.Enabled = false;
            this.tb_server.Location = new System.Drawing.Point(170, 37);
            this.tb_server.Name = "tb_server";
            this.tb_server.Size = new System.Drawing.Size(101, 20);
            this.tb_server.TabIndex = 2;
            this.tb_server.Text = "localhost";
            // 
            // tb_subnet
            // 
            this.tb_subnet.Enabled = false;
            this.tb_subnet.Location = new System.Drawing.Point(170, 59);
            this.tb_subnet.Name = "tb_subnet";
            this.tb_subnet.Size = new System.Drawing.Size(101, 20);
            this.tb_subnet.TabIndex = 5;
            this.tb_subnet.Text = "localhost";
            // 
            // lbl_old_password
            // 
            this.lbl_old_password.AutoSize = true;
            this.lbl_old_password.Location = new System.Drawing.Point(31, 245);
            this.lbl_old_password.Name = "lbl_old_password";
            this.lbl_old_password.Size = new System.Drawing.Size(87, 13);
            this.lbl_old_password.TabIndex = 6;
            this.lbl_old_password.Text = "Старый пароль:";
            // 
            // lbl_new_password
            // 
            this.lbl_new_password.AutoSize = true;
            this.lbl_new_password.Location = new System.Drawing.Point(31, 269);
            this.lbl_new_password.Name = "lbl_new_password";
            this.lbl_new_password.Size = new System.Drawing.Size(83, 13);
            this.lbl_new_password.TabIndex = 7;
            this.lbl_new_password.Text = "Новый пароль:";
            // 
            // lbl_confirm_password
            // 
            this.lbl_confirm_password.AutoSize = true;
            this.lbl_confirm_password.Location = new System.Drawing.Point(31, 294);
            this.lbl_confirm_password.Name = "lbl_confirm_password";
            this.lbl_confirm_password.Size = new System.Drawing.Size(115, 13);
            this.lbl_confirm_password.TabIndex = 8;
            this.lbl_confirm_password.Text = "Подтвердите пароль:";
            // 
            // tb_confirm_password
            // 
            this.tb_confirm_password.Location = new System.Drawing.Point(171, 291);
            this.tb_confirm_password.Name = "tb_confirm_password";
            this.tb_confirm_password.Size = new System.Drawing.Size(100, 20);
            this.tb_confirm_password.TabIndex = 12;
            // 
            // tb_old_password
            // 
            this.tb_old_password.Location = new System.Drawing.Point(171, 242);
            this.tb_old_password.Name = "tb_old_password";
            this.tb_old_password.Size = new System.Drawing.Size(100, 20);
            this.tb_old_password.TabIndex = 10;
            // 
            // tb_new_password
            // 
            this.tb_new_password.Location = new System.Drawing.Point(171, 266);
            this.tb_new_password.Name = "tb_new_password";
            this.tb_new_password.Size = new System.Drawing.Size(100, 20);
            this.tb_new_password.TabIndex = 11;
            // 
            // btp_ping
            // 
            this.btp_ping.Location = new System.Drawing.Point(310, 84);
            this.btp_ping.Name = "btp_ping";
            this.btp_ping.Size = new System.Drawing.Size(120, 37);
            this.btp_ping.TabIndex = 9;
            this.btp_ping.Text = "Ping";
            this.btp_ping.UseVisualStyleBackColor = true;
            this.btp_ping.Click += new System.EventHandler(this.btp_ping_Click);
            // 
            // btn_change_auto
            // 
            this.btn_change_auto.AutoEllipsis = true;
            this.btn_change_auto.Location = new System.Drawing.Point(310, 127);
            this.btn_change_auto.Name = "btn_change_auto";
            this.btn_change_auto.Size = new System.Drawing.Size(120, 37);
            this.btn_change_auto.TabIndex = 13;
            this.btn_change_auto.Text = "Сменить пароль (автоматически)";
            this.btn_change_auto.UseVisualStyleBackColor = true;
            this.btn_change_auto.Click += new System.EventHandler(this.btn_change_Click);
            // 
            // lbl_service_name
            // 
            this.lbl_service_name.AutoSize = true;
            this.lbl_service_name.Location = new System.Drawing.Point(31, 124);
            this.lbl_service_name.Name = "lbl_service_name";
            this.lbl_service_name.Size = new System.Drawing.Size(61, 13);
            this.lbl_service_name.TabIndex = 14;
            this.lbl_service_name.Text = "Имя базы:";
            // 
            // lbl_port
            // 
            this.lbl_port.AutoSize = true;
            this.lbl_port.Location = new System.Drawing.Point(31, 147);
            this.lbl_port.Name = "lbl_port";
            this.lbl_port.Size = new System.Drawing.Size(35, 13);
            this.lbl_port.TabIndex = 15;
            this.lbl_port.Text = "Порт:";
            // 
            // tb_service_name
            // 
            this.tb_service_name.Enabled = false;
            this.tb_service_name.Location = new System.Drawing.Point(171, 121);
            this.tb_service_name.Name = "tb_service_name";
            this.tb_service_name.Size = new System.Drawing.Size(100, 20);
            this.tb_service_name.TabIndex = 16;
            this.tb_service_name.Text = "ORCL";
            // 
            // tb_port
            // 
            this.tb_port.Enabled = false;
            this.tb_port.Location = new System.Drawing.Point(171, 144);
            this.tb_port.Name = "tb_port";
            this.tb_port.Size = new System.Drawing.Size(100, 20);
            this.tb_port.TabIndex = 17;
            this.tb_port.Text = "8888";
            // 
            // lbl_user
            // 
            this.lbl_user.AutoSize = true;
            this.lbl_user.Location = new System.Drawing.Point(31, 193);
            this.lbl_user.Name = "lbl_user";
            this.lbl_user.Size = new System.Drawing.Size(83, 13);
            this.lbl_user.TabIndex = 18;
            this.lbl_user.Text = "Пользователь:";
            // 
            // cb_user
            // 
            this.cb_user.FormattingEnabled = true;
            this.cb_user.Items.AddRange(new object[] {
            "CUSTOM_USER",
            "SYS",
            "SYSTEM"});
            this.cb_user.Location = new System.Drawing.Point(171, 190);
            this.cb_user.Name = "cb_user";
            this.cb_user.Size = new System.Drawing.Size(100, 21);
            this.cb_user.TabIndex = 19;
            // 
            // rb_ip
            // 
            this.rb_ip.Location = new System.Drawing.Point(34, 38);
            this.rb_ip.Name = "rb_ip";
            this.rb_ip.Size = new System.Drawing.Size(97, 17);
            this.rb_ip.TabIndex = 0;
            this.rb_ip.TabStop = true;
            this.rb_ip.Text = "Ip-адрес:";
            this.rb_ip.UseVisualStyleBackColor = true;
            this.rb_ip.CheckedChanged += new System.EventHandler(this.rb_ip_CheckedChanged);
            // 
            // rb_subnet
            // 
            this.rb_subnet.AutoSize = true;
            this.rb_subnet.Location = new System.Drawing.Point(34, 61);
            this.rb_subnet.Name = "rb_subnet";
            this.rb_subnet.Size = new System.Drawing.Size(71, 17);
            this.rb_subnet.TabIndex = 21;
            this.rb_subnet.TabStop = true;
            this.rb_subnet.Text = "Подсеть:";
            this.rb_subnet.UseVisualStyleBackColor = true;
            this.rb_subnet.CheckedChanged += new System.EventHandler(this.rb_subnet_CheckedChanged);
            // 
            // rb_choose
            // 
            this.rb_choose.AutoSize = true;
            this.rb_choose.Location = new System.Drawing.Point(34, 85);
            this.rb_choose.Name = "rb_choose";
            this.rb_choose.Size = new System.Drawing.Size(188, 17);
            this.rb_choose.TabIndex = 22;
            this.rb_choose.TabStop = true;
            this.rb_choose.Text = "Выбрать из списка (выбрано: 0)";
            this.rb_choose.UseVisualStyleBackColor = true;
            this.rb_choose.CheckedChanged += new System.EventHandler(this.rb_choose_CheckedChanged);
            this.rb_choose.Click += new System.EventHandler(this.rb_choose_Click);
            // 
            // lbl_protocol
            // 
            this.lbl_protocol.AutoSize = true;
            this.lbl_protocol.Location = new System.Drawing.Point(31, 170);
            this.lbl_protocol.Name = "lbl_protocol";
            this.lbl_protocol.Size = new System.Drawing.Size(59, 13);
            this.lbl_protocol.TabIndex = 25;
            this.lbl_protocol.Text = "Протокол:";
            // 
            // cb_protocol
            // 
            this.cb_protocol.Enabled = false;
            this.cb_protocol.FormattingEnabled = true;
            this.cb_protocol.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cb_protocol.Items.AddRange(new object[] {
            "TCP",
            "UDP"});
            this.cb_protocol.Location = new System.Drawing.Point(171, 167);
            this.cb_protocol.Name = "cb_protocol";
            this.cb_protocol.Size = new System.Drawing.Size(100, 21);
            this.cb_protocol.TabIndex = 27;
            // 
            // btn_open_old_password
            // 
            this.btn_open_old_password.BackgroundImage = global::ASPK_Password.Properties.Resources.eye;
            this.btn_open_old_password.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_open_old_password.Location = new System.Drawing.Point(278, 242);
            this.btn_open_old_password.Name = "btn_open_old_password";
            this.btn_open_old_password.Size = new System.Drawing.Size(20, 20);
            this.btn_open_old_password.TabIndex = 28;
            this.btn_open_old_password.UseVisualStyleBackColor = true;
            // 
            // btn_open_new_password
            // 
            this.btn_open_new_password.BackgroundImage = global::ASPK_Password.Properties.Resources.eye;
            this.btn_open_new_password.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_open_new_password.Location = new System.Drawing.Point(278, 266);
            this.btn_open_new_password.Name = "btn_open_new_password";
            this.btn_open_new_password.Size = new System.Drawing.Size(20, 20);
            this.btn_open_new_password.TabIndex = 29;
            this.btn_open_new_password.UseVisualStyleBackColor = true;
            // 
            // btn_open_confirm_password
            // 
            this.btn_open_confirm_password.BackgroundImage = global::ASPK_Password.Properties.Resources.eye;
            this.btn_open_confirm_password.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_open_confirm_password.Location = new System.Drawing.Point(278, 291);
            this.btn_open_confirm_password.Name = "btn_open_confirm_password";
            this.btn_open_confirm_password.Size = new System.Drawing.Size(20, 20);
            this.btn_open_confirm_password.TabIndex = 30;
            this.btn_open_confirm_password.UseVisualStyleBackColor = true;
            // 
            // btn_create_password
            // 
            this.btn_create_password.AutoEllipsis = true;
            this.btn_create_password.Location = new System.Drawing.Point(310, 256);
            this.btn_create_password.Name = "btn_create_password";
            this.btn_create_password.Size = new System.Drawing.Size(120, 37);
            this.btn_create_password.TabIndex = 31;
            this.btn_create_password.Text = "Сгенерировать пароль";
            this.btn_create_password.UseVisualStyleBackColor = true;
            this.btn_create_password.Click += new System.EventHandler(this.btn_create_password_Click);
            // 
            // lbl_generate_password
            // 
            this.lbl_generate_password.AutoSize = true;
            this.lbl_generate_password.Location = new System.Drawing.Point(350, 308);
            this.lbl_generate_password.Name = "lbl_generate_password";
            this.lbl_generate_password.Size = new System.Drawing.Size(0, 13);
            this.lbl_generate_password.TabIndex = 32;
            this.lbl_generate_password.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_change_server
            // 
            this.btn_change_server.AutoEllipsis = true;
            this.btn_change_server.Location = new System.Drawing.Point(310, 170);
            this.btn_change_server.Name = "btn_change_server";
            this.btn_change_server.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_change_server.Size = new System.Drawing.Size(120, 37);
            this.btn_change_server.TabIndex = 33;
            this.btn_change_server.Text = "Сменить пароль на сервере";
            this.btn_change_server.UseVisualStyleBackColor = true;
            this.btn_change_server.Click += new System.EventHandler(this.btn_change_server_Click);
            // 
            // btn_change_local
            // 
            this.btn_change_local.AutoEllipsis = true;
            this.btn_change_local.Location = new System.Drawing.Point(310, 213);
            this.btn_change_local.Name = "btn_change_local";
            this.btn_change_local.Size = new System.Drawing.Size(120, 37);
            this.btn_change_local.TabIndex = 34;
            this.btn_change_local.Text = "Сменить пароль на локале";
            this.btn_change_local.UseVisualStyleBackColor = true;
            this.btn_change_local.Click += new System.EventHandler(this.btn_change_local_Click);
            // 
            // btn_custom_script
            // 
            this.btn_custom_script.Location = new System.Drawing.Point(310, 37);
            this.btn_custom_script.Name = "btn_custom_script";
            this.btn_custom_script.Size = new System.Drawing.Size(120, 41);
            this.btn_custom_script.TabIndex = 35;
            this.btn_custom_script.Text = "Выполнить скрипт из файла";
            this.btn_custom_script.UseVisualStyleBackColor = true;
            this.btn_custom_script.Click += new System.EventHandler(this.btn_custom_script_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 416);
            this.Controls.Add(this.btn_custom_script);
            this.Controls.Add(this.btn_change_local);
            this.Controls.Add(this.btn_change_server);
            this.Controls.Add(this.tb_subnet);
            this.Controls.Add(this.tb_server);
            this.Controls.Add(this.rb_subnet);
            this.Controls.Add(this.rb_choose);
            this.Controls.Add(this.lbl_generate_password);
            this.Controls.Add(this.rb_ip);
            this.Controls.Add(this.btn_create_password);
            this.Controls.Add(this.btn_open_confirm_password);
            this.Controls.Add(this.btn_open_new_password);
            this.Controls.Add(this.btn_open_old_password);
            this.Controls.Add(this.cb_protocol);
            this.Controls.Add(this.lbl_protocol);
            this.Controls.Add(this.cb_user);
            this.Controls.Add(this.lbl_user);
            this.Controls.Add(this.tb_port);
            this.Controls.Add(this.tb_service_name);
            this.Controls.Add(this.lbl_port);
            this.Controls.Add(this.lbl_service_name);
            this.Controls.Add(this.btn_change_auto);
            this.Controls.Add(this.btp_ping);
            this.Controls.Add(this.tb_new_password);
            this.Controls.Add(this.tb_old_password);
            this.Controls.Add(this.tb_confirm_password);
            this.Controls.Add(this.lbl_confirm_password);
            this.Controls.Add(this.lbl_new_password);
            this.Controls.Add(this.lbl_old_password);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_server;
        private System.Windows.Forms.TextBox tb_subnet;
        private System.Windows.Forms.Label lbl_old_password;
        private System.Windows.Forms.Label lbl_new_password;
        private System.Windows.Forms.Label lbl_confirm_password;
        private System.Windows.Forms.TextBox tb_confirm_password;
        private System.Windows.Forms.TextBox tb_old_password;
        private System.Windows.Forms.TextBox tb_new_password;
        private System.Windows.Forms.Button btp_ping;
        private System.Windows.Forms.Button btn_change_auto;
        private System.Windows.Forms.Label lbl_service_name;
        private System.Windows.Forms.Label lbl_port;
        private System.Windows.Forms.TextBox tb_service_name;
        private System.Windows.Forms.TextBox tb_port;
        private System.Windows.Forms.Label lbl_user;
        private System.Windows.Forms.ComboBox cb_user;
        private System.Windows.Forms.RadioButton rb_ip;
        private System.Windows.Forms.RadioButton rb_subnet;
        private System.Windows.Forms.RadioButton rb_choose;
        private System.Windows.Forms.Label lbl_protocol;
        private System.Windows.Forms.ComboBox cb_protocol;
        private System.Windows.Forms.Button btn_open_old_password;
        private System.Windows.Forms.Button btn_open_new_password;
        private System.Windows.Forms.Button btn_open_confirm_password;
        private System.Windows.Forms.Button btn_create_password;
        private System.Windows.Forms.Label lbl_generate_password;
        private System.Windows.Forms.Button btn_change_server;
        private System.Windows.Forms.Button btn_change_local;
        private System.Windows.Forms.Button btn_custom_script;
    }
}

