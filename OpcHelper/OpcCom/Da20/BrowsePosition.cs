namespace Jund.OpcHelper.OpcCom.Da20
{
    using Opc;
    using Opc.Da;
    using OpcCom;
    using System;

    [Serializable]
    internal class BrowsePosition : Opc.Da.BrowsePosition
    {
        internal EnumString Enumerator;
        internal int Index;
        internal bool IsBranch;
        internal string[] Names;

        internal BrowsePosition(ItemIdentifier itemID, BrowseFilters filters, EnumString enumerator, bool isBranch) : base(itemID, filters)
        {
            this.Enumerator = null;
            this.IsBranch = true;
            this.Names = null;
            this.Index = 0;
            this.Enumerator = enumerator;
            this.IsBranch = isBranch;
        }

        public override object Clone()
        {
            OpcCom.Da20.BrowsePosition position = (OpcCom.Da20.BrowsePosition) base.MemberwiseClone();
            position.Enumerator = this.Enumerator.Clone();
            return position;
        }

        public override void Dispose()
        {
            if (this.Enumerator != null)
            {
                this.Enumerator.Dispose();
                this.Enumerator = null;
            }
        }
    }
}

