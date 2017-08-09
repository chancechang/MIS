using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackingManagement
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //登录界面
            UserLogin login = new UserLogin();
            login.StartPosition = FormStartPosition.CenterScreen;
            //Application.Run(new Form1());
            if (login.ShowDialog() == DialogResult.Yes)
            {
                Application.Run(new Form1());

            }
        }
    }
}
