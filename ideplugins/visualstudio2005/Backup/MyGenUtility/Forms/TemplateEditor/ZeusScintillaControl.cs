using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;

using Scintilla;
using Scintilla.Forms;
using Scintilla.Enums;
using Zeus;
using Zeus.Templates;
using MyGeneration.AutoCompletion;

namespace MyGeneration
{
	public interface IEditControl 
	{
		string Mode { get; }
        string Language { get; }
        string Text { get; set; }
        void GrabFocus();
        void Activate();
	}

	/// <summary>
	/// Summary description for JScriptScintillaControl.
	/// </summary>
	public class ZeusScintillaControl : ScintillaControl, IEditControl, IScintillaNet
	{
        private const int WM_KEYDOWN = 0x0100;
        private const int DEFAULT_CODE_PAGE = 65001;

        private string _language;
        private string _mode;
        private string _fontFamilyOverride;
        private int _codePageOverride = DEFAULT_CODE_PAGE;

		private static string LastSearchTextStatic = string.Empty;
        private static bool LastSearchIsCaseSensitiveStatic = true;
        private static bool LastSearchIsRegexStatic = false;
        private Hashtable ignoredKeys = new Hashtable();
        
        public static FindForm FindDialog = new FindForm();
        public static ReplaceForm ReplaceDialog = new ReplaceForm();
        public static ScintillaConfigureDelegate StaticConfigure = null;

		public ZeusScintillaControl() : base() 
		{
            Scintilla.Forms.SearchHelper.Translations.MessageBoxTitle = "MyGeneration";
            this.SmartIndentType = SmartIndent.Simple;
            this.GotFocus += new EventHandler(ZeusScintillaControl_GotFocus);

            this.Configure = StaticConfigure;
            this.CharAdded += new EventHandler<CharAddedEventArgs>(ZeusScintillaControl_CharAdded);
            this.AllowDrop = true;
            this.IsAutoCIgnoreCase = true;
        }

        

        private void ZeusScintillaControl_CharAdded(object sender, CharAddedEventArgs e)
        {
            this.Cursor_ = (int)Scintilla.Enums.CursorShape.Wait;
            AutoCompleteHelper.CharAdded(this, e.Ch);
            this.Cursor_ = (int)Scintilla.Enums.CursorShape.Normal;
        }

        void ZeusScintillaControl_GotFocus(object sender, EventArgs e)
        {
            InitializeFindReplace();
        }

        public void InitializeFindReplace()
        {
            FindDialog.Initialize(this);
            ReplaceDialog.Initialize(this);
        }

        public string FontFamilyOverride
        {
            get
            {
                return this._fontFamilyOverride;
            }
            set
            {
                if (value != _fontFamilyOverride)
                {
                    _fontFamilyOverride = value;
                    UpdateCurrentStyles();
                }
                
            }
        }

        public int CodePageOverride
        {
            get
            {
                return this._codePageOverride;
            }
            set
            {
                int tmp = (value == -1) ? DEFAULT_CODE_PAGE : value;
                if (tmp != _codePageOverride)
                {
                    _codePageOverride = tmp;
                    UpdateCurrentStyles();
                }
            }
        }


        public string Mode
        {
            get
            {
                return this._mode;
            }
        }

		public string Language 
		{
			get 
			{
				return this._language;
			}
		}

		public string LastSearchText { get { return ZeusScintillaControl.LastSearchTextStatic; } }
		public bool LastSearchIsCaseSensitive { get { return ZeusScintillaControl.LastSearchIsCaseSensitiveStatic; } }
		public bool LastSearchIsRegex { get { return ZeusScintillaControl.LastSearchIsRegexStatic; } }

		public override string Text
		{
			get
			{
                return base.Text;
			}
			set
			{
				if ((value != null) && (value.Length > 0))
					base.Text = value;
			}
		}

        public void SpecialRefresh()
        {
            UpdateCurrentStyles();
        }

