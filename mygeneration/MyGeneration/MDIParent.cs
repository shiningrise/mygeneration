using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Net;
using Microsoft.Win32;
using System.Reflection;
using System.Runtime.InteropServices;

using WeifenLuo.WinFormsUI.Docking;

using Zeus;
using Zeus.ScriptModel;
using Zeus.Templates;
using Zeus.ErrorHandling;
using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;
using Scintilla;
using Scintilla.Forms;
using Scintilla.Configuration;
using Scintilla.Configuration.Legacy;

using MyGeneration.com.mygenerationsoftware.www;

namespace MyGeneration
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MDIParent : System.Windows.Forms.Form, IMyGenerationMDI
    {
        [DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);


        private const string URL_HOMEPAGE = "http://www.mygenerationsoftware.com/home/";
        private const string URL_DOCUMENTATION = "http://www.mygenerationsoftware.com/Documentation.aspx";
        private const string URL_LATESTVERSION = "http://www.mygenerationsoftware.com/LatestVersion";
        private const string PROJECT_FILE_TYPES = "Zeus Project (*.zprj)|*.zprj|";
        private const string REPLACEMENT_SUFFIX = "$REPLACEMENT$.dll";
        private const string DOCK_CONFIG_FILE = "MyGenDockManager.config";
        private const string EMPTY_CONSTANT = "$@EMPTY@$";

        private string[] validExtentions = new string[] { ".zeus", ".jgen", ".vbgen", ".csgen" };
        private string[] validProjectExtentions = new string[] { ".zprj" };

        // Dockable Windows
        private LanguageMappings languageMappings;
        private DbTargetMappings dbTargetMappings;
        private MetaDataBrowser metaDataBrowser;
        private UserMetaData userMetaData;
        private GlobalUserMetaData globalUserMetaData;
        public MetaProperties metaProperties;

        private TemplateBrowser _templateBrowser;
        //private SimpleFindReplace _findDialog = null;

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem menuItem10;

        //internal static Configuration.MyGeneration configFile = null;
        private System.Windows.Forms.StatusBar statusBar;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.MenuItem menuItemExit;
        private System.Windows.Forms.MenuItem menuItemFile;
        private System.Windows.Forms.MenuItem menuItemEdit;
        private System.Windows.Forms.MenuItem menuItemHelp;
        private System.Windows.Forms.MenuItem menuItemHelpAbout;
        private System.Windows.Forms.MenuItem menuItemNew;
        private System.Windows.Forms.MenuItem menuItemNewJScript;
        private System.Windows.Forms.MenuItem menuItemNewVBScript;
        private System.Windows.Forms.MenuItem menuItemNewBrowser;
        private System.Windows.Forms.MenuItem menuItemFileOpen;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItemDefaultProperties;
        private System.Windows.Forms.ToolBar toolBar1;
        private System.Windows.Forms.ImageList toolbarImages;
        private System.Windows.Forms.ToolBarButton toolBarButton_OpenBrowser;
        private System.Windows.Forms.ToolBarButton toolBarButton2;
        private System.Windows.Forms.ToolBarButton toolBarButton_newVBTemplate;
        private System.Windows.Forms.ToolBarButton toolBarButton_newJavaTemplate;
        private DockPanel dockManager;
        private System.Windows.Forms.ToolBarButton toolBarButton_LanguageMappings;
        private System.Windows.Forms.ToolBarButton toolBarButton_DbTargetMappings;
        private System.Windows.Forms.ToolBarButton toolBarButton_MetaBrowser;
        private System.Windows.Forms.ToolBarButton toolBarButton_fileOpen;
        private System.Windows.Forms.ToolBarButton toolBarButton_UserData;

        public static MDIParent TheParent = null;
        private System.Windows.Forms.MenuItem menuItemWebSite;
        private System.Windows.Forms.MenuItem menuItem6;
        public System.Windows.Forms.StatusBarPanel statusRow;
        public System.Windows.Forms.StatusBarPanel statusCol;

        private string[] startupFiles = null;
        private ArrayList dockingStartupTemplates = new ArrayList();
        private ArrayList dockingStartupProjects = new ArrayList();

        private string startupPath;
        private System.Windows.Forms.MenuItem menuItemNewCSharp;
        private System.Windows.Forms.MenuItem menuItemNewVBNet;
        private System.Windows.Forms.MenuItem menuItemMyMetaHelp;
        private System.Windows.Forms.ToolBarButton toolBarButton_newCSharpTemplate;
        private System.Windows.Forms.ToolBarButton toolBarButton_newVBNetTemplate;
        private System.Windows.Forms.ToolBarButton toolBarButton_GlobalUserData;
        private System.Windows.Forms.ToolBarButton toolBarButton3;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem menuItem12;
        private System.Windows.Forms.MenuItem menuItemdOOdadsHelp;
        private System.Windows.Forms.MenuItem menuItemZuesHelp;
        private System.Windows.Forms.MenuItem menuItemTemplateBrowser;
        private System.Windows.Forms.ToolBarButton toolBarButton_TemplateBrowser;
        private System.Windows.Forms.ToolBarButton toolBarButton4;
        // Web Service Version Check
        IAsyncResult asyncResult = null;
        VersionInfo versionInfo = null;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItemTemplate;
        private System.Windows.Forms.MenuItem menuItemProject;
        private System.Windows.Forms.MenuItem menuItemNewProject;
        private System.Windows.Forms.MenuItem menuItem11;
        private System.Windows.Forms.ToolBarButton toolBarButton_newProject;
        private System.Windows.Forms.MenuItem menuItemContents;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItemFreeMemory;
        private System.Windows.Forms.MenuItem menuItemRecentFiles;
        private System.Windows.Forms.ToolBarButton toolBarButton_OpenOutputFolder;
        private System.Windows.Forms.MenuItem menuItem13;
        private System.Windows.Forms.MenuItem menuItemDnpUtils;
        bool webServiceError = false;

        
        private FindForm findDialog = new FindForm();
        private ReplaceForm replaceDialog = new ReplaceForm();
        private ScintillaConfigureDelegate configureDelegate;

        private static ConfigFile configFile;

        public static ConfigFile ScintillaConfigFile { get { return configFile; } }


        /// <summary>
        /// Constructor loads up the default settings, starts an async version check, loads the scintilla config, etc.
        /// </summary>
        /// <param name="startupPath"></param>
        /// <param name="args"></param>
        public MDIParent(string startupPath, params string[] args)
        {
            TheParent = this;
            DefaultSettings settings = DefaultSettings.Instance;
            languageMappings = new LanguageMappings(this);
            dbTargetMappings = new DbTargetMappings(this);
       metaDataBrowser = new MetaDataBrowser(this);
        userMetaData = new UserMetaData(this);
        globalUserMetaData = new GlobalUserMetaData(this);
        metaProperties = new MetaProperties(this);


            //Any files that were locked when the TemplateLibrary downloaded and tried to replace them will be replaced now.
            ProcessReplacementFiles();

            StartVersionCheck(settings);

            userMetaData.MetaDataBrowser = this.metaDataBrowser;
            globalUserMetaData.MetaDataBrowser = this.metaDataBrowser;

            InitializeComponent();

            this.startupPath = startupPath;

            //Configuration.MyGeneration x = Configuration.MyGeneration.PopulatedObject;
            Scintilla.Configuration.Legacy.ConfigurationUtility cu = new Scintilla.Configuration.Legacy.ConfigurationUtility();

            // If the file doesn't exist, create it.
            FileInfo scintillaConfigFile = new FileInfo(startupPath + @"\settings\ScintillaNET.xml");
            if (scintillaConfigFile.Exists)
            {
                //TODO: Retry this with a copy of the file until we can upgrade Scintilla.Net with a fix.
                int maxTries = 3;
                while (maxTries > 0)
                {
                    try
                    {
                        //ConfigFile cf = new ConfigFile();
                        configFile = cu.LoadConfiguration(scintillaConfigFile.FullName) as ConfigFile;
                        //cf.
                        //object o = cu.LoadConfiguration(typeof(Configuration.MyGeneration), scintillaConfigFile.FullName);
                        //configFile = o as Configuration.;
                        //configFile.CollectScintillaNodes(null);
                        break;
                    }
                    catch
                    {
                        if (--maxTries == 1)
                        {
                            File.Copy(scintillaConfigFile.FullName, scintillaConfigFile.FullName + ".tmp", true);
                            scintillaConfigFile = new FileInfo(scintillaConfigFile.FullName + ".tmp");
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(10);
                        }
                    }
                }
            }

            if (configFile != null)
            {
                configureDelegate = configFile.MasterScintilla.Configure;
                ZeusScintillaControl.StaticConfigure = configureDelegate;
            }

            this.IsMdiContainer = true;
            this.MdiChildActivate += new EventHandler(this.MDIChildActivated);

            startupFiles = args;

            if (settings.CompactMemoryOnStartup) FlushMemory();

            this.RefreshRecentFiles();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>

        /// Required method for Designer support - do not modify

        /// the contents of this method with the code editor.

        /// </summary>

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MDIParent));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItemFile = new System.Windows.Forms.MenuItem();
            this.menuItemNew = new System.Windows.Forms.MenuItem();
            this.menuItemNewJScript = new System.Windows.Forms.MenuItem();
            this.menuItemNewVBScript = new System.Windows.Forms.MenuItem();
            this.menuItemNewCSharp = new System.Windows.Forms.MenuItem();
            this.menuItemNewVBNet = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItemNewProject = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItemNewBrowser = new System.Windows.Forms.MenuItem();
            this.menuItemTemplateBrowser = new System.Windows.Forms.MenuItem();
            this.menuItemFileOpen = new System.Windows.Forms.MenuItem();
            this.menuItemRecentFiles = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItemEdit = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItemFreeMemory = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItemDefaultProperties = new System.Windows.Forms.MenuItem();
            this.menuItemTemplate = new System.Windows.Forms.MenuItem();
            this.menuItemProject = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItemHelp = new System.Windows.Forms.MenuItem();
            this.menuItemContents = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItemdOOdadsHelp = new System.Windows.Forms.MenuItem();
            this.menuItemMyMetaHelp = new System.Windows.Forms.MenuItem();
            this.menuItemZuesHelp = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItemWebSite = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItemHelpAbout = new System.Windows.Forms.MenuItem();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.statusRow = new System.Windows.Forms.StatusBarPanel();
            this.statusCol = new System.Windows.Forms.StatusBarPanel();
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.toolBarButton_fileOpen = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton_TemplateBrowser = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton_OpenBrowser = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton_newProject = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton_OpenOutputFolder = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton_newJavaTemplate = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton_newVBTemplate = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton_newCSharpTemplate = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton_newVBNetTemplate = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton_MetaBrowser = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton_LanguageMappings = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton_DbTargetMappings = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton_UserData = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton_GlobalUserData = new System.Windows.Forms.ToolBarButton();
            this.toolbarImages = new System.Windows.Forms.ImageList(this.components);
            this.dockManager = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuItemDnpUtils = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.statusRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusCol)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItemFile,
																					  this.menuItemEdit,
																					  this.menuItemTemplate,
																					  this.menuItemProject,
																					  this.menuItem5,
																					  this.menuItemHelp});
            // 
            // menuItemFile
            // 
            this.menuItemFile.Index = 0;
            this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemNew,
																						 this.menuItemFileOpen,
																						 this.menuItemRecentFiles,
																						 this.menuItem1,
																						 this.menuItemExit});
            this.menuItemFile.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.menuItemFile.Text = "&File";
            // 
            // menuItemNew
            // 
            this.menuItemNew.Index = 0;
            this.menuItemNew.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.menuItemNewJScript,
																						this.menuItemNewVBScript,
																						this.menuItemNewCSharp,
																						this.menuItemNewVBNet,
																						this.menuItem11,
																						this.menuItemNewProject,
																						this.menuItem4,
																						this.menuItemNewBrowser,
																						this.menuItemTemplateBrowser});
            this.menuItemNew.Text = "&New";
            // 
            // menuItemNewJScript
            // 
            this.menuItemNewJScript.Index = 0;
            this.menuItemNewJScript.Text = "&JScript Template";
            this.menuItemNewJScript.Click += new System.EventHandler(this.WindowFileNewJScriptTemplate_Clicked);
            // 
            // menuItemNewVBScript
            // 
            this.menuItemNewVBScript.Index = 1;
            this.menuItemNewVBScript.Text = "V&BScript Template";
            this.menuItemNewVBScript.Click += new System.EventHandler(this.WindowFileNewVBScriptTemplate_Clicked);
            // 
            // menuItemNewCSharp
            // 
            this.menuItemNewCSharp.Index = 2;
            this.menuItemNewCSharp.Text = "&C# Template";
            this.menuItemNewCSharp.Click += new System.EventHandler(this.menuItemNewCSharp_Click);
            // 
            // menuItemNewVBNet
            // 
            this.menuItemNewVBNet.Index = 3;
            this.menuItemNewVBNet.Text = "&VB.Net Template";
            this.menuItemNewVBNet.Click += new System.EventHandler(this.menuItemNewVBNet_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 4;
            this.menuItem11.Text = "-";
            // 
            // menuItemNewProject
            // 
            this.menuItemNewProject.Index = 5;
            this.menuItemNewProject.Text = "&Project";
            this.menuItemNewProject.Click += new System.EventHandler(this.menuItemNewProject_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 6;
            this.menuItem4.Text = "-";
            // 
            // menuItemNewBrowser
            // 
            this.menuItemNewBrowser.Index = 7;
            this.menuItemNewBrowser.Text = "&Database Browser";
            this.menuItemNewBrowser.Click += new System.EventHandler(this.MyMetaBrowser_Click);
            // 
            // menuItemTemplateBrowser
            // 
            this.menuItemTemplateBrowser.Index = 8;
            this.menuItemTemplateBrowser.Text = "&Template Browser";
            this.menuItemTemplateBrowser.Click += new System.EventHandler(this.menuItemTemplateBrowser_Click);
            // 
            // menuItemFileOpen
            // 
            this.menuItemFileOpen.Index = 1;
            this.menuItemFileOpen.Text = "&Open";
            this.menuItemFileOpen.Click += new System.EventHandler(this.FileOpen_Click);
            // 
            // menuItemRecentFiles
            // 
            this.menuItemRecentFiles.Index = 2;
            this.menuItemRecentFiles.Text = "&Recent Files";
            this.menuItemRecentFiles.Visible = false;
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text = "-";
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 4;
            this.menuItemExit.MergeOrder = 2;
            this.menuItemExit.Text = "E&xit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItemEdit
            // 
            this.menuItemEdit.Index = 1;
            this.menuItemEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem8,
																						 this.menuItem9,
																						 this.menuItem10,
																						 this.menuItem3,
																						 this.menuItemFreeMemory,
																						 this.menuItem2,
																						 this.menuItemDefaultProperties});
            this.menuItemEdit.MergeOrder = 1;
            this.menuItemEdit.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.menuItemEdit.Text = "&Edit";
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 0;
            this.menuItem8.Text = "Cu&t";
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 1;
            this.menuItem9.Text = "&Copy";
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 2;
            this.menuItem10.Text = "&Paste";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 3;
            this.menuItem3.MergeOrder = 3;
            this.menuItem3.Text = "-";
            // 
            // menuItemFreeMemory
            // 
            this.menuItemFreeMemory.Index = 4;
            this.menuItemFreeMemory.MergeOrder = 3;
            this.menuItemFreeMemory.Text = "Free Memory";
            this.menuItemFreeMemory.Click += new System.EventHandler(this.menuItemFreeMemory_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 5;
            this.menuItem2.MergeOrder = 4;
            this.menuItem2.Text = "-";
            // 
            // menuItemDefaultProperties
            // 
            this.menuItemDefaultProperties.Index = 6;
            this.menuItemDefaultProperties.MergeOrder = 4;
            this.menuItemDefaultProperties.Text = "Default Settings ...";
            this.menuItemDefaultProperties.Click += new System.EventHandler(this.menuItemDefaultProperties_Click);
            // 
            // menuItemTemplate
            // 
            this.menuItemTemplate.Index = 2;
            this.menuItemTemplate.MergeOrder = 2;
            this.menuItemTemplate.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.menuItemTemplate.Text = "&Template";
            this.menuItemTemplate.Visible = false;
            // 
            // menuItemProject
            // 
            this.menuItemProject.Index = 3;
            this.menuItemProject.MergeOrder = 3;
            this.menuItemProject.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.menuItemProject.Text = "&Project";
            this.menuItemProject.Visible = false;
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 4;
            this.menuItem5.MdiList = true;
            this.menuItem5.MergeOrder = 4;
            this.menuItem5.Text = "&Window";
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.Index = 5;
            this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemContents,
																						 this.menuItem7,
																						 this.menuItemdOOdadsHelp,
																						 this.menuItemMyMetaHelp,
																						 this.menuItemZuesHelp,
																						 this.menuItem13,
																						 this.menuItemDnpUtils,
																						 this.menuItem12,
																						 this.menuItemWebSite,
																						 this.menuItem6,
																						 this.menuItemHelpAbout});
            this.menuItemHelp.MergeOrder = 5;
            this.menuItemHelp.Text = "&Help";
            // 
            // menuItemContents
            // 
            this.menuItemContents.Index = 0;
            this.menuItemContents.Text = "Contents";
            this.menuItemContents.Click += new System.EventHandler(this.menuItemContents_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 1;
            this.menuItem7.Text = "-";
            // 
            // menuItemdOOdadsHelp
            // 
            this.menuItemdOOdadsHelp.Index = 2;
            this.menuItemdOOdadsHelp.Text = "dOOdads API ...";
            this.menuItemdOOdadsHelp.Click += new System.EventHandler(this.menuItemdOOdadsHelp_Click);
            // 
            // menuItemMyMetaHelp
            // 
            this.menuItemMyMetaHelp.Index = 3;
            this.menuItemMyMetaHelp.Text = "MyMeta API  ...";
            this.menuItemMyMetaHelp.Click += new System.EventHandler(this.menuItemMyMetaHelp_Click);
            // 
            // menuItemZuesHelp
            // 
            this.menuItemZuesHelp.Index = 4;
            this.menuItemZuesHelp.Text = "Zeus Script API  ...";
            this.menuItemZuesHelp.Click += new System.EventHandler(this.menuItemZuesHelp_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 7;
            this.menuItem12.Text = "-";
            // 
            // menuItemWebSite
            // 
            this.menuItemWebSite.Index = 8;
            this.menuItemWebSite.Text = "MyGeneration &Website";
            this.menuItemWebSite.Click += new System.EventHandler(this.menuItemWebSite_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 9;
            this.menuItem6.Text = "-";
            // 
            // menuItemHelpAbout
            // 
            this.menuItemHelpAbout.Index = 10;
            this.menuItemHelpAbout.Text = "&About";
            this.menuItemHelpAbout.Click += new System.EventHandler(this.menuItemHelpAbout_Click);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 665);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						 this.statusRow,
																						 this.statusCol});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(952, 24);
            this.statusBar.TabIndex = 0;
            // 
            // statusRow
            // 
            this.statusRow.Width = 90;
            // 
            // toolBar1
            // 
            this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.toolBarButton_fileOpen,
																						this.toolBarButton_TemplateBrowser,
																						this.toolBarButton_OpenBrowser,
																						this.toolBarButton_newProject,
																						this.toolBarButton_OpenOutputFolder,
																						this.toolBarButton4,
																						this.toolBarButton_newJavaTemplate,
																						this.toolBarButton_newVBTemplate,
																						this.toolBarButton_newCSharpTemplate,
																						this.toolBarButton_newVBNetTemplate,
																						this.toolBarButton2,
																						this.toolBarButton_MetaBrowser,
																						this.toolBarButton_LanguageMappings,
																						this.toolBarButton_DbTargetMappings,
																						this.toolBarButton3,
																						this.toolBarButton_UserData,
																						this.toolBarButton_GlobalUserData});
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.toolbarImages;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(952, 36);
            this.toolBar1.TabIndex = 1;
            this.toolBar1.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // toolBarButton_fileOpen
            // 
            this.toolBarButton_fileOpen.ImageIndex = 0;
            this.toolBarButton_fileOpen.Tag = "fileOpen";
            this.toolBarButton_fileOpen.ToolTipText = "File Open ...";
            // 
            // toolBarButton_TemplateBrowser
            // 
            this.toolBarButton_TemplateBrowser.ImageIndex = 15;
            this.toolBarButton_TemplateBrowser.Tag = "templateBrowser";
            this.toolBarButton_TemplateBrowser.ToolTipText = "Template Browser";
            // 
            // toolBarButton_OpenBrowser
            // 
            this.toolBarButton_OpenBrowser.ImageIndex = 6;
            this.toolBarButton_OpenBrowser.Tag = "DbBrowser";
            this.toolBarButton_OpenBrowser.ToolTipText = "MyMeta Browser";
            // 
            // toolBarButton_newProject
            // 
            this.toolBarButton_newProject.ImageIndex = 16;
            this.toolBarButton_newProject.Tag = "newProject";
            this.toolBarButton_newProject.ToolTipText = "Create a New Project";
            // 
            // toolBarButton_OpenOutputFolder
            // 
            this.toolBarButton_OpenOutputFolder.ImageIndex = 17;
            this.toolBarButton_OpenOutputFolder.Tag = "openOutputFolder";
            this.toolBarButton_OpenOutputFolder.ToolTipText = "Launch the default generated Output folder";
            // 
            // toolBarButton4
            // 
            this.toolBarButton4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton_newJavaTemplate
            // 
            this.toolBarButton_newJavaTemplate.ImageIndex = 10;
            this.toolBarButton_newJavaTemplate.Tag = "newJava";
            this.toolBarButton_newJavaTemplate.ToolTipText = "Create New JScript Template";
            // 
            // toolBarButton_newVBTemplate
            // 
            this.toolBarButton_newVBTemplate.ImageIndex = 11;
            this.toolBarButton_newVBTemplate.Tag = "newVB";
            this.toolBarButton_newVBTemplate.ToolTipText = "Create New VBScript Template";
            // 
            // toolBarButton_newCSharpTemplate
            // 
            this.toolBarButton_newCSharpTemplate.ImageIndex = 12;
            this.toolBarButton_newCSharpTemplate.Tag = "newC#";
            this.toolBarButton_newCSharpTemplate.ToolTipText = "Create New C# Template";
            // 
            // toolBarButton_newVBNetTemplate
            // 
            this.toolBarButton_newVBNetTemplate.ImageIndex = 13;
            this.toolBarButton_newVBNetTemplate.Tag = "newVB.NET";
            this.toolBarButton_newVBNetTemplate.ToolTipText = "Create New VB.Net Template";
            // 
            // toolBarButton2
            // 
            this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton_MetaBrowser
            // 
            this.toolBarButton_MetaBrowser.ImageIndex = 5;
            this.toolBarButton_MetaBrowser.Tag = "metaBrowser";
            this.toolBarButton_MetaBrowser.ToolTipText = "MyMeta Properties";
            // 
            // toolBarButton_LanguageMappings
            // 
            this.toolBarButton_LanguageMappings.ImageIndex = 3;
            this.toolBarButton_LanguageMappings.Tag = "languageMappings";
            this.toolBarButton_LanguageMappings.ToolTipText = "Language Mappings";
            // 
            // toolBarButton_DbTargetMappings
            // 
            this.toolBarButton_DbTargetMappings.ImageIndex = 4;
            this.toolBarButton_DbTargetMappings.Tag = "dbTargetMappings";
            this.toolBarButton_DbTargetMappings.ToolTipText = "DbTarget Mappings";
            // 
            // toolBarButton3
            // 
            this.toolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton_UserData
            // 
            this.toolBarButton_UserData.ImageIndex = 7;
            this.toolBarButton_UserData.Tag = "userMetaData";
            this.toolBarButton_UserData.ToolTipText = "User Meta Data";
            // 
            // toolBarButton_GlobalUserData
            // 
            this.toolBarButton_GlobalUserData.ImageIndex = 14;
            this.toolBarButton_GlobalUserData.Tag = "globalUserMetaData";
            this.toolBarButton_GlobalUserData.ToolTipText = "Global User Data";
            // 
            // toolbarImages
            // 
            this.toolbarImages.ImageSize = new System.Drawing.Size(24, 24);
            this.toolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbarImages.ImageStream")));
            this.toolbarImages.TransparentColor = System.Drawing.Color.Magenta;
            // 
            // dockManager
            // 
            this.dockManager.ActiveAutoHideContent = null;
            this.dockManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockManager.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.dockManager.Location = new System.Drawing.Point(0, 36);
            this.dockManager.Name = "dockManager";
            this.dockManager.Size = new System.Drawing.Size(952, 629);
            this.dockManager.TabIndex = 1;
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 5;
            this.menuItem13.Text = "-";
            // 
            // menuItemDnpUtils
            // 
            this.menuItemDnpUtils.Index = 6;
            this.menuItemDnpUtils.Text = "DnpUtils Plugin ...";
            this.menuItemDnpUtils.Click += new System.EventHandler(this.menuItemDnpUtils_Click);
            // 
            // MDIParent
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(952, 689);
            this.Controls.Add(this.dockManager);
            this.Controls.Add(this.toolBar1);
            this.Controls.Add(this.statusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "MDIParent";
            this.Text = "MyGeneration";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MDIParent_Closing);
            this.Load += new System.EventHandler(this.MDIParent_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MDIParent_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MDIParent_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.statusRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusCol)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        #region Menu Events
        private void MyMetaBrowser_Click(object sender, System.EventArgs e)
        {
            OpenDatabaseBrowser();
        }

        private void FileOpen_Click(object sender, System.EventArgs e)
        {
            FileOpen();
        }

        private void menuItemOpenRecentFile_Click(object sender, System.EventArgs e)
        {
            if (sender is MenuItem)
            {
                MenuItem item = sender as MenuItem;
                this.LaunchFileEditor(item.Text);

                AddRecentFile(item.Text);
            }
        }

        private void menuItemFreeMemory_Click(object sender, System.EventArgs e)
        {
            FlushMemory();
        }

        private void menuItemHelpAbout_Click(object sender, System.EventArgs e)
        {
            NewAbout about = new NewAbout();
            about.ShowDialog();
            about.Dispose();
        }

        private void menuItemDefaultProperties_Click(object sender, System.EventArgs e)
        {
            DefaultProperties dp = new DefaultProperties(this);
            DialogResult result = dp.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                IMyGenContent bw = null;
                DockContentCollection coll = this.dockManager.Contents;

                DefaultSettings settings = DefaultSettings.Instance;

                for (int i = 0; i < coll.Count; i++)
                {
                    bw = coll[i] as IMyGenContent;

                    if (bw != null)
                    {
                        try
                        {
                            bw.Alert(dp, "UpdateDefaultSettings");
                        }
#if DEBUG
						catch (Exception ex) { throw ex; }
#else
                        catch { }
#endif
                    }
                }
            }
        }

        private void menuItemExit_Click(object sender, System.EventArgs e)
        {
            if (Shutdown(true))
            {
                this.Close();
                Application.Exit();
            }
        }

        private void menuItemWebSite_Click(object sender, System.EventArgs e)
        {
            Program.LaunchBrowser(URL_HOMEPAGE);
        }

        private void menuItemDocs_Click(object sender, System.EventArgs e)
        {
            Program.LaunchBrowser(URL_DOCUMENTATION);
        }

        private void menuItemMyMetaHelp_Click(object sender, System.EventArgs e)
        {
            try
            {
                Process myProcess = new Process();

                myProcess.StartInfo.FileName = this.startupPath + @"\MyMeta.chm";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                myProcess.Start();
            }
#if DEBUG
				catch (Exception ex) { throw ex; }
#else
            catch { }
#endif

        }

        private void menuItemdOOdadsHelp_Click(object sender, System.EventArgs e)
        {
            try
            {
                Process myProcess = new Process();

                myProcess.StartInfo.FileName = this.startupPath + @"\dOOdads.chm";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                myProcess.Start();
            }
#if DEBUG
				catch (Exception ex) { throw ex; }
#else
            catch { }
#endif

        }

        private void menuItemZuesHelp_Click(object sender, System.EventArgs e)
        {
            try
            {
                Process myProcess = new Process();

                myProcess.StartInfo.FileName = this.startupPath + @"\Zeus.chm";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                myProcess.Start();
            }
#if DEBUG
				catch (Exception ex) { throw ex; }
#else
            catch { }
#endif

        }

        private void menuItemDnpUtils_Click(object sender, System.EventArgs e)
        {
            try
            {
                Process myProcess = new Process();

                myProcess.StartInfo.FileName = this.startupPath + @"\Dnp.Utils.chm";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                myProcess.Start();
            }
#if DEBUG
				catch (Exception ex) { throw ex; }
#else
            catch { }
#endif
        }

        private void menuItemTemplateBrowser_Click(object sender, System.EventArgs e)
        {
            templateBrowser.Show(dockManager);
        }

        private void menuItemContents_Click(object sender, System.EventArgs e)
        {
            ShowMyGenerationHelp();
        }

        protected void WindowFileNewJScriptTemplate_Clicked(object sender, System.EventArgs e)
        {
            OpenTemplateEditor(ZeusConstants.Engines.MICROSOFT_SCRIPT, ZeusConstants.Languages.JSCRIPT);
        }

        protected void WindowFileNewVBScriptTemplate_Clicked(object sender, System.EventArgs e)
        {
            OpenTemplateEditor(ZeusConstants.Engines.MICROSOFT_SCRIPT, ZeusConstants.Languages.VBSCRIPT);
        }

        private void menuItemNewCSharp_Click(object sender, System.EventArgs e)
        {
            OpenTemplateEditor(ZeusConstants.Engines.DOT_NET_SCRIPT, ZeusConstants.Languages.CSHARP);
        }

        private void menuItemNewVBNet_Click(object sender, System.EventArgs e)
        {
            OpenTemplateEditor(ZeusConstants.Engines.DOT_NET_SCRIPT, ZeusConstants.Languages.VBNET);
        }

        private void menuItemNewProject_Click(object sender, System.EventArgs e)
        {
            OpenProjectEditor();
        }

        protected void WindowCascade_Clicked(object sender, System.EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        protected void WindowHorizontal_Clicked(object sender, System.EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        protected void WindowVertical_Clicked(object sender, System.EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }
        #endregion

        #region Toolbar Events
        private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            switch (e.Button.Tag as string)
            {
                case "DbBrowser":
                    OpenDatabaseBrowser();
                    break;

                case "fileOpen":
                    FileOpen();
                    break;

                case "openOutputFolder":
                    OpenOutputFolder();
                    break;

                case "newJava":
                    OpenTemplateEditor(ZeusConstants.Engines.MICROSOFT_SCRIPT, ZeusConstants.Languages.JSCRIPT);
                    break;

                case "newVB":
                    OpenTemplateEditor(ZeusConstants.Engines.MICROSOFT_SCRIPT, ZeusConstants.Languages.VBSCRIPT);
                    break;

                case "newC#":
                    OpenTemplateEditor(ZeusConstants.Engines.DOT_NET_SCRIPT, ZeusConstants.Languages.CSHARP);
                    break;

                case "newVB.NET":
                    OpenTemplateEditor(ZeusConstants.Engines.DOT_NET_SCRIPT, ZeusConstants.Languages.VBNET);
                    break;

                case "newProject":
                    this.OpenProjectEditor();
                    break;

                case "languageMappings":
                    this.languageMappings.Show(dockManager);
                    break;

                case "dbTargetMappings":
                    this.dbTargetMappings.Show(dockManager);
                    break;

                case "metaBrowser":
                    this.metaProperties.Show(dockManager);
                    break;

                case "userMetaData":
                    this.userMetaData.Show(dockManager);
                    break;

                case "globalUserMetaData":
                    this.globalUserMetaData.Show(dockManager);
                    break;

                case "templateBrowser":
                    this.templateBrowser.Show(dockManager);
                    break;

                case "tileVert":
                    this.LayoutMdi(MdiLayout.TileVertical);
                    break;

                case "tileHorz":
                    this.LayoutMdi(MdiLayout.TileHorizontal);
                    break;

                case "cascade":
                    this.LayoutMdi(MdiLayout.Cascade);
                    break;
            }
        }
        #endregion

        #region MDIParent Event Handlers

        private void MDIParent_Load(object sender, System.EventArgs e)
        {
            DefaultSettings settings = DefaultSettings.Instance;

            if (settings.CompactMemoryOnStartup) FlushMemory();

            switch (settings.WindowState)
            {
                case "Maximized":

                    this.WindowState = FormWindowState.Maximized;
                    break;

                case "Minimized":

                    this.WindowState = FormWindowState.Minimized;
                    break;

                case "Normal":

                    int x = Convert.ToInt32(settings.WindowPosLeft);
                    int y = Convert.ToInt32(settings.WindowPosTop);
                    int w = Convert.ToInt32(settings.WindowPosWidth);
                    int h = Convert.ToInt32(settings.WindowPosHeight);

                    this.Location = new System.Drawing.Point(x, y);
                    this.Size = new Size(w, h);
                    break;
            }

            if (settings.FirstLoad)
            {
                DefaultProperties dp = new DefaultProperties(this);
                DialogResult result = dp.ShowDialog(this);
            }

            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), DOCK_CONFIG_FILE);
            try
            {
                if (File.Exists(configFile))
                {
                    dockManager.LoadFromXml(configFile, new DeserializeDockContent(GetContentFromPersistString));

                    //TemplateEditor e1 = this.OpenTemplateEditor();
                    foreach (DockContent content in dockManager.Contents)
                    {
                        if (content is IMyGenContent)
                        {
                            IMyGenContent baseWindow = content as IMyGenContent;
                            //baseWindow.ResetMenu();
                        }
                    }
                }
            }
