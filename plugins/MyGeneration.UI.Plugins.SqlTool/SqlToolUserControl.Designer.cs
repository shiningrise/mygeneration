namespace MyGeneration.UI.Plugins.SqlTool
{
    partial class SqlToolUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.scintilla = new Scintilla.ScintillaControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.tabControlResults = new System.Windows.Forms.TabControl();
            this.tabPageResults1 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.scintilla)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.tabControlResults.SuspendLayout();
            this.tabPageResults1.SuspendLayout();
            this.SuspendLayout();
            // 
            // scintilla
            // 
            this.scintilla.ConfigurationLanguage = "sql";
            this.scintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla.IsBraceMatching = false;
            this.scintilla.Location = new System.Drawing.Point(0, 0);
            this.scintilla.Name = "scintilla";
            this.scintilla.Size = new System.Drawing.Size(784, 304);
            this.scintilla.SmartIndentType = Scintilla.Enums.SmartIndent.None;
            this.scintilla.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.scintilla);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControlResults);
            this.splitContainer1.Size = new System.Drawing.Size(784, 600);
            this.splitContainer1.SplitterDistance = 304;
            this.splitContainer1.TabIndex = 1;
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.AllowUserToAddRows = false;
            this.dataGridViewResults.AllowUserToDeleteRows = false;
            this.dataGridViewResults.AllowUserToOrderColumns = true;
            this.dataGridViewResults.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewResults.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.ReadOnly = true;
            this.dataGridViewResults.Size = new System.Drawing.Size(770, 260);
            this.dataGridViewResults.TabIndex = 0;
            // 
            // tabControlResults
            // 
            this.tabControlResults.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControlResults.Controls.Add(this.tabPageResults1);
            this.tabControlResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlResults.HotTrack = true;
            this.tabControlResults.Location = new System.Drawing.Point(0, 0);
            this.tabControlResults.Name = "tabControlResults";
            this.tabControlResults.SelectedIndex = 0;
            this.tabControlResults.Size = new System.Drawing.Size(784, 292);
            this.tabControlResults.TabIndex = 0;
            // 
            // tabPageResults1
            // 
            this.tabPageResults1.Controls.Add(this.dataGridViewResults);
            this.tabPageResults1.Location = new System.Drawing.Point(4, 4);
            this.tabPageResults1.Name = "tabPageResults1";
            this.tabPageResults1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageResults1.Size = new System.Drawing.Size(776, 266);
            this.tabPageResults1.TabIndex = 0;
            this.tabPageResults1.Text = "Results 01";
            this.tabPageResults1.UseVisualStyleBackColor = true;
            // 
            // SqlToolUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "SqlToolUserControl";
            this.Size = new System.Drawing.Size(784, 600);
            ((System.ComponentModel.ISupportInitialize)(this.scintilla)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.tabControlResults.ResumeLayout(false);
            this.tabPageResults1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Scintilla.ScintillaControl scintilla;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.TabControl tabControlResults;
        private System.Windows.Forms.TabPage tabPageResults1;
    }
}