        private void UpdateCurrentStyles()
        {
            UpdateModeAndLanguage(_language, _mode);
        }
		public void UpdateModeAndLanguage(string language, string mode) 
		{
			this.StyleClearAll();
			this.StyleResetDefault();
			this.ClearDocumentStyle();
		
			this._language = language;
			this._mode     = mode;			

			switch (this._language)
			{
				case ZeusConstants.Languages.VB:
				case ZeusConstants.Languages.VBNET:
					if (this.Mode == ZeusConstants.Modes.MARKUP) this.ConfigureControlForTaggedVBNet();
					else this.ConfigureControlForVBNet();
					break;

				case ZeusConstants.Languages.JSCRIPT:
				case ZeusConstants.Languages.JAVA:
					if (this.Mode == ZeusConstants.Modes.MARKUP) this.ConfigureControlForTaggedJScript();
					else this.ConfigureControlForJScript();
					break;

				case ZeusConstants.Languages.VBSCRIPT:
					if (this.Mode == ZeusConstants.Modes.MARKUP) this.ConfigureControlForTaggedVBScript();
					else this.ConfigureControlForVBScript();
					break;

				case ZeusConstants.Languages.CSHARP:
					if (this.Mode == ZeusConstants.Modes.MARKUP) this.ConfigureControlForTaggedCSharp();
					else this.ConfigureControlForCSharp();
					break;

				case ZeusConstants.Languages.TSQL:	
					this.ConfigureControlForMSSQL();
					break;

				case ZeusConstants.Languages.SQL:
				case ZeusConstants.Languages.PLSQL:
					this.ConfigureControlForSQL();
					break;

				case ZeusConstants.Languages.JETSQL:
					this.ConfigureControlForJetSQL();
					break;

				case ZeusConstants.Languages.PHP:
					this.ConfigureControlForPHP();
					break;

				default:
					this.ConfigureControlForUnsupported();
					break;
			}

            DefaultSettings settings = DefaultSettings.Instance;
			this.TabWidth = settings.Tabs;

            if (!string.IsNullOrEmpty(this.FontFamilyOverride))
            {
                Font f = new Font(this.FontFamilyOverride, this.Font.Size);
                for (int i = 0; i < 128; i++)
                {
                    this.StyleSetFont(i, f.Name);
                }
            }
            this.CodePage = this.CodePageOverride;

			this.Colorize(0, -1);
            //this.Colourise(0, -1);
            
			this.Refresh();
		}

        public void Activate()
        {
            this.InitializeFindReplace();
            this.GrabFocus();
            this.Focus();
        }

		#region Base Scintilla Settings
		protected void ConfigureBaseSettings() 
		{
			//SC_CP_UTF8 (65001)
			this.CodePage = this.CodePageOverride;

			this.Anchor_ = 0;
	//		this.BackSpaceUnIndents = false;
	//		this.BufferedDraw = true;
	//		this.CaretForeground = 0;
	//		this.CaretLineBackground = 65535;
	//		this.CaretLineVisible = false;
			this.CaretPeriod = 500;
			this.CaretWidth = 1;
            //this.Configuration = null;
			this.ConfigurationLanguage = "";
			this.ControlCharSymbol = 0;
			this.CurrentPos = 0;
			this.Cursor_ = -1;
			this.Dock = System.Windows.Forms.DockStyle.Fill;
			//this.EdgeColour = 12632256;
            this.EdgeColor = 12632256;
			this.EdgeColumn = 0;
			this.EdgeMode = 0;
	//		this.EndAtLastLine = true;
	//		this.EOLCharactersVisible = false;
            this.EndOfLineMode = EndOfLine.Crlf;
	//		this.focus = false;
			this.HighlightGuide = 0;
	//		this.HorizontalScrollBarVisible = true;
			this.Indent = 0;
	//		this.IndentationGuidesVisible = false;
			this.LayoutCache = 1;
			this.Lexer = 0;
	//		this.LexerLanguage = null;
			this.Location = new System.Drawing.Point(0, 0);
			this.MarginLeft = 1;
			this.MarginRight = 1;
			this.ModEventMask = 3959;
	//		this.MouseDownCaptures = true;
			this.MouseDwellTime = 10000000;
			this.Name = "scintillaControl";
	//		this.Overtype = false;
			this.PrintColorMode = 0;
			this.PrintMagnification = 0;
			this.PrintWrapMode = 1;
	//		this.ReadOnly = false;
			this.ScrollWidth = 2000;
			this.SearchFlags = 0;
			this.SelectionEnd = 0;
			this.SelectionStart = 0;
			this.Size = new System.Drawing.Size(736, 721);
			this.Status = 0;
			this.StyleBits = 5;
	//		this.TabIndents = true;
			this.TabIndex = 0;
			this.TabWidth = 8;
			this.TargetEnd = 0;
			this.TargetStart = 0;
	//		this.UsePalette = false;
	//		this.UseTabs = true;
	//		this.VerticalScrollBarVisible = true;
	//		this.WhitespaceVisibleState = 0;
			this.WrapMode = 0;
			this.XOffset = 0;
			this.Zoom = 0;
			this.Visible = true;
			this.Width = 200;
			this.Height = 200;
			this.Dock = DockStyle.Fill;
	//		this.TabIndents = true;

			this.SetSelectionBackground(true, 65535);  // yellow
			this.SetSelectionForeground(true, 0);
		}
		#endregion
		
