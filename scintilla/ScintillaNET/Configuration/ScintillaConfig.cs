using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Scintilla.Enums;

namespace Scintilla.Configuration
{
    public class ScintillaConfig : IScintillaConfig
    {
        private IScintillaConfigProvider provider;
        private ILexerConfigCollection lexers;
        private ILanguageConfigCollection languages;
        private ILanguageConfig languageDefaults;
        private SortedDictionary<string, string> properties;
        private SortedDictionary<string, string> extensionLanguages;
        // private SortedDictionary<int, ILexerStyle> styles;
        private List<string> languageNames;
        private List<IMenuItemConfig> languageMenuItems;
        private string defaultFileExtention;
        private string fileOpenFilter;

        public ScintillaConfig()
        {
            this.provider = new ScintillaConfigProvider();
            provider.PopulateScintillaConfig(this);
        }

        public ScintillaConfig(IScintillaConfigProvider provider) 
        {
            if (provider == null)
                throw new Exception("IScintillaConfigProvider must be provided to the ScintillaConfig constructor!");

            this.provider = provider;
            provider.PopulateScintillaConfig(this);
        }

        public SortedDictionary<string, string> ExtensionLanguages
        {
            get
            {
                if (extensionLanguages == null)
                {
                    extensionLanguages = new SortedDictionary<string, string>();
                }
                return extensionLanguages;
            }
        }

        public SortedDictionary<string, string> Properties
        {
            get
            {
                if (properties == null)
                {
                    properties = new SortedDictionary<string, string>();
                }
                return properties;
            }
        }

        public ILanguageConfig LanguageDefaults
        {
            get
            {
                if (languageDefaults == null)
                {
                    languageDefaults = new LanguageConfig(this, "global");
                }
                return languageDefaults;
            }
        }

        public ILanguageConfigCollection Languages
        {
            get
            {
                if (languages == null)
                {
                    languages = new LanguageConfigCollection(this, provider, Lexers);
                }
                return languages;
            }
        }

        public List<string> LanguageNames
        {
            get
            {
                if (languageNames == null)
                {
                    languageNames = new List<string>();
                }
                return languageNames;
            }
        }

        public ILexerConfigCollection Lexers
        {
            get 
            {
                if (lexers == null)
                {
                    lexers = new LexerConfigCollection(this, provider);
                }
                return lexers; 
            }
        }

        public List<IMenuItemConfig> LanguageMenuItems
        {
            get
            {
                if (languageMenuItems == null)
                {
                    languageMenuItems = new List<IMenuItemConfig>();
                }
                return languageMenuItems;
            }
        }

        public string DefaultFileExtention
        {
            get { return defaultFileExtention; }
            set { defaultFileExtention = value; }
        }

        public string FileOpenFilter
        {
            get { return fileOpenFilter; }
            set { fileOpenFilter = value; }
        }

