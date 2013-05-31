using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

namespace Zeus
{
	/// <summary>
	/// The ZeusTemplate class is primarily a data structure class that stores the various contents
	/// of a Template. Using the secondary constructor, you can open up a template from a file.
	/// </summary>
	public class ZeusTemplate : IZeusTemplate, IZeusTemplateStub
	{
		protected string _fileType = ZeusConstants.Types.TEMPLATE;
		protected string _outputLanguage = ZeusConstants.Languages.NONE;
		protected string _tagStart = ZeusConstants.DEFAULT_START_TAG;
		protected string _tagEnd = ZeusConstants.DEFAULT_END_TAG;
		protected string _tagStartShortcut = ZeusConstants.DEFAULT_START_TAG + ZeusConstants.DEFAULT_SHORTCUT_CHAR;
		protected string _tagStartSpecial = ZeusConstants.DEFAULT_START_TAG + ZeusConstants.DEFAULT_SPECIAL_CHAR;
		protected string _type = ZeusConstants.Types.TEMPLATE;
		protected string _sourceType = ZeusConstants.SourceTypes.SOURCE;
		protected string _title = string.Empty;
		protected string _uniqueId = string.Empty;
		protected string _comments = string.Empty;
		protected string _filePath = string.Empty;
		protected string _fileName = string.Empty;
		protected string[] _namespacePath = new string[] {};
		protected NameValueCollection _directives = new NameValueCollection();
		protected ArrayList _includedTemplates = null;
		protected ArrayList _includedTemplatePaths = new ArrayList();
		protected ArrayList _requiredInputVariables = new ArrayList();
		protected ZeusCodeSegment _bodySegment = null;
		protected ZeusCodeSegment _guiSegment = null;

		/// <summary>
		/// Creates a new Zeus template.
		/// </summary>
		public ZeusTemplate() {}

		/// <summary>
		/// Opens an existing Zeus template from the specified path.
		/// </summary>
		/// <param name="path">The file path of the template to be opened.</param>
		public ZeusTemplate(string path) 
		{
			ZeusTemplateParser templateParser = new ZeusTemplateParser();
			templateParser.LoadIntoTemplate(path, this);
		}

		public ZeusTemplate(Stream stream, string path) 
		{
			ZeusTemplateParser templateParser = new ZeusTemplateParser();
			templateParser.LoadIntoTemplate(stream, path, this);
		}

		public ZeusTemplate(StreamReader reader, string path) 
		{
			ZeusTemplateParser templateParser = new ZeusTemplateParser();
			templateParser.LoadIntoTemplate(reader, path, this);
		}

		/// <summary>
		/// This ZeusCodeSegment object contains all the information necessary for the template body code
		/// segment including the engine, language, mode, and the code itself.
		/// </summary>
		public IZeusCodeSegment BodySegment 
		{
			get 
			{
				if (_bodySegment == null) 
				{
					_bodySegment = new ZeusCodeSegment(this, ZeusConstants.CodeSegmentTypes.BODY_SEGMENT);
					_bodySegment.Mode = ZeusConstants.Modes.MARKUP;
				}
				return _bodySegment;
			}
		}

		/// <summary>
		/// This ZeusCodeSegment object contains all the information necessary for the interface code segment
		/// including the engine, language, mode, and the code itself. 
		/// </summary>
		public IZeusCodeSegment GuiSegment 
		{
			get 
			{
				if (_guiSegment == null) 
				{
					_guiSegment = new ZeusCodeSegment(this, ZeusConstants.CodeSegmentTypes.GUI_SEGMENT);
					_guiSegment.Mode = ZeusConstants.Modes.PURE;
				}
				return _guiSegment;
			}
		}

		/// <summary>
		/// This ZeusCodeSegmentStub object contains all the information necessary for the template body code
		/// segment including the engine, language, mode, and the code itself.
		/// </summary>
		public IZeusCodeSegmentStub BodySegmentStub
		{
			get { return BodySegment as IZeusCodeSegmentStub; }
		}

		/// <summary>
		/// This ZeusCodeSegmentStub object contains all the information necessary for the interface code segment
		/// including the engine, language, mode, and the code itself. 
		/// </summary>
		public IZeusCodeSegmentStub GuiSegmentStub
		{
			get { return GuiSegment as IZeusCodeSegmentStub; }
		}

		/// <summary>
		/// Returns true if there are any comments in the template.
		/// </summary>
		public bool HasCommentBlock 
		{
			get { return (this._comments != string.Empty); }
		}

		/// <summary>
		/// Returns a Name-Value collection of all of the directives
		/// defined in this template. Custom directives included.
		/// </summary>
		public NameValueCollection Directives
		{
			get { return this._directives; }
		}
		
