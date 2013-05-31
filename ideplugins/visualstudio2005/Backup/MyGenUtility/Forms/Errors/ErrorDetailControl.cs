using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MyGeneration.Forms
{
    public partial class ErrorDetailControl : UserControl
    {
        private IMyGenError error;

        public ErrorDetailControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
        }

        public void Update(IMyGenError error)
        {
            this.error = error;

            this.BindControl();
        }

        private void BindControl()
        {
            if (error.Class == MyGenErrorClass.Template)
            {
                this.labelTitle.Text = "Template Error";
                if (string.IsNullOrEmpty(error.TemplateFileName))
                {
                    this.textBoxFile.Text = error.SourceFile;
                }
                else
                {
                    this.textBoxFile.Text = error.TemplateFileName;
                }
            }
            else
            {
                this.labelTitle.Text = "Application Error";
                this.textBoxFile.Text = error.SourceFile;
            }
            this.textBoxClass.Text = error.Class.ToString();
            this.textBoxColumn.Text = error.ColumnNumber.ToString();
            this.textBoxDetail.Text = error.Detail;
            this.textBoxExceptionType.Text = error.ErrorType;
            this.textBoxLine.Text = error.LineNumber.ToString();
            this.textBoxMessage.Text = error.Message;
            this.textBoxDate.Text = error.DateTimeOccurred.ToString();
            this.textBoxSource.Text = error.SourceLine;

            this.checkBoxRuntime.Checked = error.IsRuntime;
            this.checkBoxWarning.Checked = error.IsWarning;
        }

        public string TextContent
        {
            get
            {
                return string.Empty;
            }
        }
    }
}
