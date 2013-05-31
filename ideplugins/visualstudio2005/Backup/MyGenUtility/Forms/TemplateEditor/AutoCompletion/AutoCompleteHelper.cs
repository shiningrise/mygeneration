using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

using Scintilla;
using Scintilla.Forms;
using Scintilla.Enums;
using Zeus;
using Zeus.Templates;

namespace MyGeneration.AutoCompletion
{
    public abstract class AutoCompleteHelper
    {
        private const char DEFAULT_MEMBER_SEPERATOR = '.';
        #region Static Members
        private static Dictionary<string, AutoCompleteNodeInfo> _rootNodes;
        private static string _rootNodesString;

        private static CSharpAutoCompleteHelper _cSharpAutoCompleteHelper = new CSharpAutoCompleteHelper();
        private static VBNetAutoCompleteHelper _vbNetAutoCompleteHelper = new VBNetAutoCompleteHelper();
        private static JScriptAutoCompleteHelper _jScriptAutoCompleteHelper = new JScriptAutoCompleteHelper();
        private static VBScriptAutoCompleteHelper _vbScriptAutoCompleteHelper = new VBScriptAutoCompleteHelper();
        
        public static Dictionary<string, AutoCompleteNodeInfo> RootNodes
        {
            get
            {
                if (_rootNodes == null)
                {
                    _rootNodes = AutoCompleteNodeInfo.LoadIntrinsicObjects();
                }
                return _rootNodes;
            }
        }

        public static string RootNodesAutoCompleteString
        {
            get
            {
                if (_rootNodesString == null)
                {
                    List<string> names = new List<string>();
                    foreach (string name in RootNodes.Keys)
                    {
                        names.Add(name);
                    }
                    names.Sort();
                    _rootNodesString = string.Join(" ", names.ToArray());
                }
                return _rootNodesString;
            }
        }

        public static void CharAdded(ZeusScintillaControl scintilla, char ch)
        {
            AutoCompleteHelper ach = null;
            bool isTagged = (scintilla.ConfigurationLanguage == null) ? false : scintilla.ConfigurationLanguage.StartsWith("Tagged");

            switch (scintilla.ConfigurationLanguage) 
            {
                case "TaggedC#":
                case "C#":
                    ach = _cSharpAutoCompleteHelper;
                    break;
                case "TaggedVB.Net":
                case "VB.Net":
                    ach = _vbNetAutoCompleteHelper;
                    break;
                case "TaggedJScript":
                case "JScript":
                    ach = _jScriptAutoCompleteHelper;
                    break;
                case "TaggedVBScript":
                case "VBScript":
                    ach = _vbScriptAutoCompleteHelper;
                    break;
            }


            if (ach != null) 
            {
                ach.HandleCharAdded(scintilla, isTagged, ch);        
            }
        }
        #endregion


        public AutoCompleteHelper() { }

        public abstract bool IsCommentStyle(bool isTagged, int style);
        public abstract bool IsCodeStyle(bool isTagged, int style);
       
        public virtual string SelfQualifier
        {
            get { return string.Empty; }
        }

        public virtual StringComparison SelfQualifierStringComparisonRule
        {
            get { return StringComparison.InvariantCultureIgnoreCase; }
        }

        public virtual bool IsAutoCompleteSeek(char ch, bool isCtrlPressed)
        {
            return (ch == ' ' && isCtrlPressed);
        }

        public virtual bool IsAutoCompleteMember(char ch)
        {
            return (ch == MemberSeperator);
        }

        public virtual bool IsCallTip(char ch)
        {
            return (ch == '(');
        }

        public virtual bool IsValidIdentifierChar(char ch)
        {
            return (ch.Equals('_') || char.IsLetterOrDigit(ch));
        }

        public virtual char MemberSeperator
        {
            get { return DEFAULT_MEMBER_SEPERATOR; }
        }

        public virtual bool ScanCodeForVariable(ZeusScintillaControl scintilla, string varname, out AutoCompleteNodeInfo node)
        {
            node = null;
            return false;
        }

