using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Zeus;
using Zeus.Configuration;
using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;
using MyMeta;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for TemplateTreeBuilder.
	/// </summary>
	public class TemplateTreeBuilder
	{
		private TreeView _tree;
		private RootTreeNode _fsRootNode;
		private RootTreeNode _nsRootNode;
		private RootTreeNode _wuRootNode;
		private Hashtable _idPathHash = null;
		private string defaultTemplatePath = string.Empty;
		
		public TemplateTreeBuilder(TreeView tree)
		{
			this._tree = tree;
		}

		public RootTreeNode FSRootNode { get { return _fsRootNode; } }
		public RootTreeNode NSRootNode { get { return _nsRootNode; } }
		public RootTreeNode WURootNode { get { return _wuRootNode; } }

		public Hashtable IdPathHash 
		{ 
			get 
			{ 
				if (_idPathHash == null) 
				{
					_idPathHash = new Hashtable();
				}
				return _idPathHash; 
			} 
		}
		public string DefaultTemplatePath { get { return defaultTemplatePath; } }

		public void Clear() 
		{
			_fsRootNode = null;
			_nsRootNode = null;
		}

        // for "Zeus Templates by Namespace"
		public void LoadTemplates() 
		{
			if (_tree != null) 
			{
				this._tree.Nodes.Clear();
				IdPathHash.Clear();

				if (_nsRootNode == null) 
				{
					DefaultSettings settings = DefaultSettings.Instance;
					string exePath = Directory.GetCurrentDirectory();
					try 
					{
						defaultTemplatePath = settings.DefaultTemplateDirectory;
			
						if (!Directory.Exists(defaultTemplatePath)) 
						{
							defaultTemplatePath = exePath;
						}
					}
					catch 
					{
						defaultTemplatePath = exePath;
					}

					ArrayList templates = new ArrayList();
					DirectoryInfo rootInfo = new DirectoryInfo(defaultTemplatePath);

					_buildChildren(rootInfo, templates);

#if RUN_AS_NON_ADMIN
                    string userTemplates = settings.UserTemplateDirectory;

                    if ((userTemplates != null) && (userTemplates != defaultTemplatePath))
                    {
                        rootInfo = new DirectoryInfo(userTemplates);
                        _buildChildren(rootInfo, templates);
                    }
#endif
          
					_nsRootNode = new RootTreeNode("Zeus Templates by Namespace");
					string[] nsarray;
					foreach (ZeusTemplateHeader template in templates) 
					{
						if (template.Namespace.Trim() == string.Empty) 
						{
							_nsRootNode.AddSorted(new TemplateTreeNode(template));
						}
						else 
						{
							SortedTreeNode parentNode = _nsRootNode;

							nsarray = template.Namespace.Split('.');
							foreach (string ns in nsarray) 
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

					_nsRootNode.Expand();
				}

				this._tree.Nodes.Add(_nsRootNode);
			}
		}
		
		public void LoadTemplatesFromWeb() 
		{
			if (_tree != null) 
			{
				IdPathHash.Clear();
				this._tree.Nodes.Clear();

				if (_wuRootNode == null) 
				{
                    DefaultSettings settings = DefaultSettings.Instance;

					defaultTemplatePath = settings.DefaultTemplateDirectory;
					string exePath = Directory.GetCurrentDirectory();
			
					if (!Directory.Exists(defaultTemplatePath)) 
					{
						defaultTemplatePath = exePath;
					}

					//ArrayList templates = new ArrayList();
					DirectoryInfo rootInfo = new DirectoryInfo(defaultTemplatePath);

					ArrayList templates = templates = TemplateWebUpdateHelper.GetTempates(rootInfo); 

					_wuRootNode = new RootTreeNode("Online Template Library");
					foreach (string[] template in templates) 
					{
						IdPathHash.Add(template[1].ToUpper(), template[0]);

						string fullns = template[4];
						if (fullns.Trim() == string.Empty) 
						{
							_wuRootNode.AddSorted(new TemplateTreeNode(template[0], template[1], template[2] + " (" + template[5] + ")", string.Empty, template[3], template[4], template[6]));
						}
						else 
						{
							SortedTreeNode parentNode = _wuRootNode;

							string[] x = fullns.Split('.');
							foreach (string ns in x) 
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

							parentNode.AddSorted(new TemplateTreeNode(template[0], template[1], template[2] + " [" + template[5] + "]", string.Empty, template[3], template[4], template[6]));
						}
					}

					_wuRootNode.Expand();
				}

				this._tree.Nodes.Add(_wuRootNode);
			}
		}

		public void LoadTemplatesByFile() 
		{
			if (_tree != null) 
			{
				this._tree.Nodes.Clear();
				IdPathHash.Clear();

				if (_fsRootNode == null) 
				{
                    DefaultSettings settings = DefaultSettings.Instance;

					defaultTemplatePath = settings.DefaultTemplateDirectory;
					string exePath = Directory.GetCurrentDirectory();
			
					if (!Directory.Exists(defaultTemplatePath)) 
					{
						defaultTemplatePath = exePath;
					}

					// ArrayList templates = new ArrayList();
					DirectoryInfo rootInfo = new DirectoryInfo(defaultTemplatePath);

                    _fsRootNode = new RootTreeNode("Zeus Templates by File");

#if RUN_AS_NON_ADMIN
                    SortedTreeNode item = new RootTreeNode("System");
                    _fsRootNode.Nodes.Add(item);
                    _buildChildren(item, rootInfo);
                    item.Expand();

                    string userTemplates = settings.UserTemplateDirectory;

                    if ((userTemplates != null) && (userTemplates != defaultTemplatePath))
                    {
                        rootInfo = new DirectoryInfo(userTemplates);

                        item = new RootTreeNode("User:" + Environment.UserName);
                        _fsRootNode.Nodes.Add(item);
                        _buildChildren(item, rootInfo);
                        item.Expand();
                    }
#else
					_buildChildren(_fsRootNode, rootInfo);
#endif                    
                    _fsRootNode.Expand();
				}

				this._tree.Nodes.Add(_fsRootNode);
			}
		}

        /// <summary>
        /// loads all *.xxgen and *.zeus into templates<ZeusTemplateHeader>[]
        /// updates IdPathHash<TemplateID,filename>
        /// 
        /// used by "Zeus Templates by Namespace"
        /// </summary>
		private void _buildChildren(DirectoryInfo rootInfo, ArrayList templates)
		{
			ZeusTemplateHeader template;

			foreach (DirectoryInfo dirInfo in rootInfo.GetDirectories()) 
			{
				if (dirInfo.Attributes != (FileAttributes.System | dirInfo.Attributes)) 
				{
					this._buildChildren(dirInfo, templates); // recurse into subdirectories
				}
			}

			foreach (FileInfo fileInfo in rootInfo.GetFiles()) 
			{
				if (IsTemplateFile(fileInfo)) 
				{
					string filename = fileInfo.FullName;

					if (!templates.Contains(filename))
					{ 
						try 
						{
							
							template = ZeusTemplateParser.LoadTemplateHeader(filename);
							IdPathHash.Add(template.UniqueID.ToUpper(), template.FilePath + template.FileName);
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

		private void _buildChildren(SortedTreeNode rootNode, DirectoryInfo rootInfo)
		{
			ZeusTemplateHeader template;

			foreach (DirectoryInfo dirInfo in rootInfo.GetDirectories()) 
			{
				if (dirInfo.Attributes != (FileAttributes.System | dirInfo.Attributes)) 
				{
					FolderTreeNode node = new FolderTreeNode(dirInfo.Name);
					rootNode.AddSorted(node);

					this._buildChildren(node, dirInfo);
				}
			}

			foreach (FileInfo fileInfo in rootInfo.GetFiles()) 
			{
				if (IsTemplateFile(fileInfo)) 
				{
					string filename = fileInfo.FullName;

					try 
					{
						template = ZeusTemplateParser.LoadTemplateHeader(filename);
						IdPathHash.Add(template.UniqueID.ToUpper(), template.FilePath + template.FileName);
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

        private static bool IsTemplateFile(FileInfo fileInfo)
        {
            if (fileInfo.Attributes != (FileAttributes.System | fileInfo.Attributes))
            {
                return (fileInfo.Extension == ".jgen")
                                        || (fileInfo.Extension == ".vbgen")
                                        || (fileInfo.Extension == ".csgen")
                                        || (fileInfo.Extension == ".zeus");
            }
            return false;
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
		public string UniqueId = string.Empty;
		public string Namespace = string.Empty;
		public string Url = "http://www.mygenerationsoftware.com/";
		public bool IsLocked = false;

		public TemplateTreeNode(ZeusTemplateHeader template, bool usePathAsTitle)
		{
			//this.NodeFont = new System.Drawing.Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold, GraphicsUnit.Point);
			this.ForeColor = Color.Blue;
			if (usePathAsTitle) this.Text = template.FileName;
			else this.Text = template.Title;
			this.Tag = template.FilePath + template.FileName;
			this.Comments = template.Comments;
			this.UniqueId = template.UniqueID;
			this.Namespace = template.Namespace;
			this.IsLocked = !((template.SourceType == null) || (template.SourceType == ZeusConstants.SourceTypes.SOURCE));

			if (IsLocked) this.ImageIndex = this.SelectedImageIndex = 11;
			else this.ImageIndex = this.SelectedImageIndex = 6;
		}

		public TemplateTreeNode(ZeusTemplateHeader template)
		{
			//this.NodeFont = new System.Drawing.Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold, GraphicsUnit.Point);
			this.ForeColor = Color.Blue;
			this.Text = template.Title;
			this.Tag = template.FilePath + template.FileName;
			this.Comments = template.Comments;
			this.UniqueId = template.UniqueID;
			this.Namespace = template.Namespace;
			this.IsLocked = !((template.SourceType == null) || (template.SourceType == ZeusConstants.SourceTypes.SOURCE));

			if (IsLocked) this.ImageIndex = this.SelectedImageIndex = 11;
			else this.ImageIndex = this.SelectedImageIndex = 6;
		}

		public TemplateTreeNode(string path, string id, string title, string comments, string url, string ns, string sourceType)
		{
			//this.NodeFont = new System.Drawing.Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold, GraphicsUnit.Point);
			this.ForeColor = Color.Blue;
			this.Text = title;
			this.Tag = path;
			this.Comments = comments;
			this.UniqueId = id;
			this.Url = url;
			this.Namespace = ns;
			this.IsLocked = !((sourceType == null) || (sourceType == ZeusConstants.SourceTypes.SOURCE));

			if (IsLocked) this.ImageIndex = this.SelectedImageIndex = 11;
			else this.ImageIndex = this.SelectedImageIndex = 6;

		}
	}
}
