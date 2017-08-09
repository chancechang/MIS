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
    public partial class ComCommonFreePark : Form
    {
        public static BindingSource bs = new BindingSource();

        public ComCommonFreePark()
        {
            InitializeComponent();
        }
        private void CommonFreePark_Load(object sender, EventArgs e)
        {
            //显示空闲车位
            bs.DataSource = DAL.CommonMessDAL.QueryListByFree();
            dataGridView1.DataSource = bs;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        //选中车位
        private void button1_Click(object sender, EventArgs e)
        {
            Model.CommonMess commonmess = (Model.CommonMess)bs.Current;
            commonmess.PackingCondition = "packed";
            MessageBox.Show("已选中" + commonmess.PlaceID + "车位");
            DAL.CommonMessDAL.change(commonmess.PlaceID, commonmess.PackingCondition);
            bs.DataSource = DAL.CommonMessDAL.QueryListByFree();
            dataGridView1.DataSource = bs;
        }
        //上一行
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell.RowIndex > 0)
            {
                this.dataGridView1.CurrentCell = this.dataGridView1[0, dataGridView1.CurrentCell.RowIndex - 1];
            }
            else
            {
                MessageBox.Show("已是第一行");
            }
        }
        //下一行
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell.RowIndex < dataGridView1.Rows.Count - 2)
            {
                this.dataGridView1.CurrentCell = this.dataGridView1[0, dataGridView1.CurrentCell.RowIndex + 1];
            }
            else
            {
                MessageBox.Show("已是最后一行");
            }
        }
    }
}
