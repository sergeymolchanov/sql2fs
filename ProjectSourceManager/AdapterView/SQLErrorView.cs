using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectSourceManager.Adapters.Impl;
using sql2fsbase.Adapters.Impl;

namespace ProjectSourceManager.Adapters.Impl.DBContent
{
    public partial class SQLErrorView : Form
    {
        public SQLErrorView()
        {
            InitializeComponent();
        }

        private bool doCancel = false;
        public bool ShowSQL(List<AdapterBaseSQL.AdapterSqlException> queryList)
        {
            AdapterBaseSQL.AdapterSqlException q = queryList[0];
            textBox1.Text = q.Sql;
            textBox2.Text = q.Message;
            textBox3.Text = q.InnerException.StackTrace;

            ShowDialog();
            return doCancel;
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
