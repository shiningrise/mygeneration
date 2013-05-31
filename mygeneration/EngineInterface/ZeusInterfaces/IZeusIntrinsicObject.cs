using System;
using System.Collections;
using System.Reflection;

namespace Zeus
{
	public interface IZeusIntrinsicObject
    {
        string VariableName { get; }
		string ClassPath { get; }
		string AssemblyPath { get; }
        string DllReference { get; }
        string Namespace { get; }
        bool Disabled { get; }
        Assembly Assembly { get; }
	}
}
