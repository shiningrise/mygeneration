using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using Scintilla;
using Scintilla.Forms;
using Scintilla.Enums;
using Scintilla.Configuration;
using MyMeta;

namespace MyGeneration.UI.Plugins.SqlTool
{
    public partial class SqlToolUserControl : UserControl
    {
        private IMyGenerationMDI _mdi;
        private ScintillaConfig _scintillaConfig;
        private string _filename = string.Empty;
        private string _fileId;
        private string _dbDriver = null;
        private string _connString = null;
        private string _databaseName = null;
        private string _commandSeperator = "GO";
        private List<string> _databaseNames = new List<string>();
        private bool _isNew = true;
        private bool _isUsingCurrentConnection = false;

        public SqlToolUserControl()
        {
            InitializeComponent();

            _scintillaConfig = new ScintillaConfig();
            this.scintilla.Configure = _scintillaConfig.Configure;
            this.scintilla.ConfigurationLanguage = "sql";
            this.scintilla.SmartIndentType = SmartIndent.Simple;
        }

        public void Initialize(IMyGenerationMDI mdi, ToolStripItemCollection menuItems)
        {
            this.scintilla.AddShortcuts(menuItems);
            this._fileId = Guid.NewGuid().ToString();
            _mdi = mdi;
            RefreshConnectionInfo();
        }

        public dbRoot CurrentDbRoot()
        {
            dbRoot mymeta = null;

            try
            {
                mymeta = _mdi.PerformMdiFuntion(this.Parent as IMyGenContent, "getstaticdbroot") as dbRoot;
                _isUsingCurrentConnection = true;
                if (!(mymeta is dbRoot))
                {
                    mymeta = new dbRoot();
                    mymeta.Connect(DbDriver, ConnectionString);
                    _isUsingCurrentConnection = false;
                }
            }
            catch (Exception ex)
            {
                this._mdi.ErrorList.AddErrors(ex);
            }

            return mymeta;
        }

        public IDbConnection OpenConnection()
        {
            return OpenConnection(null);
        }

        public IDbConnection OpenConnection(string database)
        {
            //dbRoot mymeta = new dbRoot();
            dbRoot mymeta = CurrentDbRoot();
            IDbConnection _connection = null;
            try
            {
                // special code to handle databases that cannot have multiple open connections.
                object v = mymeta.PluginSpecificData(DbDriver, "requiresinternalconnection");
                if (v != null && v.GetType() == typeof(bool))
                {
                    if ((bool)v)
                    {
                        _connection = mymeta.PluginSpecificData(DbDriver, "internalconnection") as IDbConnection; 
                    }
                }

                // The standard connection code.
                if (_connection == null)
                {
                    _connection = mymeta.BuildConnection(DbDriver, ConnectionString);
                }

                if (_connection != null)
                {
                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }

                    if (!string.IsNullOrEmpty(database))
                    {
                        mymeta.ChangeDatabase(_connection, database);
                    }
                }
            }
            catch (Exception ex)
            {
                this._mdi.ErrorList.AddErrors(ex);
            }
            return _connection;
        }


