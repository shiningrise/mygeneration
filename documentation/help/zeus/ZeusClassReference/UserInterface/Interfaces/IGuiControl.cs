using System;

namespace Zeus.UserInterface
{
	public interface IGuiControl
	{
		string ID { get; set; }
		string ToolTip { get; set; }
		string ForeColor { get; set; }
		string BackColor { get; set; }
		bool Visible { get; set; }
		bool Enabled { get; set; }
		int Width { get; set; }
		int Height { get; set; }
		int Top { get; set; }
		int Left { get; set; }
		object Value { get; }
		void AttachEvent(string eventType, string functionName);
		bool HasEventHandlers(string eventType);
		string[] GetEventHandlers(string eventType);

	}
}
