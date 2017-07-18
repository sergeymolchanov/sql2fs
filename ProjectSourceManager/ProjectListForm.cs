using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using sql2fsbase.Adapters;
using sql2fsbase.Exceptions;
using System.IO;
using sql2fsbase;
using ProjectSourceManager.Adapters.Impl.DBContent;

namespace ProjectSourceManager
{
    public partial class ProjectListForm : Form
    {
        private static ProjectListForm _instance;
        private ProjectDirectory dir;
        private AdapterManager manager { get { return BuildAdapterManager(dir); } }

        public ProjectListForm()
        {
            _instance = this;
            InitializeComponent();

            dir = new ProjectDirectory(Common.RootDir);
            dir.CheckGitHooks();
        }

        private void lbDir_DoubleClick(object sender, EventArgs e)
        {            
            ProjectEditor editForm = new ProjectEditor();
            editForm.ShowProject(dir);

            reloadControls();
        }

        private AdapterManager BuildAdapterManager(ProjectDirectory dir)
        {
            return new AdapterManager(dir);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DoThreadedAction(dir, ThreadedAction.Dump);

            reloadControls();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            DoThreadedAction(dir, ThreadedAction.Restore);

            reloadControls();
        }

        private static ProjectDirectory _project;
        private static ThreadedAction _threadAction;
        private static Exception _threadException = null;
        private static Thread _thread = null;

        enum ThreadedAction
        {
            Dump,
            Restore,
            Merge,
            Check
        }

        private void DoThreadedAction(ProjectDirectory project, ThreadedAction action)
        {
            _project = project;
            _threadAction = action;

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
                    if (_threadAction == ThreadedAction.Dump)
                    {
                        _project.Dump(cbForce.Checked);
                    }
                    else if (_threadAction == ThreadedAction.Restore)
                    {
                        _project.Restore(cbForce.Checked);
                    }
                    else if (_threadAction == ThreadedAction.Merge)
                    {
                        _project.Merge(false);
                    }
                    else if (_threadAction == ThreadedAction.Check)
                    {
                        _project.Merge(true);
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
                if (_threadAction == ThreadedAction.Dump)
                {
                    _project.Dump(cbForce.Checked);
                }
                else if (_threadAction == ThreadedAction.Restore)
                {
                    _project.Restore(cbForce.Checked);
                }
                else if (_threadAction == ThreadedAction.Merge)
                {
                    _project.Merge(false);
                }
                else if (_threadAction == ThreadedAction.Check)
                {
                    _project.Merge(true);
                }
            }

            needReloadControls = true;
        }

        private bool needReloadControls = false;

        private void btn_commit_Click(object sender, EventArgs e)
        {
            dir.Commit();
        }

        private void btn_push_Click(object sender, EventArgs e)
        {
            dir.Push();
        }

        private void btn_pull_Click(object sender, EventArgs e)
        {
            dir.Pull(true);
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            dir.MergeWindow();
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            dir.Log();
        }

        private bool onTimer = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (onTimer) return;
            onTimer = true;
            bool isThreadRunning = _thread != null && _thread.ThreadState == ThreadState.Running;
            btnDump.Enabled = cbExpert.Checked && !isThreadRunning;
            btnRestore.Enabled = cbExpert.Checked && !isThreadRunning;

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
            reloadControls();
        }

        private void lbDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            reloadControls();
        }

        private void ProjectListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void reloadControls()
        {
            cbForce.Enabled = cbExpert.Checked;
            if (!cbForce.Enabled && cbForce.Checked)
                cbForce.Checked = false;
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            dir.Switch();
        }

        private void ProjectListForm_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void ProjectListForm_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cbExpert_CheckedChanged(object sender, EventArgs e)
        {
            reloadControls();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DoThreadedAction(dir, ThreadedAction.Merge);

            reloadControls();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            DoThreadedAction(dir, ThreadedAction.Check);

            reloadControls();
        }

        private void btn_config_Click(object sender, EventArgs e)
        {
            new ProjectEditor().ShowProject(dir);
        }
    }
}
