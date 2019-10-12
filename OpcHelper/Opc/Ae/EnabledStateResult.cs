namespace Jund.OpcHelper.Opc.Ae
{
    using Opc;
    using System;

    public class EnabledStateResult : IResult
    {
        private string m_diagnosticInfo;
        private bool m_effectivelyEnabled;
        private bool m_enabled;
        private string m_qualifiedName;
        private Opc.ResultID m_resultID;

        public EnabledStateResult()
        {
            this.m_qualifiedName = null;
            this.m_enabled = false;
            this.m_effectivelyEnabled = false;
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
        }

        public EnabledStateResult(string qualifiedName)
        {
            this.m_qualifiedName = null;
            this.m_enabled = false;
            this.m_effectivelyEnabled = false;
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.m_qualifiedName = qualifiedName;
        }

        public EnabledStateResult(string qualifiedName, Opc.ResultID resultID)
        {
            this.m_qualifiedName = null;
            this.m_enabled = false;
            this.m_effectivelyEnabled = false;
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.m_qualifiedName = qualifiedName;
            this.m_resultID = this.ResultID;
        }

        public virtual object Clone()
        {
            return base.MemberwiseClone();
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

        public bool EffectivelyEnabled
        {
            get
            {
                return this.m_effectivelyEnabled;
            }
            set
            {
                this.m_effectivelyEnabled = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.m_enabled;
            }
            set
            {
                this.m_enabled = value;
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

