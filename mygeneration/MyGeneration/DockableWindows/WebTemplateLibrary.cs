using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Zeus;
using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;
using MyMeta;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for WebTemplateLibrary.
	/// </summary>
	public class WebTemplateLibrary : Form
    {
        private const int INDEX_TEMPLATE = 6;
		private const int INDEX_CLOSED_FOLDER = 3;
		private const int INDEX_OPEN_FOLDER = 4;

		private System.Windows.Forms.ToolBar toolBarToolbar;
		private System.Windows.Forms.ToolBarButton toolBarSeparator2;
		private System.Windows.Forms.ImageList imageListFormIcons;
		private System.Windows.Forms.TreeView treeViewTemplates;
		private System.Windows.Forms.ToolBarButton toolBarButtonRefresh;
		private System.Windows.Forms.ToolTip toolTipTemplateBrowser;
		private System.Windows.Forms.ContextMenu contextMenuTree;
		private System.Windows.Forms.MenuItem menuItemOpen;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolBarButton toolBarButtonSaveLocal;
		private System.Windows.Forms.MenuItem menuItemSave;
		private System.Windows.Forms.ToolBarButton toolBarButtonView;

		private TemplateTreeBuilder treeBuilder;
		private Hashtable existingTemplates;
		private ArrayList updatedTemplateIDs;

		public WebTemplateLibrary(Hashtable existingTemplates)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.existingTemplates = existingTemplates;

			treeViewTemplates.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewTemplates_MouseDown);
			treeViewTemplates.DoubleClick += new System.EventHandler(this.treeViewTemplates_OnDoubleClick);
			treeViewTemplates.AfterExpand += new TreeViewEventHandler(this.treeViewTemplates_AfterExpand);
			treeViewTemplates.AfterCollapse += new TreeViewEventHandler(this.treeViewTemplates_AfterCollapse);
			treeViewTemplates.KeyDown += new KeyEventHandler(this.treeViewTemplates_KeyDown);
			treeViewTemplates.MouseMove += new MouseEventHandler(treeViewTemplates_MouseMove);
			
			treeBuilder = new TemplateTreeBuilder(treeViewTemplates);

			treeBuilder.LoadTemplatesFromWeb();
		}

		public ArrayList UpdatedTemplateIDs
		{
			get 
			{
				if (updatedTemplateIDs == null) updatedTemplateIDs = new ArrayList();
				return updatedTemplateIDs;
			}
		}

		private void View() 
		{
			TemplateTreeNode node = this.treeViewTemplates.SelectedNode as TemplateTreeNode;
			if (node != null) 
			{
				try
                {
                    Program.LaunchBrowser(node.Url);
				}
				catch 
				{
					Help.ShowHelp(this, node.Url);
				}
			}
		}

		private void Save() 
		{
			TreeNode node = this.treeViewTemplates.SelectedNode as TreeNode;
			if(node != null)
			{
                if (node.Parent == null)
                {
                    MessageBox.Show("Cannot Save from the Root Level", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Save(node, false);
                    Cursor.Current = Cursors.Default;
                }
            }
		}


		private void Save(TreeNode node, bool isGrouped)
		{
			if ( node is TemplateTreeNode )
			{
				TemplateTreeNode tnode = node as TemplateTreeNode;

				string path = tnode.Tag.ToString();
				string id = tnode.UniqueId.ToUpper();
				if (this.existingTemplates.Contains( tnode.UniqueId.ToUpper() )) 
				{
					path = existingTemplates[id].ToString();
				}

				if (!UpdatedTemplateIDs.Contains(tnode.UniqueId)) 
				{
					UpdatedTemplateIDs.Add(tnode.UniqueId);
				}
				TemplateWebUpdateHelper.WebUpdate(tnode.UniqueId, path, isGrouped);
				
			}
			else if (node != null)
			{
				foreach (TreeNode child in node.Nodes) 
				{
					Save(child, true);
				}
			}
		}

		public void RefreshTree() 
		{
			treeBuilder.Clear();
			this.treeBuilder.LoadTemplatesFromWeb();
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

		private void treeViewTemplates_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e) 
		{
			if (e.Node is FolderTreeNode)
			{
				e.Node.SelectedImageIndex = INDEX_OPEN_FOLDER;
				e.Node.ImageIndex = INDEX_OPEN_FOLDER;
			}
		}

		private void treeViewTemplates_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e) 
		{
			if (e.Node is FolderTreeNode)
			{
				e.Node.SelectedImageIndex = INDEX_CLOSED_FOLDER;
				e.Node.ImageIndex = INDEX_CLOSED_FOLDER;
			}
		}

		
		private void treeViewTemplates_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) 
		{
			TreeNode node = (TreeNode)treeViewTemplates.GetNodeAt(e.X, e.Y);
			treeViewTemplates.SelectedNode = node;
		}

		private void treeViewTemplates_KeyDown(object sender, KeyEventArgs e) 
		{
			if (e.KeyCode == Keys.F5) 
			{
				this.treeBuilder.Clear();
				this.treeBuilder.LoadTemplatesFromWeb();
				return;
			}
		}

		private void treeViewTemplates_OnDoubleClick(object sender, System.EventArgs e) 
		{
			View();
		}

		private void treeViewTemplates_MouseMove(object sender, MouseEventArgs e)
		{
			object obj = treeViewTemplates.GetNodeAt(e.X, e.Y);
			if (obj is TemplateTreeNode) 
			{
				this.toolTipTemplateBrowser.SetToolTip(treeViewTemplates, string.Empty);
			}
			else if ((obj is RootTreeNode) && (DateTime.Now.Hour == 1))
			{
				this.toolTipTemplateBrowser.SetToolTip(treeViewTemplates, "Worship me as I generate your code.");
			}
			else
			{
				this.toolTipTemplateBrowser.SetToolTip(treeViewTemplates, string.Empty);
			}
		}

		private void toolBarToolbar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == this.toolBarButtonRefresh)
			{
				this.RefreshTree();
			}
			else if (e.Button == this.toolBarButtonView) 
			{
				this.View();
			}
			else if (e.Button == this.toolBarButtonSaveLocal) 
			{
				this.Save();
			}
		}

		private void contextMenuTree_Popup(object sender, System.EventArgs e)
		{
			System.Drawing.Point point = this.treeViewTemplates.PointToClient(Cursor.Position);
			object node = this.treeViewTemplates.GetNodeAt(point.X, point.Y);
			
			if (node is TemplateTreeNode) 
			{
				foreach (MenuItem item in contextMenuTree.MenuItems) item.Visible = true;
			}
			else if (node is FolderTreeNode)
			{
				this.menuItemOpen.Visible = false;
				this.menuItemSave.Visible = true;
			}
			else if (node is RootTreeNode)
			{
				this.menuItemOpen.Visible = false;
				this.menuItemSave.Visible = false;
			}
			else 
			{
				foreach (MenuItem item in contextMenuTree.MenuItems) item.Visible = false;
			}
		}

		private void TemplateBrowser_MouseLeave(object sender, System.EventArgs e)
		{
			this.toolTipTemplateBrowser.SetToolTip(treeViewTemplates, string.Empty);
		}

		private void menuItemSave_Click(object sender, System.EventArgs e)
		{
			this.Save();
		}

		private void menuItemView_Click(object sender, System.EventArgs e)
		{
			this.View();
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebTemplateLibrary));
            this.toolBarToolbar = new System.Windows.Forms.ToolBar();
            this.toolBarButtonRefresh = new System.Windows.Forms.ToolBarButton();
            this.toolBarSeparator2 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonView = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonSaveLocal = new System.Windows.Forms.ToolBarButton();
            this.imageListFormIcons = new System.Windows.Forms.ImageList(this.components);
            this.treeViewTemplates = new System.Windows.Forms.TreeView();
            this.contextMenuTree = new System.Windows.Forms.ContextMenu();
            this.menuItemOpen = new System.Windows.Forms.MenuItem();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.toolTipTemplateBrowser = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // toolBarToolbar
            // 
            this.toolBarToolbar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBarToolbar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonRefresh,
            this.toolBarSeparator2,
            this.toolBarButtonView,
            this.toolBarButtonSaveLocal});
            this.toolBarToolbar.DropDownArrows = true;
            this.toolBarToolbar.ImageList = this.imageListFormIcons;
            this.toolBarToolbar.Location = new System.Drawing.Point(0, 0);
            this.toolBarToolbar.Name = "toolBarToolbar";
            this.toolBarToolbar.ShowToolTips = true;
            this.toolBarToolbar.Size = new System.Drawing.Size(464, 28);
            this.toolBarToolbar.TabIndex = 0;
            this.toolBarToolbar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarToolbar_ButtonClick);
            // 
            // toolBarButtonRefresh
            // 
            this.toolBarButtonRefresh.ImageIndex = 2;
            this.toolBarButtonRefresh.Name = "toolBarButtonRefresh";
            this.toolBarButtonRefresh.ToolTipText = "Refresh Template Browser";
            // 
            // toolBarSeparator2
            // 
            this.toolBarSeparator2.Name = "toolBarSeparator2";
            this.toolBarSeparator2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButtonView
            // 
            this.toolBarButtonView.ImageIndex = 7;
            this.toolBarButtonView.Name = "toolBarButtonView";
            this.toolBarButtonView.ToolTipText = "View Template Information";
            // 
            // toolBarButtonSaveLocal
            // 
            this.toolBarButtonSaveLocal.ImageIndex = 10;
            this.toolBarButtonSaveLocal.Name = "toolBarButtonSaveLocal";
            this.toolBarButtonSaveLocal.ToolTipText = "Save template to the local hard drive.";
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
            this.imageListFormIcons.Images.SetKeyName(11, "icon_template_locked.bmp");
            // 
            // treeViewTemplates
            // 
            this.treeViewTemplates.ContextMenu = this.contextMenuTree;
            this.treeViewTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewTemplates.ImageIndex = 0;
            this.treeViewTemplates.ImageList = this.imageListFormIcons;
            this.treeViewTemplates.Location = new System.Drawing.Point(0, 28);
            this.treeViewTemplates.Name = "treeViewTemplates";
            this.treeViewTemplates.SelectedImageIndex = 0;
            this.treeViewTemplates.Size = new System.Drawing.Size(464, 338);
            this.treeViewTemplates.TabIndex = 1;
            // 
            // contextMenuTree
            // 
            this.contextMenuTree.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemOpen,
            this.menuItemSave});
            this.contextMenuTree.Popup += new System.EventHandler(this.contextMenuTree_Popup);
            // 
            // menuItemOpen
            // 
            this.menuItemOpen.Index = 0;
            this.menuItemOpen.Text = "&View";
            this.menuItemOpen.Click += new System.EventHandler(this.menuItemView_Click);
            // 
            // menuItemSave
            // 
            this.menuItemSave.Index = 1;
            this.menuItemSave.Text = "&Save";
            this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // WebTemplateLibrary
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(464, 366);
            this.Controls.Add(this.treeViewTemplates);
            this.Controls.Add(this.toolBarToolbar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WebTemplateLibrary";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Online Template Library";
            this.MouseLeave += new System.EventHandler(this.TemplateBrowser_MouseLeave);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

	}

}