#if DEBUG
			catch (Exception ex)
			{
				throw ex;
			}
#else
            catch
            {
                if (File.Exists(configFile))
                {
                    try
                    {
                        File.Delete(configFile);
                    }
                    catch { }
                }
            }
#endif

            if (this.startupFiles != null)
            {
                foreach (string filename in this.startupFiles)
                {
                    try
                    {
                        LaunchFileEditor(filename);
                    }
#if DEBUG
				catch (Exception ex) { throw ex; }
#else
                    catch { }
#endif

                }
            }

            if (settings.CompactMemoryOnStartup)
            {
                FlushMemory();
            }

            if (this.dockingStartupTemplates.Count > 0)
            {
                foreach (string filename in this.dockingStartupTemplates)
                {
                    try
                    {
                        if (filename == EMPTY_CONSTANT)
                        {
                            this.OpenTemplateEditor();
                        }
                        else
                        {
                            this.OpenTemplateEditor(filename);
                        }
                    }
#if DEBUG
				catch (Exception ex) { throw ex; }
#else
                    catch { }
#endif

                }
            }

            if (this.dockingStartupProjects.Count > 0)
            {
                foreach (string filename in this.dockingStartupProjects)
                {
                    try
                    {
                        this.OpenProjectEditor(filename);
                    }
#if DEBUG
				catch (Exception ex) { throw ex; }
#else
                    catch { }
#endif

                }
            }

            if (settings.CompactMemoryOnStartup)
            {
                this.Update();
                FlushMemory();
            }

            FinishVersionCheck(settings);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // This saves the window state and size
            DefaultSettings ds = DefaultSettings.Instance;
            FormWindowState state = this.WindowState;

            switch (state)
            {
                case FormWindowState.Normal:

                    ds.WindowState = "Normal";
                    break;

                case FormWindowState.Maximized:

                    ds.WindowState = "Maximized";
                    break;

                case FormWindowState.Minimized:

                    ds.WindowState = "Minimized";
                    break;
            }

            Rectangle pos = this.RectangleToScreen(this.ClientRectangle);

            ds.WindowPosTop = this.Top.ToString();
            ds.WindowPosLeft = this.Left.ToString();
            ds.WindowPosWidth = this.Width.ToString();
            ds.WindowPosHeight = this.Height.ToString();

            ds.Save();

            base.OnClosing(e);
        }

        private void MDIParent_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string filename in filenames)
            {
                LaunchFileEditor(filename);
            }
        }

        private void MDIParent_Layout(object sender, System.Windows.Forms.LayoutEventArgs e)
        {
            DefaultSettings settings = DefaultSettings.Instance;

            switch (settings.WindowState)
            {
                case "Maximized":

                    this.WindowState = FormWindowState.Maximized;
                    break;

                case "Minimized":

                    this.WindowState = FormWindowState.Minimized;
                    break;

                case "Normal":

                    int x = Convert.ToInt32(settings.WindowPosLeft);
                    int y = Convert.ToInt32(settings.WindowPosTop);
                    int w = Convert.ToInt32(settings.WindowPosWidth);
                    int h = Convert.ToInt32(settings.WindowPosHeight);


                    this.Location = new System.Drawing.Point(x, y);
                    this.Size = new Size(w, h);
                    break;
            }
        }

        private void MDIParent_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                bool foundValidFile = false;
                string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string filename in filenames)
                {
                    foreach (string ext in this.validExtentions)
                    {
                        if (filename.ToLower().EndsWith(ext))
                        {
                            foundValidFile = true;
                            break;
                        }
                    }

                    foreach (string ext in this.validProjectExtentions)
                    {
                        if (filename.ToLower().EndsWith(ext))
                        {
                            foundValidFile = true;
                            break;
                        }
                    }
                    if (foundValidFile) break;
                }

                // allow them to continue
                // (without this, the cursor stays a "NO" symbol
                if (foundValidFile)
                {
                    e.Effect = DragDropEffects.All;
                }
            }
        }

        private void MDIParent_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.Shutdown(true))
            {
                e.Cancel = true;
                return;
            }

            Application.Exit();
        }
        #endregion

        #region MyMeta Helper Methods
        private void OpenDatabaseBrowser()
        {
            metaDataBrowser.Show(dockManager);
        }
        #endregion

        #region Template Editor Helper Methods
        private TemplateEditor OpenTemplateEditor(string engine, string language)
        {
            TemplateEditor template = new TemplateEditor(this);
            template.FileNew("ENGINE", engine, "LANGUAGE", language);
            template.Show(dockManager);
            return template;
        }

        private TemplateEditor OpenTemplateEditor()
        {
            TemplateEditor edit = new TemplateEditor(this);
            edit.FileNew("ENGINE", ZeusConstants.Engines.DOT_NET_SCRIPT, "LANGUAGE", ZeusConstants.Languages.CSHARP);
            edit.Show(dockManager);
            return edit;
        }

        private TemplateEditor OpenTemplateEditor(string filename)
        {
            TemplateEditor edit = null;

            if (File.Exists(filename))
            {
                bool isopen = IsTemplateOpen(filename);

                if (!isopen)
                {
                    edit = new TemplateEditor(this);
                    edit.FileOpen(filename);
                    edit.Show(dockManager);
                }
                else
                {
                    edit = GetTemplateEditor(filename);
                    edit.Activate();
                }
            }
            else
            {
                edit = OpenTemplateEditor();
            }

            return edit;
        }

        private void CloseTemplateEditor(string filename)
        {
            // Check to see if file is already open
            FileInfo inf = new FileInfo(filename);
            string tmp = inf.FullName;
            foreach (DockContent cont in dockManager.Contents)
            {
                if (cont is TemplateEditor)
                {
                    TemplateEditor editor = cont as TemplateEditor;
                    if (tmp.ToUpper() == editor.CompleteFilePath.ToUpper())
                    {
                        editor.Close();
                        break;
                    }
                }
            }
        }

        private void RefreshTemplateEditor(string uniqueId)
        {
            foreach (DockContent cont in dockManager.Contents)
            {
                if (cont is TemplateEditor)
                {
                    TemplateEditor editor = cont as TemplateEditor;
                    if (uniqueId.ToLower() == editor.UniqueID.ToLower())
                    {
                        if (MessageBox.Show(this,
                            "Template [" + editor.Title + "] has been updated outside of the Template Editor.\r\nWould you like to refresh it?",
                            "Refresh Updated Template?",
                            MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            string filename = editor.CompleteFilePath;
                            editor.Close();

                            if (File.Exists(filename))
                            {
                                editor = new TemplateEditor(this);
                                editor.FileOpen(filename);
                                editor.Show(dockManager);
                            }
                        }

                        break;
                    }
                }
            }
        }

        public bool IsTemplateOpen(string filename)
        {
            return IsTemplateOpen(filename, null);
        }

        public bool IsTemplateOpen(string filename, TemplateEditor exclude)
        {
            TemplateEditor editor = GetTemplateEditor(filename, exclude);
            return (editor != null);
        }

        public TemplateEditor GetTemplateEditor(string filename)
        {
            return GetTemplateEditor(filename, null);
        }

        public TemplateEditor GetTemplateEditor(string filename, TemplateEditor exclude)
        {
            TemplateEditor editor = null;

            FileInfo inf = new FileInfo(filename);
            string tmp = inf.FullName;
            foreach (DockContent cont in dockManager.Contents)
            {
                if ((cont is TemplateEditor) && (cont != exclude))
                {
                    editor = cont as TemplateEditor;
                    if (tmp.ToUpper() == editor.CompleteFilePath.ToUpper())
                    {
                        break;
                    }
                    else
                    {
                        editor = null;
                    }
                }
            }
            return editor;
        }

        private void templateBrowser_TemplateOpen(object sender, EventArgs e)
        {
            OpenTemplateEditor(sender.ToString());
        }

        private void templateBrowser_TemplateDelete(object sender, EventArgs e)
        {
            CloseTemplateEditor(sender.ToString());
        }

        private void templateBrowser_TemplateUpdate(object sender, EventArgs e)
        {
            RefreshTemplateEditor(sender.ToString());
        }

        public IEditControl CurrentEditControl
        {
            get
            {
                TemplateEditor editor = dockManager.ActiveDocument as TemplateEditor;
                if (editor != null)
                {
                    return editor.CurrentEditControl;
                }

                return null;
            }
        }
        #endregion

        #region Project Editor Helper Methods
        public bool IsProjectOpen(string filename)
        {
            return IsProjectOpen(filename, null);
        }

        public bool IsProjectOpen(string filename, ProjectBrowser exclude)
        {
            ProjectBrowser editor = GetProjectBrowser(filename, exclude);
            return (editor != null);
        }

        public ProjectBrowser GetProjectBrowser(string filename)
        {
            return GetProjectBrowser(filename, null);
        }

        public ProjectBrowser GetProjectBrowser(string filename, ProjectBrowser exclude)
        {
            ProjectBrowser editor = null;

            FileInfo inf = new FileInfo(filename);
            string tmp = inf.FullName;
            foreach (DockContent cont in dockManager.Contents)
            {
                if ((cont is ProjectBrowser) && (cont != exclude))
                {
                    editor = cont as ProjectBrowser;
                    if (tmp.ToUpper() == editor.CompleteFilePath.ToUpper())
                    {
                        break;
                    }
                    else
                    {
                        editor = null;
                    }
                }
            }
            return editor;
        }

        private ProjectBrowser OpenProjectEditor()
        {
            ProjectBrowser proj = new ProjectBrowser(this);
            proj.CreateNewProject();
            proj.Show(dockManager);

            return proj;
        }

        private ProjectBrowser OpenProjectEditor(string filename)
        {
            ProjectBrowser proj = null;

            bool isopen = IsProjectOpen(filename);

            if (!isopen)
            {
                proj = new ProjectBrowser(this);
                proj.LoadProject(filename);
                proj.Show(dockManager);
            }

            return proj;
        }
        #endregion

        #region Find/Replace Methods
        public void LaunchFindReplace(bool enableReplace) //, string findText)
        {
            if (enableReplace)
            {
                if (!ZeusScintillaControl.ReplaceDialog.Visible)
                {
                    ZeusScintillaControl.ReplaceDialog.Show(this);
                }
            }
            else
            {
                if (!ZeusScintillaControl.FindDialog.Visible)
                {
                    ZeusScintillaControl.FindDialog.Show(this);
                }
            }
            /*if (_findDialog == null)
            {
                _findDialog = new SimpleFindReplace();
                this.AddOwnedForm(this._findDialog);
            }

            this._findDialog.Show(this, enableReplace, findText);*/
        }
        #endregion

        #region Check Application Version on Server
        private void StartVersionCheck(DefaultSettings settings)
        {
            if (settings.CheckForNewBuild)
            {
                versionInfo = new VersionInfo();

                try
                {
                    asyncResult = versionInfo.BeginGetVersion(null, null);
                }
                catch
                {
                    webServiceError = true;
                }
            }
        }

        private void FinishVersionCheck(DefaultSettings settings)
        {
            if (settings.CheckForNewBuild)
            {
                if (webServiceError == false)
                {
                    try
                    {
                        string newVersion = versionInfo.EndGetVersion(asyncResult);

                        Assembly asmblyMyGen = System.Reflection.Assembly.GetAssembly(typeof(NewAbout));
                        string thisVersion = asmblyMyGen.GetName().Version.ToString();

                        System.Version newVersionObject = new System.Version(newVersion);

                        if (newVersionObject > asmblyMyGen.GetName().Version)
                        {
                            this.Update();

                            UpdatesForm form = new UpdatesForm();
                            form.NewVersion = newVersion;
                            form.ThisVersion = thisVersion;
                            form.UpgradeText = versionInfo.GetUpdateText();
                            DialogResult result = form.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                DownloadLatestVersion();
                            }
                        }
                    }
                    catch { };
                }
            }

            this.asyncResult = null;
            this.versionInfo = null;
        }

        private void DownloadLatestVersion()
        {
            try
            {
                Program.LaunchBrowser(URL_LATESTVERSION, ProcessWindowStyle.Minimized, true);
                /*Process myProcess = new Process();

                myProcess.StartInfo.FileName = URL_LATESTVERSION;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                myProcess.Start();*/

                Application.Exit();
            }
#if DEBUG
				catch (Exception ex) { throw ex; }
#else
            catch { }
#endif

        }
        #endregion

        public MetaProperties MetaDataWindow
        {
            get
            {
                return this.metaProperties;
            }
        }

        public UserMetaData UserMetaWindow
        {
            get
            {
                return this.userMetaData;
            }
        }

        public GlobalUserMetaData GlobalUserMetaWindow
        {
            get
            {
                return this.globalUserMetaData;
            }
        }

        public FindForm FindDialog
        {
            get { return findDialog; }
        }

        public ReplaceForm ReplaceDialog
        {
            get { return replaceDialog; }
        }

        public ScintillaConfigureDelegate ConfigureDelegate
        {
            get { return configureDelegate; }
        }

        public DockPanel DockPanel
        {
            get { return this.dockManager; }
        }

        protected void MDIChildActivated(object sender, System.EventArgs e)
        {
            if (null == this.ActiveMdiChild)
            {
                statusBar.Text = string.Empty;
            }
            else
            {
                statusBar.Text = this.ActiveMdiChild.Text;

                if (ActiveMdiChild is TemplateEditor)
                {
                    TemplateEditor ed = ActiveMdiChild as TemplateEditor;
                    ed.CurrentEditControl.Activate();
                }
            }
        }

        protected TemplateBrowser templateBrowser
        {
            get
            {
                if (_templateBrowser == null)
                {
                    _templateBrowser = new TemplateBrowser(this);
                    this._templateBrowser.TemplateOpen += new EventHandler(templateBrowser_TemplateOpen);
                    this._templateBrowser.TemplateUpdate += new EventHandler(templateBrowser_TemplateUpdate);
                    this._templateBrowser.TemplateDelete += new EventHandler(templateBrowser_TemplateDelete);
                }

                return _templateBrowser;
            }
        }

        private void OpenOutputFolder()
        {
            DefaultSettings ds = DefaultSettings.Instance;
            Process p = new Process();
            p.StartInfo.FileName = "explorer";
            p.StartInfo.Arguments = "/e," + ds.DefaultOutputDirectory;
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }

        private void FileOpen()
        {
            DefaultSettings settings = DefaultSettings.Instance;

            Stream myStream;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = settings.DefaultTemplateDirectory;
            openFileDialog.Filter = PROJECT_FILE_TYPES + TemplateEditor.FILE_TYPES;
            openFileDialog.FilterIndex = TemplateEditor.DEFAULT_OPEN_FILE_TYPE_INDEX + 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                myStream = openFileDialog.OpenFile();
                if (null != myStream)
                {
                    myStream.Close();

                    foreach (string filename in openFileDialog.FileNames)
                    {
                        int extindex = filename.LastIndexOf(".");
                        if (extindex >= 0)
                        {
                            FileInfo f = new FileInfo(filename);
                            if (f.Exists)
                            {
                                AddRecentFile(f.FullName);

                                string ext = filename.Substring(extindex);
                                bool validProjectExt = false;
                                foreach (string vpe in validProjectExtentions)
                                {
                                    if (vpe == ext)
                                        validProjectExt = true;
                                }

                                if (validProjectExt)
                                {
                                    this.OpenProjectEditor(filename);
                                }
                                else
                                {
                                    this.OpenTemplateEditor(filename);
                                }
                            }
                        }
                    }
                }
            }
        }

        #region Dock Manager related members
        private bool Shutdown(bool allowPrevent)
        {
            try
            {
                IMyGenContent bw = null;
                DockContentCollection coll = this.dockManager.Contents;
                bool canClose = true;

                for (int i = 0; i < coll.Count; i++)
                {
                    bw = coll[i] as IMyGenContent;

                    // We need the MetaDataBrowser window to be closed last because it houses the UserMetaData.
                    if (null == (bw as MetaDataBrowser))
                    {
                        canClose = bw.CanClose(allowPrevent);

                        if (allowPrevent && !canClose)
                        {
                            return false;
                        }
                    }

                    // Close and hidden windows.
                    if (coll[i].DockHandler.IsHidden)
                    {
                        coll[i].DockHandler.VisibleState = DockState.Hidden;
                        coll[i].DockHandler.Close();
                    }
                }

                string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), DOCK_CONFIG_FILE);
                dockManager.SaveAsXml(configFile);
            }
