namespace Jund.OpcHelper.Opc.Cpx
{
    using Opc;
    using Opc.Da;
    using System;
    using System.Collections;
    using System.IO;
    using System.Xml;

    public class ComplexItem : ItemIdentifier
    {
        public const string CPX_BRANCH = "CPX";
        public const string CPX_DATA_FILTERS = "DataFilters";
        public static readonly PropertyID[] CPX_PROPERTIES = new PropertyID[] { Property.TYPE_SYSTEM_ID, Property.DICTIONARY_ID, Property.TYPE_ID, Property.UNCONVERTED_ITEM_ID, Property.UNFILTERED_ITEM_ID, Property.DATA_FILTER_VALUE };
        private string m_dictionaryID;
        private ItemIdentifier m_dictionaryItemID;
        private ItemIdentifier m_filterItem;
        private string m_filterValue;
        private string m_name;
        private string m_typeID;
        private ItemIdentifier m_typeItemID;
        private string m_typeSystemID;
        private ItemIdentifier m_unconvertedItemID;
        private ItemIdentifier m_unfilteredItemID;

        public ComplexItem()
        {
            this.m_name = null;
            this.m_typeSystemID = null;
            this.m_dictionaryID = null;
            this.m_typeID = null;
            this.m_dictionaryItemID = null;
            this.m_typeItemID = null;
            this.m_unconvertedItemID = null;
            this.m_unfilteredItemID = null;
            this.m_filterItem = null;
            this.m_filterValue = null;
        }

        public ComplexItem(ItemIdentifier itemID)
        {
            this.m_name = null;
            this.m_typeSystemID = null;
            this.m_dictionaryID = null;
            this.m_typeID = null;
            this.m_dictionaryItemID = null;
            this.m_typeItemID = null;
            this.m_unconvertedItemID = null;
            this.m_unfilteredItemID = null;
            this.m_filterItem = null;
            this.m_filterValue = null;
            base.ItemPath = itemID.ItemPath;
            base.ItemName = itemID.ItemName;
        }

        private void Clear()
        {
            this.m_typeSystemID = null;
            this.m_dictionaryID = null;
            this.m_typeID = null;
            this.m_dictionaryItemID = null;
            this.m_typeItemID = null;
            this.m_unconvertedItemID = null;
            this.m_unfilteredItemID = null;
            this.m_filterItem = null;
            this.m_filterValue = null;
        }

        public ComplexItem CreateDataFilter(Opc.Da.Server server, string filterName, string filterValue)
        {
            ComplexItem item2;
            if (this.m_unfilteredItemID != null)
            {
                return null;
            }
            if (this.m_filterItem == null)
            {
                return null;
            }
            BrowsePosition position = null;
            try
            {
                ItemValue value2 = new ItemValue(this.m_filterItem);
                StringWriter w = new StringWriter();
                XmlTextWriter writer2 = new XmlTextWriter(w);
                writer2.WriteStartElement("DataFilters");
                writer2.WriteAttributeString("Name", filterName);
                writer2.WriteString(filterValue);
                writer2.WriteEndElement();
                writer2.Close();
                value2.Value = w.ToString();
                value2.Quality = Quality.Bad;
                value2.QualitySpecified = false;
                value2.Timestamp = DateTime.MinValue;
                value2.TimestampSpecified = false;
                IdentifiedResult[] resultArray = server.Write(new ItemValue[] { value2 });
                if ((resultArray == null) || (resultArray.Length == 0))
                {
                    throw new ApplicationException("Unexpected result from server.");
                }
                if (resultArray[0].ResultID.Failed())
                {
                    throw new ApplicationException("Could not create new data filter.");
                }
                BrowseFilters filters = new BrowseFilters {
                    ElementNameFilter = filterName,
                    BrowseFilter = browseFilter.item,
                    ReturnAllProperties = false,
                    PropertyIDs = CPX_PROPERTIES,
                    ReturnPropertyValues = true
                };
                BrowseElement[] elementArray = server.Browse(this.m_filterItem, filters, out position);
                if ((elementArray == null) || (elementArray.Length == 0))
                {
                    throw new ApplicationException("Could not browse to new data filter.");
                }
                ComplexItem item = new ComplexItem();
                if (!item.Init(elementArray[0]))
                {
                    throw new ApplicationException("Could not initialize to new data filter.");
                }
                item2 = item;
            }
            finally
            {
                if (position != null)
                {
                    position.Dispose();
                    position = null;
                }
            }
            return item2;
        }

