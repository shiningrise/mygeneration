using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Zeus;
using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;
using MyMeta;

namespace MyGeneration.TemplateForms
{
	/// <summary>
	/// Summary description for TemplateBrowser.
	/// </summary>
	public class TemplateBrowser : BaseWindow
	{
		private System.Windows.Forms.ToolBar toolBarToolbar;
		private System.Windows.Forms.ToolBarButton toolBarButtonOpen;
		private System.Windows.Forms.ToolBarButton toolBarSeparator2;
		private System.Windows.Forms.ToolBarButton toolBarButtonExecute;
		private System.Windows.Forms.ImageList imageListFormIcons;
		private System.Windows.Forms.TreeView treeViewTemplates;
		private System.Windows.Forms.ToolBarButton toolBarButtonRefresh;
		private System.Windows.Forms.ToolTip toolTipTemplateBrowser;
		private System.Windows.Forms.ToolBarButton toolBarButtonMode;
		private System.ComponentModel.IContainer components;

		public TemplateBrowser()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			treeViewTemplates.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewTemplates_MouseDown);
			treeViewTemplates.DoubleClick += new System.EventHandler(this.treeViewTemplates_OnDoubleClick);
			treeViewTemplates.AfterExpand += new TreeViewEventHandler(this.treeViewTemplates_AfterExpand);
			treeViewTemplates.AfterCollapse += new TreeViewEventHandler(this.treeViewTemplates_AfterCollapse);
			treeViewTemplates.KeyDown += new KeyEventHandler(this.treeViewTemplates_KeyDown);
			treeViewTemplates.MouseMove += new MouseEventHandler(treeViewTemplates_MouseMove);
			
