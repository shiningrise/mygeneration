using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

using MyGeneration.CodeSmithConversion.Template;
using MyGeneration.CodeSmithConversion.Conversion.SimpleMapping;

namespace MyGeneration.CodeSmithConversion.Conversion
{
	/// <summary>
	/// Summary description for VBNetHelper.
	/// </summary>
	public class VBNetHelper : LanguageHelper
	{
		internal VBNetHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		protected override string PropertyText { get { return PROPERTY; } }
		protected override string BodySegmentText { get { return MYGEN_TEMPLATE_BODY; } }
		protected override string GuiSegmentText { get { return MYGEN_TEMPLATE_GUI; } }
		protected override string StringAppendChar { get { return APPEND_CHAR; } }

		protected override void ApplyMaps(CstToken token)
		{
			SimpleMapCollection c = SimpleMap.VBNetMaps;
			foreach (SimpleMap map in c) 
			{
				if (map.IsRegExp) 
				{
					Regex regex = new Regex(map.Source);
					token.Text = regex.Replace(token.Text, map.Target);
				}
				else 
				{
					token.Text = token.Text.Replace(map.Source, map.Target);
				}
			}
		}


		protected override string BuildGuiCodeForProperties(CstTemplate template)
		{
			StringBuilder code = new StringBuilder();
			StringBuilder tailCode = new StringBuilder();

			string label = @"Dim lbl{0} As GuiLabel = ui.AddLabel(""lbl_{0}"", ""{1}"", ""{2}"")";
			string textbox = @"Dim txt{0} As GuiTextBox = ui.AddTextBox(""{0}"", ""{1}"", ""{2}"")";
			string combobox = @"Dim cbo{0} As GuiComboBox = ui.AddComboBox(""{0}"", ""{1}"")";
			string checkbox = @"Dim chk{0} As GuiCheckBox = ui.AddCheckBox(""{0}"", ""{1}"", {2}, ""{3}"")";
			string setupcontrol = @"cbo{0}_Setup(cbo{0})";
			string attachevent = @"cbo{0}.AttachEvent(""onchange"", ""cbo{0}_OnChange"")";
			
			foreach (CstProperty prop in template.Properties)
			{
				switch (prop.Type) 
				{
					case "System.String":
					case "System.Guid":
					case "System.Int32":
					case "System.Decimal":
						code.Append("\t\t\t");
						code.AppendFormat(label, prop.Name, prop.Name, prop.Description);
						code.Append(Environment.NewLine);
						code.Append("\t\t\t");
						code.AppendFormat(textbox, prop.Name, prop.DefaultValue, prop.Description);
						code.Append(Environment.NewLine);
						code.Append(Environment.NewLine);
						break;
					case "System.Boolean":
						code.Append("\t\t\t");
						code.AppendFormat(checkbox, prop.Name, prop.Name, prop.DefaultValue.ToLower(), prop.Description);
						code.Append(Environment.NewLine);
						code.Append(Environment.NewLine);
						break;
					case "SchemaExplorer.DatabaseSchema":
					case "SchemaExplorer.TableSchema":
					case "SchemaExplorer.CommandSchema":
					case "SchemaExplorer.ViewSchema":
						code.Append("\t\t\t");
						code.AppendFormat(label, prop.Name, prop.Name, prop.Description);
						code.Append(Environment.NewLine);
						code.Append("\t\t\t");
						code.AppendFormat(combobox, prop.Name, prop.Description);
						code.Append(Environment.NewLine);
						code.Append(Environment.NewLine);

						if (prop.Type == "SchemaExplorer.DatabaseSchema")
						{
							tailCode.Append(Environment.NewLine);
							tailCode.Append("\t\t\t");
							tailCode.AppendFormat(setupcontrol, prop.Name);
							tailCode.Append(Environment.NewLine);

							tailCode.Append("\t\t\t");
							tailCode.AppendFormat(attachevent, prop.Name);
							tailCode.Append(Environment.NewLine);
						}

						break;
					default:
						code.Append("\t\t\t' --- Unsupported DataType: [" + prop.Type + "] ---");
						code.Append(Environment.NewLine);
						code.Append("\t\t\t'");
						code.AppendFormat(label, prop.Name, prop.Name, prop.Description);
						code.Append(Environment.NewLine);
						code.Append("\t\t\t'");
						code.AppendFormat(textbox, prop.Name, prop.DefaultValue, prop.Description);
						code.Append(Environment.NewLine);
						code.Append(Environment.NewLine);
						break;
				}
			}
			code.Append(tailCode.ToString());

			return code.ToString();
		}

