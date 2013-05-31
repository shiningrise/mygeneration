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

namespace MyGeneration
{
	/// <summary>
	/// Summary description for ProjectBrowser.
	/// </summary>
	public class ProjectBrowser : BaseWindow
	{
		private const int INDEX_CLOSED_FOLDER = 0;
		private const int INDEX_OPEN_FOLDER = 1;

		private System.Windows.Forms.ToolBar toolBarToolbar;
		private System.Windows.Forms.ImageList imageListFormIcons;
		private System.Windows.Forms.ContextMenu contextMenuTree;
		private System.Windows.Forms.TreeView treeViewProject;
		private System.Windows.Forms.ToolTip toolTipProjectBrowser;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolBarButton toolBarButtonRefresh;
		private System.Windows.Forms.ToolBarButton toolBarButtonSep1;
		private System.Windows.Forms.ToolBarButton toolBarButtonView;
		private System.Windows.Forms.ToolBarButton toolBarButtonExecute;
		private System.Windows.Forms.ToolBarButton toolBarButtonSave;
		private System.Windows.Forms.ToolBarButton toolBarButtonSep2;

		private ProjectTreeNode rootNode;

		public ProjectBrowser()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ProjectBrowser));
			this.toolBarToolbar = new System.Windows.Forms.ToolBar();
			this.toolBarButtonRefresh = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonView = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSep1 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonExecute = new System.Windows.Forms.ToolBarButton();
			this.imageListFormIcons = new System.Windows.Forms.ImageList(this.components);
			this.treeViewProject = new System.Windows.Forms.TreeView();
			this.contextMenuTree = new System.Windows.Forms.ContextMenu();
			this.toolTipProjectBrowser = new System.Windows.Forms.ToolTip(this.components);
			this.toolBarButtonSave = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSep2 = new System.Windows.Forms.ToolBarButton();
			this.SuspendLayout();
			// 
			// toolBarToolbar
			// 
			this.toolBarToolbar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBarToolbar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																							  this.toolBarButtonSave,
																							  this.toolBarButtonRefresh,
																							  this.toolBarButtonSep1,
																							  this.toolBarButtonView,
																							  this.toolBarButtonSep2,
																							  this.toolBarButtonExecute});
			this.toolBarToolbar.DropDownArrows = true;
			this.toolBarToolbar.ImageList = this.imageListFormIcons;
			this.toolBarToolbar.Location = new System.Drawing.Point(0, 0);
			this.toolBarToolbar.Name = "toolBarToolbar";
			this.toolBarToolbar.ShowToolTips = true;
			this.toolBarToolbar.Size = new System.Drawing.Size(384, 28);
			this.toolBarToolbar.TabIndex = 0;
			// 
			// toolBarButtonRefresh
			// 
			this.toolBarButtonRefresh.ImageIndex = 3;
			// 
			// toolBarButtonView
			// 
			this.toolBarButtonView.ImageIndex = 4;
			// 
			// toolBarButtonSep1
			// 
			this.toolBarButtonSep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonExecute
			// 
			this.toolBarButtonExecute.ImageIndex = 5;
			// 
			// imageListFormIcons
			// 
			this.imageListFormIcons.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListFormIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListFormIcons.ImageStream")));
			this.imageListFormIcons.TransparentColor = System.Drawing.Color.Fuchsia;
			// 
			// treeViewProject
			// 
			this.treeViewProject.ContextMenu = this.contextMenuTree;
			this.treeViewProject.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeViewProject.ImageList = this.imageListFormIcons;
			this.treeViewProject.Location = new System.Drawing.Point(0, 28);
			this.treeViewProject.Name = "treeViewProject";
			this.treeViewProject.Size = new System.Drawing.Size(384, 514);
			this.treeViewProject.TabIndex = 1;
			this.treeViewProject.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewProject_KeyDown);
			this.treeViewProject.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewProject_MouseDown);
			this.treeViewProject.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeViewProject_AfterExpand);
			this.treeViewProject.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeViewProject_AfterCollapse);
			this.treeViewProject.DoubleClick += new System.EventHandler(this.treeViewProject_OnDoubleClick);
			this.treeViewProject.MouseMove += new System.Windows.Forms.MouseEventHandler(this.treeViewProject_MouseMove);
			// 
			// toolBarButtonSave
			// 
			this.toolBarButtonSave.ImageIndex = 6;
			// 
			// toolBarButtonSep2
			// 
			this.toolBarButtonSep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// ProjectBrowser
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(384, 542);
			this.ControlBox = false;
			this.Controls.Add(this.treeViewProject);
			this.Controls.Add(this.toolBarToolbar);
			this.DockableAreas = ((WeifenLuo.WinFormsUI.DockAreas)(((((WeifenLuo.WinFormsUI.DockAreas.Float | WeifenLuo.WinFormsUI.DockAreas.DockLeft) 
				| WeifenLuo.WinFormsUI.DockAreas.DockRight) 
				| WeifenLuo.WinFormsUI.DockAreas.DockTop) 
				| WeifenLuo.WinFormsUI.DockAreas.DockBottom)));
			this.HideOnClose = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ProjectBrowser";
			this.ShowHint = WeifenLuo.WinFormsUI.DockState.DockLeft;
			this.Text = "Project Browser";
			this.ResumeLayout(false);

		}
		#endregion

		#region Load Project Tree
		public void LoadProject(string filename) 
		{
			this.treeViewProject.Nodes.Clear();

			if (rootNode == null) 
			{
				ZeusProject proj = new ZeusProject(filename);
				if (proj.Load()) 
				{
					//rootNode = new ProjectTreeNode(proj);
					LoadModule(null, proj);
				}
				rootNode.Expand();
			}

			this.treeViewProject.Nodes.Add(rootNode);
		}

		private void LoadModule(SortedProjectTreeNode parentNode, ZeusModule parentModule) 
		{
			SortedProjectTreeNode node;
			if ((parentNode == null) && (parentModule is ZeusProject)) 
			{
				rootNode = new ProjectTreeNode(parentModule as ZeusProject);
				node = rootNode;
			}
			else
			{
				node = new ModuleTreeNode(parentModule);
				parentNode.AddSorted(node);
			}
			
			foreach (ZeusModule module in parentModule.ChildModules) 
			{
				LoadModule(node, module);
			}

			foreach (SavedTemplateInput input in parentModule.SavedObjects) 
			{
				node.AddSorted( new SavedObjectTreeNode(input) );
			}
		}
		#endregion

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

		//TODO: Add double click event
		private void treeViewProject_OnDoubleClick(object sender, System.EventArgs e) 
		{
			if (this.treeViewProject.SelectedNode is SavedObjectTreeNode)
			{
				//------------------
			}
		}

		private void treeViewProject_MouseMove(object sender, MouseEventArgs e)
		{
			object obj = treeViewProject.GetNodeAt(e.X, e.Y);
			if (obj is SavedObjectTreeNode) 
			{
				//this.toolTipProjectBrowser.SetToolTip(treeViewProject, ((SavedObjectTreeNode)obj));
			}
			else
			{
				//this.toolTipProjectBrowser.SetToolTip(treeViewProject, string.Empty);
			}
		}

		/*
		#region Execute, Open, RefreshTree, ChangeMode, WebUpdate
		public void Execute() 
		{
			if (this.treeViewTemplates.SelectedNode is TemplateTreeNode)
			{
				ZeusTemplate template = new ZeusTemplate(this.treeViewTemplates.SelectedNode.Tag.ToString());
				ExecuteTemplate(template);
			}
		}

		public void SaveInput() 
		{
			if (this.treeViewTemplates.SelectedNode is TemplateTreeNode)
			{
				ZeusTemplate template = new ZeusTemplate(this.treeViewTemplates.SelectedNode.Tag.ToString());
				SaveInput(template);
			}
		}

		public void ExecuteSavedInput() 
		{
			ExecuteLoadedInput();
		}

		public void Open() 
		{
			if (this.treeViewTemplates.SelectedNode is TemplateTreeNode)
			{
				OnTemplateOpen( this.treeViewTemplates.SelectedNode.Tag.ToString() );
			}
		}

		public void RefreshTree() 
		{
			fsRootNode = null;
			nsRootNode = null;

			if (this.toolBarButtonMode.Pushed) this.LoadTemplatesByFile();
			else this.LoadTemplates();
		}
		#endregion
		*/

		private void toolBarToolbar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == this.toolBarButtonRefresh)
			{
				//this.RefreshTree();
			}
			else if (e.Button == this.toolBarButtonExecute)
			{
				//this.ChangeMode();
			}
			else if (e.Button == this.toolBarButtonView) 
			{
				//this.Open();
			}
		}

