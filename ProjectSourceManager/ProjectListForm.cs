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

            dir = new ProjectDirectory(Common.RootDir, new Tools());
            dir.CheckGitHooks();
        }

        private void lbDir_DoubleClick(object sender, EventArgs e)
        {            
            ProjectEditor editForm = new ProjectEditor();
            editForm.ShowProject(dir);
        }

        private AdapterManager BuildAdapterManager(ProjectDirectory dir)
        {
            return new AdapterManager(dir);
        }

        private static ProjectDirectory _project;
        private static Exception _threadException = null;
        private static Thread _thread = null;

        private void DoThreadedAction(ProjectDirectory project)
        {
            _project = project;

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
        }

        private void DoAction()
        {
            if (cbOtherThread.Checked)
            {
                try
                {
                    _project.Merge();
                }
                catch (Exception exception)
                {
                    _threadException = exception;
                }

                _thread = null;
            }
            else
            {
                _project.Merge();
            }

        }

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

        private void ProjectListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_thread != null && _thread.ThreadState == ThreadState.Running)
                _thread.Abort();
        }

        private void btn_config_Click(object sender, EventArgs e)
        {
            new ProjectEditor().ShowProject(dir);
        }

        private void btnMergeProj_Click(object sender, EventArgs e)
        {
            DoThreadedAction(dir);
        }

        private void btnFetch_Click(object sender, EventArgs e)
        {
            dir.Fetch();
        }

        private bool onTimer = false;
        private void progressBarTimer_Tick(object sender, EventArgs e)
        {
            if (onTimer) return;
            onTimer = true;
            bool isThreadRunning = _thread != null && (_thread.ThreadState == ThreadState.Running || _thread.ThreadState == ThreadState.WaitSleepJoin);

            if (btnMergeProj.Enabled == isThreadRunning)
                btnMergeProj.Enabled = !isThreadRunning;
            
            if (_threadException != null)
            {
                if (_threadException is ObjectChangedException)
                {
                    String itemName = ((ObjectChangedException)_threadException).Item.Name;
                    LongOperationState.Timer1Text = "Ошибка";                    
                    LongOperationState.Timer2Text = itemName;
                    
                    MessageBox.Show(String.Format(@"Объект '{0}' изменился и в БД, и в репозитарии. Необходимо разрешить конфликт.", itemName));
                }
                else
                {
                    MessageBox.Show(_threadException.Message);
                }
                _threadException = null;
            }

            if (LongOperationState.Timer1Max >= LongOperationState.Timer1Pos || LongOperationState.Timer2Max >= LongOperationState.Timer2Pos)
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

            onTimer = false;
        }
    }
}
