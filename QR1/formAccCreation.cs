using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QR1
{
    public partial class formAccCreation : Form
    {
        public formAccCreation()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var nform = new formMain();
            nform.Show();
            this.Close();
        }

        private void formAccCreation_Load(object sender, EventArgs e)
        {
            using (var db = new Session1Entities())
            {
                var userTypeList = (from x in db.User_Type
                                    select x.userTypeName).ToList();
                cbUserType.DataSource = userTypeList;
            }
        }

        private void btnCreateAcc_Click(object sender, EventArgs e)
        {
            //data entry check
            var error = true;
            if (tbUserID.TextLength < 8)
            {
                MessageBox.Show("User ID must be at least 8 characters.");
                error = false;
                
            }
            if (tbPW.Text != tbRPW.Text)
            {
                MessageBox.Show("Passwords do not match.");
                error = false;
            }

            //make the acc if no error
            using (var db = new Session1Entities())
            {
                var userList = (from x in db.Users
                                select x.userId).ToList();
                if (userList.Contains(tbUserID.Text))
                {
                    MessageBox.Show("User ID is already in use.");
                }
                else
                {
                    var newUser = new User();
                    newUser.userName = tbUserName.Text;
                    newUser.userId = tbUserID.Text;
                    newUser.userPw = tbPW.Text;
                    newUser.userTypeIdFK = cbUserType.SelectedIndex + 1;

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    var nform = new formMain();
                    nform.Show();
                    this.Close();
                }
            }
        }
    }
}
