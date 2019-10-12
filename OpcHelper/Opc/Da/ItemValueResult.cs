namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;

    [Serializable]
    public class ItemValueResult : ItemValue, IResult
    {
        private string m_diagnosticInfo;
        private Opc.ResultID m_resultID;

        public ItemValueResult()
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
        }

        public ItemValueResult(ItemValue item) : base(item)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
        }

        public ItemValueResult(ItemValueResult item) : base((ItemValue) item)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            if (item != null)
            {
                this.ResultID = item.ResultID;
                this.DiagnosticInfo = item.DiagnosticInfo;
            }
        }

        public ItemValueResult(ItemIdentifier item) : base(item)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
        }

        public ItemValueResult(ItemIdentifier item, Opc.ResultID resultID) : base(item)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.ResultID = resultID;
        }

        public ItemValueResult(string itemName, Opc.ResultID resultID) : base(itemName)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.ResultID = resultID;
        }

        public ItemValueResult(ItemIdentifier item, Opc.ResultID resultID, string diagnosticInfo) : base(item)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.ResultID = resultID;
            this.DiagnosticInfo = diagnosticInfo;
        }

        public ItemValueResult(string itemName, Opc.ResultID resultID, string diagnosticInfo) : base(itemName)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.ResultID = resultID;
            this.DiagnosticInfo = diagnosticInfo;
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

