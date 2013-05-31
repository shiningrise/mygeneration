using System;
using System.Runtime.InteropServices;
namespace Scintilla
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CharacterRange
    {
        public int cpMin;
        public int cpMax;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TextRange
    {
        public CharacterRange chrg;
        public IntPtr lpstrText;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TextToFind
    {
        public CharacterRange chrg;			// range to search
        public IntPtr lpstrText;			// the search pattern (zero terminated)
        public CharacterRange chrgText;		// returned as position of matching text
    }

    /// <summary>
    /// Struct used for specifying the printing bounds
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PrintRectangle 
    {
        /// <summary>
        /// Left X Bounds Coordinate
        /// </summary>
        public int Left;
        /// <summary>
        /// Top Y Bounds Coordinate
        /// </summary>
        public int Top;
        /// <summary>
        /// Right X Bounds Coordinate
        /// </summary>
        public int Right;
        /// <summary>
        /// Bottom Y Bounds Coordinate
        /// </summary>
        public int Bottom;

        public PrintRectangle(int iLeft, int iTop, int iRight, int iBottom) {
            Left = iLeft;
            Top = iTop;
            Right = iRight;
            Bottom = iBottom;
        }
    }

    /// <summary>
    /// Struct used for passing parameters to FormatRange()
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RangeToFormat
    {
        /// <summary>
        /// The HDC (device context) we print to
        /// </summary>
        public IntPtr hdc;
        /// <summary>
        /// The HDC we use for measuring (may be same as hdc)
        /// </summary>
        public IntPtr hdcTarget;
        /// <summary>
        /// Rectangle in which to print
        /// </summary>
        public PrintRectangle rc;
        /// <summary>
        /// Physically printable page size
        /// </summary>
        public PrintRectangle rcPage;
        /// <summary>
        /// Range of characters to print
        /// </summary>
        public CharacterRange chrg;
    }

    /// <summary>
    /// This matches the Win32 NMHDR structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NotifyHeader
    {
        public IntPtr hwndFrom;	// environment specific window handle/pointer
        public IntPtr idFrom;	// CtrlID of the window issuing the notification
        // public uint code;		// The SCN_* notification code
        public Enums.Events code;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SCNotification
    {
        internal NotifyHeader nmhdr;
        internal int position;			// SCN_STYLENEEDED, SCN_MODIFIED, SCN_DWELLSTART, SCN_DWELLEND, 
        // SCN_CALLTIPCLICK, SCN_HOTSPOTCLICK, SCN_HOTSPOTDOUBLECLICK
        internal char ch;					// SCN_CHARADDED, SCN_KEY
        internal int modifiers;			// SCN_KEY
        internal int modificationType;	// SCN_MODIFIED
        internal IntPtr text;				// SCN_MODIFIED
        internal int length;				// SCN_MODIFIED
        internal int linesAdded;			// SCN_MODIFIED
        internal int message;				// SCN_MACRORECORD
        internal IntPtr wParam;			// SCN_MACRORECORD
        internal IntPtr lParam;			// SCN_MACRORECORD
        internal int line;				// SCN_MODIFIED
        internal int foldLevelNow;		// SCN_MODIFIED
        internal int foldLevelPrev;		// SCN_MODIFIED
        internal int margin;				// SCN_MARGINCLICK
        internal int listType;			// SCN_USERLISTSELECTION
        internal int x;					// SCN_DWELLSTART, SCN_DWELLEND
        internal int y;					// SCN_DWELLSTART, SCN_DWELLEND
    }

	/// <summary>
	/// Expresses a position within the Editor by Line # of Column Offset.
	/// Use ScintillaControl.LineColumnFromPosition and 
	/// ScintillaControl.PositionFromLineColumn to converto to/from a Position
	/// (absolute position within the editor)
	/// </summary>
	public struct LineColumn
	{
		public int Line;
		public int Column;

		public LineColumn(int line, int column)
		{
			Line	= line;
			Column	= column;
		}
	}
}
