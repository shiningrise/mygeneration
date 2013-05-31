using System;
using System.Collections;
using System.Reflection;
using System.IO;

using Zeus.UserInterface;

namespace Zeus
{
	/// <summary>
	/// Summary description for ZeusCodeBlock.
	/// </summary>
	public class ZeusCodeSegment : IZeusCodeSegment, IZeusCodeSegmentStub
	{
		protected const string CACHED_ASSEMBLY_PREFIX = "BASE64:";
		protected string _engine = ZeusConstants.Engines.DOT_NET_SCRIPT;
		protected string _language = ZeusConstants.Languages.CSHARP;
		protected string _mode = ZeusConstants.Modes.MARKUP;
		protected string _codeUnparsed = string.Empty;
		protected string _code = string.Empty;
		protected ArrayList _extraData = new ArrayList();
		protected ZeusTemplate _template = null;
		protected string _segmentType = ZeusConstants.CodeSegmentTypes.BODY_SEGMENT;
		protected Assembly _cachedAssembly = null;

		protected IZeusScriptingEngine _scriptingEngine = null;

		internal ZeusCodeSegment(ZeusTemplate template, string segmentType)
		{
			this._template = template;
			this._segmentType = segmentType;
		}

		public bool IsEmpty 
		{
			get { return (this._codeUnparsed == string.Empty); }
		}

		public string SegmentType 
		{
			get { return this._segmentType; }
		}

		public ZeusTemplate Template 
		{
			get { return this._template; }
		}

		public IZeusTemplate ITemplate 
		{
			get { return Template; }
		}

		public string Engine 
		{ 
			get { return this._engine; } 
			set 
			{ 
				this._engine = value; 
				this._scriptingEngine = null;
			} 
		}

		public string Language 
		{ 
			get { return this._language; } 
			set { this._language = value; } 
		}

		public string Mode 
		{ 
			get { return this._mode; } 
			set { this._mode = value; } 
		}

		public ArrayList ExtraData 
		{ 
			get { return this._extraData; } 
			set { this._extraData = value; } 
		}

		public string CodeUnparsed
		{
			get 
			{
				if (!this._codeUnparsed.StartsWith(CACHED_ASSEMBLY_PREFIX)) 
				{
					if (this.CachedAssembly != null) 
					{
						FileInfo inf = new FileInfo(this._cachedAssembly.Location);
						if (inf.Exists) 
						{
							FileStream stream = File.OpenRead(this._cachedAssembly.Location);
							BinaryReader reader = new BinaryReader(stream);
							_codeUnparsed = CACHED_ASSEMBLY_PREFIX + Convert.ToBase64String(reader.ReadBytes((int)inf.Length));
							_code = string.Empty;
							reader.Close();
							reader = null;stream = null;
						}
					}
				}
				return this._codeUnparsed; 
			}
			set
			{
				this._codeUnparsed = value;
				try 
				{
					ZeusCodeParser.ParseCode(this);
				} 
				catch 
				{
					this._code = string.Empty;
				}
			}
		}

		public string Code 
		{ 
			get 
			{ 
				if (this._code == string.Empty) 
				{
					ZeusCodeParser.ParseCode(this);
				}
				return this._code; 
			} 
			set { this._code = value; } 
		}

		public IZeusScriptingEngine ZeusScriptingEngine
		{
			get 
			{
				if (_scriptingEngine == null) 
				{
					_scriptingEngine = ZeusFactory.GetEngine(_engine);
				}

				return _scriptingEngine;
			}
		}

		public Assembly CachedAssembly 
		{
			get 
			{
				if (_cachedAssembly == null) 
				{
					if (this.Template.SourceType == ZeusConstants.SourceTypes.COMPILED) 
					{
						if (this._codeUnparsed.StartsWith(CACHED_ASSEMBLY_PREFIX)) 
						{
							byte[] assemblyBytes = Convert.FromBase64String(this._codeUnparsed.Substring(7));
							_cachedAssembly = Assembly.Load(assemblyBytes);
						}
					}
				}

				return this._cachedAssembly; 
			}
			set 
			{ 
				this._cachedAssembly = value; 
			}
		}

		public bool Execute(IZeusContext context) 
		{
			return ZeusExecutioner.ExecuteCodeSegment(this, context);
		}
	}
}
