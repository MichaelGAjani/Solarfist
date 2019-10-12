namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;

    [Serializable]
    public class Result : ICloneable, IResult
    {
        private string m_diagnosticInfo;
        private Opc.ResultID m_resultID;

        public Result()
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
        }

        public Result(IResult result)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.ResultID = result.ResultID;
            this.DiagnosticInfo = result.DiagnosticInfo;
        }

        public Result(Opc.ResultID resultID)
        {
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.ResultID = resultID;
            this.DiagnosticInfo = null;
        }

        public object Clone()
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

