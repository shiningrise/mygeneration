using System;
using System.Runtime;
using System.Xml.Serialization;

namespace Scintilla.Configuration.Legacy
{
    [SerializableAttribute()]
    public class Lexer : ConfigItem
    {
		
        [XmlAttributeAttribute()]
        public int key;
		
		[XmlAttributeAttribute("name")]
		public string name;

		[XmlAttributeAttribute("style-bits")]
		public int stylebits;
	}
}
