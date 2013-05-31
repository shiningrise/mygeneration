using System;
using System.Collections.Generic;
using System.Text;

namespace Scintilla.Configuration
{
    public interface IScintillaConfigProvider
    {
        bool PopulateScintillaConfig(IScintillaConfig config);
        bool PopulateLexerConfig(ILexerConfig config);
        bool PopulateLanguageConfig(ILanguageConfig config, ILexerConfigCollection lexers);
    }
}
