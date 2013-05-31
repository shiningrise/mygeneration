namespace MyGeneration
{
    partial class TemplateBrowserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateBrowserControl));
            this.menuItemExecute = new System.Windows.Forms.MenuItem();
            this.menuItemOpen = new System.Windows.Forms.MenuItem();
            this.contextMenuTree = new System.Windows.Forms.ContextMenu();
            this.menuItemRecord = new System.Windows.Forms.MenuItem();
            this.menuItemWebUpdate = new System.Windows.Forms.MenuItem();
            this.menuItemDelete = new System.Windows.Forms.MenuItem();
            this.toolTipTemplateBrowser = new System.Windows.Forms.ToolTip(this.components);
            this.treeViewTemplates = new System.Windows.Forms.TreeView();
            this.imageListFormIcons = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMode = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonWebLibrary = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRecord = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonReplay = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonExecute = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuItemExecute
            // 
            this.menuItemExecute.Index = 0;
            this.menuItemExecute.Text = "E&xecute";
            this.menuItemExecute.Click += new System.EventHandler(this.menuItemExecute_Click);
            // 
            // menuItemOpen
            // 
            this.menuItemOpen.Index = 1;
            this.menuItemOpen.Text = "&Open";
            this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
            // 
            // menuItemRecord
            // 
            this.menuItemRecord.Index = 2;
            this.menuItemRecord.Text = "&Record Input To File";
            this.menuItemRecord.Click += new System.EventHandler(this.menuItemRecord_Click);
            // 
            // menuItemWebUpdate
            // 
            this.menuItemWebUpdate.Index = 3;
            this.menuItemWebUpdate.Text = "&Web Update";
            this.menuItemWebUpdate.Click += new System.EventHandler(this.menuItemWebUpdate_Click);
            // 
            // menuItemDelete
            // 
            this.menuItemDelete.Index = 4;
            this.menuItemDelete.Text = "&Delete";
            this.menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
            // 
            // contextMenuTree
            // 
            this.contextMenuTree.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemExecute,
            this.menuItemOpen,
            this.menuItemRecord,
            this.menuItemWebUpdate,
            this.menuItemDelete});
            this.contextMenuTree.Popup += new System.EventHandler(this.contextMenuTree_Popup);
            // 
            // treeViewTemplates
            // 
            this.treeViewTemplates.ContextMenu = this.contextMenuTree;
            this.treeViewTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewTemplates.ImageIndex = 0;
            this.treeViewTemplates.ImageList = this.imageListFormIcons;
            this.treeViewTemplates.Location = new System.Drawing.Point(0, 25);
            this.treeViewTemplates.Name = "treeViewTemplates";
            this.treeViewTemplates.SelectedImageIndex = 0;
            this.treeViewTemplates.Size = new System.Drawing.Size(392, 677);
            this.treeViewTemplates.TabIndex = 3;
            this.treeViewTemplates.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeViewTemplates_AfterCollapse);
            this.treeViewTemplates.DoubleClick += new System.EventHandler(this.treeViewTemplates_DoubleClick);
            this.treeViewTemplates.MouseMove += new System.Windows.Forms.MouseEventHandler(this.treeViewTemplates_MouseMove);
            this.treeViewTemplates.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewTemplates_KeyDown);
            this.treeViewTemplates.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeViewTemplates_AfterExpand);
            this.treeViewTemplates.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewTemplates_MouseDown);
            // 
            // imageListFormIcons
            // 
            this.imageListFormIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListFormIcons.ImageStream")));
            this.imageListFormIcons.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageListFormIcons.Images.SetKeyName(0, "");
            this.imageListFormIcons.Images.SetKeyName(1, "");
            this.imageListFormIcons.Images.SetKeyName(2, "");
            this.imageListFormIcons.Images.SetKeyName(3, "");
            this.imageListFormIcons.Images.SetKeyName(4, "");
            this.imageListFormIcons.Images.SetKeyName(5, "");
            this.imageListFormIcons.Images.SetKeyName(6, "");
            this.imageListFormIcons.Images.SetKeyName(7, "");
            this.imageListFormIcons.Images.SetKeyName(8, "");
            this.imageListFormIcons.Images.SetKeyName(9, "");
            this.imageListFormIcons.Images.SetKeyName(10, "");
            this.imageListFormIcons.Images.SetKeyName(11, "");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRefresh,
            this.toolStripButtonMode,
            this.toolStripButtonWebLibrary,
            this.toolStripSeparator1,
            this.toolStripButtonOpen,
            this.toolStripSeparator2,
            this.toolStripButtonRecord,
            this.toolStripButtonReplay,
            this.toolStripButtonExecute,
            this.toolStripSeparator3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(392, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStripTemplateBrowser";
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefresh.Image")));
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.toolStripButtonRefresh_Click);
            // 
            // toolStripButtonMode
            // 
            this.toolStripButtonMode.CheckOnClick = true;
            this.toolStripButtonMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMode.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMode.Image")));
            this.toolStripButtonMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMode.Name = "toolStripButtonMode";
            this.toolStripButtonMode.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonMode.Click += new System.EventHandler(this.toolStripButtonMode_Click);
            // 
            // toolStripButtonWebLibrary
            // 
            this.toolStripButtonWebLibrary.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonWebLibrary.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonWebLibrary.Image")));
            this.toolStripButtonWebLibrary.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonWebLibrary.Name = "toolStripButtonWebLibrary";
            this.toolStripButtonWebLibrary.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonWebLibrary.Click += new System.EventHandler(this.toolStripButtonWebLibrary_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonOpen
            // 
            this.toolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpen.Image")));
            this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpen.Name = "toolStripButtonOpen";
            this.toolStripButtonOpen.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonOpen.Visible = false;
            this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator2.Visible = false;
            // 
            // toolStripButtonRecord
            // 
            this.toolStripButtonRecord.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRecord.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRecord.Image")));
            this.toolStripButtonRecord.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRecord.Name = "toolStripButtonRecord";
            this.toolStripButtonRecord.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRecord.Visible = false;
            this.toolStripButtonRecord.Click += new System.EventHandler(this.toolStripButtonRecord_Click);
            // 
            // toolStripButtonReplay
            // 
            this.toolStripButtonReplay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReplay.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonReplay.Image")));
            this.toolStripButtonReplay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReplay.Name = "toolStripButtonReplay";
            this.toolStripButtonReplay.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonReplay.Click += new System.EventHandler(this.toolStripButtonReplay_Click);
            // 
            // toolStripButtonExecute
            // 
            this.toolStripButtonExecute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExecute.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExecute.Image")));
            this.toolStripButtonExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExecute.Name = "toolStripButtonExecute";
            this.toolStripButtonExecute.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonExecute.Visible = false;
            this.toolStripButtonExecute.Click += new System.EventHandler(this.toolStripButtonExecute_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Visible = false;
            // 
            // TemplateBrowserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeViewTemplates);
            this.Controls.Add(this.toolStrip1);
            this.Name = "TemplateBrowserControl";
            this.Size = new System.Drawing.Size(392, 702);
            this.MouseLeave += new System.EventHandler(this.TemplateBrowserControl_MouseLeave);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItemExecute;
        private System.Windows.Forms.MenuItem menuItemOpen;
        private System.Windows.Forms.ContextMenu contextMenuTree;
        private System.Windows.Forms.MenuItem menuItemWebUpdate;
        private System.Windows.Forms.MenuItem menuItemDelete;
        private System.Windows.Forms.ToolTip toolTipTemplateBrowser;
        private System.Windows.Forms.TreeView treeViewTemplates;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.ToolStripButton toolStripButtonMode;
        private System.Windows.Forms.ToolStripButton toolStripButtonWebLibrary;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonRecord;
        private System.Windows.Forms.ToolStripButton toolStripButtonReplay;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonExecute;
        private System.Windows.Forms.ImageList imageListFormIcons;
        private System.Windows.Forms.MenuItem menuItemRecord;
    }
}
