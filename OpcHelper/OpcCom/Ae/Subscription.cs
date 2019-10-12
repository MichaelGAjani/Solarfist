namespace Jund.OpcHelper.OpcCom.Ae
{
    using Opc;
    using Opc.Ae;
    using OpcCom;
    using OpcRcw.Ae;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [Serializable]
    public class Subscription : ISubscription, IDisposable
    {
        private Callback m_callback = null;
        private object m_clientHandle = null;
        private ConnectionPoint m_connection = null;
        private object m_subscription = null;
        private bool m_supportsAE11 = true;

        public event EventChangedEventHandler EventChanged
        {
            add
            {
                lock (this)
                {
                    this.Advise();
                    this.m_callback.EventChanged += value;
                }
            }
            remove
            {
                lock (this)
                {
                    this.m_callback.EventChanged -= value;
                    this.Unadvise();
                }
            }
        }

        internal Subscription(SubscriptionState state, object subscription)
        {
            this.m_subscription = subscription;
            this.m_clientHandle = Opc.Convert.Clone(state.ClientHandle);
            this.m_supportsAE11 = true;
            this.m_callback = new Callback(state.ClientHandle);
            try
            {
                IOPCEventSubscriptionMgt2 mgt1 = (IOPCEventSubscriptionMgt2) this.m_subscription;
            }
            catch
            {
                this.m_supportsAE11 = false;
            }
        }

        private void Advise()
        {
            if (this.m_connection == null)
            {
                this.m_connection = new ConnectionPoint(this.m_subscription, typeof(IOPCEventSink).GUID);
                this.m_connection.Advise(this.m_callback);
            }
        }

        public void CancelRefresh()
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    ((IOPCEventSubscriptionMgt) this.m_subscription).CancelRefresh(0);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.CancelRefresh", exception);
                }
            }
        }

        public virtual void Dispose()
        {
            lock (this)
            {
                if (this.m_connection != null)
                {
                    this.m_connection.Dispose();
                    this.m_connection = null;
                }
                if (this.m_subscription != null)
                {
                    OpcCom.Interop.ReleaseServer(this.m_subscription);
                    this.m_subscription = null;
                }
            }
        }

        public SubscriptionFilters GetFilters()
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw new NotConnectedException();
                }
                int pdwEventType = 0;
                int pdwNumCategories = 0;
                IntPtr zero = IntPtr.Zero;
                int pdwLowSeverity = 0;
                int pdwHighSeverity = 0;
                int pdwNumAreas = 0;
                IntPtr ppszAreaList = IntPtr.Zero;
                int pdwNumSources = 0;
                IntPtr ppszSourceList = IntPtr.Zero;
                try
                {
                    ((IOPCEventSubscriptionMgt) this.m_subscription).GetFilter(out pdwEventType, out pdwNumCategories, out zero, out pdwLowSeverity, out pdwHighSeverity, out pdwNumAreas, out ppszAreaList, out pdwNumSources, out ppszSourceList);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.GetFilter", exception);
                }
                int[] collection = OpcCom.Interop.GetInt32s(ref zero, pdwNumCategories, true);
                string[] strArray = OpcCom.Interop.GetUnicodeStrings(ref ppszAreaList, pdwNumAreas, true);
                string[] strArray2 = OpcCom.Interop.GetUnicodeStrings(ref ppszSourceList, pdwNumSources, true);
                SubscriptionFilters filters = new SubscriptionFilters {
                    EventTypes = pdwEventType,
                    LowSeverity = pdwLowSeverity,
                    HighSeverity = pdwHighSeverity
                };
                filters.Categories.AddRange(collection);
                filters.Areas.AddRange(strArray);
                filters.Sources.AddRange(strArray2);
                return filters;
            }
        }

        public int[] GetReturnedAttributes(int eventCategory)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw new NotConnectedException();
                }
                int pdwCount = 0;
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCEventSubscriptionMgt) this.m_subscription).GetReturnedAttributes(eventCategory, out pdwCount, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.GetReturnedAttributes", exception);
                }
                return OpcCom.Interop.GetInt32s(ref zero, pdwCount, true);
            }
        }

        public SubscriptionState GetState()
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw new NotConnectedException();
                }
                int pbActive = 0;
                int pdwBufferTime = 0;
                int pdwMaxSize = 0;
                int phClientSubscription = 0;
                int pdwKeepAliveTime = 0;
                try
                {
                    ((IOPCEventSubscriptionMgt) this.m_subscription).GetState(out pbActive, out pdwBufferTime, out pdwMaxSize, out phClientSubscription);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.GetState", exception);
                }
                if (this.m_supportsAE11)
                {
                    try
                    {
                        ((IOPCEventSubscriptionMgt2) this.m_subscription).GetKeepAlive(out pdwKeepAliveTime);
                    }
                    catch (Exception exception2)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt2.GetKeepAlive", exception2);
                    }
                }
                return new SubscriptionState { Active = pbActive != 0, ClientHandle = this.m_clientHandle, BufferTime = pdwBufferTime, MaxSize = pdwMaxSize, KeepAlive = pdwKeepAliveTime };
            }
        }

        public SubscriptionState ModifyState(int masks, SubscriptionState state)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw new NotConnectedException();
                }
                int num = state.Active ? 1 : 0;
                GCHandle handle = GCHandle.Alloc(num, GCHandleType.Pinned);
                GCHandle handle2 = GCHandle.Alloc(state.BufferTime, GCHandleType.Pinned);
                GCHandle handle3 = GCHandle.Alloc(state.MaxSize, GCHandleType.Pinned);
                IntPtr pbActive = ((masks & 4) != 0) ? handle.AddrOfPinnedObject() : IntPtr.Zero;
                IntPtr pdwBufferTime = ((masks & 8) != 0) ? handle2.AddrOfPinnedObject() : IntPtr.Zero;
                IntPtr pdwMaxSize = ((masks & 0x10) != 0) ? handle3.AddrOfPinnedObject() : IntPtr.Zero;
                int hClientSubscription = 0;
                int pdwRevisedBufferTime = 0;
                int pdwRevisedMaxSize = 0;
                try
                {
                    ((IOPCEventSubscriptionMgt) this.m_subscription).SetState(pbActive, pdwBufferTime, pdwMaxSize, hClientSubscription, out pdwRevisedBufferTime, out pdwRevisedMaxSize);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.SetState", exception);
                }
                finally
                {
                    if (handle.IsAllocated)
                    {
                        handle.Free();
                    }
                    if (handle2.IsAllocated)
                    {
                        handle2.Free();
                    }
                    if (handle3.IsAllocated)
                    {
                        handle3.Free();
                    }
                }
                if (((masks & 0x20) != 0) && this.m_supportsAE11)
                {
                    int pdwRevisedKeepAliveTime = 0;
                    try
                    {
                        ((IOPCEventSubscriptionMgt2) this.m_subscription).SetKeepAlive(state.KeepAlive, out pdwRevisedKeepAliveTime);
                    }
                    catch (Exception exception2)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt2.SetKeepAlive", exception2);
                    }
                }
                return this.GetState();
            }
        }

        public void Refresh()
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    ((IOPCEventSubscriptionMgt) this.m_subscription).Refresh(0);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.Refresh", exception);
                }
            }
        }

        public void SelectReturnedAttributes(int eventCategory, int[] attributeIDs)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    ((IOPCEventSubscriptionMgt) this.m_subscription).SelectReturnedAttributes(eventCategory, (attributeIDs != null) ? attributeIDs.Length : 0, (attributeIDs != null) ? attributeIDs : new int[0]);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.SelectReturnedAttributes", exception);
                }
            }
        }

        public void SetFilters(SubscriptionFilters filters)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    ((IOPCEventSubscriptionMgt) this.m_subscription).SetFilter(filters.EventTypes, filters.Categories.Count, filters.Categories.ToArray(), filters.LowSeverity, filters.HighSeverity, filters.Areas.Count, filters.Areas.ToArray(), filters.Sources.Count, filters.Sources.ToArray());
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.SetFilter", exception);
                }
            }
        }

        private void Unadvise()
        {
            if ((this.m_connection != null) && (this.m_connection.Unadvise() == 0))
            {
                this.m_connection.Dispose();
                this.m_connection = null;
            }
        }

        private class Callback : IOPCEventSink
        {
            private object m_clientHandle = null;

            public event EventChangedEventHandler EventChanged
            {
                add
                {
                    lock (this)
                    {
                        this.m_EventChanged = (EventChangedEventHandler) Delegate.Combine(this.m_EventChanged, value);
                    }
                }
                remove
                {
                    lock (this)
                    {
                        this.m_EventChanged = (EventChangedEventHandler) Delegate.Remove(this.m_EventChanged, value);
                    }
                }
            }

            private event EventChangedEventHandler m_EventChanged;

            public Callback(object clientHandle)
            {
                this.m_clientHandle = clientHandle;
            }

            public void OnEvent(int hClientSubscription, int bRefresh, int bLastRefresh, int dwCount, ONEVENTSTRUCT[] pEvents)
            {
                try
                {
                    lock (this)
                    {
                        if (this.m_EventChanged != null)
                        {
                            EventNotification[] eventNotifications = OpcCom.Ae.Interop.GetEventNotifications(pEvents);
                            for (int i = 0; i < eventNotifications.Length; i++)
                            {
                                eventNotifications[i].ClientHandle = this.m_clientHandle;
                            }
                            this.m_EventChanged(eventNotifications, bRefresh != 0, bLastRefresh != 0);
                        }
                    }
                }
                catch (Exception exception)
                {
                    string stackTrace = exception.StackTrace;
                }
            }
        }
    }
}

