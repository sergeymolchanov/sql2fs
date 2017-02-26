using sql2fsbase;
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
            if (LongOperationState.Timer1Max > LongOperationState.Timer1Pos || LongOperationState.Timer2Max > LongOperationState.Timer2Pos)
            {
                progressBar1.Maximum = LongOperationState.Timer1Max;
                progressBar1.Minimum = 0;
                progressBar1.Value = LongOperationState.Timer1Pos;
                label1.Text = LongOperationState.Timer1Text;

                progressBar2.Maximum = LongOperationState.Timer2Max;
                progressBar2.Minimum = 0;
                progressBar2.Value = LongOperationState.Timer2Pos;
                label2.Text = LongOperationState.Timer2Text;
            }
        }

        public void Clean()
        {
            LongOperationState.Timer1Max = 0;
            LongOperationState.Timer1Pos = 0;
            LongOperationState.Timer2Max = 0;
            LongOperationState.Timer2Pos = 0;
        }

        static ProgressBarForm()
        {
            Instance = new ProgressBarForm();
        }
    }
}
