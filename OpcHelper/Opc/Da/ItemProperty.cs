namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;

    [Serializable]
    public class ItemProperty : ICloneable, IResult
    {
        private System.Type m_datatype = null;
        private string m_description = null;
        private string m_diagnosticInfo = null;
        private PropertyID m_id;
        private string m_itemName = null;
        private string m_itemPath = null;
        private Opc.ResultID m_resultID = Opc.ResultID.S_OK;
        private object m_value = null;

        public virtual object Clone()
        {
            ItemProperty property = (ItemProperty) base.MemberwiseClone();
            property.Value = Opc.Convert.Clone(this.Value);
            return property;
        }

        public System.Type DataType
        {
            get
            {
                return this.m_datatype;
            }
            set
            {
                this.m_datatype = value;
            }
        }

        public string Description
        {
            get
            {
                return this.m_description;
            }
            set
            {
                this.m_description = value;
            }
        }

        public string DiagnosticInfo
        {
            get
            {
                return this.m_diagnosticInfo;
            }
            set
            {
                this.m_diagnosticInfo = value;
            }
        }

        public PropertyID ID
        {
            get
            {
                return this.m_id;
            }
            set
            {
                this.m_id = value;
            }
        }

        public string ItemName
        {
            get
            {
                return this.m_itemName;
            }
            set
            {
                this.m_itemName = value;
            }
        }

        public string ItemPath
        {
            get
            {
                return this.m_itemPath;
            }
            set
            {
                this.m_itemPath = value;
            }
        }

        public Opc.ResultID ResultID
        {
            get
            {
                return this.m_resultID;
            }
            set
            {
                this.m_resultID = value;
            }
        }

        public object Value
        {
            get
            {
                return this.m_value;
            }
            set
            {
                this.m_value = value;
            }
        }
    }
}