        #region Configure Scintilla
        public void Configure(ScintillaControl scintilla, string language)
        {
            scintilla.StyleClearAll();
            scintilla.DisableMarginClickFold();

            IScintillaConfig conf = this;
            ILanguageConfig lang = conf.Languages[language];
            if (lang != null)
            {
                lang = lang.CombinedLanguageConfig;
                if (lang.CodePage.HasValue) scintilla.CodePage = lang.CodePage.Value;
                if (lang.SelectionAlpha.HasValue) scintilla.SelectionAlpha = lang.SelectionAlpha.Value;
                if (lang.SelectionBackColor != Color.Empty) scintilla.SetSelectionBackground(true, lang.SelectionBackColor);
                if (lang.TabSize.HasValue) scintilla.TabWidth = lang.TabSize.Value;
                if (lang.IndentSize.HasValue) scintilla.Indent = lang.IndentSize.Value;

                // Enable line numbers
                scintilla.MarginWidthN(0, 40);

                bool enableFolding = false;
                if (lang.Fold.HasValue) enableFolding = lang.Fold.Value;
                if (enableFolding)
                {
                    // Lexer specific properties
                    scintilla.Property("fold", "1");
                    if (lang.FoldAtElse.HasValue) scintilla.Property("fold.at.else", (lang.FoldAtElse.Value ? "1" : "0"));
                    if (lang.FoldCompact.HasValue) scintilla.Property("fold.compact", (lang.FoldCompact.Value ? "1" : "0"));
                    if (lang.FoldComment.HasValue) scintilla.Property("fold.comment", (lang.FoldComment.Value ? "1" : "0"));
                    if (lang.FoldPreprocessor.HasValue) scintilla.Property("fold.preprocessor", (lang.FoldPreprocessor.Value ? "1" : "0"));
                    if (lang.StylingWithinPreprocessor.HasValue) scintilla.Property("styling.within.preprocessor", (lang.PythonFoldQuotes.Value ? "1" : "0"));

                    if (lang.HtmlFold.HasValue) scintilla.Property("fold.html", (lang.HtmlFold.Value ? "1" : "0"));
                    if (lang.HtmlFoldPreprocessor.HasValue) scintilla.Property("fold.html.preprocessor", (lang.HtmlFoldPreprocessor.Value ? "1" : "0"));
                    if (lang.HtmlTagsCaseSensitive.HasValue) scintilla.Property("html.tags.case.sensitive", (lang.HtmlTagsCaseSensitive.Value ? "1" : "0"));

                    if (lang.PythonFoldComment.HasValue) scintilla.Property("fold.comment.python", (lang.PythonFoldComment.Value ? "1" : "0"));
                    if (lang.PythonFoldQuotes.HasValue) scintilla.Property("fold.quotes.python", (lang.PythonFoldQuotes.Value ? "1" : "0"));
                    if (lang.PythonWhingeLevel.HasValue) scintilla.Property("tab.timmy.whinge.level", lang.PythonWhingeLevel.Value.ToString());

                    if (lang.SqlBackslashEscapes.HasValue) scintilla.Property("sql.backslash.escapes", (lang.SqlBackslashEscapes.Value ? "1" : "0"));
                    if (lang.SqlBackticksIdentifier.HasValue) scintilla.Property("lexer.sql.backticks.identifier", (lang.SqlBackticksIdentifier.Value ? "1" : "0"));
                    if (lang.SqlFoldOnlyBegin.HasValue) scintilla.Property("fold.sql.only.begin", (lang.SqlFoldOnlyBegin.Value ? "1" : "0"));

                    if (lang.PerlFoldPod.HasValue) scintilla.Property("fold.perl.pod", (lang.PerlFoldPod.Value ? "1" : "0"));
                    if (lang.PerlFoldPackage.HasValue) scintilla.Property("fold.perl.package", (lang.PerlFoldPackage.Value ? "1" : "0"));

                    if (lang.NsisIgnoreCase.HasValue) scintilla.Property("nsis.ignorecase", (lang.NsisIgnoreCase.Value ? "1" : "0"));
                    if (lang.NsisUserVars.HasValue) scintilla.Property("nsis.uservars", (lang.NsisUserVars.Value ? "1" : "0"));
                    if (lang.NsisFoldUtilCommand.HasValue) scintilla.Property("nsis.foldutilcmd", (lang.NsisFoldUtilCommand.Value ? "1" : "0"));
                    if (lang.CppAllowDollars.HasValue) scintilla.Property("lexer.cpp.allow.dollars", (lang.CppAllowDollars.Value ? "1" : "0"));

                    //for HTML lexer: "asp.default.language"
                    //enum script_type { eScriptNone = 0, eScriptJS, eScriptVBS, eScriptPython, eScriptPHP, eScriptXML, eScriptSGML, eScriptSGMLblock };

                    scintilla.MarginWidthN(1, 0);
                    scintilla.MarginTypeN(1, MarginType.Symbol);
                    scintilla.MarginMaskN(1, unchecked((int)0xFE000000));
                    scintilla.MarginSensitiveN(1, true);

                    if (lang.FoldMarginWidth.HasValue) scintilla.MarginWidthN(1, lang.FoldMarginWidth.Value);
                    else scintilla.MarginWidthN(1, 20);

                    if (lang.FoldMarginColor != Color.Empty) scintilla.SetFoldMarginColor(true, lang.FoldMarginColor);
                    if (lang.FoldMarginHighlightColor != Color.Empty) scintilla.SetFoldMarginHiColor(true, lang.FoldMarginHighlightColor);
                    if (lang.FoldFlags.HasValue) scintilla.SetFoldFlags(lang.FoldFlags.Value);

                    scintilla.MarkerDefine(MarkerOutline.Folder, MarkerSymbol.Plus);
                    scintilla.MarkerDefine(MarkerOutline.FolderOpen, MarkerSymbol.Minus);
                    scintilla.MarkerDefine(MarkerOutline.FolderEnd, MarkerSymbol.Empty);
                    scintilla.MarkerDefine(MarkerOutline.FolderMidTail, MarkerSymbol.Empty);
                    scintilla.MarkerDefine(MarkerOutline.FolderOpenMid, MarkerSymbol.Minus);
                    scintilla.MarkerDefine(MarkerOutline.FolderSub, MarkerSymbol.Empty);
                    scintilla.MarkerDefine(MarkerOutline.FolderTail, MarkerSymbol.Empty);

                    scintilla.EnableMarginClickFold();
                }

                if (!string.IsNullOrEmpty(lang.WhitespaceCharacters))
                    scintilla.WhitespaceChars(lang.WhitespaceCharacters);

                if (!string.IsNullOrEmpty(lang.WordCharacters))
                    scintilla.WordChars(lang.WordCharacters);

                ILexerConfig lexer = lang.Lexer;
                if (lexer != null)
                {
                    scintilla.Lexer = lexer.LexerID;
                    //scintilla.LexerLanguage(lang.Name);
                }

                SortedDictionary<int, ILexerStyle> styles = lang.Styles;
                foreach (ILexerStyle style in styles.Values)
                {
                    if (style.ForeColor != Color.Empty)
                        scintilla.StyleSetFore(style.StyleIndex, style.ForeColor);

                    if (style.BackColor != Color.Empty)
                        scintilla.StyleSetBack(style.StyleIndex, style.BackColor);

                    if (!string.IsNullOrEmpty(style.FontName))
                        scintilla.StyleSetFont(style.StyleIndex, style.FontName);

                    if (style.FontSize.HasValue)
                        scintilla.StyleSetSize(style.StyleIndex, style.FontSize.Value);

                    if (style.Bold.HasValue)
                        scintilla.StyleSetBold(style.StyleIndex, style.Bold.Value);

                    if (style.Italics.HasValue)
                        scintilla.StyleSetItalic(style.StyleIndex, style.Italics.Value);

                    if (style.EOLFilled.HasValue)
                        scintilla.StyleSetEOLFilled(style.StyleIndex, style.EOLFilled.Value);

                    scintilla.StyleSetCase(style.StyleIndex, style.CaseVisibility);
                }
                scintilla.StyleBits = scintilla.StyleBitsNeeded;

                for (int j = 0; j < 9; j++)
                {
                    if (lang.KeywordLists.ContainsKey(j))
                        scintilla.KeyWords(j, lang.KeywordLists[j]);
                    else
                        scintilla.KeyWords(j, string.Empty);
                }
            }

            scintilla.Colorize(0, scintilla.Length);
        }
        #endregion
    }
}
