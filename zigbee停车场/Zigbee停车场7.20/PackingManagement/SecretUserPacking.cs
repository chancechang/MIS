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
    public partial class SecretUserPacking : Form
    {
        public SecretUserPacking()
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

        private void SecretUserPacking_Load(object sender, EventArgs e)
        {
            _reader = null;
            textBox1.Text = "";
            Connect();
        }
        // 连接读写器
        private void Connect()
        {
            if (_reader == null)
            {
                _reader = new ReaderHandle();

                _isConnected = _reader.Connect();
            }

            if (!_isConnected)
            {
                MessageBox.Show("桌面式读写器连接失败");
            }
            else
            {
               //MessageBox.Show("桌面式读写器连接成功");
            }

        }

        // 关闭读写器
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.DisConnect();
                _isConnected = false;
                _reader = null;
            }
        }

        int i;

        private void button1_Click(object sender, EventArgs e)
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


            i = DAL.SecretUserDAL.SecretUserPack(textBox1.Text,placeID);
            if (i == 0)
                MessageBox.Show("请缴费", "提醒");
            else if (i == 1)
                MessageBox.Show("可以停车", "提醒");
            else if (i == 2)
                MessageBox.Show("车位已被占用", "提醒");
            else if (i == 3)
                MessageBox.Show("请重新选择正确车位！", "提醒");
            //根据placeid查询cardid并验证
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (i == 1)
            {
                DAL.SecretUserDAL.changePC(textBox1.Text);
                this.Close();
            }
            else
                MessageBox.Show("无法停车");
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (i == 1)
            {
                string placeid = DAL.SecretUserDAL.Getplaceid(textBox1.Text);
                PositionManage.Form1 connect2 = new PositionManage.Form1();
                connect2.getCardID = textBox1.Text;
                connect2.getplaceID = placeid;
                connect2.Show();
            }
            else
                MessageBox.Show("无法停车");

        }

        private void SecretUserPacking_Load_1(object sender, EventArgs e)
        {
          
        }
        public static string placeID;
        public  string getplaceID
        {
            set
            {
                placeID = value;
            }
            get
            {
                return placeID;
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
       
    }
}
