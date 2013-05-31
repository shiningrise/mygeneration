using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using Scintilla.Enums;

namespace MyGeneration.AutoCompletion
{
    class VBNetAutoCompleteHelper : AutoCompleteHelper
    {//Dim test as IDatabase
        public override string SelfQualifier
        {
            get { return "Me"; }
        }

        public override bool IsCommentStyle(bool isTagged, int style)
        {
            return (style == (isTagged ? 82 : 1));
        }

        public override bool IsCodeStyle(bool isTagged, int style)
        {
            if (isTagged)
            {
                return (style == 86 || style == 84 || style == 81);
            }
            else
            {
                return (style == 7 || style == 3 || style == 0);
            }
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
                if (style == 86 || style == 7)
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
                    int endWordIndex = m + word.Length;

                    char c = scintilla.CharAt(++endWordIndex);
                    List<string> words = new List<string>();
                   
                    //skip whitespace
                    while (Char.IsWhiteSpace(c)) c = scintilla.CharAt(++endWordIndex);

                    // get "As"
                    StringBuilder nextword = new StringBuilder();
                    do
                    {
                        nextword.Append(c);
                        c = scintilla.CharAt(++endWordIndex);
                    } while (!Char.IsWhiteSpace(c));

                    if (nextword.ToString().Equals("as", StringComparison.CurrentCultureIgnoreCase))
                    {
                        //skip whitespace
                        while (Char.IsWhiteSpace(c)) c = scintilla.CharAt(++endWordIndex);

                        // get Type
                        nextword.Remove(0, nextword.Length);
                        do
                        {
                            nextword.Append(c);
                            c = scintilla.CharAt(++endWordIndex);
                        } while (!Char.IsWhiteSpace(c) && (this.IsValidIdentifierChar(c) || (c == MemberSeperator)));

                        string type = nextword.ToString();
                        List<Type> types = AutoCompleteHelper.SearchForType(type);
                        if (types.Count > 0)
                        {
                            node = new AutoCompleteNodeInfo(types[0], varname);
                            isFound = true;
                        }
                    }
                }
            }

            return isFound;
        }
    }
}
