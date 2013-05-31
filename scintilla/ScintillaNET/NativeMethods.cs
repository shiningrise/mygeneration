using System.Runtime.InteropServices;
using System;

namespace Scintilla
{
    public enum BeepType
    {
        Default = -1,
        Ok = 0x00000000,
        Error = 0x00000010,
        Question = 0x00000020,
        Warning = 0x00000030,
        Information = 0x00000040,
    }

    internal static class NativeMethods
    {
        internal const int WM_KEYDOWN = 0x0100;
        internal const int WM_DROPFILES = 0x0233;
        internal const int WM_NOTIFY = 0x004e;

        
        [DllImport("shell32.dll")]
        internal static extern int DragQueryFileA(
                IntPtr hDrop,
                uint idx,
                IntPtr buff,
                int sz
        );

        [DllImport("shell32.dll")]
        internal static extern int DragFinish(
                IntPtr hDrop
        );

        [DllImport("shell32.dll")]
        internal static extern void DragAcceptFiles(
                IntPtr hwnd,
                int accept
        );
        
        [DllImport("kernel32")]
        internal extern static IntPtr LoadLibrary(string lpLibFileName);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MessageBeep(BeepType type);

        [DllImport("kernel32.dll")]
        internal static extern bool Beep(int frequency, int time);

        [DllImport("user32.dll")]
        internal static extern IntPtr CreateWindowEx(uint dwExStyle, string lpClassName, string lpWindowName, uint dwStyle, int x, int y, int width, int height, IntPtr hWndParent, int hMenu, IntPtr hInstance, string lpParam);

        [DllImport("kernel32.dll", EntryPoint = "SendMessage")]
        internal static extern int SendMessageStr(IntPtr hWnd, int message, int data, string s);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetFocus(IntPtr hwnd);
    }
}

