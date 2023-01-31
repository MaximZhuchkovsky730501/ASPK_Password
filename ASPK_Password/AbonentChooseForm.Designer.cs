namespace ASPK_Password
{
    partial class AbonentChooseForm
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
            this.dgv_tns = new System.Windows.Forms.DataGridView();
            this.btn_choose = new System.Windows.Forms.Button();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.protocol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.host = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.service_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_tns)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_tns
            // 
            this.dgv_tns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_tns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.protocol,
            this.host,
            this.port,
            this.service_name});
            this.dgv_tns.Location = new System.Drawing.Point(12, 12);
            this.dgv_tns.Name = "dgv_tns";
            this.dgv_tns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_tns.Size = new System.Drawing.Size(593, 426);
            this.dgv_tns.TabIndex = 0;
            // 
            // btn_choose
            // 
            this.btn_choose.AutoEllipsis = true;
            this.btn_choose.Location = new System.Drawing.Point(12, 444);
            this.btn_choose.Name = "btn_choose";
            this.btn_choose.Size = new System.Drawing.Size(593, 44);
            this.btn_choose.TabIndex = 1;
            this.btn_choose.Text = "OK";
            this.btn_choose.UseVisualStyleBackColor = true;
            this.btn_choose.Click += new System.EventHandler(this.btn_choose_Click);
            // 
            // name
            // 
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 150;
            // 
            // protocol
            // 
            this.protocol.HeaderText = "Protocol";
            this.protocol.Name = "protocol";
            this.protocol.ReadOnly = true;
            this.protocol.Width = 50;
            // 
            // host
            // 
            this.host.HeaderText = "Host";
            this.host.Name = "host";
            this.host.ReadOnly = true;
            this.host.Width = 150;
            // 
            // port
            // 
            this.port.HeaderText = "Port";
            this.port.Name = "port";
            this.port.ReadOnly = true;
            this.port.Width = 50;
            // 
            // service_name
            // 
            this.service_name.HeaderText = "Service_name";
            this.service_name.Name = "service_name";
            this.service_name.ReadOnly = true;
            this.service_name.Width = 150;
            // 
            // AbonentChooseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 501);
            this.Controls.Add(this.btn_choose);
            this.Controls.Add(this.dgv_tns);
            this.Name = "AbonentChooseForm";
            this.Text = "AbonentChooseForm";
            this.Load += new System.EventHandler(this.AbonentChooseForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_tns)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgv_tns;
        private System.Windows.Forms.Button btn_choose;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn protocol;
        private System.Windows.Forms.DataGridViewTextBoxColumn host;
        private System.Windows.Forms.DataGridViewTextBoxColumn port;
        private System.Windows.Forms.DataGridViewTextBoxColumn service_name;
    }
}