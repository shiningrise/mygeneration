using System;
using System.Text;
using System.Reflection;
using System.Collections;

using Zeus.UserInterface;
using Zeus.Configuration;

namespace Zeus
{
	#region ZeusContext
	/// <summary>
	/// The ZeusGuiContext object contains the data that goes through the interface segment of the template.
	/// </summary>
	public class ZeusTemplateContext : IZeusContext
	{
		protected IZeusIntrinsicObject[] _intrinsicObjects;
		protected IZeusInput _input;
		protected IZeusOutput _output;
		protected IGuiController _gui;
		protected Stack _templateStack;
		protected Hashtable _objects;
        protected ILog _log;


        public string Describe(string prefix, object o)
        {
            StringBuilder sb = new StringBuilder();
            Describe(sb, prefix, o.GetType(), 0);
            return sb.ToString();
        }

        private void Describe(StringBuilder sb, string prefix, Type t, int depth)
        {
            if (depth > 2) return;
            ArrayList assemblyList = new ArrayList();
            assemblyList.Add("PluginInterfaces");
            assemblyList.Add("Zeus");
            assemblyList.Add("MyMeta");
            assemblyList.Add("MyGenUtility");
            assemblyList.Add("MyGeneration");

            //Type t = obj.GetType();
            PropertyInfo[] props = t.GetProperties();
            MethodInfo[] methods = t.GetMethods();
            FieldInfo[] fields = t.GetFields();

            foreach (PropertyInfo pi in props)
            {
                bool isCool = false;
                if (pi.CanRead && pi.GetGetMethod().IsPublic) isCool = true;
                if (pi.CanWrite && pi.GetSetMethod().IsPublic) isCool = true;
                
                if (isCool)
                {
                    sb.Append(prefix).Append('.').AppendLine(pi.Name);
                    if (pi.CanRead)
                    {
                        Type chtype = pi.PropertyType;
                        if (assemblyList.Contains(chtype.Assembly.GetName().Name))
                        {
                            Describe(sb, prefix + "." + pi.Name, chtype, depth+1);
                        }
                    }
                }
            }

            foreach (MethodInfo mi in methods)
            {
                if (mi.IsPublic)
                {
                    if (!mi.Name.StartsWith("get_") && !mi.Name.StartsWith("set_"))
                    {
                        sb.Append(prefix).Append('.').AppendLine(mi.Name);
                        if ((mi.ReturnType != null) && (mi.GetParameters().Length == 0))
                        {
                            Type chtype = mi.ReturnType;
                            if (assemblyList.Contains(chtype.Assembly.GetName().Name))
                            {
                                Describe(sb, prefix + "." + mi.Name + "()", chtype, depth + 1);
                            }
                        }
                    }
                }
            }

            foreach (FieldInfo fi in fields)
            {
                if (fi.IsPublic)
                {
                    sb.Append(prefix).Append('.').AppendLine(fi.Name);
                }
            }
        }


		/// <summary>
		/// Creates a new ZeusGuiContext object.
		/// </summary>
		public ZeusTemplateContext() 
		{
			this._input = new ZeusInput();
			this._output = new ZeusOutput();
			this._gui = new GuiController(this);
			this._objects = new Hashtable();

			this._objects["ui"] = _gui;
		}

		/// <summary>
		/// Creates a new ZeusGuiContext object and defaults it's properties to the passed in objects.
		/// </summary>
		/// <param name="input">The ZeusInput object to pass into the template inteface segment.</param>
		/// <param name="gui">The GuiController object to use in the template inteface segment.</param>
		/// <param name="objects">A HashTable containing any other objects that need to be included in the template context.</param>
		public ZeusTemplateContext(IZeusInput input, /*IGuiController gui,*/ Hashtable objects)
		{
			this._input = input;
            //this._gui = gui;
            this._gui = new GuiController(this);
			this._objects = objects;
			this._output = new ZeusOutput();

			this._objects["ui"] = _gui;
		}


		/// <summary>
		/// The ZeusInput object is a collection containing all of the input variables to pass into the template body segment.
		/// </summary>
		public IZeusInput Input 
		{
			get { return _input; }
		}

