using System;
using System.Text;
using System.Data;
using System.Collections;
using System.IO;
using System.Xml;
using Zeus;
using Zeus.Configuration;
using Zeus.Projects;
using Zeus.Serializers;
using Zeus.UserInterface;
using MyGeneration;
using MyMeta;
using Microsoft.Win32;

namespace Zeus
{
	/// <summary>
	/// Summary description for ZeusCmd.
	/// </summary>
	class ZeusCmd
	{
        private const string EXT_XML_NAMESPACE = "http://schemas.microsoft.com/AutomationExtensibility";
		private Log _log;
		private CmdInput _argmgr;
		private int _returnValue = 0;

        public ZeusCmd(string[] args)
        {
            _log = new Log();
            if (_ProcessArgs(args))
            {
                _log.IsInternalUseMode = _argmgr.InternalUseOnly;
                switch (_argmgr.Mode)
                {
                    case ProcessMode.Project:
                        _ProcessProject();
                        break;
                    case ProcessMode.Template:
                        _ProcessTemplate();
                        break;
                    case ProcessMode.MyMeta:
                        _ProcessMyMeta();
                        break;
                    case ProcessMode.Other:
                        if (_argmgr.InstallVS2005)
                        {
                            // Install Visual Studio 2005 Add In
                            _InstallVS2005();
                        }
                        break;
                }
            }
            else
            {
                _returnValue = -1;
            }

            _log.Close();
        }

		public int ReturnValue { get { return _returnValue; } }

		private bool _ProcessArgs(string[] args) 
		{
			//Process arguments, validate, fill variables
			_argmgr = new CmdInput(args);

			if (_argmgr.ShowHelp) 
			{
				Console.Write(HELP_TEXT);
				return false;
			}

			if (_argmgr.IntrinsicObjects.Count > 0) 
			{
				ZeusConfig cfg = ZeusConfig.Current;
                foreach (ZeusIntrinsicObject io in _argmgr.IntrinsicObjects)
                {
                    bool exists = false;
                    foreach (ZeusIntrinsicObject existingObj in cfg.IntrinsicObjects)
                    {
                        if (existingObj.VariableName == io.VariableName)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                    {
                        cfg.IntrinsicObjects.Add(io);
                    }
                }
				cfg.Save();
            }
            
            if (_argmgr.IntrinsicObjectsToRemove.Count > 0)
            {
                ZeusConfig cfg = ZeusConfig.Current;
                foreach (ZeusIntrinsicObject io in _argmgr.IntrinsicObjectsToRemove)
                {
                    cfg.IntrinsicObjects.Remove(io);
                }
                cfg.Save();
            }
			
			if (_argmgr.IsValid) 
			{
				if (_argmgr.EnableLog) 
				{
					_log.IsLogEnabled = true;
					_log.FileName = _argmgr.PathLog;
				}

				_log.IsConsoleEnabled = !_argmgr.IsSilent;
				return true;
			}
			else 
			{
				Console.WriteLine(_argmgr.ErrorMessage);
				Console.Write("Use the \"-?\" switch to view the help.");
				return false;
			}
		}

        private void _InstallVS2005()
        {
            // Parameters required to pass in from installer
            string productName = "MyGeneration Visual Studio 2005 Add-in";
            string assemblyName = "MyGenVS2005";

            // Setup .addin path and assembly path
            string addinTargetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Visual Studio 2005\Addins");
            string assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string addinControlFileName = assemblyName + ".Addin";
            string addinAssemblyFileName = assemblyName + ".dll";

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(addinTargetPath);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                string sourceFile = Path.Combine(assemblyPath, addinControlFileName);
                XmlDocument doc = new XmlDocument();
                doc.Load(sourceFile);
                XmlNamespaceManager xnm = new XmlNamespaceManager(doc.NameTable);
                xnm.AddNamespace("def", EXT_XML_NAMESPACE);

                // Update Addin/Assembly node
                XmlNode node = doc.SelectSingleNode("/def:Extensibility/def:Addin/def:Assembly", xnm);
                if (node != null)
                {
                    node.InnerText = Path.Combine(assemblyPath, addinAssemblyFileName);
                }

                // Update ToolsOptionsPage/Assembly node
                node = doc.SelectSingleNode("/def:Extensibility/def:ToolsOptionsPage/def:Category/def:SubCategory/def:Assembly", xnm);
                if (node != null)
                {
                    node.InnerText = Path.Combine(assemblyPath, addinAssemblyFileName);
                }

                doc.Save(sourceFile);

                string targetFile = Path.Combine(addinTargetPath, addinControlFileName);
                File.Copy(sourceFile, targetFile, true);
            }
            catch (Exception ex)
            {
                _log.Write(ex);
                _log.Write("Installation of visual studio add-in failed.");
            }
        }

