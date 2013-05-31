using System;
using System.Collections;
using System.Collections.Generic;
using MyGeneration;

namespace Zeus
{
	public interface IZeusController
	{
        IZeusSavedInput CollectTemplateInput(IZeusContext context, string templatePath);
        IZeusSavedInput ExecuteTemplateAndCollectInput(IZeusContext context, string templatePath);
        void ExecuteTemplate(IZeusContext context, string templateFilePath);
        void ExecuteTemplate(IZeusContext context, string templatePath, string inputFilePath);
        void ExecuteProject(IZeusContext context, string projectFilePath);
        void ExecuteProjectModule(IZeusContext context, string projectFilePath, params string[] modules);
        List<IAppRelease> ReleaseList { get; }
	}

    public interface IAppRelease
    {
        string Title { get; }
        string Description { get; }
        string Author { get; }
        Version AppVersion { get; }
        Uri DownloadLink { get; }
        Uri ReleaseNotesLink { get; }
        DateTime Date { get; }
    }
}