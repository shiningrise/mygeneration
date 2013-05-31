
using System;
using System.Collections;

namespace Zeus
{
	public interface IZeusGuiControl
	{
		bool IsCanceled { get; set; }
		bool ShowGui { get; set; }
		Hashtable Defaults { get; }
		bool ForceDisplay { get; set; }
	}
}
