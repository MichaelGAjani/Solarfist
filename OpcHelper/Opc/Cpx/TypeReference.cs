namespace Jund.OpcHelper.Opc.Cpx
{
    using System;
    using System.Xml.Serialization;

    [XmlType(Namespace="http://opcfoundation.org/OPCBinary/1.0/")]
    public class TypeReference : FieldType
    {
        [XmlAttribute]
        public string TypeID;
    }
}

