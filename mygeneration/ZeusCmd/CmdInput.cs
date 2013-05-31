using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Zeus;
using Zeus.Projects;
using Zeus.Serializers;
using Zeus.UserInterface;
using Zeus.Configuration;

namespace Zeus
{
	public enum ProcessMode 
	{
		Other = 0,
		Template,
		Project,
		MyMeta
	}

	class CmdInput
	{
        private ProcessMode _mode = ProcessMode.Other;
		private int _timeout = -1;
		private bool _silent = false;
		private bool _valid = true;
		private bool _showHelp = false;
        private bool _enableLog = false;
        private bool _makeRelative = false;
        private bool _installVS2005 = false;
        private bool _metaDataMerge = false;
        private bool _internalUse = true;
		private string _errorMessage = null;
		private string _pathProject = null;
		private string _pathTemplate = null;
		private string _pathOutput = null;
		private string _pathXmlData = null;
		private string _pathCollectXmlData = null;
		private string _pathLog = null;
        private string _connType = null;
        private string _connString = null;
        private string _metaDatabase1 = null;
        private string _metaDataFile1 = null;
        private string _metaDatabase2 = null;
        private string _metaDataFile2 = null;
        private string _metaDataFileMerged = null;
        private string _projectItemToRecord = null;
        private List<string> _moduleNames = new List<string>();
        private List<string> _projectItems = new List<string>();
		private ZeusTemplate _template = null;
		private ZeusProject _project = null;
		private ZeusSavedInput _savedInput = null;
		private ZeusSavedInput _inputToSave = null;
        private List<ZeusIntrinsicObject> _intrinsicObjects = new List<ZeusIntrinsicObject>();
        private List<ZeusIntrinsicObject>  _intrinsicObjectsToRemove = new List<ZeusIntrinsicObject>();

		public CmdInput(string[] args)
		{
			Parse(args);
		}

