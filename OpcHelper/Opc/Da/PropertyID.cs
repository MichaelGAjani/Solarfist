namespace Jund.OpcHelper.Opc.Da
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Xml;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct PropertyID : ISerializable
    {
        private int m_code;
        private XmlQualifiedName m_name;
        private PropertyID(SerializationInfo info, StreamingContext context)
        {
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            string name = "";
            string ns = "";
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Name.Equals("NA"))
                {
                    name = (string) enumerator.Current.Value;
                }
                else if (enumerator.Current.Name.Equals("NS"))
                {
                    ns = (string) enumerator.Current.Value;
                }
            }
            this.m_name = new XmlQualifiedName(name, ns);
            this.m_code = (int) info.GetValue("CO", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (this.m_name != null)
            {
                info.AddValue("NA", this.m_name.Name);
                info.AddValue("NS", this.m_name.Namespace);
            }
            info.AddValue("CO", this.m_code);
        }

        public XmlQualifiedName Name
        {
            get
            {
                return this.m_name;
            }
        }
        public int Code
        {
            get
            {
                return this.m_code;
            }
        }
        public static bool operator ==(PropertyID a, PropertyID b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(PropertyID a, PropertyID b)
        {
            return !a.Equals(b);
        }

        public PropertyID(XmlQualifiedName name)
        {
            this.m_name = name;
            this.m_code = 0;
        }

        public PropertyID(int code)
        {
            this.m_name = null;
            this.m_code = code;
        }

        public PropertyID(string name, int code, string ns)
        {
            this.m_name = new XmlQualifiedName(name, ns);
            this.m_code = code;
        }

        public override bool Equals(object target)
        {
            if ((target != null) && (target.GetType() == typeof(PropertyID)))
            {
                PropertyID yid = (PropertyID) target;
                if ((yid.Code != 0) && (this.Code != 0))
                {
                    return (yid.Code == this.Code);
                }
                if ((yid.Name != null) && (this.Name != null))
                {
                    return (yid.Name == this.Name);
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            if (this.Code != 0)
            {
                return this.Code.GetHashCode();
            }
            if (this.Name != null)
            {
                return this.Name.GetHashCode();
            }
            return base.GetHashCode();
        }

        public override string ToString()
        {
            if ((this.Name != null) && (this.Code != 0))
            {
                return string.Format("{0} ({1})", this.Name.Name, this.Code);
            }
            if (this.Name != null)
            {
                return this.Name.Name;
            }
            if (this.Code != 0)
            {
                return string.Format("{0}", this.Code);
            }
            return "";
        }
        private class Names
        {
            internal const string CODE = "CO";
            internal const string NAME = "NA";
            internal const string NAMESPACE = "NS";
        }
    }
}