#if DEBUG
				catch (Exception ex) { throw ex; }
#else
            catch { }
#endif

            return true;
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            DockContent returnDockContent = null;

            try
            {
                if (persistString == typeof(LanguageMappings).ToString())
                {
                    returnDockContent = this.languageMappings;
                }
                else if (persistString == typeof(DbTargetMappings).ToString())
                {
                    returnDockContent = this.dbTargetMappings;
                }
                else if (persistString == typeof(MetaProperties).ToString())
                {
                    returnDockContent = this.metaProperties;
                }
                else if (persistString == typeof(MetaDataBrowser).ToString())
                {
                    returnDockContent = this.metaDataBrowser;
                }
                else if (persistString == typeof(UserMetaData).ToString())
                {
                    returnDockContent = this.userMetaData;
                }
                else if (persistString == typeof(GlobalUserMetaData).ToString())
                {
                    returnDockContent = this.globalUserMetaData;
                }
                else if (persistString == typeof(TemplateBrowser).ToString())
                {
                    returnDockContent = this.templateBrowser;
                }
                else if (persistString == typeof(ProjectBrowser).ToString())
                {
                    returnDockContent = OpenProjectEditor();
                }
                else if (persistString == typeof(TemplateEditor).ToString())
                {
                    returnDockContent = OpenTemplateEditor();
                }
                else
                {
                    string[] parsedStrings = persistString.Split(new char[] { ',' });
                    if (parsedStrings.Length == 2)
                    {
                        if (parsedStrings[0] == typeof(TemplateEditor).ToString())
                        {
                            //returnDockContent = OpenTemplateEditor(parsedStrings[1]);
                            dockingStartupTemplates.Add(parsedStrings[1]);
                        }
                        else if (parsedStrings[0] == typeof(ProjectBrowser).ToString())
                        {
                            if (!dockingStartupProjects.Contains(parsedStrings[1]))
                            {
                                returnDockContent = OpenProjectEditor(parsedStrings[1]);
                            }
                            else
                            {
                                returnDockContent = OpenProjectEditor();
                            }
                        }
                        else
                        {
                            //returnDockContent = OpenTemplateEditor();
                            dockingStartupTemplates.Add(EMPTY_CONSTANT);
                        }
                    }
                    else
                    {
                        //returnDockContent = OpenTemplateEditor();
                        dockingStartupTemplates.Add(EMPTY_CONSTANT);
                    }
                }
            }