        private void _ProcessMyMeta()
        {
            if (_argmgr.MergeMetaFiles)
            {
                try 
                {
                    dbRoot.MergeUserMetaDataFiles(_argmgr.MetaFile1, _argmgr.MetaDatabase1, _argmgr.MetaFile2, _argmgr.MetaDatabase2, _argmgr.MetaFileMerged);
                }
                catch (Exception ex)
                {
                    this._log.Write(ex);
                    this._log.Write("Merge UserMetaData files failed.");
                }
            }
            else
            {
                IDbConnection connection = null;
                try
                {
                    dbRoot mymeta = new dbRoot();
                    connection = mymeta.BuildConnection(_argmgr.ConnectionType, _argmgr.ConnectionString);
                    _log.Write("Beginning test for {0}: {1}", connection.GetType().ToString(), _argmgr.ConnectionString);
                    connection.Open();
                    connection.Close();
                    _log.Write("Test Successful");
                }
                catch (Exception ex)
                {
                    _log.Write("Test Failed");
                    if (_log != null) _log.Write(ex);
                    _returnValue = -1;
                }

                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }
        }

		private void _ProcessProject() 
		{
			ZeusProject proj = this._argmgr.Project;

            this._log.Write("Begin Project Processing: " + proj.Name);
            if (this._argmgr.ModuleNames.Count > 0)
            {
                foreach (string mod in _argmgr.ModuleNames)
                {
                    this._log.Write("Executing: " + mod);
                    try
                    {
                        if (!ExecuteModule(proj, mod))
                        {
                            this._log.Write("Project Folder not found: {0}.", mod);
                        }
                    }
                    catch (Exception ex)
                    {
                        this._log.Write(ex);
                        this._log.Write("Project Folder execution failed for folder: {0}.", mod);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(this._argmgr.ProjectItemToRecord) &&
               !string.IsNullOrEmpty(this._argmgr.PathTemplate))
            {
                string item = _argmgr.ProjectItemToRecord;
                string template = _argmgr.PathTemplate;
                this._log.Write("Collecting: " + item + " for template " + template);
                try
                {
                    if (!CollectProjectItem(proj, item, template))
                    {
                        this._log.Write("Project Item not found: {0}.", item);
                    }
                }
                catch (Exception ex)
                {
                    this._log.Write(ex);
                    this._log.Write("Project Item collection failed for item: {0}.", item);
                }
            }
            else if (this._argmgr.ProjectItems.Count > 0)
            {
                foreach (string item in _argmgr.ProjectItems)
                {
                    this._log.Write("Executing: " + item);
                    try
                    {
                        if (!ExecuteProjectItem(proj, item))
                        {
                            this._log.Write("Project Item not found: {0}.", item);
                        }
                    }
                    catch (Exception ex)
                    {
                        this._log.Write(ex);
                        this._log.Write("Project Item execution failed for item: {0}.", item);
                    }
                }
            }
            else
            {
                this._log.Write("Executing: " + proj.Name);
                try
                {
                    proj.Execute(this._argmgr.Timeout, this._log);

                    if (this._argmgr.InternalUseOnly)
                    {
                        foreach (string file in proj.SavedFiles)
                        {
                            this._log.Write("[GENERATED_FILE]" + file);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._log.Write(ex);
                    this._log.Write("Project execution failed.");
                }
            }
			this._log.Write("End Project Processing: " + proj.Name);
            if (_argmgr.InternalUseOnly) this._log.Write("[PROJECT_GENERATION_COMPLETE]");
		}

		private bool ExecuteModule(ZeusModule parent, string projectPath) 
		{
            bool complete = false;
            ZeusModule m = FindModule(parent, projectPath);
            if (m != null)
            {
                complete = true;
                m.Execute(this._argmgr.Timeout, this._log);

                if (this._argmgr.InternalUseOnly)
                {
                    foreach (string file in m.SavedFiles)
                    {
                        this._log.Write("[GENERATED_FILE]" + file);
                    }
                }
            }
            return complete;
        }

        private bool ExecuteProjectItem(ZeusModule parent, string projectPath)
        {
            bool complete = false;
            int moduleIndex = projectPath.LastIndexOf('/');
            if (moduleIndex >= 0)
            {
                string modulePath = projectPath.Substring(0, moduleIndex),
                    objectName = projectPath.Substring(moduleIndex + 1);

                ZeusModule m = FindModule(parent, modulePath);
                if (m != null)
                {
                    if (m.SavedObjects.Contains(objectName))
                    {
                        m.SavedObjects[objectName].Execute(this._argmgr.Timeout, this._log);

                        if (this._argmgr.InternalUseOnly)
                        {
                            foreach (string file in m.SavedObjects[objectName].SavedFiles)
                            {
                                this._log.Write("[GENERATED_FILE]" + file);
                            }
                        }
                    }
                    complete = true;
                }
            }
            return complete;
        }

        private bool CollectProjectItem(ZeusModule parent, string projectPath, string templatePath)
        {
            bool complete = false;
            int moduleIndex = projectPath.LastIndexOf('/');
            if (moduleIndex >= 0)
            {
                string modulePath = projectPath.Substring(0, moduleIndex),
                    objectName = projectPath.Substring(moduleIndex + 1);

                ZeusModule m = FindModule(parent, modulePath);
                if (m != null)
                {
                    ZeusTemplate template = new ZeusTemplate(templatePath);
                    DefaultSettings settings = DefaultSettings.Instance;

                    SavedTemplateInput savedInput = null;
                    if (m.SavedObjects.Contains(objectName))
                    {
                        savedInput = m.SavedObjects[objectName];
                    }
                    else
                    {
                        savedInput = new SavedTemplateInput();
                        savedInput.SavedObjectName = objectName;
                    }


                    ZeusContext context = new ZeusContext();
                    context.Log = this._log;

                    savedInput.TemplateUniqueID = template.UniqueID;
                    savedInput.TemplatePath = template.FilePath + template.FileName;

                    settings.PopulateZeusContext(context);
                    if (m != null)
                    {
                        m.PopulateZeusContext(context);
                        m.OverrideSavedData(savedInput.InputItems);
                    }

                    if (template.Collect(context, settings.ScriptTimeout, savedInput.InputItems))
                    {
                        //this._lastRecordedSelectedNode = this.SelectedTemplate;
                    }


                    if (this._argmgr.InternalUseOnly)
                    {
                        this._log.Write("[BEGIN_RECORDING]");

                        this._log.Write(savedInput.XML);

                        this._log.Write("[END_RECORDING]");
                    }
                    complete = true;
                }
            }
            return complete;
        }

        private ZeusModule FindModule(ZeusModule parent, string projectPath)
        {
            ZeusModule m = null;
            if (parent.ProjectPath.Equals(projectPath, StringComparison.CurrentCultureIgnoreCase))
            {
                m = parent;
            }
            else
            {
                foreach (ZeusModule module in parent.ChildModules)
                {
                    if (module.ProjectPath.Equals(projectPath, StringComparison.CurrentCultureIgnoreCase))
                    {
                        m = module;
                        break;
                    }
                    else
                    {
                        m = FindModule(module, projectPath);
                        if (m != null) break;
                    }
                }
            }

            return m;
        }

		private void _ProcessTemplate() 
		{
			ZeusTemplate template = this._argmgr.Template;
			ZeusSavedInput savedInput = this._argmgr.SavedInput;
			ZeusSavedInput collectedInput = this._argmgr.InputToSave;
			ZeusContext context = new ZeusContext();
			context.Log = _log;
			DefaultSettings settings;
			
			this._log.Write("Executing: " + template.Title);
			try 
			{
				if (savedInput != null) 
				{
					context.Input.AddItems(savedInput.InputData.InputItems);
					template.Execute(context, this._argmgr.Timeout, true);
				}
				else if (collectedInput != null) 
				{
					settings = DefaultSettings.Instance;
					settings.PopulateZeusContext(context);
					template.ExecuteAndCollect(context, this._argmgr.Timeout, collectedInput.InputData.InputItems);
					collectedInput.Save();
				}
				else 
				{
					settings = DefaultSettings.Instance;
					settings.PopulateZeusContext(context);
					template.Execute(context, this._argmgr.Timeout, false);
				}
				
				if (this._argmgr.PathOutput != null) 
				{
					StreamWriter writer = File.CreateText(this._argmgr.PathOutput);
					writer.Write(context.Output.text);
					writer.Flush();
					writer.Close();
				}
				else 
				{
					if (!_argmgr.IsSilent)
						Console.WriteLine(context.Output.text);
				}
			}
			catch (Exception ex)
			{
				this._log.Write(ex);
				this._log.Write("Template execution failed.");
			}

            if (context != null && this._argmgr.InternalUseOnly)
            {
                foreach (string file in context.Output.SavedFiles)
                {
                    this._log.Write("[GENERATED_FILE]" + file);
                }
            }
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			ZeusCmd cmd = new ZeusCmd(args);
			return cmd.ReturnValue;
		}

		#region Help Text
		public const string HELP_TEXT = @"
|========================================================================
| ZeusCmd.exe: Switches, arguments, etc.
|========================================================================
| General switches
|------------------------------------------------------------------------
| -?, -h                               | show usage text
| -s                                   | silent mode (no console output)
| -r					               | make paths relative to exe
| -l <logpath>                         | log process events and errors
| -e <integer>                         | template execution timeout
|------------------------------------------------------------------------
| Project switches
|------------------------------------------------------------------------
| -p <projectfilepath>                 | generate an entire project
| -pf <projectlocation>                | regenerate a project folder
| -m  <projectlocation>                | same as -pf above for modules
| -ti <projectlocation>                | same as -pf above for templates
|------------------------------------------------------------------------
| Template switches
|------------------------------------------------------------------------
| -i <xmldatapath>                     | xml input file
| -t <templatepath>                    | template file path
| -o <outputpath>                      | output path
| -c <saveinputpath>                   | collect input and save to file
|------------------------------------------------------------------------
| Intrinsic Object switches
|------------------------------------------------------------------------
| -aio <dllpath> <classpath> <varname> | add an intrinsic object
| -rio <varname>                       | remove an intrinsic object
|------------------------------------------------------------------------
| MyMeta switches
|------------------------------------------------------------------------
| -tc <providername> <connectstring>               | test a db connection
| -mumd <file1> <entry1> <file2> <entry2> <result> | merge user meta data
|========================================================================
| EXAMPLE 1
| Execute a template:
|------------------------------------------------------------------------
| ZeusCmd -t c:\template.jgen
|========================================================================
| EXAMPLE 2
| Execute a template, no console output, log to file:
|------------------------------------------------------------------------
| ZeusCmd -s -t c:\template.jgen -l c:\zeuscmd.log
|========================================================================
| EXAMPLE 3
| Execute template, save input to an xml file:
|------------------------------------------------------------------------
| ZeusCmd -t c:\template.jgen -c c:\savedInput.zinp
|========================================================================
| EXAMPLE 4
| Regenerate from the saved input in example 3 above:
|------------------------------------------------------------------------
| ZeusCmd -i c:\savedInput.zinp
|=========================================================================
| EXAMPLE 5
| Add an intrinsic object to the Zeus Configuration file.
|------------------------------------------------------------------------
| ZeusCmd -aio TheDLLOfmyDreams.dll Lib.MyGen.Plugin.Utilities myVar
|=========================================================================
| EXAMPLE 6
| Remove an intrinsic object to the Zeus Configuration file.
|------------------------------------------------------------------------
| ZeusCmd -rio myVar
|=========================================================================
| EXAMPLE 7
| Test a MyMeta Connection
|------------------------------------------------------------------------
| ZeusCmd -tc SQL ""Provider=SQLOLEDB.1;User ID=sa;Data Source=localhost""
|=========================================================================
| EXAMPLE 8
| Merge two UserMetaData files into a new one.
|------------------------------------------------------------------------
| ZeusCmd -mumd umd1.xml Northwind umd2.xml Northwind22 out.xml
|=========================================================================
";
		#endregion
	}
}
