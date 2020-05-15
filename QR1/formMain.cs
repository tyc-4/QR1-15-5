using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QR1
{
    public partial class formMain : Form
    {
        public formMain()
        {
            InitializeComponent();
        }

        private void btnCreateAcc_Click(object sender, EventArgs e)
        {
            var nform = new formAccCreation();
            nform.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var nform = new formRMLogin();
            nform.Show();
            this.Hide();
        }
    }
}
