using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using System.Data.SqlClient;
using PositionManage;

namespace PackingManagement
{
    public partial class Form1 : Form

    {
        public int C = 0;
        public static BindingSource bs = new BindingSource();
        public Form1()
        {
            InitializeComponent();
          
        }
        ModuleReaderManager.Form1 connect2 = new ModuleReaderManager.Form1();
        private void Form1_Load(object sender, EventArgs e)
        {
            //在实验室运行固定式读写器需要的代码
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormServer_FormClosed);

            connect2.connect("192.168.0.102", "四天线");
            connect2.start(true, false, false, false);
            System.Timers.Timer t = new System.Timers.Timer(15000);
            t.Elapsed += new System.Timers.ElapsedEventHandler(connect2.clear);
            t.AutoReset = true;
            t.Enabled = true;
            initpark(1);

            int a = DAL.AdminDAL.CheckUser();//判断登录者身份，是否为超级管理员
            if (a == 1)//登录者是超级管理员
            {
                label5.Text = "超级管理员";
            }
            else//登录者为系统管理员
            label5.Text = "系统管理员";
            label6.Text = DAL.AdminDAL.ReturnName();

            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.time1_Tick);
            this.timer1.Start();

       
            //textBox4.Text = DAL.RentingTimeDAL.checkPlaceCondition("common");
            //textBox5.Text = DAL.RentingTimeDAL.checkPlaceCondition("secret");
            //textBox6.Text = (18 - Convert.ToInt32(textBox4.Text)).ToString();
            //textBox7.Text = (6 - Convert.ToInt32(textBox5.Text)).ToString();
          
        }



        private void FormServer_FormClosed(object sender, EventArgs e)
        {
            connect2.disconnect1(null);
            Application.Exit();


        }
        private void time1_Tick(object sender, EventArgs e)
        {
            textBox3.Text = DateTime.Now.ToString();
        }

        private void initpark(int initParkNum)
        {
            //传入层数
            //1.从数据库查询
            //清空所有按钮


            foreach (Control ct in this.Controls)
            {
                if (ct is Button)
                {
                    if (ct.Name.StartsWith("btnpark"))
                    {
                        this.Controls.Remove(ct);
                    }
                }
            }
            for (int i = 0; i < 5; i++)
            {
                foreach (Control ct in this.Controls)
                {
                    if (ct is Label)
                    {
                        if (ct.Name.StartsWith("park"))
                        {
                            this.Controls.Remove(ct);
                        }
                    }
                }
            }

            int lastParkNum = initParkNum + 23;
            textBox4.Text = DAL.RentingTimeDAL.checkPlaceCondition("common");
            textBox5.Text = DAL.RentingTimeDAL.checkPlaceCondition("secret");
            textBox6.Text = (54 - Convert.ToInt32(textBox4.Text)).ToString();
            textBox7.Text = (18 - Convert.ToInt32(textBox5.Text)).ToString();
            int j = 0;
            for (int i = initParkNum; i <= lastParkNum; i++)
            {
                string placeid = i.ToString();
                if (i < 10)
                {
                    placeid = "0" + placeid;
                }

                Button button = new Button();
                button.Name = "btnpark" + i.ToString();
                button.Text = string.Empty;
                button.Width = 51;
                button.Height = 42;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
                Label label = new Label();
                label.Text = placeid;
                label.AutoSize = true;
                label.Font = new Font("隶书", 12, FontStyle.Bold);
                label.ForeColor = Color.White;
                label.Name = "park" + i.ToString();
               
                //button.Click += new System.EventHandler(this.btn_Click);
                string free = DAL.CommonMessDAL.QueryFreebyPlaceID(placeid);
                if (free.Trim() == "packed")
                {
                    button.Image   = Image.FromFile("car1.jpg");
                    button.Click += new System.EventHandler(this.btn1_Click);
                 

                }
                if (free.Trim() == "unpacked")
                {
                    button.Image = Image.FromFile("car2.png");
                    button.Click += new System.EventHandler(this.btn_Click);
                }
                
                this.Controls.Add(button);
                this.Controls.Add(label);
                if ((i>=1&&i <= 6)||(i>=25&&i<=30)||(i>=49&&i<=54))
                {
                    button.Location = new Point(110 + (i - initParkNum) / 3 * 80, 70 + (3 - j) * 80);
                    label.Location = new Point(124 + (i - initParkNum) / 3 * 80, 55 + (3 - j) * 80);
                }
                else
                {
                    button.Location = new Point(160 + (i - initParkNum) / 3 * 80, 70 + (3 - j) * 80);
                    label.Location = new Point(174 + (i - initParkNum) / 3 * 80, 55 + (3 - j) * 80);
                }
                j++;
                if (j == 3)
                {
                    j = 0;
                }
            }
        }
        private void btn_Click(object sender, EventArgs e)
 	    {
            // 通过sender截获当前是选中哪个座位
            Button button = (Button)sender;

            button.Image = Image.FromFile("car1.jpg");

            int i = int.Parse(button.Name.ToString().Replace("btnpark", ""));
            string placeid = i.ToString();
            if (i < 10)
            {
                placeid = "0" + placeid;
            }
            if (int.Parse(placeid) <= 6)
            {
                SecretUserPacking s = new SecretUserPacking();
                s.getplaceID = placeid;
                s.Show();
            } else{
                getCardID getcardid = new getCardID();
                getcardid.getplaceID = placeid;
                getcardid.Show();
            }
          



            //string type="secret";
            ////如果卡号是B开头,包括固定车位临时用户和公共车位普通用户
            //if (true)
            //{
            //    if (int.Parse(placeid) > 6)
            //    {
            //        type = "common";
            //    }
            //    //存进CommonPayRecord表
            //    DAL.CommonPayRecordDAL.addCommonPayRecord("B003", placeid, type);
            //    string CardID = DAL.CommonPayRecordDAL.queryCardIDByPlaceID(placeid);
            //    ToolTip tooltip = new ToolTip();
            //    tooltip.AutoPopDelay = 5000;
            //    tooltip.InitialDelay = 1000;
            //    tooltip.ReshowDelay = 500;
            //    // Force the ToolTip text to be displayed whether or not the form is active.
            //    tooltip.ShowAlways = true;
            //    // Set up the ToolTip text for the Button and Checkbox.
            //    tooltip.SetToolTip((Button)(this.Controls.Find("button" + (i).ToString(), true)[0]), "车位号：" + placeid + "\n卡号：" + CardID);
            //}
            //if (MessageBox.Show("请确认车位" + placeid, "", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            //{
            //    return;//返回
            //}

            ////固定车位处理程序,
            //if (int.Parse(placeid) <= 6)
            //{
                
            //}

            ////公共车位处理程序
            //if(int.Parse(placeid)>6)
            //{
            //    //存储记录到CommonPayRecord表中
            //    DAL.CommonMessDAL.change(placeid, "packed");
            // }
            
            button.Click -= new System.EventHandler(this.btn_Click);
            button.Click += new System.EventHandler(this.btn1_Click);



        }
        private void btn1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("此车位已被占用");
        }
        
