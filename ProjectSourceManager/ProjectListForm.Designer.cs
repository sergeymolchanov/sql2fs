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
            this.btnDump = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btn_commit = new System.Windows.Forms.Button();
            this.btn_push = new System.Windows.Forms.Button();
            this.btn_pull = new System.Windows.Forms.Button();
            this.btnLog = new System.Windows.Forms.Button();
            this.btnMerge = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cbOtherThread = new System.Windows.Forms.CheckBox();
            this.grRepoControls = new System.Windows.Forms.GroupBox();
            this.btnSwitch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCheck = new System.Windows.Forms.Button();
            this.btnMergeProj = new System.Windows.Forms.Button();
            this.cbForce = new System.Windows.Forms.CheckBox();
            this.cbExpert = new System.Windows.Forms.CheckBox();
            this.btn_config = new System.Windows.Forms.Button();
            this.grRepoControls.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDump
            // 
            this.btnDump.Location = new System.Drawing.Point(6, 48);
            this.btnDump.Name = "btnDump";
            this.btnDump.Size = new System.Drawing.Size(136, 23);
            this.btnDump.TabIndex = 2;
            this.btnDump.Text = "БД -> Репозитарий";
            this.btnDump.UseVisualStyleBackColor = true;
            this.btnDump.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(6, 19);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(136, 23);
            this.btnRestore.TabIndex = 3;
            this.btnRestore.Text = "Репозитарий -> БД";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
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
            this.btn_push.Location = new System.Drawing.Point(6, 77);
            this.btn_push.Name = "btn_push";
            this.btn_push.Size = new System.Drawing.Size(136, 23);
            this.btn_push.TabIndex = 5;
            this.btn_push.Text = "Push";
            this.btn_push.UseVisualStyleBackColor = true;
            this.btn_push.Click += new System.EventHandler(this.btn_push_Click);
            // 
            // btn_pull
            // 
            this.btn_pull.Location = new System.Drawing.Point(6, 48);
            this.btn_pull.Name = "btn_pull";
            this.btn_pull.Size = new System.Drawing.Size(136, 23);
            this.btn_pull.TabIndex = 6;
            this.btn_pull.Text = "Pull";
            this.btn_pull.UseVisualStyleBackColor = true;
            this.btn_pull.Click += new System.EventHandler(this.btn_pull_Click);
            // 
            // btnLog
            // 
            this.btnLog.Location = new System.Drawing.Point(6, 173);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(136, 23);
            this.btnLog.TabIndex = 7;
            this.btnLog.Text = "Log";
            this.btnLog.UseVisualStyleBackColor = true;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(6, 106);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(136, 23);
            this.btnMerge.TabIndex = 8;
            this.btnMerge.Text = "Merge";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cbOtherThread
            // 
            this.cbOtherThread.AutoSize = true;
            this.cbOtherThread.Checked = true;
            this.cbOtherThread.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOtherThread.Location = new System.Drawing.Point(17, 228);
            this.cbOtherThread.Name = "cbOtherThread";
            this.cbOtherThread.Size = new System.Drawing.Size(86, 17);
            this.cbOtherThread.TabIndex = 10;
            this.cbOtherThread.Text = "OtherThread";
            this.cbOtherThread.UseVisualStyleBackColor = true;
            // 
            // grRepoControls
            // 
            this.grRepoControls.Controls.Add(this.btnSwitch);
            this.grRepoControls.Controls.Add(this.btn_commit);
            this.grRepoControls.Controls.Add(this.btn_push);
            this.grRepoControls.Controls.Add(this.btn_pull);
            this.grRepoControls.Controls.Add(this.btnMerge);
            this.grRepoControls.Controls.Add(this.btnLog);
            this.grRepoControls.Location = new System.Drawing.Point(174, 9);
            this.grRepoControls.Name = "grRepoControls";
            this.grRepoControls.Size = new System.Drawing.Size(151, 202);
            this.grRepoControls.TabIndex = 15;
            this.grRepoControls.TabStop = false;
            this.grRepoControls.Text = "Репозитарий";
            // 
            // btnSwitch
            // 
            this.btnSwitch.Location = new System.Drawing.Point(6, 135);
            this.btnSwitch.Name = "btnSwitch";
            this.btnSwitch.Size = new System.Drawing.Size(136, 23);
            this.btnSwitch.TabIndex = 9;
            this.btnSwitch.Text = "Switch";
            this.btnSwitch.UseVisualStyleBackColor = true;
            this.btnSwitch.Click += new System.EventHandler(this.btnSwitch_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCheck);
            this.groupBox1.Controls.Add(this.btnMergeProj);
            this.groupBox1.Controls.Add(this.btnDump);
            this.groupBox1.Controls.Add(this.btnRestore);
            this.groupBox1.Location = new System.Drawing.Point(17, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(151, 139);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Синхронизация";
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(6, 106);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(136, 23);
            this.btnCheck.TabIndex = 4;
            this.btnCheck.Text = "Проверить различия";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // btnMergeProj
            // 
            this.btnMergeProj.Location = new System.Drawing.Point(6, 77);
            this.btnMergeProj.Name = "btnMergeProj";
            this.btnMergeProj.Size = new System.Drawing.Size(136, 23);
            this.btnMergeProj.TabIndex = 5;
            this.btnMergeProj.Text = "Синхронизировать";
            this.btnMergeProj.UseVisualStyleBackColor = true;
            this.btnMergeProj.Click += new System.EventHandler(this.button2_Click);
            // 
            // cbForce
            // 
            this.cbForce.AutoSize = true;
            this.cbForce.Location = new System.Drawing.Point(220, 228);
            this.cbForce.Name = "cbForce";
            this.cbForce.Size = new System.Drawing.Size(53, 17);
            this.cbForce.TabIndex = 19;
            this.cbForce.Text = "Force";
            this.cbForce.UseVisualStyleBackColor = true;
            // 
            // cbExpert
            // 
            this.cbExpert.AutoSize = true;
            this.cbExpert.Location = new System.Drawing.Point(109, 228);
            this.cbExpert.Name = "cbExpert";
            this.cbExpert.Size = new System.Drawing.Size(105, 17);
            this.cbExpert.TabIndex = 20;
            this.cbExpert.Text = "I know what I do";
            this.cbExpert.UseVisualStyleBackColor = true;
            this.cbExpert.CheckedChanged += new System.EventHandler(this.cbExpert_CheckedChanged);
            // 
            // btn_config
            // 
            this.btn_config.Location = new System.Drawing.Point(23, 28);
            this.btn_config.Name = "btn_config";
            this.btn_config.Size = new System.Drawing.Size(136, 23);
            this.btn_config.TabIndex = 21;
            this.btn_config.Text = "Настройки";
            this.btn_config.UseVisualStyleBackColor = true;
            this.btn_config.Click += new System.EventHandler(this.btn_config_Click);
            // 
            // ProjectListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 257);
            this.Controls.Add(this.btn_config);
            this.Controls.Add(this.cbExpert);
            this.Controls.Add(this.cbForce);
            this.Controls.Add(this.groupBox1);
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
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDump;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btn_commit;
        private System.Windows.Forms.Button btn_push;
        private System.Windows.Forms.Button btn_pull;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox cbOtherThread;
        private System.Windows.Forms.GroupBox grRepoControls;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSwitch;
        private System.Windows.Forms.CheckBox cbForce;
        private System.Windows.Forms.CheckBox cbExpert;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Button btnMergeProj;
        private System.Windows.Forms.Button btn_config;

    }
}

