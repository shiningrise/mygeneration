using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MyMeta;
using WeifenLuo.WinFormsUI;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration
{
    public partial class DefaultSettingsControl : UserControl
    {
        private const string MISSING = "*&?$%";
        private string lastLoadedConnection = string.Empty;
        private dbRoot myMeta;
        private DefaultSettings settings;
        private System.Drawing.Color defaultOleDbButtonColor;
        private DataTable driversTable;
        public ShowOleDbDialogHandler ShowOLEDBDialog;
        private IMyGenerationMDI mdi;

        public delegate void AfterSaveDelegate();
        public event EventHandler AfterSave;

        public DefaultSettingsControl()
        {
            InitializeComponent();
        }

        public void Initialize(IMyGenerationMDI mdi)
        {
            this.mdi = mdi;
            settings = DefaultSettings.Instance;
        }

        public void Populate()
        {
            myMeta = new dbRoot();
            this.defaultOleDbButtonColor = this.btnOleDb.BackColor;

            this.cboDbDriver.DisplayMember = "DISPLAY";
            this.cboDbDriver.ValueMember = "VALUE";
            this.cboDbDriver.DataSource = DriversTable;

            switch (settings.DbDriver)
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

            this.cboDbDriver.SelectedValue = settings.DbDriver;
            this.txtConnectionString.Text = settings.ConnectionString;
            this.txtLanguageFile.Text = settings.LanguageMappingFile;
            this.txtDbTargetFile.Text = settings.DbTargetMappingFile;
            this.txtUserMetaDataFile.Text = settings.UserMetaDataFileName;

            myMeta.ShowDefaultDatabaseOnly = settings.ShowDefaultDatabaseOnly;
            myMeta.LanguageMappingFileName = settings.LanguageMappingFile;
            myMeta.DbTargetMappingFileName = settings.DbTargetMappingFile;

            this.cboLanguage.Enabled = true;
            this.cboDbTarget.Enabled = true;

            PopulateLanguages();
            PopulateDbTargets();

            this.cboLanguage.SelectedItem = settings.Language;
            this.cboDbTarget.SelectedItem = settings.DbTarget;

            this.textBoxDbUserMetaMappings.Text = settings.DatabaseUserDataXmlMappingsString;

            this.checkBoxClipboard.Checked = settings.EnableClipboard;
            this.checkBoxRunTemplatesAsync.Checked = settings.ExecuteFromTemplateBrowserAsync;
            this.checkBoxDefaultDBOnly.Checked = settings.ShowDefaultDatabaseOnly;
            this.checkBoxShowConsoleOutput.Checked = settings.ConsoleWriteGeneratedDetails;
            this.checkBoxDocumentStyleSettings.Checked = settings.EnableDocumentStyleSettings;
            this.checkBoxLineNumbers.Checked = settings.EnableLineNumbering;
            this.txtTabs.Text = settings.Tabs.ToString();
            this.txtDefaultTemplatePath.Text = settings.DefaultTemplateDirectory;
            this.textBoxOutputPath.Text = settings.DefaultOutputDirectory;

            this.chkForUpdates.Checked = settings.CheckForNewBuild;
            this.chkDomainOverride.Checked = settings.DomainOverride;

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

        public bool Save()
        {
            if (this.TestConnection(true))
            {
                this.BindControlsToSettings();

                settings.Save();
                return true;
            }
            return false;
        }

        protected void OnAfterSave()
        {
            if (AfterSave != null) AfterSave(this, EventArgs.Empty);
        }

        public void Cancel()
        {
            settings.DiscardChanges();
        }

        public bool ConnectionInfoModified
        {
            get
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
                                            (info.UserMetaDataPath != this.txtUserMetaDataFile.Text) ||
                                            (info.DatabaseUserDataXmlMappingsString != this.textBoxDbUserMetaMappings.Text);
            }
        }

        public bool SettingsModified
        {
            get
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

        private void PopulateLanguages()
        {
            this.cboLanguage.Items.Clear();
            this.cboLanguage.SelectedText = "";

            string[] languages = myMeta.GetLanguageMappings(this.cboDbDriver.SelectedValue as string);

            if (null != languages)
            {
                foreach (string language in languages)
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

            if (null != targets)
            {
                foreach (string target in targets)
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

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            else return string.Empty;
        }

        private void BindControlsToSettings()
        {
            settings.DbDriver = this.cboDbDriver.SelectedValue as string;
            settings.ConnectionString = this.txtConnectionString.Text;
            settings.LanguageMappingFile = this.txtLanguageFile.Text;
            settings.Language = this.cboLanguage.SelectedItem as string;
            settings.DbTargetMappingFile = this.txtDbTargetFile.Text;
            settings.DbTarget = this.cboDbTarget.SelectedItem as string;
            settings.UserMetaDataFileName = this.txtUserMetaDataFile.Text;
            settings.EnableClipboard = this.checkBoxClipboard.Checked;
            settings.ExecuteFromTemplateBrowserAsync = this.checkBoxRunTemplatesAsync.Checked;
            settings.ShowDefaultDatabaseOnly = this.checkBoxDefaultDBOnly.Checked;
            settings.ConsoleWriteGeneratedDetails = this.checkBoxShowConsoleOutput.Checked;
            settings.EnableDocumentStyleSettings = this.checkBoxDocumentStyleSettings.Checked;
            settings.EnableLineNumbering = this.checkBoxLineNumbers.Checked;
            settings.Tabs = Convert.ToInt32(this.txtTabs.Text);
            settings.CheckForNewBuild = this.chkForUpdates.Checked;
            settings.DomainOverride = this.chkDomainOverride.Checked;

            settings.DatabaseUserDataXmlMappingsString = this.textBoxDbUserMetaMappings.Text;

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
            settings.DefaultOutputDirectory = this.textBoxOutputPath.Text;

            if (this.checkBoxDisableTimeout.Checked)
            {
                settings.ScriptTimeout = -1;
            }
            else
            {
                settings.ScriptTimeout = Convert.ToInt32(this.textBoxTimeout.Text);
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
            }
            else
            {
                MessageBox.Show(this, "Choosing '<None>' will eliminate your ability to run 99.9% of the MyGeneration Templates. Most templates will crash if you run them",
                    "Warning !!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public string TextContent
        {
            get { return settings.ConnectionString; }
        }

        private bool TestConnection(bool silent)
        {
            string driver = string.Empty;
            string connstr = string.Empty;

            try
            {
                if (null != cboDbDriver.SelectedValue) driver = cboDbDriver.SelectedValue as string;
                connstr = this.txtConnectionString.Text;

                if (driver == string.Empty)
                {
                    MessageBox.Show("You Must Choose Driver", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (connstr == string.Empty && driver != "NONE")
                {
                    MessageBox.Show("Please Supply a Connection String", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (driver.ToUpper() != "NONE")
                {
                    return TestConnection(silent, driver, connstr);
                }

                return true;
            }
            catch (Exception ex)
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
                return false;
            }
            else if (!silent)
            {
                //?
            }
            tcf = null;
            return true;
        }

        public DataTable DriversTable
        {
            get
            {
                if (driversTable == null)
                {
                    driversTable = new DataTable();
                    driversTable.Columns.Add("DISPLAY");
                    driversTable.Columns.Add("VALUE");
                    driversTable.Columns.Add("ISPLUGIN");
                    driversTable.Rows.Add(new object[] { "<None>", "NONE", false });
                    driversTable.Rows.Add(new object[] { "Advantage Database Server", "ADVANTAGE", false });
                    driversTable.Rows.Add(new object[] { "Firebird", "FIREBIRD", false });
                    driversTable.Rows.Add(new object[] { "IBM DB2", "DB2", false });
                    driversTable.Rows.Add(new object[] { "IBM iSeries (AS400)", "ISERIES", false });
                    driversTable.Rows.Add(new object[] { "Interbase", "INTERBASE", false });
                    driversTable.Rows.Add(new object[] { "Microsoft SQL Server", "SQL", false });
                    driversTable.Rows.Add(new object[] { "Microsoft Access", "ACCESS", false });
                    driversTable.Rows.Add(new object[] { "MySQL", "MYSQL", false });
                    driversTable.Rows.Add(new object[] { "MySQL2", "MYSQL2", false });
                    driversTable.Rows.Add(new object[] { "Oracle", "ORACLE", false });
                    driversTable.Rows.Add(new object[] { "Pervasive", "PERVASIVE", false });
                    driversTable.Rows.Add(new object[] { "PostgreSQL", "POSTGRESQL", false });
                    driversTable.Rows.Add(new object[] { "PostgreSQL 8+", "POSTGRESQL8", false });
                    driversTable.Rows.Add(new object[] { "SQLite", "SQLITE", false });
#if !IGNORE_VISTA
                    driversTable.Rows.Add(new object[] { "VistaDB", "VISTADB", false });
#endif

                    foreach (IMyMetaPlugin plugin in MyMeta.dbRoot.Plugins.Values)
                    {
                        driversTable.Rows.Add(new object[] { plugin.ProviderName, plugin.ProviderUniqueKey, true });
                    }

                    driversTable.DefaultView.Sort = "DISPLAY";
                }
                return driversTable;
            }
        }

        #region Control Event Handlers
        private void buttonCancelSettings_Click(object sender, EventArgs e)
        {
            this.ParentForm.Close();
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                mdi.SendAlert(this.ParentForm as IMyGenContent, "UpdateDefaultSettings");
                this.OnAfterSave();
            }
        }

        private void btnOleDb_Click(object sender, System.EventArgs e)
        {
            try
            {
                string driver = string.Empty;
                string connstr = string.Empty;

                if (null != cboDbDriver.SelectedValue) driver = cboDbDriver.SelectedValue as string;
                connstr = this.txtConnectionString.Text;

                if (driver == string.Empty)
                {
                    MessageBox.Show("You Must Choose Driver", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                driver = driver.ToUpper();
                InternalDriver drv = InternalDriver.Get(driver);
                if (ShowOLEDBDialog != null)
                    drv.ShowOLEDBDialog = new ShowOleDbDialogHandler(ShowOLEDBDialog);
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
            catch { }
        }

        private void btnLanguageFile_Click(object sender, System.EventArgs e)
        {
            string fileName = this.PickFile("Langauge File (*.xml)|*.xml");

            if (string.Empty != fileName)
            {
                this.txtLanguageFile.Text = fileName;
                myMeta.LanguageMappingFileName = fileName;
                PopulateLanguages();
            }
        }

        private void btnDbTargetFile_Click(object sender, System.EventArgs e)
        {
            string fileName = this.PickFile("Database Target File (*.xml)|*.xml");

            if (string.Empty != fileName)
            {
                this.txtDbTargetFile.Text = fileName;
                myMeta.DbTargetMappingFileName = fileName;
                PopulateDbTargets();
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

            string driver = string.Empty;
            if (null != cboDbDriver.SelectedValue) driver = cboDbDriver.SelectedValue as string;
            driver = driver.ToUpper();

            this.btnOleDb.BackColor = defaultOleDbButtonColor;
            this.btnOleDb.Enabled = true;
            this.btnTestConnection.Enabled = true;

            InternalDriver drv = InternalDriver.Get(driver);
            if (drv != null)
            {
                bool oleDB = drv.IsOleDB;
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

        private void btnBrowse_Click(object sender, System.EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.SelectedPath = settings.DefaultTemplateDirectory;
            folderDialog.Description = "Select Default Template Directory";
            folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderDialog.ShowNewFolderButton = true;

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                settings.DefaultTemplateDirectory = folderDialog.SelectedPath;
                this.txtDefaultTemplatePath.Text = settings.DefaultTemplateDirectory;
            }
        }

        private void buttonBrowseOutPath_Click(object sender, System.EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.SelectedPath = settings.DefaultOutputDirectory;
            folderDialog.Description = "Select Default Output Directory";
            folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderDialog.ShowNewFolderButton = true;

            if (folderDialog.ShowDialog() == DialogResult.OK)
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
            if (text == string.Empty)
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
                info.DatabaseUserDataXmlMappingsString = this.textBoxDbUserMetaMappings.Text;

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

                this.cboDbDriver.SelectedValue = settings.DbDriver;
                this.txtConnectionString.Text = settings.ConnectionString;
                this.txtLanguageFile.Text = settings.LanguageMappingFile;
                this.txtDbTargetFile.Text = settings.DbTargetMappingFile;
                this.txtUserMetaDataFile.Text = settings.UserMetaDataFileName;

                myMeta.LanguageMappingFileName = settings.LanguageMappingFile;
                myMeta.DbTargetMappingFileName = settings.DbTargetMappingFile;

                this.cboLanguage.Enabled = true;
                this.cboDbTarget.Enabled = true;

                PopulateLanguages();
                PopulateDbTargets();

                this.cboLanguage.SelectedItem = settings.Language;
                this.cboDbTarget.SelectedItem = settings.DbTarget;

                this.textBoxDbUserMetaMappings.Text = info.DatabaseUserDataXmlMappingsString;

                settings.DatabaseUserDataXmlMappings.Clear();
                foreach (string key in info.DatabaseUserDataXmlMappings.Keys)
                {
                    settings.DatabaseUserDataXmlMappings[key] = info.DatabaseUserDataXmlMappings[key];
                }
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

                if (this.comboBoxSavedConns.Items.Count > 0)
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

        private void btnUserMetaDataFile_Click(object sender, System.EventArgs e)
        {
            string fileName = this.PickFile("Langauge File (*.xml)|*.xml");

            if (string.Empty != fileName)
            {
                this.txtUserMetaDataFile.Text = fileName;
            }
        }
#endregion
    }
}