        public void GetDataFilterItem(Opc.Da.Server server)
        {
            this.m_filterItem = null;
            if (this.m_unfilteredItemID == null)
            {
                BrowsePosition position = null;
                try
                {
                    ItemIdentifier itemID = new ItemIdentifier(this);
                    BrowseFilters filters = new BrowseFilters {
                        ElementNameFilter = "DataFilters",
                        BrowseFilter = browseFilter.all,
                        ReturnAllProperties = false,
                        PropertyIDs = null,
                        ReturnPropertyValues = false
                    };
                    BrowseElement[] elementArray = null;
                    if (this.m_unconvertedItemID == null)
                    {
                        filters.ElementNameFilter = "CPX";
                        elementArray = server.Browse(itemID, filters, out position);
                        if ((elementArray == null) || (elementArray.Length == 0))
                        {
                            return;
                        }
                        if (position != null)
                        {
                            position.Dispose();
                            position = null;
                        }
                        itemID = new ItemIdentifier(elementArray[0].ItemPath, elementArray[0].ItemName);
                        filters.ElementNameFilter = "DataFilters";
                    }
                    elementArray = server.Browse(itemID, filters, out position);
                    if ((elementArray != null) && (elementArray.Length != 0))
                    {
                        this.m_filterItem = new ItemIdentifier(elementArray[0].ItemPath, elementArray[0].ItemName);
                    }
                }
                finally
                {
                    if (position != null)
                    {
                        position.Dispose();
                        position = null;
                    }
                }
            }
        }

        public ComplexItem[] GetDataFilters(Opc.Da.Server server)
        {
            ComplexItem[] itemArray;
            if (this.m_unfilteredItemID != null)
            {
                return null;
            }
            if (this.m_filterItem == null)
            {
                return null;
            }
            BrowsePosition position = null;
            try
            {
                BrowseFilters filters = new BrowseFilters {
                    ElementNameFilter = null,
                    BrowseFilter = browseFilter.item,
                    ReturnAllProperties = false,
                    PropertyIDs = CPX_PROPERTIES,
                    ReturnPropertyValues = true
                };
                BrowseElement[] elementArray = server.Browse(this.m_filterItem, filters, out position);
                if ((elementArray == null) || (elementArray.Length == 0))
                {
                    return new ComplexItem[0];
                }
                ArrayList list = new ArrayList(elementArray.Length);
                foreach (BrowseElement element in elementArray)
                {
                    ComplexItem item = new ComplexItem();
                    if (item.Init(element))
                    {
                        list.Add(item);
                    }
                }
                itemArray = (ComplexItem[]) list.ToArray(typeof(ComplexItem));
            }
            finally
            {
                if (position != null)
                {
                    position.Dispose();
                    position = null;
                }
            }
            return itemArray;
        }

        public ComplexItem GetRootItem()
        {
            if (this.m_unconvertedItemID != null)
            {
                return ComplexTypeCache.GetComplexItem(this.m_unconvertedItemID);
            }
            if (this.m_unfilteredItemID != null)
            {
                return ComplexTypeCache.GetComplexItem(this.m_unfilteredItemID);
            }
            return this;
        }

        public ComplexItem[] GetTypeConversions(Opc.Da.Server server)
        {
            ComplexItem[] itemArray;
            if ((this.m_unconvertedItemID != null) || (this.m_unfilteredItemID != null))
            {
                return null;
            }
            BrowsePosition position = null;
            try
            {
                BrowseFilters filters = new BrowseFilters {
                    ElementNameFilter = "CPX",
                    BrowseFilter = browseFilter.branch,
                    ReturnAllProperties = false,
                    PropertyIDs = null,
                    ReturnPropertyValues = false
                };
                BrowseElement[] elementArray = server.Browse(this, filters, out position);
                if ((elementArray == null) || (elementArray.Length == 0))
                {
                    return null;
                }
                if (position != null)
                {
                    position.Dispose();
                    position = null;
                }
                ItemIdentifier itemID = new ItemIdentifier(elementArray[0].ItemPath, elementArray[0].ItemName);
                filters.ElementNameFilter = null;
                filters.BrowseFilter = browseFilter.item;
                filters.ReturnAllProperties = false;
                filters.PropertyIDs = CPX_PROPERTIES;
                filters.ReturnPropertyValues = true;
                elementArray = server.Browse(itemID, filters, out position);
                if ((elementArray == null) || (elementArray.Length == 0))
                {
                    return new ComplexItem[0];
                }
                ArrayList list = new ArrayList(elementArray.Length);
                foreach (BrowseElement element in elementArray)
                {
                    if (element.Name != "DataFilters")
                    {
                        ComplexItem item = new ComplexItem();
                        if (item.Init(element))
                        {
                            item.GetDataFilterItem(server);
                            list.Add(item);
                        }
                    }
                }
                itemArray = (ComplexItem[]) list.ToArray(typeof(ComplexItem));
            }
            finally
            {
                if (position != null)
                {
                    position.Dispose();
                    position = null;
                }
            }
            return itemArray;
        }

        public string GetTypeDescription(Opc.Da.Server server)
        {
            PropertyID[] propertyIDs = new PropertyID[] { Property.TYPE_DESCRIPTION };
            ItemPropertyCollection[] propertysArray = server.GetProperties(new ItemIdentifier[] { this.m_typeItemID }, propertyIDs, true);
            if (((propertysArray == null) || (propertysArray.Length == 0)) || (propertysArray[0].Count == 0))
            {
                return null;
            }
            ItemProperty property = propertysArray[0][0];
            if (!property.ResultID.Succeeded())
            {
                return null;
            }
            return (string) property.Value;
        }

