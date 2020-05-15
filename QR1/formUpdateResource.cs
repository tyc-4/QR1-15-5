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
    public partial class formUpdateResource : Form
    {
        public int resid;
        public formUpdateResource(int resId)
        {
            InitializeComponent();
            resid = resId;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var nform = new formResourceManagement();
            nform.Show();
            this.Close();
        }

        private void formUpdateResource_Load(object sender, EventArgs e)
        {
            using (var db = new Session1Entities())
            {
                var res = (from x in db.Resources
                           where x.resId == resid
                           select x).First();

                label7.Text = res.resName;
                label8.Text = res.Resource_Type.resTypeName;
                tbQty.Text = res.remainingQuantity.ToString();

                var skillList = (from x in db.Skills
                                 select x.skillName).ToList();
                foreach (var item in skillList)
                {
                    checkedListBox1.Items.Add(item);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var db = new Session1Entities())
            {
            var qcheck = int.TryParse(tbQty.Text, out int quantity);
                if (!qcheck || quantity < 0)
                {
                    MessageBox.Show("Invalid input for quantity");
                }
                else
                {
                    if (quantity == 0 && checkedListBox1.CheckedItems.Count > 0)
                    {
                        MessageBox.Show("You cannot assign items without any quantity to skills");
                    }
                    else
                    {
                        var res = (from x in db.Resources
                                  where x.resId == resid
                                  select x).First();

                        res.remainingQuantity = quantity;

                        var allocO = from x in db.Resource_Allocation
                                    where x.resIdFK == resid
                                    select x;
                        foreach (var a in allocO)
                        {
                            db.Resource_Allocation.Remove(a);
                        }
                        db.SaveChanges();

                        foreach (string skill in checkedListBox1.CheckedItems)
                        {
                            var skillid = (from x in db.Skills
                                           where x.skillName == skill
                                           select x.skillId).First();
                            var alloc = new Resource_Allocation();
                            alloc.resIdFK = res.resId;
                            alloc.skillIdFK = skillid;

                            db.Resource_Allocation.Add(alloc);
                        }

                        db.SaveChanges();

                        MessageBox.Show("update success");
                        var nform = new formResourceManagement();
                        nform.Show();
                        this.Close();
                    }
                }
            }
        }
    }
}
