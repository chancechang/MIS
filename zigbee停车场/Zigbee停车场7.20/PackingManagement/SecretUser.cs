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
    public partial class SecretUser : Form
    {
        public static BindingSource bs = new BindingSource();
        public SecretUser()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Model.SecretUser model = new Model.SecretUser();
            model.ID = int.Parse(textBox1.Text);
            model.Name = textBox2.Text;
            model.Gender = textBox3.Text;
            model.CardID = textBox4.Text;
            model.Phone = textBox5.Text;
            model.BankCard = textBox7.Text;
            DAL.SecretUserDAL.Modify(model);
            bs.DataSource = DAL.SecretUserDAL.SecretUListByCondition(null);
            dataGridView1.DataSource = bs;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string ID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            int a = DAL.SecretUserDAL.DelById(ID);
            string CID = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            int b = DAL.SecretCardDAL.DelById(CID);
            //int c = DAL.SecretPlaceDAL.DelById1(CID);
            if (a == 1)
            {
                MessageBox.Show("删除成功");
                bs.DataSource = DAL.SecretUserDAL.SecretUListByCondition(null);
                dataGridView1.DataSource = bs;

            }
            else
            {
                MessageBox.Show("删除失败");
                bs.DataSource = DAL.SecretUserDAL.SecretUListByCondition(null);
                dataGridView1.DataSource = bs;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlParameter[] paras = new SqlParameter[1];
            if (comboBox1.Text.Trim() == "编号")//按ID查找
            {
                paras[0] = new SqlParameter("@ID", Convert.ToInt32(textBox6.Text));
                bs.DataSource = SecretUserDAL.SecretUListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
                dataGridView1.DataSource = bs;
            }
            else if (comboBox1.Text.Trim() == "姓名")//按Name查找
            {
                paras[0] = new SqlParameter("@Name", textBox6.Text);
                bs.DataSource = SecretUserDAL.SecretUListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
                dataGridView1.DataSource = bs;
            }
            else if (comboBox1.Text.Trim() == "性别")//按Right查找
            {
                paras[0] = new SqlParameter("@Gender", textBox6.Text);
                bs.DataSource = SecretUserDAL.SecretUListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
                dataGridView1.DataSource = bs;
            }
            else if (comboBox1.Text.Trim() == "卡号")
            {
                paras[0] = new SqlParameter("@CardID", textBox6.Text);
                bs.DataSource = SecretUserDAL.SecretUListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
                dataGridView1.DataSource = bs;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bs.DataSource = DAL.SecretUserDAL.SecretUListByCondition(null);
            dataGridView1.DataSource = bs;
        }

        private void SecretUser_Load(object sender, EventArgs e)
        {
            //List作为BindingSource的数据源
            bs.DataSource = DAL.SecretUserDAL.SecretUListByCondition(null);
            dataGridView1.DataSource = bs;
            this.dataGridView1.Columns[0].HeaderText = "编号";
            this.dataGridView1.Columns[1].HeaderText = "姓名";
            this.dataGridView1.Columns[2].HeaderText = "性别";
            this.dataGridView1.Columns[3].HeaderText = "卡号";
            this.dataGridView1.Columns[4].HeaderText = "联系方式";
            this.dataGridView1.Columns[5].HeaderText = "银行卡号";
            //dataGridView1.Columns[2].Visible = false;//隐藏密码
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                if (i != 5 && i != 4)
                    comboBox1.Items.Add(this.dataGridView1.Columns[i].HeaderText);
            }
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox7.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
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
            if (dataGridView1.CurrentRow.Cells[3].Value != null)
                textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            else textBox4.Text = null;
            if (dataGridView1.CurrentRow.Cells[4].Value != null)
                textBox5.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            else textBox5.Text = null;
            if (dataGridView1.CurrentRow.Cells[5].Value != null)
                textBox7.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            else textBox7.Text = null;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddSU a = new AddSU();
            a.StartPosition = FormStartPosition.CenterScreen;
            a.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

       
    }
}
