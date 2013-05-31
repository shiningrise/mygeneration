using System;
using System.Collections;
using System.Text;

using MyGeneration.CodeSmithConversion.Template;

namespace MyGeneration.CodeSmithConversion.Conversion
{
	/// <summary>
	/// Summary description for GenerateGuiCode.
	/// </summary>
	public class GenerateGuiCode
	{
		public GenerateGuiCode()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static string GenerateCode(CstTemplate cstTemplate) 
		{
			StringBuilder builder = new StringBuilder();
			Type type;

			foreach (CstProperty p in cstTemplate.Properties) 
			{
				type = p.SystemType;
				if (cstTemplate.Language == CstTemplate.LANGUAGE_CSHARP) 
				{
					//
					
				}
				else if (cstTemplate.Language == CstTemplate.LANGUAGE_VBNET) 
				{
					// 
				}
			}

			return builder.ToString();
		}

	}
}
