namespace ProjectSourceManager.Adapters.Impl.DBContent
{
    partial class TableRowComparer
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.field = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.before = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.after = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_replace = new System.Windows.Forms.Button();
            this.btn_skip = new System.Windows.Forms.Button();
            this.lAction = new System.Windows.Forms.Label();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_replace_all = new System.Windows.Forms.Button();
            this.btnSkipAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.field,
            this.before,
            this.after});
            this.dataGridView1.Location = new System.Drawing.Point(12, 46);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(708, 362);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // field
            // 
            this.field.HeaderText = "Поле";
            this.field.Name = "field";
            this.field.ReadOnly = true;
            this.field.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.field.Width = 150;
            // 
            // before
            // 
            this.before.HeaderText = "Значение в БД";
            this.before.Name = "before";
            this.before.ReadOnly = true;
            this.before.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.before.Width = 200;
            // 
            // after
            // 
            this.after.HeaderText = "Новое значение";
            this.after.Name = "after";
            this.after.ReadOnly = true;
            this.after.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.after.Width = 200;
            // 
            // btn_replace
            // 
            this.btn_replace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_replace.Location = new System.Drawing.Point(69, 425);
            this.btn_replace.Name = "btn_replace";
            this.btn_replace.Size = new System.Drawing.Size(115, 23);
            this.btn_replace.TabIndex = 1;
            this.btn_replace.Text = "Заменить";
            this.btn_replace.UseVisualStyleBackColor = true;
            this.btn_replace.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_skip
            // 
            this.btn_skip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_skip.Location = new System.Drawing.Point(318, 425);
            this.btn_skip.Name = "btn_skip";
            this.btn_skip.Size = new System.Drawing.Size(115, 23);
            this.btn_skip.TabIndex = 2;
            this.btn_skip.Text = "Пропустить";
            this.btn_skip.UseVisualStyleBackColor = true;
            this.btn_skip.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // lAction
            // 
            this.lAction.AutoSize = true;
            this.lAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lAction.Location = new System.Drawing.Point(12, 9);
            this.lAction.Name = "lAction";
            this.lAction.Size = new System.Drawing.Size(91, 20);
            this.lAction.TabIndex = 3;
            this.lAction.Text = "Действие";
            // 
            // btn_cancel
            // 
            this.btn_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_cancel.Location = new System.Drawing.Point(573, 425);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(115, 23);
            this.btn_cancel.TabIndex = 4;
            this.btn_cancel.Text = "Выход";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click_1);
            // 
            // btn_replace_all
            // 
            this.btn_replace_all.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_replace_all.Location = new System.Drawing.Point(185, 425);
            this.btn_replace_all.Name = "btn_replace_all";
            this.btn_replace_all.Size = new System.Drawing.Size(115, 23);
            this.btn_replace_all.TabIndex = 5;
            this.btn_replace_all.Text = "Заменить (все)";
            this.btn_replace_all.UseVisualStyleBackColor = true;
            this.btn_replace_all.Click += new System.EventHandler(this.btn_replace_all_Click);
            // 
            // btnSkipAll
            // 
            this.btnSkipAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSkipAll.Location = new System.Drawing.Point(439, 425);
            this.btnSkipAll.Name = "btnSkipAll";
            this.btnSkipAll.Size = new System.Drawing.Size(115, 23);
            this.btnSkipAll.TabIndex = 6;
            this.btnSkipAll.Text = "Пропустить (все)";
            this.btnSkipAll.UseVisualStyleBackColor = true;
            this.btnSkipAll.Click += new System.EventHandler(this.btnSkipAll_Click);
            // 
            // TableRowComparer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 463);
            this.Controls.Add(this.btnSkipAll);
            this.Controls.Add(this.btn_replace_all);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.lAction);
            this.Controls.Add(this.btn_skip);
            this.Controls.Add(this.btn_replace);
            this.Controls.Add(this.dataGridView1);
            this.Name = "TableRowComparer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.TableRowComparer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_replace;
        private System.Windows.Forms.Button btn_skip;
        private System.Windows.Forms.Label lAction;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn field;
        private System.Windows.Forms.DataGridViewTextBoxColumn before;
        private System.Windows.Forms.DataGridViewTextBoxColumn after;
        private System.Windows.Forms.Button btn_replace_all;
        private System.Windows.Forms.Button btnSkipAll;
    }
}