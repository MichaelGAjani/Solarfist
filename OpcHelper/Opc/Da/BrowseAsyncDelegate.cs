namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public delegate BrowseElement[] BrowseAsyncDelegate(ItemIdentifier itemID, BrowseFilters filters, out BrowsePosition position);
}

