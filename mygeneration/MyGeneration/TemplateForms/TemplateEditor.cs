using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Reflection;

using Zeus;
using Zeus.ErrorHandling;
using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;

using MyMeta;
using WeifenLuo.WinFormsUI.Docking;
using Scintilla;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for TemplateProperties.
	/// </summary>
    public class TemplateEditor : DockContent, IScintillaEditControl, ILogger
	{
        #region gui elements
        public const string FILE_TYPES = "JScript Templates (*.jgen)|*.jgen|VBScript Templates (*.vbgen)|*.vbgen|C# Templates (*.csgen)|*.csgen|Zeus Templates (*.zeus)|*.zeus|All files (*.*)|*.*";
        public const int DEFAULT_OPEN_FILE_TYPE_INDEX = 5;
        public const int DEFAULT_SAVE_FILE_TYPE_INDEX = 4;

        private IMyGenerationMDI mdi;

        private ZeusScintillaControl scintillaTemplateCode = null;
		private ZeusScintillaControl scintillaGUICode = null;
		private ZeusScintillaControl scintillaTemplateSource = null;
		private ZeusScintillaControl scintillaGuiSource = null;
		private ZeusScintillaControl scintillaOutput = null;
		
		private System.Windows.Forms.TabPage tabTemplateCode;
		private System.Windows.Forms.TabPage tabInterfaceCode;
		private System.Windows.Forms.TabPage tabTemplateSource;
        private System.Windows.Forms.TabPage tabOutput;
		private System.Windows.Forms.Panel panelConsole;
		private System.Windows.Forms.Panel panelProperties;
		private NJFLib.Controls.CollapsibleSplitter splitterProperties;
		private System.Windows.Forms.TabControl tabControlTemplate;
		private System.Windows.Forms.TextBox textBoxConsole;
		private System.Windows.Forms.Label labelComments;
		private System.Windows.Forms.TextBox textBoxComments;
		private System.Windows.Forms.GroupBox groupBoxScripting;
		private System.Windows.Forms.Label labelEndTag;
		private System.Windows.Forms.Label labelStartTag;
		private System.Windows.Forms.TextBox textBoxStartTag;
		private System.Windows.Forms.TextBox textBoxShortcutTag;
		private System.Windows.Forms.TextBox textBoxEndTag;
		private System.Windows.Forms.ComboBox comboBoxLanguage;
		private System.Windows.Forms.Label labelLanguage;
        private NJFLib.Controls.CollapsibleSplitter splitterConsole;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button buttonSelectFile;
		private System.Windows.Forms.Label labelUniqueID;
        private System.Windows.Forms.TextBox textBoxUniqueID;
        private OpenFileDialog openFileDialog = new OpenFileDialog();
		private System.Windows.Forms.Label labelShortcutTag;
		private System.Windows.Forms.Label labelMode;
        private System.Windows.Forms.ComboBox comboBoxMode;
        private System.Windows.Forms.ListBox listBoxIncludedTemplateFiles;

		private System.Windows.Forms.ComboBox comboBoxOutputLanguage;
		private System.Windows.Forms.Label labelOutputLanguage;
		private System.Windows.Forms.Label labelGroup;
		private System.Windows.Forms.TextBox textBoxGroup;
		private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TextBox textBoxTitle;
		private System.Windows.Forms.ComboBox comboBoxEngine;
        private System.Windows.Forms.Label labelEngine;

		protected bool _isDirty = false;
		private System.Windows.Forms.ComboBox comboBoxGuiEngine;
		private System.Windows.Forms.Label labelGuiEngine;
		private System.Windows.Forms.ComboBox comboBoxGuiLanguage;
		private System.Windows.Forms.Label labelGuiLanguage;
		private System.Windows.Forms.ComboBox comboBoxType;
		private System.Windows.Forms.Label labelType;
		private System.Windows.Forms.Label labelIncludedTemplates;
		private System.Windows.Forms.TabPage tabInterfaceSource;
        private System.Windows.Forms.Button buttonNewGuid;
        private ToolStrip toolStripOptions;
        private ToolStripButton toolStripButtonSave;
        private ToolStripButton toolStripButtonSaveAs;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripButtonProperties;
        private ToolStripButton toolStripButtonConsole;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton toolStripButtonExecute;
        private ToolStripSeparator toolStripSeparator3;
        private MenuStrip menuStripMain;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem templateToolStripMenuItem;
        private ToolStripMenuItem executeToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem encryptAsToolStripMenuItem;
        private ToolStripMenuItem compileAsToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem lineNumbersToolStripMenuItem;
        private ToolStripMenuItem whitespaceToolStripMenuItem;
        private ToolStripMenuItem indentationGuidsToolStripMenuItem;
        private ToolStripMenuItem endOfLineToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem enlargeFontToolStripMenuItem;
        private ToolStripMenuItem reduceFontToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripMenuItem copyOutputToClipboardToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem findToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripMenuItem replaceHiddenToolStripMenuItem;
        private ToolStripMenuItem findNextToolStripMenuItem;
        private ToolStripMenuItem findPrevToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripMenuItem goToLineToolStripMenuItem;
		private ZeusTemplate _template;
        #endregion

		public TemplateEditor(IMyGenerationMDI mdi)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            this.mdi = mdi;
			            
            // Create Scintilla Code controls for editing the template
            scintillaTemplateCode = new ZeusScintillaControl();
            scintillaTemplateCode.AddShortcuts(this.menuStripMain.Items);
            scintillaTemplateCode.InitializeFindReplace();

			scintillaGUICode = new ZeusScintillaControl();
            scintillaGUICode.AddShortcuts(this.menuStripMain.Items);

			scintillaTemplateSource = new ZeusScintillaControl();
            scintillaTemplateSource.AddShortcuts(this.menuStripMain.Items);

			scintillaGuiSource = new ZeusScintillaControl();
            scintillaGuiSource.AddShortcuts(this.menuStripMain.Items);

			scintillaOutput = new ZeusScintillaControl();
            scintillaOutput.AddShortcuts(this.menuStripMain.Items);

            DefaultSettings settings = DefaultSettings.Instance;
            this.SetCodePageOverride(settings.CodePage);
            this.SetFontOverride(settings.FontFamily);

            this.tabTemplateCode.Controls.Add(this.scintillaTemplateCode);
			this.tabInterfaceCode.Controls.Add(this.scintillaGUICode);
			this.tabTemplateSource.Controls.Add(this.scintillaTemplateSource);
			this.tabInterfaceSource.Controls.Add(this.scintillaGuiSource);
			this.tabOutput.Controls.Add(this.scintillaOutput);

			this.lineNumbersToolStripMenuItem.Checked = false;
			if (settings.EnableLineNumbering) 
			{
				lineNumbersToolStripMenuItem_Click(null, new EventArgs());
			}

			copyOutputToClipboardToolStripMenuItem.Checked = false;
			if (settings.EnableClipboard) 
			{
				copyOutputToClipboardToolStripMenuItem_Click(null, new EventArgs());
			}

			//LoseFocus IsDirty Handlers
			this.textBoxComments.Leave += new EventHandler(this.CheckPropertiesDirty);

			// Keep the Status bar up to date
			this.scintillaTemplateCode.UpdateUI += new EventHandler<UpdateUIEventArgs>(UpdateUI);
            this.scintillaTemplateSource.UpdateUI += new EventHandler<UpdateUIEventArgs>(UpdateUI);
            this.scintillaGUICode.UpdateUI += new EventHandler<UpdateUIEventArgs>(UpdateUI);
            this.scintillaGuiSource.UpdateUI += new EventHandler<UpdateUIEventArgs>(UpdateUI);
            this.scintillaOutput.UpdateUI += new EventHandler<UpdateUIEventArgs>(UpdateUI);

			//this.KeyDown += new KeyEventHandler(TemplateEditor_KeyDown);
		}

		protected override string GetPersistString()
		{
            if (this.IsNew)
            {
                string langType = string.Empty;
                switch (this._template.BodySegment.Language)
                {
                    case ZeusConstants.Languages.CSHARP:
                        langType = TemplateEditorManager.CSHARP_TEMPLATE;
                        break;
                    case ZeusConstants.Languages.VBNET:
                        langType = TemplateEditorManager.VBNET_TEMPLATE;
                        break;
                    case ZeusConstants.Languages.VBSCRIPT:
                        langType = TemplateEditorManager.VBSCRIPT_TEMPLATE;
                        break;
                    case ZeusConstants.Languages.JSCRIPT:
                    default:
                        langType = TemplateEditorManager.JSCRIPT_TEMPLATE;
                        break;
                }

                return "type," + langType;
            }
            else
            {
                return "file," + this.FileName;
            }
		}

		public IEditControl CurrentEditControl 
		{
			get { return CurrentScintilla; }
		}

		protected ZeusScintillaControl CurrentScintilla 
		{
			get 
			{
				switch (this.tabControlTemplate.SelectedIndex)
				{
					case 1:
						return this.scintillaGUICode;
					case 2:
						return this.scintillaTemplateSource;
					case 3:
						return this.scintillaGuiSource;
					case 4:
						return this.scintillaOutput;
					default:
						return this.scintillaTemplateCode;
				}
			}
		}

		private void UpdateUI(object sender, UpdateUIEventArgs args)
		{
            ScintillaControl scintilla = sender as ScintillaControl;

            int caretPos = scintilla.CurrentPos;
			int line = scintilla.LineFromPosition(caretPos);

			//mdi.StatusText = "Line: "   + (scintilla.LineFromPosition(caretPos) + 1).ToString() + " " + 
			//  "Column: " + (scintilla.Column(caretPos) + 1).ToString(); 
		}

		protected void CheckPropertiesDirty(object sender, EventArgs args) 
		{
			if (!this._isDirty) 
			{
				if ((this._template.UniqueID != this.textBoxUniqueID.Text) ||
					(this._template.Title != this.textBoxTitle.Text) ||
					(this._template.Comments != this.textBoxComments.Text) ||
					(this._template.TagEnd != this.textBoxEndTag.Text))
				{
					this._isDirty = true;
				}
			}
        }

        private void SetCodePageOverride(int codePage)
        {
            this.scintillaGUICode.CodePageOverride = codePage;
            this.scintillaGuiSource.CodePageOverride = codePage;
            this.scintillaOutput.CodePageOverride = codePage;
            this.scintillaTemplateCode.CodePageOverride = codePage;
            this.scintillaTemplateSource.CodePageOverride = codePage;
        }

        private void SetFontOverride(string family)
        {
            this.scintillaGUICode.FontFamilyOverride = family;
            this.scintillaGuiSource.FontFamilyOverride = family;
            this.scintillaOutput.FontFamilyOverride = family;
            this.scintillaTemplateCode.FontFamilyOverride = family;
            this.scintillaTemplateSource.FontFamilyOverride = family;
        }

		public bool CanClose(bool allowPrevent)
		{
			return PromptForSave(allowPrevent);
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

		public bool IsDirty 
		{
			get 
			{
				if (this.scintillaTemplateCode.IsModify || this.scintillaGUICode.IsModify)
					this._isDirty = true;

				return this._isDirty;
			}
		}

		private string[] PickFiles() 
		{
			openFileDialog.InitialDirectory = this._template.FilePath;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.Multiselect = true;
			openFileDialog.Filter = TemplateEditor.FILE_TYPES;
			openFileDialog.FilterIndex = TemplateEditor.DEFAULT_OPEN_FILE_TYPE_INDEX;
       
			if(openFileDialog.ShowDialog() == DialogResult.OK)
			{
				return openFileDialog.FileNames;
			}
			else 
			{
				return new string[] {};
			}
		}

		public bool IsNew 
		{
			get 
			{
				return (this._template.FilePath == string.Empty);
			}
		}

		public void Log(string input) 
		{
			if (input != null)
				this.textBoxConsole.AppendText(input);

			this.textBoxConsole.Refresh();
		}

		public void LogLn(string input) 
		{
			if (input != null)
				this.textBoxConsole.AppendText(DateTime.Now.ToString() + " - " + input + System.Environment.NewLine);

			this.textBoxConsole.Refresh();
		}

		private void Log_EntryAdded(object sender, EventArgs args) 
		{
			if (sender != null) 
				this.LogLn(sender.ToString());
		}

		private void ZeusDisplayError_ErrorIndexChanged(object source, EventArgs args) 
		{
			ZeusDisplayError formError = source as ZeusDisplayError;

			ZeusScintillaControl ctrl = (formError.LastErrorIsTemplate ? scintillaTemplateSource : scintillaGuiSource);
			TabPage tab = (formError.LastErrorIsTemplate ? tabTemplateSource : tabInterfaceSource);
			this.tabControlTemplate.SelectedTab = tab;

			if (formError.LastErrorIsScript)
			{
				try 
				{
					if (formError.LastErrorFileName == this.FileName)
					{
						int lineNumber = formError.LastErrorLineNumber - (formError.LastErrorLineNumber > 0 ? 1 : 0);

						ctrl.GrabFocus();
						ctrl.GotoLine(lineNumber);
						ctrl.EnsureVisibleEnforcePolicy(lineNumber);
						ctrl.SelectionStart = ctrl.CurrentPos;
						ctrl.SelectionEnd   = ctrl.CurrentPos + ctrl.GetCurLine().Length - 1;
					}
				}
				catch {}
			}
		}

		public void HandleExecuteException(Exception ex) 
		{
			this.scintillaOutput.IsReadOnly = false;
			this.scintillaOutput.Text = string.Empty;
			this.scintillaOutput.IsReadOnly = true;

			ZeusDisplayError formError = new ZeusDisplayError(ex);
			formError.ErrorIndexChange += new EventHandler(ZeusDisplayError_ErrorIndexChanged);
			formError.ShowDialog(this);
			
			foreach (string message in formError.LastErrorMessages) 
			{
				LogLn(message);
			}
		}

		public void FileNew(params object[] options) 
		{
			this._Initialize(string.Empty, options);
			SetClean();
		}

		public void FileOpen(string path)
		{
			if (File.Exists(path)) 
			{
				this._Initialize(path);
				SetClean();
			}
		}

		public void FileSave()
		{
			this.RefreshTemplateFromControl();

			if (this._template.FileName != string.Empty) 
			{
				string path = this._template.FilePath + this._template.FileName;
				FileInfo fi = new FileInfo(path);
                if (fi.Exists && fi.IsReadOnly)
                {
                    MessageBox.Show(this, "File is read only.");
                }
                else
                {
                    try
                    {
                        _template.Save(path);
                        SetClean();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Error saving file. " + ex.Message);
                    }

                }
			}

		}

		public void FileSaveAs(string path)
		{
			bool isopen = mdi.IsDocumentOpen(path, this);
			
			if (!isopen) 
			{
				this.RefreshTemplateFromControl();

                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                {
                    if (fi.IsReadOnly)
                    {
                        MessageBox.Show(this, "File is read only.");
                    }
                    else
                    {
                        try
                        {
                            _template.Save(path);
                            string dir = Path.GetDirectoryName(path);
                            if (!dir.EndsWith("\\")) dir += "\\";

                            this._template.FilePath = dir;
                            this._template.FileName = Path.GetFileName(path);

                            this.RefreshControlFromTemplate();
                            SetClean();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "Error saving file. " + ex.Message);
                        }
                    }
                }
			}
			else 
			{
                MessageBox.Show(this, "The template you are trying to overwrite is currently open.\r\nClose the editor window for that template if you want to overwrite it.");
			}
		}

		protected void SetClean()
        {

            scintillaTemplateCode.EmptyUndoBuffer();
            scintillaTemplateCode.SetSavePoint();
            scintillaGUICode.EmptyUndoBuffer();
			scintillaGUICode.SetSavePoint();
			this._isDirty = false;
		}

		public string FileName 
		{
			get 
			{
				if ((_template != null) && (_template.FileName != string.Empty))
				{
					return _template.FilePath + _template.FileName;
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

		public string UniqueID
		{
			get 
			{
				return this.textBoxUniqueID.Text;
			}
		}

		public string Title
		{
			get 
			{
				return this.textBoxTitle.Text;
			}
		}

		private bool listBoxContainsItem(System.Windows.Forms.ListBox l, String i) 
		{
			foreach (FileListItem item in l.Items) 
			{
				if (item.CompareTo(i) == 0)
					return true;
			}
			return false;
		}

		private bool PromptForSave(bool allowPrevent)
		{
			bool canClose = true;

			if(this.IsDirty)
			{
				DialogResult result;

				if(allowPrevent)
				{
					result = MessageBox.Show("This template has been modified, Do you wish to save before closing?", 
						this._template.FileName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				}
				else
				{
					result = MessageBox.Show("This template has been modified, Do you wish to save before closing?", 
						this._template.FileName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				}

				switch(result)
				{
					case DialogResult.Yes:
						this._Save();
						break;
					case DialogResult.Cancel:
						canClose = false;
						break;
				}
			}

			return canClose;
		}

		private void FillLanguageDropdown() 
		{
			this.comboBoxOutputLanguage.Items.Clear();
			foreach (string lang in ZeusFactory.AvailableLanguages)
			{
				this.comboBoxOutputLanguage.Items.Add(lang);
			}
		}

		private void FillBodyLanguageDropdown() 
		{
			this.comboBoxLanguage.Items.Clear();
			foreach (string lang in _template.BodySegment.ZeusScriptingEngine.SupportedLanguages)
			{
				this.comboBoxLanguage.Items.Add(lang);
			}
			this.comboBoxLanguage.SelectedIndex = -1;
		}

		private void FillGuiLanguageDropdown() 
		{
			this.comboBoxGuiLanguage.Items.Clear();
			foreach (string lang in _template.GuiSegment.ZeusScriptingEngine.SupportedLanguages)
			{
				this.comboBoxGuiLanguage.Items.Add(lang);
			}
			this.comboBoxGuiLanguage.SelectedIndex = -1;
		}

		private void FillModeDropdown() 
		{
			this.comboBoxMode.Items.Clear();
			foreach (string mode in ZeusFactory.TemplateModes)
			{
				this.comboBoxMode.Items.Add(mode);
			}
		}

		private void FillTypeDropdown() 
		{
			this.comboBoxType.Items.Clear();
			foreach (string type in ZeusFactory.TemplateTypes)
			{
				this.comboBoxType.Items.Add(type);
			}
		}

		private void FillEngineDropdowns() 
		{
			this.comboBoxEngine.Items.Clear();
			this.comboBoxGuiEngine.Items.Clear();
			foreach (string engine in ZeusFactory.EngineNames)
			{
				this.comboBoxEngine.Items.Add(engine);
				this.comboBoxGuiEngine.Items.Add(engine);
			}
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateEditor));
            this.panelConsole = new System.Windows.Forms.Panel();
            this.textBoxConsole = new System.Windows.Forms.TextBox();
            this.splitterConsole = new NJFLib.Controls.CollapsibleSplitter();
            this.panelProperties = new System.Windows.Forms.Panel();
            this.buttonNewGuid = new System.Windows.Forms.Button();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.labelType = new System.Windows.Forms.Label();
            this.comboBoxGuiEngine = new System.Windows.Forms.ComboBox();
            this.labelGuiEngine = new System.Windows.Forms.Label();
            this.comboBoxGuiLanguage = new System.Windows.Forms.ComboBox();
            this.labelGuiLanguage = new System.Windows.Forms.Label();
            this.comboBoxEngine = new System.Windows.Forms.ComboBox();
            this.labelEngine = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.comboBoxOutputLanguage = new System.Windows.Forms.ComboBox();
            this.labelOutputLanguage = new System.Windows.Forms.Label();
            this.listBoxIncludedTemplateFiles = new System.Windows.Forms.ListBox();
            this.labelUniqueID = new System.Windows.Forms.Label();
            this.textBoxUniqueID = new System.Windows.Forms.TextBox();
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.labelIncludedTemplates = new System.Windows.Forms.Label();
            this.groupBoxScripting = new System.Windows.Forms.GroupBox();
            this.labelEndTag = new System.Windows.Forms.Label();
            this.labelShortcutTag = new System.Windows.Forms.Label();
            this.labelStartTag = new System.Windows.Forms.Label();
            this.textBoxStartTag = new System.Windows.Forms.TextBox();
            this.textBoxShortcutTag = new System.Windows.Forms.TextBox();
            this.textBoxEndTag = new System.Windows.Forms.TextBox();
            this.labelComments = new System.Windows.Forms.Label();
            this.textBoxComments = new System.Windows.Forms.TextBox();
            this.labelGroup = new System.Windows.Forms.Label();
            this.textBoxGroup = new System.Windows.Forms.TextBox();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.labelLanguage = new System.Windows.Forms.Label();
            this.labelMode = new System.Windows.Forms.Label();
            this.splitterProperties = new NJFLib.Controls.CollapsibleSplitter();
            this.tabControlTemplate = new System.Windows.Forms.TabControl();
            this.tabTemplateCode = new System.Windows.Forms.TabPage();
            this.tabInterfaceCode = new System.Windows.Forms.TabPage();
            this.tabTemplateSource = new System.Windows.Forms.TabPage();
            this.tabInterfaceSource = new System.Windows.Forms.TabPage();
            this.tabOutput = new System.Windows.Forms.TabPage();
            this.toolStripOptions = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonProperties = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonConsole = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonExecute = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceHiddenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findNextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findPrevToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.goToLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.templateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.encryptAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.lineNumbersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whitespaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indentationGuidsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.endOfLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.enlargeFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reduceFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.copyOutputToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelConsole.SuspendLayout();
            this.panelProperties.SuspendLayout();
            this.groupBoxScripting.SuspendLayout();
            this.tabControlTemplate.SuspendLayout();
            this.toolStripOptions.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelConsole
            // 
            this.panelConsole.Controls.Add(this.textBoxConsole);
            this.panelConsole.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelConsole.Location = new System.Drawing.Point(0, 601);
            this.panelConsole.Name = "panelConsole";
            this.panelConsole.Size = new System.Drawing.Size(900, 88);
            this.panelConsole.TabIndex = 0;
            this.panelConsole.Visible = false;
            // 
            // textBoxConsole
            // 
            this.textBoxConsole.BackColor = System.Drawing.Color.Black;
            this.textBoxConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxConsole.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxConsole.ForeColor = System.Drawing.Color.Lime;
            this.textBoxConsole.Location = new System.Drawing.Point(0, 0);
            this.textBoxConsole.MaxLength = 9999999;
            this.textBoxConsole.Multiline = true;
            this.textBoxConsole.Name = "textBoxConsole";
            this.textBoxConsole.ReadOnly = true;
            this.textBoxConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxConsole.Size = new System.Drawing.Size(900, 88);
            this.textBoxConsole.TabIndex = 0;
            this.textBoxConsole.WordWrap = false;
            // 
            // splitterConsole
            // 
            this.splitterConsole.AnimationDelay = 20;
            this.splitterConsole.AnimationStep = 20;
            this.splitterConsole.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
            this.splitterConsole.ControlToHide = this.panelConsole;
            this.splitterConsole.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterConsole.ExpandParentForm = false;
            this.splitterConsole.Location = new System.Drawing.Point(0, 593);
            this.splitterConsole.Name = "splitterConsole";
            this.splitterConsole.TabIndex = 5;
            this.splitterConsole.TabStop = false;
            this.splitterConsole.UseAnimations = false;
            this.splitterConsole.VisualStyle = NJFLib.Controls.VisualStyles.Mozilla;
            // 
            // panelProperties
            // 
            this.panelProperties.AutoScroll = true;
            this.panelProperties.Controls.Add(this.buttonNewGuid);
            this.panelProperties.Controls.Add(this.comboBoxType);
            this.panelProperties.Controls.Add(this.labelType);
            this.panelProperties.Controls.Add(this.comboBoxGuiEngine);
            this.panelProperties.Controls.Add(this.labelGuiEngine);
            this.panelProperties.Controls.Add(this.comboBoxGuiLanguage);
            this.panelProperties.Controls.Add(this.labelGuiLanguage);
            this.panelProperties.Controls.Add(this.comboBoxEngine);
            this.panelProperties.Controls.Add(this.labelEngine);
            this.panelProperties.Controls.Add(this.labelTitle);
            this.panelProperties.Controls.Add(this.textBoxTitle);
            this.panelProperties.Controls.Add(this.comboBoxOutputLanguage);
            this.panelProperties.Controls.Add(this.labelOutputLanguage);
            this.panelProperties.Controls.Add(this.listBoxIncludedTemplateFiles);
            this.panelProperties.Controls.Add(this.labelUniqueID);
            this.panelProperties.Controls.Add(this.textBoxUniqueID);
            this.panelProperties.Controls.Add(this.buttonSelectFile);
            this.panelProperties.Controls.Add(this.labelIncludedTemplates);
            this.panelProperties.Controls.Add(this.groupBoxScripting);
            this.panelProperties.Controls.Add(this.labelComments);
            this.panelProperties.Controls.Add(this.textBoxComments);
            this.panelProperties.Controls.Add(this.labelGroup);
            this.panelProperties.Controls.Add(this.textBoxGroup);
            this.panelProperties.Controls.Add(this.comboBoxLanguage);
            this.panelProperties.Controls.Add(this.comboBoxMode);
            this.panelProperties.Controls.Add(this.labelLanguage);
            this.panelProperties.Controls.Add(this.labelMode);
            this.panelProperties.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelProperties.Location = new System.Drawing.Point(0, 0);
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.Size = new System.Drawing.Size(304, 593);
            this.panelProperties.TabIndex = 3;
            // 
            // buttonNewGuid
            // 
            this.buttonNewGuid.Location = new System.Drawing.Point(216, 24);
            this.buttonNewGuid.Name = "buttonNewGuid";
            this.buttonNewGuid.Size = new System.Drawing.Size(64, 23);
            this.buttonNewGuid.TabIndex = 6;
            this.buttonNewGuid.Text = "New Guid";
            this.buttonNewGuid.Click += new System.EventHandler(this.buttonNewGuid_Click);
            // 
            // comboBoxType
            // 
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.Location = new System.Drawing.Point(8, 144);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(128, 21);
            this.comboBoxType.TabIndex = 15;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(8, 128);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(112, 16);
            this.labelType.TabIndex = 36;
            this.labelType.Text = "Type:";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // comboBoxGuiEngine
            // 
            this.comboBoxGuiEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGuiEngine.Location = new System.Drawing.Point(152, 184);
            this.comboBoxGuiEngine.Name = "comboBoxGuiEngine";
            this.comboBoxGuiEngine.Size = new System.Drawing.Size(128, 21);
            this.comboBoxGuiEngine.TabIndex = 27;
            this.comboBoxGuiEngine.SelectedIndexChanged += new System.EventHandler(this.comboBoxGuiEngine_SelectedIndexChanged);
            // 
            // labelGuiEngine
            // 
            this.labelGuiEngine.Location = new System.Drawing.Point(152, 168);
            this.labelGuiEngine.Name = "labelGuiEngine";
            this.labelGuiEngine.Size = new System.Drawing.Size(128, 16);
            this.labelGuiEngine.TabIndex = 34;
            this.labelGuiEngine.Text = "Gui Scripting Engine:";
            this.labelGuiEngine.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // comboBoxGuiLanguage
            // 
            this.comboBoxGuiLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGuiLanguage.Location = new System.Drawing.Point(152, 224);
            this.comboBoxGuiLanguage.Name = "comboBoxGuiLanguage";
            this.comboBoxGuiLanguage.Size = new System.Drawing.Size(128, 21);
            this.comboBoxGuiLanguage.TabIndex = 30;
            this.comboBoxGuiLanguage.SelectedIndexChanged += new System.EventHandler(this.comboBoxGuiLanguage_SelectedIndexChanged);
            // 
            // labelGuiLanguage
            // 
            this.labelGuiLanguage.Location = new System.Drawing.Point(152, 208);
            this.labelGuiLanguage.Name = "labelGuiLanguage";
            this.labelGuiLanguage.Size = new System.Drawing.Size(128, 16);
            this.labelGuiLanguage.TabIndex = 32;
            this.labelGuiLanguage.Text = "Gui Language:";
            this.labelGuiLanguage.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // comboBoxEngine
            // 
            this.comboBoxEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEngine.Location = new System.Drawing.Point(8, 184);
            this.comboBoxEngine.Name = "comboBoxEngine";
            this.comboBoxEngine.Size = new System.Drawing.Size(128, 21);
            this.comboBoxEngine.TabIndex = 18;
            this.comboBoxEngine.SelectedIndexChanged += new System.EventHandler(this.comboBoxEngine_SelectedIndexChanged);
            // 
            // labelEngine
            // 
            this.labelEngine.Location = new System.Drawing.Point(8, 168);
            this.labelEngine.Name = "labelEngine";
            this.labelEngine.Size = new System.Drawing.Size(144, 16);
            this.labelEngine.TabIndex = 30;
            this.labelEngine.Text = "Template Scripting Engine:";
            this.labelEngine.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelTitle
            // 
            this.labelTitle.Location = new System.Drawing.Point(8, 48);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(208, 16);
            this.labelTitle.TabIndex = 29;
            this.labelTitle.Text = "Title:";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(8, 64);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(272, 20);
            this.textBoxTitle.TabIndex = 9;
            // 
            // comboBoxOutputLanguage
            // 
            this.comboBoxOutputLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOutputLanguage.Location = new System.Drawing.Point(152, 272);
            this.comboBoxOutputLanguage.Name = "comboBoxOutputLanguage";
            this.comboBoxOutputLanguage.Size = new System.Drawing.Size(128, 21);
            this.comboBoxOutputLanguage.Sorted = true;
            this.comboBoxOutputLanguage.TabIndex = 39;
            this.comboBoxOutputLanguage.SelectedIndexChanged += new System.EventHandler(this.comboBoxOutputLanguage_SelectedIndexChanged);
            // 
            // labelOutputLanguage
            // 
            this.labelOutputLanguage.Location = new System.Drawing.Point(152, 256);
            this.labelOutputLanguage.Name = "labelOutputLanguage";
            this.labelOutputLanguage.Size = new System.Drawing.Size(112, 16);
            this.labelOutputLanguage.TabIndex = 26;
            this.labelOutputLanguage.Text = "Output Language:";
            this.labelOutputLanguage.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listBoxIncludedTemplateFiles
            // 
            this.listBoxIncludedTemplateFiles.Location = new System.Drawing.Point(8, 472);
            this.listBoxIncludedTemplateFiles.Name = "listBoxIncludedTemplateFiles";
            this.listBoxIncludedTemplateFiles.Size = new System.Drawing.Size(248, 82);
            this.listBoxIncludedTemplateFiles.TabIndex = 45;
            this.listBoxIncludedTemplateFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxIncludedTemplateFiles_KeyDown);
            // 
            // labelUniqueID
            // 
            this.labelUniqueID.Location = new System.Drawing.Point(8, 8);
            this.labelUniqueID.Name = "labelUniqueID";
            this.labelUniqueID.Size = new System.Drawing.Size(208, 16);
            this.labelUniqueID.TabIndex = 21;
            this.labelUniqueID.Text = "Unique ID:";
            this.labelUniqueID.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxUniqueID
            // 
            this.textBoxUniqueID.Location = new System.Drawing.Point(8, 24);
            this.textBoxUniqueID.Name = "textBoxUniqueID";
            this.textBoxUniqueID.Size = new System.Drawing.Size(208, 20);
            this.textBoxUniqueID.TabIndex = 5;
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.Location = new System.Drawing.Point(256, 472);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(24, 24);
            this.buttonSelectFile.TabIndex = 48;
            this.buttonSelectFile.Text = "...";
            this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            // 
            // labelIncludedTemplates
            // 
            this.labelIncludedTemplates.Location = new System.Drawing.Point(8, 448);
            this.labelIncludedTemplates.Name = "labelIncludedTemplates";
            this.labelIncludedTemplates.Size = new System.Drawing.Size(208, 16);
            this.labelIncludedTemplates.TabIndex = 14;
            this.labelIncludedTemplates.Text = "Included Template Scripts:";
            this.labelIncludedTemplates.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // groupBoxScripting
            // 
            this.groupBoxScripting.Controls.Add(this.labelEndTag);
            this.groupBoxScripting.Controls.Add(this.labelShortcutTag);
            this.groupBoxScripting.Controls.Add(this.labelStartTag);
            this.groupBoxScripting.Controls.Add(this.textBoxStartTag);
            this.groupBoxScripting.Controls.Add(this.textBoxShortcutTag);
            this.groupBoxScripting.Controls.Add(this.textBoxEndTag);
            this.groupBoxScripting.Location = new System.Drawing.Point(8, 256);
            this.groupBoxScripting.Name = "groupBoxScripting";
            this.groupBoxScripting.Size = new System.Drawing.Size(128, 96);
            this.groupBoxScripting.TabIndex = 32;
            this.groupBoxScripting.TabStop = false;
            this.groupBoxScripting.Text = "Dynamic Tags";
            // 
            // labelEndTag
            // 
            this.labelEndTag.Location = new System.Drawing.Point(8, 40);
            this.labelEndTag.Name = "labelEndTag";
            this.labelEndTag.Size = new System.Drawing.Size(56, 23);
            this.labelEndTag.TabIndex = 19;
            this.labelEndTag.Text = "End:";
            this.labelEndTag.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelShortcutTag
            // 
            this.labelShortcutTag.Location = new System.Drawing.Point(8, 64);
            this.labelShortcutTag.Name = "labelShortcutTag";
            this.labelShortcutTag.Size = new System.Drawing.Size(56, 23);
            this.labelShortcutTag.TabIndex = 18;
            this.labelShortcutTag.Text = "Shortcut:";
            this.labelShortcutTag.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelStartTag
            // 
            this.labelStartTag.Location = new System.Drawing.Point(8, 16);
            this.labelStartTag.Name = "labelStartTag";
            this.labelStartTag.Size = new System.Drawing.Size(56, 23);
            this.labelStartTag.TabIndex = 17;
            this.labelStartTag.Text = "Start:";
            this.labelStartTag.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxStartTag
            // 
            this.textBoxStartTag.Location = new System.Drawing.Point(64, 16);
            this.textBoxStartTag.Name = "textBoxStartTag";
            this.textBoxStartTag.Size = new System.Drawing.Size(40, 20);
            this.textBoxStartTag.TabIndex = 3;
            this.textBoxStartTag.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxStartTag_KeyUp);
            // 
            // textBoxShortcutTag
            // 
            this.textBoxShortcutTag.Location = new System.Drawing.Point(64, 64);
            this.textBoxShortcutTag.Name = "textBoxShortcutTag";
            this.textBoxShortcutTag.ReadOnly = true;
            this.textBoxShortcutTag.Size = new System.Drawing.Size(40, 20);
            this.textBoxShortcutTag.TabIndex = 9;
            // 
            // textBoxEndTag
            // 
            this.textBoxEndTag.Location = new System.Drawing.Point(64, 40);
            this.textBoxEndTag.Name = "textBoxEndTag";
            this.textBoxEndTag.Size = new System.Drawing.Size(40, 20);
            this.textBoxEndTag.TabIndex = 6;
            // 
            // labelComments
            // 
            this.labelComments.Location = new System.Drawing.Point(8, 352);
            this.labelComments.Name = "labelComments";
            this.labelComments.Size = new System.Drawing.Size(208, 16);
            this.labelComments.TabIndex = 5;
            this.labelComments.Text = "Comments:";
            this.labelComments.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxComments
            // 
            this.textBoxComments.Location = new System.Drawing.Point(8, 368);
            this.textBoxComments.Multiline = true;
            this.textBoxComments.Name = "textBoxComments";
            this.textBoxComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxComments.Size = new System.Drawing.Size(272, 72);
            this.textBoxComments.TabIndex = 42;
            // 
            // labelGroup
            // 
            this.labelGroup.Location = new System.Drawing.Point(8, 88);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(208, 16);
            this.labelGroup.TabIndex = 1;
            this.labelGroup.Text = "Namespace:";
            this.labelGroup.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxGroup
            // 
            this.textBoxGroup.Location = new System.Drawing.Point(8, 104);
            this.textBoxGroup.Name = "textBoxGroup";
            this.textBoxGroup.Size = new System.Drawing.Size(272, 20);
            this.textBoxGroup.TabIndex = 12;
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLanguage.Location = new System.Drawing.Point(8, 224);
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.comboBoxLanguage.Size = new System.Drawing.Size(128, 21);
            this.comboBoxLanguage.TabIndex = 21;
            this.comboBoxLanguage.SelectedIndexChanged += new System.EventHandler(this.comboBoxLanguage_SelectedIndexChanged);
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMode.Location = new System.Drawing.Point(152, 144);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(128, 21);
            this.comboBoxMode.TabIndex = 24;
            this.comboBoxMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxMode_SelectedIndexChanged);
            // 
            // labelLanguage
            // 
            this.labelLanguage.Location = new System.Drawing.Point(8, 208);
            this.labelLanguage.Name = "labelLanguage";
            this.labelLanguage.Size = new System.Drawing.Size(128, 16);
            this.labelLanguage.TabIndex = 12;
            this.labelLanguage.Text = "Template Language:";
            this.labelLanguage.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelMode
            // 
            this.labelMode.Location = new System.Drawing.Point(152, 128);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(112, 16);
            this.labelMode.TabIndex = 22;
            this.labelMode.Text = "Mode:";
            this.labelMode.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // splitterProperties
            // 
            this.splitterProperties.AnimationDelay = 20;
            this.splitterProperties.AnimationStep = 20;
            this.splitterProperties.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
            this.splitterProperties.ControlToHide = this.panelProperties;
            this.splitterProperties.ExpandParentForm = false;
            this.splitterProperties.Location = new System.Drawing.Point(304, 0);
            this.splitterProperties.Name = "splitterProperties";
            this.splitterProperties.TabIndex = 1;
            this.splitterProperties.TabStop = false;
            this.splitterProperties.UseAnimations = false;
            this.splitterProperties.VisualStyle = NJFLib.Controls.VisualStyles.Mozilla;
            // 
            // tabControlTemplate
            // 
            this.tabControlTemplate.Controls.Add(this.tabTemplateCode);
            this.tabControlTemplate.Controls.Add(this.tabInterfaceCode);
            this.tabControlTemplate.Controls.Add(this.tabTemplateSource);
            this.tabControlTemplate.Controls.Add(this.tabInterfaceSource);
            this.tabControlTemplate.Controls.Add(this.tabOutput);
            this.tabControlTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlTemplate.Location = new System.Drawing.Point(312, 0);
            this.tabControlTemplate.Name = "tabControlTemplate";
            this.tabControlTemplate.SelectedIndex = 0;
            this.tabControlTemplate.Size = new System.Drawing.Size(588, 593);
            this.tabControlTemplate.TabIndex = 2;
            this.tabControlTemplate.SelectedIndexChanged += new System.EventHandler(this.tabControlTemplate_SelectedIndexChanged);
            // 
            // tabTemplateCode
            // 
            this.tabTemplateCode.BackColor = System.Drawing.Color.Transparent;
            this.tabTemplateCode.Location = new System.Drawing.Point(4, 22);
            this.tabTemplateCode.Name = "tabTemplateCode";
            this.tabTemplateCode.Size = new System.Drawing.Size(580, 567);
            this.tabTemplateCode.TabIndex = 0;
            this.tabTemplateCode.Text = "Template Code";
            this.tabTemplateCode.UseVisualStyleBackColor = true;
            // 
            // tabInterfaceCode
            // 
            this.tabInterfaceCode.BackColor = System.Drawing.Color.Transparent;
            this.tabInterfaceCode.Location = new System.Drawing.Point(4, 22);
            this.tabInterfaceCode.Name = "tabInterfaceCode";
            this.tabInterfaceCode.Size = new System.Drawing.Size(580, 567);
            this.tabInterfaceCode.TabIndex = 1;
            this.tabInterfaceCode.Text = "Interface Code";
            this.tabInterfaceCode.UseVisualStyleBackColor = true;
            // 
            // tabTemplateSource
            // 
            this.tabTemplateSource.Location = new System.Drawing.Point(4, 22);
            this.tabTemplateSource.Name = "tabTemplateSource";
            this.tabTemplateSource.Size = new System.Drawing.Size(580, 567);
            this.tabTemplateSource.TabIndex = 2;
            this.tabTemplateSource.Text = "Template Source";
            this.tabTemplateSource.UseVisualStyleBackColor = true;
            // 
            // tabInterfaceSource
            // 
            this.tabInterfaceSource.Location = new System.Drawing.Point(4, 22);
            this.tabInterfaceSource.Name = "tabInterfaceSource";
            this.tabInterfaceSource.Size = new System.Drawing.Size(580, 567);
            this.tabInterfaceSource.TabIndex = 4;
            this.tabInterfaceSource.Text = "Interface Source";
            this.tabInterfaceSource.UseVisualStyleBackColor = true;
            // 
            // tabOutput
            // 
            this.tabOutput.Location = new System.Drawing.Point(4, 22);
            this.tabOutput.Name = "tabOutput";
            this.tabOutput.Size = new System.Drawing.Size(580, 567);
            this.tabOutput.TabIndex = 3;
            this.tabOutput.Text = "Output";
            this.tabOutput.UseVisualStyleBackColor = true;
            // 
            // toolStripOptions
            // 
            this.toolStripOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave,
            this.toolStripButtonSaveAs,
            this.toolStripSeparator1,
            this.toolStripButtonProperties,
            this.toolStripButtonConsole,
            this.toolStripSeparator2,
            this.toolStripButtonExecute,
            this.toolStripSeparator3});
            this.toolStripOptions.Location = new System.Drawing.Point(312, 0);
            this.toolStripOptions.Name = "toolStripOptions";
            this.toolStripOptions.Size = new System.Drawing.Size(588, 25);
            this.toolStripOptions.TabIndex = 35;
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
            // toolStripButtonProperties
            // 
            this.toolStripButtonProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProperties.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonProperties.Image")));
            this.toolStripButtonProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProperties.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripButtonProperties.MergeIndex = 3;
            this.toolStripButtonProperties.Name = "toolStripButtonProperties";
            this.toolStripButtonProperties.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonProperties.Text = "Template Properties";
            this.toolStripButtonProperties.Click += new System.EventHandler(this.toolStripButtonProperties_Click);
            // 
            // toolStripButtonConsole
            // 
            this.toolStripButtonConsole.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonConsole.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonConsole.Image")));
            this.toolStripButtonConsole.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonConsole.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripButtonConsole.MergeIndex = 4;
            this.toolStripButtonConsole.Name = "toolStripButtonConsole";
            this.toolStripButtonConsole.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonConsole.Text = "Console";
            this.toolStripButtonConsole.Click += new System.EventHandler(this.toolStripButtonConsole_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripSeparator2.MergeIndex = 5;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonExecute
            // 
            this.toolStripButtonExecute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExecute.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExecute.Image")));
            this.toolStripButtonExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExecute.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripButtonExecute.MergeIndex = 6;
            this.toolStripButtonExecute.Name = "toolStripButtonExecute";
            this.toolStripButtonExecute.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonExecute.Text = "Execute";
            this.toolStripButtonExecute.Click += new System.EventHandler(this.toolStripButtonExecute_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripSeparator3.MergeIndex = 7;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.templateToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(312, 25);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStripMain.Size = new System.Drawing.Size(588, 24);
            this.menuStripMain.TabIndex = 36;
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
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripMenuItem4,
            this.findToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.replaceHiddenToolStripMenuItem,
            this.findNextToolStripMenuItem,
            this.findPrevToolStripMenuItem,
            this.toolStripMenuItem6,
            this.goToLineToolStripMenuItem});
            this.editToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.editToolStripMenuItem.MergeIndex = 1;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.cutToolStripMenuItem.Text = "Cu&t";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(192, 6);
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.findToolStripMenuItem.Text = "&Find";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.replaceToolStripMenuItem.Text = "&Replace";
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // replaceHiddenToolStripMenuItem
            // 
            this.replaceHiddenToolStripMenuItem.Name = "replaceHiddenToolStripMenuItem";
            this.replaceHiddenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.replaceHiddenToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.replaceHiddenToolStripMenuItem.Text = "Replace&Hidden";
            this.replaceHiddenToolStripMenuItem.Visible = false;
            this.replaceHiddenToolStripMenuItem.Click += new System.EventHandler(this.replaceHiddenToolStripMenuItem_Click);
            // 
            // findNextToolStripMenuItem
            // 
            this.findNextToolStripMenuItem.Name = "findNextToolStripMenuItem";
            this.findNextToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.findNextToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.findNextToolStripMenuItem.Text = "Find &Next";
            this.findNextToolStripMenuItem.Click += new System.EventHandler(this.findNextToolStripMenuItem_Click);
            // 
            // findPrevToolStripMenuItem
            // 
            this.findPrevToolStripMenuItem.Name = "findPrevToolStripMenuItem";
            this.findPrevToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F3)));
            this.findPrevToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.findPrevToolStripMenuItem.Text = "Find &Previous";
            this.findPrevToolStripMenuItem.Click += new System.EventHandler(this.findPrevToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(192, 6);
            // 
            // goToLineToolStripMenuItem
            // 
            this.goToLineToolStripMenuItem.Name = "goToLineToolStripMenuItem";
            this.goToLineToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.goToLineToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.goToLineToolStripMenuItem.Text = "Go to Line ...";
            // 
            // templateToolStripMenuItem
            // 
            this.templateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executeToolStripMenuItem,
            this.toolStripMenuItem1,
            this.encryptAsToolStripMenuItem,
            this.compileAsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.lineNumbersToolStripMenuItem,
            this.whitespaceToolStripMenuItem,
            this.indentationGuidsToolStripMenuItem,
            this.endOfLineToolStripMenuItem,
            this.toolStripMenuItem3,
            this.enlargeFontToolStripMenuItem,
            this.reduceFontToolStripMenuItem,
            this.toolStripMenuItem5,
            this.copyOutputToClipboardToolStripMenuItem});
            this.templateToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.templateToolStripMenuItem.MergeIndex = 2;
            this.templateToolStripMenuItem.Name = "templateToolStripMenuItem";
            this.templateToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.templateToolStripMenuItem.Text = "&Template";
            // 
            // executeToolStripMenuItem
            // 
            this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
            this.executeToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.executeToolStripMenuItem.Text = "E&xecute";
            this.executeToolStripMenuItem.Click += new System.EventHandler(this.executeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(207, 6);
            // 
            // encryptAsToolStripMenuItem
            // 
            this.encryptAsToolStripMenuItem.Name = "encryptAsToolStripMenuItem";
            this.encryptAsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.encryptAsToolStripMenuItem.Text = "Encr&ypt As ...";
            this.encryptAsToolStripMenuItem.Click += new System.EventHandler(this.encryptAsToolStripMenuItem_Click);
            // 
            // compileAsToolStripMenuItem
            // 
            this.compileAsToolStripMenuItem.Name = "compileAsToolStripMenuItem";
            this.compileAsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.compileAsToolStripMenuItem.Text = "&Compile As ...";
            this.compileAsToolStripMenuItem.Click += new System.EventHandler(this.compileAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(207, 6);
            // 
            // lineNumbersToolStripMenuItem
            // 
            this.lineNumbersToolStripMenuItem.Name = "lineNumbersToolStripMenuItem";
            this.lineNumbersToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.lineNumbersToolStripMenuItem.Text = "Line &Numbers";
            this.lineNumbersToolStripMenuItem.Click += new System.EventHandler(this.lineNumbersToolStripMenuItem_Click);
            // 
            // whitespaceToolStripMenuItem
            // 
            this.whitespaceToolStripMenuItem.Name = "whitespaceToolStripMenuItem";
            this.whitespaceToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.whitespaceToolStripMenuItem.Text = "Whitespace";
            this.whitespaceToolStripMenuItem.Click += new System.EventHandler(this.whitespaceToolStripMenuItem_Click);
            // 
            // indentationGuidsToolStripMenuItem
            // 
            this.indentationGuidsToolStripMenuItem.Name = "indentationGuidsToolStripMenuItem";
            this.indentationGuidsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.indentationGuidsToolStripMenuItem.Text = "Indentation Guids";
            this.indentationGuidsToolStripMenuItem.Click += new System.EventHandler(this.indentationGuidsToolStripMenuItem_Click);
            // 
            // endOfLineToolStripMenuItem
            // 
            this.endOfLineToolStripMenuItem.Name = "endOfLineToolStripMenuItem";
            this.endOfLineToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.endOfLineToolStripMenuItem.Text = "End of Line";
            this.endOfLineToolStripMenuItem.Click += new System.EventHandler(this.endOfLineToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(207, 6);
            // 
            // enlargeFontToolStripMenuItem
            // 
            this.enlargeFontToolStripMenuItem.Name = "enlargeFontToolStripMenuItem";
            this.enlargeFontToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.enlargeFontToolStripMenuItem.Text = "Enlarge Font";
            this.enlargeFontToolStripMenuItem.Click += new System.EventHandler(this.enlargeFontToolStripMenuItem_Click);
            // 
            // reduceFontToolStripMenuItem
            // 
            this.reduceFontToolStripMenuItem.Name = "reduceFontToolStripMenuItem";
            this.reduceFontToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.reduceFontToolStripMenuItem.Text = "Reduce Font";
            this.reduceFontToolStripMenuItem.Click += new System.EventHandler(this.reduceFontToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(207, 6);
            // 
            // copyOutputToClipboardToolStripMenuItem
            // 
            this.copyOutputToClipboardToolStripMenuItem.Name = "copyOutputToClipboardToolStripMenuItem";
            this.copyOutputToClipboardToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.copyOutputToClipboardToolStripMenuItem.Text = "Copy &Output To Clipboard";
            this.copyOutputToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyOutputToClipboardToolStripMenuItem_Click);
            // 
            // TemplateEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(900, 689);
            this.Controls.Add(this.menuStripMain);
            this.Controls.Add(this.toolStripOptions);
            this.Controls.Add(this.tabControlTemplate);
            this.Controls.Add(this.splitterProperties);
            this.Controls.Add(this.panelProperties);
            this.Controls.Add(this.splitterConsole);
            this.Controls.Add(this.panelConsole);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TemplateEditor";
            this.TabText = "Template Editor";
            this.Text = "Template Editor";
            this.Activated += new System.EventHandler(this.TemplateEditor_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TemplateEditor_Closing);
            this.DockStateChanged += new System.EventHandler(this.TemplateEditor_DockStateChanged);
            this.Load += new System.EventHandler(this.TemplateEditor_Load);
            this.panelConsole.ResumeLayout(false);
            this.panelConsole.PerformLayout();
            this.panelProperties.ResumeLayout(false);
            this.panelProperties.PerformLayout();
            this.groupBoxScripting.ResumeLayout(false);
            this.groupBoxScripting.PerformLayout();
            this.tabControlTemplate.ResumeLayout(false);
            this.toolStripOptions.ResumeLayout(false);
            this.toolStripOptions.PerformLayout();
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Populating the Control from the Template and vise-versa
        private void RefreshControlFromTemplate()
        {
            if (this.comboBoxEngine.Items.Count == 0)
            {
                this.FillEngineDropdowns();
            }

            this.comboBoxEngine.SelectedIndex = this.comboBoxEngine.Items.IndexOf(this._template.BodySegment.Engine);
            this.comboBoxGuiEngine.SelectedIndex = this.comboBoxGuiEngine.Items.IndexOf(this._template.GuiSegment.Engine);

            if (this.comboBoxLanguage.Items.Count == 0)
            {
                this.FillBodyLanguageDropdown();
            }

            if (this.comboBoxGuiLanguage.Items.Count == 0)
            {
                this.FillGuiLanguageDropdown();
            }

            if (this.comboBoxOutputLanguage.Items.Count == 0)
            {
                this.FillLanguageDropdown();
            }

            if (this.comboBoxMode.Items.Count == 0)
            {
                this.FillModeDropdown();
            }

            if (this.comboBoxType.Items.Count == 0)
            {
                this.FillTypeDropdown();
            }

            int tmpPos = this.CurrentScintilla.CurrentPos;
            int tmpStartPos = this.CurrentScintilla.SelectionStart;
            int tmpEndPos = this.CurrentScintilla.SelectionEnd;
            int tmpFirstLineVisible = this.CurrentScintilla.FirstVisibleLine;

            this.Text = this._template.Title;
            this.TabText = this._template.Title;

            this.textBoxTitle.Text = this._template.Title;
            this.textBoxGroup.Text = this._template.NamespacePathString;
            this.textBoxUniqueID.Text = this._template.UniqueID;
            this.textBoxComments.Text = this._template.Comments;

            this.comboBoxMode.SelectedIndex = (this._template.BodySegment.Mode == ZeusConstants.Modes.MARKUP ? 0 : (this._template.BodySegment.Mode == ZeusConstants.Modes.PURE ? 1 : 2));

            if (this._template.Type == ZeusConstants.Types.GROUP)
            {
                this.buttonSelectFile.Visible = true;
                this.listBoxIncludedTemplateFiles.Visible = true;
                this.labelIncludedTemplates.Visible = true;
                this.listBoxIncludedTemplateFiles.Items.Clear();
                foreach (string path in _template.IncludedTemplatePaths)
                {
                    this.listBoxIncludedTemplateFiles.Items.Add(new FileListItem(path));
                }
            }
            else
            {
                this.buttonSelectFile.Visible = false;
                this.listBoxIncludedTemplateFiles.Visible = false;
                this.labelIncludedTemplates.Visible = false;
            }

            if (this._template.BodySegment.Mode == ZeusConstants.Modes.MARKUP)
            {
                this.textBoxStartTag.Text = this._template.TagStart;
                this.textBoxEndTag.Text = this._template.TagEnd;
                this.textBoxShortcutTag.Text = this._template.TagStartShortcut;
            }

            this.scintillaTemplateCode.Clear();
            this.scintillaTemplateCode.Text = this._template.BodySegment.CodeUnparsed;

            this.scintillaTemplateSource.IsReadOnly = false;
            this.scintillaTemplateSource.Text = this._template.BodySegment.Code;
            this.scintillaTemplateSource.IsReadOnly = true;

            this.scintillaGUICode.Clear();
            this.scintillaGUICode.Text = this._template.GuiSegment.CodeUnparsed;

            this.scintillaGuiSource.IsReadOnly = false;
            this.scintillaGuiSource.Text = this._template.GuiSegment.Code;
            this.scintillaGuiSource.IsReadOnly = true;

            this.CurrentScintilla.LineScroll(0, tmpFirstLineVisible);
            this.CurrentScintilla.CurrentPos = tmpPos;
            this.CurrentScintilla.SelectionStart = tmpStartPos;
            this.CurrentScintilla.SelectionEnd = tmpEndPos;

            this.comboBoxType.SelectedIndex = this.comboBoxType.Items.IndexOf(this._template.Type);
            this.comboBoxMode.SelectedIndex = this.comboBoxMode.Items.IndexOf(this._template.BodySegment.Mode);

            this.comboBoxLanguage.SelectedIndex = this.comboBoxLanguage.Items.IndexOf(this._template.BodySegment.Language);
            this.comboBoxGuiLanguage.SelectedIndex = this.comboBoxGuiLanguage.Items.IndexOf(this._template.GuiSegment.Language);
            this.comboBoxOutputLanguage.SelectedIndex = this.comboBoxOutputLanguage.Items.IndexOf(this._template.OutputLanguage);
        }

        private void RefreshTemplateFromControl()
        {
            try
            {
                this._template.Title = this.textBoxTitle.Text;
                this._template.NamespacePathString = this.textBoxGroup.Text;
                this._template.UniqueID = this.textBoxUniqueID.Text;
                this._template.Comments = this.textBoxComments.Text;

                if (comboBoxEngine.SelectedIndex >= 0)
                {
                    string newEngine = comboBoxEngine.SelectedItem.ToString();

                    if (newEngine != this._template.BodySegment.Engine)
                    {
                        this._template.BodySegment.Engine = newEngine;
                    }
                }
                if (comboBoxLanguage.SelectedIndex >= 0)
                {
                    this._template.BodySegment.Language = comboBoxLanguage.SelectedItem.ToString();
                }
                if (comboBoxOutputLanguage.SelectedIndex >= 0)
                {
                    this._template.OutputLanguage = comboBoxOutputLanguage.SelectedItem.ToString();
                }

                this._template.GuiSegment.CodeUnparsed = this.scintillaGUICode.Text;

                if (this._template.Type == ZeusConstants.Types.GROUP)
                {
                    this._template.IncludedTemplatePaths.Clear();
                    foreach (FileListItem item in this.listBoxIncludedTemplateFiles.Items)
                    {
                        this._template.AddIncludedTemplatePath(item.Value);
                    }
                }

                this._template.BodySegment.Mode = comboBoxMode.SelectedIndex == 0 ? ZeusConstants.Modes.MARKUP : ZeusConstants.Modes.PURE;

                this._template.TagStart = this.textBoxStartTag.Text;
                this._template.TagEnd = this.textBoxEndTag.Text;

                this._template.BodySegment.CodeUnparsed = this.scintillaTemplateCode.Text;

                this.scintillaTemplateSource.IsReadOnly = false;
                this.scintillaTemplateSource.Text = this._template.BodySegment.Code;
                this.scintillaTemplateSource.IsReadOnly = true;

                this.scintillaGuiSource.IsReadOnly = false;
                this.scintillaGuiSource.Text = this._template.GuiSegment.Code;
                this.scintillaGuiSource.IsReadOnly = true;
            }
            catch (Exception x)
            {
                ZeusDisplayError formError = new ZeusDisplayError(x);
                formError.ErrorIndexChange += new EventHandler(ZeusDisplayError_ErrorIndexChanged);
                formError.ShowDialog(this);

                foreach (string message in formError.LastErrorMessages)
                {
                    LogLn(message);
                }
            }
        }
        #endregion

        #region Action Methods (Save, SaveAs, Initialize, Execute)
        protected void _Save()
        {
            if (this.IsNew)
            {
                this.saveAsToolStripMenuItem_Click(this.saveAsToolStripMenuItem, new EventArgs());
            }
            else
            {
                this.FileSave();
            }
            this.CurrentScintilla.GrabFocus();
        }

        protected void _EncryptAs()
        {
            DialogResult dr = MessageBox.Show(this,
                    "Be careful not to overwrite your source template or you will lose your work!\r\n Are you ready to Encrypt?",
                    "Encryption Warning",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);
            if (dr == DialogResult.OK)
            {
                RefreshTemplateFromControl();
                _template.Encrypt();
                RefreshControlFromTemplate();

                _SaveAs();

                this.compileAsToolStripMenuItem.Enabled = false;
                this.encryptAsToolStripMenuItem.Enabled = false;
            }
        }

        protected void _CompileAs()
        {
            DialogResult dr = MessageBox.Show(this,
                "In order to finish compiling a template, the template must be executed completely.\r\nBe careful not to overwrite your source template or you will lose your work!\r\n Are you ready to Compile?",
                "Compilation Warning",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information);
            if (dr == DialogResult.OK)
            {
                RefreshTemplateFromControl();
                _template.Compile();
                _Execute();
                RefreshControlFromTemplate();

                _SaveAs();

                this.compileAsToolStripMenuItem.Enabled = false;
                this.encryptAsToolStripMenuItem.Enabled = false;
            }
        }

        protected void _SaveAs()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = FILE_TYPES;
            saveFileDialog.FilterIndex = DEFAULT_SAVE_FILE_TYPE_INDEX;
            saveFileDialog.RestoreDirectory = true;

            saveFileDialog.FileName = GetSaveAsFileName();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                myStream = saveFileDialog.OpenFile();

                if (null != myStream)
                {
                    myStream.Close();
                    this.FileSaveAs(saveFileDialog.FileName);
                }
            }
            this.CurrentScintilla.GrabFocus();// = true;
        }

        private string GetSaveAsFileName()
        {
            string fname = this.FileName;
#if RUN_AS_NON_ADMIN

            DefaultSettings settings = DefaultSettings.Settings;

            string dir = settings.DefaultTemplateDirectory;
            if (fname.StartsWith(dir))
                fname = settings.UserTemplateDirectory + fname.Substring(dir.Length);

            dir = Path.GetDirectoryName(fname);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
                
#endif
            return fname;
        }

        private void _Initialize(string path, params object[] options)
        {
            bool isNew = false;
            if (path == string.Empty)
            {
                isNew = true;
            }
            else if (File.Exists(path))
            {
                try
                {
                    _template = new ZeusTemplate(path);

                    if (_template.SourceType == ZeusConstants.SourceTypes.SOURCE)
                    {
                        this.LogLn("Opened Template: \"" + _template.Title + "\" from \"" + _template.FilePath + _template.FileName + "\".");
                    }
                    else
                    {
                        throw new Exception("Cannot edit locked templates.");
                    }
                }
                catch (Exception x)
                {
                    this.LogLn("Error loading template with path: " + path);

                    ZeusDisplayError formError = new ZeusDisplayError(x);
                    formError.ShowDialog(this);

                    foreach (string message in formError.LastErrorMessages)
                    {
                        LogLn(message);
                    }

                    // Make sure it's treated as a new template
                    isNew = true;
                }
            }

            if (isNew)
            {
                _template = new ZeusTemplate();
                _template.Title = "Untitled";
                _template.UniqueID = Guid.NewGuid().ToString();

                this.panelProperties.Visible = true;

                if ((options.Length % 2) == 0)
                {
                    string key, val;
                    for (int i = 0; i < options.Length; i += 2)
                    {
                        key = options[i].ToString();
                        val = options[i + 1].ToString();

                        if (key == "ENGINE")
                        {
                            _template.BodySegment.Engine = val;
                            _template.GuiSegment.Engine = val;
                        }
                        else if (key == "LANGUAGE")
                        {
                            _template.BodySegment.Language = val;
                            _template.GuiSegment.Language = val;
                        }
                    }
                }
                _template.GuiSegment.CodeUnparsed = _template.GuiSegment.ZeusScriptingEngine.GetNewGuiText(_template.GuiSegment.Language);
                _template.BodySegment.CodeUnparsed = _template.BodySegment.ZeusScriptingEngine.GetNewTemplateText(_template.BodySegment.Language);
            }

            this.RefreshControlFromTemplate();
        }

        private void _SaveTemplateInput()
        {
            try
            {
                Directory.SetCurrentDirectory(Application.StartupPath);
                this.RefreshTemplateFromControl();

                DefaultSettings settings = DefaultSettings.Instance;

                ZeusSimpleLog log = new ZeusSimpleLog();
                log.LogEntryAdded += new EventHandler(Log_EntryAdded);
                ZeusContext context = new ZeusContext();
                context.Log = log;

                ZeusSavedInput collectedInput = new ZeusSavedInput();
                collectedInput.InputData.TemplateUniqueID = _template.UniqueID;
                collectedInput.InputData.TemplatePath = _template.FilePath + _template.FileName;

                settings.PopulateZeusContext(context);

                _template.Collect(context, settings.ScriptTimeout, collectedInput.InputData.InputItems);

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
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
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
                HandleExecuteException(ex);
            }

            Cursor.Current = Cursors.Default;
        }

        private void _Execute()
        {
            this.Cursor = Cursors.WaitCursor;

            Directory.SetCurrentDirectory(Application.StartupPath);

            this.RefreshTemplateFromControl();

            DefaultSettings settings = DefaultSettings.Instance;

            ZeusContext context = new ZeusContext();
            if (context.Log is ZeusSimpleLog)
            {
                ZeusSimpleLog log = context.Log as ZeusSimpleLog;
                log.LogEntryAdded += new EventHandler(Log_EntryAdded);
            }
            IZeusGuiControl guiController = context.Gui;
            IZeusOutput zout = context.Output;

            settings.PopulateZeusContext(context);

            bool exceptionOccurred = false;
            bool result = false;
            Exception tmpEx = null;
            try
            {
                _template.GuiSegment.ZeusScriptingEngine.ExecutionHelper.Timeout = settings.ScriptTimeout;
                _template.GuiSegment.ZeusScriptingEngine.ExecutionHelper.SetShowGuiHandler(new ShowGUIEventHandler(DynamicGUI_Display));
                result = _template.GuiSegment.Execute(context);
                _template.GuiSegment.ZeusScriptingEngine.ExecutionHelper.Cleanup();

                if (result)
                {
                    _template.BodySegment.ZeusScriptingEngine.ExecutionHelper.Timeout = settings.ScriptTimeout;
                    result = _template.BodySegment.Execute(context);
                    _template.BodySegment.ZeusScriptingEngine.ExecutionHelper.Cleanup();
                }
            }
            catch (Exception x)
            {
                HandleExecuteException(x);
                tmpEx = x;
                exceptionOccurred = true;
            }

            if (context.Log is ZeusSimpleLog)
            {
                ZeusSimpleLog simpleLog = context.Log as ZeusSimpleLog;
                if (simpleLog.HasExceptions)
                {
                    foreach (Exception ex in simpleLog.Exceptions)
                    {
                        if (tmpEx != ex)
                        {
                            HandleExecuteException(ex);
                            exceptionOccurred = true;
                        }
                    }
                }
            }

            if (!exceptionOccurred && result)
            {
                zout = context.Output;

                this.scintillaOutput.IsReadOnly = false;

                if (zout.text == string.Empty)
                    this.scintillaOutput.ClearAll();
                else
                    this.scintillaOutput.Text = zout.text;

                this.scintillaOutput.IsReadOnly = true;

                if (this.tabControlTemplate.SelectedTab != this.tabOutput)
                    this.tabControlTemplate.SelectedTab = this.tabOutput;

                this.LogLn("Successfully rendered template: " + this._template.Title);

                if (this.copyOutputToClipboardToolStripMenuItem.Checked)
                {
                    try
                    {
                        Clipboard.SetDataObject(zout.text, true);
                    }
                    catch
                    {
                        // HACK: For some reason, Clipboard.SetDataObject throws an error on some systems. I'm cathing it and doing nothing for now.
                    }
                }
            }

            this.Cursor = Cursors.Default;
        }
        #endregion

        #region Event Handlers
        private void TemplateEditor_Load(object sender, System.EventArgs e)
        {
            this.splitterProperties.ToggleState();
        }

        private void TemplateEditor_Activated(object sender, EventArgs e)
        {
            mdi.FindDialog.Initialize(CurrentScintilla);
            mdi.ReplaceDialog.Initialize(CurrentScintilla);
            this.CurrentScintilla.GrabFocus();
        }


        private void textBoxStartTag_KeyUp(object sender, KeyEventArgs e)
        {
            this.textBoxShortcutTag.Text = textBoxStartTag.Text + "=";

            this.scintillaTemplateCode.UpdateModeAndLanguage(_template.BodySegment.Language, _template.BodySegment.Mode);

            this._isDirty = true;
        }

        private void buttonNewGuid_Click(object sender, System.EventArgs e)
        {
            this.textBoxUniqueID.Text = Guid.NewGuid().ToString();

            this._isDirty = true;
        }

        public void DynamicGUI_Display(IZeusGuiControl gui, IZeusFunctionExecutioner executioner)
        {
            this.Cursor = Cursors.Default;

            try
            {
                DynamicForm df = new DynamicForm(gui as GuiController, executioner);
                df.Logger = this;
                DialogResult result = df.ShowDialog(this);

                if (result == DialogResult.Cancel)
                {
                    gui.IsCanceled = true;
                }
            }
            catch (Exception x)
            {
                HandleExecuteException(x);
            }

            this.Cursor = Cursors.WaitCursor;
        }

        private void comboBoxEngine_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string eng = comboBoxEngine.SelectedItem.ToString();

            this._template.BodySegment.Engine = eng;

            this.comboBoxLanguage.SelectedIndex = -1;
            this.comboBoxLanguage.Items.Clear();
            this.FillBodyLanguageDropdown();
            //if (comboBoxLanguage.Items.Count > 0) comboBoxLanguage.SelectedIndex = 0;

            this._isDirty = true;
        }

        private void comboBoxGuiEngine_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string eng = comboBoxGuiEngine.SelectedItem.ToString();

            this._template.GuiSegment.Engine = eng;

            this.comboBoxGuiLanguage.SelectedIndex = -1;
            this.comboBoxGuiLanguage.Items.Clear();
            this.FillGuiLanguageDropdown();
            //if (comboBoxGuiLanguage.Items.Count > 0) comboBoxGuiLanguage.SelectedIndex = 0;

            this._isDirty = true;
        }

        private void comboBoxLanguage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboBoxLanguage.SelectedIndex >= 0)
            {
                string lang = comboBoxLanguage.SelectedItem.ToString();
                string mode = this._template.BodySegment.Mode;

                this._template.BodySegment.Language = lang;

                this.scintillaTemplateCode.UpdateModeAndLanguage(lang, mode);
                this.scintillaTemplateSource.UpdateModeAndLanguage(lang, ZeusConstants.Modes.PURE);
            }
            this._isDirty = true;
        }

        private void comboBoxGuiLanguage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboBoxGuiLanguage.SelectedIndex >= 0)
            {
                string lang = comboBoxGuiLanguage.SelectedItem.ToString();

                this._template.GuiSegment.Language = lang;

                this.scintillaGUICode.UpdateModeAndLanguage(lang, ZeusConstants.Modes.PURE);
                this.scintillaGuiSource.UpdateModeAndLanguage(lang, ZeusConstants.Modes.PURE);
            }
            this._isDirty = true;
        }

        //TODO: Fix the TemplateGroup Stuff!!!
        private void comboBoxMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboBoxMode.SelectedIndex >= 0)
            {
                switch (comboBoxMode.Items[comboBoxMode.SelectedIndex].ToString())
                {
                    case ZeusConstants.Modes.MARKUP:
                        this._template.BodySegment.Mode = ZeusConstants.Modes.MARKUP;

                        this.scintillaTemplateCode.UpdateModeAndLanguage(_template.BodySegment.Language, _template.BodySegment.Mode);

                        //						this.scintillaTemplateCode.Language = _template.BodySegment.Language;
                        //						this.scintillaTemplateCode.Mode = _template.BodySegment.Mode;
                        this.groupBoxScripting.Enabled = true;

                        break;
                    case ZeusConstants.Modes.PURE:
                        this._template.BodySegment.Mode = ZeusConstants.Modes.PURE;

                        this.scintillaTemplateCode.UpdateModeAndLanguage(_template.BodySegment.Language, _template.BodySegment.Mode);

                        //						this.scintillaTemplateCode.Language = _template.BodySegment.Language;
                        this.groupBoxScripting.Enabled = false;

                        break;
                }

                this._isDirty = true;
            }
        }

        private void comboBoxType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboBoxType.SelectedIndex >= 0)
            {
                switch (comboBoxType.Items[comboBoxType.SelectedIndex].ToString())
                {
                    case ZeusConstants.Types.TEMPLATE:
                        this.listBoxIncludedTemplateFiles.Visible = false;
                        this.buttonSelectFile.Visible = false;
                        this.labelIncludedTemplates.Visible = false;

                        if (this._template != null) this._template.Type = ZeusConstants.Types.TEMPLATE;

                        break;
                    case ZeusConstants.Types.GROUP:
                        this.listBoxIncludedTemplateFiles.Visible = true;
                        this.buttonSelectFile.Visible = true;
                        this.labelIncludedTemplates.Visible = true;

                        if (this._template != null) this._template.Type = ZeusConstants.Types.GROUP;

                        break;
                }

                this._isDirty = true;
            }
        }

        private void comboBoxOutputLanguage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string lang = comboBoxOutputLanguage.SelectedItem.ToString();
            this._template.OutputLanguage = lang;
            this.scintillaOutput.UpdateModeAndLanguage(lang, "");
            //	this.scintillaOutput.Language = lang;
            this._isDirty = true;
        }

        private void buttonSelectFile_Click(object sender, System.EventArgs e)
        {
            string[] paths = PickFiles();
            foreach (string path in paths)
            {
                if (path != string.Empty && !listBoxContainsItem(this.listBoxIncludedTemplateFiles, path))
                {
                    this.listBoxIncludedTemplateFiles.Items.Add(new FileListItem(path));
                    this._isDirty = true;
                }
            }
        }

        private void listBoxIncludedTemplateFiles_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
            {
                int index = listBoxIncludedTemplateFiles.SelectedIndex;
                if (index > 0) index--;

                listBoxIncludedTemplateFiles.Items.Remove(listBoxIncludedTemplateFiles.SelectedItem);

                if (index < listBoxIncludedTemplateFiles.Items.Count) listBoxIncludedTemplateFiles.SelectedIndex = index;
                this._isDirty = true;
            }
        }

        private void TemplateEditor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.CanClose(true))
            {
                e.Cancel = true;
            }
        }

        private void tabControlTemplate_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (tabControlTemplate.SelectedTab == this.tabTemplateSource)
            {
                this.RefreshTemplateFromControl();
            }

            //if (tabControlTemplate.SelectedIndex < 2) 
            //{
            this.CurrentScintilla.Activate();
            //}

            // Clear the status bar row/col coordinates text
            //MDIParent.TheParent.statusRow.Text = "";
            //MDIParent.TheParent.statusCol.Text = "";
        }

        private void TemplateEditor_DockStateChanged(object sender, EventArgs e)
        {
            if ((this.DockState != DockState.Unknown) &&
                (this.DockState != DockState.Hidden))
            {
                this.scintillaGUICode.SpecialRefresh();
                this.scintillaGuiSource.SpecialRefresh();
                this.scintillaOutput.SpecialRefresh();
                this.scintillaTemplateCode.SpecialRefresh();
                this.scintillaTemplateSource.SpecialRefresh();
            }
        }
        #endregion

        #region Tab String Events
        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            this._Save();
        }

        private void toolStripButtonSaveAs_Click(object sender, EventArgs e)
        {
            this._SaveAs();
        }

        private void toolStripButtonProperties_Click(object sender, EventArgs e)
        {
            this.splitterProperties.ToggleState();
        }

        private void toolStripButtonConsole_Click(object sender, EventArgs e)
        {
            this.splitterConsole.ToggleState();
        }

        private void toolStripButtonExecute_Click(object sender, EventArgs e)
        {
            this._Execute();
        }
        #endregion

        #region Menu Item Events
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._SaveAs();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!mdi.FindDialog.Visible && !mdi.ReplaceDialog.Visible)
                mdi.FindDialog.Show(this);
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!mdi.FindDialog.Visible && !mdi.ReplaceDialog.Visible)
                mdi.ReplaceDialog.Show(this);
        }

        private void replaceHiddenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            replaceToolStripMenuItem_Click(sender, e);
        }

        private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mdi.FindDialog.FindNext();
        }

        private void findPrevToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mdi.FindDialog.FindPrevious();
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._Execute();
        }

        private void encryptAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._EncryptAs();
        }

        private void compileAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._CompileAs();
        }

        private void lineNumbersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool lineNumbersEnabled = !this.lineNumbersToolStripMenuItem.Checked;

            if (lineNumbersEnabled)
            {
                this.scintillaTemplateCode.MarginWidthN(0, 40);
                this.scintillaGUICode.MarginWidthN(0, 40);
                this.scintillaTemplateSource.MarginWidthN(0, 40);
                this.scintillaGuiSource.MarginWidthN(0, 40);
                this.scintillaOutput.MarginWidthN(0, 40);
            }
            else
            {
                this.scintillaTemplateCode.MarginWidthN(0, 0);
                this.scintillaGUICode.MarginWidthN(0, 0);
                this.scintillaTemplateSource.MarginWidthN(0, 0);
                this.scintillaGuiSource.MarginWidthN(0, 0);
                this.scintillaOutput.MarginWidthN(0, 0);
            }

            this.lineNumbersToolStripMenuItem.Checked = lineNumbersEnabled;
        }

        private void whitespaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // We reverse the state
            bool on = this.scintillaTemplateCode.ViewWhitespace != Scintilla.Enums.WhiteSpace.Invisible ? false : true;

            this.scintillaTemplateCode.ViewWhitespace = on ? Scintilla.Enums.WhiteSpace.VisibleAlways : Scintilla.Enums.WhiteSpace.Invisible;
            this.whitespaceToolStripMenuItem.Checked = on;
        }

        private void indentationGuidsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggle the state
            bool on = !this.scintillaTemplateCode.IsIndentationGuides;

            this.scintillaTemplateCode.IsIndentationGuides = on;
            this.indentationGuidsToolStripMenuItem.Checked = on;
        }

        private void endOfLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggle the state
            bool on = !this.scintillaTemplateCode.IsViewEOL;

            this.scintillaTemplateCode.IsViewEOL = on;
            this.endOfLineToolStripMenuItem.Checked = on;
            DefaultSettings settings = DefaultSettings.Instance;
        }

        private void goToLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Goto go = new Goto();
            DialogResult result = go.ShowDialog();

            if (result == DialogResult.OK)
            {
                int lineNumber = Math.Max(0, go.LineNumber - 1);
                lineNumber = Math.Min(this.CurrentScintilla.LineCount, lineNumber);

                this.CurrentScintilla.GrabFocus();
                this.CurrentScintilla.EnsureVisibleEnforcePolicy(lineNumber);
                this.CurrentScintilla.EnsureVisible(lineNumber);
                this.CurrentScintilla.GotoLine(lineNumber);
            }
        }

        private void enlargeFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.scintillaTemplateCode.ZoomIn();
        }

        private void reduceFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scintillaTemplateCode.Zoom > 0)
            {
                this.scintillaTemplateCode.ZoomOut();
            }
        }

        private void copyOutputToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.copyOutputToClipboardToolStripMenuItem.Checked = !this.copyOutputToClipboardToolStripMenuItem.Checked;
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentScintilla.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentScintilla.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentScintilla.Paste();
        }
        #endregion

        #region IScintillaEditControl Members

        public ScintillaControl ScintillaEditor
        {
            get { return this.CurrentScintilla; }
        }

        #endregion

        #region IMyGenDocument Members

        public string DocumentIndentity
        {
            get { return (this.IsNew ? this.UniqueID : this.FileName); }
        }

        public ToolStrip ToolStrip
        {
            get { return this.toolStripOptions; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            if (command == "UpdateDefaultSettings")
            {
                DefaultSettings settings = DefaultSettings.Instance;
                SetCodePageOverride(settings.CodePage);
                SetFontOverride(settings.FontFamily);

                this.scintillaTemplateCode.TabWidth = settings.Tabs;
                this.copyOutputToClipboardToolStripMenuItem.Checked = settings.EnableClipboard;
            }
            else if (command == "UpdateTemplate")
            {
                if (args.Length > 0)
                {
                    if (string.Equals(args[0].ToString(), this._template.UniqueID, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (MessageBox.Show(this,
                               "The file \r\n\"" + this._template.FileName + "\"\r\n has been updated outside of the editor.\r\nWould you like to refresh with the new contents?",
                               "Refresh Updated File?",
                               MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this.Activate();
                            this._Initialize(this.FileName);
                        }
                    }
                }
            }
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        #endregion
	}

	/// <summary>
	/// An item for a listbox that holds a filename
	/// </summary>
	public class FileListItem : IComparable
	{
		private string _value;
		private string _text;

		public FileListItem(string filename) 
		{
			this._value = filename;
			
			FileInfo f = new FileInfo(filename);
			this._text = f.Name;
		}

		public string Text 
		{
			get 
			{
				return _text;
			}
			set 
			{
				_text = value;
			}
		}

		public string Value 
		{
			get 
			{
				return _value;
			}
			set 
			{
				_value = value;
			}
		}

		public override string ToString()
		{
			return _text;
        }

		#region IComparable Members

		public int CompareTo(object obj)
		{
			return this._value.CompareTo(obj.ToString());
		}

		#endregion

	}
}
