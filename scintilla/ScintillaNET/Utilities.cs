using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Scintilla.Enums;

namespace Scintilla
{
    public class Utilities
    {
        private static Dictionary<Lexer, Type> lexerEnumTypes = new Dictionary<Lexer, Type>();
 
        private Utilities()
        {
        }
        
        public static int ColorToRgb(Color c)
        {
            return c.R + (c.G << 8) + (c.B << 16);
        }

        public static Color RgbToColor(int color)
        {
            return Color.FromArgb(color & 0x0000ff, (color & 0x00ff00) >> 8, (color & 0xff0000) >> 16);
        }

        public static int SignedLoWord(IntPtr loWord)
        {
            return (short)((int)(long)loWord & 0xffff);
        }

		public static int SignedHiWord(IntPtr hiWord)
        {
            return (short)(((int)(long)hiWord >> 0x10) & 0xffff);
        }

		public unsafe static string PtrToStringUtf8(IntPtr ptr, int length)
        {
            if (ptr == IntPtr.Zero)
                return null;

            byte[] buff = new byte[length];
            Marshal.Copy(ptr, buff, 0, length);
            return System.Text.UTF8Encoding.UTF8.GetString(buff);
        }

        public static Type GetLexerEnumFromLexerType(Lexer lexerType)
        {
            if (lexerEnumTypes.ContainsKey(lexerType))
            {
                return lexerEnumTypes[lexerType];
            }
            else
            {
                Type lexerEnum = null;
                try
                {
                    lexerEnum = System.Reflection.Assembly.GetExecutingAssembly().GetType("Scintilla.Lexers." + lexerType.ToString());
                    lexerEnumTypes[lexerType] = lexerEnum;
                }
                catch
                {
                    lexerEnum = null;
                }

                return lexerEnum;
            }
        }
    }
}
 