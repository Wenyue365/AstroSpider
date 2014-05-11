using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using Newtonsoft.Json;
using System.IO;

namespace AstroSpider
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SinaBirthdayBook sbb = new SinaBirthdayBook();
            sbb.Go();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            SinaAstroBook sab = new SinaAstroBook();
            sab.Go();
        }

        private void txbUrlEncode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txbUrlDecode_TextChanged(object sender, EventArgs e)
        {

        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (txbUrlEncode.Text != "")
            {
                txbUrlEncode.Text = HttpUtility.UrlEncode(txbUrlEncode.Text, Encoding.GetEncoding("GB2312"));
            }
            if (txbUrlDecode.Text != "")
            {
                txbUrlDecode.Text = HttpUtility.UrlDecode(txbUrlDecode.Text, Encoding.GetEncoding("GB2312"));
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            SinaMonthdayBook smb = new SinaMonthdayBook();
            smb.Go();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (txbJSON.Text != "")
            {
                txbUrlDecode.Text = HttpUtility.UrlEncodeUnicode(txbJSON.Text);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txbJSON.Text = ZUtilities.ConvertUnicodeStringToChinese(txbJSON.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SinaHuangDaoBook hdd = new SinaHuangDaoBook();
            hdd.Go();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            TXHuangDaoBook hdb = new TXHuangDaoBook();
            hdb.Go();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            LaoHuangDaoBook lhlb = new LaoHuangDaoBook();
            lhlb.Go();
        }
    }
}
