using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DAL;

namespace PackingManagement
{
    public partial class AddSU : Form
    {
        public AddSU()
        {
            InitializeComponent();
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("dfsdf" + textBox1.Text + "fsdasfd");

            if (textBox1.Text == "" && comboBox3.Text == "" || textBox4.Text == "" || textBox2.Text == "" || textBox6.Text == "" || comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("信息不完整！");

            }
            else if (comboBox2.Text == "allow" && textBox8.Text == textBox9.Text)
            {

                MessageBox.Show("可租用信息不完整！");
            }
            else
            {
                string Name = textBox1.Text;
                string Gender = comboBox3.Text;
                string BankCard = textBox4.Text;
                string CardID = textBox2.Text;
                string Phone = textBox6.Text;
                string PlaceID = comboBox1.Text;
                string IsAllowingRenting = comboBox2.Text;
                DateTime ValidDate = Convert.ToDateTime(dateTimePicker1.Value);
                float StartTime1 = float.Parse(textBox8.Text);
                float StopTime1 = float.Parse(textBox9.Text);
                float StartTime2 = float.Parse(textBox7.Text);
                float StopTime2 = float.Parse(textBox3.Text);
                float StartTime3 = float.Parse(textBox11.Text);
                float StopTime3 = float.Parse(textBox10.Text);
                string RentingCondition = "unrented";
                string ValidCondition = "normal";

                SqlParameter[] paras = new SqlParameter[1];
                paras[0] = new SqlParameter("@CardID", CardID);
                if (SecretPlaceDAL.SecretPListByCondition(paras) != null)
                {
                    MessageBox.Show(CardID + "已被占用");
                }
                else
                {
                    Model.SecretUser model1 = new Model.SecretUser();
                    //model1.ID = ID;
                    model1.Name = Name;
                    model1.Gender = Gender;
                    model1.BankCard = BankCard;
                    model1.CardID = CardID;
                    model1.Phone = Phone;
                    int a = DAL.SecretUserDAL.ADDSecretU(model1);
                    if (a != 0)
                        MessageBox.Show("添加固定用户成功");
                    else MessageBox.Show("信息有误，请重新填写");

                    Model.SecretCard model2 = new Model.SecretCard();
                    model2.ID = CardID;
                    model2.PlaceID = PlaceID;
                    model2.ValidDate = ValidDate;
                    model2.RentingCondition = RentingCondition;
                    model2.ValidCondition = ValidCondition;
                    int b = DAL.SecretCardDAL.ADD(model2);
                    if (b != 0)
                        MessageBox.Show("添加固定用户停车卡成功");
                    else MessageBox.Show("信息有误，请重新填写");


                    Model.SecretPlace model3 = new Model.SecretPlace();
                    model3.PlaceID = PlaceID;
                    model3.CardID = CardID;
                    model3.IsAllowingRenting = IsAllowingRenting;
                    int c = DAL.SecretPlaceDAL.ADDSecretP(model3);
                    if (c != 0)
                        MessageBox.Show("添加固定用户停车位成功");
                    else MessageBox.Show("信息有误，请重新填写");

                    if (comboBox2.Text == "allow")
                    {
                        Model.RentingTime model4 = new Model.RentingTime();
                        model4.PlaceID = PlaceID;
                        model4.StartTime1 = StartTime1;
                        model4.StartTime2 = StartTime2;
                        model4.StartTime3 = StartTime3;
                        model4.StopTime1 = StopTime1;
                        model4.StopTime2 = StopTime2;
                        model4.StopTime3 = StopTime3;
                        int d = DAL.RentingTimeDAL.ADDRentingT(model4);
                        if (d != 0)
                            MessageBox.Show("添加可租用信息成功");
                        else MessageBox.Show("信息有误，请重新填写");

                    }
                    SecretUser.bs.DataSource = DAL.SecretUserDAL.SecretUListByCondition(null);
                    SecretUser.bs.ResetCurrentItem();
                    SecretUser.bs.ResetBindings(false);
                    this.Close();
                }
               


            }

        }

    }
}
