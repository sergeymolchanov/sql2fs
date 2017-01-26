using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectSourceManager
{
    public partial class ProgressBarForm : Form
    {
        public int Timer1Max { get; set; }
        public int Timer1Pos { get; set; }
        public String Timer1Text { get; set; }

        public int Timer2Max { get; set; }
        public int Timer2Pos { get; set; }
        public String Timer2Text { get; set; }

        public static ProgressBarForm Instance { get; private set; }

        public ProgressBarForm()
        {
            InitializeComponent();
        }

        private void ProgressBarForm_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Timer1Max > Timer1Pos || Timer2Max > Timer2Pos)
            {
                progressBar1.Maximum = Timer1Max;
                progressBar1.Minimum = 0;
                progressBar1.Value = Timer1Pos;
                label1.Text = Timer1Text;

                progressBar2.Maximum = Timer2Max;
                progressBar2.Minimum = 0;
                progressBar2.Value = Timer2Pos;
                label2.Text = Timer2Text;
            }
        }

        public void Clean()
        {
            Instance.Timer1Max = 0;
            Instance.Timer1Pos = 0;
            Instance.Timer2Max = 0;
            Instance.Timer2Pos = 0;
        }

        static ProgressBarForm()
        {
            Instance = new ProgressBarForm();
        }
    }
}
