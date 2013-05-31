using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using Scintilla;
using Scintilla.Forms;
using Scintilla.Enums;
using Zeus;
using Zeus.Templates;

namespace MyGeneration.AutoCompletion
{
    public class AutoCompleteNodeInfo
    {
        public Type NodeType;
        public string Name;
        public MemberTypes MemberType = MemberTypes.Custom;
        public ParameterInfo[] Parameters = null;

        public AutoCompleteNodeInfo(Type type, string name)
        {
            this.NodeType = type;
            this.Name = name;
        }

        public AutoCompleteNodeInfo(Type type, string name, MemberTypes mtype, ParameterInfo[] parameters)
        {
            this.NodeType = type;
            this.Name = name;
            this.MemberType = mtype;
            this.Parameters = parameters;
        }

        public string ParameterString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (Parameters != null)
                {
                    sb.Append("(");
                    foreach (ParameterInfo p in Parameters)
                    {
                        if (sb.Length > 1) sb.Append(", ");

                        sb.Append(p.ParameterType.Name).Append(" ").Append(p.Name);
                    }
                    sb.Append(") returns ").Append(NodeType.Name);
                }
                return sb.ToString();
            }
        }

        public string MembersString
        {
            get
            {
                return string.Join(" ", Members.ToArray());
            }
        }

        public List<string> Members
        {
            get
            {
                List<string> list;
                if (_memberListsByType.ContainsKey(this.NodeType))
                {
                    list = _memberListsByType[this.NodeType];
                }
                else
                {
                    list = new List<string>();
                    foreach (FieldInfo fi in NodeType.GetFields())
                    {
                        if (fi.IsPublic && !fi.IsStatic)
                        {
                            if (!list.Contains(fi.Name)) list.Add(fi.Name);
                        }
                    }
                    foreach (MethodInfo mi in NodeType.GetMethods())
                    {
                        if (mi.IsPublic && !mi.IsStatic && !mi.IsSpecialName)
                        {
                            if (!list.Contains(mi.Name)) list.Add(mi.Name);
                        }
                    }
                    foreach (PropertyInfo pi in NodeType.GetProperties())
                    {
                        MethodInfo m = null;
                        if (pi.CanRead)
                        {
                            m = pi.GetGetMethod();
                        }
                        else if (pi.CanWrite)
                        {
                            m = pi.GetSetMethod();
                        }

                        if (m != null)
                        {
                            if (m.IsPublic && !m.IsStatic)
                            {
                                if (!list.Contains(pi.Name)) list.Add(pi.Name);
                            }
                        }
                    }
                    list.Sort();

                    _memberListsByType[NodeType] = list;
                }
                return list;
            }
        }

        public List<AutoCompleteNodeInfo> this[string member]
        {
            get
            {
                List<AutoCompleteNodeInfo> infs = new List<AutoCompleteNodeInfo>();
                MemberInfo[] mis = NodeType.GetMember(member);
                foreach (MemberInfo mi in mis)
                {
                    AutoCompleteNodeInfo ni = AutoCompleteNodeInfo.BuildNodeInfo(mi);
                    infs.Add(ni);
                }
                return infs;
            }
        }

        private static Dictionary<Type, List<string>> _memberListsByType = new Dictionary<Type, List<string>>();

        public static AutoCompleteNodeInfo BuildNodeInfo(MemberInfo mi)
        {
            AutoCompleteNodeInfo node = null;
            if (mi.MemberType == MemberTypes.Field)
            {
                FieldInfo f = mi as FieldInfo;
                if (f.IsPublic && !f.IsStatic)
                {
                    node = new AutoCompleteNodeInfo(f.FieldType, f.Name, f.MemberType, null);
                }
            }
            else if (mi.MemberType == MemberTypes.Method)
            {
                MethodInfo m = mi as MethodInfo;
                if (m.IsPublic && !m.IsStatic)
                {
                    node = new AutoCompleteNodeInfo(m.ReturnType, m.Name, m.MemberType, m.GetParameters());
                }
            }
            else if (mi.MemberType == MemberTypes.Property)
            {
                PropertyInfo p = mi as PropertyInfo;
                MethodInfo m = null;
                if (p.CanRead)
                {
                    m = p.GetGetMethod();
                }
                else if (p.CanWrite)
                {
                    m = p.GetSetMethod();
                }

                if (m != null)
                {
                    if (m.IsPublic && !m.IsStatic)
                    {
                        node = new AutoCompleteNodeInfo(p.PropertyType, p.Name, p.MemberType, null);
                    }
                }
            }
            return node;
        }

        public static Dictionary<string, AutoCompleteNodeInfo> LoadIntrinsicObjects()
        {
            Dictionary<string, AutoCompleteNodeInfo> rootNodes = new Dictionary<string, AutoCompleteNodeInfo>();
            rootNodes["input"] = new AutoCompleteNodeInfo(typeof(IZeusInput), "input");
            rootNodes["output"] = new AutoCompleteNodeInfo(typeof(IZeusOutput), "output");
            rootNodes["context"] = new AutoCompleteNodeInfo(typeof(IZeusContext), "context");

            foreach (IZeusIntrinsicObject zio in ZeusFactory.IntrinsicObjectsArray)
            {
                try
                {
                    AutoCompleteNodeInfo node = new AutoCompleteNodeInfo(zio.Assembly.GetType(zio.ClassPath), zio.VariableName);
                    rootNodes[node.Name] = node;
                }
                catch { }
            }

            return rootNodes;
        }
    }
}
