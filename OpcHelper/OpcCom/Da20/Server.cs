namespace Jund.OpcHelper.OpcCom.Da20
{
    using Opc;
    using Opc.Da;
    using OpcCom;
    using OpcCom.Da;
    using OpcRcw.Comn;
    using OpcRcw.Da;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public class Server : OpcCom.Da.Server
    {
        private object m_group;
        private int m_groupHandle;

        internal Server()
        {
            this.m_group = null;
            this.m_groupHandle = 0;
        }

        internal Server(URL url, object server) : base(url, server)
        {
            this.m_group = null;
            this.m_groupHandle = 0;
        }

        private IdentifiedResult[] AddItems(Item[] items)
        {
            int length = items.Length;
            OPCITEMDEF[] oPCITEMDEFs = OpcCom.Da.Interop.GetOPCITEMDEFs(items);
            for (int i = 0; i < oPCITEMDEFs.Length; i++)
            {
                oPCITEMDEFs[i].bActive = 0;
                oPCITEMDEFs[i].vtRequestedDataType = 0;
            }
            IntPtr zero = IntPtr.Zero;
            IntPtr ppErrors = IntPtr.Zero;
            int pdwLcid = 0;
            ((IOPCCommon) base.m_server).GetLocaleID(out pdwLcid);
            GCHandle handle = GCHandle.Alloc(pdwLcid, GCHandleType.Pinned);
            try
            {
                int pRevisedUpdateRate = 0;
                ((IOPCGroupStateMgt) this.m_group).SetState(IntPtr.Zero, out pRevisedUpdateRate, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, handle.AddrOfPinnedObject(), IntPtr.Zero);
            }
            catch (Exception exception)
            {
                throw OpcCom.Interop.CreateException("IOPCGroupStateMgt.SetState", exception);
            }
            finally
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }
            try
            {
                ((IOPCItemMgt) this.m_group).AddItems(length, oPCITEMDEFs, out zero, out ppErrors);
            }
            catch (Exception exception2)
            {
                throw OpcCom.Interop.CreateException("IOPCItemMgt.AddItems", exception2);
            }
            finally
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }
            int[] numArray = OpcCom.Da.Interop.GetItemResults(ref zero, length, true);
            int[] numArray2 = OpcCom.Interop.GetInt32s(ref ppErrors, length, true);
            IdentifiedResult[] resultArray = new IdentifiedResult[length];
            for (int j = 0; j < length; j++)
            {
                resultArray[j] = new IdentifiedResult(items[j]);
                resultArray[j].ServerHandle = null;
                resultArray[j].ResultID = OpcCom.Interop.GetResultID(numArray2[j]);
                resultArray[j].DiagnosticInfo = null;
                if (resultArray[j].ResultID.Succeeded())
                {
                    resultArray[j].ServerHandle = numArray[j];
                }
            }
            return resultArray;
        }

        public override BrowseElement[] Browse(ItemIdentifier itemID, BrowseFilters filters, out Opc.Da.BrowsePosition position)
        {
            if (filters == null)
            {
                throw new ArgumentNullException("filters");
            }
            position = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                OpcCom.Da20.BrowsePosition position2 = null;
                ArrayList list = new ArrayList();
                if (filters.BrowseFilter != browseFilter.item)
                {
                    BrowseElement[] c = this.GetElements(list.Count, itemID, filters, true, ref position2);
                    if (c != null)
                    {
                        list.AddRange(c);
                    }
                    position = position2;
                    if (position != null)
                    {
                        return (BrowseElement[]) list.ToArray(typeof(BrowseElement));
                    }
                }
                if (filters.BrowseFilter != browseFilter.branch)
                {
                    BrowseElement[] elementArray2 = this.GetElements(list.Count, itemID, filters, false, ref position2);
                    if (elementArray2 != null)
                    {
                        list.AddRange(elementArray2);
                    }
                    position = position2;
                }
                return (BrowseElement[]) list.ToArray(typeof(BrowseElement));
            }
        }

        public override BrowseElement[] BrowseNext(ref Opc.Da.BrowsePosition position)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if ((position == null) && (position.GetType() != typeof(OpcCom.Da20.BrowsePosition)))
                {
                    throw new BrowseCannotContinueException();
                }
                OpcCom.Da20.BrowsePosition position2 = (OpcCom.Da20.BrowsePosition) position;
                ItemIdentifier itemID = position2.ItemID;
                BrowseFilters filters = position2.Filters;
                ArrayList list = new ArrayList();
                if (position2.IsBranch)
                {
                    BrowseElement[] c = this.GetElements(list.Count, itemID, filters, true, ref position2);
                    if (c != null)
                    {
                        list.AddRange(c);
                    }
                    position = position2;
                    if (position != null)
                    {
                        return (BrowseElement[]) list.ToArray(typeof(BrowseElement));
                    }
                }
                if (filters.BrowseFilter != browseFilter.branch)
                {
                    BrowseElement[] elementArray2 = this.GetElements(list.Count, itemID, filters, false, ref position2);
                    if (elementArray2 != null)
                    {
                        list.AddRange(elementArray2);
                    }
                    position = position2;
                }
                return (BrowseElement[]) list.ToArray(typeof(BrowseElement));
            }
        }

        protected override OpcCom.Da.Subscription CreateSubscription(object group, SubscriptionState state, int filters)
        {
            return new OpcCom.Da20.Subscription(group, state, filters);
        }

        public override void Dispose()
        {
            lock (this)
            {
                if (this.m_group != null)
                {
                    OpcCom.Interop.ReleaseServer(this.m_group);
                    this.m_group = null;
                    try
                    {
                        ((IOPCServer) base.m_server).RemoveGroup(this.m_groupHandle, 0);
                    }
                    catch
                    {
                    }
                    this.m_group = null;
                    this.m_groupHandle = 0;
                    base.Dispose();
                }
            }
        }

        private ItemProperty[] GetAvailableProperties(string itemID)
        {
            if ((itemID == null) || (itemID.Length == 0))
            {
                throw new ResultIDException(ResultID.Da.E_INVALID_ITEM_NAME);
            }
            int pdwCount = 0;
            IntPtr zero = IntPtr.Zero;
            IntPtr ppDescriptions = IntPtr.Zero;
            IntPtr ppvtDataTypes = IntPtr.Zero;
            try
            {
                ((IOPCItemProperties) base.m_server).QueryAvailableProperties(itemID, out pdwCount, out zero, out ppDescriptions, out ppvtDataTypes);
            }
            catch (Exception)
            {
                throw new ResultIDException(ResultID.Da.E_UNKNOWN_ITEM_NAME);
            }
            int[] numArray = OpcCom.Interop.GetInt32s(ref zero, pdwCount, true);
            short[] numArray2 = OpcCom.Interop.GetInt16s(ref ppvtDataTypes, pdwCount, true);
            string[] strArray = OpcCom.Interop.GetUnicodeStrings(ref ppDescriptions, pdwCount, true);
            if (pdwCount == 0)
            {
                return null;
            }
            ItemProperty[] propertyArray = new ItemProperty[pdwCount];
            for (int i = 0; i < pdwCount; i++)
            {
                propertyArray[i] = new ItemProperty();
                propertyArray[i].ID = OpcCom.Da.Interop.GetPropertyID(numArray[i]);
                propertyArray[i].Description = strArray[i];
                propertyArray[i].DataType = OpcCom.Interop.GetType((VarEnum) numArray2[i]);
                propertyArray[i].ItemName = null;
                propertyArray[i].ItemPath = null;
                propertyArray[i].ResultID = ResultID.S_OK;
                propertyArray[i].Value = null;
            }
            return propertyArray;
        }

        private BrowseElement GetElement(string name, BrowseFilters filters, bool isBranch)
        {
            if (name == null)
            {
                return null;
            }
            BrowseElement element = new BrowseElement {
                Name = name,
                HasChildren = isBranch,
                ItemPath = null
            };
            try
            {
                string szItemID = null;
                ((IOPCBrowseServerAddressSpace) base.m_server).GetItemID(element.Name, out szItemID);
                element.ItemName = szItemID;
                OPCITEMDEF opcitemdef = new OPCITEMDEF {
                    szItemID = element.ItemName,
                    szAccessPath = null,
                    hClient = 0,
                    bActive = 0,
                    vtRequestedDataType = 0,
                    dwBlobSize = 0,
                    pBlob = IntPtr.Zero
                };
                IntPtr zero = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                OPCITEMDEF[] pItemArray = new OPCITEMDEF[] { opcitemdef };
                ((IOPCItemMgt) this.m_group).ValidateItems(1, pItemArray, 0, out zero, out ppErrors);
                OpcCom.Da.Interop.GetItemResults(ref zero, 1, true);
                int[] numArray = OpcCom.Interop.GetInt32s(ref ppErrors, 1, true);
                element.IsItem = numArray[0] >= 0;
            }
            catch
            {
                element.ItemName = null;
            }
            try
            {
                if (filters.ReturnAllProperties)
                {
                    element.Properties = this.GetProperties(element.ItemName, null, filters.ReturnPropertyValues);
                    return element;
                }
                if (filters.PropertyIDs != null)
                {
                    element.Properties = this.GetProperties(element.ItemName, filters.PropertyIDs, filters.ReturnPropertyValues);
                }
            }
            catch
            {
                element.Properties = null;
            }
            return element;
        }

        private BrowseElement[] GetElements(int elementsFound, ItemIdentifier itemID, BrowseFilters filters, bool branches, ref OpcCom.Da20.BrowsePosition position)
        {
            EnumString enumerator = null;
            if (position == null)
            {
                IOPCBrowseServerAddressSpace server = (IOPCBrowseServerAddressSpace) base.m_server;
                OPCNAMESPACETYPE pNameSpaceType = OPCNAMESPACETYPE.OPC_NS_HIERARCHIAL;
                try
                {
                    server.QueryOrganization(out pNameSpaceType);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCBrowseServerAddressSpace.QueryOrganization", exception);
                }
                if ((pNameSpaceType == OPCNAMESPACETYPE.OPC_NS_FLAT) && branches)
                {
                    return new BrowseElement[0];
                }
                enumerator = this.GetEnumerator((itemID != null) ? itemID.ItemName : null, filters, branches);
            }
            else
            {
                enumerator = position.Enumerator;
            }
            ArrayList list = new ArrayList();
            BrowseElement element = null;
            int index = 0;
            string[] names = null;
            if (position != null)
            {
                index = position.Index;
                names = position.Names;
                position = null;
            }
        Label_0089:
            if (names != null)
            {
                for (int i = index; i < names.Length; i++)
                {
                    if ((filters.MaxElementsReturned != 0) && (filters.MaxElementsReturned == (list.Count + elementsFound)))
                    {
                        position = new OpcCom.Da20.BrowsePosition(itemID, filters, enumerator, branches);
                        position.Names = names;
                        position.Index = i;
                        break;
                    }
                    element = this.GetElement(names[i], filters, branches);
                    if (element == null)
                    {
                        break;
                    }
                    list.Add(element);
                }
            }
            if (position == null)
            {
                names = enumerator.Next(10);
                index = 0;
                if ((names != null) && (names.Length > 0))
                {
                    goto Label_0089;
                }
            }
            if (position == null)
            {
                enumerator.Dispose();
            }
            return (BrowseElement[]) list.ToArray(typeof(BrowseElement));
        }

        private EnumString GetEnumerator(string itemID, BrowseFilters filters, bool branches)
        {
            EnumString str2;
            IOPCBrowseServerAddressSpace server = (IOPCBrowseServerAddressSpace) base.m_server;
            try
            {
                server.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_TO, (itemID != null) ? itemID : "");
            }
            catch
            {
                try
                {
                    server.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_DOWN, (itemID != null) ? itemID : "");
                }
                catch
                {
                    throw new ResultIDException(ResultID.Da.E_UNKNOWN_ITEM_NAME);
                }
            }
            try
            {
                OpcRcw.Da.IEnumString ppIEnumString = null;
                server.BrowseOPCItemIDs(branches ? OPCBROWSETYPE.OPC_BRANCH : OPCBROWSETYPE.OPC_LEAF, (filters.ElementNameFilter != null) ? filters.ElementNameFilter : "", 0, 0, out ppIEnumString);
                str2 = new EnumString(ppIEnumString);
            }
            catch
            {
                throw new ResultIDException(ResultID.Da.E_UNKNOWN_ITEM_NAME);
            }
            return str2;
        }

        private void GetItemIDs(string itemID, ItemProperty[] properties)
        {
            try
            {
                int[] pdwPropertyIDs = new int[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    pdwPropertyIDs[i] = properties[i].ID.Code;
                }
                IntPtr zero = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCItemProperties) base.m_server).LookupItemIDs(itemID, properties.Length, pdwPropertyIDs, out zero, out ppErrors);
                string[] strArray = OpcCom.Interop.GetUnicodeStrings(ref zero, properties.Length, true);
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref ppErrors, properties.Length, true);
                for (int j = 0; j < properties.Length; j++)
                {
                    properties[j].ItemName = null;
                    properties[j].ItemPath = null;
                    if (numArray2[j] >= 0)
                    {
                        properties[j].ItemName = strArray[j];
                    }
                }
            }
            catch
            {
                foreach (ItemProperty property in properties)
                {
                    property.ItemName = null;
                    property.ItemPath = null;
                }
            }
        }

        private ItemProperty[] GetProperties(string itemID, PropertyID[] propertyIDs, bool returnValues)
        {
            ItemProperty[] properties = null;
            if (propertyIDs == null)
            {
                properties = this.GetAvailableProperties(itemID);
            }
            else
            {
                ItemProperty[] availableProperties = this.GetAvailableProperties(itemID);
                properties = new ItemProperty[propertyIDs.Length];
                for (int i = 0; i < propertyIDs.Length; i++)
                {
                    foreach (ItemProperty property in availableProperties)
                    {
                        if (property.ID == propertyIDs[i])
                        {
                            properties[i] = (ItemProperty) property.Clone();
                            properties[i].ID = propertyIDs[i];
                            break;
                        }
                    }
                    if (properties[i] == null)
                    {
                        properties[i] = new ItemProperty();
                        properties[i].ID = propertyIDs[i];
                        properties[i].ResultID = ResultID.Da.E_INVALID_PID;
                    }
                }
            }
            if (properties != null)
            {
                this.GetItemIDs(itemID, properties);
                if (returnValues)
                {
                    this.GetValues(itemID, properties);
                }
            }
            return properties;
        }

        public override ItemPropertyCollection[] GetProperties(ItemIdentifier[] itemIDs, PropertyID[] propertyIDs, bool returnValues)
        {
            if (itemIDs == null)
            {
                throw new ArgumentNullException("itemIDs");
            }
            if (itemIDs.Length == 0)
            {
                return new ItemPropertyCollection[0];
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                ItemPropertyCollection[] propertysArray = new ItemPropertyCollection[itemIDs.Length];
                for (int i = 0; i < itemIDs.Length; i++)
                {
                    propertysArray[i] = new ItemPropertyCollection();
                    propertysArray[i].ItemName = itemIDs[i].ItemName;
                    propertysArray[i].ItemPath = itemIDs[i].ItemPath;
                    try
                    {
                        ItemProperty[] c = this.GetProperties(itemIDs[i].ItemName, propertyIDs, returnValues);
                        if (c != null)
                        {
                            propertysArray[i].AddRange(c);
                        }
                        propertysArray[i].ResultID = ResultID.S_OK;
                    }
                    catch (ResultIDException exception)
                    {
                        propertysArray[i].ResultID = exception.Result;
                    }
                    catch (Exception exception2)
                    {
                        propertysArray[i].ResultID = new ResultID((long) Marshal.GetHRForException(exception2));
                    }
                }
                return propertysArray;
            }
        }

        private void GetValues(string itemID, ItemProperty[] properties)
        {
            try
            {
                int[] pdwPropertyIDs = new int[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    pdwPropertyIDs[i] = properties[i].ID.Code;
                }
                IntPtr zero = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCItemProperties) base.m_server).GetItemProperties(itemID, properties.Length, pdwPropertyIDs, out zero, out ppErrors);
                object[] objArray = OpcCom.Interop.GetVARIANTs(ref zero, properties.Length, true);
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref ppErrors, properties.Length, true);
                for (int j = 0; j < properties.Length; j++)
                {
                    properties[j].Value = null;
                    if (properties[j].ResultID.Succeeded())
                    {
                        properties[j].ResultID = OpcCom.Interop.GetResultID(numArray2[j]);
                        if (numArray2[j] == -1073479674)
                        {
                            properties[j].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                        }
                        if (properties[j].ResultID.Succeeded())
                        {
                            properties[j].Value = OpcCom.Da.Interop.UnmarshalPropertyValue(properties[j].ID, objArray[j]);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ResultID tid = new ResultID((long) Marshal.GetHRForException(exception));
                foreach (ItemProperty property in properties)
                {
                    property.Value = null;
                    property.ResultID = tid;
                }
            }
        }

        public override void Initialize(URL url, ConnectData connectData)
        {
            lock (this)
            {
                base.Initialize(url, connectData);
                try
                {
                    int pdwLcid = 0;
                    ((IOPCCommon) base.m_server).GetLocaleID(out pdwLcid);
                    Guid gUID = typeof(IOPCItemMgt).GUID;
                    int pRevisedUpdateRate = 0;
                    ((IOPCServer) base.m_server).AddGroup("", 1, 0, 0, IntPtr.Zero, IntPtr.Zero, pdwLcid, out this.m_groupHandle, out pRevisedUpdateRate, ref gUID, out this.m_group);
                }
                catch (Exception exception)
                {
                    this.Uninitialize();
                    throw OpcCom.Interop.CreateException("IOPCServer.AddGroup", exception);
                }
            }
        }

        public override ItemValueResult[] Read(Item[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (items.Length == 0)
            {
                return new ItemValueResult[0];
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                IdentifiedResult[] resultArray = this.AddItems(items);
                ItemValueResult[] resultArray2 = new ItemValueResult[items.Length];
                try
                {
                    ArrayList list = new ArrayList(items.Length);
                    ArrayList list2 = new ArrayList(items.Length);
                    ArrayList list3 = new ArrayList(items.Length);
                    ArrayList list4 = new ArrayList(items.Length);
                    for (int i = 0; i < items.Length; i++)
                    {
                        resultArray2[i] = new ItemValueResult(resultArray[i]);
                        if (resultArray[i].ResultID.Failed())
                        {
                            resultArray2[i].ResultID = resultArray[i].ResultID;
                            resultArray2[i].DiagnosticInfo = resultArray[i].DiagnosticInfo;
                        }
                        else if (items[i].MaxAgeSpecified && ((items[i].MaxAge < 0) || (items[i].MaxAge == 0x7fffffff)))
                        {
                            list.Add(items[i]);
                            list2.Add(resultArray2[i]);
                        }
                        else
                        {
                            list3.Add(items[i]);
                            list4.Add(resultArray2[i]);
                        }
                    }
                    if (list2.Count > 0)
                    {
                        try
                        {
                            int[] phServer = new int[list2.Count];
                            for (int j = 0; j < list2.Count; j++)
                            {
                                phServer[j] = (int) ((ItemValueResult) list2[j]).ServerHandle;
                            }
                            IntPtr zero = IntPtr.Zero;
                            ((IOPCItemMgt) this.m_group).SetActiveState(list2.Count, phServer, 1, out zero);
                            Marshal.FreeCoTaskMem(zero);
                        }
                        catch (Exception exception)
                        {
                            throw OpcCom.Interop.CreateException("IOPCItemMgt.SetActiveState", exception);
                        }
                        this.ReadValues((Item[]) list.ToArray(typeof(Item)), (ItemValueResult[]) list2.ToArray(typeof(ItemValueResult)), true);
                    }
                    if (list4.Count > 0)
                    {
                        this.ReadValues((Item[]) list3.ToArray(typeof(Item)), (ItemValueResult[]) list4.ToArray(typeof(ItemValueResult)), false);
                    }
                }
                finally
                {
                    this.RemoveItems(resultArray);
                }
                return resultArray2;
            }
        }

        private void ReadValues(Item[] items, ItemValueResult[] results, bool cache)
        {
            if ((items.Length != 0) && (results.Length != 0))
            {
                int[] phServer = new int[results.Length];
                for (int i = 0; i < results.Length; i++)
                {
                    phServer[i] = System.Convert.ToInt32(results[i].ServerHandle);
                }
                IntPtr zero = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCSyncIO) this.m_group).Read(cache ? OPCDATASOURCE.OPC_DS_CACHE : OPCDATASOURCE.OPC_DS_DEVICE, results.Length, phServer, out zero, out ppErrors);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCSyncIO.Read", exception);
                }
                ItemValue[] valueArray = OpcCom.Da.Interop.GetItemValues(ref zero, results.Length, true);
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref ppErrors, results.Length, true);
                this.GetLocale();
                for (int j = 0; j < results.Length; j++)
                {
                    results[j].ResultID = OpcCom.Interop.GetResultID(numArray2[j]);
                    results[j].DiagnosticInfo = null;
                    if (results[j].ResultID.Succeeded())
                    {
                        results[j].Value = valueArray[j].Value;
                        results[j].Quality = valueArray[j].Quality;
                        results[j].QualitySpecified = valueArray[j].QualitySpecified;
                        results[j].Timestamp = valueArray[j].Timestamp;
                        results[j].TimestampSpecified = valueArray[j].TimestampSpecified;
                    }
                    if (numArray2[j] == -1073479674)
                    {
                        results[j].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                    }
                    if ((results[j].Value != null) && (items[j].ReqType != null))
                    {
                        try
                        {
                            results[j].Value = base.ChangeType(results[j].Value, items[j].ReqType, "en-US");
                        }
                        catch (Exception exception2)
                        {
                            results[j].Value = null;
                            results[j].Quality = Quality.Bad;
                            results[j].QualitySpecified = true;
                            results[j].Timestamp = DateTime.MinValue;
                            results[j].TimestampSpecified = false;
                            if (exception2.GetType() == typeof(OverflowException))
                            {
                                results[j].ResultID = OpcCom.Interop.GetResultID(-1073479669);
                            }
                            else
                            {
                                results[j].ResultID = OpcCom.Interop.GetResultID(-1073479676);
                            }
                        }
                    }
                }
            }
        }

        private void RemoveItems(IdentifiedResult[] items)
        {
            try
            {
                ArrayList list = new ArrayList(items.Length);
                foreach (IdentifiedResult result in items)
                {
                    if (result.ResultID.Succeeded() && (result.ServerHandle.GetType() == typeof(int)))
                    {
                        list.Add((int) result.ServerHandle);
                    }
                }
                if (list.Count != 0)
                {
                    IntPtr zero = IntPtr.Zero;
                    ((IOPCItemMgt) this.m_group).RemoveItems(list.Count, (int[]) list.ToArray(typeof(int)), out zero);
                    OpcCom.Interop.GetInt32s(ref zero, list.Count, true);
                }
            }
            catch
            {
            }
        }

        public override IdentifiedResult[] Write(ItemValue[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (items.Length == 0)
            {
                return new IdentifiedResult[0];
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                Item[] itemArray = new Item[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    itemArray[i] = new Item(items[i]);
                }
                IdentifiedResult[] resultArray = this.AddItems(itemArray);
                try
                {
                    ArrayList list = new ArrayList(items.Length);
                    ArrayList list2 = new ArrayList(items.Length);
                    for (int j = 0; j < items.Length; j++)
                    {
                        if (!resultArray[j].ResultID.Failed())
                        {
                            if (items[j].QualitySpecified || items[j].TimestampSpecified)
                            {
                                resultArray[j].ResultID = ResultID.Da.E_NO_WRITEQT;
                                resultArray[j].DiagnosticInfo = null;
                            }
                            else
                            {
                                list.Add(resultArray[j]);
                                list2.Add(items[j]);
                            }
                        }
                    }
                    if (list.Count > 0)
                    {
                        int[] phServer = new int[list.Count];
                        object[] pItemValues = new object[list.Count];
                        for (int k = 0; k < phServer.Length; k++)
                        {
                            phServer[k] = (int) ((IdentifiedResult) list[k]).ServerHandle;
                            pItemValues[k] = OpcCom.Interop.GetVARIANT(((ItemValue) list2[k]).Value);
                        }
                        IntPtr zero = IntPtr.Zero;
                        try
                        {
                            ((IOPCSyncIO) this.m_group).Write(list.Count, phServer, pItemValues, out zero);
                        }
                        catch (Exception exception)
                        {
                            throw OpcCom.Interop.CreateException("IOPCSyncIO.Write", exception);
                        }
                        int[] numArray2 = OpcCom.Interop.GetInt32s(ref zero, list.Count, true);
                        for (int m = 0; m < list.Count; m++)
                        {
                            IdentifiedResult result = (IdentifiedResult) list[m];
                            result.ResultID = OpcCom.Interop.GetResultID(numArray2[m]);
                            result.DiagnosticInfo = null;
                            if (numArray2[m] == -1073479674)
                            {
                                resultArray[m].ResultID = new ResultID(ResultID.Da.E_READONLY, -1073479674L);
                            }
                        }
                    }
                }
                finally
                {
                    this.RemoveItems(resultArray);
                }
                return resultArray;
            }
        }
    }
}

