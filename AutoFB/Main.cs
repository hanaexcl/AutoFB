using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoFB
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FBClient web = new FBClient();
            web.loginFB(this.textBox1.Text , this.textBox2.Text);
            if (web.isLogin)
            {
                MessageBox.Show(web.userName);
            } else {
                MessageBox.Show("登入失敗");
            }

            web.Dispose();

        }
    }
}
