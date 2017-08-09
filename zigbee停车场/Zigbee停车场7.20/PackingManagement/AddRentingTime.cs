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
    public partial class AddRentingTime : Form
    {
        public string Pid;
        public AddRentingTime(string PID)
        {
            InitializeComponent();
            Pid = PID;
        }
        private void AddRentingTime_Load(object sender, EventArgs e)
        {
            textBox1.Text = Pid;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float StartTime1 = float.Parse(textBox8.Text);
            float StopTime1 = float.Parse(textBox9.Text);
            float StartTime2 = float.Parse(textBox7.Text);
            float StopTime2 = float.Parse(textBox3.Text);
            float StartTime3 = float.Parse(textBox11.Text);
            float StopTime3 = float.Parse(textBox10.Text);
            Model.RentingTime model4 = new Model.RentingTime();
            model4.PlaceID = Pid;
            model4.StartTime1 = StartTime1;
            model4.StartTime2 = StartTime2;
            model4.StartTime3 = StartTime3;
            model4.StopTime1 = StopTime1;
            model4.StopTime2 = StopTime2;
            model4.StopTime3 = StopTime3;
            int d = DAL.RentingTimeDAL.ADDRentingT(model4);
            if (d != 0)
                MessageBox.Show("添加可租用信息成功");
            this.Close();
        }

        
    }
}
