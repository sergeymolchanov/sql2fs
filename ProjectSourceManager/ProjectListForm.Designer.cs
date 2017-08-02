namespace ProjectSourceManager
{
    partial class ProjectListForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btn_commit = new System.Windows.Forms.Button();
            this.btn_push = new System.Windows.Forms.Button();
            this.btn_pull = new System.Windows.Forms.Button();
            this.btnLog = new System.Windows.Forms.Button();
            this.btnMerge = new System.Windows.Forms.Button();
            this.cbOtherThread = new System.Windows.Forms.CheckBox();
            this.grRepoControls = new System.Windows.Forms.GroupBox();
            this.btnFetch = new System.Windows.Forms.Button();
            this.btnMergeProj = new System.Windows.Forms.Button();
            this.btn_config = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBarTimer = new System.Windows.Forms.Timer(this.components);
            this.grRepoControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_commit
            // 
            this.btn_commit.Location = new System.Drawing.Point(6, 19);
            this.btn_commit.Name = "btn_commit";
            this.btn_commit.Size = new System.Drawing.Size(136, 23);
            this.btn_commit.TabIndex = 4;
            this.btn_commit.Text = "Commit";
            this.btn_commit.UseVisualStyleBackColor = true;
            this.btn_commit.Click += new System.EventHandler(this.btn_commit_Click);
            // 
            // btn_push
            // 
            this.btn_push.Location = new System.Drawing.Point(148, 48);
            this.btn_push.Name = "btn_push";
            this.btn_push.Size = new System.Drawing.Size(136, 23);
            this.btn_push.TabIndex = 5;
            this.btn_push.Text = "Push";
            this.btn_push.UseVisualStyleBackColor = true;
            this.btn_push.Click += new System.EventHandler(this.btn_push_Click);
            // 
            // btn_pull
            // 
            this.btn_pull.Location = new System.Drawing.Point(148, 19);
            this.btn_pull.Name = "btn_pull";
            this.btn_pull.Size = new System.Drawing.Size(136, 23);
            this.btn_pull.TabIndex = 6;
            this.btn_pull.Text = "Pull";
            this.btn_pull.UseVisualStyleBackColor = true;
            this.btn_pull.Click += new System.EventHandler(this.btn_pull_Click);
            // 
            // btnLog
            // 
            this.btnLog.Location = new System.Drawing.Point(6, 48);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(136, 23);
            this.btnLog.TabIndex = 7;
            this.btnLog.Text = "Log";
            this.btnLog.UseVisualStyleBackColor = true;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(290, 48);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(136, 23);
            this.btnMerge.TabIndex = 8;
            this.btnMerge.Text = "Merge";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // cbOtherThread
            // 
            this.cbOtherThread.AutoSize = true;
            this.cbOtherThread.Checked = true;
            this.cbOtherThread.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOtherThread.Location = new System.Drawing.Point(17, 208);
            this.cbOtherThread.Name = "cbOtherThread";
            this.cbOtherThread.Size = new System.Drawing.Size(86, 17);
            this.cbOtherThread.TabIndex = 10;
            this.cbOtherThread.Text = "OtherThread";
            this.cbOtherThread.UseVisualStyleBackColor = true;
            // 
            // grRepoControls
            // 
            this.grRepoControls.Controls.Add(this.btnFetch);
            this.grRepoControls.Controls.Add(this.btn_commit);
            this.grRepoControls.Controls.Add(this.btn_push);
            this.grRepoControls.Controls.Add(this.btn_pull);
            this.grRepoControls.Controls.Add(this.btnMerge);
            this.grRepoControls.Controls.Add(this.btnLog);
            this.grRepoControls.Location = new System.Drawing.Point(17, 41);
            this.grRepoControls.Name = "grRepoControls";
            this.grRepoControls.Size = new System.Drawing.Size(451, 87);
            this.grRepoControls.TabIndex = 15;
            this.grRepoControls.TabStop = false;
            this.grRepoControls.Text = "Репозитарий";
            // 
            // btnFetch
            // 
            this.btnFetch.Location = new System.Drawing.Point(290, 19);
            this.btnFetch.Name = "btnFetch";
            this.btnFetch.Size = new System.Drawing.Size(136, 23);
            this.btnFetch.TabIndex = 9;
            this.btnFetch.Text = "Fetch";
            this.btnFetch.UseVisualStyleBackColor = true;
            this.btnFetch.Click += new System.EventHandler(this.btnFetch_Click);
            // 
            // btnMergeProj
            // 
            this.btnMergeProj.Location = new System.Drawing.Point(23, 12);
            this.btnMergeProj.Name = "btnMergeProj";
            this.btnMergeProj.Size = new System.Drawing.Size(136, 23);
            this.btnMergeProj.TabIndex = 5;
            this.btnMergeProj.Text = "Синхронизировать";
            this.btnMergeProj.UseVisualStyleBackColor = true;
            this.btnMergeProj.Click += new System.EventHandler(this.btnMergeProj_Click);
            // 
            // btn_config
            // 
            this.btn_config.Location = new System.Drawing.Point(165, 12);
            this.btn_config.Name = "btn_config";
            this.btn_config.Size = new System.Drawing.Size(136, 23);
            this.btn_config.TabIndex = 21;
            this.btn_config.Text = "Настройки";
            this.btn_config.UseVisualStyleBackColor = true;
            this.btn_config.Click += new System.EventHandler(this.btn_config_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "-";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "-";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(17, 184);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(445, 18);
            this.progressBar2.TabIndex = 23;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(17, 147);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(445, 18);
            this.progressBar1.TabIndex = 22;
            // 
            // progressBarTimer
            // 
            this.progressBarTimer.Enabled = true;
            this.progressBarTimer.Tick += new System.EventHandler(this.progressBarTimer_Tick);
            // 
            // ProjectListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 241);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnMergeProj);
            this.Controls.Add(this.btn_config);
            this.Controls.Add(this.grRepoControls);
            this.Controls.Add(this.cbOtherThread);
            this.Name = "ProjectListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Проект";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProjectListForm_FormClosing);
            this.Load += new System.EventHandler(this.ProjectListForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProjectListForm_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ProjectListForm_KeyPress);
            this.grRepoControls.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_commit;
        private System.Windows.Forms.Button btn_push;
        private System.Windows.Forms.Button btn_pull;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.CheckBox cbOtherThread;
        private System.Windows.Forms.GroupBox grRepoControls;
        private System.Windows.Forms.Button btnFetch;
        private System.Windows.Forms.Button btnMergeProj;
        private System.Windows.Forms.Button btn_config;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer progressBarTimer;

    }
}

