using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMetaPluginTestTool
{
    public interface IMyMetaTestContext
    {
        string ProviderType { get; }
        string ConnectionString { get; }
        bool DefaultDatabaseOnly { get; }
        bool IncludeTables { get; }
        bool IncludeTableColumns { get; }
        bool IncludeTableOther { get; }
        bool IncludeViews { get; }
        bool IncludeViewColumns { get; }
        bool IncludeViewOther { get; }
        bool IncludeProcedures { get; }
        bool IncludeParameters { get; }
        bool IncludeProcOther { get; }

        bool HasErrors { get; }
        List<Exception> Errors { get; }

        void AppendLog(string message);
        void AppendLog(string message, Exception ex);
    }
}
