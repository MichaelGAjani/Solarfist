namespace Jund.OpcHelper.Opc.Cpx
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [XmlType(Namespace="http://opcfoundation.org/OPCBinary/1.0/"), XmlRoot(Namespace="http://opcfoundation.org/OPCBinary/1.0/", IsNullable=false)]
    public class TypeDictionary
    {
        [XmlAttribute, DefaultValue(true)]
        public bool DefaultBigEndian = true;
        [DefaultValue(2), XmlAttribute]
        public int DefaultCharWidth = 2;
        [DefaultValue("IEEE-754"), XmlAttribute]
        public string DefaultFloatFormat = "IEEE-754";
        [DefaultValue("UCS-2"), XmlAttribute]
        public string DefaultStringEncoding = "UCS-2";
        [XmlAttribute]
        public string DictionaryName;
        [XmlElement("TypeDescription")]
        public Opc.Cpx.TypeDescription[] TypeDescription;
    }
}

