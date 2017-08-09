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

namespace PackingManagement
{
    public partial class UserLogin : Form
    {
        public UserLogin()
        {
            InitializeComponent();
        }

        //textBox1键盘点击enter键，切换到textBox2
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)     // 判断 按键的事件, 13 表示按下了 回车键
            {
                textBox2.Focus();              // 直接让下一个文本框获取焦点
            }
        }


        //textBox2键盘点击enter键，切换到点击登录
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)     // 判断 按键的事件, 13 表示按下了 回车键
            {
                button1_Click(sender, e);              // 触发点击“登录事件”
            }
        }

        //点击“登录”按钮
        private void button1_Click(object sender, EventArgs e)
        {
            //当用户名或密码为空时，弹出提示
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty)
                MessageBox.Show("请输入用户名或密码", "消息");
            else
            {
                String name = textBox1.Text;
                String pwd = textBox2.Text;
                //调用webservice
                int res = DAL.AdminDAL.Login(name, pwd);
                if (res == 0)
                {
                    DialogResult = DialogResult.Yes;//通过DialogResult传递参数
                    this.Close();
                }
                else if (res == 1)
                {
                    MessageBox.Show("密码输入错误", "登录失败");
                }

                else if (res == 2)
                {
                    MessageBox.Show("用户名不存在", "登录失败");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();//退出应用程序
        }

     


    }
}
