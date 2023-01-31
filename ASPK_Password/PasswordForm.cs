using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASPK_Password
{
    public partial class PasswordForm : Form
    {
        public PasswordForm()
        {
            InitializeComponent();
            Form1 main = this.Owner as Form1;
        }

        private void PasswordForm_Load(object sender, EventArgs e)
        {
            cb_user.SelectedIndex = 0;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
