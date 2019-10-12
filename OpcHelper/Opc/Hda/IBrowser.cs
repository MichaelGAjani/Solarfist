namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Runtime.InteropServices;

    public interface IBrowser : IDisposable
    {
        BrowseElement[] Browse(ItemIdentifier itemID);
        BrowseElement[] Browse(ItemIdentifier itemID, int maxElements, out IBrowsePosition position);
        BrowseElement[] BrowseNext(int maxElements, ref IBrowsePosition position);

        BrowseFilterCollection Filters { get; }
    }
}

