using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Zeus;
using Zeus.ErrorHandling;

namespace MyGeneration
{
    public class MyGenError : IMyGenError
    {
        private string templateFileName;
        private string templateIdentifier;
        private DateTime dateTimeOccurred = DateTime.Now;
        private string uniqueIdentifier;
        private MyGenErrorClass errorClass = MyGenErrorClass.Application;
        private bool isWarning = false;
        private bool isRuntime = true;
        private bool isTemplateCodeSegment = true;
        private Guid errorGuid = Guid.NewGuid();
        private string errorNumber;
        private string errorType;
        private string sourceFile;
        private string sourceLine;
        private int lineNumber = 0;
        private int columnNumber = 0;
        private string message;
        private string detail;

        public static List<IMyGenError> CreateErrors(params Exception[] exceptions)
        {
            MyGenError error;
            List<IMyGenError> myGenErrors = new List<IMyGenError>();
            foreach (Exception loopex in exceptions)
            {
                Exception ex = loopex,
                    bex = ex.GetBaseException();

                if (ex is ZeusRuntimeException)
                {
                    ZeusRuntimeException zrex = ex as ZeusRuntimeException;
                    if (bex is ZeusExecutionException)
                    {
                        ZeusExecutionException zeex = bex as ZeusExecutionException;

                        foreach (IZeusExecutionError zee in zeex.Errors)
                        {
                            error = new MyGenError();
                            error.errorClass = MyGenErrorClass.Template;
                            error.templateFileName = zrex.Template.FilePath + zrex.Template.FileName;
                            error.TemplateIdentifier = zrex.Template.UniqueID;
                            error.IsTemplateCodeSegment = zrex.IsTemplateScript;
                            
                            PopulateErrorFromZeusExecError(error, zee);
                            
                            myGenErrors.Add(error);
                        }
                    }
                    else
                    {
                        error = new MyGenError();
                        error.errorClass = MyGenErrorClass.Template;
                        error.templateFileName = zrex.Template.FilePath + zrex.Template.FileName;
                        error.TemplateIdentifier = zrex.Template.UniqueID;
                        error.IsTemplateCodeSegment = zrex.IsTemplateScript;
                        error.isRuntime = true;

                        PopulateErrorFromException(error, bex);
                        
                        myGenErrors.Add(error);
                    }
                }
                else if (ex is ZeusExecutionException)
                {                    
                    ZeusExecutionException zeex = ex as ZeusExecutionException;

                    foreach (IZeusExecutionError zee in zeex.Errors)
                    {
                        error = new MyGenError();
                        error.errorClass = MyGenErrorClass.Template;
                        error.templateFileName = zeex.Template.FilePath + zeex.Template.FileName;
                        error.TemplateIdentifier = zeex.Template.UniqueID;
                        error.IsTemplateCodeSegment = zeex.IsTemplateScript;

                        PopulateErrorFromZeusExecError(error, zee);

                        myGenErrors.Add(error);
                    }
                }
                else
                {
                    if (bex == null) bex = ex;

                    error = new MyGenError();
                    error.errorClass = MyGenErrorClass.Application;
                    PopulateErrorFromException(error, bex);
                    myGenErrors.Add(error);
                }
            }
            return myGenErrors;
        }

        private static void PopulateErrorFromZeusExecError(MyGenError error, IZeusExecutionError zee)
        {
            error.columnNumber = zee.Column;
            error.SourceFile = zee.FileName;
            error.isRuntime = zee.IsRuntime;
            error.IsWarning = zee.IsWarning;
            error.lineNumber = zee.Line;
            error.message = zee.Message;
            error.ErrorNumber = zee.Number;
            error.SourceLine = zee.Source;
            error.detail = zee.StackTrace;
        }

        public static void PopulateErrorFromException(MyGenError error, Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            error.errorType = ex.GetType().Name;
            error.message = ex.Message;
            error.detail = ex.StackTrace;
            error.SourceFile = ex.Source;
            
            StackTrace st = new StackTrace(ex);
            StackFrame[] sfs = st.GetFrames();
            foreach (StackFrame sf in sfs)
            {
                error.lineNumber = sf.GetFileLineNumber();
                error.ColumnNumber = sf.GetFileColumnNumber();
                error.SourceFile = sf.GetFileName();
                error.SourceLine = sf.GetMethod().Name;
                break;
            }
        }

        public MyGenError()
        {
        }

        public string TemplateFileName
        {
            get { return templateFileName; }
            set { templateFileName = value; }
        }

        public string TemplateIdentifier
        {
            get { return templateIdentifier; }
            set { templateIdentifier = value; }
        }

        public DateTime DateTimeOccurred
        {
            get { return dateTimeOccurred; }
            set { dateTimeOccurred = value; }
        }

        public string UniqueIdentifier
        {
            get { return uniqueIdentifier; }
            set { uniqueIdentifier = value; }
        }

        public MyGenErrorClass Class
        {
            get { return this.errorClass; }
            set { errorClass = value; }
        }

        public bool IsWarning
        {
            get { return isWarning; }
            set { isWarning = value; }
        }

        public bool IsRuntime
        {
            get { return isRuntime; }
            set { isRuntime = value; }
        }

        public bool IsTemplateCodeSegment
        {
            get { return isTemplateCodeSegment; }
            set { isTemplateCodeSegment = value; }
        }

        public Guid ErrorGuid
        {
            get { return errorGuid; }
            set { errorGuid = value; }
        }

        public string ErrorNumber
        {
            get { return errorNumber; }
            set { errorNumber = value; }
        }

        public string ErrorType
        {
            get { return errorType; }
            set { errorType = value; }
        }

        public string SourceFile
        {
            get { return sourceFile; }
            set { sourceFile = value; }
        }

        public string SourceLine
        {
            get { return sourceLine; }
            set { sourceLine = value; }
        }

        public int LineNumber
        {
            get { return lineNumber; }
            set { lineNumber = value; }
        }

        public int ColumnNumber
        {
            get { return columnNumber; }
            set { columnNumber = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public string Detail
        {
            get { return detail; }
            set { detail = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Date: ").AppendLine(this.DateTimeOccurred.ToString());
            if (TemplateFileName != null) sb.Append("TemplateFileName: ").AppendLine(TemplateFileName);
            if (TemplateIdentifier != null) sb.Append("TemplateIdentifier: ").AppendLine(TemplateIdentifier);
            sb.Append("Class: ").AppendLine(Class.ToString());
            sb.Append("IsWarning: ").AppendLine(IsWarning.ToString());
            sb.Append("IsRuntime: ").AppendLine(IsRuntime.ToString());
            sb.Append("IsTemplateCodeSegment: ").AppendLine(IsTemplateCodeSegment.ToString());
            if (ErrorNumber != null) sb.Append("ErrorNumber: ").AppendLine(ErrorNumber);
            if (ErrorType != null) sb.Append("ErrorType: ").AppendLine(ErrorType);
            if (SourceFile != null) sb.Append("SourceFile: ").AppendLine(SourceFile);
            if (SourceLine != null) sb.Append("SourceLine: ").AppendLine(SourceLine);
            sb.Append("LineNumber: ").AppendLine(LineNumber.ToString());
            sb.Append("ColumnNumber: ").AppendLine(ColumnNumber.ToString());
            if (Message != null) sb.Append("Message: ").AppendLine(Message);
            if (Detail != null) sb.Append("Detail: ").AppendLine(Detail);
            return sb.ToString();
        }
    }
}
