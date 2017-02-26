namespace ProjectSourceManager
{
    partial class ProjectEditor
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbConnectionString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lLinkVorlagen = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbVorlagen = new System.Windows.Forms.TextBox();
            this.tbReportURL = new System.Windows.Forms.TextBox();
            this.tbReportUsername = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbReportPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbReportRoot = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.Location = new System.Drawing.Point(160, 254);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(379, 254);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tbConnectionString
            // 
            this.tbConnectionString.Location = new System.Drawing.Point(120, 61);
            this.tbConnectionString.Multiline = true;
            this.tbConnectionString.Name = "tbConnectionString";
            this.tbConnectionString.Size = new System.Drawing.Size(463, 72);
            this.tbConnectionString.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Vorlagen";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Application database";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Report URL";
            // 
            // lLinkVorlagen
            // 
            this.lLinkVorlagen.AutoSize = true;
            this.lLinkVorlagen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lLinkVorlagen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lLinkVorlagen.ForeColor = System.Drawing.Color.Maroon;
            this.lLinkVorlagen.Location = new System.Drawing.Point(534, 38);
            this.lLinkVorlagen.Name = "lLinkVorlagen";
            this.lLinkVorlagen.Size = new System.Drawing.Size(49, 13);
            this.lLinkVorlagen.TabIndex = 8;
            this.lLinkVorlagen.Text = "Связать";
            this.lLinkVorlagen.Click += new System.EventHandler(this.lLinkVorlagen_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Name";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(120, 9);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(194, 20);
            this.tbName.TabIndex = 10;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(267, 254);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbVorlagen
            // 
            this.tbVorlagen.Location = new System.Drawing.Point(120, 35);
            this.tbVorlagen.Name = "tbVorlagen";
            this.tbVorlagen.ReadOnly = true;
            this.tbVorlagen.Size = new System.Drawing.Size(408, 20);
            this.tbVorlagen.TabIndex = 12;
            // 
            // tbReportURL
            // 
            this.tbReportURL.Location = new System.Drawing.Point(120, 139);
            this.tbReportURL.Name = "tbReportURL";
            this.tbReportURL.Size = new System.Drawing.Size(463, 20);
            this.tbReportURL.TabIndex = 13;
            // 
            // tbReportUsername
            // 
            this.tbReportUsername.Location = new System.Drawing.Point(120, 165);
            this.tbReportUsername.Name = "tbReportUsername";
            this.tbReportUsername.Size = new System.Drawing.Size(463, 20);
            this.tbReportUsername.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Report Username";
            // 
            // tbReportPassword
            // 
            this.tbReportPassword.Location = new System.Drawing.Point(120, 191);
            this.tbReportPassword.Name = "tbReportPassword";
            this.tbReportPassword.PasswordChar = '*';
            this.tbReportPassword.Size = new System.Drawing.Size(463, 20);
            this.tbReportPassword.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 194);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Report Password";
            // 
            // tbReportRoot
            // 
            this.tbReportRoot.Location = new System.Drawing.Point(120, 217);
            this.tbReportRoot.Name = "tbReportRoot";
            this.tbReportRoot.Size = new System.Drawing.Size(463, 20);
            this.tbReportRoot.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 220);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Report Root";
            // 
            // ProjectEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 289);
            this.Controls.Add(this.tbReportRoot);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbReportPassword);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbReportUsername);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbReportURL);
            this.Controls.Add(this.tbVorlagen);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lLinkVorlagen);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbConnectionString);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "ProjectEditor";
            this.Text = "Редактировать проект";
            this.Load += new System.EventHandler(this.ProjectEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbConnectionString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lLinkVorlagen;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox tbVorlagen;
        private System.Windows.Forms.TextBox tbReportURL;
        private System.Windows.Forms.TextBox tbReportUsername;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbReportPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbReportRoot;
        private System.Windows.Forms.Label label7;
    }
}