﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using sql2fsbase.Adapters;
using sql2fsbase.Exceptions;
using System.IO;
using sql2fsbase;
using ProjectSourceManager.Adapters.Impl.DBContent;
using sql2fsbase.Adapters.Impl.DBContent;

namespace ProjectSourceManager
{
    public partial class ProjectListForm : Form
    {
        private static ProjectListForm _instance;

        public ProjectListForm()
        {
            _instance = this;
            InitializeComponent();
            ReloadProjects();
        }

        private void ReloadProjects()
        {
            List<ProjectDirectory> l = new List<ProjectDirectory>();

            foreach (var dir in Common.RootDir.GetDirectories())
            {
                if (dir.Name.ToLower() == "git")
                    continue;

                if (dir.Name.ToLower() == "bazaar")
                    continue;

                l.Add(new ProjectDirectory(dir));
            }

            l.Sort();

            lbDir.Items.Clear();
            foreach (var dir in l)
            {
                lbDir.Items.Add(dir);
                if (dir.Name == Common.GlobalSettings.Instance.LastProject)
                    lbDir.SelectedItem = dir;
            }

            reloadControls();
        }

        private void lbDir_DoubleClick(object sender, EventArgs e)
        {
            if (lbDir.Items.Count == 0) return;

            ProjectDirectory dir = (ProjectDirectory)lbDir.SelectedItem;

            ProjectEditor editForm = new ProjectEditor();
            editForm.ShowProject(dir);

            reloadControls();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProjectEditor editForm = new ProjectEditor();
            editForm.ShowProject(null);

            reloadControls();
        }

        private AdapterManager BuildAdapterManager(ProjectDirectory dir)
        {
            return new AdapterManager(dir);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lbDir.Items.Count == 0) return;

            ProjectDirectory dir = lbDir.SelectedItem as ProjectDirectory;

            if (lbDir.SelectedItem == null) return;

            AdapterManager manager = BuildAdapterManager(dir);

            DoThreadedAction(dir, true);

            reloadControls();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (lbDir.Items.Count == 0) return;

            ProjectDirectory dir = (ProjectDirectory)lbDir.SelectedItem;

            if (dir == null) return;

            DoThreadedAction(dir, false);

            reloadControls();
        }

        private static ProjectDirectory _project;
        private static bool _threadIsDump;
        private static Exception _threadException = null;
        private static Thread _thread = null;

        private void DoThreadedAction(ProjectDirectory project, bool isDump)
        {
            _project = project;
            _threadIsDump = isDump;

            if (cbOtherThread.Checked)
            {
                _thread = new Thread(DoAction);
                _thread.IsBackground = false;
                _thread.Start();
            }
            else
            {
                DoAction();
            }

            timer1_Tick(null, null);
            reloadControls();
        }

        private void DoAction()
        {
            if (cbOtherThread.Checked)
            {
                try
                {
                    if (_threadIsDump)
                    {
                        _project.Dump(cbForce.Checked);
                    }
                    else
                    {
                        _project.Restore(cbForce.Checked);
                    }
                }
                catch (Exception exception)
                {
                    _threadException = exception;
                }

                _thread = null;
            }
            else
            {
                if (_threadIsDump)
                {
                    _project.Dump(cbForce.Checked);
                }
                else
                {
                    _project.Restore(cbForce.Checked);
                }
            }

            needReloadControls = true;
        }

        private bool needReloadControls = false;

        private void btn_commit_Click(object sender, EventArgs e)
        {
            if (lbDir.Items.Count == 0) return;

            ProjectDirectory dir = (ProjectDirectory)lbDir.SelectedItem;

            if (dir == null) return;

            dir.Commit();
        }

        private void btn_push_Click(object sender, EventArgs e)
        {
            if (lbDir.Items.Count == 0) return;

            ProjectDirectory dir = (ProjectDirectory)lbDir.SelectedItem;

            if (dir == null) return;

            dir.Push();
        }

        private void btn_pull_Click(object sender, EventArgs e)
        {

            if (lbDir.Items.Count == 0) return;

            ProjectDirectory dir = (ProjectDirectory)lbDir.SelectedItem;

            if (dir == null) return;

            dir.Pull(true);
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            if (lbDir.Items.Count == 0) return;

            ProjectDirectory dir = (ProjectDirectory)lbDir.SelectedItem;

            if (dir == null) return;

            dir.Merge();
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            if (lbDir.Items.Count == 0) return;

            ProjectDirectory dir = (ProjectDirectory)lbDir.SelectedItem;

            if (dir == null) return;

            dir.Log();
        }

        private bool onTimer = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (onTimer) return;
            onTimer = true;
            bool isThreadRunning = _thread != null && _thread.ThreadState == ThreadState.Running;
            btnDump.Enabled = !isThreadRunning && (isRepoClosed || cbExpert.Checked);
            btnRestore.Enabled = !isThreadRunning && (!isRepoClosed || cbExpert.Checked);

            ProgressBarForm.Instance.Visible = isThreadRunning;

            if (_threadException != null)
            {
                if (_threadException is ObjectChangedException)
                {
                    MessageBox.Show(String.Format(@"Состояние сервера изменилось, необходимо заново выполнить дамп. Объект {0}", ((ObjectChangedException)_threadException).Item.Name));
                }
                else
                {
                    MessageBox.Show(_threadException.Message);
                }
                _threadException = null;
            }

            onTimer = false;

            if (needReloadControls)
            {
                needReloadControls = false;
                reloadControls();
            }
        }

        private void ProjectListForm_Load(object sender, EventArgs e)
        {
            tbName.Text = Common.GlobalSettings.Instance.UserName;
            tbEmail.Text = Common.GlobalSettings.Instance.Email;
            reloadControls();
        }

        private void lbDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            reloadControls();
        }

        private void ProjectListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            tbName_TextChanged(sender, null);

            if (lbDir.Items.Count > 0 && lbDir.SelectedItem != null)
                Common.GlobalSettings.Instance.LastProject = ((ProjectDirectory)lbDir.SelectedItem).Name;

            Common.GlobalSettings.Save();
        }

        private void reloadControls()
        {
            if (lbDir.Items.Count == 0) return;

            ProjectDirectory dir = (ProjectDirectory)lbDir.SelectedItem;

            if (dir == null) return;

            isRepoClosed = dir.IsRepoLocked;

            grRepoControls.Enabled = !isRepoClosed;
            lMode.Text = isRepoClosed?"Работа в БД":"Работа в репозитарии";

            cbForce.Enabled = cbExpert.Checked;
            if (!cbForce.Enabled && cbForce.Checked)
                cbForce.Checked = false;
        }

        private bool isRepoClosed = false;

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            Common.GlobalSettings.Instance.UserName = tbName.Text;
            Common.GlobalSettings.Instance.Email = tbEmail.Text;
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            if (lbDir.Items.Count == 0) return;

            ProjectDirectory dir = (ProjectDirectory)lbDir.SelectedItem;

            if (dir == null) return;

            dir.Switch();
        }

        private void ProjectListForm_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void ProjectListForm_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        }

        private void tbName_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void cbExpert_CheckedChanged(object sender, EventArgs e)
        {
            reloadControls();
        }
    }
}