        public string GetTypeDictionary(Opc.Da.Server server)
        {
            PropertyID[] propertyIDs = new PropertyID[] { Property.DICTIONARY };
            ItemPropertyCollection[] propertysArray = server.GetProperties(new ItemIdentifier[] { this.m_dictionaryItemID }, propertyIDs, true);
            if (((propertysArray == null) || (propertysArray.Length == 0)) || (propertysArray[0].Count == 0))
            {
                return null;
            }
            ItemProperty property = propertysArray[0][0];
            if (!property.ResultID.Succeeded())
            {
                return null;
            }
            return (string) property.Value;
        }

        private bool Init(BrowseElement element)
        {
            base.ItemPath = element.ItemPath;
            base.ItemName = element.ItemName;
            this.m_name = element.Name;
            return this.Init(element.Properties);
        }

        private bool Init(ItemProperty[] properties)
        {
            this.Clear();
            if ((properties == null) || (properties.Length < 3))
            {
                return false;
            }
            foreach (ItemProperty property in properties)
            {
                if (property.ResultID.Succeeded())
                {
                    if (property.ID == Property.TYPE_SYSTEM_ID)
                    {
                        this.m_typeSystemID = (string) property.Value;
                    }
                    else if (property.ID == Property.DICTIONARY_ID)
                    {
                        this.m_dictionaryID = (string) property.Value;
                        this.m_dictionaryItemID = new ItemIdentifier(property.ItemPath, property.ItemName);
                    }
                    else if (property.ID == Property.TYPE_ID)
                    {
                        this.m_typeID = (string) property.Value;
                        this.m_typeItemID = new ItemIdentifier(property.ItemPath, property.ItemName);
                    }
                    else if (property.ID == Property.UNCONVERTED_ITEM_ID)
                    {
                        this.m_unconvertedItemID = new ItemIdentifier(base.ItemPath, (string) property.Value);
                    }
                    else if (property.ID == Property.UNFILTERED_ITEM_ID)
                    {
                        this.m_unfilteredItemID = new ItemIdentifier(base.ItemPath, (string) property.Value);
                    }
                    else if (property.ID == Property.DATA_FILTER_VALUE)
                    {
                        this.m_filterValue = (string) property.Value;
                    }
                }
            }
            return (((this.m_typeSystemID != null) && (this.m_dictionaryID != null)) && (this.m_typeID != null));
        }

        public override string ToString()
        {
            if ((this.m_name == null) && (this.m_name.Length == 0))
            {
                return base.ItemName;
            }
            return this.m_name;
        }

        public void Update(Opc.Da.Server server)
        {
            this.Clear();
            ItemPropertyCollection[] propertysArray = server.GetProperties(new ItemIdentifier[] { this }, CPX_PROPERTIES, true);
            if ((propertysArray == null) || (propertysArray.Length != 1))
            {
                throw new ApplicationException("Unexpected results returned from server.");
            }
            if (!this.Init((ItemProperty[]) propertysArray[0].ToArray(typeof(ItemProperty))))
            {
                throw new ApplicationException("Not a valid complex item.");
            }
            this.GetDataFilterItem(server);
        }

        public void UpdateDataFilter(Opc.Da.Server server, string filterValue)
        {
            if (this.m_unfilteredItemID == null)
            {
                throw new ApplicationException("Cannot update the data filter for this item.");
            }
            ItemValue value2 = new ItemValue(this) {
                Value = filterValue,
                Quality = Quality.Bad,
                QualitySpecified = false,
                Timestamp = DateTime.MinValue,
                TimestampSpecified = false
            };
            IdentifiedResult[] resultArray = server.Write(new ItemValue[] { value2 });
            if ((resultArray == null) || (resultArray.Length == 0))
            {
                throw new ApplicationException("Unexpected result from server.");
            }
            if (resultArray[0].ResultID.Failed())
            {
                throw new ApplicationException("Could not update data filter.");
            }
            this.m_filterValue = filterValue;
        }

        public ItemIdentifier DataFilterItem
        {
            get
            {
                return this.m_filterItem;
            }
        }

        public string DataFilterValue
        {
            get
            {
                return this.m_filterValue;
            }
            set
            {
                this.m_filterValue = value;
            }
        }

        public string DictionaryID
        {
            get
            {
                return this.m_dictionaryID;
            }
        }

        public ItemIdentifier DictionaryItemID
        {
            get
            {
                return this.m_dictionaryItemID;
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
        }

        public string TypeID
        {
            get
            {
                return this.m_typeID;
            }
        }

        public ItemIdentifier TypeItemID
        {
            get
            {
                return this.m_typeItemID;
            }
        }

        public string TypeSystemID
        {
            get
            {
                return this.m_typeSystemID;
            }
        }

        public ItemIdentifier UnconvertedItemID
        {
            get
            {
                return this.m_unconvertedItemID;
            }
        }

        public ItemIdentifier UnfilteredItemID
        {
            get
            {
                return this.m_unfilteredItemID;
            }
        }
    }
}

