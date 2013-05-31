using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Permissions;

using Scintilla.Enums;
using Scintilla.Configuration;

namespace Scintilla.Enums
{
    public enum SmartIndent
    {
        None = 0,
        CPP = 1,
        CPP2 = 4,
        Simple = 2,
        Custom = 3
    }
}

namespace Scintilla
{
    internal enum VOID
    {
        NULL
    }

    public delegate void ScintillaConfigureDelegate(ScintillaControl scintillaControl, string language);

    [DefaultBindingProperty("Text"), DefaultProperty("Text"), DefaultEvent("DocumentChanged")]
    public partial class ScintillaControl : System.Windows.Forms.Control, ISupportInitialize
    {
        public const string DefaultDllName = "SciLexer.dll";
        
        static ScintillaControl()
        {
            // setup Enum-based-indexers
            Collection<IndicatorStyle>.Setup(2080, 2081);
        }

        private static readonly object _nativeEventKey = new object();
        
        private Encoding _encoding;
        private string _sciLexerDllName = null;
		//private IntPtr _hwndScintilla;
        private string _configurationLanguage = null;
        private bool _isBraceMatching = false;
        private Enums.SmartIndent _smartIndentType = SmartIndent.None;
        private Dictionary<int, int> _ignoredKeys = new Dictionary<int, int>();
        
        private ScintillaConfigureDelegate _configure = null;
        private EventHandler<CharAddedEventArgs> _smartIndenting = null;
        
        public ScintillaControl()
            : this(DefaultDllName)
        {
        }

        public ScintillaControl(string sciLexerDllName)
        {
            _sciLexerDllName = sciLexerDllName;
            
            // Instantiate the indexers for this instance
            IndicatorStyle = new Collection<IndicatorStyle>(this);
            IndicatorForegroundColor = new IntCollection(this);
            MarkerForegroundColor = new CachingIntCollection(this);
            MarkerBackgroundColor = new CachingIntCollection(this);
            Line = new ReadOnlyStringCollection(this);

            // setup instance-based-indexers
            IndicatorForegroundColor.Setup(2082, 2083);
            MarkerForegroundColor.Setup(2041);
            MarkerBackgroundColor.Setup(2042);
            Line.Setup(2153);
            
            // Set up default encoding
            _encoding = Encoding.GetEncoding(this.CodePage);
            
            InitializeComponent();

            // Drag Drop Support
            NativeMethods.DragAcceptFiles(this.Handle, 1);

            // Brace Matching/Highlighting
            UpdateUI += new EventHandler<UpdateUIEventArgs>(scintillaControlBraceMatch_UpdateUI);
            DoubleClick += new EventHandler(scintillaControlBlockSelect_DoubleClick);
        }

        #region Custom Drag/Drop Support for UriDropped event
        unsafe void HandleFileDrop(IntPtr hDrop)
        {
            int nfiles = NativeMethods.DragQueryFileA(hDrop, 0xffffffff, (IntPtr)null, 0);
            string files = "";
            byte[] buffer = new byte[1024];
            for (uint i = 0; i < nfiles; i++)
            {
                fixed (byte* b = buffer)
                {
                    NativeMethods.DragQueryFileA(hDrop, i, (IntPtr)b, 1024);
                    if (files.Length > 0) files += "|";
                    files += MarshalStr((IntPtr)b);
                }
            }
            NativeMethods.DragFinish(hDrop);

            if (Events[UriDroppedEventKey] != null)
            {
                ((EventHandler<UriDroppedEventArgs>)Events[UriDroppedEventKey])(this, new UriDroppedEventArgs(files));
            }
        }
        #endregion
		
        #region Event Dispatch Mechanism
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            //	Uh-oh. Code based on undocumented unsupported .NET behavior coming up!
            //	Windows Forms Sends Notify messages back to the originating
            //	control ORed with 0x2000. This is way cool becuase we can listen for
            //	WM_NOTIFY messages originating form our own hWnd (from Scintilla)
            if ((m.Msg) == NativeMethods.WM_DROPFILES)
            {
                HandleFileDrop(m.WParam);
                return;
            }
            else if ((m.Msg ^ 0x2000) != NativeMethods.WM_NOTIFY)
            {
                base.WndProc(ref m);
                return;
            }

            SCNotification scnotification = (SCNotification)Marshal.PtrToStructure(m.LParam, typeof(SCNotification));
            // dispatch to listeners of the native event first
            // this allows listeners to get the raw event if they really wish
            // but ideally, they'd just use the .NET event 
            if (Events[_nativeEventKey] != null)
                ((EventHandler<NativeScintillaEventArgs>)Events[_nativeEventKey])(this, new NativeScintillaEventArgs(m, scnotification));

            DispatchScintillaEvent(scnotification);
            base.WndProc(ref m);

        }
        
        protected event EventHandler<NativeScintillaEventArgs> NativeScintillaEvent
        {
            add { Events.AddHandler(_nativeEventKey, value); }
            remove { Events.RemoveHandler(_nativeEventKey, value); }
        }
        #endregion

        #region SendMessageDirect