		private void Parse(string[] args) 
		{
			int numargs = args.Length;
			string arg;

			if (numargs == 0) 
			{
				this._showHelp = true;
				return;
			}

			for (int i = 0; i < numargs; i++) 
			{
				arg = args[i];

				switch (arg)
                {
                    case "-internaluse":
                        this._internalUse = true;
                        break;
                    case "-installvs2005":
                        this._installVS2005 = true;
                        break;
                    case "-tc":
                    case "-testconnection":
                        this._mode = ProcessMode.MyMeta;
                        if (numargs > (i + 2))
                        {
                            this._connType = args[++i];
                            this._connString = args[++i];
                        }
                        else
                        {
                            this._valid = false;
                            this._errorMessage = "Invalid switch usage: " + arg;
                        }
                        break;
					case "-aio":
					case "-addintrinsicobject":
						if (numargs > (i+3)) 
						{
							string assembly = args[++i];
							string classpath = args[++i];
							string varname = args[++i];

							ZeusIntrinsicObject iobj = new ZeusIntrinsicObject(assembly, classpath, varname);
							this._intrinsicObjects.Add(iobj);
						}
						else 
						{
							this._valid = false;
							this._errorMessage = "Invalid switch usage: " + arg;
						}
						break;
					case "-rio":
					case "-removeintrinsicobject":
                        if (numargs > (i + 1))
                        {
                            string varname = args[++i];
                            foreach (ZeusIntrinsicObject zio in ZeusConfig.Current.IntrinsicObjects)
                            {
                                if (zio.VariableName == varname && !_intrinsicObjectsToRemove.Contains(zio))
                                {
                                    this._intrinsicObjectsToRemove.Add(zio);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            this._valid = false;
                            this._errorMessage = "Invalid switch usage: " + arg;
                        }
                        break;
					case "-r":
					case "-relative":
						this._makeRelative = true;
						break;
					case "-s":
					case "-silent":
						this._silent = true;
						break;
					case "-?":
					case "-h":
					case "-help":
						this._showHelp = true;
						break;
					case "-l":
					case "-logfile":
						this._enableLog = true;
						if (numargs > (i+1)) 
							this._pathLog = args[++i];
						else 
						{
							this._valid = false;
							this._errorMessage = "Invalid switch usage: " + arg;
						}
						break;
					case "-o":
					case "-outfile":
						if (numargs > (i+1)) 
							this._pathOutput = args[++i];
						else 
						{
							this._valid = false;
							this._errorMessage = "Invalid switch usage: " + arg;
						}
						break;
					case "-p":
					case "-project":
						this._mode = ProcessMode.Project;
						if (numargs > (i+1)) 
							this._pathProject = args[++i];
						else 
						{
							this._valid = false;
							this._errorMessage = "Invalid switch usage: " + arg;
						}
						break;
					case "-mumd":
                    case "-mergeusermetadata":
                        this._mode = ProcessMode.MyMeta;
                        this._metaDataMerge = true;
                        if (numargs > (i + 5))
                        {
                            this._metaDataFile1 = args[++i];
                            this._metaDatabase1 = args[++i];
                            this._metaDataFile2 = args[++i];
                            this._metaDatabase2 = args[++i];
                            this._metaDataFileMerged = args[++i];
                        }
                        else
                        {
                            this._valid = false;
                            this._errorMessage = "Invalid switch usage: " + arg;
                        }
                        break;
                    case "-m":
                    case "-pf":
                    case "-module":
                    case "-projectfolder":
						if (numargs > (i+1)) 
						{
							string data = args[++i];
							if (!_moduleNames.Contains(data))
								this._moduleNames.Add(data);
						}
						else 
						{
							this._valid = false;
							this._errorMessage = "Invalid switch usage: " + arg;
						}
                        break;
                    case "-rti":
                    case "-recordtemplateinstance":
                        if (numargs > (i + 1))
                        {
                            this._projectItemToRecord = args[++i];
                            this._mode = ProcessMode.Project;
                        }
                        else
                        {
                            this._valid = false;
                            this._errorMessage = "Invalid switch usage: " + arg;
                        }
                        break;
                    case "-ti":
                    case "-templateinstance":
                        if (numargs > (i + 1))
                        {
                            string data = args[++i];
                            if (!_projectItems.Contains(data))
                                this._projectItems.Add(data);
                        }
                        else
                        {
                            this._valid = false;
                            this._errorMessage = "Invalid switch usage: " + arg;
                        }
                        break;
					case "-i":
					case "-inputfile":
						this._mode = ProcessMode.Template;
						if (numargs > (i+1)) 
							this._pathXmlData = args[++i];
						else  
						{
							this._valid = false;
							this._errorMessage = "Invalid switch usage: " + arg;
						}
						break;
					case "-t":
					case "-template":
						this._mode = ProcessMode.Template;
						if (numargs > (i+1)) 
							this._pathTemplate = args[++i];
						else 
						{
							this._valid = false;
							this._errorMessage = "Invalid switch usage: " + arg;
						}
						break;
					case "-c":
					case "-collect":
						this._mode = ProcessMode.Template;
						if (numargs > (i+1)) 
							this._pathCollectXmlData = args[++i];
						else 
						{
							this._valid = false;
							this._errorMessage = "Invalid switch usage: " + arg;
						}
						break;
					case "-e":
					case "-timeout":
						if (numargs > (i+1)) 
						{
							try 
							{
								this._timeout = Int32.Parse(args[++i]);
							}
							catch 
							{
								this._timeout = -1;
							}
						}
						else 
						{
							this._valid = false;
							this._errorMessage = "Invalid switch usage: " + arg;
						}
						break;
					default:
						_valid = false;
						this._errorMessage = "Invalid argument: " + arg;
						break;
				}
			}

			if (this._makeRelative) 
			{
				if (this._pathCollectXmlData != null)
					this._pathCollectXmlData = Zeus.FileTools.MakeAbsolute(this._pathCollectXmlData, FileTools.ApplicationPath);
				if (this._pathLog != null)
                    this._pathLog = Zeus.FileTools.MakeAbsolute(this._pathLog, FileTools.ApplicationPath);
				if (this._pathOutput != null)
                    this._pathOutput = Zeus.FileTools.MakeAbsolute(this._pathOutput, FileTools.ApplicationPath);
				if (this._pathProject != null)
                    this._pathProject = Zeus.FileTools.MakeAbsolute(this._pathProject, FileTools.ApplicationPath);
				if (this._pathTemplate != null)
                    this._pathTemplate = Zeus.FileTools.MakeAbsolute(this._pathTemplate, FileTools.ApplicationPath);
				if (this._pathXmlData != null)
                    this._pathXmlData = Zeus.FileTools.MakeAbsolute(this._pathXmlData, FileTools.ApplicationPath);
                if (this._metaDataFile1 != null)
                    this._metaDataFile1 = Zeus.FileTools.MakeAbsolute(this._metaDataFile1, FileTools.ApplicationPath);
                if (this._metaDataFile2 != null)
                    this._metaDataFile2 = Zeus.FileTools.MakeAbsolute(this._metaDataFile2, FileTools.ApplicationPath);
                if (this._metaDataFileMerged != null)
                    this._metaDataFileMerged = Zeus.FileTools.MakeAbsolute(this._metaDataFileMerged, FileTools.ApplicationPath);
			}


			// Validate required fields are filled out for the selected mode.
			if (_valid) 
			{
                if (this.Mode == ProcessMode.MyMeta) 
                {
                    if (this._metaDataMerge)
                    {
                        if (!System.IO.File.Exists(_metaDataFile1) || !System.IO.File.Exists(_metaDataFile2))
                        {
                            _valid = false;
                            this._errorMessage = "The two source files must exist for the merge to work!";
                        }
                    }
                }
				else if (this._mode == ProcessMode.Project) 
				{
					if (this._pathProject == null)
					{
						_valid = false;
						this._errorMessage = "Project Path Required";
					}
					else 
					{
						try 
						{
							this._project = new ZeusProject(this._pathProject);
							this._project.Load();
						}
						catch (Exception ex)
						{
							this._project = null;
							this._valid = false;
							this._errorMessage = ex.Message;
						}
					}


                    if (this._pathTemplate != null)
                    {
                        try
                        {
                            this._template = new ZeusTemplate(this._pathTemplate);
                        }
                        catch (Exception ex)
                        {
                            this._template = null;
                            this._valid = false;
                            this._errorMessage = ex.Message;
                        }
                    }
				}
				else if (this._mode == ProcessMode.Template) 
				{
					if ( (this._pathTemplate == null) && (this._pathXmlData == null) )
					{
						_valid = false;
						this._errorMessage = "Template path or XML input path required.";
					}
					else 
					{
						if (this._pathTemplate != null)
						{
							try 
							{
								this._template = new ZeusTemplate(this._pathTemplate);
							}
							catch (Exception ex)
							{
								this._template = null;
								this._valid = false;
								this._errorMessage = ex.Message;
							}
						}

						if ( (this._valid) && (this._pathXmlData != null) )
						{
							try 
							{
								this._savedInput = new ZeusSavedInput(this._pathXmlData);
								this._savedInput.Load();

								if (this._template == null) 
								{
									this._template = new ZeusTemplate(this._savedInput.InputData.TemplatePath);
								}
							}
							catch (Exception ex)
							{
								this._savedInput = null;
								this._template = null;
								this._valid = false;
								this._errorMessage = ex.Message;
							}
						}

						if ( (this._valid) && (this._pathCollectXmlData != null) )
						{
							try 
							{
								this._inputToSave = new ZeusSavedInput(this._pathCollectXmlData);
								this._inputToSave.InputData.TemplatePath = this._template.FilePath + this._template.FileName;
								this._inputToSave.InputData.TemplateUniqueID = this._template.UniqueID;
							}
							catch (Exception ex)
							{
								this._inputToSave = null;
								this._valid = false;
								this._errorMessage = ex.Message;
							}
						}
					}
				}

			}
		}

		public ProcessMode Mode
		{ get { return _mode; } }

        public bool InternalUseOnly
        { get { return _internalUse; } }

        public bool InstallVS2005
		{ get { return _installVS2005; } }

		public bool MakeRelative
		{ get { return _makeRelative; } }

		public bool IsSilent
		{ get { return _silent; } }

		public bool IsValid
		{ get { return _valid; } }

		public bool ShowHelp
		{ get { return _showHelp; } }

		public int Timeout
		{ get { return _timeout; } }

		public bool EnableLog
		{ get { return _enableLog; } }

		public string ErrorMessage
		{ get { return _errorMessage; } }

		public string PathProject
		{ get { return _pathProject; } }

		public string PathTemplate
		{ get { return _pathTemplate; } }

		public string PathXmlData
		{ get { return _pathXmlData; } }

		public string PathCollectXmlData
		{ get { return _pathCollectXmlData; } }

		public string PathOutput
		{ get { return _pathOutput; } }

		public string PathLog
        { get { return _pathLog; } }

        public List<ZeusIntrinsicObject> IntrinsicObjects
        { get { return _intrinsicObjects; } }

        public List<ZeusIntrinsicObject> IntrinsicObjectsToRemove
        { get { return _intrinsicObjectsToRemove; } }

        public List<string> ModuleNames
		{ get { return _moduleNames; } }

        public List<string> ProjectItems
        { get { return _projectItems; } }

		public ZeusTemplate Template
		{ get { return _template; } }

		public ZeusProject Project
		{ get { return _project; } }

		public ZeusSavedInput SavedInput
		{ get { return _savedInput; } }

		public ZeusSavedInput InputToSave
		{ get { return _inputToSave; } }
		
		public string ConnectionType
		{ get { return this._connType; } }

		public string ConnectionString
		{ get { return _connString; } }
        
        public bool MergeMetaFiles
        { get { return _metaDataMerge; } }

		public string MetaFile1
        { get { return _metaDataFile1; } }

		public string MetaDatabase1
        { get { return _metaDatabase1; } }

		public string MetaFile2
		{ get { return _metaDataFile2; } }

		public string MetaDatabase2
        { get { return _metaDatabase2; } }

        public string MetaFileMerged
        { get { return _metaDataFileMerged; } }

        public string ProjectItemToRecord
        { get { return _projectItemToRecord; } }

	}
}