#if DEBUG
			catch (Exception ex) { throw ex; }
#else
            catch { }
#endif
            return returnDockContent;
        }
        #endregion

        private void ShowMyGenerationHelp()
        {
            Process myProcess = new Process();

            myProcess.StartInfo.FileName = this.startupPath + @"\MyGeneration.chm";
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            myProcess.Start();
        }

        private void ProcessReplacementFiles()
        {
            DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(Application.ExecutablePath));
            foreach (FileInfo info in dir.GetFiles("*" + REPLACEMENT_SUFFIX))
            {
                FileInfo fileToReplace = new FileInfo(info.FullName.Replace(REPLACEMENT_SUFFIX, ".dll"));
                try
                {
                    if (fileToReplace.Exists)
                    {
                        fileToReplace.MoveTo(fileToReplace.FullName + "." + DateTime.Now.ToString("yyyyMMddhhmmss") + ".bak");
                    }

                    info.MoveTo(fileToReplace.FullName);
                }
#if DEBUG
				catch (Exception ex) { throw ex; }
#else
                catch { }
#endif
            }
        }

        public void RefreshRecentFiles()
        {
            // Clear the Recent Items List
            menuItemRecentFiles.MenuItems.Clear();

            DefaultSettings ds = DefaultSettings.Instance;
            if (ds.RecentFiles.Count == 0)
            {
                menuItemRecentFiles.Visible = false;
            }
            else
            {
                menuItemRecentFiles.Visible = true;

                foreach (string path in ds.RecentFiles)
                {
                    MenuItem item = new MenuItem(path); //need to attach an event to this!
                    item.Click += new EventHandler(menuItemOpenRecentFile_Click);

                    menuItemRecentFiles.MenuItems.Add(item);
                }
            }
        }

        public void AddRecentFile(string path)
        {
            DefaultSettings ds = DefaultSettings.Instance;

            if (ds.RecentFiles.Contains(path))
            {
                ds.RecentFiles.Remove(path);
            }

            ds.RecentFiles.Insert(0, path);
            ds.Save();

            RefreshRecentFiles();
        }

        private void LaunchFileEditor(string path)
        {
            FileInfo info = new FileInfo(path);
            if (info.Exists)
            {
                bool found = false;
                foreach (string ext in this.validExtentions)
                {
                    if (path.ToLower().EndsWith(ext))
                    {
                        this.OpenTemplateEditor(path);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    foreach (string ext in this.validProjectExtentions)
                    {
                        if (path.ToLower().EndsWith(ext))
                        {
                            this.OpenProjectEditor(path);
                            found = true;
                            break;
                        }
                    }
                }
            }
        }

        public static void FlushMemory()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                }
            }
#if DEBUG
			catch (Exception ex) { throw ex; }
#else
            catch { }
#endif
        }

        #region IMyGenerationMDI Members

        public void OpenDocuments(params string[] filenames)
        {
            //
        }

        public void CreateDocument(params string[] args)
        {
            //
        }

        public bool IsDocumentOpen(string text, params IMyGenDocument[] docsToExclude)
        {
            return false;
        }

        public IMyGenDocument FindDocument(string text, params IMyGenDocument[] docsToExclude)
        {
            return null;
        }

        #endregion
    }
}