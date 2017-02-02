using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectSourceManager.Adapters.Impl.DBContent
{
    public partial class SQLErrorView : Form
    {
        public SQLErrorView()
        {
            InitializeComponent();
        }

        private bool doCancel = false;
        public static bool ShowSQL(ProjectSourceManager.Adapters.Impl.AdapterBaseSQL.AdapterSqlException q)
        {
            SQLErrorView f = new SQLErrorView();
            f.textBox1.Text = q.Sql;
            f.textBox2.Text = q.Message;
            f.textBox3.Text = q.InnerException.StackTrace;

            f.ShowDialog();
            return f.doCancel;
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            doCancel = true;
            Close();
        }
    }
}
