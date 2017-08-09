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
    public partial class CasualUserPacking : Form
    {
        public CasualUserPacking()
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

        private void CasualUserPacking_Load(object sender, EventArgs e)
        {

            _reader = null;
            textBox1.Text = "";
            Connect();
        }
         //连接读写器
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

        private void button4_Click(object sender, EventArgs e)
        {

        }


        string time;
        public static DateTime timenow;
        int i;

        private void button3_Click_1(object sender, EventArgs e)
        {
            float h = DateTime.Now.Hour;
            float m = DateTime.Now.Minute;
            //int h = Convert.ToInt32(textBox2.Text);
            float t = Convert.ToInt32(textBox3.Text);
            float time1 = h + m / 60;
            float time2 = h + t + m / 60;

            String PlaceID = DAL.SecretPlaceDAL.TemporaryPacking(time1, time2);
            if (PlaceID != null)
            {
                textBox6.Text = PlaceID;
                textBox4.Text = timenow.ToString();
                textBox5.Text = timenow.AddHours(t).ToString();
                timenow = timenow.AddHours(t);

                i = 1;

            }
            else
            {
                MessageBox.Show("当前没有可租赁车位!");
                i = 0;
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
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

        //private void button2_Click_1(object sender, EventArgs e)
        //{
        //    //int h = DateTime.Now.Hour;
        //    time = DateTime.Now.ToLongTimeString().ToString();
        //    timenow = DateTime.Now;
        //    label7.Text = time.ToString();
        //}

        private void button4_Click_2(object sender, EventArgs e)
        {
            if (i == 1)
            {

                PositionManage.Form1 connect2 = new PositionManage.Form1();
                connect2.getCardID = textBox1.Text;
                MessageBox.Show(textBox6.Text);
                string placeid = textBox6.Text;
                DAL.SecretPlaceDAL.changePC(textBox6.Text);
                DAL.SecretPlaceDAL.AddCommonPayRecord(textBox1.Text, textBox6.Text);
                connect2.getplaceID = placeid;
                connect2.Show();
            }
            else
                MessageBox.Show("无法停车");
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (i == 1)
            {
                //DAL.SecretPlaceDAL.changePC(textBox6.Text);
                //DAL.SecretPlaceDAL.AddCommonPayRecord(textBox1.Text, textBox6.Text);
                this.Close();
            }
            else
                MessageBox.Show("无法停车");

        }

        private void CasualUserPacking_Load_1(object sender, EventArgs e)
        {
            time = DateTime.Now.ToLongTimeString().ToString();
            timenow = DateTime.Now;
            label7.Text = time.ToString();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        
    }
}
