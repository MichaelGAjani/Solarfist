namespace Jund.OpcHelper.Opc.Cpx
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [XmlInclude(typeof(Opc.Cpx.UInt64)), XmlType(Namespace="http://opcfoundation.org/OPCBinary/1.0/"), XmlInclude(typeof(Opc.Cpx.Int32)), XmlInclude(typeof(Opc.Cpx.UInt32)), XmlInclude(typeof(Opc.Cpx.UInt16)), XmlInclude(typeof(UInt8)), XmlInclude(typeof(Opc.Cpx.Int64)), XmlInclude(typeof(Int8)), XmlInclude(typeof(Opc.Cpx.Int16))]
    public class Integer : FieldType
    {
        [DefaultValue(true), XmlAttribute]
        public bool Signed = true;
    }
}

