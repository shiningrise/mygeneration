using System;
using System.Collections;

namespace Zeus.UserInterface
{
	public interface IGuiController : IGuiControl, IZeusGuiControl, IEnumerable
	{
		string Title { get; set; }
		string StartPosition { get; set; }
		GuiControl this[string id] { get; }
		GuiLabel AddLabel(string id, string text, string tooltip); 
		GuiCheckBox AddCheckBox(string id, string text, bool isChecked, string tooltip);
		GuiButton AddButton( string id, string text, string tooltip);
		GuiFilePicker AddFilePicker(string id, string text, string tooltip, string targetcontrol, bool picksFolder);
		bool DoesOkButtonExist { get; }
		void AddOkButtonIfNonExistant();
		GuiButton AddOkButton( string id, string text, string tooltip);
		GuiTextBox AddTextBox(string id, string text, string tooltip);
		GuiComboBox AddComboBox(string id, string tooltip);
		GuiListBox AddListBox(string id, string tooltip);
		GuiCheckBoxList AddCheckBoxList(string id, string tooltip);
		GuiGrid AddGrid(string id, string tooltip);
		void AddTab(string tabText);

        GuiComboBox AddDatabaseSelector(string id, string tooltip, string parentId);
        GuiComboBox AddTableSelector(string id, string tooltip, string parentId);
        GuiComboBox AddViewSelector(string id, string tooltip, string parentId);
        GuiComboBox AddProcedureSelector(string id, string tooltip, string parentId);

        GuiListBox AddDatabaseMultiSelector(string id, string tooltip, string parentId);
        GuiListBox AddTableMultiSelector(string id, string tooltip, string parentId);
        GuiListBox AddViewMultiSelector(string id, string tooltip, string parentId);
        GuiListBox AddProcedureMultiSelector(string id, string tooltip, string parentId);
	}
}