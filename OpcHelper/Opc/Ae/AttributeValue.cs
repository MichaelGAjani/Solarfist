namespace Jund.OpcHelper.Opc.Ae
{
    using Opc;
    using System;

    [Serializable]
    public class AttributeValue : ICloneable, IResult
    {
        private string m_diagnosticInfo = null;
        private int m_id = 0;
        private Opc.ResultID m_resultID = Opc.ResultID.S_OK;
        private object m_value = null;

        public virtual object Clone()
        {
            AttributeValue value2 = (AttributeValue) base.MemberwiseClone();
            value2.Value = Opc.Convert.Clone(this.Value);
            return value2;
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

        public int ID
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

