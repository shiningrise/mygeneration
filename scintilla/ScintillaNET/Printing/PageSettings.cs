using System;
using System.Drawing.Printing;

using Scintilla.Enums;

namespace Scintilla.Printing {
    /// <summary>
    /// ScintillaNET derived class for handling printed page settings.  It holds information 
    /// on how and what to print in the header and footer of pages.
    /// </summary>
    public class PageSettings : System.Drawing.Printing.PageSettings {
        /// <summary>
        /// Default header style used when no header is provided.
        /// </summary>
        public static readonly PageInformation DefaultHeader = new PageInformation(PageInformationBorder.Bottom, InformationType.DocumentName, InformationType.Nothing, InformationType.PageNumber);
        /// <summary>
        /// Default footer style used when no footer is provided.
        /// </summary>
        public static readonly PageInformation DefaultFooter = new PageInformation(PageInformationBorder.Top, InformationType.Nothing, InformationType.Nothing, InformationType.Nothing);

        private PageInformation m_oHeader;
        private PageInformation m_oFooter;
        private short m_sFontMagification;
        private PrintOption m_eColorMode;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PageSettings() {
            m_oHeader = DefaultHeader;
            m_oFooter = DefaultFooter;
            m_sFontMagification = 0;
            m_eColorMode = PrintOption.Normal;

            // Set default margins to 1/2 inch (50/100ths)
            base.Margins.Top = 50;
            base.Margins.Left = 50;
            base.Margins.Right = 50;
            base.Margins.Bottom = 50;
        }

        #region Properties

        /// <summary>
        /// Page Information printed in header of the page
        /// </summary>
        public PageInformation Header {
            get { return m_oHeader; }
            set { m_oHeader = value; }
        }

        /// <summary>
        /// Page Information printed in the footer of the page
        /// </summary>
        public PageInformation Footer {
            get { return m_oFooter; }
            set { m_oFooter = value; }
        }

        /// <summary>
        /// Number of points to add or subtract to the size of each screen font during printing
        /// </summary>
        public short FontMagification {
            get { return m_sFontMagification; }
            set { m_sFontMagification = value; }
        }

        /// <summary>
        /// Method used to render colored text on a printer
        /// </summary>
        public PrintOption ColorMode {
            get { return m_eColorMode; }
            set { m_eColorMode = value; }
        }

        #endregion

    }
}
