using System;
using System.Collections;
using System.Xml;
using System.Text;

namespace Zeus
{
    public interface IZeusSavedTemplateInput
    {
        string TemplateUniqueID { get; set; }
        string TemplatePath { get; set; }
        string SavedObjectName { get; set; }
        IDictionary InputItemsI { get; set; }
        void BuildXML(XmlTextWriter xml);
        string ReadXML(XmlTextReader xr);
        void Execute(int timeout, ILog log);
        IZeusSavedTemplateInput CopyI();
    }
}
