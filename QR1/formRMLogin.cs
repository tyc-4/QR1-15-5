using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QR1
{
    public partial class formRMLogin : Form
    {
        public formRMLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var db = new Session1Entities())
            {
                var checkUser = from x in db.Users
                                where x.userId == tbUser.Text && x.userPw == tbPW.Text
                                select x;

                if (checkUser.Count() != 1)
                {
                    MessageBox.Show("Invalid Username or Password!");
                }
                else if (checkUser.First().userTypeIdFK != 1)
                {
                    MessageBox.Show("This tool is only for resource managers.");
                }
                else
                {
                    //login success
                    MessageBox.Show($"Welcome, {checkUser.First().userName}");
                    var nform = new formResourceManagement();
                    nform.Show();
                    this.Close();
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var nform = new formMain();
            nform.Show();
            this.Close();
        }
    }
}
