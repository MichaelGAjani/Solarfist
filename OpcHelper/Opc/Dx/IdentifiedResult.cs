namespace Jund.OpcHelper.Opc.Dx
{
    using Opc;
    using System;

    [Serializable]
    public class IdentifiedResult : Opc.Dx.ItemIdentifier
    {
        private string m_diagnosticInfo;
        private Opc.ResultID m_resultID;

        public IdentifiedResult()
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
        }

        public IdentifiedResult(Opc.Dx.IdentifiedResult item) : base((Opc.Dx.ItemIdentifier) item)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            if (item != null)
            {
                this.ResultID = item.ResultID;
                this.DiagnosticInfo = item.DiagnosticInfo;
            }
        }

        public IdentifiedResult(Opc.Dx.ItemIdentifier item) : base(item)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
        }

        public IdentifiedResult(Opc.Dx.ItemIdentifier item, Opc.ResultID resultID) : base(item)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.ResultID = resultID;
        }

        public IdentifiedResult(string itemName, Opc.ResultID resultID) : base(itemName)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.ResultID = resultID;
        }

        public IdentifiedResult(Opc.Dx.ItemIdentifier item, Opc.ResultID resultID, string diagnosticInfo) : base(item)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.ResultID = resultID;
            this.DiagnosticInfo = diagnosticInfo;
        }

        public IdentifiedResult(string itemName, Opc.ResultID resultID, string diagnosticInfo) : base(itemName)
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

