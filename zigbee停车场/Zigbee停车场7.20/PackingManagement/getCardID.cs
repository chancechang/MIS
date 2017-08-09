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
    public partial class getCardID : Form
    {
        public getCardID()
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
        private void getCardID_Load(object sender, EventArgs e)
        {
            
        }
        public static string placeID;
        public string getplaceID
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
                //MessageBox.Show("读写器连接成功");
            }

        }
        //写入
        private void button2_Click(object sender, EventArgs e)
        {
            DAL.CommonMessDAL.change(placeID, "packed");
            DAL.CommonPayRecordDAL.addCommonPayRecord(this.textBox1.Text.Trim(), placeID,"common");
            this.Close();
        }

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
        }

    }
}
