using sql2fsbase.Adapters.Impl.DBContent;
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
    public partial class TableRowComparer : Form, ITableRowComparer
    {
        public TableRowComparer()
        {
            InitializeComponent();
        }

        private UserAction _retval = UserAction.Cancel;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void TableRowComparer_Load(object sender, EventArgs e)
        {
        }

        public UserAction CheckRow(String[] fields, String[] before, String[] after, String state)
        {
            lAction.Text = state;

            for (int i = 0; i < fields.Length; i++)
            {
                dataGridView1.Rows.Add(new string[] { fields[i], before != null ? before[i] : "", after != null ? after[i] : "" });
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (!dataGridView1.Rows[i].Cells[1].Value.Equals(dataGridView1.Rows[i].Cells[2].Value))
                {
                    dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.LightSalmon;
                    dataGridView1.Rows[i].Cells[2].Style.BackColor = Color.LightSalmon;
                }
            }

            ShowDialog();

            return _retval;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            _retval = UserAction.Replace;
            Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            _retval = UserAction.Skip;
            Close();
        }

        private void btn_cancel_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSkipAll_Click(object sender, EventArgs e)
        {
            _retval = UserAction.SkipAll;
            Close();
        }

        private void btn_replace_all_Click(object sender, EventArgs e)
        {
            _retval = UserAction.ReplaceAll;
            Close();
        }
    }
}
