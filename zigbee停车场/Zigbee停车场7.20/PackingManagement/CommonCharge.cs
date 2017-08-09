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
    public partial class CommonCharge : Form
    {
        public static BindingSource bs = new BindingSource();

        public CommonCharge()
        {
            InitializeComponent();
        }

        private static ReaderHandle _reader;
        private static bool _isConnected = false;
        private static string _comPort;

        public static ReaderHandle Reader
        {
            set { _reader = value; }
            get { return _reader; }
        }

        public static bool IsConnected
        {
            set { _isConnected = value; }
            get { return _isConnected; }
        }

        public static string ComPort
        {
            set { _comPort = value; }
            get { return _comPort; }
        }
        private void Connect()
        {
            if (_reader == null)
            {
                _reader = new ReaderHandle();

                _isConnected = _reader.Connect();
            }

            if (!_isConnected)
            {
                MessageBox.Show("读写器连接失败");
            }
            else
            {
               // MessageBox.Show("读写器连接成功");
            }

        }

        private void CommonPack_Load(object sender, EventArgs e)
        {
            bs.DataSource = DAL.CommonPayRecordDAL.QueryListByCondition();
            dataGridView1.DataSource = bs;
            //宽度自适应
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            label1.Text = "公共车位普通用户每小时收费："+allCharge[0].ToString();
            label3.Text = "固定车位临时用户每小时收费：" + allCharge[2].ToString();

            textBox1.Text = "";

        }

        private int[] allCharge;
        private DateTime PreOutTime;
        //结算按钮，点击后，指定到对应行，同时弹出费用信息，并更新数据库
        private void button1_Click(object sender, EventArgs e)
        {
          
            bs.DataSource = DAL.CommonPayRecordDAL.QueryListByCondition();
            dataGridView1.DataSource = bs;
            //bs.DataSource = DAL.CommonPayRecordDAL.queryByCardID(this.textBox1.Text.Trim());
            //dataGridView1.DataSource = bs;
            ////宽度自适应
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            int revenue = 0;
            string CardID = this.textBox1.Text.Trim();
            int row = -1;
            int rows = dataGridView1.Rows.Count;//得到总行数 

            //当出库用户为固定用户时
            if (CardID[0] == 'A')
            {

                if (MessageBox.Show("卡号为" + CardID + "的用户应缴费0元，请归还定位卡！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DAL.SecretUserDAL.ModifyPackingPlace(CardID);
                }

            }
            //得到总行数并在之内循环,获取最新一行 
            for (int i = rows - 2; i > -1; i--)//得到总行数并在之内循环            
            {
                //MessageBox.Show(i.ToString());
                //MessageBox.Show(dataGridView1.Rows[i].Cells[0].Value.ToString().Trim());
                //对比TexBox中的值是否与dataGridView中的值相同 
                if (CardID == dataGridView1.Rows[i].Cells[6].Value.ToString().Trim())
                {

                    //定位到单元格
                    this.dataGridView1.CurrentCell = this.dataGridView1[0, i];
                    row = i;
                    break;
                }
            }
            if (row != -1)
            {
                if (dataGridView1.Rows[row].Cells[3].Value.ToString().Trim() == "0001/1/1 0:00:00")
                {
                    MessageBox.Show("车辆未进入车库，无法结算");
                }
                else if (dataGridView1.Rows[row].Cells[4].Value.ToString().Trim() == "0001/1/1 0:00:00")
                {
                    MessageBox.Show("车辆未出车库，无法结算");
                }
                else
                {
                    //获取时间，结算
                    DateTime InTime = DateTime.Parse(dataGridView1.Rows[row].Cells[3].Value.ToString().Trim());
                    DateTime OutTime = DateTime.Parse(dataGridView1.Rows[row].Cells[4].Value.ToString().Trim());
                    string placeType = dataGridView1.Rows[row].Cells[2].Value.ToString().Trim();
                    string PlaceID = dataGridView1.Rows[row].Cells[7].Value.ToString().Trim();
                    int timeDiff = (OutTime.Day - InTime.Day) * 24 * 60 + (OutTime.Hour - InTime.Hour) * 60 + OutTime.Minute - InTime.Minute;
                    if (placeType == "common")
                    {

                        revenue = timeDiff * allCharge[0];
                        // MessageBox.Show(timeDiff+"  "+allCharge[0]);
                    }
                    else
                    {
                        revenue = timeDiff * allCharge[2] * 2;
                        int revenue1 = timeDiff * allCharge[2];
                        DAL.SecretUserDAL.ModifyRe(PlaceID,revenue1);
                    }
                        
                    MessageBox.Show("卡号为" + CardID + "的用户应缴费" + revenue.ToString() + "元");
                    dataGridView1.Rows[row].Cells[5].Value = revenue.ToString();
                    int RecordID = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString().Trim());
                    DAL.CommonPayRecordDAL.insertRevenuebyRecordID(RecordID, revenue);
                   // DAL.CommonMessDAL.change(dataGridView1.Rows[row].Cells[7].Value.ToString().Trim(), "packed");
                    DAL.PackingPlaceDAL.ModifyPackingPlace(PlaceID);
                    //queryByCardID(cardid);
                }
            }



        }
        public DateTime GetOutTime
        {
            set
            {
                PreOutTime = value;
            }
            get
            {
                return PreOutTime;
            }
        }
        public int[] getAllCharge
        {
            set
            {
                allCharge = value;
            }
            get
            {
                return allCharge;
            }
        }
        //获取卡号
        private void button2_Click(object sender, EventArgs e)
        {
            _reader = null;
            textBox1.Text = "";
            Connect();
            textBox1.Text = _reader.ReadEpc();

            if (_reader != null)
            {
                _reader.DisConnect();
                _isConnected = false;
                _reader = null;
            }
        }
    }
}
