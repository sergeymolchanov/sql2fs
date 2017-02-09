using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectSourceManager
{
    public partial class SendForm : Form
    {
        public SendForm()
        {
            InitializeComponent();
        }

        private void SendForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = Common.RootDir.FullName + @"\out.rar";
            textBox2.Text = "192.168.43.34";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(textBox1.Text))
                return;

            UdpConnect.SendFile(textBox1.Text, textBox2.Text, Convert.ToInt32(textBox3.Text));
        }
    }
}
