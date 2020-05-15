using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QR1
{
    public partial class formResourceManagement : Form
    {
        public bool starting = true;
        public formResourceManagement()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var nform = new formRMLogin();
            nform.Show();
            this.Close();
        }

        private void formResourceManagement_Load(object sender, EventArgs e)
        {
            using (var db = new Session1Entities())
            {
                var typeList = (from x in db.Resource_Type
                                select x.resTypeName).ToList();
                var skillList = (from x in db.Skills
                                 select x.skillName).ToList();
                cbType.DataSource = typeList;
                cbSkill.DataSource = skillList;
            }

            fillDGV(-1, -1);

            starting = false;
        }

        private void fillDGV(int skillID, int typeID)
        {
            dataGridView1.Rows.Clear();
            var resList = new List<int>();
            using (var db = new Session1Entities())
            {
                if (skillID == -1)
                {
                    resList = (from x in db.Resources
                               orderby x.remainingQuantity descending
                               select x.resId).ToList();
                }
                else
                {
                    resList = (from x in db.Resource_Allocation
                               where x.skillIdFK == skillID && x.Resource.resTypeIdFK == typeID
                               orderby x.Resource.remainingQuantity descending
                               select x.resIdFK).ToList();
                }

                foreach (var resid in resList)
                {
                    var skills = from x in db.Resource_Allocation
                                 where x.resIdFK == resid
                                 select x;
                    var res = (from x in db.Resources
                               where x.resId == resid
                               select x).FirstOrDefault();

                    //making skill names
                    var skillname = "";
                    if (skills.Count() != 0)
                    {
                        foreach (var q in skills)
                        {
                            skillname = $"{skillname}, {q.Skill.skillName}";
                        }
                    }
                    else
                    {
                        skillname = "Nil";
                    }

                    //filtering into avail qty
                    if (res.remainingQuantity > 5)
                    {
                        dataGridView1.Rows.Add(res.resId, res.resName, res.Resource_Type.resTypeName, skills.Count(), skillname, "Sufficient");
                    }
                    else if (res.remainingQuantity > 0)
                    {
                        dataGridView1.Rows.Add(res.resId, res.resName, res.Resource_Type.resTypeName, skills.Count(), skillname, "Low Stock");
                    }
                    else
                    {
                        var row = dataGridView1.Rows.Add(res.resId, res.resName, res.Resource_Type.resTypeName, skills.Count(), skillname, "Not Available");
                        dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.Red;
                    }

                }
            }
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!starting)
            {
                using (var db = new Session1Entities())
                {
                    var skillid = (from x in db.Skills
                                   where x.skillName == cbSkill.SelectedItem.ToString()
                                   select x.skillId).FirstOrDefault();
                    var typeid = (from x in db.Resource_Type
                                  where x.resTypeName == cbSkill.SelectedItem.ToString()
                                  select x.resTypeId).FirstOrDefault();
                    fillDGV(skillid, typeid);
                }
            }
        }

        private void cbSkill_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!starting)
            {
                using (var db = new Session1Entities())
                {
                    var skillid = (from x in db.Skills
                                   where x.skillName == cbSkill.SelectedItem.ToString()
                                   select x.skillId).FirstOrDefault();
                    var typeid = (from x in db.Resource_Type
                                  where x.resTypeName == cbSkill.SelectedItem.ToString()
                                  select x.resTypeId).FirstOrDefault();
                    fillDGV(skillid, typeid);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //add button
            var nform = new formAddNewResource();
            nform.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //update
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var restoupdate = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                var nform = new formUpdateResource(restoupdate);
                nform.Show();
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //delete
            if (dataGridView1.SelectedRows.Count != 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    var resid = int.Parse(row.Cells[0].Value.ToString());
                    using (var db = new Session1Entities())
                    {
                        var restodel = (from x in db.Resources
                                        where x.resId == resid
                                        select x).First();
                        db.Resources.Remove(restodel);
                        db.SaveChanges();
                        fillDGV(-1, -1);
                    }
                }
            }
        }
    }
}
