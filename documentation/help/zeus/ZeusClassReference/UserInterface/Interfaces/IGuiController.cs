using System;
using System.Collections;

namespace Zeus.UserInterface
{
	public interface IGuiController : IGuiControl, IZeusGuiControl, IEnumerable
	{		string Title { get; set; }		string StartPosition { get; set; }		GuiControl this[string id] { get; }		GuiLabel AddLabel(string id, string text, string tooltip); 		GuiCheckBox AddCheckBox(string id, string text, bool isChecked, string tooltip);		GuiButton AddButton( string id, string text, string tooltip);		GuiFilePicker AddFilePicker(string id, string text, string tooltip, string targetcontrol, bool picksFolder);		bool DoesOkButtonExist { get; }		void AddOkButtonIfNonExistant();		GuiButton AddOkButton( string id, string text, string tooltip);		GuiTextBox AddTextBox(string id, string text, string tooltip);		GuiComboBox AddComboBox(string id, string tooltip);		GuiListBox AddListBox(string id, string tooltip);
		GuiCheckBoxList AddCheckBoxList(string id, string tooltip);
		GuiGrid AddGrid(string id, string tooltip);		void AddTab(string tabText);
	}
}