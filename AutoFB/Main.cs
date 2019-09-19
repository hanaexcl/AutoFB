using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoFB {
    public partial class Main : Form {
        FBClient web;

        public Main() {
            InitializeComponent();

            web = new FBClient();
        }

        private void Button1_Click(object sender, EventArgs e) {
            web.ReadCookies();
            if (web.checkLogin()) {
                if (MessageBox.Show("目前已入帳號：" + web.userName + "\n是否登入其他帳號", "訊息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    web.loginOut();
                } else {
                    return;
                }
            }

            web.loginFB(this.textBox1.Text, this.textBox2.Text);
            if (web.isLogin) {
                web.SaveCookies();
                MessageBox.Show(web.userName);
                this.button1.Enabled = false;
            } else {
                MessageBox.Show("登入失敗");
                web.Dispose();
            }
        }

        private void Button2_Click(object sender, EventArgs e) {
            web.SaveCookies();
        }

        private void Button3_Click(object sender, EventArgs e) {
            web.ReadCookies();
        }

        private void Button4_Click(object sender, EventArgs e) {
            web.loginOut();
        }

        private void Button5_Click(object sender, EventArgs e) {
            if (web.checkLogin()) {
                MessageBox.Show(web.userName);
            } else {
                MessageBox.Show("登入失敗");
                web.Dispose();
            }
        }
    }
}
