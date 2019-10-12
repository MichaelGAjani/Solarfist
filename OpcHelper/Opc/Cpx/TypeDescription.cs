namespace Jund.OpcHelper.Opc.Cpx
{
    using System;
    using System.Xml.Serialization;

    [XmlType(Namespace="http://opcfoundation.org/OPCBinary/1.0/")]
    public class TypeDescription
    {
        [XmlAttribute]
        public bool DefaultBigEndian;
        [XmlIgnore]
        public bool DefaultBigEndianSpecified;
        [XmlAttribute]
        public int DefaultCharWidth;
        [XmlIgnore]
        public bool DefaultCharWidthSpecified;
        [XmlAttribute]
        public string DefaultFloatFormat;
        [XmlAttribute]
        public string DefaultStringEncoding;
        [XmlElement("Field")]
        public FieldType[] Field;
        [XmlAttribute]
        public string TypeID;
    }
}

