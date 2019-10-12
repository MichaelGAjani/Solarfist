namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    [Serializable]
    public class Server : Opc.Server, Opc.Hda.IServer, Opc.IServer, IDisposable
    {
        private AggregateCollection m_aggregates;
        private AttributeCollection m_attributes;
        private Hashtable m_items;
        private TrendCollection m_trends;

        public Server(Factory factory, URL url) : base(factory, url)
        {
            this.m_items = new Hashtable();
            this.m_attributes = new AttributeCollection();
            this.m_aggregates = new AggregateCollection();
            this.m_trends = new TrendCollection();
        }

        protected Server(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.m_items = new Hashtable();
            this.m_attributes = new AttributeCollection();
            this.m_aggregates = new AggregateCollection();
            this.m_trends = new TrendCollection();
            Trend[] trendArray = (Trend[]) info.GetValue("Trends", typeof(Trend[]));
            if (trendArray != null)
            {
                foreach (Trend trend in trendArray)
                {
                    trend.SetServer(this);
                    this.m_trends.Add(trend);
                }
            }
        }

        public IdentifiedResult[] AdviseProcessed(Time startTime, decimal resampleInterval, int numberOfIntervals, Item[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).AdviseProcessed(startTime, resampleInterval, numberOfIntervals, items, requestHandle, callback, out request);
        }

        public IdentifiedResult[] AdviseRaw(Time startTime, decimal updateInterval, ItemIdentifier[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).AdviseRaw(startTime, updateInterval, items, requestHandle, callback, out request);
        }

        public void CancelRequest(IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            ((Opc.Hda.IServer) base.m_server).CancelRequest(request);
        }

        public void CancelRequest(IRequest request, CancelCompleteEventHandler callback)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            ((Opc.Hda.IServer) base.m_server).CancelRequest(request, callback);
        }

        public override object Clone()
        {
            return (Opc.Hda.Server) base.Clone();
        }

        public override void Connect(URL url, ConnectData connectData)
        {
            base.Connect(url, connectData);
            this.GetAttributes();
            this.GetAggregates();
            foreach (Trend trend in this.m_trends)
            {
                ArrayList list = new ArrayList();
                foreach (Item item in trend.Items)
                {
                    list.Add(new ItemIdentifier(item));
                }
                IdentifiedResult[] resultArray = this.CreateItems((ItemIdentifier[]) list.ToArray(typeof(ItemIdentifier)));
                if (resultArray != null)
                {
                    for (int i = 0; i < resultArray.Length; i++)
                    {
                        trend.Items[i].ServerHandle = null;
                        if (resultArray[i].ResultID.Succeeded())
                        {
                            trend.Items[i].ServerHandle = resultArray[i].ServerHandle;
                        }
                    }
                }
            }
        }

        public IBrowser CreateBrowser(BrowseFilter[] filters, out ResultID[] results)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).CreateBrowser(filters, out results);
        }

        public IdentifiedResult[] CreateItems(ItemIdentifier[] items)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            IdentifiedResult[] resultArray = ((Opc.Hda.IServer) base.m_server).CreateItems(items);
            if (resultArray != null)
            {
                foreach (IdentifiedResult result in resultArray)
                {
                    if (result.ResultID.Succeeded())
                    {
                        this.m_items.Add(result.ServerHandle, new ItemIdentifier(result));
                    }
                }
            }
            return resultArray;
        }

        public IdentifiedResult[] Delete(Time startTime, Time endTime, ItemIdentifier[] items)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).Delete(startTime, endTime, items);
        }

        public IdentifiedResult[] Delete(Time startTime, Time endTime, ItemIdentifier[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).Delete(startTime, endTime, items, requestHandle, callback, out request);
        }

        public ResultCollection[] DeleteAtTime(ItemTimeCollection[] items)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).DeleteAtTime(items);
        }

        public IdentifiedResult[] DeleteAtTime(ItemTimeCollection[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).DeleteAtTime(items, requestHandle, callback, out request);
        }

        public override void Disconnect()
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            if (this.m_items.Count > 0)
            {
                try
                {
                    ArrayList list = new ArrayList(this.m_items.Count);
                    list.AddRange(this.m_items);
                    ((Opc.Hda.IServer) base.m_server).ReleaseItems((ItemIdentifier[]) list.ToArray(typeof(ItemIdentifier)));
                }
                catch
                {
                }
                this.m_items.Clear();
            }
            foreach (Trend trend in this.m_trends)
            {
                foreach (Item item in trend.Items)
                {
                    item.ServerHandle = null;
                }
            }
            base.Disconnect();
        }

        public Aggregate[] GetAggregates()
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            this.m_aggregates.Clear();
            Aggregate[] aggregates = ((Opc.Hda.IServer) base.m_server).GetAggregates();
            if (aggregates != null)
            {
                this.m_aggregates.Init(aggregates);
            }
            return aggregates;
        }

        public Opc.Hda.Attribute[] GetAttributes()
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            this.m_attributes.Clear();
            Opc.Hda.Attribute[] attributes = ((Opc.Hda.IServer) base.m_server).GetAttributes();
            if (attributes != null)
            {
                this.m_attributes.Init(attributes);
            }
            return attributes;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            Trend[] trendArray = null;
            if (this.m_trends.Count > 0)
            {
                trendArray = new Trend[this.m_trends.Count];
                for (int i = 0; i < trendArray.Length; i++)
                {
                    trendArray[i] = this.m_trends[i];
                }
            }
            info.AddValue("Trends", trendArray);
        }

        public ServerStatus GetStatus()
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            ServerStatus status = ((Opc.Hda.IServer) base.m_server).GetStatus();
            if (status.StatusInfo == null)
            {
                status.StatusInfo = base.GetString("serverState." + status.ServerState.ToString());
            }
            return status;
        }

        public ResultCollection[] Insert(ItemValueCollection[] items, bool replace)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).Insert(items, replace);
        }

        public IdentifiedResult[] Insert(ItemValueCollection[] items, bool replace, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).Insert(items, replace, requestHandle, callback, out request);
        }

        public ResultCollection[] InsertAnnotations(AnnotationValueCollection[] items)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).InsertAnnotations(items);
        }

        public IdentifiedResult[] InsertAnnotations(AnnotationValueCollection[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).InsertAnnotations(items, requestHandle, callback, out request);
        }

        public IdentifiedResult[] PlaybackProcessed(Time startTime, Time endTime, decimal resampleInterval, int numberOfIntervals, decimal updateInterval, Item[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).PlaybackProcessed(startTime, endTime, resampleInterval, numberOfIntervals, updateInterval, items, requestHandle, callback, out request);
        }

        public IdentifiedResult[] PlaybackRaw(Time startTime, Time endTime, int maxValues, decimal updateInterval, decimal playbackDuration, ItemIdentifier[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).PlaybackRaw(startTime, endTime, maxValues, updateInterval, playbackDuration, items, requestHandle, callback, out request);
        }

        public AnnotationValueCollection[] ReadAnnotations(Time startTime, Time endTime, ItemIdentifier[] items)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).ReadAnnotations(startTime, endTime, items);
        }

        public IdentifiedResult[] ReadAnnotations(Time startTime, Time endTime, ItemIdentifier[] items, object requestHandle, ReadAnnotationsEventHandler callback, out IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).ReadAnnotations(startTime, endTime, items, requestHandle, callback, out request);
        }

        public ItemValueCollection[] ReadAtTime(DateTime[] timestamps, ItemIdentifier[] items)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).ReadAtTime(timestamps, items);
        }

        public IdentifiedResult[] ReadAtTime(DateTime[] timestamps, ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).ReadAtTime(timestamps, items, requestHandle, callback, out request);
        }

        public ItemAttributeCollection ReadAttributes(Time startTime, Time endTime, ItemIdentifier item, int[] attributeIDs)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).ReadAttributes(startTime, endTime, item, attributeIDs);
        }

        public ResultCollection ReadAttributes(Time startTime, Time endTime, ItemIdentifier item, int[] attributeIDs, object requestHandle, ReadAttributesEventHandler callback, out IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).ReadAttributes(startTime, endTime, item, attributeIDs, requestHandle, callback, out request);
        }

        public ModifiedValueCollection[] ReadModified(Time startTime, Time endTime, int maxValues, ItemIdentifier[] items)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).ReadModified(startTime, endTime, maxValues, items);
        }

        public IdentifiedResult[] ReadModified(Time startTime, Time endTime, int maxValues, ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).ReadModified(startTime, endTime, maxValues, items, requestHandle, callback, out request);
        }

        public ItemValueCollection[] ReadProcessed(Time startTime, Time endTime, decimal resampleInterval, Item[] items)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).ReadProcessed(startTime, endTime, resampleInterval, items);
        }

        public IdentifiedResult[] ReadProcessed(Time startTime, Time endTime, decimal resampleInterval, Item[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).ReadProcessed(startTime, endTime, resampleInterval, items, requestHandle, callback, out request);
        }

        public ItemValueCollection[] ReadRaw(Time startTime, Time endTime, int maxValues, bool includeBounds, ItemIdentifier[] items)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).ReadRaw(startTime, endTime, maxValues, includeBounds, items);
        }

        public IdentifiedResult[] ReadRaw(Time startTime, Time endTime, int maxValues, bool includeBounds, ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).ReadRaw(startTime, endTime, maxValues, includeBounds, items, requestHandle, callback, out request);
        }

        public IdentifiedResult[] ReleaseItems(ItemIdentifier[] items)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            IdentifiedResult[] resultArray = ((Opc.Hda.IServer) base.m_server).ReleaseItems(items);
            if (resultArray != null)
            {
                foreach (IdentifiedResult result in resultArray)
                {
                    if (result.ResultID.Succeeded())
                    {
                        this.m_items.Remove(result.ServerHandle);
                    }
                }
            }
            return resultArray;
        }

        public ResultCollection[] Replace(ItemValueCollection[] items)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).Replace(items);
        }

        public IdentifiedResult[] Replace(ItemValueCollection[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).Replace(items, requestHandle, callback, out request);
        }

        public IdentifiedResult[] ValidateItems(ItemIdentifier[] items)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Hda.IServer) base.m_server).ValidateItems(items);
        }

        public AggregateCollection Aggregates
        {
            get
            {
                return this.m_aggregates;
            }
        }

        public AttributeCollection Attributes
        {
            get
            {
                return this.m_attributes;
            }
        }

        public ItemIdentifierCollection Items
        {
            get
            {
                return new ItemIdentifierCollection(this.m_items.Values);
            }
        }

        public TrendCollection Trends
        {
            get
            {
                return this.m_trends;
            }
        }

        private class Names
        {
            internal const string TRENDS = "Trends";
        }
    }
}

