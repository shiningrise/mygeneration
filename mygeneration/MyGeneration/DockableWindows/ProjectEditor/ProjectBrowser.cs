using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Zeus;
using Zeus.Projects;
using Zeus.Serializers;
using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;
using MyMeta;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for ProjectBrowser.
	/// </summary>
    public class ProjectBrowser : DockContent, IMyGenDocument
	{
		private const int INDEX_CLOSED_FOLDER = 0;
		private const int INDEX_OPEN_FOLDER = 1;

        private IMyGenerationMDI mdi;
		private System.Windows.Forms.ContextMenu contextMenuTree;
		private System.Windows.Forms.TreeView treeViewProject;
		private System.Windows.Forms.ToolTip toolTipProjectBrowser;
        private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem contextItemEdit;
		private System.Windows.Forms.MenuItem contextItemAddModule;
		private System.Windows.Forms.MenuItem contextItemAddSavedObject;
		private System.Windows.Forms.MenuItem contextItemExecute;
		private System.Windows.Forms.MenuItem contextItemSave;
		private System.Windows.Forms.MenuItem contextItemSaveAs;
		private System.Windows.Forms.MenuItem contextItemRemove;

		private ProjectTreeNode rootNode;
		private FormAddEditModule formEditModule = new FormAddEditModule();
		private FormAddEditSavedObject formEditSavedObject = new FormAddEditSavedObject();
		private System.Windows.Forms.MenuItem menuItemSep01;
		private System.Windows.Forms.MenuItem menuItemSep02;
		private System.Windows.Forms.MenuItem menuItemSep03;
		private System.Windows.Forms.MenuItem contextItemCopy;
		private System.Windows.Forms.MenuItem contextItemCacheSettings;
        private MenuStrip menuStripMain;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStrip toolStripOptions;
        private ToolStripButton toolStripButtonSave;
        private ToolStripButton toolStripButtonSaveAs;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripButtonExecute;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem projectToolStripMenuItem;
        private ToolStripMenuItem executeToolStripMenuItem;
        private ToolStripButton toolStripButtonEdit;
        private ToolStripSeparator toolStripSeparator2;
		
		private bool _isDirty = false;

        public ProjectBrowser(IMyGenerationMDI mdi)
		{
            InitializeComponent();
            this.mdi = mdi;
		}

		protected override string GetPersistString()
		{
			if(this.rootNode != null &&
				this.rootNode.Project != null &&
				this.rootNode.Project.FilePath != null)
			{
				return GetType().ToString() + "," + this.rootNode.Project.FilePath;
			}
			else
			{
                return "type," + ProjectEditorManager.MYGEN_PROJECT;
			}
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

		public string FileName 
		{
			get 
			{
				if (rootNode.Project.FilePath != null)
				{
					return rootNode.Project.FilePath;
				}
				else
				{
					return string.Empty;
				}
			}
		}

		public string CompleteFilePath
		{
			get 
			{
				string tmp = this.FileName;
				if (tmp != string.Empty) 
				{
					FileInfo attr = new FileInfo(tmp);
					tmp = attr.FullName;
				}

				return tmp;
			}
		}

		public bool IsDirty 
		{
			get 
			{
				return this._isDirty;
			}
		}

		public bool CanClose(bool allowPrevent)
		{
			return PromptForSave(allowPrevent);
		}

		private bool PromptForSave(bool allowPrevent)
		{
			bool canClose = true;

			if(this.IsDirty)
			{
				DialogResult result;

				if(allowPrevent)
				{
					result = MessageBox.Show("This project has been modified, Do you wish to save before closing?", 
						this.FileName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				}
				else
				{
					result = MessageBox.Show("This project has been modified, Do you wish to save before closing?", 
						this.FileName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}

				switch(result)
				{
					case DialogResult.Yes:
                        this.Save();
						break;
					case DialogResult.Cancel:
						canClose = false;
						break;
				}
			}

			return canClose;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectBrowser));
            this.treeViewProject = new System.Windows.Forms.TreeView();
            this.contextMenuTree = new System.Windows.Forms.ContextMenu();
            this.contextItemExecute = new System.Windows.Forms.MenuItem();
            this.contextItemCacheSettings = new System.Windows.Forms.MenuItem();
            this.menuItemSep01 = new System.Windows.Forms.MenuItem();
            this.contextItemAddModule = new System.Windows.Forms.MenuItem();
            this.contextItemAddSavedObject = new System.Windows.Forms.MenuItem();
            this.menuItemSep02 = new System.Windows.Forms.MenuItem();
            this.contextItemEdit = new System.Windows.Forms.MenuItem();
            this.contextItemCopy = new System.Windows.Forms.MenuItem();
            this.contextItemRemove = new System.Windows.Forms.MenuItem();
            this.menuItemSep03 = new System.Windows.Forms.MenuItem();
            this.contextItemSave = new System.Windows.Forms.MenuItem();
            this.contextItemSaveAs = new System.Windows.Forms.MenuItem();
            this.toolTipProjectBrowser = new System.Windows.Forms.ToolTip(this.components);
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripOptions = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonExecute = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripMain.SuspendLayout();
            this.toolStripOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewProject
            // 
            this.treeViewProject.ContextMenu = this.contextMenuTree;
            this.treeViewProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewProject.Location = new System.Drawing.Point(0, 0);
            this.treeViewProject.Name = "treeViewProject";
            this.treeViewProject.Size = new System.Drawing.Size(384, 542);
            this.treeViewProject.TabIndex = 1;
            this.treeViewProject.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeViewProject_AfterCollapse);
            this.treeViewProject.DoubleClick += new System.EventHandler(this.treeViewProject_OnDoubleClick);
            this.treeViewProject.MouseMove += new System.Windows.Forms.MouseEventHandler(this.treeViewProject_MouseMove);
            this.treeViewProject.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewProject_KeyDown);
            this.treeViewProject.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeViewProject_AfterExpand);
            this.treeViewProject.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewProject_MouseDown);
            // 
            // contextMenuTree
            // 
            this.contextMenuTree.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.contextItemExecute,
            this.contextItemCacheSettings,
            this.menuItemSep01,
            this.contextItemAddModule,
            this.contextItemAddSavedObject,
            this.menuItemSep02,
            this.contextItemEdit,
            this.contextItemCopy,
            this.contextItemRemove,
            this.menuItemSep03,
            this.contextItemSave,
            this.contextItemSaveAs});
            this.contextMenuTree.Popup += new System.EventHandler(this.contextMenuTree_Popup);
            // 
            // contextItemExecute
            // 
            this.contextItemExecute.Index = 0;
            this.contextItemExecute.Text = "E&xecute";
            this.contextItemExecute.Click += new System.EventHandler(this.contextItemExecute_Click);
            // 
            // contextItemCacheSettings
            // 
            this.contextItemCacheSettings.Index = 1;
            this.contextItemCacheSettings.Text = "Cache &Default Settings";
            this.contextItemCacheSettings.Click += new System.EventHandler(this.contextItemCacheSettings_Click);
            // 
            // menuItemSep01
            // 
            this.menuItemSep01.Index = 2;
            this.menuItemSep01.Text = "-";
            // 
            // contextItemAddModule
            // 
            this.contextItemAddModule.Index = 3;
            this.contextItemAddModule.Text = "Add &Module";
            this.contextItemAddModule.Click += new System.EventHandler(this.contextItemAddModule_Click);
            // 
            // contextItemAddSavedObject
            // 
            this.contextItemAddSavedObject.Index = 4;
            this.contextItemAddSavedObject.Text = "Add &Template Instance";
            this.contextItemAddSavedObject.Click += new System.EventHandler(this.contextItemAddSavedObject_Click);
            // 
            // menuItemSep02
            // 
            this.menuItemSep02.Index = 5;
            this.menuItemSep02.Text = "-";
            // 
            // contextItemEdit
            // 
            this.contextItemEdit.Index = 6;
            this.contextItemEdit.Text = "&Edit";
            this.contextItemEdit.Click += new System.EventHandler(this.contextItemEdit_Click);
            // 
            // contextItemCopy
            // 
            this.contextItemCopy.Index = 7;
            this.contextItemCopy.Text = "Make &Copy";
            this.contextItemCopy.Click += new System.EventHandler(this.contextItemCopy_Click);
            // 
            // contextItemRemove
            // 
            this.contextItemRemove.Index = 8;
            this.contextItemRemove.Text = "&Remove";
            this.contextItemRemove.Click += new System.EventHandler(this.contextItemRemove_Click);
            // 
            // menuItemSep03
            // 
            this.menuItemSep03.Index = 9;
            this.menuItemSep03.Text = "-";
            // 
            // contextItemSave
            // 
            this.contextItemSave.Index = 10;
            this.contextItemSave.Text = "&Save";
            this.contextItemSave.Click += new System.EventHandler(this.contextItemSave_Click);
            // 
            // contextItemSaveAs
            // 
            this.contextItemSaveAs.Index = 11;
            this.contextItemSaveAs.Text = "Save &As";
            this.contextItemSaveAs.Click += new System.EventHandler(this.contextItemSaveAs_Click);
            // 
            // toolTipProjectBrowser
            // 
            this.toolTipProjectBrowser.AutoPopDelay = 5000;
            this.toolTipProjectBrowser.InitialDelay = 500;
            this.toolTipProjectBrowser.ReshowDelay = 100;
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.projectToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStripMain.Size = new System.Drawing.Size(384, 24);
            this.menuStripMain.TabIndex = 38;
            this.menuStripMain.Text = "menuStrip1";
            this.menuStripMain.Visible = false;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripMenuItem7});
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
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.saveAsToolStripMenuItem.MergeIndex = 5;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.saveAsToolStripMenuItem.Text = "Save  &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.closeToolStripMenuItem.MergeIndex = 6;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripMenuItem7.MergeIndex = 7;
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(144, 6);
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executeToolStripMenuItem});
            this.projectToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.projectToolStripMenuItem.MergeIndex = 1;
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.projectToolStripMenuItem.Text = "&Project";
            // 
            // executeToolStripMenuItem
            // 
            this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
            this.executeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.executeToolStripMenuItem.Text = "E&xecute";
            this.executeToolStripMenuItem.Click += new System.EventHandler(this.executeToolStripMenuItem_Click);
            // 
            // toolStripOptions
            // 
            this.toolStripOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave,
            this.toolStripButtonSaveAs,
            this.toolStripSeparator1,
            this.toolStripButtonEdit,
            this.toolStripSeparator2,
            this.toolStripButtonExecute,
            this.toolStripSeparator3});
            this.toolStripOptions.Location = new System.Drawing.Point(0, 0);
            this.toolStripOptions.Name = "toolStripOptions";
            this.toolStripOptions.Size = new System.Drawing.Size(384, 25);
            this.toolStripOptions.TabIndex = 37;
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
            // toolStripButtonSaveAs
            // 
            this.toolStripButtonSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSaveAs.Image")));
            this.toolStripButtonSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveAs.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripButtonSaveAs.MergeIndex = 1;
            this.toolStripButtonSaveAs.Name = "toolStripButtonSaveAs";
            this.toolStripButtonSaveAs.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSaveAs.Text = "Save As";
            this.toolStripButtonSaveAs.Click += new System.EventHandler(this.toolStripButtonSaveAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripSeparator1.MergeIndex = 2;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonEdit
            // 
            this.toolStripButtonEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEdit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEdit.Image")));
            this.toolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEdit.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripButtonEdit.MergeIndex = 3;
            this.toolStripButtonEdit.Name = "toolStripButtonEdit";
            this.toolStripButtonEdit.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonEdit.Text = "Template Properties";
            this.toolStripButtonEdit.Click += new System.EventHandler(this.toolStripButtonEdit_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripSeparator2.MergeIndex = 4;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonExecute
            // 
            this.toolStripButtonExecute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExecute.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExecute.Image")));
            this.toolStripButtonExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExecute.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripButtonExecute.MergeIndex = 5;
            this.toolStripButtonExecute.Name = "toolStripButtonExecute";
            this.toolStripButtonExecute.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonExecute.Text = "Execute";
            this.toolStripButtonExecute.Click += new System.EventHandler(this.toolStripButtonExecute_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripSeparator3.MergeIndex = 6;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // ProjectBrowser
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(384, 542);
            this.Controls.Add(this.toolStripOptions);
            this.Controls.Add(this.menuStripMain);
            this.Controls.Add(this.treeViewProject);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProjectBrowser";
            this.TabText = "Project Browser";
            this.Text = "Project Browser";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.ProjectBrowser_Closing);
            this.MouseLeave += new System.EventHandler(this.ProjectBrowser_MouseLeave);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.toolStripOptions.ResumeLayout(false);
            this.toolStripOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region TreeView Event Handlers
		private void treeViewProject_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e) 
		{
			if ((e.Node is ProjectTreeNode) || (e.Node is ModuleTreeNode))
			{
				e.Node.SelectedImageIndex = INDEX_OPEN_FOLDER;
				e.Node.ImageIndex = INDEX_OPEN_FOLDER;
			}
		}

		private void treeViewProject_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e) 
		{
			if ((e.Node is ProjectTreeNode) || (e.Node is ModuleTreeNode))
			{
				e.Node.SelectedImageIndex = INDEX_CLOSED_FOLDER;
				e.Node.ImageIndex = INDEX_CLOSED_FOLDER;
			}
		}

		
		private void treeViewProject_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) 
		{
			TreeNode node = (TreeNode)treeViewProject.GetNodeAt(e.X, e.Y);
			treeViewProject.SelectedNode = node;
		}

		//TODO: Add keypress events
		private void treeViewProject_KeyDown(object sender, KeyEventArgs e) 
		{
			if (e.KeyCode == Keys.F5) 
			{
				//------------------
			}
		}

		private void treeViewProject_OnDoubleClick(object sender, System.EventArgs e) 
		{
			SortedProjectTreeNode parentnode;
			TreeNode node = this.treeViewProject.SelectedNode;
			if (node is SavedObjectTreeNode)
			{
				SavedTemplateInput input = node.Tag as SavedTemplateInput;
				ZeusModule parentMod = node.Parent.Tag as ZeusModule;
				this.formEditSavedObject.Module = parentMod;
				this.formEditSavedObject.SavedObject = input;
				if (this.formEditSavedObject.ShowDialog() == DialogResult.OK) 
				{
					this._isDirty = true;

					node.Text = input.SavedObjectName;
					parentnode = node.Parent as SortedProjectTreeNode;
					if (parentnode != null) 
					{
						node.Remove();
						parentnode.AddSorted(node);
						this.treeViewProject.SelectedNode = node;
					}
				}
			}
		}

        private object lastObject = null;

        private void treeViewProject_MouseMove(object sender, MouseEventArgs e)
		{
            object obj = treeViewProject.GetNodeAt(e.X, e.Y);
            if (object.Equals(obj, lastObject) || (obj == null && lastObject == null))
            {
                return;
            }
            else
            {
                if (obj is SavedObjectTreeNode)
                {
                    this.toolTipProjectBrowser.SetToolTip(treeViewProject, ((SavedObjectTreeNode)obj).SavedObject.SavedObjectName);
                }
                else if (obj is ProjectTreeNode)
                {
                    this.toolTipProjectBrowser.SetToolTip(treeViewProject, ((ProjectTreeNode)obj).Project.Description);
                }
                else if (obj is ModuleTreeNode)
                {
                    this.toolTipProjectBrowser.SetToolTip(treeViewProject, ((ModuleTreeNode)obj).Module.Description);
                }
                else
                {
                    this.toolTipProjectBrowser.SetToolTip(treeViewProject, string.Empty);
                }

                lastObject = obj;
            }
		}
		#endregion

        #region Main Menu Event Handlers
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveAs();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Execute();
        }
        #endregion

        #region ToolStrip Event Handlers
        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void toolStripButtonSaveAs_Click(object sender, EventArgs e)
        {

            this.SaveAs();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            TreeNode node = this.treeViewProject.SelectedNode;

            SavedTemplateInput input = node.Tag as SavedTemplateInput;
            ZeusModule parentMod = node.Parent.Tag as ZeusModule;
            this.formEditSavedObject.Module = parentMod;
            this.formEditSavedObject.SavedObject = input;
            if (this.formEditSavedObject.ShowDialog() == DialogResult.OK)
            {
                this._isDirty = true;
                node.Text = input.SavedObjectName;
                SortedProjectTreeNode parentnode = node.Parent as SortedProjectTreeNode;
                if (parentnode != null)
                {
                    node.Remove();
                    parentnode.AddSorted(node);
                }
            }
        }

        private void toolStripButtonExecute_Click(object sender, EventArgs e)
        {
            Execute();
        }
        #endregion
		
		#region ProjectBrowser Event Handlers
		private void ProjectBrowser_MouseLeave(object sender, System.EventArgs e)
		{
			this.toolTipProjectBrowser.SetToolTip(treeViewProject, string.Empty);
		}

		private void ProjectBrowser_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!this.CanClose(true)) 
			{
				e.Cancel = true;
			}
		}
		#endregion

		#region ContextMenu Event Handlers
		private void contextMenuTree_Popup(object sender, System.EventArgs e)
		{
			this.contextItemAddModule.Visible = 
				this.contextItemAddSavedObject.Visible =
				this.contextItemEdit.Visible = 
				this.contextItemExecute.Visible = 
				this.contextItemRemove.Visible = 
				this.contextItemSave.Visible =
				this.contextItemCopy.Visible =
				this.contextItemSaveAs.Visible =  
				this.contextItemCacheSettings.Visible = 
				this.menuItemSep01.Visible =
				this.menuItemSep02.Visible = 
				this.menuItemSep03.Visible = false;

			SortedProjectTreeNode node = this.treeViewProject.SelectedNode as SortedProjectTreeNode;
			if (node is ProjectTreeNode) 
			{
				this.contextItemSave.Visible = 
					this.contextItemSaveAs.Visible = 
					this.contextItemAddModule.Visible =
					this.contextItemAddSavedObject.Visible = 
					this.contextItemEdit.Visible = 
					this.contextItemExecute.Visible = 
					this.contextItemCacheSettings.Visible = true;

				this.menuItemSep01.Visible = true;
				this.menuItemSep02.Visible = true;
				this.menuItemSep03.Visible = true;
			}
			else if (node is ModuleTreeNode)
			{
				this.contextItemAddModule.Visible = 
					this.contextItemAddSavedObject.Visible = 
					this.contextItemEdit.Visible = 
					this.contextItemExecute.Visible = 
					this.contextItemRemove.Visible = 
					this.contextItemCacheSettings.Visible = true;

				this.menuItemSep01.Visible = true;
				this.menuItemSep02.Visible = true;
				this.menuItemSep03.Visible = false;
			}
			else if (node is SavedObjectTreeNode)
			{
				this.contextItemEdit.Visible =
					this.contextItemExecute.Visible =
					this.contextItemCopy.Visible =
					this.contextItemRemove.Visible = true;

				this.menuItemSep01.Visible = true;
				this.menuItemSep02.Visible = false;
				this.menuItemSep03.Visible = false;
			}
		}

		private void contextItemEdit_Click(object sender, System.EventArgs e)
		{
			SortedProjectTreeNode node = this.treeViewProject.SelectedNode as SortedProjectTreeNode;
			SortedProjectTreeNode parentnode;
			if ( (node is ModuleTreeNode) || (node is ProjectTreeNode) ) 
			{
				ZeusModule module = node.Tag as ZeusModule;
				this.formEditModule.Module = module;
				if (this.formEditModule.ShowDialog() == DialogResult.OK) 
				{
					this._isDirty = true;
					node.Text = module.Name;
					parentnode = node.Parent as SortedProjectTreeNode;
					if (parentnode != null) 
					{
						node.Remove();
						parentnode.AddSorted(node);
						this.treeViewProject.SelectedNode = node;
					}

					if (node is ProjectTreeNode) 
					{
						this.Text = "Project: " + module.Name;
						this.TabText = module.Name;
					}
				}
			}
			else if (node is SavedObjectTreeNode)
			{
				SavedTemplateInput input = node.Tag as SavedTemplateInput;
				ZeusModule parentMod = node.Parent.Tag as ZeusModule;
				this.formEditSavedObject.Module = parentMod;
				this.formEditSavedObject.SavedObject = input;
				if (this.formEditSavedObject.ShowDialog() == DialogResult.OK) 
				{
					this._isDirty = true;
					node.Text = input.SavedObjectName;
					parentnode = node.Parent as SortedProjectTreeNode;
					if (parentnode != null) 
					{
						node.Remove();
						parentnode.AddSorted(node);
						this.treeViewProject.SelectedNode = node;
					}
				}
			}
		}

		private void contextItemAddModule_Click(object sender, System.EventArgs e)
		{
			SortedProjectTreeNode node = this.treeViewProject.SelectedNode as SortedProjectTreeNode;
			if ( (node is ModuleTreeNode) || (node is ProjectTreeNode) ) 
			{
				ZeusModule module = node.Tag as ZeusModule;

				ZeusModule newmodule = new ZeusModule();

				this.formEditModule.Module = newmodule;
				if (this.formEditModule.ShowDialog() == DialogResult.OK) 
				{
					this._isDirty = true;
					module.ChildModules.Add(newmodule);

					ModuleTreeNode newNode = new ModuleTreeNode(newmodule);
					node.AddSorted(newNode);
					node.Expand();
				}
			}
		}

		private void contextItemAddSavedObject_Click(object sender, System.EventArgs e)
		{
			SortedProjectTreeNode node = this.treeViewProject.SelectedNode as SortedProjectTreeNode;
			if ( (node is ModuleTreeNode) || (node is ProjectTreeNode) ) 
			{
				ZeusModule module = node.Tag as ZeusModule;

				SavedTemplateInput savedInput = new SavedTemplateInput();
				this.formEditSavedObject.Module = module;
				this.formEditSavedObject.SavedObject = savedInput;
				if (this.formEditSavedObject.ShowDialog() == DialogResult.OK) 
				{
					this._isDirty = true;
					module.SavedObjects.Add(savedInput);

					SavedObjectTreeNode newNode = new SavedObjectTreeNode(savedInput);
					node.AddSorted(newNode);
					node.Expand();
					this.treeViewProject.SelectedNode = newNode;
				}
			}
		}

		private void contextItemCopy_Click(object sender, System.EventArgs e)
		{
			SortedProjectTreeNode node = this.treeViewProject.SelectedNode as SortedProjectTreeNode;
			if (node is SavedObjectTreeNode)
			{
				SavedTemplateInput input = node.Tag as SavedTemplateInput;
				SavedTemplateInput copy = input.Copy();

				SortedProjectTreeNode moduleNode = node.Parent as SortedProjectTreeNode;
		
				ZeusModule module = moduleNode.Tag as ZeusModule;

				string copyName = copy.SavedObjectName;
				string newName = copyName;
				int count = 1;
				bool found;
				do
				{
					found = false;
					foreach (SavedTemplateInput tmp in module.SavedObjects) 
					{
						if (tmp.SavedObjectName == newName) 
						{ 
							found = true;
							newName = copyName + " " + count++;
							break;
						}
					}
				} while (found);

				copy.SavedObjectName = newName;

				module.SavedObjects.Add(copy);

				SavedObjectTreeNode copiedNode = new SavedObjectTreeNode(copy);
				moduleNode.AddSorted(copiedNode);

				this._isDirty = true;
			}
		}

		private void contextItemCacheSettings_Click(object sender, System.EventArgs e)
		{
			SortedProjectTreeNode node = this.treeViewProject.SelectedNode as SortedProjectTreeNode;
			if ( (node is ModuleTreeNode) || (node is ProjectTreeNode) ) 
			{
				ZeusModule module = node.Tag as ZeusModule;
				ZeusContext context = new ZeusContext();
                DefaultSettings settings = DefaultSettings.Instance;
				settings.PopulateZeusContext(context);
				module.SavedItems.Add(context.Input);
			}
		}

		private void contextItemRemove_Click(object sender, System.EventArgs e)
		{
			SortedProjectTreeNode node = this.treeViewProject.SelectedNode as SortedProjectTreeNode;
			SortedProjectTreeNode parentnode;
			if (node is ModuleTreeNode)
			{
				parentnode = node.Parent as SortedProjectTreeNode;

				ZeusModule parentmodule = parentnode.Tag as ZeusModule;
				ZeusModule module = node.Tag as ZeusModule;

				parentmodule.ChildModules.Remove(module);
				parentnode.Nodes.Remove(node);
				this._isDirty = true;
			}
			else if (node is SavedObjectTreeNode)
			{
				parentnode = node.Parent as SortedProjectTreeNode;

				ZeusModule parentmodule = parentnode.Tag as ZeusModule;
				SavedTemplateInput savedobj = node.Tag as SavedTemplateInput;

				parentmodule.SavedObjects.Remove(savedobj);
				parentnode.Nodes.Remove(node);
				this._isDirty = true;
			}
		}

		private void contextItemSave_Click(object sender, System.EventArgs e)
		{
			Save();
		}

		private void contextItemSaveAs_Click(object sender, System.EventArgs e)
		{
			SaveAs();
		}

		private void contextItemExecute_Click(object sender, System.EventArgs e)
		{
			this.Execute();
		}
		#endregion

		#region Load Project Tree
		public void CreateNewProject() 
		{
			this.treeViewProject.Nodes.Clear();

			ZeusProject proj = new ZeusProject();
			proj.Name = "New Project";
			proj.Description = "New Zeus Project file";

			rootNode = new ProjectTreeNode(proj);
			rootNode.Expand();

			this.Text = "Project: " + proj.Name;
			this.TabText = proj.Name;

			this.treeViewProject.Nodes.Add(rootNode);
		}

		public void LoadProject(string filename) 
		{
			this.treeViewProject.Nodes.Clear();

			ZeusProject proj = new ZeusProject(filename);
			if (proj.Load()) 
			{
				this.Text = "Project: " + proj.Name;
				this.TabText = proj.Name;

				rootNode = new ProjectTreeNode(proj);
					
				foreach (ZeusModule module in proj.ChildModules) 
				{
					LoadModule(rootNode, module);
				}
		
				foreach (SavedTemplateInput input in proj.SavedObjects) 
				{
					rootNode.AddSorted( new SavedObjectTreeNode(input) );
				}
			}
			rootNode.Expand();
			

			this.treeViewProject.Nodes.Add(rootNode);
		}

		private void LoadModule(SortedProjectTreeNode parentNode, ZeusModule childModule) 
		{
			ModuleTreeNode childNode = new ModuleTreeNode(childModule);
			parentNode.AddSorted(childNode);

			foreach (ZeusModule grandchildModule in childModule.ChildModules) 
			{
				LoadModule(childNode, grandchildModule);
			}
		
			foreach (SavedTemplateInput input in childModule.SavedObjects) 
			{
				childNode.AddSorted( new SavedObjectTreeNode(input) );
			}
		}
		#endregion

		#region Execute, Save, SaveAs
		public void Execute() 
		{
			Cursor.Current = Cursors.WaitCursor;

			TreeNode node = this.treeViewProject.SelectedNode;
            DefaultSettings settings = DefaultSettings.Instance;

			ProjectExecuteStatus log = new ProjectExecuteStatus();
			log.Show();

			if ((node is ModuleTreeNode) || (node is ProjectTreeNode) )
			{
				ZeusModule module = node.Tag as ZeusModule;
				module.Execute(settings.ScriptTimeout, log);
			}
			else if (node is SavedObjectTreeNode)
			{
				SavedTemplateInput savedinput = node.Tag as SavedTemplateInput;
				savedinput.Execute(settings.ScriptTimeout, log);
			}

			log.Finished = true;

			Cursor.Current = Cursors.Default;
		}

		private void Save()
		{
			if (this.rootNode.Project.FilePath != null)
			{
				_isDirty = false;

				this.rootNode.Project.Save();
			}
			else 
			{
				this.SaveAs();
			}
		}

		private void SaveAs() 
		{
			Stream myStream;
			SaveFileDialog saveFileDialog = new SaveFileDialog();
       
			saveFileDialog.Filter = "Zeus Project (*.zprj)|*.zprj|All files (*.*)|*.*";
			saveFileDialog.FilterIndex = 0;
			saveFileDialog.RestoreDirectory = true;

			ZeusProject proj = this.rootNode.Project;
			if (proj.FilePath != null) 
			{
				saveFileDialog.FileName = proj.FilePath;
			}

			if(saveFileDialog.ShowDialog() == DialogResult.OK)
			{

				myStream = saveFileDialog.OpenFile();

				if(null != myStream) 
				{
					_isDirty = false;

					myStream.Close();
					proj.FilePath = saveFileDialog.FileName;
					proj.Save();
				}
			}
		}

		#endregion

        #region IMyGenDocument Members

        public string DocumentIndentity
        {
            get { return (this.FileName == string.Empty) ? this.rootNode.Project.Name : this.FileName; }
        }

        public ToolStrip ToolStrip
        {
            get { return this.toolStripOptions; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            //
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        #endregion
    }

	#region Project Browser Tree Node Classes
	public abstract class SortedProjectTreeNode : TreeNode 
	{
		public int AddSorted(TreeNode newnode) 
		{
			int insertIndex = -1;
			for (int i = 0; i < this.Nodes.Count; i++)
			{
				TreeNode node = this.Nodes[i];
				if (node.GetType() == newnode.GetType()) 
				{
					if (newnode.Text.CompareTo(node.Text) <= 0) 
					{
						insertIndex = i;
						break;
					}
				}
				else if (newnode is SavedObjectTreeNode) 
				{
					continue;
				}
				else 
				{
					insertIndex = i;
					break;
				}
			}

			if (insertIndex == -1) 
			{
				insertIndex = this.Nodes.Add(newnode);
			}
			else 
			{
				this.Nodes.Insert(insertIndex, newnode);
			}

			return insertIndex;
		}
	}

	public class ProjectTreeNode : SortedProjectTreeNode 
	{
		public ProjectTreeNode(ZeusProject proj) 
		{
			this.Tag = proj;

			this.Text = proj.Name;
			this.ImageIndex = 1;
			this.SelectedImageIndex = 1;
		}

		public ZeusProject Project
		{
			get 
			{
				return this.Tag as ZeusProject;
			}
		}
	}
	
	public class ModuleTreeNode : SortedProjectTreeNode 
	{
		public ModuleTreeNode(ZeusModule module) 
		{
			this.Tag = module;

			this.Text = module.Name;
			this.ImageIndex = 0;
			this.SelectedImageIndex = 0;
		}

		public ZeusModule Module
		{
			get 
			{
				return this.Tag as ZeusModule;
			}
		}
	}

	public class SavedObjectTreeNode : SortedProjectTreeNode 
	{
		public SavedObjectTreeNode(SavedTemplateInput templateInput)
		{
			this.Tag = templateInput;

			this.ForeColor = Color.Blue;
			this.Text = templateInput.SavedObjectName;
			this.ImageIndex = 2;
			this.SelectedImageIndex = 2;
		}

		public SavedTemplateInput SavedObject
		{
			get 
			{
				return this.Tag as SavedTemplateInput;
			}
		}
	}
	#endregion
}
