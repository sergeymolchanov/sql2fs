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
    public partial class ProjectEditor : Form
    {
        private ProjectDirectory Project;

        public ProjectEditor()
        {
            InitializeComponent();
        }

        private void ProjectEditor_Load(object sender, EventArgs e)
        {

        }

        public void ShowProject(ProjectDirectory project)
        {
            this.Project = project;

            ReloadData();
            ShowDialog();
        }

        private void ReloadData()
        {
            if (Project != null)
            {
                tbConnectionString.Text = Project.Settings.ConnectionString;
                tbReportURL.Text = Project.Settings.ReportServerURL;
                tbReportUsername.Text = Project.Settings.ReportServerUsername;
                tbReportPassword.Text = Project.Settings.ReportServerPassword;
                tbName.ReadOnly = true;
                tbName.Text = Project.Name;
                tbVorlagen.Text = Project.Settings.VorlagenDir;
                tbReportRoot.Text = Project.Settings.ReportRoot;
            }
            else
            {
                tbName.ReadOnly = false;
            }
        }

        private void Save()
        {
            if (Project == null)
            {
                if (tbName.Text.Length < 4)
                {
                    MessageBox.Show("Введите name");
                    return;
                }

                DirectoryInfo dir = new DirectoryInfo(Common.RootDir + @"\" + tbName.Text);
                if (dir.Exists)
                {
                    MessageBox.Show("Такой проект уже существует");
                    return;
                }
                dir.Create();
                Project = new ProjectDirectory(dir);
            }

            Project.Settings.ConnectionString = tbConnectionString.Text;
            Project.Settings.ReportServerURL = tbReportURL.Text;
            Project.Settings.ReportServerUsername = tbReportUsername.Text;
            Project.Settings.ReportServerPassword = tbReportPassword.Text;
            Project.Settings.VorlagenDir = tbVorlagen.Text;
            Project.Settings.ReportRoot = tbReportRoot.Text;

            Project.SaveSettings();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Save();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lLinkVorlagen_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo dir = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
                if (!dir.Exists)
                {
                    MessageBox.Show("Not found");
                }
                tbVorlagen.Text = dir.FullName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
            ReloadData();
        }
    }
}
