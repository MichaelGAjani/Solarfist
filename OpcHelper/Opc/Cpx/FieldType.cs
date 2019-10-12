namespace Jund.OpcHelper.Opc.Cpx
{
    using System;
    using System.Xml.Serialization;

    [XmlInclude(typeof(Ascii)), XmlInclude(typeof(Opc.Cpx.Int16)), XmlInclude(typeof(Int8)), XmlInclude(typeof(BitString)), XmlInclude(typeof(Opc.Cpx.Single)), XmlInclude(typeof(Opc.Cpx.Int32)), XmlInclude(typeof(CharString)), XmlInclude(typeof(Unicode)), XmlType(Namespace="http://opcfoundation.org/OPCBinary/1.0/"), XmlInclude(typeof(FloatingPoint)), XmlInclude(typeof(Opc.Cpx.Double)), XmlInclude(typeof(TypeReference)), XmlInclude(typeof(Integer)), XmlInclude(typeof(Opc.Cpx.UInt64)), XmlInclude(typeof(Opc.Cpx.UInt32)), XmlInclude(typeof(Opc.Cpx.UInt16)), XmlInclude(typeof(UInt8)), XmlInclude(typeof(Opc.Cpx.Int64))]
    public class FieldType
    {
        [XmlAttribute]
        public int ElementCount;
        [XmlAttribute]
        public string ElementCountRef;
        [XmlIgnore]
        public bool ElementCountSpecified;
        [XmlAttribute]
        public string FieldTerminator;
        [XmlAttribute]
        public string Format;
        [XmlAttribute]
        public int Length;
        [XmlIgnore]
        public bool LengthSpecified;
        [XmlAttribute]
        public string Name;
    }
}

