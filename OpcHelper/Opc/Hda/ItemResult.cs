namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;

    [Serializable]
    public class ItemResult : Item, IResult
    {
        private string m_diagnosticInfo;
        private Opc.ResultID m_resultID;

        public ItemResult()
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
        }

        public ItemResult(Item item) : base(item)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
        }

        public ItemResult(ItemResult item) : base((Item) item)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            if (item != null)
            {
                this.ResultID = item.ResultID;
                this.DiagnosticInfo = item.DiagnosticInfo;
            }
        }

        public ItemResult(ItemIdentifier item) : base(item)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
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
    }
}

