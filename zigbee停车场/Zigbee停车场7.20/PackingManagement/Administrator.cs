using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DAL;


namespace PackingManagement
{
    public partial class Administrator : Form
    {
        public static BindingSource bs = new BindingSource();
        public Administrator()
        {
            InitializeComponent();
        }

        private void Administrator_Load(object sender, EventArgs e)
        {
            //List作为BindingSource的数据源
            bs.DataSource = DAL.AdminDAL.AdminListByCondition(null);
            dataGridView1.DataSource = bs;
            //dataGridView1.DataSource = CommonPayRecordDAL.QueryCommonPay().Tables[0]
            //dataGridView1.Columns[2].Visible = false;//隐藏密码
            //for (int j = 0; j < this.dataGridView1.Columns.Count; j++)
            //{
            //    this.dataGridView1.Columns[i].HeaderText = AdminDAL.
            //}
            this.dataGridView1.Columns[0].HeaderText ="编号";
            this.dataGridView1.Columns[1].HeaderText ="姓名";
            this.dataGridView1.Columns[2].HeaderText ="密码";
            this.dataGridView1.Columns[3].HeaderText ="联系方式";
            this.dataGridView1.Columns[4].HeaderText = "权限";
               

            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                if (i != 2 && i != 3)
                    comboBox1.Items.Add(this.dataGridView1.Columns[i].HeaderText);
            }
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
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
                textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            else textBox3.Text = null;
            if (dataGridView1.CurrentRow.Cells[4].Value != null)
                textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            else textBox4.Text = null;
            if (dataGridView1.CurrentRow.Cells[3].Value != null)
                textBox5.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            else textBox5.Text = null;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Model.Administrator model = new Model.Administrator();
            model.ID = int.Parse(textBox1.Text);
            model.Name = textBox2.Text;
            model.Password = textBox3.Text;
            model.Right1 = textBox4.Text;
            model.Phone = textBox5.Text;
            DAL.AdminDAL.Modify(model);
            bs.DataSource = DAL.AdminDAL.AdminListByCondition(null);
            dataGridView1.DataSource = bs;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string ID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            int a = DAL.AdminDAL.DelById(ID);
            if (a == 1)
            {
                MessageBox.Show("删除成功");
                bs.DataSource = DAL.AdminDAL.AdminListByCondition(null);
                dataGridView1.DataSource = bs;

            }
            else
            {
                MessageBox.Show("删除失败");
                bs.DataSource = DAL.AdminDAL.AdminListByCondition(null);
                dataGridView1.DataSource = bs;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlParameter[] paras = new SqlParameter[1];
            if (comboBox1.Text.Trim() == "编号")//按ID查找
            {
                paras[0] = new SqlParameter("@ID", Convert.ToInt32(textBox6.Text));
                bs.DataSource = AdminDAL.AdminListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
                dataGridView1.DataSource = bs;
            }
            else if (comboBox1.Text.Trim() == "姓名")//按Name查找
            {
                paras[0] = new SqlParameter("@Name", textBox6.Text);
                bs.DataSource = AdminDAL.AdminListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
                dataGridView1.DataSource = bs;
            }
            else if (comboBox1.Text.Trim() == "权限")//按Right查找
            {
                paras[0] = new SqlParameter("@Right1", textBox6.Text);
                bs.DataSource = AdminDAL.AdminListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
                dataGridView1.DataSource = bs;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bs.DataSource = DAL.AdminDAL.AdminListByCondition(null);
            dataGridView1.DataSource = bs;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddAdmin a = new AddAdmin();
            a.StartPosition = FormStartPosition.CenterScreen;
            a.Show();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
