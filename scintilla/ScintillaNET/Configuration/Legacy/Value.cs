using System;
using System.Runtime;
using System.Xml.Serialization;

namespace Scintilla.Configuration.Legacy
{
    [SerializableAttribute()]
    public class Value : ConfigItem
    {
        [XmlAttributeAttribute()]
        public string name;

        [XmlTextAttribute()]
        public string val;
    }
}
