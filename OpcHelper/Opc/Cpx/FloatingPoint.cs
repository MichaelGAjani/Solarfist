namespace Jund.OpcHelper.Opc.Cpx
{
    using System;
    using System.Xml.Serialization;

    [XmlInclude(typeof(Opc.Cpx.Double)), XmlType(Namespace="http://opcfoundation.org/OPCBinary/1.0/"), XmlInclude(typeof(Opc.Cpx.Single))]
    public class FloatingPoint : FieldType
    {
        [XmlAttribute]
        public string FloatFormat;
    }
}

