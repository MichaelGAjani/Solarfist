namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public interface ISubscription : IDisposable
    {
        event DataChangedEventHandler DataChanged;

        ItemResult[] AddItems(Item[] items);
        void Cancel(IRequest request, CancelCompleteEventHandler callback);
        bool GetEnabled();
        int GetResultFilters();
        SubscriptionState GetState();
        ItemResult[] ModifyItems(int masks, Item[] items);
        SubscriptionState ModifyState(int masks, SubscriptionState state);
        ItemValueResult[] Read(Item[] items);
        IdentifiedResult[] Read(Item[] items, object requestHandle, ReadCompleteEventHandler callback, out IRequest request);
        void Refresh();
        void Refresh(object requestHandle, out IRequest request);
        IdentifiedResult[] RemoveItems(ItemIdentifier[] items);
        void SetEnabled(bool enabled);
        void SetResultFilters(int filters);
        IdentifiedResult[] Write(ItemValue[] items);
        IdentifiedResult[] Write(ItemValue[] items, object requestHandle, WriteCompleteEventHandler callback, out IRequest request);
    }
}

