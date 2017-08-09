using PackingManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PositionManage
{
    public  partial class 租用信息1 : Form
    {
        public static BindingSource bs = new BindingSource();
        public string IS;
        public 租用信息1()
        {
            InitializeComponent();
            IS = comboBox1.Text;
        }

        private void 租用信息_Load(object sender, EventArgs e)
        {
            //List作为BindingSource的数据源
            bs.DataSource = DAL.SecretPlaceDAL.SecretPListByCondition(null);
            dataGridView1.DataSource = bs;
            //dataGridView1.Columns[2].Visible = false;//隐藏密码
            this.dataGridView1.Columns[0].HeaderText = "位置编号";
            this.dataGridView1.Columns[1].HeaderText = "卡号";
            this.dataGridView1.Columns[2].HeaderText = "是否允许租用";

            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            IS = comboBox1.Text;
           
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells[0].Value != null)
                textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            else textBox1.Text = null;
            if (dataGridView1.CurrentRow.Cells[1].Value != null)
                textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            else textBox2.Text = null;
            if (dataGridView1.CurrentRow.Cells[2].Value != null)
                comboBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            else comboBox1.Text = null;
            IS = comboBox1.Text.Trim();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Model.SecretPlace model = new Model.SecretPlace();
            model.PlaceID = textBox1.Text.Trim();
            model.CardID = textBox2.Text.Trim();
            model.IsAllowingRenting = comboBox1.Text.Trim();
            DAL.SecretPlaceDAL.Modify(model);
            if (IS == "allow" && comboBox1.Text.Trim() == "not")
            {
                string ID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                int a = DAL.RentingTimeDAL.DelById(ID);
            }
            if (IS == "not" && comboBox1.Text.Trim() == "allow")
            {
                AddRentingTime a = new AddRentingTime(textBox1.Text);
                a.StartPosition = FormStartPosition.CenterScreen;
                a.Show();
            }
            bs.DataSource = DAL.SecretPlaceDAL.SecretPListByCondition(null);
            dataGridView1.DataSource = bs;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string PlaceID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            int a = DAL.SecretPlaceDAL.DelById(PlaceID);
            if (a == 1)
            {
                MessageBox.Show(PlaceID +"删除成功");
                bs.DataSource = DAL.SecretPlaceDAL.SecretPListByCondition(null);
                dataGridView1.DataSource = bs;

            }
            else
            {
                MessageBox.Show(PlaceID+"删除失败");
                bs.DataSource = DAL.SecretPlaceDAL.SecretPListByCondition(null);
                dataGridView1.DataSource = bs;

            }
           
            int b = DAL.RentingTimeDAL.DelById(PlaceID);
            if (b == 1)
            {
                MessageBox.Show("删除成功");
                bs.DataSource = DAL.SecretPlaceDAL.SecretPListByCondition(null);
                dataGridView1.DataSource = bs;

            }
            //else
            //{
            //    MessageBox.Show("删除失败");
            //    bs.DataSource = DAL.SecretPlaceDAL.SecretPListByCondition(null);
            //    dataGridView1.DataSource = bs;

            //}
        }

        private void button6_Click(object sender, EventArgs e)
        {
            可租用时间 a = new 可租用时间();
            a.StartPosition = FormStartPosition.CenterScreen;
            a.Show();
        }
    }
}
