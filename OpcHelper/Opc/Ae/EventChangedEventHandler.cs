namespace Jund.OpcHelper.Opc.Ae
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void EventChangedEventHandler(EventNotification[] notifications, bool refresh, bool lastRefresh);
}