		/// <summary>
		/// Variables required by the Template body to run. If these all exist in the
		/// input object, there will be no need to run the interface code.
		/// </summary>
		public ArrayList RequiredInputVariables
		{
			get { return this._requiredInputVariables; }
		}
		
		/// <summary>
		/// Variables required by the Template body to run. If these all exist in the
		/// input object, there will be no need to run the interface code. (This is split by 
		/// spaces to populate RequiredInputVariables)
		/// </summary>
		public string RequiredInputVariablesString
		{
			get 
			{ 
				string tmpStr = "";
				foreach (string var in _requiredInputVariables) 
				{
					if (tmpStr != string.Empty) tmpStr += " ";
					tmpStr += var;
			}
				return tmpStr; 
			}
			set 
			{ 
				this._requiredInputVariables.AddRange( value.Split(' ') ); 
			}
		}

		/// <summary>
		/// A collection of included template paths for TemplateGroup type templates.
		/// </summary>
		public ArrayList IncludedTemplatePaths
		{
			get { return this._includedTemplatePaths; }
		}
		
		/// <summary>
		/// A collection of included templates for TemplateGroup type templates.
		/// </summary>
		public ArrayList IncludedTemplates
		{
			get 
			{  
				if (this._includedTemplates == null) 
					this._includedTemplates = new ArrayList();
				else 
					this._includedTemplates.Clear();

				ZeusTemplateParser templateParser = new ZeusTemplateParser();

				foreach (string path in this._includedTemplatePaths) 
				{
					this._includedTemplates.Add( templateParser.LoadTemplate( FileTools.MakeAbsolute(path, this.FilePath) ) );
				}

				return this._includedTemplates;
			}
		}

		/// <summary>
		/// The NamespacePath broken down into a string array.
		/// </summary>
		public string[] NamespacePath
		{
			get { return this._namespacePath; }
			set { this._namespacePath = value; }
		}

		/// <summary>
		/// A string representation of the template Namespace Hierarchy. (split by periods 
		/// to fill the NamespacePath) This hierarchy will be used in the template browser 
		/// to categorize the templates.
		/// </summary>
		public string NamespacePathString
		{
			get { return string.Join(".", this._namespacePath); }
			set { this._namespacePath = value.Split('.'); }
		}

		/// <summary>
		/// The filename of the template. (without a path)
		/// </summary>
		public string FileName
		{
			get { return this._fileName; }
			set { this._fileName = value; }
		}

		/// <summary>
		/// The directory path where this template lives.
		/// </summary>
		public string FilePath
		{
			get { return this._filePath; }
			set { this._filePath = value; }
		}

        /// <summary>
        /// 
        /// </summary>
        public string FullFileName
        {
            get
            {
                return Path.Combine(this.FilePath, this.FileName);
            }
        }

