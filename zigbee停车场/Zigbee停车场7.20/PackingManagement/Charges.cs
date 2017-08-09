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
    public partial class Charges : Form
    {

        public Charges()
        {
            InitializeComponent();
          
            textBox1.Text = Form1.allCharges[0].ToString();
            textBox2.Text = Form1.allCharges[1].ToString();
            textBox3.Text = Form1.allCharges[2].ToString();

        }

        private int[] chargeList = new int[3];


        private void button1_Click(object sender, EventArgs e)
        {
            chargeList[0] = int.Parse(textBox1.Text.Trim());
            chargeList[1] = int.Parse(textBox2.Text.Trim());
            chargeList[2] = int.Parse(textBox3.Text.Trim());
            if(chargeList[0]==0 || chargeList[1]==0|| chargeList[2]==0)
            {
                MessageBox.Show("请输入合法数据");
            }
            else
            {
                Form1.allCharges = chargeList;
                this.Close();
            }
            
        }
        public int[] getChargeList
        {
            get
            {              
                return chargeList;
            }
        }

        private void Charges_Load(object sender, EventArgs e)
        {

        }

       
    }
}
