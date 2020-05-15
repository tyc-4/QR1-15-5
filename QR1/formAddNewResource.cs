using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QR1
{
    public partial class formAddNewResource : Form
    {
        public formAddNewResource()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var nform = new formResourceManagement();
            nform.Show();
            this.Close();
        }

        private void formAddNewResource_Load(object sender, EventArgs e)
        {
            using (var db = new Session1Entities())
            {
                var skillList = (from x in db.Skills
                                 select x.skillName).ToList();
                foreach (var item in skillList)
                {
                    checkedListBox1.Items.Add(item);
                }
                var typelist = (from x in db.Resource_Type
                                select x.resTypeName).ToList();
                comboBox1.DataSource = typelist;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //add resource data checks
            using (var db = new Session1Entities())
            {
                var itemlist = from x in db.Resources
                               select x.resName;
                if (itemlist.Contains(tbResname.Text))
                {
                    MessageBox.Show("Resource already exist");
                }
                else
                {
                    var qcheck = int.TryParse(textBox1.Text, out int quantity);
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
                            var res = new Resource();
                            res.resName = tbResname.Text;
                            res.resTypeIdFK = comboBox1.SelectedIndex + 1;
                            res.remainingQuantity = quantity;

                            db.Resources.Add(res);
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
                            MessageBox.Show("add success");
                            var nform = new formResourceManagement();
                            nform.Show();
                            this.Close();
                        }
                    }
                }
            }
        }
    }
}