		/// <summary>
		/// The tempate type. Currently this can only be TemplateGroup or Template.
		/// </summary>
		public string Type 
		{
			get { return this._type; }
			set { this._type = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public string SourceType 
		{ 
			get { return this._sourceType; } 
			set 
            {
                if (value != _sourceType)
                {
                    if ((value == ZeusConstants.SourceTypes.COMPILED) ||
                        (value == ZeusConstants.SourceTypes.ENCRYPTED) ||
                        (value == ZeusConstants.SourceTypes.SOURCE))
                    {
                        this._sourceType = value;
                    }
                    else
                    {
                        this._sourceType = ZeusConstants.SourceTypes.SOURCE;
                    }
                }
            } 
		}

		/// <summary>
		/// A Guid that identifies this template globally. This will be used when
		/// we implement WebUpdates.
		/// </summary>
		public string UniqueID
		{
			get { return this._uniqueId; }
			set { this._uniqueId = value; }
		}

		/// <summary>
		/// The title of the template
		/// </summary>
		public string Title
		{
			get { return this._title; }
			set { this._title = value; }
		}

		/// <summary>
		/// General comments section that can be used to describe the template, 
		/// it's purpose, author information, etc.
		/// </summary>
		public string Comments
		{
			get { return this._comments; }
			set { this._comments = value; }
		}

		/// <summary>
		/// The start tag to be used in Markup mode in Zeus templates. The TagStartShortcut
		/// and TagStartSpecial properties are generated off of this.
		/// </summary>
		public string TagStart
		{
			get 
			{ 
				return this._tagStart; 
			}
			set 
			{ 
				this._tagStart = value; 
				this._tagStartShortcut = this._tagStart + ZeusConstants.DEFAULT_SHORTCUT_CHAR;
				this._tagStartSpecial = this._tagStart + ZeusConstants.DEFAULT_SPECIAL_CHAR;
			}
		}

		/// <summary>
		/// Returns the print shortcut start tag (the start tag plus an equals sign).
		/// </summary>
		public string TagStartShortcut
		{
			get { return this._tagStartShortcut; }
		}

		/// <summary>
		/// Returns the Special call start tag (the start tag plus a pound sign).
		/// </summary>
		public string TagStartSpecial
		{
			get { return this._tagStartSpecial; }
		}

		/// <summary>
		/// The end tag to be used in Markup mode in Zeus templates.
		/// </summary>
		public string TagEnd
		{
			get { return this._tagEnd; }
			set { this._tagEnd = value; }
		}

		/// <summary>
		/// The output language for the template.
		/// </summary>
		public string OutputLanguage
		{
			get { return this._outputLanguage; }
			set { this._outputLanguage = value; }
		}

		/// <summary>
		/// Checks to see if a mode is valid.
		/// </summary>
		/// <param name="mode">A template Mode to validate.</param>
		/// <returns>True if this is a valid mode.</returns>
		public bool IsValidMode(string mode) 
		{
			bool isValid = false;
			switch (mode) 
			{
				case ZeusConstants.Modes.MARKUP:
				case ZeusConstants.Modes.PURE:
					isValid = true;
					break;
			}
			return isValid;
		}

		/// <summary>
		//// Checks to see if a template Type is valid.
		/// </summary>
		/// <param name="type">A template Type to validate.</param>
		/// <returns>True if this is a valid type.</returns>
		public bool IsValidTemplateType(string type) 
		{
			bool isValid = false;
			switch (type) 
			{
				case ZeusConstants.Types.TEMPLATE:
				case ZeusConstants.Types.GROUP:
					isValid = true;
					break;
			}
			return isValid;
		}

		/// <summary>
		/// Adds a directive to the directives NameValue collection.
		/// </summary>
		/// <param name="name">The directive</param>
		/// <param name="val">The data</param>
		public void AddDirective(string name, string val)
		{
			this._directives.Add(name, val);
		}

		/// <summary>
		/// Adds a child tempalte to this template. This is used for the TemplateGroup type
		/// template.
		/// </summary>
		/// <param name="templatePath"></param>
		public void AddIncludedTemplatePath(string templatePath) 
		{
			this._includedTemplatePaths.Add(FileTools.MakeRelative(templatePath, this.FilePath));
		}

		public IZeusTemplate LoadTemplateFromFile(string path) 
		{
			ZeusTemplate template = new ZeusTemplate(path);
			ZeusTemplateParser templateParser = new ZeusTemplateParser();
			templateParser.LoadIntoTemplate(path, template);
			return template;
		}

		public void Encrypt() 
		{
			if (this.SourceType == ZeusConstants.SourceTypes.SOURCE) 
			{
				this.SourceType = ZeusConstants.SourceTypes.ENCRYPTED;

				if (_bodySegment.CodeUnparsed.Length > 0)
					this._bodySegment.CodeUnparsed = ZeusEncryption.Encrypt(_bodySegment.CodeUnparsed);
			
				if (_guiSegment.CodeUnparsed.Length > 0)
					this._guiSegment.CodeUnparsed = ZeusEncryption.Encrypt(_guiSegment.CodeUnparsed);
			}
		}
		
		public void Compile() 
		{
			if ((this.SourceType == ZeusConstants.SourceTypes.SOURCE) && 
				(this.BodySegment.Engine == ZeusConstants.Engines.DOT_NET_SCRIPT) && 
				(this.GuiSegment.Engine == ZeusConstants.Engines.DOT_NET_SCRIPT))
			{
				this.SourceType = ZeusConstants.SourceTypes.COMPILED;
			}
		}

		public void Save() 
		{
			ZeusWriter writer = new ZeusWriter();
			writer.Write(this.FilePath + this.FileName, this);
		}

		public void Save(string filename) 
		{
			ZeusWriter writer = new ZeusWriter();
			writer.Write(filename, this);
		}

		//public IZeusContext Execute(IZeusContext context, int timeout, ILog log, bool skipGui) 
		public IZeusContext Execute(IZeusContext context, int timeout, bool skipGui) 
		{
			ZeusExecutioner exec = new ZeusExecutioner(context.Log);
			return exec.Execute(this, context, timeout, null, null, skipGui);
		}

		//public IZeusContext ExecuteAndCollect(IZeusContext context, int timeout, InputItemCollection inputitems, ILog log) 
		public IZeusContext ExecuteAndCollect(IZeusContext context, int timeout, InputItemCollection inputitems) 
		{
			ZeusExecutioner exec = new ZeusExecutioner(context.Log);
			return exec.ExecuteAndCollect(this, context, timeout, inputitems);
		}

		//public bool Collect(IZeusContext context, int timeout, InputItemCollection inputitems, ILog log) 
		public bool Collect(IZeusContext context, int timeout, InputItemCollection inputitems) 
		{
			ZeusExecutioner exec = new ZeusExecutioner(context.Log);
			return exec.Collect(this, context, timeout, inputitems);
		}
	}
}
