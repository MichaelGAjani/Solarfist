namespace Jund.OpcHelper.Opc.Ae
{
    using System;

    [Serializable]
    public class SubCondition : ICloneable
    {
        private string m_definition = null;
        private string m_description = null;
        private string m_name = null;
        private int m_severity = 1;

        public virtual object Clone()
        {
            return base.MemberwiseClone();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string Definition
        {
            get
            {
                return this.m_definition;
            }
            set
            {
                this.m_definition = value;
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

        public string Name
        {
            get
            {
                return this.m_name;
            }
            set
            {
                this.m_name = value;
            }
        }

        public int Severity
        {
            get
            {
                return this.m_severity;
            }
            set
            {
                this.m_severity = value;
            }
        }
    }
}

