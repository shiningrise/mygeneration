using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using MyMeta;

namespace MyGeneration.UI.Plugins.EntityManager
{
    public partial class TableBrowser : DockContent, IMyGenDocument
    {
        private IMyGenerationMDI _mdi;
        private string _filename = string.Empty;
        IDatabase _database = null;
        ITable _table = null;

        #region db

        private string _fileId;
        private string _dbDriver = null;
        private string _connString = null;
        private string _databaseName = null;
        private string _tableName = null;
        private string _commandSeperator = "GO";
        private List<string> _databaseNames = new List<string>();
        private bool _isNew = true;
        private bool _isUsingCurrentConnection = false;

        #endregion

        #region  实现接口
        
        public TableBrowser(IMyGenerationMDI mdi)
        {
            //,ITable table
            this._mdi = mdi;
            this.DockAreas = DockAreas.Document;
            //_table = table;

            InitializeComponent();
            
        }

        //public TableBrowser(IMyGenerationMDI mdi,string databaseName,string tableName)
        //{
        //    //,ITable table
        //    this._mdi = mdi;
        //    this.DockAreas = DockAreas.Document;
        //    this._databaseName = databaseName;
        //    this._tableName = tableName;
        //    this._database = this.CurrentDbRoot().Databases[this._databaseName];
        //    _table = _database.Tables[_tableName];

        //    InitializeComponent();
        //}

        public string DocumentIndentity
        {
            get { return "test"; }
        }

        public string TextContent
        {
            get { return "hello"; }
        }

        public ToolStrip ToolStrip
        {
            get { return this.toolStrip1; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            //if (command.Equals("UpdateDefaultSettings", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    sqlToolUserControl1.RefreshConnectionInfo();
            //    BindForm();
            //}
        }

        public bool CanClose(bool allowPrevent)
        {
        //    throw new NotImplementedException();
            return true;
        }

        public DockContent DockContent
        {
            get { return this; }
        }
        
        #endregion

        #region database
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
            get { return false; }
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

        #endregion

        private void TableBrowser_Load(object sender, EventArgs e)
        {
            BindData();
            
        }

        private void BindData()
        {
            var dbRoot = this.CurrentDbRoot();
            if (dbRoot != null)
            {
                var dbs = dbRoot.Databases;
                this.tableEditorControl1.BindData(dbs);
            }
        }

        private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            switch (e.Button.Tag as string)
            {
                case "refresh":
                    BindData();
                    break;
                case "execute":
                    break;
            }
        }
    }
}
