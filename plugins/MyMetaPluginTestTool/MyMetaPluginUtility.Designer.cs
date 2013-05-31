namespace MyMetaPluginTestTool
{
    partial class MyMetaPluginUtility
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
            this.tabControlPluginStuff = new System.Windows.Forms.TabControl();
            this.tabPageTest = new System.Windows.Forms.TabPage();
            this.checkBoxDefaultDb = new System.Windows.Forms.CheckBox();
            this.checkBoxProcOther = new System.Windows.Forms.CheckBox();
            this.checkBoxViewOther = new System.Windows.Forms.CheckBox();
            this.checkBoxViewColumns = new System.Windows.Forms.CheckBox();
            this.checkBoxTableOther = new System.Windows.Forms.CheckBox();
            this.checkBoxTableColumns = new System.Windows.Forms.CheckBox();
            this.checkBoxViews = new System.Windows.Forms.CheckBox();
            this.checkBoxParameters = new System.Windows.Forms.CheckBox();
            this.checkBoxTables = new System.Windows.Forms.CheckBox();
            this.checkBoxProcedures = new System.Windows.Forms.CheckBox();
            this.checkBoxPlugin = new System.Windows.Forms.CheckBox();
            this.checkBoxAPI = new System.Windows.Forms.CheckBox();
            this.textBoxResults = new System.Windows.Forms.TextBox();
            this.buttonTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxConnectionString = new System.Windows.Forms.TextBox();
            this.tabPageData = new System.Windows.Forms.TabPage();
            this.comboBoxPlugins = new System.Windows.Forms.ComboBox();
            this.tabControlPluginStuff.SuspendLayout();
            this.tabPageTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlPluginStuff
            // 
            this.tabControlPluginStuff.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlPluginStuff.Controls.Add(this.tabPageTest);
            this.tabControlPluginStuff.Controls.Add(this.tabPageData);
            this.tabControlPluginStuff.Location = new System.Drawing.Point(12, 39);
            this.tabControlPluginStuff.Name = "tabControlPluginStuff";
            this.tabControlPluginStuff.SelectedIndex = 0;
            this.tabControlPluginStuff.Size = new System.Drawing.Size(720, 459);
            this.tabControlPluginStuff.TabIndex = 0;
            // 
            // tabPageTest
            // 
            this.tabPageTest.Controls.Add(this.checkBoxDefaultDb);
            this.tabPageTest.Controls.Add(this.checkBoxProcOther);
            this.tabPageTest.Controls.Add(this.checkBoxViewOther);
            this.tabPageTest.Controls.Add(this.checkBoxViewColumns);
            this.tabPageTest.Controls.Add(this.checkBoxTableOther);
            this.tabPageTest.Controls.Add(this.checkBoxTableColumns);
            this.tabPageTest.Controls.Add(this.checkBoxViews);
            this.tabPageTest.Controls.Add(this.checkBoxParameters);
            this.tabPageTest.Controls.Add(this.checkBoxTables);
            this.tabPageTest.Controls.Add(this.checkBoxProcedures);
            this.tabPageTest.Controls.Add(this.checkBoxPlugin);
            this.tabPageTest.Controls.Add(this.checkBoxAPI);
            this.tabPageTest.Controls.Add(this.textBoxResults);
            this.tabPageTest.Controls.Add(this.buttonTest);
            this.tabPageTest.Controls.Add(this.label1);
            this.tabPageTest.Controls.Add(this.textBoxConnectionString);
            this.tabPageTest.Location = new System.Drawing.Point(4, 22);
            this.tabPageTest.Name = "tabPageTest";
            this.tabPageTest.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTest.Size = new System.Drawing.Size(712, 433);
            this.tabPageTest.TabIndex = 0;
            this.tabPageTest.Text = "Test";
            this.tabPageTest.UseVisualStyleBackColor = true;
            // 
            // checkBoxDefaultDb
            // 
            this.checkBoxDefaultDb.AutoSize = true;
            this.checkBoxDefaultDb.Checked = true;
            this.checkBoxDefaultDb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDefaultDb.Location = new System.Drawing.Point(219, 48);
            this.checkBoxDefaultDb.Name = "checkBoxDefaultDb";
            this.checkBoxDefaultDb.Size = new System.Drawing.Size(139, 17);
            this.checkBoxDefaultDb.TabIndex = 14;
            this.checkBoxDefaultDb.Text = "Default Database Only?";
            this.checkBoxDefaultDb.UseVisualStyleBackColor = true;
            // 
            // checkBoxProcOther
            // 
            this.checkBoxProcOther.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxProcOther.AutoSize = true;
            this.checkBoxProcOther.Checked = true;
            this.checkBoxProcOther.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxProcOther.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.checkBoxProcOther.Location = new System.Drawing.Point(621, 78);
            this.checkBoxProcOther.Name = "checkBoxProcOther";
            this.checkBoxProcOther.Size = new System.Drawing.Size(52, 17);
            this.checkBoxProcOther.TabIndex = 13;
            this.checkBoxProcOther.Text = "Other";
            this.checkBoxProcOther.UseVisualStyleBackColor = true;
            // 
            // checkBoxViewOther
            // 
            this.checkBoxViewOther.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxViewOther.AutoSize = true;
            this.checkBoxViewOther.Checked = true;
            this.checkBoxViewOther.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxViewOther.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.checkBoxViewOther.Location = new System.Drawing.Point(545, 78);
            this.checkBoxViewOther.Name = "checkBoxViewOther";
            this.checkBoxViewOther.Size = new System.Drawing.Size(52, 17);
            this.checkBoxViewOther.TabIndex = 11;
            this.checkBoxViewOther.Text = "Other";
            this.checkBoxViewOther.UseVisualStyleBackColor = true;
            // 
            // checkBoxViewColumns
            // 
            this.checkBoxViewColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxViewColumns.AutoSize = true;
            this.checkBoxViewColumns.Checked = true;
            this.checkBoxViewColumns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxViewColumns.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.checkBoxViewColumns.Location = new System.Drawing.Point(545, 63);
            this.checkBoxViewColumns.Name = "checkBoxViewColumns";
            this.checkBoxViewColumns.Size = new System.Drawing.Size(66, 17);
            this.checkBoxViewColumns.TabIndex = 12;
            this.checkBoxViewColumns.Text = "Columns";
            this.checkBoxViewColumns.UseVisualStyleBackColor = true;
            // 
            // checkBoxTableOther
            // 
            this.checkBoxTableOther.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxTableOther.AutoSize = true;
            this.checkBoxTableOther.Checked = true;
            this.checkBoxTableOther.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTableOther.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.checkBoxTableOther.Location = new System.Drawing.Point(461, 78);
            this.checkBoxTableOther.Name = "checkBoxTableOther";
            this.checkBoxTableOther.Size = new System.Drawing.Size(52, 17);
            this.checkBoxTableOther.TabIndex = 11;
            this.checkBoxTableOther.Text = "Other";
            this.checkBoxTableOther.UseVisualStyleBackColor = true;
            // 
            // checkBoxTableColumns
            // 
            this.checkBoxTableColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxTableColumns.AutoSize = true;
            this.checkBoxTableColumns.Checked = true;
            this.checkBoxTableColumns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTableColumns.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.checkBoxTableColumns.Location = new System.Drawing.Point(461, 63);
            this.checkBoxTableColumns.Name = "checkBoxTableColumns";
            this.checkBoxTableColumns.Size = new System.Drawing.Size(66, 17);
            this.checkBoxTableColumns.TabIndex = 10;
            this.checkBoxTableColumns.Text = "Columns";
            this.checkBoxTableColumns.UseVisualStyleBackColor = true;
            // 
            // checkBoxViews
            // 
            this.checkBoxViews.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxViews.AutoSize = true;
            this.checkBoxViews.Checked = true;
            this.checkBoxViews.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxViews.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.checkBoxViews.Location = new System.Drawing.Point(545, 48);
            this.checkBoxViews.Name = "checkBoxViews";
            this.checkBoxViews.Size = new System.Drawing.Size(54, 17);
            this.checkBoxViews.TabIndex = 9;
            this.checkBoxViews.Text = "Views";
            this.checkBoxViews.UseVisualStyleBackColor = true;
            this.checkBoxViews.CheckedChanged += new System.EventHandler(this.checkBoxViews_CheckedChanged);
            // 
            // checkBoxParameters
            // 
            this.checkBoxParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxParameters.AutoSize = true;
            this.checkBoxParameters.Checked = true;
            this.checkBoxParameters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxParameters.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.checkBoxParameters.Location = new System.Drawing.Point(621, 63);
            this.checkBoxParameters.Name = "checkBoxParameters";
            this.checkBoxParameters.Size = new System.Drawing.Size(79, 17);
            this.checkBoxParameters.TabIndex = 8;
            this.checkBoxParameters.Text = "Parameters";
            this.checkBoxParameters.UseVisualStyleBackColor = true;
            // 
            // checkBoxTables
            // 
            this.checkBoxTables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxTables.AutoSize = true;
            this.checkBoxTables.Checked = true;
            this.checkBoxTables.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTables.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.checkBoxTables.Location = new System.Drawing.Point(461, 48);
            this.checkBoxTables.Name = "checkBoxTables";
            this.checkBoxTables.Size = new System.Drawing.Size(58, 17);
            this.checkBoxTables.TabIndex = 7;
            this.checkBoxTables.Text = "Tables";
            this.checkBoxTables.UseVisualStyleBackColor = true;
            this.checkBoxTables.CheckedChanged += new System.EventHandler(this.checkBoxTables_CheckedChanged);
            // 
            // checkBoxProcedures
            // 
            this.checkBoxProcedures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxProcedures.AutoSize = true;
            this.checkBoxProcedures.Checked = true;
            this.checkBoxProcedures.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxProcedures.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.checkBoxProcedures.Location = new System.Drawing.Point(621, 48);
            this.checkBoxProcedures.Name = "checkBoxProcedures";
            this.checkBoxProcedures.Size = new System.Drawing.Size(80, 17);
            this.checkBoxProcedures.TabIndex = 6;
            this.checkBoxProcedures.Text = "Procedures";
            this.checkBoxProcedures.UseVisualStyleBackColor = true;
            this.checkBoxProcedures.CheckedChanged += new System.EventHandler(this.checkBoxProcedures_CheckedChanged);
            // 
            // checkBoxPlugin
            // 
            this.checkBoxPlugin.AutoSize = true;
            this.checkBoxPlugin.Checked = true;
            this.checkBoxPlugin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPlugin.Location = new System.Drawing.Point(7, 48);
            this.checkBoxPlugin.Name = "checkBoxPlugin";
            this.checkBoxPlugin.Size = new System.Drawing.Size(107, 17);
            this.checkBoxPlugin.TabIndex = 5;
            this.checkBoxPlugin.Text = "Do Plugin Tests?";
            this.checkBoxPlugin.UseVisualStyleBackColor = true;
            // 
            // checkBoxAPI
            // 
            this.checkBoxAPI.AutoSize = true;
            this.checkBoxAPI.Checked = true;
            this.checkBoxAPI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAPI.Location = new System.Drawing.Point(118, 48);
            this.checkBoxAPI.Name = "checkBoxAPI";
            this.checkBoxAPI.Size = new System.Drawing.Size(95, 17);
            this.checkBoxAPI.TabIndex = 4;
            this.checkBoxAPI.Text = "Do API Tests?";
            this.checkBoxAPI.UseVisualStyleBackColor = true;
            // 
            // textBoxResults
            // 
            this.textBoxResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResults.Location = new System.Drawing.Point(7, 101);
            this.textBoxResults.MaxLength = 999999;
            this.textBoxResults.Multiline = true;
            this.textBoxResults.Name = "textBoxResults";
            this.textBoxResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxResults.Size = new System.Drawing.Size(702, 326);
            this.textBoxResults.TabIndex = 3;
            this.textBoxResults.WordWrap = false;
            // 
            // buttonTest
            // 
            this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTest.Location = new System.Drawing.Point(644, 19);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(62, 23);
            this.buttonTest.TabIndex = 2;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Connection String";
            // 
            // textBoxConnectionString
            // 
            this.textBoxConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxConnectionString.Location = new System.Drawing.Point(6, 22);
            this.textBoxConnectionString.Name = "textBoxConnectionString";
            this.textBoxConnectionString.Size = new System.Drawing.Size(632, 20);
            this.textBoxConnectionString.TabIndex = 0;
            // 
            // tabPageData
            // 
            this.tabPageData.Location = new System.Drawing.Point(4, 22);
            this.tabPageData.Name = "tabPageData";
            this.tabPageData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageData.Size = new System.Drawing.Size(712, 433);
            this.tabPageData.TabIndex = 1;
            this.tabPageData.Text = "Data";
            this.tabPageData.UseVisualStyleBackColor = true;
            // 
            // comboBoxPlugins
            // 
            this.comboBoxPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPlugins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlugins.FormattingEnabled = true;
            this.comboBoxPlugins.Location = new System.Drawing.Point(12, 13);
            this.comboBoxPlugins.Name = "comboBoxPlugins";
            this.comboBoxPlugins.Size = new System.Drawing.Size(716, 21);
            this.comboBoxPlugins.TabIndex = 1;
            this.comboBoxPlugins.SelectedIndexChanged += new System.EventHandler(this.comboBoxPlugins_SelectedIndexChanged);
            // 
            // MyMetaPluginUtility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 510);
            this.Controls.Add(this.comboBoxPlugins);
            this.Controls.Add(this.tabControlPluginStuff);
            this.Name = "MyMetaPluginUtility";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MyMetaPluginUtility_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MyMetaPluginUtility_FormClosing);
            this.tabControlPluginStuff.ResumeLayout(false);
            this.tabPageTest.ResumeLayout(false);
            this.tabPageTest.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlPluginStuff;
        private System.Windows.Forms.TabPage tabPageTest;
        private System.Windows.Forms.TabPage tabPageData;
        private System.Windows.Forms.ComboBox comboBoxPlugins;
        private System.Windows.Forms.TextBox textBoxConnectionString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.TextBox textBoxResults;
        private System.Windows.Forms.CheckBox checkBoxAPI;
        private System.Windows.Forms.CheckBox checkBoxPlugin;
        private System.Windows.Forms.CheckBox checkBoxViews;
        private System.Windows.Forms.CheckBox checkBoxParameters;
        private System.Windows.Forms.CheckBox checkBoxTables;
        private System.Windows.Forms.CheckBox checkBoxProcedures;
        private System.Windows.Forms.CheckBox checkBoxProcOther;
        private System.Windows.Forms.CheckBox checkBoxViewOther;
        private System.Windows.Forms.CheckBox checkBoxViewColumns;
        private System.Windows.Forms.CheckBox checkBoxTableOther;
        private System.Windows.Forms.CheckBox checkBoxTableColumns;
        private System.Windows.Forms.CheckBox checkBoxDefaultDb;
    }
}

