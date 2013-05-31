using System;
using System.Collections;

namespace Zeus
{
	public interface IZeusCodeSegmentStub
	{
		bool IsEmpty { get; }
		string SegmentType { get; }
		string Engine { get; }
		string Language { get; }
		string Mode { get; }
		string CodeUnparsed { get; }
		string Code { get; }
	}
}