		protected override string BuildBindingFunctions(CstTemplate template) 
		{
			StringBuilder code = new StringBuilder();
			StringBuilder entity = new StringBuilder();
			StringBuilder entitySetup = new StringBuilder();
			CstProperty dbProp = null;
			
			string setupDatabase =
				@"	Public Sub cbo§NAME§_Setup(cbo§NAME§ As GuiComboBox)
		If MyMeta.IsConnected Then
			cbo§NAME§.BindData(MyMeta.Databases)
			If Not MyMeta.DefaultDatabase Is Nothing Then
				cbo§NAME§.SelectedValue = MyMeta.DefaultDatabase.Name

				§EXTRA§
			End If
		End If
	End Sub";

			string setupEntity =
				@"	Public Sub cbo§NAME§_Bind(databaseName As string)
		Dim cbo§NAME§ As GuiComboBox = CType(ui(""§NAME§""), GuiComboBox)
		
		Dim database As IDatabase = MyMeta.Databases(databaseName)
		cbo§NAME§.BindData(database.§MYMETATYPE§)
	End Sub";


			string databaseEventHandler = 
				@"	Public Sub cbo§NAME§_OnChange(control As GuiComboBox)
		Dim cbo§NAME§ As GuiComboBox = CType(ui(""§NAME§""), GuiComboBox)
		
		§EXTRA§
	End Sub";
			string bindFunction = @"cbo{0}_Bind(cbo{1}.SelectedValue);";

			
			foreach (CstProperty prop in template.Properties)
			{
				if (prop.Type == "SchemaExplorer.DatabaseSchema")
				{
					dbProp = prop;
					break;
				}
			}

			if (dbProp != null) 
			{
				foreach (CstProperty prop in template.Properties)
				{
					string myMetaName = string.Empty;

					if (prop.Type == "SchemaExplorer.ViewSchema")
					{
						myMetaName = "Views";
					}
					else if (prop.Type == "SchemaExplorer.TableSchema")
					{
						myMetaName = "Tables";
					}
					else if (prop.Type == "SchemaExplorer.CommandSchema")
					{
						myMetaName = "Procedures";
					}

					if (myMetaName != string.Empty) 
					{
						entitySetup.Append(setupEntity.Replace("§NAME§", prop.Name).Replace("§MYMETATYPE§", myMetaName));
						entitySetup.Append(Environment.NewLine);
						entitySetup.Append(Environment.NewLine);

						entity.AppendFormat(bindFunction, prop.Name, dbProp.Name);
						entity.Append(Environment.NewLine);
					}
				}
			
				code.Append(setupDatabase.Replace("§NAME§", dbProp.Name).Replace("§EXTRA§", entity.ToString()));
				code.Append(Environment.NewLine);
				code.Append(Environment.NewLine);
				code.Append(databaseEventHandler.Replace("§NAME§", dbProp.Name).Replace("§EXTRA§", entity.ToString()));
				code.Append(Environment.NewLine);
				code.Append(Environment.NewLine);
				code.Append(entitySetup.ToString());
			}

			return code.ToString();
		}

		private const string PROPERTY = 
			@"
	Public Property §NAME§() As §TYPE§
		Get
			Return CType(input(""§NAME§""), §TYPE§)
        End Get
		Set
			input(""§NAME§"") = Value
		End Set
	End Property
";

		private const string MYGEN_TEMPLATE_BODY = 
			@"§DIRECTIVES§<%
Public Class GeneratedTemplate
			Inherits DotNetScriptTemplate

	Public Sub New(context As ZeusContext)
		MyBase.New(context)
	End Sub

	'---------------------------------------------------
	' Render() is where you want to write your logic    
	'---------------------------------------------------
	Public Overrides Sub Render
%>§CODE§<%
	End Sub

§RUNAT_TEMPLATE_CODE§

§INPUT_PROPERTIES§

End Class
%>";

		private const string APPEND_CHAR = "&";
		private const string MYGEN_TEMPLATE_GUI = 
			@"
Public Class GeneratedGui
			Inherits DotNetScriptGui

	Public Sub New(context As ZeusContext)
		MyBase.New(context)
	End Sub

	Public Overrides Sub Setup
		ui.Title = ""§TITLE§""
§SETUPCODE§

		ui.ShowGui = true
	End Sub

§MEMBERCODE§

End Class";
	}
}
