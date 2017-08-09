using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackingManagement
{
    public partial class 可租用时间 : Form
    {
        public static BindingSource bs = new BindingSource();
        public 可租用时间()
        {
            InitializeComponent();
        }

        private void 可租用时间_Load(object sender, EventArgs e)
        {
            bs.DataSource = DAL.RentingTimeDAL.RentingTimeListByCondition(null);
            dataGridView1.DataSource = bs;
            this.dataGridView1.Columns[0].HeaderText = "位置编号";
            this.dataGridView1.Columns[1].HeaderText = "开始时间1";
            this.dataGridView1.Columns[2].HeaderText = "结束时间1";
            this.dataGridView1.Columns[3].HeaderText = "开始时间2";
            this.dataGridView1.Columns[4].HeaderText = "结束时间2";
            this.dataGridView1.Columns[5].HeaderText = "开始时间3";
            this.dataGridView1.Columns[6].HeaderText = "结束时间3";
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox8.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox9.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox7.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox11.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox10.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Model.RentingTime model = new Model.RentingTime();
            model.PlaceID = textBox1.Text;
            model.StartTime1 = float.Parse(textBox8.Text);
            model.StopTime1 = float.Parse(textBox9.Text);
            model.StartTime2 = float.Parse(textBox7.Text);
            model.StopTime2 = float.Parse(textBox3.Text);
            model.StartTime3 = float.Parse(textBox11.Text);
            model.StopTime3 = float.Parse(textBox10.Text);
            DAL.RentingTimeDAL.Modify(model);
            bs.DataSource = DAL.RentingTimeDAL.RentingTimeListByCondition(null);
            dataGridView1.DataSource = bs;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells[0].Value != null)
                textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            else textBox1.Text = null;
            if (dataGridView1.CurrentRow.Cells[1].Value != null)
                textBox8.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            else textBox8.Text = null;
            if (dataGridView1.CurrentRow.Cells[2].Value != null)
                textBox9.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            else textBox9.Text = null;
            if (dataGridView1.CurrentRow.Cells[3].Value != null)
                textBox7.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            else textBox7.Text = null;
            if (dataGridView1.CurrentRow.Cells[4].Value != null)
                textBox3.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            else textBox3.Text = null;
            if (dataGridView1.CurrentRow.Cells[5].Value != null)
                textBox11.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            else textBox11.Text = null;
            if (dataGridView1.CurrentRow.Cells[6].Value != null)
                textBox10.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            else textBox10.Text = null;

        }

        
    }
}
