using System;
using System.Runtime;
using System.Xml.Serialization;

namespace Scintilla.Configuration.Legacy
{
    [SerializableAttribute()]
    public class include : ConfigItem
    {
        [XmlAttributeAttribute()]
        public string file;
    }
}