        /// <summary>
        /// This is the primary Native communication method with Scintilla
        /// used by this control. All the other overloads call into this one.
        /// </summary>
        internal IntPtr SendMessageDirect(uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (!this.IsDisposed)
            {
                Message m = new Message();
                m.Msg = (int)msg;
                m.WParam = wParam;
                m.LParam = lParam;
                m.HWnd = Handle;

                //  DefWndProc is the Window Proc associated with the window
                //  class for this control created by Windows Forms. It will
                //  in turn call Scintilla's DefWndProc Directly. This has 
                //  the same net effect as using Scintilla's DirectFunction
                //  in that SendMessage isn't used to get the message to 
                //  Scintilla but requires 1 less PInvoke and I don't have
                //  to maintain the FunctionPointer and "this" reference
                DefWndProc(ref m);

                return m.Result;
            }
            else
            {
                return IntPtr.Zero;
            }
        }

        //  Various overloads provided for syntactical convinience.
        //  note that the return value is int (32 bit signed Integer). 
        //  If you are invoking a message that returns a pointer or
        //  handle like SCI_GETDIRECTFUNCTION or SCI_GETDOCPOINTER
        //  you MUST use the IntPtr overload to ensure 64bit compatibility

        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (,)
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <returns></returns>
        internal int SendMessageDirect(uint msg)
        {
            return (int)SendMessageDirect(msg, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (int,int)    
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <param name="wParam">wParam</param>
        /// <param name="lParam">lParam</param>
        /// <returns></returns>
        internal int SendMessageDirect(uint msg, int wParam, int lParam)
        {
            return (int)SendMessageDirect(msg, (IntPtr)wParam, (IntPtr)lParam);
        }

        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (int,)    
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <param name="wParam">wParam</param>
        /// <returns></returns>
        internal int SendMessageDirect(uint msg, int wParam)
        {
            return (int)SendMessageDirect(msg, (IntPtr)wParam, IntPtr.Zero);
        }

        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (,int)    
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <param name="NULL">always pass null--Unused parameter</param>
        /// <param name="lParam">lParam</param>
        /// <returns></returns>
        internal int SendMessageDirect(uint msg, VOID NULL, int lParam)
        {
            return (int)SendMessageDirect(msg, IntPtr.Zero, (IntPtr)lParam);
        }

        
        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (bool,int)    
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <param name="wParam">boolean wParam</param>
        /// <param name="lParam">int lParam</param>
        /// <returns></returns>
        internal int SendMessageDirect(uint msg, bool wParam, int lParam)
        {
            return (int)SendMessageDirect(msg, (IntPtr)(wParam ? 1 : 0), (IntPtr)lParam);
        }

        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (bool,)    
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <param name="wParam">boolean wParam</param>
        /// <returns></returns>
        internal int SendMessageDirect(uint msg, bool wParam)
        {
            return (int)SendMessageDirect(msg, (IntPtr)(wParam ? 1 : 0), IntPtr.Zero);
        }

        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (int,bool)    
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <param name="wParam">int wParam</param>
        /// <param name="lParam">boolean lParam</param>
        /// <returns></returns>
        internal int SendMessageDirect(uint msg, int wParam, bool lParam)
        {
            return (int)SendMessageDirect(msg, (IntPtr)wParam, (IntPtr)(lParam ? 1 : 0));
        }

        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (,stringresult)    
        /// Notes:
        ///  Helper method to wrap all calls to messages that take a char*
        ///  in the lParam and returns a regular .NET String. This overload
        ///  assumes there will be no wParam and obtains the string length
        ///  by calling the message with a 0 lParam. 
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <param name="text">String output</param>
        /// <returns></returns>
        internal int SendMessageDirect(uint msg, out string text)
        {
            int length = SendMessageDirect(msg, 0, 0);
            return SendMessageDirect(msg, IntPtr.Zero, out text, length);
        }

        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (int,stringresult)    
        /// Notes:
        ///  Helper method to wrap all calls to messages that take a char*
        ///  in the lParam and returns a regular .NET String. This overload
        ///  assumes there will be no wParam and obtains the string length
        ///  by calling the message with a 0 lParam. 
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <param name="text">String output</param>
        /// <returns></returns>
        internal int SendMessageDirect(uint msg, int wParam, out string text)
        {
            int length = SendMessageDirect(msg, 0, 0);
            return SendMessageDirect(msg, (IntPtr)wParam, out text, length);
        }

        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (?)    
        /// Notes:
        ///  Helper method to wrap all calls to messages that take a char*
        ///  in the wParam and set a regular .NET String in the lParam. 
        ///  Both the length of the string and an additional wParam are used 
        ///  so that various string Message styles can be acommodated.
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <param name="wParam">int wParam</param>
        /// <param name="text">String output</param>
        /// <param name="length">length of the input buffer</param>
        /// <returns></returns>
        internal unsafe int SendMessageDirect(uint msg, IntPtr wParam, out string text, int length)
        {
            IntPtr ret;

            //  Allocate a buffer the size of the string + 1 for 
            //  the NULL terminator. Scintilla always sets this
            //  regardless of the encoding
            byte[] buffer = new byte[length + 1];

            //  Get a direct pointer to the the head of the buffer
            //  to pass to the message along with the wParam. 
            //  Scintilla will fill the buffer with string data.
            fixed (byte* bp = buffer)
            {
                ret = SendMessageDirect(msg, wParam, (IntPtr)bp);

                //	If this string is NULL terminated we want to trim the
                //	NULL before converting it to a .NET String
                if (bp[length - 1] == 0)
                    length--;
            }


            //  We always assume UTF8 encoding to ensure maximum
            //  compatibility. Manually changing the encoding to 
            //  something else will cuase 2 Byte characters to
            //  be interpreted as junk.
            text = _encoding.GetString(buffer, 0, length);

            return (int)ret;
        }
 
        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (int,string)    
        /// Notes:
        ///  This helper method handles all messages that take
        ///  const char* as an input string in the lParam. In
        ///  some messages Scintilla expects a NULL terminated string
        ///  and in others it depends on the string length passed in
        ///  as wParam. This method handles both situations and will
        ///  NULL terminate the string either way. 
        /// 
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <param name="wParam">int wParam</param>
        /// <param name="lParam">string lParam</param>
        /// <returns></returns>
        internal unsafe int SendMessageDirect(uint msg, int wParam, string lParam)
        {
            //  Just as when retrieving we make to convert .NET's
            //  UTF-16 strings into a UTF-8 encoded byte array.
            fixed (byte* bp = _encoding.GetBytes(ZeroTerminated(lParam)))
                return (int)SendMessageDirect(msg, (IntPtr)wParam, (IntPtr)bp);
        }

        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (,string)    
        /// 
        /// Notes:
        ///  This helper method handles all messages that take
        ///  const char* as an input string in the lParam. In
        ///  some messages Scintilla expects a NULL terminated string
        ///  and in others it depends on the string length passed in
        ///  as wParam. This method handles both situations and will
        ///  NULL terminate the string either way. 
        /// 
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <param name="NULL">always pass null--Unused parameter</param>
        /// <param name="lParam">string lParam</param>
        /// <returns></returns>
        internal unsafe int SendMessageDirect(uint msg, VOID NULL, string lParam)
        {
            //  Just as when retrieving we make to convert .NET's
            //  UTF-16 strings into a UTF-8 encoded byte array.
            fixed (byte* bp = _encoding.GetBytes(ZeroTerminated(lParam)))
                return (int)SendMessageDirect(msg, IntPtr.Zero, (IntPtr)bp);
        }

        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (string,string)    
        /// 
        /// Notes:
        ///    Used by SCI_SETPROPERTY
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <param name="wParam">string wParam</param>
        /// <param name="lParam">string lParam</param>
        /// <returns></returns>
        internal unsafe int SendMessageDirect(uint msg, string wParam, string lParam)
        {
            fixed (byte* bpw = _encoding.GetBytes(ZeroTerminated(wParam)))
            fixed (byte* bpl = _encoding.GetBytes(ZeroTerminated(lParam)))
                return (int)SendMessageDirect(msg, (IntPtr)bpw, (IntPtr)bpl);
        }

        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (string,stringresult)    
        /// 
        /// Notes:
        ///  This one is used specifically by SCI_GETPROPERTY and SCI_GETPROPERTYEXPANDED
        ///  so it assumes it's usage
        /// 
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <param name="wParam">string wParam</param>
        /// <param name="stringResult">Stringresult output</param>
        /// <returns></returns>
        internal unsafe int SendMessageDirect(uint msg, string wParam, out string stringResult)
        {
            IntPtr ret;

            fixed (byte* bpw = _encoding.GetBytes(ZeroTerminated(wParam)))
            {
                int length = (int)SendMessageDirect(msg, (IntPtr)bpw, IntPtr.Zero);


                byte[] buffer = new byte[length + 1];

                fixed (byte* bpl = buffer)
                    ret = SendMessageDirect(msg, (IntPtr)bpw, (IntPtr)bpl);

                stringResult = _encoding.GetString(buffer, 0, length);
            }

            return (int)ret;
        }
   
        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (string,int)    
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <param name="wParam">string wParam</param>
        /// <param name="lParam">int lParam</param>
        /// <returns></returns>
        internal unsafe int SendMessageDirect(uint msg, string wParam, int lParam)
        {
            fixed (byte* bp = _encoding.GetBytes(ZeroTerminated(wParam)))
                return (int)SendMessageDirect(msg, (IntPtr)bp, (IntPtr)lParam);
        }

        /// <summary>
        /// Handles Scintilla Call Style:
        ///    (string,)    
        /// </summary>
        /// <param name="msg">Scintilla Message Number</param>
        /// <param name="wParam">string wParam</param>
        /// <returns></returns>
        internal unsafe int SendMessageDirect(uint msg, string wParam)
        {
            fixed (byte* bp = _encoding.GetBytes(ZeroTerminated(wParam)))
                return (int)SendMessageDirect(msg, (IntPtr)bp, IntPtr.Zero);
        }

        private static String ZeroTerminated(string param)
        {
            if (string.IsNullOrEmpty(param))
                return "\0";
            else if (!param.EndsWith("\0"))
                return param + "\0";
            return param;
        }
        #endregion

        #region Hand crafted members

		// Function void AddStyledText(int,cells) skipped.
		unsafe public void AddStyledText(int length, byte[] s)
		{
			fixed(byte* bp = s)
				SendMessageDirect(2002, (IntPtr)length, (IntPtr)bp);
		}


		// Function int GetStyledText(,textrange) skipped.
		unsafe public void GetStyledText(ref TextRange tr)
		{
			fixed(TextRange* trp = &tr)
				SendMessageDirect(2015, IntPtr.Zero, (IntPtr)trp);
		}

		// Function position FindText(int,findtext) skipped.
		unsafe public int FindText(int searchFlags, ref TextToFind ttf)
		{
			fixed(TextToFind* ttfp = &ttf)
				return (int)SendMessageDirect(2150, IntPtr.Zero, (IntPtr)ttfp);
		}

        /// <summary>
        /// Prints the text to the device given in the RangeToFormat struct (SCI_FORMATRANGE 2151)
        /// </summary>
        /// <param name="bDraw">Whether or not to actually draw the text to the device</param>
        /// <param name="oRangeToFormat">Provides the device to draw to as well as the bounds</param>
        /// <returns>End position of the text that was printed</returns>
		unsafe public int FormatRange(bool bDraw, ref RangeToFormat oRangeToFormat)
		{
            fixed (RangeToFormat* pRangeToFormat = &oRangeToFormat) {
                return (int) SendMessageDirect(2151, (IntPtr) (bDraw ? 1 : 0), (IntPtr) pRangeToFormat);
            }
		}

		// Function int GetTextRange(,textrange) skipped.
		unsafe public int GetTextRange(ref TextRange tr)
		{
			fixed(TextRange* trp = &tr)
				return (int)SendMessageDirect(2162, IntPtr.Zero, (IntPtr)trp);
		}

		public char CharAt(int position)
		{
			return (char)SendMessageDirect(2007, position, 0);
		}

		public IntPtr DocPointer()
		{
			return SendMessageDirect(2357, IntPtr.Zero, IntPtr.Zero);
		}

		public IntPtr CreateDocument()
		{
			return SendMessageDirect(2375, IntPtr.Zero, IntPtr.Zero);
		}

		public void AddRefDocument(IntPtr pDoc)
		{
			SendMessageDirect(2376, IntPtr.Zero, pDoc);
		}

		public void ReleaseDocument(IntPtr pDoc)
		{
			SendMessageDirect(2377, IntPtr.Zero, pDoc);
		}

		public void AssignCmdKey(System.Windows.Forms.Keys keyDefinition, uint sciCommand)
		{
			SendMessageDirect(2070, (int)keyDefinition, (int)sciCommand);
		}

		public void ClearCmdKey(System.Windows.Forms.Keys keyDefinition)
		{
			SendMessageDirect(2071, (int)keyDefinition, 0);
        }

        /// <summary>
        /// Retrieve all the text in the document. Returns number of characters retrieved. 
        /// </summary>
        public virtual string GetText()
        {
            string result;
            int length = SendMessageDirect(2182, 0, 0);
            this.SendMessageDirect(2182, length, out result);
            return result;
        }


        /// <summary>
        /// Get the code page used to interpret the bytes of the document as characters. 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int CodePage
        {
            get
            {
                return this.SendMessageDirect(2137);
            }
            set
            {
                this.SendMessageDirect(2037, value);
                this._encoding = Encoding.GetEncoding(value);
            }
        }

        /// <summary>
        /// Are white space characters currently visible? Returns one of SCWS_* constants. 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Enums.WhiteSpace ViewWhitespace
        {
            get
            {
                return (Enums.WhiteSpace)this.SendMessageDirect(2020);
            }
            set
            {
                this.SendMessageDirect(2021, (int)value);
            }
        }

        /// <summary>
        /// Retrieve the current end of line mode - one of CRLF, CR, or LF. 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Enums.EndOfLine EndOfLineMode
        {
            get
            {
                return (Enums.EndOfLine)this.SendMessageDirect(2030);
            }
            set
            {
                this.SendMessageDirect(2031, (int)value);
            }
        }

        /// <summary>
        /// Convert all line endings in the document to one mode.
        /// </summary>
        public virtual void ConvertEOLs(Enums.EndOfLine eolMode)
        {
            this.SendMessageDirect(2029, (int) eolMode);
        }

        /// <summary>
        /// Set the symbol used for a particular marker number.
        /// </summary>
        public virtual void MarkerDefine(int markerNumber, Enums.MarkerSymbol markerSymbol)
        {
            this.SendMessageDirect(2040, markerNumber, (int) markerSymbol);
        }

        /// <summary>
        /// Set the symbol used for a particular marker outline.
        /// </summary>
        public virtual void MarkerDefine(MarkerOutline markerOutline, Enums.MarkerSymbol markerSymbol)
        {
            this.MarkerDefine((int)markerOutline, markerSymbol);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="margin"></param>
        /// <param name="marginType"></param>
        public virtual void MarginTypeN(int margin, MarginType marginType)
        {
            this.MarginTypeN(margin, (int)marginType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useSetting"></param>
        /// <param name="back"></param>
        public virtual void SetFoldMarginColor(bool useSetting, Color back)
        {
            this.SetFoldMarginColor(useSetting, Utilities.ColorToRgb(back));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useSetting"></param>
        /// <param name="fore"></param>
        public virtual void SetFoldMarginHiColor(bool useSetting, Color fore)
        {
            this.SetFoldMarginHiColor(useSetting, Utilities.ColorToRgb(fore));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useSetting"></param>
        /// <param name="back"></param>
        public virtual void SetSelectionBackground(bool useSetting, Color back)
        {
            this.SetSelectionBackground(useSetting, Utilities.ColorToRgb(back));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="style"></param>
        /// <param name="fore"></param>
        public virtual void StyleSetFore(int style, Color fore)
        {
            this.StyleSetFore(style, Utilities.ColorToRgb(fore));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="style"></param>
        /// <param name="back"></param>
        public virtual void StyleSetBack(int style, Color back)
        {
            this.StyleSetBack(style, Utilities.ColorToRgb(back));
        }

        /// <summary>
        /// Set the character set of the font in a style.
        /// </summary>
        public void StyleSetCharacterSet(int style, Enums.CharacterSet characterSet)
        {
            this.SendMessageDirect(2066, style, (int)characterSet);
        }

        /// <summary>
        /// Set a style to be mixed case, or to force upper or lower case.
        /// </summary>
        public void StyleSetCase(int style, Enums.CaseVisible caseForce)
        {
            this.SendMessageDirect(2060, style, (int)caseForce);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public readonly Collection<IndicatorStyle> IndicatorStyle;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public readonly IntCollection IndicatorForegroundColor;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public readonly CachingIntCollection MarkerBackgroundColor;
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public readonly CachingIntCollection MarkerForegroundColor;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public readonly ReadOnlyStringCollection Line;

        #endregion

        #region Configuration
        public string ConfigurationLanguage
        {
            set
            {
                this._configurationLanguage = value;
                if (_configure != null)
                {
                    _configure(this, value);
                }
            }
            get { return _configurationLanguage;  }
        }

        public ScintillaConfigureDelegate Configure
        {
            set
            {
                this._configure = value;
            }
        }
        #endregion

        #region Margin Click Event Members to enable folding

        internal void EnableMarginClickFold()
        {
            MarginClick += new EventHandler<MarginClickEventArgs>(ScintillaControl_MarginClick);
        }

        internal void DisableMarginClickFold()
        {
            MarginClick -= new EventHandler<MarginClickEventArgs>(ScintillaControl_MarginClick);
        }

        private void ScintillaControl_MarginClick(object sender, MarginClickEventArgs e)
        {
            if (e.Margin == 1)
            {
                int lineNumber = this.LineFromPosition(e.Position);
                ToggleFold(lineNumber);
            }
        }

        #endregion

        #region Smart indenting support
        /// <summary>
        /// Enables the Smart Indenter so that On enter, it indents the next line.
        /// </summary>
        public Enums.SmartIndent SmartIndentType
        {
            get { return _smartIndentType; }
            set
            {
                _smartIndentType = value;

                if (_smartIndentType != SmartIndent.None)
                {
                    if (_smartIndenting == null)
                    {
                        _smartIndenting = new EventHandler<CharAddedEventArgs>(SmartIndenting_CharAdded);
                        CharAdded += SmartIndenting_CharAdded;
                    }
                }
                else
                {
                    if (_smartIndenting != null)
                    {
                        CharAdded -= SmartIndenting_CharAdded;
                        _smartIndenting = null;
                    }
                }
            }
        }

        /// <summary>
        /// If Smart Indenting is enabled, this delegate will be added to the CharAdded multicast event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SmartIndenting_CharAdded(object sender, CharAddedEventArgs e)
        {
            char ch = e.Ch;
            char newline = (EndOfLineMode == Enums.EndOfLine.CR) ? '\r' : '\n';

            switch (SmartIndentType)
            {
                case Enums.SmartIndent.None:
                    return;
                case Enums.SmartIndent.Simple:
                    if (ch == newline)
                    {
                        int curLine = LineFromPosition(CurrentPos);
                        int previousIndent = this.LineIndentation(curLine - 1);
                        IndentLine(curLine, previousIndent);
                        int position = LineIndentPosition(curLine);
                        SetSelection(position, position);
                    }
                    break;
                case Enums.SmartIndent.CPP:
                case Enums.SmartIndent.CPP2:
                    if (ch == newline)
                    {
                        int curLine = LineFromPosition(CurrentPos);
                        int tempLine = curLine;
                        int previousIndent;
                        string tempText;

                        do
                        {
                            
                            previousIndent = LineIndentation(tempLine - 1);
                            tempText = this.Line[tempLine - 1].Trim();
                            if (tempText.Length == 0) previousIndent = -1;
                            tempLine--;
                        }
                        while ((tempLine > 1) && (previousIndent < 0));
                        if (tempText.EndsWith("{"))
                        {
                            int bracePos = CurrentPos - 1;
                            while (bracePos > 0 && CharAt(bracePos) != '{') bracePos--;
                            if (bracePos > 0 && BaseStyleAt(bracePos) == 10)
                                previousIndent += TabWidth;
                        }
                        IndentLine(curLine, previousIndent);
                        int position = LineIndentPosition(curLine);
                        SetSelection(position, position);
                    }
                    else if (ch == '}')
                    {
                        int position = CurrentPos;
                        int curLine = LineFromPosition(position);
                        int previousIndent = LineIndentation(curLine - 1);
                        int match = SafeBraceMatch(position - 1);
                        if (match != -1)
                        {
                            previousIndent = LineIndentation(LineFromPosition(match));
                            IndentLine(curLine, previousIndent);
                        }
                    }
                    break;
                case Enums.SmartIndent.Custom:
                    if (ch == newline)
                    {
                        if (SmartIndentCustomAction != null) SmartIndentCustomAction(this, e);
                    }
                    break;
            }
        }

        /// <summary>
        /// For Custom Smart Indenting, assign a handler to this delegate property.
        /// </summary>
        public EventHandler<CharAddedEventArgs> SmartIndentCustomAction;

        /// <summary>
        /// Smart Indenting helper method
        /// </summary>
        /// <param name="line"></param>
        /// <param name="indent"></param>
        private void IndentLine(int line, int indent)
        {
            if (indent < 0)
            {
                return;
            }

            int selStart = this.SelectionStart;
            int selEnd = this.SelectionEnd;

            int posBefore = this.LineIndentPosition(line);
            this.LineIndentation(line, indent);

            int posAfter = LineIndentPosition(line);
            int posDifference = posAfter - posBefore;

            if (posAfter > posBefore)
            {
                // Move selection on
                if (selStart >= posBefore)
                {
                    selStart += posDifference;
                }

                if (selEnd >= posBefore)
                {
                    selEnd += posDifference;
                }
            }
            else if (posAfter < posBefore)
            {
                // Move selection back
                if (selStart >= posAfter)
                {
                    if (selStart >= posBefore)
                        selStart += posDifference;
                    else
                        selStart = posAfter;
                }
                if (selEnd >= posAfter)
                {
                    if (selEnd >= posBefore)
                        selEnd += posDifference;
                    else
                        selEnd = posAfter;
                }
            }

            this.SetSelection(selStart, selEnd);
        }
        #endregion

        #region Block/Brace Matching
        /// <summary>
        /// Enables the brace matching from current position.
        /// </summary>
        public bool IsBraceMatching
        {
            get { return _isBraceMatching; }
            set { _isBraceMatching = value; }
        }

        /// <summary>
        /// Provides the support for code block selection
        /// </summary>
        private void scintillaControlBlockSelect_DoubleClick(object obj, EventArgs args)
        {
            if (_isBraceMatching)
            {
                int position = CurrentPos - 1,
                       bracePosStart = -1,
                       bracePosEnd = -1;

                char character = (char)CharAt(position);

                switch (character)
                {
                    case '{':
                    case '(':
                    case '[':
                        if (!this.PositionIsOnComment(position))
                        {
                            bracePosStart = position;
                            bracePosEnd = BraceMatch(position) + 1;
                            SetSelection(bracePosStart, bracePosEnd);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Provides the support for brace matching
        /// </summary>
        private void scintillaControlBraceMatch_UpdateUI(object obj, UpdateUIEventArgs args)
        {
            if (_isBraceMatching && (this.GetSelectedText().Length == 0))
            {
                int position = CurrentPos - 1,
                    bracePosStart = -1,
                    bracePosEnd = -1;

                char character = (char)CharAt(position);

                switch (character)
                {
                    case '{':
                    case '}':
                    case '(':
                    case ')':
                    case '[':
                    case ']':
                        if (!this.PositionIsOnComment(position))
                        {
                            bracePosStart = position;
                            bracePosEnd = BraceMatch(position);
                        }
                        break;
                    default:
                        position = CurrentPos;
                        character = (char)CharAt(position);
                        break;
                }

                BraceHighlight(bracePosStart, bracePosEnd);
            }
        }

        /// <summary>
        /// Custom way to find the matching brace when BraceMatch() does not work
        /// </summary>
        public int SafeBraceMatch(int position)
        {
            int match = this.CharAt(position);
            int toMatch = 0;
            int length = TextLength;
            int ch;
            int sub = 0;
            int lexer = Lexer;
            Colorize(0, -1);
            bool comment = PositionIsOnComment(position, lexer);
            switch (match)
            {
                case '{':
                    toMatch = '}';
                    goto down;
                case '(':
                    toMatch = ')';
                    goto down;
                case '[':
                    toMatch = ']';
                    goto down;
                case '}':
                    toMatch = '{';
                    goto up;
                case ')':
                    toMatch = '(';
                    goto up;
                case ']':
                    toMatch = '[';
                    goto up;
            }
            return -1;
        // search up
        up:
            while (position >= 0)
            {
                position--;
                ch = CharAt(position);
                if (ch == match)
                {
                    if (comment == PositionIsOnComment(position, lexer)) sub++;
                }
                else if (ch == toMatch && comment == PositionIsOnComment(position, lexer))
                {
                    sub--;
                    if (sub < 0) return position;
                }
            }
            return -1;
        // search down
        down:
            while (position < length)
            {
                position++;
                ch = CharAt(position);
                if (ch == match)
                {
                    if (comment == PositionIsOnComment(position, lexer)) sub++;
                }
                else if (ch == toMatch && comment == PositionIsOnComment(position, lexer))
                {
                    sub--;
                    if (sub < 0) return position;
                }
            }
            return -1;
        }

        #endregion

        #region Add Shortcuts from form to Scintilla control

        public virtual void AddShortcuts(Form parentForm)
        {
            if (parentForm != null)
            {
                if (parentForm.MainMenuStrip != null)
                {
                    AddShortcuts(parentForm.MainMenuStrip.Items);
                }
                else if (parentForm.Menu != null)
                {
                    AddShortcuts(parentForm.Menu.MenuItems);
                }
            }
        }

        public virtual void AddShortcuts(Menu.MenuItemCollection m)
        {
            foreach (MenuItem mi in m)
            {
                if (mi != null)
                {
                    Shortcut sh = mi.Shortcut;
                    if (sh != Shortcut.None)
                    {
                        AddIgnoredKey(sh);
                    }

                    if (mi.MenuItems.Count > 0)
                    {
                        AddShortcuts(mi.MenuItems);
                    }
                }
            }
        }

        public virtual void AddShortcuts(ToolStripItemCollection m)
        {
            foreach (ToolStripItem tmi in m)
            {
                if (tmi is ToolStripMenuItem)
                {
                    ToolStripMenuItem mi = tmi as ToolStripMenuItem;
                    if (mi.ShortcutKeys != System.Windows.Forms.Keys.None)
                    {
                        AddIgnoredKey(mi.ShortcutKeys);
                    }

                    if (mi.DropDownItems.Count > 0)
                    {
                        AddShortcuts(mi.DropDownItems);
                    }
                }
            }
        }

        public virtual void AddIgnoredKey(System.Windows.Forms.Shortcut shortcutkey)
        {
            int key = (int)shortcutkey;
            if (!this._ignoredKeys.ContainsKey(key))
                this._ignoredKeys.Add(key, key);
        }

        public virtual void AddIgnoredKey(System.Windows.Forms.Keys shortcutkey)
        {
            int key = (int)shortcutkey;
            if (!this._ignoredKeys.ContainsKey(key))
                this._ignoredKeys.Add(key, key);
        }

        public virtual void ClearIgnoredKeys() 
        {
            this._ignoredKeys.Clear();
        }

        public override bool PreProcessMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_KEYDOWN:
                    {
                        if (_ignoredKeys.ContainsKey((int)Control.ModifierKeys + (int)m.WParam))
                            return base.PreProcessMessage(ref m);
                    }
                    break;
            }
            return false;
        }

        #endregion

        #region Miscellaneous Override Members
        override protected CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                //	Otherwise Scintilla won't paint. When UserPaint is set to
                //	true the base Class (Control) eats the WM_PAINT message.
                //	Of course when this set to false we can't use the Paint
                //	events. This is why I'm relying on the Paint notification
                //	sent from scintilla to paint the Marker Arrows.
                SetStyle(ControlStyles.UserPaint, false);

                //	Registers the Scintilla Window Class
                //	I'm relying on the fact that a version specific renamed
                //	SciLexer exists either in the Current Dir or a global path
                //	(See LoadLibrary Windows API Search Rules)
                NativeMethods.LoadLibrary(_sciLexerDllName);

                //	Tell Windows Forms to create a Scintilla
                //	derived Window Class for this control
                CreateParams cp = base.CreateParams;
                cp.ClassName = "Scintilla";
                return cp;
            }
        }

        public override string Text
        {
            get
            {
                return this.GetText();
            }
            set
            {
                this.SetText(value);
            }
        }
        #endregion

        #region Miscellaneous Custom Members

        unsafe string MarshalStr(IntPtr p)
        {
            // instead of 
            // System.Runtime.InteropServices.Marshal.PtrToStringAuto(p)
            sbyte* ps = (sbyte*)p;
            int size = 0;
            for (; ps[size] != 0; ++size)
                ;
            return new String(ps, 0, size);
        }

        /// <summary>
        /// Adds a line end marker to the end of the document
        /// </summary>
        public void AddLastLineEnd()
        {
            string eolMarker = "\r\n";
            if (this.EndOfLineMode == EndOfLine.CR) eolMarker = "\r";
            else if (this.EndOfLineMode == EndOfLine.LF) eolMarker = "\n";

            if (!this.Text.EndsWith(eolMarker))
            {
                this.TargetStart = this.TargetEnd = this.TextLength;
                this.ReplaceTarget(eolMarker);
            }
        }

        /// <summary>
        /// Removes trailing spaces from each line
        /// </summary>
        public void StripTrailingSpaces()
        {
            this.BeginUndoAction();
            int maxLines = this.LineCount;
            for (int line = 0; line < maxLines; line++)
            {
                int lineStart = this.PositionFromLine(line);
                int lineEnd = this.LineEndPosition(line);
                int i = lineEnd - 1;
                char ch = (char)this.CharAt(i);
                while ((i >= lineStart) && ((ch == ' ') || (ch == '\t')))
                {
                    i--;
                    ch = (char)this.CharAt(i);
                }
                if (i == lineStart - 1)
                {
                    ch = (char)this.CharAt(i + 1);
                    while (i < lineEnd && ch == '\t')
                    {
                        i++;
                        ch = (char)this.CharAt(i + 1);
                    }
                }
                if (i < (lineEnd - 1))
                {
                    this.TargetStart = i + 1;
                    this.TargetEnd = lineEnd;
                    this.ReplaceTarget(string.Empty);
                }
            }
            this.EndUndoAction();
        }

        /// <summary>
        /// Checks that if the specified position is on comment.
        /// </summary>
        public bool PositionIsOnComment(int position)
        {
            this.Colorize(0, -1);
            return PositionIsOnComment(position, Lexer);
        }

        /// <summary>
        /// Checks that if the specified position is on comment.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="lexer"></param>
        /// <returns></returns>
        public bool PositionIsOnComment(int position, int lexer)
        {
            int style = BaseStyleAt(position);
            if ((lexer == 2 || lexer == 21)
                && style == 1
                || style == 12)
            {
                return true; // python or lisp
            }
            else if ((Lexer == 3 || lexer == 18 || lexer == 25 || lexer == 27)
                && style == 1
                || style == 2
                || style == 3
                || style == 15
                || style == 17
                || style == 18)
            {
                return true; // cpp, tcl, bullant or pascal
            }
            else if ((lexer == 4 || lexer == 5)
                && style == 9
                || style == 20
                || style == 29
                || style == 30
                || style == 42
                || style == 43
                || style == 44
                || style == 57
                || style == 58
                || style == 59
                || style == 72
                || style == 82
                || style == 92
                || style == 107
                || style == 124
                || style == 125)
            {
                return true; // html or xml
            }
            else if ((lexer == 6 || lexer == 22 || lexer == 45 || lexer == 62)
                && style == 2)
            {
                return true; // perl, bash, clarion/clw or ruby
            }
            else if ((Lexer == 7)
                && style == 1
                || style == 2
                || style == 3
                || style == 13
                || style == 15
                || style == 17
                || style == 18)
            {
                return true; // sql
            }
            else if ((lexer == 8 || lexer == 9 || lexer == 11 || lexer == 12 || lexer == 16 || lexer == 17 || lexer == 19 || lexer == 23 || lexer == 24 || lexer == 26 || lexer == 28 || lexer == 32 || lexer == 36 || lexer == 37 || lexer == 40 || lexer == 44 || lexer == 48 || lexer == 51 || lexer == 53 || lexer == 54 || lexer == 57 || lexer == 63)
                && style == 1)
            {
                return true; // asn1, vb, diff, batch, makefile, avenue, eiffel, eiffelkw, vbscript, matlab, crontab, fortran, f77, lout, mmixal, yaml, powerbasic, erlang, octave, kix or properties
            }
            else if ((lexer == 14)
                && style == 4)
            {
                return true; // latex
            }
            else if ((lexer == 15 || lexer == 41 || lexer == 56)
                && style == 1
                || style == 2
                || style == 3)
            {
                return true; // lua, verilog or escript
            }
            else if ((lexer == 20)
                && style == 10)
            {
                return true; // ada
            }
            else if ((lexer == 31 || lexer == 39 || lexer == 42 || lexer == 52 || lexer == 55 || lexer == 58 || lexer == 60 || lexer == 61 || lexer == 64 || lexer == 71)
                && style == 1
                || style == 2)
            {
                return true; // au3, apdl, baan, ps, mssql, rebol, forth, gui4cli, vhdl or pov
            }
            else if ((lexer == 34)
                && style == 1
                || style == 11)
            {
                return true; // asm
            }
            else if ((lexer == 43)
                && style == 1
                || style == 18)
            {
                return true; // nsis
            }
            else if ((lexer == 59)
                && style == 2
                || style == 3)
            {
                return true; // specman
            }
            else if ((lexer == 70)
                && style == 3
                || style == 4)
            {
                return true; // tads3
            }
            else if ((lexer == 74)
                && style == 1
                || style == 9)
            {
                return true; // csound
            }
            else if ((lexer == 65)
                && style == 12
                || style == 13
                || style == 14
                || style == 15)
            {
                return true; // caml
            }
            else if ((lexer == 68)
                && style == 13
                || style == 14
                || style == 15
                || style == 16)
            {
                return true; // haskell
            }
            else if ((lexer == 73)
                && style == 1
                || style == 2
                || style == 3
                || style == 4
                || style == 5
                || style == 6)
            {
                return true; // flagship
            }
            else if ((lexer == 72)
                && style == 3)
            {
                return true; // smalltalk
            }
            else if ((lexer == 38)
                && style == 9)
            {
                return true; // css
            }
            return false;
        }

        /// <summary>
        /// Expands all folds
        /// </summary>
        public void ExpandAllFolds()
        {
            for (int i = 0; i < LineCount; i++)
            {
                FoldExpanded(i, true);
                ShowLines(i + 1, i + 1);
            }
        }

        /// <summary>
        /// Collapses all folds
        /// </summary>
        public void CollapseAllFolds()
        {
            for (int i = 0; i < LineCount; i++)
            {
                int maxSubOrd = LastChild(i, -1);
                FoldExpanded(i, false);
                HideLines(i + 1, maxSubOrd);
            }
        }

        /// <summary>
        /// Gets a word from the specified position
        /// </summary>
        public string GetWordFromPosition(int position)
        {
            try
            {
                int startPosition = this.WordStartPosition(position - 1, true);
                int endPosition = this.WordEndPosition(position - 1, true);
                string keyword = this.Text.Substring(startPosition, endPosition - startPosition);
                if (keyword == "" || keyword == " ") return null;
                return keyword.Trim();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the base style (without indicators) byte at the position.
        /// </summary>
        public int BaseStyleAt(int pos)
        {
            return (int)(this.StyleAt(pos) & ((1 << this.StyleBits) - 1));
        }

		public Range Range(int position) 
		{
			return Range(position, position);
		}

		public Range Range(int start, int end)
		{
			return new Range(start, end, this);
		}
		#endregion

        #region ISupportInitialize Members
        private bool _isInitializing = false;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool IsInitializing
        {
            get
            {
                return _isInitializing;
            }
            set
            {
                _isInitializing = value;
            }
        }

        public void BeginInit()
        {
            _isInitializing = true;
        }

        public void EndInit()
        {
            _isInitializing = false;
            //foreach (ScintillaHelperBase helper in _helpers)
            //{
            //    helper.Initialize();
            //}
        }

        #endregion

    }
}
