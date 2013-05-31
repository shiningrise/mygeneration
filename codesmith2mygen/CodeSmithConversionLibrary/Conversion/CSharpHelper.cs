using System;
using System.Text;
using System.Text.RegularExpressions;
using MyGeneration.CodeSmithConversion.Template;
using MyGeneration.CodeSmithConversion.Conversion.SimpleMapping;

namespace MyGeneration.CodeSmithConversion.Conversion
{
	/// <summary>
	/// Summary description for CSharpHelper.
	/// </summary>
	public class CSharpHelper : LanguageHelper
	{
		internal CSharpHelper() {}

		protected override string PropertyText { get { return PROPERTY; } }
		protected override string BodySegmentText { get { return MYGEN_TEMPLATE_BODY; } }
		protected override string GuiSegmentText { get { return MYGEN_TEMPLATE_GUI; } }
		protected override string StringAppendChar { get { return APPEND_CHAR; } }

		protected override void ApplyMaps(CstToken token)
		{
			SimpleMapCollection c = SimpleMap.CSharpMaps;
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
			string label = @"GuiLabel lbl{0} = ui.AddLabel(""lbl_{0}"", ""{1}"", ""{2}"");";
			string textbox = @"GuiTextBox txt{0} = ui.AddTextBox(""{0}"", ""{1}"", ""{2}"");";
			string combobox = @"GuiComboBox cbo{0} = ui.AddComboBox(""{0}"", ""{1}"");";
			string checkbox = @"GuiCheckBox chk{0} = ui.AddCheckBox(""{0}"", ""{1}"", {2}, ""{3}"");";
			string setupcontrol = @"cbo{0}_Setup(cbo{0});";
			string attachevent = @"cbo{0}.AttachEvent(""onchange"", ""cbo{0}_OnChange"");";
			
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
						code.Append("\t\t\t/* --- Unsupported DataType: [" + prop.Type + "] ---");
						code.Append(Environment.NewLine);
						code.Append("\t\t\t");
						code.AppendFormat(label, prop.Name, prop.Name, prop.Description);
						code.Append(Environment.NewLine);
						code.Append("\t\t\t");
						code.AppendFormat(textbox, prop.Name, prop.DefaultValue, prop.Description);
						code.Append(Environment.NewLine);
						code.Append("\t\t\t*/");
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
@"	public void cbo§NAME§_Setup(GuiComboBox cbo§NAME§)
	{
		if (MyMeta.IsConnected) 
		{
			cbo§NAME§.BindData(MyMeta.Databases);
			if (MyMeta.DefaultDatabase != null) 
			{
				cbo§NAME§.SelectedValue = MyMeta.DefaultDatabase.Name;
				
				§EXTRA§
			}
		}
	}";

			string setupEntity =
@"	public void cbo§NAME§_Bind(string databaseName)
	{
		GuiComboBox cbo§NAME§ = ui[""§NAME§""] as GuiComboBox;
		
		IDatabase database = MyMeta.Databases[databaseName];
		cbo§NAME§.BindData(database.§MYMETATYPE§);
	}";


			string databaseEventHandler = 
@"	public void cbo§NAME§_OnChange(GuiComboBox control)
	{
		GuiComboBox cbo§NAME§ = ui[""§NAME§""] as GuiComboBox;
		
		§EXTRA§
	}";
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
	public §TYPE§ §NAME§
	{
		get { return (§TYPE§)input[""§NAME§""]; }
		set { input[""§NAME§""] = value; }
	}
";

		private const string MYGEN_TEMPLATE_BODY = 
			@"§DIRECTIVES§<%
public class GeneratedTemplate : DotNetScriptTemplate
{
	public GeneratedTemplate(ZeusContext context) : base(context) {}

	public override void Render()
	{
%>§CODE§<%
	}

	§RUNAT_TEMPLATE_CODE§

	§INPUT_PROPERTIES§
}
%>";
		private const string APPEND_CHAR = "+";
		private const string MYGEN_TEMPLATE_GUI = 
			@"
public class GeneratedGui : DotNetScriptGui
{
	public GeneratedGui(ZeusContext context) : base(context) {}

	public override void Setup()
	{
			ui.Title = @""§TITLE§"";
	
§SETUPCODE§

			ui.ShowGui = true;
	}
	
§MEMBERCODE§
}";
		
	}
}
