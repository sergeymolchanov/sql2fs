namespace ProjectSourceManager
{
    partial class AskMergeStyleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRepo2Db = new System.Windows.Forms.Button();
            this.btnDb2Repo = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnMerge = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRepo2Db
            // 
            this.btnRepo2Db.Location = new System.Drawing.Point(12, 36);
            this.btnRepo2Db.Name = "btnRepo2Db";
            this.btnRepo2Db.Size = new System.Drawing.Size(127, 23);
            this.btnRepo2Db.TabIndex = 0;
            this.btnRepo2Db.Text = "Репозитарий - БД";
            this.btnRepo2Db.UseVisualStyleBackColor = true;
            this.btnRepo2Db.Click += new System.EventHandler(this.btnRepo2Db_Click);
            // 
            // btnDb2Repo
            // 
            this.btnDb2Repo.Location = new System.Drawing.Point(12, 65);
            this.btnDb2Repo.Name = "btnDb2Repo";
            this.btnDb2Repo.Size = new System.Drawing.Size(127, 23);
            this.btnDb2Repo.TabIndex = 1;
            this.btnDb2Repo.Text = "БД - Репозитарий";
            this.btnDb2Repo.UseVisualStyleBackColor = true;
            this.btnDb2Repo.Click += new System.EventHandler(this.btnDb2Repo_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(176, 65);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Выход";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(176, 36);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(127, 23);
            this.btnMerge.TabIndex = 3;
            this.btnMerge.Text = "Слияние";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(91, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Начальная синхронизация";
            // 
            // AskMergeStyleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 106);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDb2Repo);
            this.Controls.Add(this.btnRepo2Db);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AskMergeStyleForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRepo2Db;
        private System.Windows.Forms.Button btnDb2Repo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Label label1;
    }
}