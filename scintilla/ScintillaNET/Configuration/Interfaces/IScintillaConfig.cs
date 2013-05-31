using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Scintilla.Configuration
{
    public interface IScintillaConfig
    {
        void Configure(ScintillaControl scintilla, string language);

        ILanguageConfig LanguageDefaults { get; }

        ILanguageConfigCollection Languages { get; }

        List<string> LanguageNames { get; }

        ILexerConfigCollection Lexers { get; }

        List<IMenuItemConfig> LanguageMenuItems { get; }

        SortedDictionary<string, string> ExtensionLanguages { get; }

        SortedDictionary<string, string> Properties { get; }

        string DefaultFileExtention { get; set; }

        string FileOpenFilter { get; set; }

    }
}
