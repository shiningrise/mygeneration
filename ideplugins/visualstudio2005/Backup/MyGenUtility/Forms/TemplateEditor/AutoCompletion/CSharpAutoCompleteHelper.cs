using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using Scintilla.Enums;

namespace MyGeneration.AutoCompletion
{
    class CSharpAutoCompleteHelper : AutoCompleteHelper
    {
        public override bool IsCommentStyle(bool isTagged, int style)
        {
            return (style == (isTagged ? 58 : 2));
        }

        public override bool IsCodeStyle(bool isTagged, int style)
        {
            if (isTagged)
            {
                return (style == 62 || style == 61 || style == 56);
            }
            else
            {
                return (style == 11 || style == 5 || style == 0);
            }
        }

        public override string SelfQualifier
        {
            get { return "this"; }
        }

        public override StringComparison SelfQualifierStringComparisonRule
        {
            get { return StringComparison.CurrentCulture; }
        }

        public override bool ScanCodeForVariable(ZeusScintillaControl scintilla, String varname, out AutoCompleteNodeInfo node)
        {
            node = null;
            bool isFound = false;

            int currentIndex = scintilla.CurrentPos;
            int selectionStart = scintilla.SelectionStart;
            int selectionEnd = scintilla.SelectionEnd;
            int searchEndIndex = (currentIndex - varname.Length - 1);

            Stack<int> matches = new Stack<int>();

            scintilla.CurrentPos = searchEndIndex;
            scintilla.SearchAnchor();
            int matchPosition = scintilla.SearchPrevious((int)FindOption.WholeWord, varname);
            while ((matchPosition >= 0) && (matchPosition < searchEndIndex))
            {
                int style = scintilla.StyleAt(matchPosition);
                if (style == 61 || style == 11)
                {
                    matches.Push(matchPosition);
                }

                scintilla.CurrentPos = matchPosition;
                scintilla.SearchAnchor();

                matchPosition = scintilla.SearchPrevious((int)FindOption.WholeWord, varname);
            }

            scintilla.CurrentPos = currentIndex;
            scintilla.SelectionStart = selectionStart;
            scintilla.SelectionEnd = selectionEnd;
            scintilla.SearchAnchor();

            foreach (int m in matches)
            {
                string word = scintilla.GetWordFromPosition(m + 1);
                if (string.Equals(word, varname, StringComparison.CurrentCultureIgnoreCase))
                {
                    varname = word;

                    int beginWordIndex = m;
                    //scintilla.
                    char c = scintilla.CharAt(--beginWordIndex);
                    while (Char.IsWhiteSpace(c) && (beginWordIndex > 0)) c = scintilla.CharAt(--beginWordIndex);
                    StringBuilder typeName = new StringBuilder();
                    while (this.IsValidIdentifierChar(c) || (c == MemberSeperator))
                    {
                        if (typeName.Length == 0)
                            typeName.Append(c);
                        else 
                            typeName.Insert(0, c);

                        c = scintilla.CharAt(--beginWordIndex);
                    }
                    
                    string type = typeName.ToString();
                    List<Type> types = AutoCompleteHelper.SearchForType(type);
                    if (types.Count > 0)
                    {
                        node = new AutoCompleteNodeInfo(types[0], varname);
                        isFound = true;
                    }
                } 
            }

            return isFound;
        }
    }
}
