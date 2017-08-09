using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;

namespace PackingManagement
{
    public partial class AddAdmin : Form
    {
        public AddAdmin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(textBox4.Text);
            string Name = textBox1.Text;
            string Password = textBox2.Text;
            string Right1 = comboBox1.Text;
            string Phone = textBox5.Text;
            Model.Administrator model = new Model.Administrator();
            model.Name = Name;
            model.Password = Password;
            model.Phone = Phone;
            model.Right1 = Right1;
            model.ID = ID;
             int a  = AdminDAL.ADD(model);
            if (a != 0)
                MessageBox.Show("添加管理员成功");
            Administrator.bs.DataSource = DAL.AdminDAL.AdminListByCondition(null);
            Administrator.bs.ResetCurrentItem();
            Administrator.bs.ResetBindings(false);
            this.Close();
        }
    }
}
