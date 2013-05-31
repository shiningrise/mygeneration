using System;
using System.Collections.Generic;
using System.Text;
using Scintilla.Enums;

namespace Scintilla.Configuration
{
    public interface ILexerConfig
    {
        Lexer Type { get; }

        int LexerID { get; }

        string LexerName { get; }

        IScintillaConfig ScintillaConfig { get; }

        SortedDictionary<int, ILexerStyle> Styles { get; }

        SortedDictionary<string, string> Properties { get; }
    }
}
