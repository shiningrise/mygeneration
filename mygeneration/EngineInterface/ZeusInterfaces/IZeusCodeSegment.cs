using System;
using System.Collections;
using System.Reflection;

namespace Zeus
{
	public interface IZeusCodeSegment
	{
		IZeusTemplate ITemplate { get; }
		IZeusScriptingEngine ZeusScriptingEngine { get; }
		bool IsEmpty { get; }
		string SegmentType { get; }
		string Engine { get; set; }
		string Language { get; set; }
		string Mode { get; set; }
		ArrayList ExtraData { get; set; }
		string CodeUnparsed { get; set; }
		string Code { get; set; }
		Assembly CachedAssembly { get; set; }
		bool Execute(IZeusContext context);
	}
}