		/// <summary>
		/// The ZeusOutput object is the output buffer in which the template output is written.
		/// </summary>
		public IZeusOutput Output 
		{
			get { return _output; }
		}

		/// <summary>
		/// The GuiController object describes the graphical user interface that attains input for the template body segment.
		/// </summary>
		public IZeusGuiControl Gui 
		{
			get { return _gui; }
		}

		/// <summary>
		/// The GuiController object describes the graphical user interface that attains input for the template body segment.
		/// </summary>
		public IZeusIntrinsicObject[] IntrinsicObjects 
		{
            get { return _intrinsicObjects; }
		}

		/// <summary>
		/// Depricated! Use: Execute(string path, bool copyContext) 
		/// </summary>
		/// <param name="path"></param>
		public void ExecuteTemplate(string path) 
		{
			ZeusTemplate template = new ZeusTemplate( FileTools.MakeAbsolute(path, this.ExecutingTemplate.FilePath) );
			template.Execute(this, 0, true);
		}

		public void Execute(string path, bool copyContext) 
		{
			ZeusTemplate template = new ZeusTemplate( FileTools.MakeAbsolute(path, this.ExecutingTemplate.FilePath) );
			if (copyContext) 
			{
				template.Execute(this.Copy(), 0, true);
			}
			else 
			{
				template.Execute(this, 0, true);
			}
		}

		/// <summary>
		/// A HashTable containing any other objects that need to be included in the template context.
		/// </summary>
		public Hashtable Objects 
		{
			get { return _objects; }
		}

		public void SetIntrinsicObjects(IZeusIntrinsicObject[] intrinsicObjects) 
		{
			this._intrinsicObjects = intrinsicObjects;
		}

		public void SetIntrinsicObjects(ArrayList intrinsicObjects) 
		{
			this.SetIntrinsicObjects( intrinsicObjects.ToArray(typeof(IZeusIntrinsicObject)) as IZeusIntrinsicObject[] );
		}

		public IZeusTemplateStub ExecutingTemplate
		{
			get 
			{
				return this.TemplateStack.Peek() as IZeusTemplateStub;
			}
		}

		public int ExecutionDepth
		{
			get 
			{
				return this.TemplateStack.Count;
			}
		}

		public ILog Log
		{
			get 
			{
				if (_log == null)
				{
					_log = new ZeusSimpleLog();
				}
				return _log;
			}
			set 
			{
				if (value != null) 
				{
					_log = value;
				}
			}
		}

		public Stack TemplateStack 
		{
			get 
			{
				if (this._templateStack == null) 
				{
					this._templateStack = new Stack();
				}
				return _templateStack;
			}
		}

		/// <summary>
		/// Creates a copy of the ZeusContext with a new output object;
		/// </summary>
		/// <returns></returns>
		public IZeusContext Copy()
		{
			ZeusTemplateContext context = Activator.CreateInstance( this.GetType() ) as ZeusTemplateContext;
			context._input = this._input;
			context._output = new ZeusOutput();
			context._objects = this._objects;
			context._log = this._log;
			context._gui = this._gui;
			context._intrinsicObjects = this._intrinsicObjects;
			context._templateStack = this._templateStack;
			return context;
		}
	}
	#endregion
	
	#region ZeusGuiContext
	/// <summary>
	/// The ZeusGuiContext object is just here for legacy support.
	/// </summary>
	public class ZeusGuiContext : ZeusTemplateContext
	{
		public ZeusGuiContext() : base() {}
		public ZeusGuiContext(IZeusInput input, /*IGuiController gui,*/ Hashtable objects) : base(input, /*gui,*/ objects) {}
	}
	#endregion

	#region ZeusTemplateContext
	/// <summary>
	/// The ZeusTemplateContext object is just here for legacy support.
	/// </summary>
	public class ZeusContext : ZeusGuiContext
	{
		public ZeusContext() : base() {}
		public ZeusContext(IZeusInput input, /*IGuiController gui,*/ Hashtable objects) : base(input, /*gui,*/ objects) {}
	}
	#endregion
	
}