        public void CloseConnection(IDbConnection connection)
        {
            if (_isUsingCurrentConnection)
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }
        }

        public bool IsEmpty
        {
            get { return (this.scintilla.Length == 0); }
        }

        public bool IsNew
        {
            get { return (this._isNew); }
        }

        public List<string> DatabaseNames
        {
            get
            {
                return _databaseNames;
            }
        }

        public string CommandSeperator
        {
            get
            {
                return _commandSeperator;
            }
            set
            {
                this._commandSeperator = value;
            }
        }

        public string SelectedDatabase
        {
            get
            {
                return _databaseName;
            }
            set
            {
                this._databaseName = value;
            }
        }

        public string ConnectionString
        {
            get
            {
                if (_connString == null)
                {
                    _connString = _mdi.PerformMdiFuntion(this.Parent as IMyGenContent, "getmymetaconnection") as String;
                }
                return _connString;
            }
        }

        public string DbDriver
        {
            get
            {
                if (_dbDriver == null)
                {
                    _dbDriver = _mdi.PerformMdiFuntion(this.Parent as IMyGenContent, "getmymetadbdriver") as String;
                }
                return _dbDriver;
            }
        }

        public void RefreshConnectionInfo()
        {
            _dbDriver = null;
            _connString = null;
            dbRoot mymeta = CurrentDbRoot();
            _databaseNames.Clear();

            if (mymeta.Databases != null)
            {
                foreach (IDatabase db in mymeta.Databases)
                {
                    _databaseNames.Add(db.Name);
                }

                if (mymeta.Databases.Count >= 1)
                {
                    try
                    {
                        //show databases dropdown - select current
                        if (mymeta.DefaultDatabase != null && (string.IsNullOrEmpty(_databaseName)) || (!_databaseNames.Contains(_databaseName)))
                        {
                            this._databaseName = mymeta.DefaultDatabase.Name;
                        }
                    }
                    catch (Exception)
                    {
                        this._databaseName = mymeta.Databases[0].Name;
                    }
                }
            }
        }

        public void Open(string filename)
        {
            this._filename = filename;
            if (File.Exists(_filename))
            {
                try
                {
                    this.scintilla.Clear();
                    this.scintilla.Text = File.ReadAllText(_filename);
                    _isNew = false;
                    SetClean();
                }
                catch (Exception x)
                {
                    _mdi.Console.Write(x);
                    _isNew = true;
                }
            }
        }

        public List<string> SqlToExecute
        {
            get
            {
                List<string> commands = new List<string>();
                StringBuilder sb = new StringBuilder();
                string exectext = this.scintilla.GetSelectedText();
                if (string.IsNullOrEmpty(exectext))
                {
                    if (string.IsNullOrEmpty(this.CommandSeperator))
                    {
                        commands.Add(scintilla.Text);
                    }
                    else
                    {
                        for (int i = 0; i < scintilla.LineCount; i++)
                        {
                            string line = scintilla.Line[i].TrimEnd('\r', '\n');
                            if (line.TrimEnd() == CommandSeperator)
                            {
                                exectext = sb.ToString().Trim();
                                if (exectext.Length > 0)
                                {
                                    commands.Add(exectext);
                                }
                                sb.Remove(0, sb.Length);
                            }
                            else
                            {
                                sb.AppendLine(line);
                            }
                        }
                        exectext = sb.ToString().Trim();
                        if (exectext.Length > 0)
                        {
                            commands.Add(exectext);
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(this.CommandSeperator))
                    {
                        commands.Add(exectext);
                    }
                    else
                    {
                        string[] lines = exectext.Split('\n');
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string line = lines[i].TrimEnd('\r', '\n');
                            if (line.TrimEnd() == CommandSeperator)
                            {
                                exectext = sb.ToString().Trim();
                                if (exectext.Length > 0)
                                {
                                    commands.Add(exectext);
                                }
                                sb.Remove(0, sb.Length);
                            }
                            else
                            {
                                sb.AppendLine(line);
                            }
                        }
                        exectext = sb.ToString().Trim();
                        if (exectext.Length > 0)
                        {
                            commands.Add(exectext);
                        }
                    }
                }
                return commands;
            }
        }

        private void FileSave()
        {
            if (!string.IsNullOrEmpty(this.FileName))
            {
                string path = this.FileName;
                FileInfo fi = new FileInfo(path);
                if (fi.Exists && fi.IsReadOnly)
                {
                    MessageBox.Show(this, "File is read only.");
                    this._mdi.Console.Write("File \"{0}\" is read only, cannot save.", fi.FullName);
                }
                else
                {
                    try
                    {
                        File.WriteAllText(path, scintilla.Text);
                        SetClean();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Error saving file. " + ex.Message);
                        this._mdi.Console.Write(ex);
                    }

                }
            }
        }

        private void FileSaveAs(string path)
        {
            bool isopen = _mdi.IsDocumentOpen(path, this.Parent as IMyGenDocument);

            if (!isopen)
            {
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                {
                    if (fi.IsReadOnly)
                    {
                        MessageBox.Show(this, "File is read only, cannot save.");
                        this._mdi.Console.Write("File \"{0}\" is read only, cannot save.", fi.FullName);
                    }
                    else
                    {
                        try
                        {
                            File.WriteAllText(path, scintilla.Text);
                            this._filename = path;

                            SetClean();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "Error saving file. " + ex.Message);
                            this._mdi.Console.Write("Error saving file.");
                            this._mdi.Console.Write(ex);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(this, "The template you are trying to overwrite is currently open.\r\nClose the editor window for that template if you want to overwrite it.");
            }
        }

        public void Save()
        {
            if (this._isNew)
            {
                this.SaveAs();
            }
            else
            {
                this.FileSave();
            }
            this.scintilla.GrabFocus();
        }

        public void SaveAs()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "SQL Files (*.sql)|*.sql|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            saveFileDialog.FileName = this.FileName;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                myStream = saveFileDialog.OpenFile();

                if (null != myStream)
                {
                    myStream.Close();
                    this.FileSaveAs(saveFileDialog.FileName);
                    _isNew = false;
                }
            }
            this.scintilla.GrabFocus();
        }

        public void Execute()
        {
            for (int i = (this.tabControlResults.TabPages.Count-1); i > 0 ; i--)
            {
                this.tabControlResults.TabPages.RemoveAt(i);
            }

            IDbConnection conn = null;
            IDataReader r = null;
            int resultSetIndex = 0, numRows = 0, gridIndex = 1;
            try
            {
                conn = OpenConnection(_databaseName);

                List<string> sqlCommands = this.SqlToExecute;
                foreach (string sql in sqlCommands)
                {
                    if (conn == null)
                    {
                        conn = OpenConnection(_databaseName);
                    }
                    
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    IDbCommand db = conn.CreateCommand();
                    db.CommandText = sql;
                    r = db.ExecuteReader();

                    do
                    {
                        DataGridView dgv;
                        if (resultSetIndex > 0)
                        {
                            dgv = new DataGridView();
                            dgv.Dock = DockStyle.Fill;
                            dgv.AllowUserToAddRows = false;
                            dgv.AllowUserToDeleteRows = false;
                            dgv.AllowUserToOrderColumns = true;
                            dgv.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                            dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                            dgv.ReadOnly = true;
                        }
                        else
                        {
                            dgv = this.dataGridViewResults;
                        }
                        dgv.Rows.Clear();
                        dgv.Columns.Clear();
                        int rowindex = 0;

                        if ((r != null) && (!r.IsClosed))
                        {
                            while (r.Read())
                            {
                                if (rowindex == 0)
                                {
                                    for (int i = 0; i < r.FieldCount; i++)
                                    {
                                        dgv.Columns.Add(r.GetName(i), r.GetName(i));
                                    }
                                }

                                dgv.Rows.Add();
                                for (int i = 0; i < r.FieldCount; i++)
                                {
                                    dgv.Rows[rowindex].Cells[i].Value = r[i];
                                }
                                rowindex++;
                                numRows++;
                            }
                        }

                        if (resultSetIndex > 0)
                        {
                            if (rowindex > 0)
                            {
                                string post = (++gridIndex).ToString().PadLeft(2, '0');
                                dgv.Name = "dataGridViewResults" + post;
                                TabPage ntp = new TabPage("Results " + post);
                                ntp.Controls.Add(dgv);
                                this.tabControlResults.TabPages.Add(ntp);
                            }
                            else
                            {
                                dgv.Dispose();
                                dgv = null;
                            }
                        }

                        resultSetIndex++;
                    } while (r.NextResult());

                    r.Close();
                    r.Dispose();

                    if (numRows == 0)
                    {
                        dataGridViewResults.Columns.Add("Result", "Result");
                        dataGridViewResults.Rows.Add();
                        dataGridViewResults.Rows[0].Cells[0].Value = "";
                    }
                }

                CloseConnection(conn);
            }
            catch (Exception ex)
            {
                if (r != null) r.Dispose();
                if (conn != null) conn.Dispose();

                dataGridViewResults.Rows.Clear();
                dataGridViewResults.Columns.Clear();
                dataGridViewResults.Columns.Add("Error", "Error");
                dataGridViewResults.Rows.Add();
                dataGridViewResults.Rows[0].Cells[0].Value = ex.Message;

                this._mdi.ErrorList.AddErrors(ex);
            }
        }

        public string FileName
        {
            get
            {
                return _filename; 
            }
        }

        public bool IsDirty
        {
            get
            {
                return (this.scintilla.IsModify);
            }
        }

        public string DocumentIndentity
        {
            get { return string.IsNullOrEmpty(_filename) ? _fileId : _filename; }
        }

        public string TextContent
        {
            get
            {
                return scintilla.Text;
            }
            set
            {
                this.scintilla.Text = value;
            }
        }

        protected void SetClean()
        {
            scintilla.EmptyUndoBuffer();
            scintilla.SetSavePoint();
        }

        public void Cut()
        {
            if (this.scintilla.Focused)
            {
                this.scintilla.Cut();
            }
        }

        public void Copy()
        {
            if (this.scintilla.Focused)
            {
                this.scintilla.Copy();
            }
        }

        public void Paste()
        {
            if (this.scintilla.Focused)
            {
                this.scintilla.Paste();
            }
        }
    }
}
