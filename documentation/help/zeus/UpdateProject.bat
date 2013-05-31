REM currently in \documentation\help\zeus

MKDIR ZeusClassReference
MKDIR ZeusClassReference\Data
MKDIR ZeusClassReference\UserInterface
MKDIR ZeusClassReference\UserInterface\Interfaces

COPY ..\..\..\mygeneration\Zeus\ZeusInput.cs .\ZeusClassReference\ /y
COPY ..\..\..\mygeneration\Zeus\ZeusOutput.cs .\ZeusClassReference\ /y
 
COPY ..\..\..\mygeneration\Zeus\Data\SimpleColumn.cs ZeusClassReference\Data\ /y
COPY ..\..\..\mygeneration\Zeus\Data\SimpleColumnCollection.cs ZeusClassReference\Data\ /y
COPY ..\..\..\mygeneration\Zeus\Data\SimpleRow.cs ZeusClassReference\Data\ /y
COPY ..\..\..\mygeneration\Zeus\Data\SimpleRowCollection.cs ZeusClassReference\Data\ /y
COPY ..\..\..\mygeneration\Zeus\Data\SimpleTable.cs ZeusClassReference\Data\ /y
 
COPY ..\..\..\mygeneration\Zeus\UserInterface\GuiButton.cs ZeusClassReference\UserInterface\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\GuiCheckBox.cs ZeusClassReference\UserInterface\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\GuiComboBox.cs ZeusClassReference\UserInterface\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\GuiControl.cs ZeusClassReference\UserInterface\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\GuiController.cs ZeusClassReference\UserInterface\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\GuiDataBinder.cs ZeusClassReference\UserInterface\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\GuiFilePicker.cs ZeusClassReference\UserInterface\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\GuiGrid.cs ZeusClassReference\UserInterface\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\GuiLabel.cs ZeusClassReference\UserInterface\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\GuiListBox.cs ZeusClassReference\UserInterface\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\GuiTextBox.cs ZeusClassReference\UserInterface\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\GuiCheckBoxList.cs ZeusClassReference\UserInterface\ /y
 
COPY ..\..\..\mygeneration\Zeus\UserInterface\Interfaces\IGuiButton.cs ZeusClassReference\UserInterface\Interfaces\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\Interfaces\IGuiCheckBox.cs ZeusClassReference\UserInterface\Interfaces\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\Interfaces\IGuiComboBox.cs ZeusClassReference\UserInterface\Interfaces\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\Interfaces\IGuiControl.cs ZeusClassReference\UserInterface\Interfaces\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\Interfaces\IGuiController.cs ZeusClassReference\UserInterface\Interfaces\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\Interfaces\IGuiFilePicker.cs ZeusClassReference\UserInterface\Interfaces\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\Interfaces\IGuiGrid.cs ZeusClassReference\UserInterface\Interfaces\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\Interfaces\IGuiLabel.cs ZeusClassReference\UserInterface\Interfaces\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\Interfaces\IGuiListBox.cs ZeusClassReference\UserInterface\Interfaces\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\Interfaces\IGuiListControl.cs ZeusClassReference\UserInterface\Interfaces\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\Interfaces\IGuiTextBox.cs ZeusClassReference\UserInterface\Interfaces\ /y
COPY ..\..\..\mygeneration\Zeus\UserInterface\Interfaces\IGuiCheckBoxList.cs ZeusClassReference\UserInterface\Interfaces\ /y
