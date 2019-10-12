namespace Jund.OpcHelper.OpcCom.Da
{
    using Opc;
    using Opc.Da;
    using System;

    [Serializable]
    internal class BrowsePosition : Opc.Da.BrowsePosition
    {
        internal string ContinuationPoint;
        internal bool MoreElements;

        internal BrowsePosition(ItemIdentifier itemID, BrowseFilters filters, string continuationPoint) : base(itemID, filters)
        {
            this.ContinuationPoint = null;
            this.MoreElements = false;
            this.ContinuationPoint = continuationPoint;
        }
    }
}

