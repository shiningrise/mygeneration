using System;

namespace Zeus
{
	public interface ITypeSerializer
	{
		Type DataType { get; }
		Type GetTypeFromName(string typeName);
		string Dissolve(object source);
		object Reconstitute(string source);
	}
}
