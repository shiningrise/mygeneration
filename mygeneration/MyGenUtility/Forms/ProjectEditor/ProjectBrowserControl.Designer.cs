namespace MyGeneration
{
    partial class ProjectBrowserControl
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
            this.components = new System.ComponentModel.Container();
            this.menuItemSep01 = new System.Windows.Forms.MenuItem();
            this.contextItemAddModule = new System.Windows.Forms.MenuItem();
            this.contextItemAddSavedObject = new System.Windows.Forms.MenuItem();
            this.contextItemCacheSettings = new System.Windows.Forms.MenuItem();
            this.treeViewProject = new System.Windows.Forms.TreeView();
            this.contextMenuTree = new System.Windows.Forms.ContextMenu();
            this.contextItemExecute = new System.Windows.Forms.MenuItem();
            this.menuItemSep02 = new System.Windows.Forms.MenuItem();
            this.contextItemEdit = new System.Windows.Forms.MenuItem();
            this.contextItemCopy = new System.Windows.Forms.MenuItem();
            this.contextItemRemove = new System.Windows.Forms.MenuItem();
            this.menuItemSep03 = new System.Windows.Forms.MenuItem();
            this.contextItemSave = new System.Windows.Forms.MenuItem();
            this.contextItemSaveAs = new System.Windows.Forms.MenuItem();
            this.toolTipProjectBrowser = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // menuItemSep01
            // 
            this.menuItemSep01.Index = 2;
            this.menuItemSep01.Text = "-";
            // 
            // contextItemAddModule
            // 
            this.contextItemAddModule.Index = 3;
            this.contextItemAddModule.Text = "Add &Folder";
            this.contextItemAddModule.Click += new System.EventHandler(this.contextItemAddModule_Click);
            // 
            // contextItemAddSavedObject
            // 
            this.contextItemAddSavedObject.Index = 4;
            this.contextItemAddSavedObject.Text = "Add &Template Instance";
            this.contextItemAddSavedObject.Click += new System.EventHandler(this.contextItemAddSavedObject_Click);
            // 
            // contextItemCacheSettings
            // 
            this.contextItemCacheSettings.Index = 1;
            this.contextItemCacheSettings.Text = "Cache &Default Settings";
            this.contextItemCacheSettings.Click += new System.EventHandler(this.contextItemCacheSettings_Click);
            // 
            // treeViewProject
            // 
            this.treeViewProject.ContextMenu = this.contextMenuTree;
            this.treeViewProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewProject.Location = new System.Drawing.Point(0, 0);
            this.treeViewProject.Name = "treeViewProject";
            this.treeViewProject.Size = new System.Drawing.Size(378, 674);
            this.treeViewProject.TabIndex = 39;
            this.treeViewProject.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeViewProject_AfterCollapse);
            this.treeViewProject.DoubleClick += new System.EventHandler(this.treeViewProject_DoubleClick);
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
            // 
            // contextItemExecute
            // 
            this.contextItemExecute.Index = 0;
            this.contextItemExecute.Text = "E&xecute";
            this.contextItemExecute.Click += new System.EventHandler(this.contextItemExecute_Click);
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
            // ProjectBrowserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeViewProject);
            this.Name = "ProjectBrowserControl";
            this.Size = new System.Drawing.Size(378, 674);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItemSep01;
        private System.Windows.Forms.MenuItem contextItemAddModule;
        private System.Windows.Forms.MenuItem contextItemAddSavedObject;
        private System.Windows.Forms.MenuItem contextItemCacheSettings;
        private System.Windows.Forms.TreeView treeViewProject;
        private System.Windows.Forms.ContextMenu contextMenuTree;
        private System.Windows.Forms.MenuItem contextItemExecute;
        private System.Windows.Forms.MenuItem menuItemSep02;
        private System.Windows.Forms.MenuItem contextItemEdit;
        private System.Windows.Forms.MenuItem contextItemCopy;
        private System.Windows.Forms.MenuItem contextItemRemove;
        private System.Windows.Forms.MenuItem menuItemSep03;
        private System.Windows.Forms.MenuItem contextItemSave;
        private System.Windows.Forms.MenuItem contextItemSaveAs;
        private System.Windows.Forms.ToolTip toolTipProjectBrowser;
    }
}
