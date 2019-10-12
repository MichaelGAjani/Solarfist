namespace Jund.OpcHelper.Opc.Ae
{
    using System;
    using System.Runtime.CompilerServices;

    public interface ISubscription : IDisposable
    {
        event EventChangedEventHandler EventChanged;

        void CancelRefresh();
        SubscriptionFilters GetFilters();
        int[] GetReturnedAttributes(int eventCategory);
        SubscriptionState GetState();
        SubscriptionState ModifyState(int masks, SubscriptionState state);
        void Refresh();
        void SelectReturnedAttributes(int eventCategory, int[] attributeIDs);
        void SetFilters(SubscriptionFilters filters);
    }
}

