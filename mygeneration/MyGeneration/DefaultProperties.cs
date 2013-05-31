using System;
using System.Xml;
using System.IO;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Text;

//using ADODB;
//using MSDASC;

using MyMeta;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for DefaultProperties.
	/// </summary>
    public class DefaultProperties : DockContent, IMyGenDocument
	{
		private const string MISSING = "*&?$%";

        #region gui elements
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabConnection;
		private System.Windows.Forms.TabPage tabScript;
        private OpenFileDialog openFileDialog = new OpenFileDialog();
		private System.Windows.Forms.Button btnUserMetaDataFile;
		private System.Windows.Forms.TextBox txtUserMetaDataFile;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cboLanguage;
		private System.Windows.Forms.TextBox txtLanguageFile;
		private System.Windows.Forms.Button btnLanguageFile;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox txtDbTargetFile;
		private System.Windows.Forms.Button btnDbTargetFile;
		private System.Windows.Forms.ComboBox cboDbTarget;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TextBox txtConnectionString;
		private System.Windows.Forms.ComboBox cboDbDriver;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox checkBoxLineNumbers;
		private System.Windows.Forms.CheckBox checkBoxClipboard;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtTabs;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox txtDefaultTemplatePath;
		private System.Windows.Forms.Button buttonBrowseOutPath;
		private System.Windows.Forms.Label labelOutputPath;
		private System.Windows.Forms.TextBox textBoxOutputPath;

		private System.Windows.Forms.TextBox textBoxTimeout;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.CheckBox checkBoxDisableTimeout;
		private System.Windows.Forms.GroupBox groupBoxTimout;
		private System.Windows.Forms.TabPage tabMisc;
		private System.Windows.Forms.CheckBox chkForUpdates;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.CheckBox checkBoxUseProxyServer;
		private System.Windows.Forms.TextBox textBoxProxyServer;
		private System.Windows.Forms.Label labelProxyServer;
		private System.Windows.Forms.TextBox textBoxProxyUser;
		private System.Windows.Forms.TextBox textBoxProxyPassword;
		private System.Windows.Forms.Label labelProxyUser;
		private System.Windows.Forms.Label labelProxyPassword;
		private System.Windows.Forms.TextBox textBoxProxyDomain;
		private System.Windows.Forms.Label labelProxyDomain;
		private System.Windows.Forms.Button btnOleDb;
		private System.Windows.Forms.CheckBox chkDomainOverride;
		private System.Windows.Forms.Button btnTestConnection;
		private DefaultSettings settings = DefaultSettings.Instance;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Button buttonDelete;
		private System.Windows.Forms.Button buttonLoad;
		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.ComboBox comboBoxSavedConns;
		private System.Windows.Forms.Label labelUserMetaData;
		private System.Windows.Forms.GroupBox groupBoxUserMetaData;
		private System.Drawing.Color defaultOleDbButtonColor;

        private GroupBox groupBox6;
        private Button buttonFont;
        private TextBox textBoxFont;
        private Label label10;
        private Label label4;
        private ComboBox comboBoxCodePage;
        private FontDialog fontDialog1;
        #endregion // gui elements
		// private bool haveSettingsChanged = false; // not used

        private string lastLoadedConnection = string.Empty;
        private ToolStrip toolStripOptions;
        private ToolStripButton toolStripButtonSave;
        private ToolStripSeparator toolStripSeparator1;
        private IMyGenerationMDI mdi;
        private MenuStrip menuStripMain;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem7;
        private dbRoot myMeta = new dbRoot();

        public DefaultProperties(IMyGenerationMDI mdi)
		{
			InitializeComponent();

            this.mdi = mdi;
			this.defaultOleDbButtonColor = this.btnOleDb.BackColor;

			DataTable dt = new DataTable();
            dt.Columns.Add("DISPLAY");
            dt.Columns.Add("VALUE");
            dt.Columns.Add("ISPLUGIN");

            // dt.Rows.Add(new object[] { "<None>", "NONE", false });
            dt.Rows.Add(new object[] { "Advantage Database Server", "ADVANTAGE", false });
            dt.Rows.Add(new object[] { "Firebird", "FIREBIRD", false });
            dt.Rows.Add(new object[] { "IBM DB2", "DB2", false });
            dt.Rows.Add(new object[] { "IBM iSeries (AS400)", "ISERIES", false });
            dt.Rows.Add(new object[] { "Interbase", "INTERBASE", false });
            dt.Rows.Add(new object[] { "Microsoft SQL Server", "SQL", false });
            dt.Rows.Add(new object[] { "Microsoft Access", "ACCESS", false });
            dt.Rows.Add(new object[] { "MySQL", "MYSQL", false });
            dt.Rows.Add(new object[] { "MySQL2", "MYSQL2", false });
            dt.Rows.Add(new object[] { "Oracle", "ORACLE", false });
            dt.Rows.Add(new object[] { "Pervasive", "PERVASIVE", false });
            dt.Rows.Add(new object[] { "PostgreSQL", "POSTGRESQL", false });
            dt.Rows.Add(new object[] { "PostgreSQL 8+", "POSTGRESQL8", false });
            dt.Rows.Add(new object[] { "SQLite", "SQLITE", false });
#if !IGNORE_VISTA
            dt.Rows.Add(new object[] { "VistaDB", "VISTADB", false });
#endif

            foreach (IMyMetaPlugin plugin in MyMeta.dbRoot.Plugins.Values)
            {
                dt.Rows.Add(new object[] { plugin.ProviderName, plugin.ProviderUniqueKey, true });
            }

			dt.DefaultView.Sort = "DISPLAY";

			this.cboDbDriver.DisplayMember = "DISPLAY";
			this.cboDbDriver.ValueMember   = "VALUE";
			this.cboDbDriver.DataSource = dt;
			
			switch(settings.DbDriver)
			{
				case "PERVASIVE":
				case "POSTGRESQL":
				case "POSTGRESQL8":
				case "FIREBIRD":
				case "INTERBASE":
				case "SQLITE":
				case "MYSQL2":
#if !IGNORE_VISTA
				case "VISTADB":
#endif
				case "ISERIES":
				case "NONE":
				case "":
					this.btnOleDb.Enabled = false;
					break;
			}

			this.txtConnectionString.Enabled = true;

			this.cboDbDriver.SelectedValue  = settings.DbDriver;
			this.txtConnectionString.Text	= settings.ConnectionString;
			this.txtLanguageFile.Text		= settings.LanguageMappingFile;
			this.txtDbTargetFile.Text		= settings.DbTargetMappingFile;
			this.txtUserMetaDataFile.Text	= settings.UserMetaDataFileName;

			myMeta.LanguageMappingFileName	= settings.LanguageMappingFile;
			myMeta.DbTargetMappingFileName	= settings.DbTargetMappingFile;

			this.cboLanguage.Enabled = true;
			this.cboDbTarget.Enabled = true;

			PopulateLanguages();
			PopulateDbTargets();

			this.cboLanguage.SelectedItem = settings.Language;
			this.cboDbTarget.SelectedItem = settings.DbTarget;

			this.checkBoxClipboard.Checked   = settings.EnableClipboard;
			this.checkBoxLineNumbers.Checked = settings.EnableLineNumbering;
			this.txtTabs.Text                = settings.Tabs.ToString();
			this.txtDefaultTemplatePath.Text = settings.DefaultTemplateDirectory;
			this.textBoxOutputPath.Text		 = settings.DefaultOutputDirectory;

			this.chkForUpdates.Checked		 = settings.CheckForNewBuild;
			this.chkDomainOverride.Checked	 = settings.DomainOverride;

			int timeout = settings.ScriptTimeout;
			if (timeout == -1) 
			{
				this.checkBoxDisableTimeout.Checked = true;
				this.checkBoxDisableTimeout_CheckedChanged(this, new EventArgs());
			}
			else 
			{
				this.checkBoxDisableTimeout.Checked = false;
				this.checkBoxDisableTimeout_CheckedChanged(this, new EventArgs());
				this.textBoxTimeout.Text = timeout.ToString();
			}

			this.checkBoxUseProxyServer.Checked = settings.UseProxyServer;
			this.textBoxProxyServer.Text = settings.ProxyServerUri;
			this.textBoxProxyUser.Text = settings.ProxyAuthUsername;
			this.textBoxProxyPassword.Text = settings.ProxyAuthPassword;
			this.textBoxProxyDomain.Text = settings.ProxyAuthDomain;

			this.rebindSavedConns();

            EncodingInfo[] encodings = Encoding.GetEncodings();
            this.comboBoxCodePage.Items.Add(string.Empty);
            foreach (EncodingInfo encoding in encodings)
            {
                string windowsCodePage = encoding.CodePage.ToString() + ": " + encoding.DisplayName;
                int idx = this.comboBoxCodePage.Items.Add(windowsCodePage);
                
                if (encoding.CodePage == settings.CodePage) 
                {
                    comboBoxCodePage.SelectedIndex = idx;
                }
            }

            this.textBoxFont.Text = this.settings.FontFamily;
		}

		private void rebindSavedConns() 
		{
			this.comboBoxSavedConns.Items.Clear();
			foreach (ConnectionInfo info in settings.SavedConnections.Values) 
			{
				this.comboBoxSavedConns.Items.Add(info);
			}
			this.comboBoxSavedConns.Sorted = true;
			this.comboBoxSavedConns.SelectedIndex = -1;
		}

		private void DefaultProperties_Load(object sender, System.EventArgs e)
		{
		
		}

		private void PopulateLanguages()
		{
			this.cboLanguage.Items.Clear();
			this.cboLanguage.SelectedText = "";

			string[] languages = myMeta.GetLanguageMappings(this.cboDbDriver.SelectedValue as string);

			if(null != languages)
			{
				foreach(string language in languages)
				{
					this.cboLanguage.Items.Add(language);
				}
			}
		}

		private void PopulateDbTargets()
		{
			this.cboDbTarget.Items.Clear();
			this.cboDbTarget.SelectedText = "";

			string[] targets = myMeta.GetDbTargetMappings(this.cboDbDriver.SelectedValue as string);

			if(null != targets)
			{
				foreach(string target in targets)
				{
					this.cboDbTarget.Items.Add(target);
				}
			}
		}

		private string PickFile(string filter) 
		{
			openFileDialog.InitialDirectory = Application.StartupPath + @"\Settings";
			openFileDialog.Filter = filter;
			openFileDialog.RestoreDirectory = true;
       
			if(openFileDialog.ShowDialog() == DialogResult.OK)
			{
				return openFileDialog.FileName;
			}
			else return string.Empty;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefaultProperties));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabConnection = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.comboBoxSavedConns = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboDbTarget = new System.Windows.Forms.ComboBox();
            this.txtDbTargetFile = new System.Windows.Forms.TextBox();
            this.btnDbTargetFile = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboLanguage = new System.Windows.Forms.ComboBox();
            this.txtLanguageFile = new System.Windows.Forms.TextBox();
            this.btnLanguageFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cboDbDriver = new System.Windows.Forms.ComboBox();
            this.btnOleDb = new System.Windows.Forms.Button();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.groupBoxUserMetaData = new System.Windows.Forms.GroupBox();
            this.btnUserMetaDataFile = new System.Windows.Forms.Button();
            this.txtUserMetaDataFile = new System.Windows.Forms.TextBox();
            this.labelUserMetaData = new System.Windows.Forms.Label();
            this.tabScript = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.buttonFont = new System.Windows.Forms.Button();
            this.textBoxFont = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxCodePage = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxProxyDomain = new System.Windows.Forms.TextBox();
            this.labelProxyDomain = new System.Windows.Forms.Label();
            this.labelProxyPassword = new System.Windows.Forms.Label();
            this.labelProxyUser = new System.Windows.Forms.Label();
            this.textBoxProxyPassword = new System.Windows.Forms.TextBox();
            this.textBoxProxyUser = new System.Windows.Forms.TextBox();
            this.checkBoxUseProxyServer = new System.Windows.Forms.CheckBox();
            this.textBoxProxyServer = new System.Windows.Forms.TextBox();
            this.labelProxyServer = new System.Windows.Forms.Label();
            this.groupBoxTimout = new System.Windows.Forms.GroupBox();
            this.checkBoxDisableTimeout = new System.Windows.Forms.CheckBox();
            this.textBoxTimeout = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonBrowseOutPath = new System.Windows.Forms.Button();
            this.labelOutputPath = new System.Windows.Forms.Label();
            this.textBoxOutputPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtTabs = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBoxClipboard = new System.Windows.Forms.CheckBox();
            this.checkBoxLineNumbers = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDefaultTemplatePath = new System.Windows.Forms.TextBox();
            this.tabMisc = new System.Windows.Forms.TabPage();
            this.chkDomainOverride = new System.Windows.Forms.CheckBox();
            this.chkForUpdates = new System.Windows.Forms.CheckBox();
            this.toolStripOptions = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.tabControl.SuspendLayout();
            this.tabConnection.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxUserMetaData.SuspendLayout();
            this.tabScript.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBoxTimout.SuspendLayout();
            this.tabMisc.SuspendLayout();
            this.toolStripOptions.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabConnection);
            this.tabControl.Controls.Add(this.tabScript);
            this.tabControl.Controls.Add(this.tabMisc);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.MinimumSize = new System.Drawing.Size(595, 498);
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(706, 501);
            this.tabControl.TabIndex = 0;
            // 
            // tabConnection
            // 
            this.tabConnection.Controls.Add(this.groupBox5);
            this.tabConnection.Controls.Add(this.groupBox2);
            this.tabConnection.Controls.Add(this.groupBox1);
            this.tabConnection.Controls.Add(this.groupBox3);
            this.tabConnection.Controls.Add(this.groupBoxUserMetaData);
            this.tabConnection.Location = new System.Drawing.Point(4, 22);
            this.tabConnection.Name = "tabConnection";
            this.tabConnection.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabConnection.Size = new System.Drawing.Size(698, 475);
            this.tabConnection.TabIndex = 0;
            this.tabConnection.Text = "Connection";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.buttonDelete);
            this.groupBox5.Controls.Add(this.buttonLoad);
            this.groupBox5.Controls.Add(this.buttonSave);
            this.groupBox5.Controls.Add(this.comboBoxSavedConns);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(16, 16);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(663, 64);
            this.groupBox5.TabIndex = 32;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Saved Connections";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.BackColor = System.Drawing.SystemColors.Control;
            this.buttonDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonDelete.Location = new System.Drawing.Point(604, 24);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(48, 23);
            this.buttonDelete.TabIndex = 35;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = false;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoad.BackColor = System.Drawing.SystemColors.Control;
            this.buttonLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLoad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonLoad.Location = new System.Drawing.Point(484, 24);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(56, 23);
            this.buttonLoad.TabIndex = 34;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = false;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.BackColor = System.Drawing.SystemColors.Control;
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonSave.Location = new System.Drawing.Point(548, 24);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(48, 23);
            this.buttonSave.TabIndex = 33;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // comboBoxSavedConns
            // 
            this.comboBoxSavedConns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSavedConns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.comboBoxSavedConns.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxSavedConns.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.comboBoxSavedConns.Location = new System.Drawing.Point(8, 24);
            this.comboBoxSavedConns.Name = "comboBoxSavedConns";
            this.comboBoxSavedConns.Size = new System.Drawing.Size(468, 21);
            this.comboBoxSavedConns.TabIndex = 32;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cboDbTarget);
            this.groupBox2.Controls.Add(this.txtDbTargetFile);
            this.groupBox2.Controls.Add(this.btnDbTargetFile);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(16, 304);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(663, 88);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Database Target Mapping";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 23);
            this.label3.TabIndex = 27;
            this.label3.Text = "File: ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(16, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 23);
            this.label5.TabIndex = 26;
            this.label5.Text = "DbTarget:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboDbTarget
            // 
            this.cboDbTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDbTarget.Enabled = false;
            this.cboDbTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDbTarget.Location = new System.Drawing.Point(88, 56);
            this.cboDbTarget.Name = "cboDbTarget";
            this.cboDbTarget.Size = new System.Drawing.Size(200, 21);
            this.cboDbTarget.TabIndex = 21;
            // 
            // txtDbTargetFile
            // 
            this.txtDbTargetFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDbTargetFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDbTargetFile.Location = new System.Drawing.Point(88, 24);
            this.txtDbTargetFile.Name = "txtDbTargetFile";
            this.txtDbTargetFile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtDbTargetFile.Size = new System.Drawing.Size(532, 20);
            this.txtDbTargetFile.TabIndex = 20;
            // 
            // btnDbTargetFile
            // 
            this.btnDbTargetFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDbTargetFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDbTargetFile.Location = new System.Drawing.Point(628, 24);
            this.btnDbTargetFile.Name = "btnDbTargetFile";
            this.btnDbTargetFile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnDbTargetFile.Size = new System.Drawing.Size(24, 23);
            this.btnDbTargetFile.TabIndex = 19;
            this.btnDbTargetFile.Text = "...";
            this.btnDbTargetFile.Click += new System.EventHandler(this.btnDbTargetFile_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cboLanguage);
            this.groupBox1.Controls.Add(this.txtLanguageFile);
            this.groupBox1.Controls.Add(this.btnLanguageFile);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 208);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(663, 88);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Language Mapping";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 25;
            this.label1.Text = "File: ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboLanguage
            // 
            this.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguage.Enabled = false;
            this.cboLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLanguage.Location = new System.Drawing.Point(88, 56);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(192, 21);
            this.cboLanguage.TabIndex = 24;
            // 
            // txtLanguageFile
            // 
            this.txtLanguageFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLanguageFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLanguageFile.Location = new System.Drawing.Point(88, 24);
            this.txtLanguageFile.Name = "txtLanguageFile";
            this.txtLanguageFile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtLanguageFile.Size = new System.Drawing.Size(532, 20);
            this.txtLanguageFile.TabIndex = 23;
            // 
            // btnLanguageFile
            // 
            this.btnLanguageFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLanguageFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLanguageFile.Location = new System.Drawing.Point(628, 24);
            this.btnLanguageFile.Name = "btnLanguageFile";
            this.btnLanguageFile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnLanguageFile.Size = new System.Drawing.Size(24, 23);
            this.btnLanguageFile.TabIndex = 22;
            this.btnLanguageFile.Text = "...";
            this.btnLanguageFile.Click += new System.EventHandler(this.btnLanguageFile_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 23);
            this.label2.TabIndex = 25;
            this.label2.Text = "Language:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnTestConnection);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.cboDbDriver);
            this.groupBox3.Controls.Add(this.btnOleDb);
            this.groupBox3.Controls.Add(this.txtConnectionString);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(16, 88);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(663, 112);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Connection String";
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestConnection.Location = new System.Drawing.Point(452, 24);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(112, 23);
            this.btnTestConnection.TabIndex = 27;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(8, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 23);
            this.label6.TabIndex = 26;
            this.label6.Text = "Driver:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboDbDriver
            // 
            this.cboDbDriver.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDbDriver.Location = new System.Drawing.Point(88, 24);
            this.cboDbDriver.Name = "cboDbDriver";
            this.cboDbDriver.Size = new System.Drawing.Size(192, 21);
            this.cboDbDriver.TabIndex = 24;
            this.cboDbDriver.SelectionChangeCommitted += new System.EventHandler(this.cboDbDriver_SelectionChangeCommitted);
            this.cboDbDriver.SelectedIndexChanged += new System.EventHandler(this.cboDbDriver_SelectedIndexChanged);
            // 
            // btnOleDb
            // 
            this.btnOleDb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOleDb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOleDb.Location = new System.Drawing.Point(572, 24);
            this.btnOleDb.Name = "btnOleDb";
            this.btnOleDb.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnOleDb.Size = new System.Drawing.Size(80, 23);
            this.btnOleDb.TabIndex = 21;
            this.btnOleDb.Text = "OLEDB ...";
            this.btnOleDb.Click += new System.EventHandler(this.btnOleDb_Click);
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Enabled = false;
            this.txtConnectionString.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConnectionString.Location = new System.Drawing.Point(8, 56);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtConnectionString.Size = new System.Drawing.Size(644, 40);
            this.txtConnectionString.TabIndex = 20;
            // 
            // groupBoxUserMetaData
            // 
            this.groupBoxUserMetaData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxUserMetaData.Controls.Add(this.btnUserMetaDataFile);
            this.groupBoxUserMetaData.Controls.Add(this.txtUserMetaDataFile);
            this.groupBoxUserMetaData.Controls.Add(this.labelUserMetaData);
            this.groupBoxUserMetaData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxUserMetaData.Location = new System.Drawing.Point(16, 400);
            this.groupBoxUserMetaData.Name = "groupBoxUserMetaData";
            this.groupBoxUserMetaData.Size = new System.Drawing.Size(663, 56);
            this.groupBoxUserMetaData.TabIndex = 33;
            this.groupBoxUserMetaData.TabStop = false;
            this.groupBoxUserMetaData.Text = "User Meta-Data";
            // 
            // btnUserMetaDataFile
            // 
            this.btnUserMetaDataFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUserMetaDataFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUserMetaDataFile.Location = new System.Drawing.Point(628, 24);
            this.btnUserMetaDataFile.Name = "btnUserMetaDataFile";
            this.btnUserMetaDataFile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnUserMetaDataFile.Size = new System.Drawing.Size(24, 23);
            this.btnUserMetaDataFile.TabIndex = 15;
            this.btnUserMetaDataFile.Text = "...";
            this.btnUserMetaDataFile.Click += new System.EventHandler(this.btnCustomDataFile_Click);
            // 
            // txtUserMetaDataFile
            // 
            this.txtUserMetaDataFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserMetaDataFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserMetaDataFile.Location = new System.Drawing.Point(88, 24);
            this.txtUserMetaDataFile.Name = "txtUserMetaDataFile";
            this.txtUserMetaDataFile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtUserMetaDataFile.Size = new System.Drawing.Size(532, 20);
            this.txtUserMetaDataFile.TabIndex = 17;
            // 
            // labelUserMetaData
            // 
            this.labelUserMetaData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUserMetaData.Location = new System.Drawing.Point(16, 24);
            this.labelUserMetaData.Name = "labelUserMetaData";
            this.labelUserMetaData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelUserMetaData.Size = new System.Drawing.Size(64, 23);
            this.labelUserMetaData.TabIndex = 13;
            this.labelUserMetaData.Text = "File:";
            this.labelUserMetaData.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabScript
            // 
            this.tabScript.Controls.Add(this.groupBox6);
            this.tabScript.Controls.Add(this.groupBox4);
            this.tabScript.Controls.Add(this.groupBoxTimout);
            this.tabScript.Controls.Add(this.buttonBrowseOutPath);
            this.tabScript.Controls.Add(this.labelOutputPath);
            this.tabScript.Controls.Add(this.textBoxOutputPath);
            this.tabScript.Controls.Add(this.btnBrowse);
            this.tabScript.Controls.Add(this.txtTabs);
            this.tabScript.Controls.Add(this.label7);
            this.tabScript.Controls.Add(this.checkBoxClipboard);
            this.tabScript.Controls.Add(this.checkBoxLineNumbers);
            this.tabScript.Controls.Add(this.label8);
            this.tabScript.Controls.Add(this.txtDefaultTemplatePath);
            this.tabScript.Location = new System.Drawing.Point(4, 22);
            this.tabScript.Name = "tabScript";
            this.tabScript.Size = new System.Drawing.Size(698, 475);
            this.tabScript.TabIndex = 1;
            this.tabScript.Text = "Templates";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.buttonFont);
            this.groupBox6.Controls.Add(this.textBoxFont);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.comboBoxCodePage);
            this.groupBox6.Location = new System.Drawing.Point(12, 343);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(216, 100);
            this.groupBox6.TabIndex = 14;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Globalization";
            // 
            // buttonFont
            // 
            this.buttonFont.Location = new System.Drawing.Point(152, 72);
            this.buttonFont.Name = "buttonFont";
            this.buttonFont.Size = new System.Drawing.Size(56, 23);
            this.buttonFont.TabIndex = 18;
            this.buttonFont.Text = "Browse";
            this.buttonFont.Click += new System.EventHandler(this.buttonFont_Click);
            // 
            // textBoxFont
            // 
            this.textBoxFont.Location = new System.Drawing.Point(8, 72);
            this.textBoxFont.Name = "textBoxFont";
            this.textBoxFont.Size = new System.Drawing.Size(144, 20);
            this.textBoxFont.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(8, 56);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(200, 15);
            this.label10.TabIndex = 16;
            this.label10.Text = "Font Override";
            this.label10.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "CodePage Override";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // comboBoxCodePage
            // 
            this.comboBoxCodePage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCodePage.FormattingEnabled = true;
            this.comboBoxCodePage.Location = new System.Drawing.Point(8, 32);
            this.comboBoxCodePage.Name = "comboBoxCodePage";
            this.comboBoxCodePage.Size = new System.Drawing.Size(200, 21);
            this.comboBoxCodePage.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBoxProxyDomain);
            this.groupBox4.Controls.Add(this.labelProxyDomain);
            this.groupBox4.Controls.Add(this.labelProxyPassword);
            this.groupBox4.Controls.Add(this.labelProxyUser);
            this.groupBox4.Controls.Add(this.textBoxProxyPassword);
            this.groupBox4.Controls.Add(this.textBoxProxyUser);
            this.groupBox4.Controls.Add(this.checkBoxUseProxyServer);
            this.groupBox4.Controls.Add(this.textBoxProxyServer);
            this.groupBox4.Controls.Add(this.labelProxyServer);
            this.groupBox4.Location = new System.Drawing.Point(234, 235);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(344, 208);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Custom Proxy Server Settings";
            // 
            // textBoxProxyDomain
            // 
            this.textBoxProxyDomain.Enabled = false;
            this.textBoxProxyDomain.Location = new System.Drawing.Point(8, 168);
            this.textBoxProxyDomain.Name = "textBoxProxyDomain";
            this.textBoxProxyDomain.ReadOnly = true;
            this.textBoxProxyDomain.Size = new System.Drawing.Size(328, 20);
            this.textBoxProxyDomain.TabIndex = 16;
            // 
            // labelProxyDomain
            // 
            this.labelProxyDomain.Enabled = false;
            this.labelProxyDomain.Location = new System.Drawing.Point(8, 144);
            this.labelProxyDomain.Name = "labelProxyDomain";
            this.labelProxyDomain.Size = new System.Drawing.Size(328, 23);
            this.labelProxyDomain.TabIndex = 17;
            this.labelProxyDomain.Text = "Domain Name";
            this.labelProxyDomain.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelProxyPassword
            // 
            this.labelProxyPassword.Enabled = false;
            this.labelProxyPassword.Location = new System.Drawing.Point(168, 96);
            this.labelProxyPassword.Name = "labelProxyPassword";
            this.labelProxyPassword.Size = new System.Drawing.Size(168, 23);
            this.labelProxyPassword.TabIndex = 15;
            this.labelProxyPassword.Text = "Password";
            this.labelProxyPassword.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.labelProxyPassword.UseMnemonic = false;
            // 
            // labelProxyUser
            // 
            this.labelProxyUser.Enabled = false;
            this.labelProxyUser.Location = new System.Drawing.Point(8, 96);
            this.labelProxyUser.Name = "labelProxyUser";
            this.labelProxyUser.Size = new System.Drawing.Size(152, 23);
            this.labelProxyUser.TabIndex = 14;
            this.labelProxyUser.Text = "Username";
            this.labelProxyUser.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxProxyPassword
            // 
            this.textBoxProxyPassword.Enabled = false;
            this.textBoxProxyPassword.Location = new System.Drawing.Point(168, 120);
            this.textBoxProxyPassword.Name = "textBoxProxyPassword";
            this.textBoxProxyPassword.ReadOnly = true;
            this.textBoxProxyPassword.Size = new System.Drawing.Size(168, 20);
            this.textBoxProxyPassword.TabIndex = 13;
            // 
            // textBoxProxyUser
            // 
            this.textBoxProxyUser.Enabled = false;
            this.textBoxProxyUser.Location = new System.Drawing.Point(8, 120);
            this.textBoxProxyUser.Name = "textBoxProxyUser";
            this.textBoxProxyUser.ReadOnly = true;
            this.textBoxProxyUser.Size = new System.Drawing.Size(152, 20);
            this.textBoxProxyUser.TabIndex = 12;
            // 
            // checkBoxUseProxyServer
            // 
            this.checkBoxUseProxyServer.Location = new System.Drawing.Point(8, 24);
            this.checkBoxUseProxyServer.Name = "checkBoxUseProxyServer";
            this.checkBoxUseProxyServer.Size = new System.Drawing.Size(184, 24);
            this.checkBoxUseProxyServer.TabIndex = 11;
            this.checkBoxUseProxyServer.Text = "Use Custom Proxy Server";
            this.checkBoxUseProxyServer.CheckedChanged += new System.EventHandler(this.checkBoxUseProxyServer_CheckedChanged);
            // 
            // textBoxProxyServer
            // 
            this.textBoxProxyServer.Enabled = false;
            this.textBoxProxyServer.Location = new System.Drawing.Point(8, 72);
            this.textBoxProxyServer.Name = "textBoxProxyServer";
            this.textBoxProxyServer.ReadOnly = true;
            this.textBoxProxyServer.Size = new System.Drawing.Size(328, 20);
            this.textBoxProxyServer.TabIndex = 9;
            // 
            // labelProxyServer
            // 
            this.labelProxyServer.Enabled = false;
            this.labelProxyServer.Location = new System.Drawing.Point(8, 48);
            this.labelProxyServer.Name = "labelProxyServer";
            this.labelProxyServer.Size = new System.Drawing.Size(328, 23);
            this.labelProxyServer.TabIndex = 10;
            this.labelProxyServer.Text = "Proxy Server URL";
            this.labelProxyServer.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // groupBoxTimout
            // 
            this.groupBoxTimout.Controls.Add(this.checkBoxDisableTimeout);
            this.groupBoxTimout.Controls.Add(this.textBoxTimeout);
            this.groupBoxTimout.Controls.Add(this.label9);
            this.groupBoxTimout.Location = new System.Drawing.Point(12, 231);
            this.groupBoxTimout.Name = "groupBoxTimout";
            this.groupBoxTimout.Size = new System.Drawing.Size(216, 104);
            this.groupBoxTimout.TabIndex = 12;
            this.groupBoxTimout.TabStop = false;
            this.groupBoxTimout.Text = "Script Timeout";
            // 
            // checkBoxDisableTimeout
            // 
            this.checkBoxDisableTimeout.Checked = true;
            this.checkBoxDisableTimeout.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDisableTimeout.Location = new System.Drawing.Point(8, 24);
            this.checkBoxDisableTimeout.Name = "checkBoxDisableTimeout";
            this.checkBoxDisableTimeout.Size = new System.Drawing.Size(184, 24);
            this.checkBoxDisableTimeout.TabIndex = 11;
            this.checkBoxDisableTimeout.Text = "Disable Timeout on Scripts";
            this.checkBoxDisableTimeout.CheckedChanged += new System.EventHandler(this.checkBoxDisableTimeout_CheckedChanged);
            // 
            // textBoxTimeout
            // 
            this.textBoxTimeout.Enabled = false;
            this.textBoxTimeout.Location = new System.Drawing.Point(8, 72);
            this.textBoxTimeout.Name = "textBoxTimeout";
            this.textBoxTimeout.ReadOnly = true;
            this.textBoxTimeout.Size = new System.Drawing.Size(56, 20);
            this.textBoxTimeout.TabIndex = 9;
            this.textBoxTimeout.Text = "120";
            // 
            // label9
            // 
            this.label9.Enabled = false;
            this.label9.Location = new System.Drawing.Point(8, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(176, 23);
            this.label9.TabIndex = 10;
            this.label9.Text = "Timeout (in seconds)";
            this.label9.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonBrowseOutPath
            // 
            this.buttonBrowseOutPath.Location = new System.Drawing.Point(460, 119);
            this.buttonBrowseOutPath.Name = "buttonBrowseOutPath";
            this.buttonBrowseOutPath.Size = new System.Drawing.Size(56, 23);
            this.buttonBrowseOutPath.TabIndex = 8;
            this.buttonBrowseOutPath.Text = "Browse";
            this.buttonBrowseOutPath.Click += new System.EventHandler(this.buttonBrowseOutPath_Click);
            // 
            // labelOutputPath
            // 
            this.labelOutputPath.Location = new System.Drawing.Point(20, 103);
            this.labelOutputPath.Name = "labelOutputPath";
            this.labelOutputPath.Size = new System.Drawing.Size(312, 16);
            this.labelOutputPath.TabIndex = 5;
            this.labelOutputPath.Text = "Default Output Path:";
            this.labelOutputPath.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxOutputPath
            // 
            this.textBoxOutputPath.Location = new System.Drawing.Point(20, 119);
            this.textBoxOutputPath.Name = "textBoxOutputPath";
            this.textBoxOutputPath.Size = new System.Drawing.Size(440, 20);
            this.textBoxOutputPath.TabIndex = 6;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(460, 71);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(56, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtTabs
            // 
            this.txtTabs.Location = new System.Drawing.Point(20, 31);
            this.txtTabs.Name = "txtTabs";
            this.txtTabs.Size = new System.Drawing.Size(56, 20);
            this.txtTabs.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(20, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 16);
            this.label7.TabIndex = 2;
            this.label7.Text = "Tabs:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // checkBoxClipboard
            // 
            this.checkBoxClipboard.Location = new System.Drawing.Point(20, 183);
            this.checkBoxClipboard.Name = "checkBoxClipboard";
            this.checkBoxClipboard.Size = new System.Drawing.Size(248, 24);
            this.checkBoxClipboard.TabIndex = 1;
            this.checkBoxClipboard.Text = "Copy Generated Output to the Clipboard";
            // 
            // checkBoxLineNumbers
            // 
            this.checkBoxLineNumbers.Location = new System.Drawing.Point(20, 151);
            this.checkBoxLineNumbers.Name = "checkBoxLineNumbers";
            this.checkBoxLineNumbers.Size = new System.Drawing.Size(152, 24);
            this.checkBoxLineNumbers.TabIndex = 0;
            this.checkBoxLineNumbers.Text = "Display Line Numbers";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(20, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(400, 16);
            this.label8.TabIndex = 2;
            this.label8.Text = "Default Template Path:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtDefaultTemplatePath
            // 
            this.txtDefaultTemplatePath.Location = new System.Drawing.Point(20, 71);
            this.txtDefaultTemplatePath.Name = "txtDefaultTemplatePath";
            this.txtDefaultTemplatePath.Size = new System.Drawing.Size(440, 20);
            this.txtDefaultTemplatePath.TabIndex = 3;
            // 
            // tabMisc
            // 
            this.tabMisc.Controls.Add(this.chkDomainOverride);
            this.tabMisc.Controls.Add(this.chkForUpdates);
            this.tabMisc.Location = new System.Drawing.Point(4, 22);
            this.tabMisc.Name = "tabMisc";
            this.tabMisc.Size = new System.Drawing.Size(698, 475);
            this.tabMisc.TabIndex = 2;
            this.tabMisc.Text = "Misc";
            // 
            // chkDomainOverride
            // 
            this.chkDomainOverride.Location = new System.Drawing.Point(24, 56);
            this.chkDomainOverride.Name = "chkDomainOverride";
            this.chkDomainOverride.Size = new System.Drawing.Size(216, 24);
            this.chkDomainOverride.TabIndex = 1;
            this.chkDomainOverride.Text = "MyMeta \"DomainOverride\"";
            // 
            // chkForUpdates
            // 
            this.chkForUpdates.Location = new System.Drawing.Point(24, 24);
            this.chkForUpdates.Name = "chkForUpdates";
            this.chkForUpdates.Size = new System.Drawing.Size(216, 24);
            this.chkForUpdates.TabIndex = 0;
            this.chkForUpdates.Text = "Check For New Build When Launched";
            // 
            // toolStripOptions
            // 
            this.toolStripOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave,
            this.toolStripSeparator1});
            this.toolStripOptions.Location = new System.Drawing.Point(0, 0);
            this.toolStripOptions.Name = "toolStripOptions";
            this.toolStripOptions.Size = new System.Drawing.Size(706, 25);
            this.toolStripOptions.TabIndex = 34;
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
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripSeparator1.MergeIndex = 1;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStripMain.Size = new System.Drawing.Size(706, 24);
            this.menuStripMain.TabIndex = 39;
            this.menuStripMain.Text = "menuStrip1";
            this.menuStripMain.Visible = false;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
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
            // DefaultProperties
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(706, 500);
            this.Controls.Add(this.menuStripMain);
            this.Controls.Add(this.toolStripOptions);
            this.Controls.Add(this.tabControl);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DefaultProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TabText = "Default Settings";
            this.Text = "Default Settings";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.DefaultProperties_Closing);
            this.Load += new System.EventHandler(this.DefaultProperties_Load);
            this.tabControl.ResumeLayout(false);
            this.tabConnection.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBoxUserMetaData.ResumeLayout(false);
            this.groupBoxUserMetaData.PerformLayout();
            this.tabScript.ResumeLayout(false);
            this.tabScript.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBoxTimout.ResumeLayout(false);
            this.groupBoxTimout.PerformLayout();
            this.tabMisc.ResumeLayout(false);
            this.toolStripOptions.ResumeLayout(false);
            this.toolStripOptions.PerformLayout();
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void btnOleDb_Click(object sender, System.EventArgs e)
		{
			try
			{
            string driver = string.Empty;
            string connstr = string.Empty;

				if(null != cboDbDriver.SelectedValue) driver = cboDbDriver.SelectedValue as string;
                    connstr = this.txtConnectionString.Text;

				if(driver == string.Empty)
                {
                    MessageBox.Show("You Must Choose Driver", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                driver = driver.ToUpper();
                InternalDriver drv = InternalDriver.Get(driver);
                if (drv != null)
                {
                    if (string.Empty == connstr)
                    {
                        connstr = (drv != null) ? drv.ConnectString : string.Empty;
                    }

                    connstr = drv.BrowseConnectionString(connstr);
                    if (connstr != null)
                    {
                        try
                        {
                            if (TestConnection(true, driver, connstr))
                            //dbRoot myMeta = new dbRoot();
                            //if (myMeta.Connect(driver, connstr))
                            {
                                this.txtConnectionString.Text = connstr;
                                return;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
			catch {}
        }

		private void btnLanguageFile_Click(object sender, System.EventArgs e)
		{
			string fileName = this.PickFile("Langauge File (*.xml)|*.xml");

			if(string.Empty != fileName)
			{
				this.txtLanguageFile.Text = fileName;
				myMeta.LanguageMappingFileName = fileName;
				PopulateLanguages();
			}
		}

		private void btnDbTargetFile_Click(object sender, System.EventArgs e)
		{
			string fileName = this.PickFile("Database Target File (*.xml)|*.xml");

			if(string.Empty != fileName)
			{
				this.txtDbTargetFile.Text = fileName;
				myMeta.DbTargetMappingFileName = fileName;
				PopulateDbTargets();
			}		
		}

		private void btnCustomDataFile_Click(object sender, System.EventArgs e)
		{
			string fileName = this.PickFile("Langauge File (*.xml)|*.xml");

			if(string.Empty != fileName)
			{
				this.txtUserMetaDataFile.Text = fileName;
			}		
		}

		/*private void btnOK_Click(object sender, System.EventArgs e)
		{
			if(this.TestConnection(true))
			{
				BindControlsToSettings();

                settings.Save();
                mdi.SendAlert(this, "UpdateDefaultSettings");

				this.DialogResult = DialogResult.OK;
			}
		}*/

        private bool ConnectionInfoModified()
        {
            ConnectionInfo info = settings.SavedConnections[lastLoadedConnection] as ConnectionInfo;

            if ((lastLoadedConnection == string.Empty)
                || (settings.SavedConnections.ContainsKey(lastLoadedConnection)))
            {
                return false;
            }

            return (info.Driver != this.cboDbDriver.SelectedValue.ToString()) ||
                                        (info.ConnectionString != this.txtConnectionString.Text) ||
                                        (info.LanguagePath != this.txtLanguageFile.Text) ||
                                        (info.Language != this.cboLanguage.Text) ||
                                        (info.DbTargetPath != this.txtDbTargetFile.Text) ||
                                        (info.DbTarget != this.cboDbTarget.Text) ||
                                        (info.UserMetaDataPath != this.txtUserMetaDataFile.Text);
        }

        private bool SettingsModified()
        {
            if (this.cboDbDriver.SelectedValue == null)
                return false;
            return (settings.DbDriver != this.cboDbDriver.SelectedValue.ToString()) ||
                                (settings.ConnectionString != this.txtConnectionString.Text) ||
                                (settings.LanguageMappingFile != this.txtLanguageFile.Text) ||
                                (settings.Language != this.cboLanguage.Text) ||
                                (settings.DbTargetMappingFile != this.txtDbTargetFile.Text) ||
                                (settings.DbTarget != this.cboDbTarget.Text) ||
                                (settings.UserMetaDataFileName != this.txtUserMetaDataFile.Text);
        }

		private void BindControlsToSettings()
		{
			settings.DbDriver				= this.cboDbDriver.SelectedValue as string;
			settings.ConnectionString		= this.txtConnectionString.Text;
			settings.LanguageMappingFile	= this.txtLanguageFile.Text;
			settings.Language				= this.cboLanguage.SelectedItem as string;
			settings.DbTargetMappingFile	= this.txtDbTargetFile.Text;
			settings.DbTarget				= this.cboDbTarget.SelectedItem as string;
			settings.UserMetaDataFileName	= this.txtUserMetaDataFile.Text;
			settings.EnableClipboard		= this.checkBoxClipboard.Checked;
			settings.EnableLineNumbering	= this.checkBoxLineNumbers.Checked;
			settings.Tabs					= Convert.ToInt32(this.txtTabs.Text);
            settings.CheckForNewBuild = this.chkForUpdates.Checked;
            settings.DomainOverride = this.chkDomainOverride.Checked;

            if (this.comboBoxCodePage.SelectedIndex > 0)
            {
                string selText = this.comboBoxCodePage.SelectedItem.ToString();
                settings.CodePage = Int32.Parse(selText.Substring(0, selText.IndexOf(':')));
            }
            else
            {
                settings.CodePage = -1;
            }
            if (this.textBoxFont.Text.Trim() != string.Empty)
            {
                try
                {
                    Font f = new Font(this.textBoxFont.Text, 12);
                    settings.FontFamily = f.FontFamily.Name;
                }
                catch { }
            }
            else
            {
                settings.FontFamily = string.Empty;
            }

			settings.DefaultTemplateDirectory = this.txtDefaultTemplatePath.Text;
			settings.DefaultOutputDirectory   = this.textBoxOutputPath.Text;
				
			if (this.checkBoxDisableTimeout.Checked) 
			{
				settings.ScriptTimeout		= -1;
			}
			else 
			{
				settings.ScriptTimeout		= Convert.ToInt32(this.textBoxTimeout.Text);
			}

			settings.UseProxyServer = this.checkBoxUseProxyServer.Checked;
			settings.ProxyServerUri = this.textBoxProxyServer.Text;
			settings.ProxyAuthUsername = this.textBoxProxyUser.Text;
			settings.ProxyAuthPassword = this.textBoxProxyPassword.Text;
			settings.ProxyAuthDomain = this.textBoxProxyDomain.Text;

			settings.FirstLoad = false;

            InternalDriver drv = InternalDriver.Get(settings.DbDriver);
            if (drv != null)
            {
                drv.ConnectString = this.txtConnectionString.Text;
                settings.SetSetting(drv.DriverId, this.txtConnectionString.Text);
            } else {
				MessageBox.Show(this, "Choosing '<None>' will eliminate your ability to run 99.9% of the MyGeneration Templates. Most templates will crash if you run them", 
					"Warning !!!",MessageBoxButtons.OK, MessageBoxIcon.Stop); 
			}
		}

		//		private string sqlString = @"Provider=SQLOLEDB.1;Persist Security Info=True;User ID=sa;Data Source=localhost";
		//		private string mySqlString = @"Provider=MySQLProv;Persist Security Info=True;Data Source=test;UID=griffo;PWD=;PORT=3306";
		//		private string accessString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\access\newnorthwind.mdb;User Id=;Password=";
		//		private string oracleString = @"Provider=OraOLEDB.Oracle.1;Password=sa;Persist Security Info=True;User ID=GRIFFO;Data Source=dbMeta";
		//		private string db2String = @"Provider=IBMDADB2.1;Password=sa;User ID=DB2Admin;Data Source=MyMeta;Persist Security Info=True";
		//      private string firbirdstring = @"Provider=LCPI.IBProvider.2;Password=MyGen;Persist Security Info=True;User ID=sysdba;Data Source=MyGeneration;Location=c:\firebird\MyGeneration.gdb;ctype=;";

		private void cboDbDriver_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			this.txtConnectionString.Text = string.Empty;
			this.txtConnectionString.Enabled = true;

			this.cboLanguage.Enabled = true;
			this.cboDbTarget.Enabled = true;

//			string language = this.cboLanguage.SelectedItem as string;
//			string dbTarget = this.cboDbDriver.SelectedItem as string;

			PopulateLanguages();
			PopulateDbTargets();

//			if(-1 != this.cboLanguage.FindStringExact(language))
//			{
//				this.cboLanguage.SelectedItem = language;
//			}
//
//			if(-1 != this.cboDbDriver.FindStringExact(dbTarget))
//			{
//				this.cboDbDriver.SelectedItem = dbTarget;
//			}

			string driver  = string.Empty;
			if(null != cboDbDriver.SelectedValue) driver = cboDbDriver.SelectedValue as string;
			driver = driver.ToUpper();

            this.btnOleDb.BackColor = defaultOleDbButtonColor;
            this.btnOleDb.Enabled = true;
            this.btnTestConnection.Enabled = true;

            InternalDriver drv = InternalDriver.Get(driver);
            if (drv != null)
            {
                bool oleDB  = drv.IsOleDB;
                this.btnOleDb.Enabled = oleDB;
                if (oleDB)
                {
                    this.btnOleDb.BackColor = System.Drawing.Color.LightBlue;
                }

                this.txtConnectionString.Text = settings.GetSetting(drv.DriverId, drv.ConnectString);
            }
            else
            {
                this.btnTestConnection.Enabled = false;
                //MessageBox.Show(this, "Choosing '<None>' will eliminate your ability to run 99.9% of the MyGeneration Templates. Most templates will crash if you run them",
                //    "Warning !!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

		}

		private bool TestConnection(bool silent)
		{
			string driver  = string.Empty;
			string connstr = string.Empty;

			try
			{
				if(null != cboDbDriver.SelectedValue) driver = cboDbDriver.SelectedValue as string;
				connstr = this.txtConnectionString.Text;

				if(driver == string.Empty)
				{
					MessageBox.Show("You Must Choose Driver", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				if(connstr == string.Empty && driver != "NONE")
				{
					MessageBox.Show("Please Supply a Connection String", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				if (driver.ToUpper() != "NONE") 
				{
                    return TestConnection(silent, driver, connstr);
				}

				return true;
				/*dbRoot myMeta = new dbRoot();
				if(!myMeta.Connect(driver, connstr))
				{
					MessageBox.Show("Invalid ConnectionString", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				if(!silent)
				{
					MessageBox.Show("Test Connection Successful ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

				return true;*/
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Unable to Connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}	
		}

        private bool TestConnection(bool silent, string driver, string connstr)
        {
            TestConnectionForm tcf = new TestConnectionForm(driver, connstr, silent);//silent);
            tcf.ShowDialog(this);
            if (TestConnectionForm.State == ConnectionTestState.Error)
            {
                //MessageBox.Show("Invalid ConnectionString", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!silent)
            {

                //MessageBox.Show("Test Connection Successful ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            tcf = null;
            return true;
        }

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog folderDialog  = new FolderBrowserDialog();
			folderDialog.SelectedPath = settings.DefaultTemplateDirectory;
			folderDialog.Description = "Select Default Template Directory";
			folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
			folderDialog.ShowNewFolderButton = true;

			if(folderDialog.ShowDialog() == DialogResult.OK)
			{
				settings.DefaultTemplateDirectory = folderDialog.SelectedPath;
				this.txtDefaultTemplatePath.Text = settings.DefaultTemplateDirectory;
			}
		}

		private void buttonBrowseOutPath_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog folderDialog  = new FolderBrowserDialog();
			folderDialog.SelectedPath = settings.DefaultOutputDirectory;
			folderDialog.Description = "Select Default Output Directory";
			folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
			folderDialog.ShowNewFolderButton = true;

			if(folderDialog.ShowDialog() == DialogResult.OK)
			{
				settings.DefaultOutputDirectory = folderDialog.SelectedPath;
				this.textBoxOutputPath.Text = settings.DefaultOutputDirectory;
			}
		}

		private void checkBoxDisableTimeout_CheckedChanged(object sender, System.EventArgs e)
		{
			bool isChecked = checkBoxDisableTimeout.Checked;

			if (isChecked) 
			{
				textBoxTimeout.Text = "";
			}

			textBoxTimeout.Enabled = !isChecked;
			textBoxTimeout.ReadOnly = isChecked;
			label9.Enabled = !isChecked;
		}

		private void checkBoxUseProxyServer_CheckedChanged(object sender, System.EventArgs e)
		{
			bool isChecked = checkBoxUseProxyServer.Checked;

			textBoxProxyServer.Enabled = isChecked;
			textBoxProxyServer.ReadOnly = !isChecked;
			labelProxyServer.Enabled = isChecked;
	
			textBoxProxyUser.Enabled = isChecked;
			textBoxProxyUser.ReadOnly = !isChecked;
			labelProxyUser.Enabled = isChecked;

			textBoxProxyPassword.Enabled = isChecked;
			textBoxProxyPassword.ReadOnly = !isChecked;
			labelProxyPassword.Enabled = isChecked;

			textBoxProxyDomain.Enabled = isChecked;
			textBoxProxyDomain.ReadOnly = !isChecked;
			labelProxyDomain.Enabled = isChecked;
		}

		private void btnTestConnection_Click(object sender, System.EventArgs e)
		{
			this.TestConnection(false);		
		}

		private void cboDbDriver_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			cboDbDriver_SelectionChangeCommitted(sender, e);		
		}

		private void buttonSave_Click(object sender, System.EventArgs e)
		{
			string text = this.comboBoxSavedConns.Text.Trim();
			if(text == string.Empty)
			{
				MessageBox.Show("Please Enter a Name for this Saved Connection. Type the Name Directly into the ComboBox.", "Saved Connection Name Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				this.comboBoxSavedConns.Focus();
			}
			else 
			{
				this.lastLoadedConnection = text;

				ConnectionInfo info = null;
				info = settings.SavedConnections[text] as ConnectionInfo;
				
				if (info == null) 
				{
					info = new ConnectionInfo();
					info.Name = this.comboBoxSavedConns.Text;
					settings.SavedConnections[info.Name] = info;
				}

				info.Driver = this.cboDbDriver.SelectedValue.ToString();
				info.ConnectionString = this.txtConnectionString.Text;
				info.UserMetaDataPath = this.txtUserMetaDataFile.Text;
				info.Language = this.cboLanguage.Text;
				info.DbTarget = this.cboDbTarget.Text;
				info.LanguagePath = this.txtLanguageFile.Text;
				info.DbTargetPath = this.txtDbTargetFile.Text;

				this.rebindSavedConns();
			}
		}

		private void buttonLoad_Click(object sender, System.EventArgs e)
		{
			ConnectionInfo info = this.comboBoxSavedConns.SelectedItem as ConnectionInfo;
				
			if (info != null) 
			{
				lastLoadedConnection = info.Name;
				settings.DbDriver = info.Driver;
				settings.ConnectionString = info.ConnectionString;
				settings.UserMetaDataFileName = info.UserMetaDataPath;
				settings.Language = info.Language;
				settings.LanguageMappingFile = info.LanguagePath;
				settings.DbTarget = info.DbTarget;
				settings.DbTargetMappingFile = info.DbTargetPath;

				this.cboDbDriver.SelectedValue  = settings.DbDriver;
				this.txtConnectionString.Text	= settings.ConnectionString;
				this.txtLanguageFile.Text		= settings.LanguageMappingFile;
				this.txtDbTargetFile.Text		= settings.DbTargetMappingFile;
				this.txtUserMetaDataFile.Text	= settings.UserMetaDataFileName;

				myMeta.LanguageMappingFileName	= settings.LanguageMappingFile;
				myMeta.DbTargetMappingFileName	= settings.DbTargetMappingFile;

				this.cboLanguage.Enabled = true;
				this.cboDbTarget.Enabled = true;

				PopulateLanguages();
				PopulateDbTargets();

				this.cboLanguage.SelectedItem = settings.Language;
				this.cboDbTarget.SelectedItem = settings.DbTarget;
			}
			else
			{
				MessageBox.Show("You Must Select a Saved Connection to Load", "No Saved Connection Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void buttonDelete_Click(object sender, System.EventArgs e)
		{
			ConnectionInfo info = this.comboBoxSavedConns.SelectedItem as ConnectionInfo;
				
			if (info != null) 
			{
				settings.SavedConnections.Remove(info.Name);

				this.rebindSavedConns();

				if(this.comboBoxSavedConns.Items.Count > 0)
					this.comboBoxSavedConns.SelectedIndex = 0;
				else 
				{
					this.comboBoxSavedConns.SelectedIndex = -1;
					this.comboBoxSavedConns.Text = string.Empty;
				}
			}
			else
			{
				MessageBox.Show("You Must Select a Saved Connection to Delete", "No Saved Connection Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

        private void DefaultProperties_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult r = DialogResult.None;

            // Something's Changed since the load...
            if (SettingsModified())
            {
                r = MessageBox.Show("Default settings have changed.\r\n Would you like to save before exiting?", "Default Settings Changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
            }
            else if (ConnectionInfoModified())
            {
                r = MessageBox.Show("The loaded connection profile has changed.\r\n Would you like to save before exiting?", "Connection Profile Changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
            }

            if (r != DialogResult.None)
            {
                if (r == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (r == DialogResult.Yes)
                {
                    if (this.TestConnection(true))
                    {
                        BindControlsToSettings();

                        settings.Save();
                        mdi.SendAlert(this, "UpdateDefaultSettings");

                        this.DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    settings.DiscardChanges();
                }
            }
        }

        private void buttonFont_Click(object sender, EventArgs e)
        {
            try
            {
                Font f = new Font(this.textBoxFont.Text, 12);
                fontDialog1.Font = f;
            }
            catch 
            {
                this.textBoxFont.Text = string.Empty;
            }

            fontDialog1.ShowColor = false;
            fontDialog1.ShowEffects = false;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBoxFont.Text = fontDialog1.Font.FontFamily.Name;
            }
            else if (fontDialog1.ShowDialog() == DialogResult.None)
            {
                this.textBoxFont.Text = string.Empty;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.TestConnection(true))
            {
                BindControlsToSettings();

                settings.Save();
                mdi.SendAlert(this, "UpdateDefaultSettings");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.TestConnection(true))
            {
                BindControlsToSettings();

                settings.Save();
                mdi.SendAlert(this, "UpdateDefaultSettings");
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region IMyGenDocument Members

        public ToolStrip ToolStrip
        {
            get { return this.toolStripOptions; }
        }

        public string DocumentIndentity
        {
            get { return "::DefaultSettings::"; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public bool CanClose(bool allowPrevent)
        {
            return true;
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        #endregion

    }
}