/*
		public void SaveInput(ZeusTemplate template) 
		{
			try 
			{
				DefaultSettings settings = new DefaultSettings();

				ZeusSimpleLog log = new ZeusSimpleLog();
				ZeusContext context = new ZeusContext();
				ZeusSavedInput collectedInput = new ZeusSavedInput();
				collectedInput.InputData.TemplateUniqueID = template.UniqueID;
				collectedInput.InputData.TemplatePath = template.FilePath + template.FileName;

				settings.PopulateZeusContext(context);
				template.Collect(context, settings.ScriptTimeout, collectedInput.InputData.InputItems, log);
					
				if (log.HasExceptions) 
				{
					throw log.Exceptions[0];
				}
				else 
				{
					SaveFileDialog saveFileDialog = new SaveFileDialog();
					saveFileDialog.Filter = "Zues Input Files (*.zinp)|*.zinp";
					saveFileDialog.FilterIndex = 0;
					saveFileDialog.RestoreDirectory = true;
					if(saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						Cursor.Current = Cursors.WaitCursor;

						collectedInput.FilePath = saveFileDialog.FileName;
						collectedInput.Save();
					}
				}

				MessageBox.Show(this, "Input collected and saved to file:" + "\r\n" + collectedInput.FilePath);
			}
			catch (Exception ex)
			{
				ZeusDisplayError formError = new ZeusDisplayError(ex);
				formError.SetControlsFromException();			
				formError.ShowDialog(this);
			}

			Cursor.Current = Cursors.Default;
		}

		public void ExecuteLoadedInput() 
		{
			try 
			{
				DefaultSettings settings = new DefaultSettings();
				ZeusSimpleLog log = new ZeusSimpleLog();

				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Zues Input Files (*.zinp)|*.zinp";
				openFileDialog.FilterIndex = 0;
				openFileDialog.RestoreDirectory = true;
				openFileDialog.Multiselect = true;
				if(openFileDialog.ShowDialog() == DialogResult.OK)
				{
					Cursor.Current = Cursors.WaitCursor;

					foreach (string filename in openFileDialog.FileNames) 
					{
						ZeusSavedInput savedInput = new ZeusSavedInput(filename);
						if (savedInput.Load()) 
						{
							IZeusContext context = new ZeusContext();
							context.Input.AddItems(savedInput.InputData.InputItems);

							ZeusTemplate template = new ZeusTemplate(savedInput.InputData.TemplatePath);
							template.Execute(context, settings.ScriptTimeout, log, true);

							if (log.HasExceptions) 
							{
								throw log.Exceptions[0];
							}
						}
					}

					Cursor.Current = Cursors.Default;
					MessageBox.Show(this, "Selected files have been executed.");
				}
			}
			catch (Exception ex)
			{
				ZeusDisplayError formError = new ZeusDisplayError(ex);
				formError.SetControlsFromException();			
				formError.ShowDialog(this);
			}

			Cursor.Current = Cursors.Default;
		}

		public void DynamicGUI_Display(IZeusGuiControl gui, IZeusFunctionExecutioner executioner) 
		{
			this.Cursor = Cursors.Default;

			try 
			{
				DynamicForm df = new DynamicForm(gui as GuiController, executioner);
				DialogResult result = df.ShowDialog(this);
				
				if(result == DialogResult.Cancel) 
				{
					gui.IsCanceled = true;
				}
			}
			catch (Exception ex)
			{
				ZeusDisplayError formError = new ZeusDisplayError(ex);
				formError.SetControlsFromException();			
				formError.ShowDialog(this);
			}

			Cursor.Current = Cursors.Default;
		}

		private void contextMenuTree_Popup(object sender, System.EventArgs e)
		{
			System.Drawing.Point point = this.treeViewTemplates.PointToClient(Cursor.Position);
			object node = this.treeViewTemplates.GetNodeAt(point.X, point.Y);
			
			if (node is TemplateTreeNode) 
			{
				foreach (MenuItem item in contextMenuTree.MenuItems) item.Visible = true;
			}
			else if ((node is RootTreeNode) || (node is FolderTreeNode))
			{
				this.menuItemExecute.Visible = false;
				this.menuItemOpen.Visible = false;
				this.menuItemWebUpdate.Visible = true;
			}
			else 
			{
				foreach (MenuItem item in contextMenuTree.MenuItems) item.Visible = false;
			}
		}

		private void menuItemExecute_Click(object sender, System.EventArgs e)
		{
			this.Execute();
		}

		private void menuItemOpen_Click(object sender, System.EventArgs e)
		{
			this.Open();
		}

		private void menuItemWebUpdate_Click(object sender, System.EventArgs e)
		{
			this.WebUpdate();
		}

		private void TemplateBrowser_MouseLeave(object sender, System.EventArgs e)
		{
			this.toolTipTemplateBrowser.SetToolTip(treeViewTemplates, string.Empty);
		}
		*/
	}

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
	}

	public class SavedObjectTreeNode : SortedProjectTreeNode 
	{
		public SavedObjectTreeNode(SavedTemplateInput templateInput)
		{
			this.Tag = templateInput;

			this.ForeColor = Color.Blue;
			this.Text = templateInput.SavedObjectName;
			this.ImageIndex = 6;
			this.SelectedImageIndex = 6;
		}
	}
}
