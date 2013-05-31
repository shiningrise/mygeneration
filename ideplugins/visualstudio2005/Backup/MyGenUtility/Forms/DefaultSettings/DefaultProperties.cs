using System;
using System.Xml;
using System.IO;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Text;

//using ADODB;
//using MSDASC;

using MyMeta;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for DefaultProperties.
	/// </summary>
    public class DefaultProperties : DockContent, IMyGenDocument
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;
        private ToolStrip toolStripOptions;
        private ToolStripButton toolStripButtonSave;
        private ToolStripSeparator toolStripSeparator1;
        private MenuStrip menuStripMain;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private DefaultSettingsControl defaultSettingsControl;
        private ToolStripSeparator toolStripMenuItem1;
        private IMyGenerationMDI mdi;

        public DefaultProperties(IMyGenerationMDI mdi)
		{
			InitializeComponent();

            this.mdi = mdi;
            this.defaultSettingsControl.ShowOLEDBDialog = new ShowOleDbDialogHandler(ShowOLEDBDialog);
            this.defaultSettingsControl.Initialize(mdi);
		}

        public string ShowOLEDBDialog(string cs)
        {
            return mdi.PerformMdiFuntion(this, "ShowOLEDBDialog", cs) as String; 
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefaultProperties));
            this.toolStripOptions = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.defaultSettingsControl = new MyGeneration.DefaultSettingsControl();
            this.toolStripOptions.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripOptions
            // 
            this.toolStripOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave,
            this.toolStripSeparator1});
            this.toolStripOptions.Location = new System.Drawing.Point(0, 0);
            this.toolStripOptions.Name = "toolStripOptions";
            this.toolStripOptions.Size = new System.Drawing.Size(896, 25);
            this.toolStripOptions.TabIndex = 34;
            this.toolStripOptions.Visible = false;
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripButtonSave.MergeIndex = 0;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = "Save Settings";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripSeparator1.MergeIndex = 1;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStripMain.Size = new System.Drawing.Size(896, 24);
            this.menuStripMain.TabIndex = 39;
            this.menuStripMain.Text = "menuStrip1";
            this.menuStripMain.Visible = false;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripMenuItem1});
            this.fileToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.fileToolStripMenuItem.MergeIndex = 0;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.saveToolStripMenuItem.MergeIndex = 4;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.closeToolStripMenuItem.MergeIndex = 5;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripMenuItem1.MergeIndex = 6;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(150, 6);
            // 
            // defaultSettingsControl
            // 
            this.defaultSettingsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defaultSettingsControl.Location = new System.Drawing.Point(0, 0);
            this.defaultSettingsControl.Name = "defaultSettingsControl";
            this.defaultSettingsControl.Size = new System.Drawing.Size(896, 603);
            this.defaultSettingsControl.TabIndex = 40;
            // 
            // DefaultProperties
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(590, 531);
            this.ClientSize = new System.Drawing.Size(896, 603);
            this.Controls.Add(this.menuStripMain);
            this.Controls.Add(this.defaultSettingsControl);
            this.Controls.Add(this.toolStripOptions);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DefaultProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TabText = "Default Settings";
            this.Text = "Default Settings";
            this.Load += new System.EventHandler(this.DefaultProperties_Load);
            this.Leave += new System.EventHandler(this.DefaultProperties_Leave);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DefaultProperties_FormClosing);
            this.toolStripOptions.ResumeLayout(false);
            this.toolStripOptions.PerformLayout();
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private void DefaultProperties_Load(object sender, EventArgs e)
        {
            this.defaultSettingsControl.Populate();
        }

        void DefaultProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((e.CloseReason == CloseReason.UserClosing) ||
                (e.CloseReason == CloseReason.FormOwnerClosing))
            {
                DialogResult r = DialogResult.None;

                // Something's Changed since the load...
                if (defaultSettingsControl.SettingsModified)
                {
                    r = MessageBox.Show("Default settings have changed.\r\n Would you like to save before exiting?", "Default Settings Changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                }
                else if (defaultSettingsControl.ConnectionInfoModified)
                {
                    r = MessageBox.Show("The loaded connection profile has changed.\r\n Would you like to save before exiting?", "Connection Profile Changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                }

                if (r != DialogResult.None)
                {
                    if (r == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    else if (r == DialogResult.Yes)
                    {
                        if (defaultSettingsControl.Save())
                        {
                            this.DialogResult = DialogResult.OK;
                            mdi.SendAlert(this, "UpdateDefaultSettings");
                        }
                    }
                    else
                    {
                        defaultSettingsControl.Cancel();
                        this.DialogResult = DialogResult.Cancel;
                    }
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (defaultSettingsControl.Save())
            {
                mdi.SendAlert(this, "UpdateDefaultSettings");
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (defaultSettingsControl.Save())
            {
                mdi.SendAlert(this, "UpdateDefaultSettings");
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DefaultProperties_Leave(object sender, EventArgs e)
        {
            DialogResult r = DialogResult.None;

            // Something's Changed since the load...
            if (defaultSettingsControl.SettingsModified)
            {
                r = MessageBox.Show("Default settings have changed.\r\n Would you like to save before leaving?", "Default Settings Changed", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            }
            else if (defaultSettingsControl.ConnectionInfoModified)
            {
                r = MessageBox.Show("The loaded connection profile has changed.\r\n Would you like to save before leaving?", "Connection Profile Changed", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            }

            if (r == DialogResult.Yes)
            {
                if (defaultSettingsControl.Save())
                {
                    mdi.SendAlert(this, "UpdateDefaultSettings");
                }
            }
        }

        #region IMyGenDocument Members

        public ToolStrip ToolStrip
        {
            get { return this.toolStripOptions; }
        }

        public string DocumentIndentity
        {
            get { return "::DefaultSettings::"; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public bool CanClose(bool allowPrevent)
        {
            return true;
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        public string TextContent
        {
            get { return this.defaultSettingsControl.TextContent; }
        }

        #endregion
    }
}