		#region Default Control Setup
		protected void ConfigureControlForUnsupported() 
		{
			this.ConfigureBaseSettings();
		}
		#endregion

		#region C# Setup
		protected void ConfigureControlForCSharp()
		{
			this.ConfigureBaseSettings();

            this.ConfigurationLanguage = "C#";
			//this.LegacyConfigurationLanguage = "C#";
		}
		#endregion

		#region VBNet Setup
		protected void ConfigureControlForVBNet()
		{
			this.ConfigureBaseSettings();
			//this.LegacyConfiguration = MDIParent.scintillaXmlConfig.scintilla;
			this.ConfigurationLanguage = "VB.Net";

			this.CaretFore = 0xffffff;

            
		}
		#endregion

		#region PHP Setup
		protected void ConfigureControlForPHP()
		{
			this.ConfigureBaseSettings();

			//this.LegacyConfiguration = MDIParent.scintillaXmlConfig.scintilla;
			this.ConfigurationLanguage = "PHP";

            
		}
		#endregion
		
		#region JScript Setup
		protected void ConfigureControlForJScript()
		{
			this.ConfigureBaseSettings();

			//this.LegacyConfiguration = MDIParent.scintillaXmlConfig.scintilla;
			this.ConfigurationLanguage = "JScript";

            
		}
		#endregion

		#region VBScript Setup
		protected void ConfigureControlForVBScript()
		{
			this.ConfigureBaseSettings();

			//this.LegacyConfiguration = MDIParent.scintillaXmlConfig.scintilla;
			this.ConfigurationLanguage = "VBScript";

            
		}
		#endregion

		#region MSSQL Setup
		protected void ConfigureControlForMSSQL()
		{
			this.ConfigureBaseSettings();
			//this.LegacyConfiguration = MDIParent.scintillaXmlConfig.scintilla;
			this.ConfigurationLanguage = "MSSQL";
		}
		#endregion
		
		#region SQL Setup
		protected void ConfigureControlForSQL()
		{
			this.ConfigureBaseSettings();
			//this.LegacyConfiguration = MDIParent.scintillaXmlConfig.scintilla;
			this.ConfigurationLanguage = "SQL";
		}
		#endregion

		#region JetSQL Setup
		protected void ConfigureControlForJetSQL()
		{
			this.ConfigureBaseSettings();
			//this.LegacyConfiguration = MDIParent.scintillaXmlConfig.scintilla;
			this.ConfigurationLanguage = "JetSQL";

            
		}
		#endregion

		#region Tagged C# Setup
		protected void ConfigureControlForTaggedCSharp()
		{
			this.ConfigureBaseSettings();

			//this.LegacyConfiguration = MDIParent.scintillaXmlConfig.scintilla;
			this.ConfigurationLanguage = "TaggedC#";
		}
		#endregion

		#region Tagged JScript Setup
		protected void ConfigureControlForTaggedJScript()
		{
			this.ConfigureBaseSettings();

			//this.LegacyConfiguration = MDIParent.scintillaXmlConfig.scintilla;
            this.ConfigurationLanguage = "TaggedJScript";
            
		}
		#endregion

		#region Tagged VBScript Setup
		protected void ConfigureControlForTaggedVBScript()
		{
			this.ConfigureBaseSettings();

			//this.LegacyConfiguration = MDIParent.scintillaXmlConfig.scintilla;
			this.ConfigurationLanguage = "TaggedVBScript";

            this.CaretFore = 0x000000;
            
		}
		#endregion

		#region Tagged VBNet Setup
		protected void ConfigureControlForTaggedVBNet()
		{
			this.ConfigureBaseSettings();

			//this.LegacyConfiguration = MDIParent.scintillaXmlConfig.scintilla;
			this.ConfigurationLanguage = "TaggedVB.Net";

            this.CaretFore = 0x000000;
            
		}
		#endregion
	}
}