private void 车位类型管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            C = 1;

            租用信息1 b = new 租用信息1();
            b.StartPosition = FormStartPosition.CenterScreen;
            b.Show();
            //bs.DataSource = DAL.PackingPlaceDAL.PackingPlaceListByCondition(null);
            //dataGridView1.DataSource = bs;
            //for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            //{
            //    if (i == 0 || i == 1 || i == 8)
            //        comboBox1.Items.Add(this.dataGridView1.Columns[i].HeaderText);
            //}

        }
        private void 用户信息管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            C = 2;
            ////List作为BindingSource的数据源
            //bs.DataSource = DAL.SecretUserDAL.SecretUListByCondition(null);
            //dataGridView1.DataSource = bs;
            //for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            //{
            //    if (i != 4 && i != 5)
            //        comboBox1.Items.Add(this.dataGridView1.Columns[i].HeaderText);
            //}
        }
        private void c车位信息管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            C = 3;
            bs.DataSource = DAL.SecretPlaceDAL.SecretPListByCondition(null);
            //dataGridView1.DataSource = bs;
            //for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            //{
            //    comboBox1.Items.Add(this.dataGridView1.Columns[i].HeaderText);
            //}
        }
        private void 可租用信息管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            C = 4;
            //bs.DataSource = DAL.RentingTimeDAL.RentingTimeListByCondition(null);
            //dataGridView1.DataSource = bs;
            //comboBox1.Items.Add(this.dataGridView1.Columns[0].HeaderText);
        }

        private void 车位信息管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //bs.DataSource = DAL.CommonMessDAL.QueryListByCondition();
            //dataGridView1.DataSource = bs;
            ////宽度自适应
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

      
        private void 公共车位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComCommonPark commonmess = new ComCommonPark();
            commonmess.StartPosition = FormStartPosition.CenterScreen;
            commonmess.ShowDialog();
        }

        private void 固定用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            C = 8;
            SecretUserPacking s = new SecretUserPacking();
            s.Show();
        }

        public static DateTime PreOutTime;
        private void 临时用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            C = 9;
            CasualUserPacking c = new CasualUserPacking();
            c.ShowDialog();
        }
        private void 系统管理员管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int a = DAL.AdminDAL.CheckUser();//判断登录者身份，是否为超级管理员
            if (a == 1)//登录者是超级管理员
            {
                C = 10;
                Administrator b = new Administrator();
                b.StartPosition = FormStartPosition.CenterScreen;
                b.Show();
            }
            else//登录者为系统管理员
                MessageBox.Show("您没有权限！");
        }

       

        private void 车库管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        

      
        //新增
        //private void button3_Click(object sender, EventArgs e)
        //{
        //    switch (C)
        //    {
        //        case 0:
        //            MessageBox.Show("请添加对象");
        //            break;
        //        case 1:
        //            Model.PackingPlace model1 = new Model.PackingPlace();
        //            model1.PlaceID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        //            model1.Type = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        //            model1.HighLocator = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        //            model1.HStrength = Convert.ToInt32(dataGridView1.CurrentRow.Cells[3].Value.ToString());
        //            model1.MediumLocator = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        //            model1.MStrength = Convert.ToInt32(dataGridView1.CurrentRow.Cells[5].Value.ToString());
        //            model1.LowLocator = dataGridView1.CurrentRow.Cells[6].Value.ToString();
        //            model1.LStrength = Convert.ToInt32(dataGridView1.CurrentRow.Cells[7].Value.ToString());
        //            model1.PackingCondition = dataGridView1.CurrentRow.Cells[8].Value.ToString();
        //            PackingPlaceDAL.ADDPackingP(model1);
        //            break;
        //            break;

        //        case 2://添加固定用户
        //            Model.SecretUser model2 = new Model.SecretUser();
        //            model2.ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
        //            model2.Name = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        //            model2.Gender = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        //            model2.CardID = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        //            model2.Phone = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        //            model2.BankCard = dataGridView1.CurrentRow.Cells[5].Value.ToString();
        //            SecretUserDAL.ADDSecretU(model2);
        //            break;

        //        case 3://添加固定车位
        //            Model.SecretPlace model3 = new Model.SecretPlace();
        //            model3.PlaceID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        //            model3.CardID = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        //            model3.IsAllowingRenting = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        //            SecretPlaceDAL.ADDSecretP(model3);
        //            break;
        //        case 4:
        //            Model.RentingTime model4 = new Model.RentingTime();
        //            model4.PlaceID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        //            model4.StartTime1 = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        //            model4.StopTime1 = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        //            model4.StartTime2 = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        //            model4.StopTime2 = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        //            model4.StartTime3 = dataGridView1.CurrentRow.Cells[5].Value.ToString();
        //            model4.StopTime3 = dataGridView1.CurrentRow.Cells[6].Value.ToString();
        //            RentingTimeDAL.ADDRentingT(model4);
        //            break;
        //        case 5:
        //            break;
        //        case 6:
        //            break;
        //        case 7:
        //            break;
        //        case 8:
        //            break;
        //        case 9:
        //            break;
        //        case 10://添加系统管理员
        //            AddAdmin a = new AddAdmin();
        //            a.StartPosition = FormStartPosition.CenterScreen;
        //            a.Show();
        //            break;


        //    }

        //}

       
        ////删除
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    switch(C)
        //    {
        //        case 0:
        //            MessageBox.Show("请选择删除对象");
        //            break;
        //        case 1:
        //             string ID_1 = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        //            int a_1 = DAL.PackingPlaceDAL.DelById(ID_1);
        //            if (a_1 == 1)
        //            {
        //                MessageBox.Show("删除成功");
        //                bs.DataSource = DAL.PackingPlaceDAL.PackingPlaceListByCondition(null);
        //                dataGridView1.DataSource = bs;

        //            }
        //            else
        //            {
        //                MessageBox.Show("删除失败");
        //                bs.DataSource = DAL.PackingPlaceDAL.PackingPlaceListByCondition(null);
        //                dataGridView1.DataSource = bs;

        //            }
        //            break;
               
        //        case 2:
        //            string ID_2 = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        //            int a_2 = DAL.SecretUserDAL.DelById(ID_2);
        //            if (a_2 == 1)
        //            {
        //                MessageBox.Show("删除成功");
        //                bs.DataSource = DAL.SecretUserDAL.SecretUListByCondition(null);
        //                dataGridView1.DataSource = bs;

        //            }
        //            else
        //            {
        //                MessageBox.Show("删除失败");
        //                bs.DataSource = DAL.SecretUserDAL.SecretUListByCondition(null);
        //                dataGridView1.DataSource = bs;

        //            }
        //            break;

        //        case 3:
        //             string ID_3 = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        //             int a_3 = DAL.SecretPlaceDAL.DelById(ID_3);
        //             if (a_3 == 1)
        //             {
        //                 MessageBox.Show("删除成功");
        //                 bs.DataSource = DAL.SecretPlaceDAL.SecretPListByCondition(null);
        //                 dataGridView1.DataSource = bs;

        //             }
        //             else
        //             {
        //                 MessageBox.Show("删除失败");
        //                 bs.DataSource = DAL.SecretPlaceDAL.SecretPListByCondition(null);
        //                 dataGridView1.DataSource = bs;
        //             }
        //            break;

        //        case 4:
        //            string ID_4 = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        //            int a_4 = DAL.RentingTimeDAL.DelById(ID_4);
        //             if (a_4 == 1)
        //             {
        //                 MessageBox.Show("删除成功");
        //                 bs.DataSource = DAL.RentingTimeDAL.RentingTimeListByCondition(null);
        //                 dataGridView1.DataSource = bs;

        //             }
        //             else
        //             {
        //                 MessageBox.Show("删除失败");
        //                 bs.DataSource = DAL.RentingTimeDAL.RentingTimeListByCondition(null);
        //                 dataGridView1.DataSource = bs;
        //             }
        //            break;

        //        case 5:
        //            break;

        //        case 6:
        //            break;

        //        case 7:
        //            break;

        //        case 8:
        //            break;

        //        case 9:
        //            break;

        //        case 10:
        //            string ID_10 = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        //            int a_10 = DAL.AdminDAL.DelById(ID_10);
        //            if (a_10 == 1)
        //            {
        //                MessageBox.Show("删除成功");
        //                bs.DataSource = DAL.AdminDAL.AdminListByCondition(null);
        //                dataGridView1.DataSource = bs;
        //                dataGridView1.Columns[2].Visible = false;//隐藏密码
        //            }
        //            else
        //            {
        //                MessageBox.Show("删除失败");
        //                bs.DataSource = DAL.AdminDAL.AdminListByCondition(null);
        //                dataGridView1.DataSource = bs;
        //                dataGridView1.Columns[2].Visible = false;//隐藏密码
        //            }
        //            break;
        //        default:
        //            MessageBox.Show("出现错误");
        //            break;
        //    }
           
                
 
            
        //}

        //private void button4_Click(object sender, EventArgs e)
        //{
        //    switch (C)
        //    {
        //        case 0:
        //            MessageBox.Show("请选择修改对象");
        //            break;
        //        case 1:
        //            Model.PackingPlace model_1 = new Model.PackingPlace();
        //            string a_1 = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        //            string b_1 = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        //            string c_1 = dataGridView1.CurrentRow.Cells[5].Value.ToString();
        //            string d_1 = dataGridView1.CurrentRow.Cells[7].Value.ToString();
        //            model_1.PlaceID = a_1;
        //            model_1.Type = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    
        //            model_1.HighLocator = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        //            model_1.HStrength = int.Parse(b_1);
        //            model_1.MediumLocator = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        //            model_1.MStrength = int.Parse(c_1);
        //            model_1.LowLocator = dataGridView1.CurrentRow.Cells[6].Value.ToString();
        //            model_1.LStrength = int.Parse(d_1);
        //            model_1.PackingCondition = dataGridView1.CurrentRow.Cells[8].Value.ToString();
        //            DAL.PackingPlaceDAL.Modify(model_1);
                    
                    
        //            MessageBox.Show("修改成功");
        //            bs.DataSource = DAL.PackingPlaceDAL.PackingPlaceListByCondition(null);
        //            dataGridView1.DataSource = bs;
                        
        //            break;

        //        case 2:
        //            Model.SecretUser model_2 = new Model.SecretUser();
        //            string a_2 = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    
        //            model_2.ID = int.Parse(a_2);
        //            model_2.Name = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        //            model_2.Gender = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        //            model_2.CardID = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        //            model_2.Phone = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        //            model_2.BankCard = dataGridView1.CurrentRow.Cells[5].Value.ToString();
        //            DAL.SecretUserDAL.Modify(model_2);
                    
                    
        //            MessageBox.Show("修改成功");
        //            bs.DataSource = DAL.SecretUserDAL.SecretUListByCondition(null);
        //            dataGridView1.DataSource = bs;
                        
        //            break;

        //        case 3:
        //            Model.SecretPlace model_3 = new Model.SecretPlace();
                   
        //            model_3.PlaceID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        //            model_3.CardID = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        //            model_3.IsAllowingRenting = dataGridView1.CurrentRow.Cells[2].Value.ToString();

        //            DAL.SecretPlaceDAL.Modify(model_3);
                    
                    
        //            MessageBox.Show("修改成功");
        //            bs.DataSource = DAL.SecretPlaceDAL.SecretPListByCondition(null);
        //            dataGridView1.DataSource = bs;
        //            break;

        //        case 4:
        //            Model.RentingTime model_4 = new Model.RentingTime();
        //            string t1 = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        //            string p1 = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        //            string t2 = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        //            string p2 = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        //            string t3 = dataGridView1.CurrentRow.Cells[5].Value.ToString();
        //            string p3 = dataGridView1.CurrentRow.Cells[6].Value.ToString();
        //            model_4.PlaceID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        //            model_4.StartTime1 = t1;
        //            model_4.StopTime1 = p1;
        //            model_4.StartTime2 = t2;
        //            model_4.StopTime2 = p2;
        //            model_4.StartTime3 = t3;
        //            model_4.StopTime3 = p3;
        //            DAL.RentingTimeDAL.Modify(model_4);
                    
                    
        //            MessageBox.Show("修改成功");
        //            bs.DataSource = DAL.RentingTimeDAL.RentingTimeListByCondition(null);
        //            dataGridView1.DataSource = bs;
        //            break;

        //        case 5:
        //            break;

        //        case 6:
        //            break;

        //        case 7:
        //            break;

        //        case 8:
        //            break;

        //        case 9:
        //            break;

        //        case 10:
                    
        //            Model.Administrator model_10 = new Model.Administrator();
        //            string a_10 = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        //            model_10.ID = int.Parse(a_10);
        //            model_10.Name = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        //            model_10.Password = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        //            model_10.Phone = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        //            model_10.Right1 = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        //            DAL.AdminDAL.Modify(model_10);
                    
                    
        //                //MessageBox.Show("删除成功");
        //                bs.DataSource = DAL.AdminDAL.AdminListByCondition(null);
        //                dataGridView1.DataSource = bs;
        //                dataGridView1.Columns[2].Visible = false;//隐藏密码
                   
        //            break;
        //        default:
        //            MessageBox.Show("出现错误");
        //            break;
        //    }

        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    SqlParameter[] paras = new SqlParameter[1];
        //    switch (C)
        //    {
        //        case 0:
        //            MessageBox.Show("请选择查找对象");
        //            break;
        //        case 1:
        //            if (comboBox1.Text == "PlaceID")//按PlaceID查找
        //            {
        //                paras[0] = new SqlParameter("@PlaceID", Convert.ToInt32(textBox1.Text));
        //                bs.DataSource = PackingPlaceDAL.PackingPlaceListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
        //            }
        //            else if (comboBox1.Text == "Type")//按Type查找
        //            {
        //                paras[0] = new SqlParameter("@Type", textBox1.Text);
        //                bs.DataSource = PackingPlaceDAL.PackingPlaceListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
        //            }
        //            else if (comboBox1.Text == "PackingCondition")//按Name查找
        //            {
        //                paras[0] = new SqlParameter("@PackingCondition", textBox1.Text);
        //                bs.DataSource = PackingPlaceDAL.PackingPlaceListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
        //            }
        //            break;
        //        case 2://对固定用户进行查询
        //            if (comboBox1.Text == "ID")//按ID查找
        //            {
        //                paras[0] = new SqlParameter("@ID", Convert.ToInt32(textBox1.Text));
        //                bs.DataSource = SecretUserDAL.SecretUListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
        //            }
        //            else if (comboBox1.Text == "Name")//按Name查找
        //            {
        //                paras[0] = new SqlParameter("@Name", textBox1.Text);
        //                bs.DataSource = SecretUserDAL.SecretUListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
        //            }
        //            else if (comboBox1.Text == "Gender")//按Name查找
        //            {
        //                paras[0] = new SqlParameter("@Gender", textBox1.Text);
        //                bs.DataSource = SecretUserDAL.SecretUListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
        //            }
        //            else if (comboBox1.Text == "CardID")//按Name查找
        //            {
        //                paras[0] = new SqlParameter("@CardID", textBox1.Text);
        //                bs.DataSource = SecretUserDAL.SecretUListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
        //            }
        //            break;
        //        case 3:
        //            if (comboBox1.Text == "PlaceID")//按PlaceID查找
        //            {
        //                paras[0] = new SqlParameter("@PlaceID", textBox1.Text);
        //                bs.DataSource = SecretPlaceDAL.SecretPListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
        //            }
        //            else if (comboBox1.Text == "CardID")//按CardID查找
        //            {
        //                paras[0] = new SqlParameter("@CardID", textBox1.Text);
        //                bs.DataSource = SecretPlaceDAL.SecretPListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
        //            }
        //            else if (comboBox1.Text == "IsAllowingRenting")//按IsAllowingRenting查找
        //            {
        //                paras[0] = new SqlParameter("@IsAllowingRenting", textBox1.Text);
        //                bs.DataSource = SecretPlaceDAL.SecretPListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
        //            }
        //            break;
        //        case 4:
        //            if (comboBox1.Text == "PlaceID")//按PlaceID查找
        //            {
        //                paras[0] = new SqlParameter("@PlaceID", textBox1.Text);
        //                bs.DataSource = RentingTimeDAL.RentingTimeListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
        //            }
        //            break;
        //        case 5:
        //            break;
        //        case 6:
        //            break;
        //        case 7:
        //            break;
        //        case 8:
        //            break;
        //        case 9:
        //            break;
        //        case 10://对系统管理员进行查询
        //            if (comboBox1.Text == "ID")//按ID查找
        //            {
        //                paras[0] = new SqlParameter("@ID", Convert.ToInt32(textBox1.Text));
        //                bs.DataSource = AdminDAL.AdminListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
        //            }
        //            else if (comboBox1.Text == "Name")//按Name查找
        //            {
        //                paras[0] = new SqlParameter("@Name", textBox1.Text);
        //                bs.DataSource = AdminDAL.AdminListByCondition(paras);//调用查询函数，并重新绑定datagridview的数据源
        //            }
        //            break;
        //       }

        //    }

      
        //依次为公共车位普通用户每小时收费、固定车位普通用户每年收费、固定车位临时用户每小时收费
        public static int[] allCharges = new int[3] { 3, 200, 4 };
        private void 收费标准ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Charges charges = new Charges();
            charges.StartPosition = FormStartPosition.CenterScreen;
            charges.ShowDialog();
        }


        private void 收费ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommonCharge commoncharge = new CommonCharge();
            commoncharge.StartPosition = FormStartPosition.CenterScreen;
            commoncharge.getAllCharge = allCharges;
            commoncharge.GetOutTime = PreOutTime;
            commoncharge.ShowDialog();
        }

       



     


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            CasualUserPacking casual = new CasualUserPacking();
            casual.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //foreach (Control ct in this.Controls)
            //{
            //    if (ct is Button)
            //    {
            //        if (ct.Name.StartsWith("button"))
            //        {
            //            this.Controls.Remove(ct);
            //        }
            //    }
            //}

           // initpark();
        }

        private void 固定车位管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SecretUser c = new SecretUser();
            c.StartPosition = FormStartPosition.CenterScreen;
            c.Show();
        }

        private void 年费用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = CommonPayRecordDAL.QuerySecretPay().Tables[0];
        }

        private void 临时用户ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = CommonPayRecordDAL.QueryCommonPay().Tables[0];
        }

        private void 年费用户收入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = CommonPayRecordDAL.QueryPay().Tables[0];
        }

        private void 每天收入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = CommonPayRecordDAL.Report("day");
        }

        private void 每月收入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = CommonPayRecordDAL.Report("month");
        }

        private void 每年收入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = CommonPayRecordDAL.Report("year");
        }

        private void 每天使用率ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = CommonPayRecordDAL.analyse("day").Tables[0];
        }

        private void 每月使用率ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = CommonPayRecordDAL.analyse("month").Tables[0];
        }

        private void 每年使用率ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = CommonPayRecordDAL.analyse("year").Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
            connect2.disconnect1(null);
        }

        private void 切换账号ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Application.Restart();
            connect2.disconnect1(null);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {       

            string floor = comboBox1.Text.Trim();
            if (floor == "一楼")
            {
                initpark(1);
            }
            else if (floor == "二楼")
            {
                initpark(25);
            }
            else if (floor == "三楼")
            {
                initpark(49);
            }
            else
            {
                
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string floor = comboBox1.Text.Trim();
            if (floor == "一楼")
            {
                initpark(1);
            }
            else if (floor == "二楼")
            {
                initpark(25);
            }
            else if (floor == "三楼")
            {
                initpark(49);
            }
            else
            {

            }
        }

        
    }
    
}
