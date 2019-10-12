namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    [Serializable]
    public class Trend : ISerializable, ICloneable
    {
        private int m_aggregateID;
        private static int m_count = 0;
        private Time m_endTime;
        private bool m_includeBounds;
        private ItemCollection m_items;
        private int m_maxValues;
        private string m_name;
        private IRequest m_playback;
        private decimal m_playbackDuration;
        private decimal m_playbackInterval;
        private decimal m_resampleInterval;
        private Opc.Hda.Server m_server;
        private Time m_startTime;
        private IRequest m_subscription;
        private ItemTimeCollection m_timestamps;
        private decimal m_updateInterval;

        public Trend(Opc.Hda.Server server)
        {
            this.m_server = null;
            this.m_name = null;
            this.m_aggregateID = 0;
            this.m_startTime = null;
            this.m_endTime = null;
            this.m_maxValues = 0;
            this.m_includeBounds = false;
            this.m_resampleInterval = 0M;
            this.m_timestamps = new ItemTimeCollection();
            this.m_items = new ItemCollection();
            this.m_updateInterval = 0M;
            this.m_playbackInterval = 0M;
            this.m_playbackDuration = 0M;
            this.m_subscription = null;
            this.m_playback = null;
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }
            this.m_server = server;
            do
            {
                this.Name = string.Format("Trend{0,2:00}", ++m_count);
            }
            while (this.m_server.Trends[this.Name] != null);
        }

        protected Trend(SerializationInfo info, StreamingContext context)
        {
            this.m_server = null;
            this.m_name = null;
            this.m_aggregateID = 0;
            this.m_startTime = null;
            this.m_endTime = null;
            this.m_maxValues = 0;
            this.m_includeBounds = false;
            this.m_resampleInterval = 0M;
            this.m_timestamps = new ItemTimeCollection();
            this.m_items = new ItemCollection();
            this.m_updateInterval = 0M;
            this.m_playbackInterval = 0M;
            this.m_playbackDuration = 0M;
            this.m_subscription = null;
            this.m_playback = null;
            this.m_name = (string) info.GetValue("Name", typeof(string));
            this.m_aggregateID = (int) info.GetValue("AggregateID", typeof(int));
            this.m_startTime = (Time) info.GetValue("StartTime", typeof(Time));
            this.m_endTime = (Time) info.GetValue("EndTime", typeof(Time));
            this.m_maxValues = (int) info.GetValue("MaxValues", typeof(int));
            this.m_includeBounds = (bool) info.GetValue("IncludeBounds", typeof(bool));
            this.m_resampleInterval = (decimal) info.GetValue("ResampleInterval", typeof(decimal));
            this.m_updateInterval = (decimal) info.GetValue("UpdateInterval", typeof(decimal));
            this.m_playbackInterval = (decimal) info.GetValue("PlaybackInterval", typeof(decimal));
            this.m_playbackDuration = (decimal) info.GetValue("PlaybackDuration", typeof(decimal));
            DateTime[] timeArray = (DateTime[]) info.GetValue("Timestamps", typeof(DateTime[]));
            if (timeArray != null)
            {
                foreach (DateTime time in timeArray)
                {
                    this.m_timestamps.Add(time);
                }
            }
            Item[] itemArray = (Item[]) info.GetValue("Items", typeof(Item[]));
            if (itemArray != null)
            {
                foreach (Item item in itemArray)
                {
                    this.m_items.Add(item);
                }
            }
        }

        public Item AddItem(ItemIdentifier itemID)
        {
            if (itemID == null)
            {
                throw new ArgumentNullException("itemID");
            }
            if (itemID.ClientHandle == null)
            {
                itemID.ClientHandle = Guid.NewGuid().ToString();
            }
            IdentifiedResult[] resultArray = this.m_server.CreateItems(new ItemIdentifier[] { itemID });
            if ((resultArray == null) || (resultArray.Length != 1))
            {
                throw new InvalidResponseException();
            }
            if (resultArray[0].ResultID.Failed())
            {
                throw new ResultIDException(resultArray[0].ResultID, "Could not add item to trend.");
            }
            Item item = new Item(resultArray[0]);
            this.m_items.Add(item);
            return item;
        }

        private Item[] ApplyDefaultAggregate(Item[] items)
        {
            int aggregateID = this.AggregateID;
            if (aggregateID == 0)
            {
                aggregateID = 1;
            }
            Item[] itemArray = new Item[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                itemArray[i] = new Item(items[i]);
                if (itemArray[i].AggregateID == 0)
                {
                    itemArray[i].AggregateID = aggregateID;
                }
            }
            return itemArray;
        }

        public void ClearItems()
        {
            this.m_server.ReleaseItems(this.GetItems());
            this.m_items.Clear();
        }

        public virtual object Clone()
        {
            Trend trend = (Trend) base.MemberwiseClone();
            trend.m_items = new ItemCollection();
            foreach (Item item in this.m_items)
            {
                trend.m_items.Add(item.Clone());
            }
            trend.m_timestamps = new ItemTimeCollection();
            foreach (DateTime time in this.m_timestamps)
            {
                trend.m_timestamps.Add(time);
            }
            trend.m_subscription = null;
            trend.m_playback = null;
            return trend;
        }

        public IdentifiedResult[] Delete()
        {
            return this.Delete(this.GetItems());
        }

        public IdentifiedResult[] Delete(Item[] items)
        {
            return this.m_server.Delete(this.StartTime, this.EndTime, items);
        }

        public IdentifiedResult[] Delete(object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            return this.Delete(this.GetItems(), requestHandle, callback, out request);
        }

        public IdentifiedResult[] Delete(ItemIdentifier[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            return this.m_server.Delete(this.StartTime, this.EndTime, items, requestHandle, callback, out request);
        }

        public ResultCollection[] DeleteAtTime()
        {
            return this.DeleteAtTime(this.GetItems());
        }

        public ResultCollection[] DeleteAtTime(Item[] items)
        {
            ItemTimeCollection[] timesArray = new ItemTimeCollection[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                timesArray[i] = (ItemTimeCollection) this.Timestamps.Clone();
                timesArray[i].ItemName = items[i].ItemName;
                timesArray[i].ItemPath = items[i].ItemPath;
                timesArray[i].ClientHandle = items[i].ClientHandle;
                timesArray[i].ServerHandle = items[i].ServerHandle;
            }
            return this.m_server.DeleteAtTime(timesArray);
        }

        public IdentifiedResult[] DeleteAtTime(object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            return this.DeleteAtTime(this.GetItems(), requestHandle, callback, out request);
        }

        public IdentifiedResult[] DeleteAtTime(Item[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            ItemTimeCollection[] timesArray = new ItemTimeCollection[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                timesArray[i] = (ItemTimeCollection) this.Timestamps.Clone();
                timesArray[i].ItemName = items[i].ItemName;
                timesArray[i].ItemPath = items[i].ItemPath;
                timesArray[i].ClientHandle = items[i].ClientHandle;
                timesArray[i].ServerHandle = items[i].ServerHandle;
            }
            return this.m_server.DeleteAtTime(timesArray, requestHandle, callback, out request);
        }

        public Item[] GetItems()
        {
            Item[] itemArray = new Item[this.m_items.Count];
            for (int i = 0; i < this.m_items.Count; i++)
            {
                itemArray[i] = this.m_items[i];
            }
            return itemArray;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", this.m_name);
            info.AddValue("AggregateID", this.m_aggregateID);
            info.AddValue("StartTime", this.m_startTime);
            info.AddValue("EndTime", this.m_endTime);
            info.AddValue("MaxValues", this.m_maxValues);
            info.AddValue("IncludeBounds", this.m_includeBounds);
            info.AddValue("ResampleInterval", this.m_resampleInterval);
            info.AddValue("UpdateInterval", this.m_updateInterval);
            info.AddValue("PlaybackInterval", this.m_playbackInterval);
            info.AddValue("PlaybackDuration", this.m_playbackDuration);
            DateTime[] timeArray = null;
            if (this.m_timestamps.Count > 0)
            {
                timeArray = new DateTime[this.m_timestamps.Count];
                for (int i = 0; i < timeArray.Length; i++)
                {
                    timeArray[i] = this.m_timestamps[i];
                }
            }
            info.AddValue("Timestamps", timeArray);
            Item[] itemArray = null;
            if (this.m_items.Count > 0)
            {
                itemArray = new Item[this.m_items.Count];
                for (int j = 0; j < itemArray.Length; j++)
                {
                    itemArray[j] = this.m_items[j];
                }
            }
            info.AddValue("Items", itemArray);
        }

        public IdentifiedResult[] Playback(object playbackHandle, DataUpdateEventHandler callback)
        {
            if (this.AggregateID == 0)
            {
                return this.m_server.PlaybackRaw(this.StartTime, this.EndTime, this.MaxValues, this.PlaybackInterval, this.PlaybackDuration, this.GetItems(), playbackHandle, callback, out this.m_playback);
            }
            Item[] items = this.ApplyDefaultAggregate(this.GetItems());
            return this.m_server.PlaybackProcessed(this.StartTime, this.EndTime, this.ResampleInterval, (int) this.PlaybackDuration, this.PlaybackInterval, items, playbackHandle, callback, out this.m_playback);
        }

        public void PlaybackCancel()
        {
            if (this.m_playback != null)
            {
                this.m_server.CancelRequest(this.m_playback);
                this.m_playback = null;
            }
        }

        public ItemValueCollection[] Read()
        {
            return this.Read(this.GetItems());
        }

        public ItemValueCollection[] Read(Item[] items)
        {
            if (this.AggregateID == 0)
            {
                return this.ReadRaw(items);
            }
            return this.ReadProcessed(items);
        }

        public IdentifiedResult[] Read(object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            return this.Read(this.GetItems(), requestHandle, callback, out request);
        }

        public IdentifiedResult[] Read(Item[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            if (this.AggregateID == 0)
            {
                return this.ReadRaw(items, requestHandle, callback, out request);
            }
            return this.ReadProcessed(items, requestHandle, callback, out request);
        }

        public AnnotationValueCollection[] ReadAnnotations()
        {
            return this.ReadAnnotations(this.GetItems());
        }

        public AnnotationValueCollection[] ReadAnnotations(Item[] items)
        {
            return this.m_server.ReadAnnotations(this.StartTime, this.EndTime, items);
        }

        public IdentifiedResult[] ReadAnnotations(object requestHandle, ReadAnnotationsEventHandler callback, out IRequest request)
        {
            return this.ReadAnnotations(this.GetItems(), requestHandle, callback, out request);
        }

        public IdentifiedResult[] ReadAnnotations(Item[] items, object requestHandle, ReadAnnotationsEventHandler callback, out IRequest request)
        {
            return this.m_server.ReadAnnotations(this.StartTime, this.EndTime, items, requestHandle, callback, out request);
        }

        public ItemValueCollection[] ReadAtTime()
        {
            return this.ReadAtTime(this.GetItems());
        }

        public ItemValueCollection[] ReadAtTime(Item[] items)
        {
            DateTime[] timestamps = new DateTime[this.Timestamps.Count];
            for (int i = 0; i < this.Timestamps.Count; i++)
            {
                timestamps[i] = this.Timestamps[i];
            }
            return this.m_server.ReadAtTime(timestamps, items);
        }

        public IdentifiedResult[] ReadAtTime(object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            return this.ReadAtTime(this.GetItems(), requestHandle, callback, out request);
        }

        public IdentifiedResult[] ReadAtTime(Item[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            DateTime[] timestamps = new DateTime[this.Timestamps.Count];
            for (int i = 0; i < this.Timestamps.Count; i++)
            {
                timestamps[i] = this.Timestamps[i];
            }
            return this.m_server.ReadAtTime(timestamps, items, requestHandle, callback, out request);
        }

        public ItemAttributeCollection ReadAttributes(ItemIdentifier item, int[] attributeIDs)
        {
            return this.m_server.ReadAttributes(this.StartTime, this.EndTime, item, attributeIDs);
        }

        public ResultCollection ReadAttributes(ItemIdentifier item, int[] attributeIDs, object requestHandle, ReadAttributesEventHandler callback, out IRequest request)
        {
            return this.m_server.ReadAttributes(this.StartTime, this.EndTime, item, attributeIDs, requestHandle, callback, out request);
        }

        public ModifiedValueCollection[] ReadModified()
        {
            return this.ReadModified(this.GetItems());
        }

        public ModifiedValueCollection[] ReadModified(Item[] items)
        {
            return this.m_server.ReadModified(this.StartTime, this.EndTime, this.MaxValues, items);
        }

        public IdentifiedResult[] ReadModified(object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            return this.ReadModified(this.GetItems(), requestHandle, callback, out request);
        }

        public IdentifiedResult[] ReadModified(Item[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            return this.m_server.ReadModified(this.StartTime, this.EndTime, this.MaxValues, items, requestHandle, callback, out request);
        }

        public ItemValueCollection[] ReadProcessed()
        {
            return this.ReadProcessed(this.GetItems());
        }

        public ItemValueCollection[] ReadProcessed(Item[] items)
        {
            Item[] itemArray = this.ApplyDefaultAggregate(items);
            return this.m_server.ReadProcessed(this.StartTime, this.EndTime, this.ResampleInterval, itemArray);
        }

        public IdentifiedResult[] ReadProcessed(object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            return this.ReadProcessed(this.GetItems(), requestHandle, callback, out request);
        }

        public IdentifiedResult[] ReadProcessed(Item[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            Item[] itemArray = this.ApplyDefaultAggregate(items);
            return this.m_server.ReadProcessed(this.StartTime, this.EndTime, this.ResampleInterval, itemArray, requestHandle, callback, out request);
        }

        public ItemValueCollection[] ReadRaw()
        {
            return this.ReadRaw(this.GetItems());
        }

        public ItemValueCollection[] ReadRaw(Item[] items)
        {
            return this.m_server.ReadRaw(this.StartTime, this.EndTime, this.MaxValues, this.IncludeBounds, items);
        }

        public IdentifiedResult[] ReadRaw(object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            return this.Read(this.GetItems(), requestHandle, callback, out request);
        }

        public IdentifiedResult[] ReadRaw(ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            return this.m_server.ReadRaw(this.StartTime, this.EndTime, this.MaxValues, this.IncludeBounds, items, requestHandle, callback, out request);
        }

        public void RemoveItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            for (int i = 0; i < this.m_items.Count; i++)
            {
                if (item.Equals(this.m_items[i]))
                {
                    this.m_server.ReleaseItems(new ItemIdentifier[] { item });
                    this.m_items.RemoveAt(i);
                    return;
                }
            }
            throw new ArgumentOutOfRangeException("item", item.Key, "Item not found in collection.");
        }

        internal void SetServer(Opc.Hda.Server server)
        {
            this.m_server = server;
        }

        public IdentifiedResult[] Subscribe(object subscriptionHandle, DataUpdateEventHandler callback)
        {
            if (this.AggregateID == 0)
            {
                return this.m_server.AdviseRaw(this.StartTime, this.UpdateInterval, this.GetItems(), subscriptionHandle, callback, out this.m_subscription);
            }
            Item[] items = this.ApplyDefaultAggregate(this.GetItems());
            return this.m_server.AdviseProcessed(this.StartTime, this.ResampleInterval, (int) this.UpdateInterval, items, subscriptionHandle, callback, out this.m_subscription);
        }

        public void SubscribeCancel()
        {
            if (this.m_subscription != null)
            {
                this.m_server.CancelRequest(this.m_subscription);
                this.m_subscription = null;
            }
        }

        public int AggregateID
        {
            get
            {
                return this.m_aggregateID;
            }
            set
            {
                this.m_aggregateID = value;
            }
        }

        public Time EndTime
        {
            get
            {
                return this.m_endTime;
            }
            set
            {
                this.m_endTime = value;
            }
        }

        public bool IncludeBounds
        {
            get
            {
                return this.m_includeBounds;
            }
            set
            {
                this.m_includeBounds = value;
            }
        }

        public ItemCollection Items
        {
            get
            {
                return this.m_items;
            }
        }

        public int MaxValues
        {
            get
            {
                return this.m_maxValues;
            }
            set
            {
                this.m_maxValues = value;
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
            set
            {
                this.m_name = value;
            }
        }

        public bool PlaybackActive
        {
            get
            {
                return (this.m_playback != null);
            }
        }

        public decimal PlaybackDuration
        {
            get
            {
                return this.m_playbackDuration;
            }
            set
            {
                this.m_playbackDuration = value;
            }
        }

        public decimal PlaybackInterval
        {
            get
            {
                return this.m_playbackInterval;
            }
            set
            {
                this.m_playbackInterval = value;
            }
        }

        public decimal ResampleInterval
        {
            get
            {
                return this.m_resampleInterval;
            }
            set
            {
                this.m_resampleInterval = value;
            }
        }

        public Opc.Hda.Server Server
        {
            get
            {
                return this.m_server;
            }
        }

        public Time StartTime
        {
            get
            {
                return this.m_startTime;
            }
            set
            {
                this.m_startTime = value;
            }
        }

        public bool SubscriptionActive
        {
            get
            {
                return (this.m_subscription != null);
            }
        }

        public ItemTimeCollection Timestamps
        {
            get
            {
                return this.m_timestamps;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.m_timestamps = value;
            }
        }

        public decimal UpdateInterval
        {
            get
            {
                return this.m_updateInterval;
            }
            set
            {
                this.m_updateInterval = value;
            }
        }

        private class Names
        {
            internal const string AGGREGATE_ID = "AggregateID";
            internal const string END_TIME = "EndTime";
            internal const string INCLUDE_BOUNDS = "IncludeBounds";
            internal const string ITEMS = "Items";
            internal const string MAX_VALUES = "MaxValues";
            internal const string NAME = "Name";
            internal const string PLAYBACK_DURATION = "PlaybackDuration";
            internal const string PLAYBACK_INTERVAL = "PlaybackInterval";
            internal const string RESAMPLE_INTERVAL = "ResampleInterval";
            internal const string START_TIME = "StartTime";
            internal const string TIMESTAMPS = "Timestamps";
            internal const string UPDATE_INTERVAL = "UpdateInterval";
        }
    }
}

