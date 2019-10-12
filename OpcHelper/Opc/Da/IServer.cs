namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;
    using System.Runtime.InteropServices;

    public interface IServer : Opc.IServer, IDisposable
    {
        BrowseElement[] Browse(ItemIdentifier itemID, BrowseFilters filters, out BrowsePosition position);
        BrowseElement[] BrowseNext(ref BrowsePosition position);
        void CancelSubscription(ISubscription subscription);
        ISubscription CreateSubscription(SubscriptionState state);
        ItemPropertyCollection[] GetProperties(ItemIdentifier[] itemIDs, PropertyID[] propertyIDs, bool returnValues);
        int GetResultFilters();
        ServerStatus GetStatus();
        ItemValueResult[] Read(Item[] items);
        void SetResultFilters(int filters);
        IdentifiedResult[] Write(ItemValue[] values);
    }
}

