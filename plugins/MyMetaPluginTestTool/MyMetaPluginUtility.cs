using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using MyMeta;

namespace MyMetaPluginTestTool
{
    public partial class MyMetaPluginUtility : Form, IMyMetaTestContext
    {
        private FileInfo _cfgFile = null;
        private List<Exception> _errors = new List<Exception>();

        public MyMetaPluginUtility()
        {
            InitializeComponent();
        }

        public FileInfo ConfigFile
        {
            get
            {
                if (_cfgFile == null)
                {
                    _cfgFile = new FileInfo(Assembly.GetEntryAssembly().Location + ".cfg");
                }
                return _cfgFile;
            }
        }

        private void MyMetaPluginUtility_Load(object sender, EventArgs e)
        {
            //dbRoot root = new dbRoot();
            foreach (IMyMetaPlugin plugin in dbRoot.Plugins.Values)
            {
                IMyMetaPlugin pluginTest = dbRoot.Plugins[plugin.ProviderUniqueKey] as IMyMetaPlugin;
                if (pluginTest == plugin)
                {
                    this.comboBoxPlugins.Items.Add(plugin.ProviderUniqueKey);
                }
            }
            dbRoot root = new dbRoot();
            foreach (string dbd in Enum.GetNames(typeof(dbDriver)))
            {
                if (!dbd.Equals("Plugin", StringComparison.CurrentCultureIgnoreCase))
                    this.comboBoxPlugins.Items.Add(dbd.ToUpper());
            }

            if (ConfigFile.Exists)
            {
                string[] lines = File.ReadAllLines(ConfigFile.FullName);
                if (lines.Length > 1)
                {
                    int idx = this.comboBoxPlugins.FindStringExact(lines[0]); 
                    if (idx >= 0) this.comboBoxPlugins.SelectedIndex = idx;
                    this.textBoxConnectionString.Text = lines[1];
                }
            }
        }

        private void comboBoxPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxPlugins.SelectedItem != null)
            {
                IMyMetaPlugin plugin = dbRoot.Plugins[this.comboBoxPlugins.SelectedItem.ToString()] as IMyMetaPlugin;
                if (plugin != null)
                {
                    this.checkBoxPlugin.Enabled = true;
                    this.checkBoxPlugin.Checked = true;
                    this.textBoxConnectionString.Text = plugin.SampleConnectionString;
                }
                else
                {
                    //dbRoot root = new dbRoot();
                    this.checkBoxPlugin.Checked = false;
                    this.checkBoxPlugin.Enabled = false;
                    this.textBoxConnectionString.Text = string.Empty;
                }
            }
        }

        private void MyMetaPluginUtility_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(this.comboBoxPlugins.SelectedItem.ToString());
                sb.AppendLine(this.textBoxConnectionString.Text);

                File.WriteAllText(ConfigFile.FullName, sb.ToString());
            }
            catch {}
        }


        private void buttonTest_Click(object sender, EventArgs e)
        {
            bool doPluginTests = this.checkBoxPlugin.Checked;
            bool doAPITests = this.checkBoxAPI.Checked;

            this.textBoxResults.Clear();

            if (doPluginTests)
            {
                MyMetaPluginTests.Test(this);
            }

            if (doAPITests)
            {
                MyMetaAPITests.Test(this);
            }
        }

        private void checkBoxTables_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxTables.Checked)
            {
                this.checkBoxTableColumns.Checked = false;
                this.checkBoxTableOther.Checked = false;
            }
            this.checkBoxTableColumns.Enabled = checkBoxTables.Checked;
            this.checkBoxTableOther.Enabled = checkBoxTables.Checked;
        }

        private void checkBoxViews_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxViews.Checked)
            {
                this.checkBoxViewColumns.Checked = false;
                this.checkBoxViewOther.Checked = false;
            }
            this.checkBoxViewColumns.Enabled = checkBoxViews.Checked;
            this.checkBoxViewOther.Enabled = checkBoxViews.Checked;
        }

        private void checkBoxProcedures_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxProcedures.Checked)
            {
                this.checkBoxParameters.Checked = false;
                this.checkBoxProcOther.Checked = false;
            }
            this.checkBoxParameters.Enabled = checkBoxProcedures.Checked;
            this.checkBoxProcOther.Enabled = checkBoxProcedures.Checked;
        }

        #region IMyMetaTestContext Members

        public string ProviderType
        {
            get { return this.comboBoxPlugins.SelectedItem.ToString(); }
        }

        public string ConnectionString
        {
            get { return this.textBoxConnectionString.Text; }
        }

        public bool IncludeTables
        {
            get { return this.checkBoxTables.Checked; }
        }

        public bool IncludeTableColumns
        {
            get { return this.checkBoxTableColumns.Checked; }
        }

        public bool IncludeTableOther
        {
            get { return this.checkBoxTableOther.Checked; }
        }

        public bool IncludeViews
        {
            get { return this.checkBoxViews.Checked; }
        }

        public bool IncludeViewColumns
        {
            get { return this.checkBoxViewColumns.Checked; }
        }

        public bool IncludeViewOther
        {
            get { return this.checkBoxViewOther.Checked; }
        }

        public bool IncludeProcedures
        {
            get { return this.checkBoxProcedures.Checked; }
        }

        public bool IncludeParameters
        {
            get { return this.checkBoxParameters.Checked; }
        }

        public bool IncludeProcOther
        {
            get { return this.checkBoxProcOther.Checked; }
        }

        public bool DefaultDatabaseOnly
        {
            get { return this.checkBoxDefaultDb.Checked; }
        }

        public bool HasErrors
        {
            get
            {
                return _errors.Count > 0;
            }
        }

        public List<Exception> Errors
        {
            get { return _errors; }
        }

        public void AppendLog(string message)
        {
            this.textBoxResults.AppendText(DateTime.Now.ToString());
            this.textBoxResults.AppendText(" - " + message);
            this.textBoxResults.AppendText(Environment.NewLine);
        }

        public void AppendLog(string message, Exception ex)
        {
            Errors.Add(ex);
            this.textBoxResults.AppendText(DateTime.Now.ToString());
            this.textBoxResults.AppendText(" - " + message);
            this.textBoxResults.AppendText(": " + ex.Message + ex);
            this.textBoxResults.AppendText(Environment.NewLine);
        }

        #endregion
    }
}
