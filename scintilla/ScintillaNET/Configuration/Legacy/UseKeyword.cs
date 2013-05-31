using System;
using System.Runtime;
using System.Xml.Serialization;

namespace Scintilla.Configuration.Legacy
{
    [SerializableAttribute()]
    public class UseKeyword : ConfigItem
    {
        [XmlAttributeAttribute()]
        public int key;

        [XmlAttributeAttribute("class")]
        public string cls;
    }
}
