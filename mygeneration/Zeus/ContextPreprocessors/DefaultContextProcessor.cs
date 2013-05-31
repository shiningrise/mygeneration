using System;
using MyGeneration;
using MyMeta;
using Zeus;

namespace Zeus
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class DefaultContextProcessor : IZeusContextProcessor
	{
		public DefaultContextProcessor() {}
	
		/// <summary>
		/// Populate the context object with any deafult settings.
		/// Here we are setting the default connection information
		/// and the output/template paths.
		/// </summary>
		/// <param name="context"></param>
		public void Process(IZeusContext context) 
		{
			this._ProcessMyMeta(context);
		}

		private void _ProcessMyMeta(IZeusContext context) 
		{
			IZeusInput input = context.Input;
			dbRoot myMeta = null;

			if (context.Objects.Contains("MyMeta")) 
				myMeta = context.Objects["MyMeta"] as dbRoot;

			if (myMeta != null)
			{
				if (!myMeta.IsConnected) 
				{
					string driver = null, connectionString = null;

					if (input.Contains("__dbDriver"))
						driver = input["__dbDriver"].ToString();

					if (input.Contains("__dbConnectionString"))
						connectionString = input["__dbConnectionString"].ToString();

					if ( (driver != null) && (connectionString != null) ) 
					{
						myMeta.Connect(driver, connectionString);

                        if (input.Contains("__showDefaultDatabaseOnly"))
                            myMeta.ShowDefaultDatabaseOnly = ((string)input["__showDefaultDatabaseOnly"] == bool.TrueString);

						if (input.Contains("__dbTargetMappingFileName"))
							myMeta.DbTargetMappingFileName = (string)input["__dbTargetMappingFileName"];

						if (input.Contains("__dbLanguageMappingFileName"))
							myMeta.LanguageMappingFileName = (string)input["__dbLanguageMappingFileName"];

						if (input.Contains("__userMetaDataFileName"))
							myMeta.UserMetaDataFileName = (string)input["__userMetaDataFileName"];

						if (input.Contains("__dbTarget"))
							myMeta.DbTarget = (string)input["__dbTarget"];

						if (input.Contains("__language"))
							myMeta.Language = (string)input["__language"];

						if (input.Contains("__domainOverride"))
							myMeta.DomainOverride = (bool)input["__domainOverride"];
					}
				}
			}
		}
	}
}
