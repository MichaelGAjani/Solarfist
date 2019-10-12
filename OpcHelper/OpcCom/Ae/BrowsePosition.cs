namespace Jund.OpcHelper.OpcCom.Ae
{
    using Opc.Ae;
    using OpcCom;
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    public class BrowsePosition : Opc.Ae.BrowsePosition
    {
        private UCOMIEnumString m_enumerator;

        public BrowsePosition(string areaID, BrowseType browseType, string browseFilter, UCOMIEnumString enumerator) : base(areaID, browseType, browseFilter)
        {
            this.m_enumerator = null;
            this.m_enumerator = enumerator;
        }

        public override void Dispose()
        {
            base.Dispose();
            if (this.m_enumerator != null)
            {
                OpcCom.Interop.ReleaseServer(this.m_enumerator);
                this.m_enumerator = null;
            }
        }

        public UCOMIEnumString Enumerator
        {
            get
            {
                return this.m_enumerator;
            }
        }
    }
}

