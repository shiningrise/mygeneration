using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;

namespace MyGeneration
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application. Set the global application exception handlers here and load up the parent form.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            MyGenerationMDI mdi;
            try
            {
                mdi = new MyGenerationMDI(Application.StartupPath, args);

                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(mdi.UnhandledExceptions);
                Application.ThreadException += new ThreadExceptionEventHandler(mdi.OnThreadException);
            }
            catch (Exception ex)
            {
                MyGeneration.CrazyErrors.ExceptionDialog ed = new MyGeneration.CrazyErrors.ExceptionDialog(ex);
                ed.ShowDialog();

                mdi = null;
            }

            if (mdi != null)
            {
                Application.Run(mdi);
            }
            else
            {
                Application.Exit();
            }
        }

    }
}