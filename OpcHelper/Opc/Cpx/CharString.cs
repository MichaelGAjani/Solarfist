namespace Jund.OpcHelper.Opc.Cpx
{
    using System;
    using System.Xml.Serialization;

    [XmlInclude(typeof(Ascii)), XmlType(Namespace="http://opcfoundation.org/OPCBinary/1.0/"), XmlInclude(typeof(Unicode))]
    public class CharString : FieldType
    {
        [XmlAttribute]
        public string CharCountRef;
        [XmlAttribute]
        public int CharWidth;
        [XmlIgnore]
        public bool CharWidthSpecified;
        [XmlAttribute]
        public string StringEncoding;
    }
}

