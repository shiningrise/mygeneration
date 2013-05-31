using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Zeus;

namespace MyGeneration
{
    public class AutoCompleteHelper
    {
        private static Dictionary<string, NodeInfo> _rootNodes;
        private static string _rootNodesString;
        
        public static Dictionary<string, NodeInfo> RootNodes
        {
            get
            {
                if (_rootNodes == null)
                {
                    _rootNodes = NodeInfo.LoadIntrinsicObjects();
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
    }

    public class NodeInfo
    {
        public Type NodeType;
        public string Name;
        public MemberTypes MemberType = MemberTypes.Custom;
        public ParameterInfo[] Parameters = null;

        public NodeInfo(Type type, string name)
        {
            this.NodeType = type;
            this.Name = name;
        }

        public NodeInfo(Type type, string name, MemberTypes mtype, ParameterInfo[] parameters)
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

        public List<NodeInfo> this[string member] 
        {
            get 
            {
                List<NodeInfo> infs = new List<NodeInfo>();
                MemberInfo[] mis = NodeType.GetMember(member);
                foreach (MemberInfo mi in mis) 
                {
                    NodeInfo ni = NodeInfo.BuildNodeInfo(mi);
                    infs.Add(ni);
                }
                return infs;
            }
        }

        private static Dictionary<Type, List<string>> _memberListsByType = new Dictionary<Type, List<string>>();

        public static NodeInfo BuildNodeInfo(MemberInfo mi)
        {
            NodeInfo node = null;
            if (mi.MemberType == MemberTypes.Field)
            {
                FieldInfo f = mi as FieldInfo;
                if (f.IsPublic && !f.IsStatic)
                {
                    node = new NodeInfo(f.FieldType, f.Name, f.MemberType, null);
                }
            }
            else if (mi.MemberType == MemberTypes.Method)
            {
                MethodInfo m = mi as MethodInfo;
                if (m.IsPublic && !m.IsStatic)
                {
                    node = new NodeInfo(m.ReturnType, m.Name, m.MemberType, m.GetParameters());
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
                        node = new NodeInfo(p.PropertyType, p.Name, p.MemberType, null);
                    }
                }
            }
            return node;
        }

        public static Dictionary<string, NodeInfo> LoadIntrinsicObjects()
        {
            Dictionary<string, NodeInfo> rootNodes = new Dictionary<string, NodeInfo>();
            rootNodes["input"] = new NodeInfo(typeof(IZeusInput), "input");
            rootNodes["output"] = new NodeInfo(typeof(IZeusOutput), "output");
            rootNodes["context"] = new NodeInfo(typeof(IZeusContext), "context");
            
            foreach (IZeusIntrinsicObject zio in ZeusFactory.IntrinsicObjectsArray)
            {
                try
                {
                    NodeInfo node = new NodeInfo(zio.Assembly.GetType(zio.ClassPath), zio.VariableName);
                    rootNodes[node.Name] = node;
                }
                catch { }
            }
            if (!rootNodes.ContainsKey("gui"))
                rootNodes["gui"] = new NodeInfo(typeof(IZeusGuiControl), "gui");

            return rootNodes;
        }
    }
}