        public void HandleCharAdded(ZeusScintillaControl scintilla, bool isTagged, char ch)
        {
            bool isCtrlPressed = (Control.ModifierKeys == (System.Windows.Forms.Keys.Control | Control.ModifierKeys));
            bool isAutoSeek = IsAutoCompleteSeek(ch, isCtrlPressed);
            bool isAutoMember = IsAutoCompleteMember(ch);
            bool isCallTip = IsCallTip(ch);

            if (isAutoSeek || isAutoMember || isCallTip)
            {
                int pos = scintilla.CurrentPos - 1;
                char c = scintilla.CharAt(--pos);
                Stack<string> stk = new Stack<string>();
                List<string> lst = new List<string>();
                System.Text.StringBuilder tmp = new System.Text.StringBuilder();

                int style = scintilla.StyleAt(pos);
                if (IsCodeStyle(isTagged, style))
                {
                    while (IsValidIdentifierChar(c) || (c == MemberSeperator))
                    {
                        if (c == MemberSeperator)
                        {
                            if (tmp.Length > 0)
                            {
                                lst.Insert(0, tmp.ToString());
                                stk.Push(tmp.ToString());
                                tmp.Remove(0, tmp.Length);
                            }
                        }
                        else
                        {
                            tmp.Insert(0, c);
                        }
                        c = scintilla.CharAt(--pos);
                    }
                    if (tmp.Length > 0)
                    {
                        lst.Insert(0, tmp.ToString());
                        stk.Push(tmp.ToString());
                        tmp.Remove(0, tmp.Length);
                    }

                    AutoCompleteNodeInfo n = null;
                    AutoCompleteNodeInfo nextToLastNode = null;
                    System.Collections.Generic.List<AutoCompleteNodeInfo> ns = null;
                    string lastmsg = null, firstmsg = null;
                    int memberDepth = stk.Count;
                    if (stk.Count > 0)
                    {
                        lastmsg = stk.Pop();
                        if ((lastmsg.Equals(this.SelfQualifier, this.SelfQualifierStringComparisonRule)) && (stk.Count > 0))
                        {
                            lastmsg = stk.Pop();
                        }

                        if (AutoCompleteHelper.RootNodes.ContainsKey(lastmsg))
                        {
                            n = AutoCompleteHelper.RootNodes[lastmsg];
                        }
                        else 
                        {
                            AutoCompleteNodeInfo newRootNode;
                            if (ScanCodeForVariable(scintilla, lastmsg, out newRootNode))
                            {
                                n = newRootNode;
                            }
                        }
                    }

                    while (n != null && stk.Count > 0)
                    {
                        nextToLastNode = n;
                        int stkCount = stk.Count;
                        lastmsg = stk.Pop();

                        ns = n[lastmsg];
                        if (ns.Count == 1)
                        {
                            n = ns[0];
                        }
                        else
                        {
                            n = null;
                            if (stk.Count != 0)
                            {
                                ns = null;
                            }
                        }
                    }

                    if (isAutoSeek)
                    {
                        scintilla.DeleteBack();

                        if (n != null)
                        {
                            if (scintilla.CharAt(scintilla.CurrentPos - 1) == MemberSeperator)
                            {
                                scintilla.AutoCShow(0, n.MembersString);
                            }
                            else
                            {
                                scintilla.AutoCShow(n.Name.Length, nextToLastNode.MembersString);
                            }
                        }
                        else if (stk.Count == 0 && nextToLastNode != null)
                        {
                            scintilla.AutoCShow(lastmsg.Length, nextToLastNode.MembersString);
                        }
                        else if (stk.Count == 0 && n == null && nextToLastNode == null)
                        {
                            if (lastmsg == null) lastmsg = string.Empty;

                            scintilla.AutoCShow(lastmsg.Length, AutoCompleteHelper.RootNodesAutoCompleteString);
                        }
                    }
                    if (isAutoMember || isCallTip)
                    {
                        if (n != null)
                        {
                            if (isAutoMember)
                            {
                                scintilla.AutoCShow(0, n.MembersString);
                            }
                        }
                        else
                        {
                            if (isAutoMember && 
                                (lastmsg != null) && 
                                (lastmsg.Equals(this.SelfQualifier, this.SelfQualifierStringComparisonRule)) && 
                                (memberDepth == 1))
                            {
                                scintilla.AutoCShow(0, AutoCompleteHelper.RootNodesAutoCompleteString);
                            }
                        }

                        if (ns != null)
                        {
                            if (isCallTip)
                            {
                                string methodSigs = string.Empty;
                                int i = 0;
                                foreach (AutoCompleteNodeInfo ni in ns)
                                {
                                    if (ni.MemberType == System.Reflection.MemberTypes.Method)
                                    {
                                        i++;
                                        if (methodSigs.Length > 0) methodSigs += "\n";
                                        //methodSigs += '\u0001' + i.ToString() + " of " + ns.Count + '\u0002' + " " + ni.ParameterString;
                                        methodSigs += ni.ParameterString;
                                    }
                                }
                                if (methodSigs.Length > 0)
                                {
                                    scintilla.CallTipShow(scintilla.CurrentPos - 1, methodSigs);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static List<Type> SearchForType(string type)
        {
            List<Type> types = new List<Type>();

            if (type.Contains("."))
            {
                try
                {
                    Type t = Type.ReflectionOnlyGetType(type, false, true);
                    if (t != null)
                    {
                        types.Add(t);
                    }
                }
                catch { }
            }
            else
            {
                // native types
                switch (type)
                {
                    case "byte":
                        types.Add(typeof(Byte));
                        break;
                    case "sbyte":
                        types.Add(typeof(SByte));
                        break;
                    case "int":
                        types.Add(typeof(Int32));
                        break;
                    case "uint":
                        types.Add(typeof(UInt32));
                        break;
                    case "short":
                        types.Add(typeof(Int16));
                        break;
                    case "ushort":
                        types.Add(typeof(UInt16));
                        break;
                    case "long":
                        types.Add(typeof(Int64));
                        break;
                    case "ulong":
                        types.Add(typeof(UInt64));
                        break;
                    case "float":
                        types.Add(typeof(Single));
                        break;
                    case "double":
                        types.Add(typeof(Double));
                        break;
                    case "char":
                        types.Add(typeof(Char));
                        break;
                    case "object":
                        types.Add(typeof(Object));
                        break;
                    case "string":
                        types.Add(typeof(String));
                        break;
                    case "decimal":
                        types.Add(typeof(Decimal));
                        break;
                }

                if (types.Count == 0)
                {
                    // mygeneration types
                    Assembly asmbly;
                    List<Assembly> assemblies = new List<Assembly>();
                    foreach (AutoCompleteNodeInfo n in AutoCompleteHelper.RootNodes.Values)
                    {
                        asmbly = n.NodeType.Assembly;
                        if (!assemblies.Contains(asmbly)) assemblies.Add(asmbly);
                    }

                    foreach (Assembly a in assemblies)
                    {
                        try
                        {
                            Type[] atypes = a.GetTypes();
                            foreach (Type t in atypes)
                            {
                                if (string.Equals(t.Name, type, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    types.Add(t);
                                    break;
                                }
                            }
                        }
                        catch { }
                    }

                    if (types.Count == 0)
                    {
                        // all types
                        assemblies.Clear();
                        foreach (AutoCompleteNodeInfo n in AutoCompleteHelper.RootNodes.Values)
                        {
                            asmbly = n.NodeType.Assembly;
                            if (!assemblies.Contains(asmbly)) assemblies.Add(asmbly);
                        }
                        MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory.ToString());
                        foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            try
                            {
                                Type[] atypes = a.GetTypes();
                                foreach (Type t in atypes)
                                {
                                    if (string.Equals(t.Name, type, StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        types.Add(t);
                                        break;
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            return types;
        }
    }
}
