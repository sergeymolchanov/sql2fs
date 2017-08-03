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
    public partial class AskMergeStyleForm : Form
    {
        public AskMergeStyleForm()
        {
            InitializeComponent();
        }

        bool isSelected = false;
        Common.MergeStyle result = Common.MergeStyle.Normal;

        public static Common.MergeStyle Ask()
        {
            AskMergeStyleForm f = new AskMergeStyleForm();
            f.ShowDialog();

            if (!f.isSelected)
                throw new sql2fsbase.Adapters.AdapterManager.MergeException("Отмена пользователем");

            return f.result;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRepo2Db_Click(object sender, EventArgs e)
        {
            isSelected = true;
            result = Common.MergeStyle.Repo2Db;
            Close();
        }

        private void btnDb2Repo_Click(object sender, EventArgs e)
        {
            isSelected = true;
            result = Common.MergeStyle.Db2Repo;
            Close();
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            isSelected = true;
            result = Common.MergeStyle.Normal;
            Close();
        }
    }
}
