namespace Jund.OpcHelper.Opc.Ae
{
    using System;

    [Serializable]
    public class Category : ICloneable
    {
        private int m_id = 0;
        private string m_name = null;

        public virtual object Clone()
        {
            return base.MemberwiseClone();
        }

        public override string ToString()
        {
            return this.Name;
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
    }
}

