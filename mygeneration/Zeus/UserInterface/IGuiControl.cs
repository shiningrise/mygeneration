using System;

namespace Zeus.UserInterface
{
	/// <summary>
	/// An interface containing all of the common GuiControl methods and properties 
	/// </summary>
	public interface IGuiControl
	{
		string ForeColor { get; set; }
		string BackColor { get; set; }
		bool Visible { get; set; }
		bool Enabled { get; set; }
		int Width { get; set; }
		int Height { get; set; }
		void AttachEvent(string eventType, string functionName);
		bool HasEventHandlers(string eventType);
		string[] GetEventHandlers(string eventType);

	}
}
