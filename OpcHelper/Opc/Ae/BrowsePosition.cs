namespace Jund.OpcHelper.Opc.Ae
{
    using Opc;
    using System;

    [Serializable]
    public class BrowsePosition : IBrowsePosition, IDisposable, ICloneable
    {
        private string m_areaID = null;
        private string m_browseFilter = null;
        private Opc.Ae.BrowseType m_browseType = Opc.Ae.BrowseType.Area;

        public BrowsePosition(string areaID, Opc.Ae.BrowseType browseType, string browseFilter)
        {
            this.m_areaID = areaID;
            this.m_browseType = browseType;
            this.m_browseFilter = browseFilter;
        }

        public virtual object Clone()
        {
            return (BrowsePosition) base.MemberwiseClone();
        }

        public virtual void Dispose()
        {
        }

        public string AreaID
        {
            get
            {
                return this.m_areaID;
            }
        }

        public string BrowseFilter
        {
            get
            {
                return this.m_browseFilter;
            }
        }

        public Opc.Ae.BrowseType BrowseType
        {
            get
            {
                return this.m_browseType;
            }
        }
    }
}