			LoadTemplates();
			//LoadTemplatesByFile();
		}

		public event EventHandler TemplateOpen;

		protected void OnTemplateOpen(string path) 
		{
			if (this.TemplateOpen != null) 
			{
				this.TemplateOpen(path, new EventArgs());
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

		#region Load Templates By Namespace
		private void LoadTemplates() 
		{
			this.treeViewTemplates.Nodes.Clear();

			DefaultSettings settings = new DefaultSettings();

			string defaultTemplatePath = settings.DefaultTemplateDirectory;
			string exePath = Directory.GetCurrentDirectory();
			
			if (!Directory.Exists(defaultTemplatePath)) 
			{
				defaultTemplatePath = exePath;
			}

			ArrayList templates = new ArrayList();
			DirectoryInfo rootInfo = new DirectoryInfo(defaultTemplatePath);

			buildChildren(rootInfo, templates);

			RootTreeNode rootNode = new RootTreeNode("Zeus Templates by Namespace");
			this.treeViewTemplates.Nodes.Add(rootNode);

			foreach (ZeusTemplate template in templates) 
			{
				if (template.NamespacePathString.Trim() == string.Empty) 
				{
					rootNode.AddSorted(new TemplateTreeNode(template));
				}
				else 
				{
					SortedTreeNode parentNode = rootNode;

					foreach (string ns in template.NamespacePath) 
					{
						bool isFound = false;
						foreach (SortedTreeNode tmpNode in parentNode.Nodes) 
						{
							if (tmpNode.Text == ns) 
							{
								parentNode = tmpNode;
								isFound = true;
								break;
							}
						}

						if (!isFound) 
						{
							FolderTreeNode ftn = new FolderTreeNode(ns);
							parentNode.AddSorted(ftn);
							parentNode = ftn;
						}
					}

					parentNode.AddSorted(new TemplateTreeNode(template));
				}
			}

			rootNode.Expand();
		}

		protected void buildChildren(DirectoryInfo rootInfo, ArrayList templates)
		{
			ZeusTemplate template;

			foreach (DirectoryInfo dirInfo in rootInfo.GetDirectories()) 
			{
				this.buildChildren(dirInfo, templates);
			}

			foreach (FileInfo fileInfo in rootInfo.GetFiles()) 
			{
				if ( (fileInfo.Extension == ".jgen")
					|| (fileInfo.Extension == ".vbgen")
					|| (fileInfo.Extension == ".csgen")
					|| (fileInfo.Extension == ".zeus") ) 
				{
					string filename = fileInfo.FullName;

					if (!templates.Contains(filename))
					{ 
						try 
						{
							template = new ZeusTemplate(filename);
						}
						catch 
						{
							continue;
						}

						templates.Add(template);
					}
				}
			}
		}
		#endregion

		#region Load Templates By File
		private void LoadTemplatesByFile() 
		{
			this.treeViewTemplates.Nodes.Clear();

			DefaultSettings settings = new DefaultSettings();

			string defaultTemplatePath = settings.DefaultTemplateDirectory;
			string exePath = Directory.GetCurrentDirectory();
			
			if (!Directory.Exists(defaultTemplatePath)) 
			{
				defaultTemplatePath = exePath;
			}

			ArrayList templates = new ArrayList();
			DirectoryInfo rootInfo = new DirectoryInfo(defaultTemplatePath);

			RootTreeNode rootNode = new RootTreeNode("Zeus Templates by File");
			this.treeViewTemplates.Nodes.Add(rootNode);

			buildChildren(rootNode, rootInfo);

			rootNode.Expand();
		}

		protected void buildChildren(SortedTreeNode rootNode, DirectoryInfo rootInfo)
		{
			ZeusTemplate template;

			foreach (DirectoryInfo dirInfo in rootInfo.GetDirectories()) 
			{
				FolderTreeNode node = new FolderTreeNode(dirInfo.Name);
				rootNode.AddSorted(node);

				this.buildChildren(node, dirInfo);
			}

			foreach (FileInfo fileInfo in rootInfo.GetFiles()) 
			{
				if ( (fileInfo.Extension == ".jgen")
					|| (fileInfo.Extension == ".vbgen")
					|| (fileInfo.Extension == ".csgen")
					|| (fileInfo.Extension == ".zeus") ) 
				{
					string filename = fileInfo.FullName;

					try 
					{
						template = new ZeusTemplate(filename);
					}
					catch 
					{
						continue;
					}

					TemplateTreeNode node = new TemplateTreeNode(template, true);
					rootNode.AddSorted(node);

				}
			}
		}
		#endregion

		private void treeViewTemplates_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e) 
		{
			if (e.Node is FolderTreeNode)
			{
				e.Node.SelectedImageIndex += 1;
				e.Node.ImageIndex += 1;
			}
		}

		private void treeViewTemplates_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e) 
		{
			if (e.Node is FolderTreeNode)
			{
				e.Node.SelectedImageIndex -= 1;
				e.Node.ImageIndex -= 1;
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
				this.LoadTemplates();
				return;
			}
		}

		private void treeViewTemplates_OnDoubleClick(object sender, System.EventArgs e) 
		{
			if (this.treeViewTemplates.SelectedNode is TemplateTreeNode)
			{
				OnTemplateOpen( this.treeViewTemplates.SelectedNode.Tag.ToString() );
			}
		}

		private void treeViewTemplates_MouseMove(object sender, MouseEventArgs e)
		{
			object obj = treeViewTemplates.GetNodeAt(e.X, e.Y);
			if (obj is TemplateTreeNode) 
			{
				this.toolTipTemplateBrowser.SetToolTip(treeViewTemplates, ((TemplateTreeNode)obj).Comments);
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TemplateBrowser));
			this.toolBarToolbar = new System.Windows.Forms.ToolBar();
			this.toolBarButtonRefresh = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonMode = new System.Windows.Forms.ToolBarButton();
			this.toolBarSeparator2 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonOpen = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonExecute = new System.Windows.Forms.ToolBarButton();
			this.imageListFormIcons = new System.Windows.Forms.ImageList(this.components);
			this.treeViewTemplates = new System.Windows.Forms.TreeView();
			this.toolTipTemplateBrowser = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// toolBarToolbar
			// 
			this.toolBarToolbar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBarToolbar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																							  this.toolBarButtonRefresh,
																							  this.toolBarButtonMode,
																							  this.toolBarSeparator2,
																							  this.toolBarButtonOpen,
																							  this.toolBarButtonExecute});
			this.toolBarToolbar.DropDownArrows = true;
			this.toolBarToolbar.ImageList = this.imageListFormIcons;
			this.toolBarToolbar.Location = new System.Drawing.Point(0, 0);
			this.toolBarToolbar.Name = "toolBarToolbar";
			this.toolBarToolbar.ShowToolTips = true;
			this.toolBarToolbar.Size = new System.Drawing.Size(384, 28);
			this.toolBarToolbar.TabIndex = 0;
			this.toolBarToolbar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarToolbar_ButtonClick);
			// 
			// toolBarButtonRefresh
			// 
			this.toolBarButtonRefresh.ImageIndex = 2;
			this.toolBarButtonRefresh.ToolTipText = "Refresh Template Browser";
			// 
			// toolBarButtonMode
			// 
			this.toolBarButtonMode.ImageIndex = 7;
			this.toolBarButtonMode.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonMode.ToolTipText = "Browse Mode";
			// 
			// toolBarSeparator2
			// 
			this.toolBarSeparator2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonOpen
			// 
			this.toolBarButtonOpen.ImageIndex = 0;
			this.toolBarButtonOpen.ToolTipText = "Open Template";
			// 
			// toolBarButtonExecute
			// 
			this.toolBarButtonExecute.ImageIndex = 1;
			this.toolBarButtonExecute.ToolTipText = "Execute Template";
			// 
			// imageListFormIcons
			// 
			this.imageListFormIcons.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListFormIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListFormIcons.ImageStream")));
			this.imageListFormIcons.TransparentColor = System.Drawing.Color.Fuchsia;
			// 
			// treeViewTemplates
			// 
			this.treeViewTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeViewTemplates.ImageList = this.imageListFormIcons;
			this.treeViewTemplates.Location = new System.Drawing.Point(0, 28);
			this.treeViewTemplates.Name = "treeViewTemplates";
			this.treeViewTemplates.Size = new System.Drawing.Size(384, 514);
			this.treeViewTemplates.TabIndex = 1;
			// 
			// TemplateBrowser
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(384, 542);
			this.ControlBox = false;
			this.Controls.Add(this.treeViewTemplates);
			this.Controls.Add(this.toolBarToolbar);
			this.HideOnClose = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "TemplateBrowser";
			this.ShowHint = WeifenLuo.WinFormsUI.DockState.DockLeft;
			this.Text = "Template Browser";
			this.ResumeLayout(false);

		}
		#endregion

		private void toolBarToolbar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if ((e.Button == this.toolBarButtonRefresh) || (e.Button == this.toolBarButtonMode))
			{
				if (this.toolBarButtonMode.Pushed) this.LoadTemplatesByFile();
				else this.LoadTemplates();
			}
			else if (e.Button == this.toolBarButtonOpen) 
			{
				if (this.treeViewTemplates.SelectedNode is TemplateTreeNode)
				{
					OnTemplateOpen( this.treeViewTemplates.SelectedNode.Tag.ToString() );
				}
			}
			else if (e.Button == this.toolBarButtonExecute) 
			{
				if (this.treeViewTemplates.SelectedNode is TemplateTreeNode)
				{
					ZeusTemplate template = new ZeusTemplate(this.treeViewTemplates.SelectedNode.Tag.ToString());
					ExecuteTemplate(template);
				}
			}
		}


		public void ExecuteTemplate(ZeusTemplate template) 
		{
			this.Cursor = Cursors.WaitCursor;

			DefaultSettings settings = new DefaultSettings();
			
			ZeusInput zin = new ZeusInput();
			ZeusOutput zout = new ZeusOutput();
			Hashtable objects = new Hashtable();
			ZeusGuiContext guicontext;
			ZeusTemplateContext bodycontext = null;

			dbRoot myMeta = new dbRoot();

			// Set the global variables for the default template/output paths
			zin["defaultTemplatePath"] = settings.DefaultTemplateDirectory;
			zin["defaultOutputPath"] = settings.DefaultOutputDirectory;

			string driver, connectionString;

			//if there is a connection string set, it in the input section here.
			if (settings.DbDriver != string.Empty) 
			{
				driver = settings.DbDriver;
				connectionString = settings.ConnectionString;

				zin["dbDriver"] = driver;
				zin["dbConnectionString"] = connectionString;
				
				try 
				{
					// Try to connect to the DB with MyMeta (using default connection info)
					myMeta.Connect(settings.DbDriver, settings.ConnectionString);
					
					// Set DB global variables and also input variables

					if (settings.DbTarget != string.Empty)				
						myMeta.DbTarget	= settings.DbTarget;

					if (settings.DbTargetMappingFile != string.Empty)
						myMeta.DbTargetMappingFileName = settings.DbTargetMappingFile;

					if (settings.LanguageMappingFile != string.Empty)
						myMeta.LanguageMappingFileName = settings.LanguageMappingFile;

					if (settings.DbTarget != string.Empty)
						myMeta.DbTarget	= settings.DbTarget;

					if (settings.Language != string.Empty)
						myMeta.Language = settings.Language;

					if (settings.UserMetaDataFileName != string.Empty)
						myMeta.UserMetaDataFileName = settings.UserMetaDataFileName;
				}
				catch 
				{
					// Give them an empty MyMeta
					myMeta = new dbRoot();
				}
			}

			bool exceptionOccurred = false;

			bool result = false;
			try 
			{
				// Add any objects here that need to be embedded in the script.
				objects.Add("MyMeta", myMeta);

				guicontext = new ZeusGuiContext(zin, new GuiController(), objects);

				template.GuiSegment.ZeusScriptingEngine.Executioner.ScriptTimeout = settings.ScriptTimeout;
				template.GuiSegment.ZeusScriptingEngine.Executioner.SetShowGuiHandler(new ShowGUIEventHandler(DynamicGUI_Display));
				result = template.GuiSegment.Execute(guicontext); 

				if (result) 
				{
					bodycontext = new ZeusTemplateContext(guicontext);
					result = template.BodySegment.Execute(bodycontext);
				}
			}
			catch (Exception ex)
			{
				ZeusDisplayError formError = new ZeusDisplayError(ex);
				formError.SetControlsFromException();			
				formError.ShowDialog(this);

				exceptionOccurred = true;
			}

			if (!exceptionOccurred && result)
			{
				MessageBox.Show("Successfully rendered template: " + template.Title);
			}

			this.Cursor = Cursors.Default;
		}

		public void DynamicGUI_Display(GuiController gui, IZeusFunctionExecutioner executioner) 
		{
			this.Cursor = Cursors.Default;

			try 
			{
				DynamicForm df = new DynamicForm(gui, executioner);
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

			this.Cursor = Cursors.WaitCursor;
		}

	}

	public abstract class SortedTreeNode : TreeNode 
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
				else if (newnode is TemplateTreeNode) 
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

	public class RootTreeNode : SortedTreeNode 
	{
		public RootTreeNode(string title) 
		{
			//this.NodeFont = new System.Drawing.Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular, GraphicsUnit.Point);
			this.Text = title;
			this.ImageIndex = this.SelectedImageIndex = 5;
		}
	}
	
	public class FolderTreeNode : SortedTreeNode 
	{
		public FolderTreeNode(string text) 
		{
			//this.NodeFont = new System.Drawing.Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular, GraphicsUnit.Point);
			this.Text = text;
			this.ImageIndex = this.SelectedImageIndex = 3;
		}
	}

	public class TemplateTreeNode : SortedTreeNode 
	{
		public string Comments = string.Empty;

		public TemplateTreeNode(ZeusTemplate template, bool usePathAsTitle)
		{
			//this.NodeFont = new System.Drawing.Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold, GraphicsUnit.Point);
			this.ForeColor = Color.Blue;
			if (usePathAsTitle) this.Text = template.FileName;
			else this.Text = template.Title;
			this.Tag = template.FilePath + template.FileName;
			this.Comments = template.Comments;
			this.ImageIndex = this.SelectedImageIndex = 6;
		}

		public TemplateTreeNode(ZeusTemplate template)
		{
			//this.NodeFont = new System.Drawing.Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold, GraphicsUnit.Point);
			this.ForeColor = Color.Blue;
			this.Text = template.Title;
			this.Tag = template.FilePath + template.FileName;
			this.Comments = template.Comments;
			this.ImageIndex = this.SelectedImageIndex = 6;
		}
	}
}
