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
    public partial class ComCommonPark : Form
    {
        public static BindingSource bs = new BindingSource();

        public ComCommonPark()
        {
            InitializeComponent();
        }

        private void CommonMess_Load(object sender, EventArgs e)
        {
            bs.DataSource = DAL.CommonMessDAL.QueryListByCondition();
            dataGridView1.DataSource = bs;
            //宽度自适应
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
        }
        //选中车位
        private void button2_Click(object sender, EventArgs e)
        {

            //用于测试，后期要删掉
            //DAL.CommonMessDAL.change("B001", "unpacked");
            //DAL.CommonMessDAL.change("B002", "unpacked");
            //DAL.CommonMessDAL.change("B003", "unpacked");
            //DAL.CommonMessDAL.change("B004", "unpacked");
            Model.CommonMess commonmess = (Model.CommonMess)bs.Current;
            if (commonmess.PackingCondition.Trim() == "unpacked")                                                
            {
                commonmess.PackingCondition = "packed";
                MessageBox.Show("已选中" + commonmess.PlaceID.Trim() + "车位");
                //重置DataView
                bs.ResetCurrentItem();
                //改变车位状态
                DAL.CommonMessDAL.change(commonmess.PlaceID, commonmess.PackingCondition);
                //"B103"替换成桌面式读卡器获取的卡号
                DAL.CommonPayRecordDAL.addCommonPayRecord("B003", commonmess.PlaceID,"common");
                //DAL.CommonPayRecordDAL.insertTimebyCardID("B003");
               // DAL.CommonPayRecordDAL.insertTimebyCardID("B003");

            }
            else
            {
                //DAL.CommonMessDAL.change("B001", "unpacked");
                //DAL.CommonMessDAL.change("B002", "unpacked");
                //DAL.CommonMessDAL.change("B003", "unpacked");
                //DAL.CommonMessDAL.change("B004", "unpacked");
                MessageBox.Show(commonmess.PlaceID.Trim() + "车位已经有车，请重新选择");
            }

        }
        //查看空余车位
        private void button1_Click(object sender, EventArgs e)
        {
            ComCommonFreePark commonfreepark = new ComCommonFreePark();
            commonfreepark.StartPosition = FormStartPosition.CenterScreen;
            commonfreepark.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {
           

        }
        //上一行
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell.RowIndex >0)
            {
                this.dataGridView1.CurrentCell = this.dataGridView1[0, dataGridView1.CurrentCell.RowIndex - 1];
            }
            else
            {
                MessageBox.Show("已是第一行");
            }
        }
        //下一行
        private void button4_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dataGridView1.Rows.Count + "  " + dataGridView1.CurrentCell.RowIndex);
            if (dataGridView1.CurrentCell.RowIndex < dataGridView1.Rows.Count - 2)
            {
                this.dataGridView1.CurrentCell = this.dataGridView1[0, dataGridView1.CurrentCell.RowIndex + 1];
            }
            else
            {
                MessageBox.Show("已是最后一行");
            }
        }
        //按照车位查询
        private void button5_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.Rows.Count;//得到总行数 
            //得到总行数并在之内循环 
            for (int i = 0; i < row; i++)//得到总行数并在之内循环            
            {
                //对比TexBox中的值是否与dataGridView中的值相同 
                if (textBox1.Text.Trim() == dataGridView1.Rows[i].Cells[0].Value.ToString().Trim())
                {
                    //定位到单元格
                    this.dataGridView1.CurrentCell = this.dataGridView1[0, i];
                    return;
                }
            }
        }
      
    }
}
