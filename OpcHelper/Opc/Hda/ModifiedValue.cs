namespace Jund.OpcHelper.Opc.Hda
{
    using System;

    [Serializable]
    public class ModifiedValue : ItemValue
    {
        private Opc.Hda.EditType m_editType = Opc.Hda.EditType.Insert;
        private DateTime m_modificationTime = DateTime.MinValue;
        private string m_user = null;

        public Opc.Hda.EditType EditType
        {
            get
            {
                return this.m_editType;
            }
            set
            {
                this.m_editType = value;
            }
        }

        public DateTime ModificationTime
        {
            get
            {
                return this.m_modificationTime;
            }
            set
            {
                this.m_modificationTime = value;
            }
        }

        public string User
        {
            get
            {
                return this.m_user;
            }
            set
            {
                this.m_user = value;
            }
        }
    }
}

