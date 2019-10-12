namespace Jund.OpcHelper.OpcCom.Hda
{
    using Opc.Hda;
    using OpcCom;
    using System;

    internal class BrowsePosition : Opc.Hda.BrowsePosition
    {
        private string m_branchPath = null;
        private EnumString m_enumerator = null;
        private bool m_fetchingItems = false;

        internal BrowsePosition(string branchPath, EnumString enumerator, bool fetchingItems)
        {
            this.m_branchPath = branchPath;
            this.m_enumerator = enumerator;
            this.m_fetchingItems = fetchingItems;
        }

        public override void Dispose()
        {
            if (this.m_enumerator != null)
            {
                this.m_enumerator.Dispose();
                this.m_enumerator = null;
            }
            base.Dispose();
        }

        internal string BranchPath
        {
            get
            {
                return this.m_branchPath;
            }
            set
            {
                this.m_branchPath = value;
            }
        }

        internal EnumString Enumerator
        {
            get
            {
                return this.m_enumerator;
            }
            set
            {
                this.m_enumerator = value;
            }
        }

        internal bool FetchingItems
        {
            get
            {
                return this.m_fetchingItems;
            }
            set
            {
                this.m_fetchingItems = value;
            }
        }
    }
}

