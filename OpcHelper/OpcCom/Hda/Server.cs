namespace Jund.OpcHelper.OpcCom.Hda
{
    using Opc;
    using Opc.Hda;
    using OpcCom;
    using OpcRcw.Hda;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public class Server : OpcCom.Server, Opc.Hda.IServer, Opc.IServer, IDisposable
    {
        private DataCallback m_callback;
        private ConnectionPoint m_connection;
        private Hashtable m_items;
        private static int NextHandle = 1;

        internal Server()
        {
            this.m_items = new Hashtable();
            this.m_callback = new DataCallback();
            this.m_connection = null;
        }

        internal Server(URL url, object server)
        {
            this.m_items = new Hashtable();
            this.m_callback = new DataCallback();
            this.m_connection = null;
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            base.m_url = (URL) url.Clone();
            base.m_server = server;
            this.Advise();
        }

        private void Advise()
        {
            if (this.m_connection == null)
            {
                try
                {
                    this.m_connection = new ConnectionPoint(base.m_server, typeof(IOPCHDA_DataCallback).GUID);
                    this.m_connection.Advise(this.m_callback);
                }
                catch
                {
                    this.m_connection = null;
                }
            }
        }

        public IdentifiedResult[] AdviseProcessed(Time startTime, decimal resampleInterval, int numberOfIntervals, Item[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            request = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                Request request2 = this.m_callback.CreateRequest(requestHandle, callback);
                int requestID = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = this.GetServerHandles(items);
                int[] aggregateIDs = this.GetAggregateIDs(items);
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_FILETIME fILETIME = OpcCom.Hda.Interop.GetFILETIME(resampleInterval);
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncRead) base.m_server).AdviseProcessed(request2.RequestID, ref time, fILETIME, serverHandles.Length, serverHandles, aggregateIDs, numberOfIntervals, out pdwCancelID, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.AdviseProcessed", exception);
                }
                IdentifiedResult[] results = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    results[i] = new IdentifiedResult();
                }
                this.UpdateResults(items, results, ref zero);
                request2.Update(pdwCancelID, results);
                request = request2;
                return results;
            }
        }

        public IdentifiedResult[] AdviseRaw(Time startTime, decimal updateInterval, ItemIdentifier[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            request = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                Request request2 = this.m_callback.CreateRequest(requestHandle, callback);
                int requestID = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = this.GetServerHandles(items);
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_FILETIME fILETIME = OpcCom.Hda.Interop.GetFILETIME(updateInterval);
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncRead) base.m_server).AdviseRaw(request2.RequestID, ref time, fILETIME, serverHandles.Length, serverHandles, out pdwCancelID, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.AdviseRaw", exception);
                }
                IdentifiedResult[] results = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    results[i] = new IdentifiedResult();
                }
                this.UpdateResults(items, results, ref zero);
                request2.Update(pdwCancelID, results);
                request = request2;
                return results;
            }
        }

        public void CancelRequest(IRequest request)
        {
            this.CancelRequest(request, null);
        }

        public void CancelRequest(IRequest request, CancelCompleteEventHandler callback)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                Request request2 = (Request) request;
                this.m_callback.CancelRequest(request2, callback);
                try
                {
                    ((IOPCHDA_AsyncRead) base.m_server).Cancel(request2.CancelID);
                }
                catch (Exception exception)
                {
                    if (-2147467259 != Marshal.GetHRForException(exception))
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.Cancel", exception);
                    }
                }
            }
        }

        public IBrowser CreateBrowser(BrowseFilter[] filters, out ResultID[] results)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                int dwCount = (filters != null) ? filters.Length : 0;
                int[] pdwAttrID = new int[dwCount];
                object[] vFilter = new object[dwCount];
                OPCHDA_OPERATORCODES[] pOperator = new OPCHDA_OPERATORCODES[dwCount];
                for (int i = 0; i < dwCount; i++)
                {
                    pdwAttrID[i] = filters[i].AttributeID;
                    pOperator[i] = (OPCHDA_OPERATORCODES) Enum.ToObject(typeof(OPCHDA_OPERATORCODES), filters[i].Operator);
                    vFilter[i] = OpcCom.Interop.GetVARIANT(filters[i].FilterValue);
                }
                IOPCHDA_Browser pphBrowser = null;
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Server) base.m_server).CreateBrowse(dwCount, pdwAttrID, pOperator, vFilter, out pphBrowser, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Server.CreateBrowse", exception);
                }
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref zero, dwCount, true);
                if (((dwCount > 0) && (numArray2 == null)) || (pphBrowser == null))
                {
                    throw new InvalidResponseException();
                }
                results = new ResultID[dwCount];
                for (int j = 0; j < dwCount; j++)
                {
                    results[j] = OpcCom.Interop.GetResultID(numArray2[j]);
                }
                return new Browser(this, pphBrowser, filters, results);
            }
        }

        private int CreateHandle()
        {
            return NextHandle++;
        }

        public IdentifiedResult[] CreateItems(ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                string[] pszItemID = new string[items.Length];
                int[] phClient = new int[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null)
                    {
                        pszItemID[i] = items[i].ItemName;
                        phClient[i] = this.CreateHandle();
                    }
                }
                IntPtr zero = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Server) base.m_server).GetItemHandles(items.Length, pszItemID, phClient, out zero, out ppErrors);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Server.GetItemHandles", exception);
                }
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref zero, items.Length, true);
                int[] numArray3 = OpcCom.Interop.GetInt32s(ref ppErrors, items.Length, true);
                if ((numArray2 == null) || (numArray3 == null))
                {
                    throw new InvalidResponseException();
                }
                IdentifiedResult[] resultArray = new IdentifiedResult[items.Length];
                for (int j = 0; j < resultArray.Length; j++)
                {
                    resultArray[j] = new IdentifiedResult(items[j]);
                    resultArray[j].ResultID = OpcCom.Interop.GetResultID(numArray3[j]);
                    if (resultArray[j].ResultID.Succeeded())
                    {
                        ItemIdentifier identifier = new ItemIdentifier {
                            ItemName = items[j].ItemName,
                            ItemPath = items[j].ItemPath,
                            ServerHandle = numArray2[j],
                            ClientHandle = items[j].ClientHandle
                        };
                        this.m_items.Add(phClient[j], identifier);
                        resultArray[j].ServerHandle = phClient[j];
                        resultArray[j].ClientHandle = items[j].ClientHandle;
                    }
                }
                return resultArray;
            }
        }

        private ResultCollection[] CreateResultCollections(ItemIdentifier[] items)
        {
            ResultCollection[] resultsArray = null;
            if (items != null)
            {
                resultsArray = new ResultCollection[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    resultsArray[i] = new ResultCollection();
                    if (items[i] != null)
                    {
                        this.UpdateResult(items[i], resultsArray[i], 0);
                    }
                }
            }
            return resultsArray;
        }

        public IdentifiedResult[] Delete(Time startTime, Time endTime, ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                int[] serverHandles = this.GetServerHandles(items);
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = OpcCom.Hda.Interop.GetTime(endTime);
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncUpdate) base.m_server).DeleteRaw(ref time, ref htEndTime, serverHandles.Length, serverHandles, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncUpdate.DeleteRaw", exception);
                }
                IdentifiedResult[] results = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    results[i] = new IdentifiedResult();
                }
                this.UpdateResults(items, results, ref zero);
                return results;
            }
        }

        public IdentifiedResult[] Delete(Time startTime, Time endTime, ItemIdentifier[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            request = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                Request request2 = this.m_callback.CreateRequest(requestHandle, callback);
                int requestID = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = this.GetServerHandles(items);
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = OpcCom.Hda.Interop.GetTime(endTime);
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncUpdate) base.m_server).DeleteRaw(request2.RequestID, ref time, ref htEndTime, serverHandles.Length, serverHandles, out pdwCancelID, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncUpdate.DeleteRaw", exception);
                }
                IdentifiedResult[] results = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    results[i] = new IdentifiedResult();
                }
                this.UpdateResults(items, results, ref zero);
                if (request2.Update(pdwCancelID, results))
                {
                    request = null;
                    this.m_callback.CancelRequest(request2, null);
                    return results;
                }
                this.UpdateActualTimes(new IActualTime[] { request2 }, time, htEndTime);
                request = request2;
                return results;
            }
        }

        public ResultCollection[] DeleteAtTime(ItemTimeCollection[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new ResultCollection[0];
                }
                ResultCollection[] results = this.CreateResultCollections(items);
                int[] handles = null;
                DateTime[] timestamps = null;
                int count = this.MarshalTimestamps(items, ref handles, ref timestamps);
                if (count != 0)
                {
                    OPCHDA_FILETIME[] fILETIMEs = OpcCom.Hda.Interop.GetFILETIMEs(timestamps);
                    IntPtr zero = IntPtr.Zero;
                    try
                    {
                        ((IOPCHDA_SyncUpdate) base.m_server).DeleteAtTime(handles.Length, handles, fILETIMEs, out zero);
                    }
                    catch (Exception exception)
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_SyncUpdate.DeleteAtTime", exception);
                    }
                    this.UpdateResults(items, results, count, ref zero);
                }
                return results;
            }
        }

        public IdentifiedResult[] DeleteAtTime(ItemTimeCollection[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            request = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                ResultCollection[] results = this.CreateResultCollections(items);
                int[] handles = null;
                DateTime[] timestamps = null;
                int count = this.MarshalTimestamps(items, ref handles, ref timestamps);
                if (count != 0)
                {
                    OPCHDA_FILETIME[] fILETIMEs = OpcCom.Hda.Interop.GetFILETIMEs(timestamps);
                    Request request2 = this.m_callback.CreateRequest(requestHandle, callback);
                    IntPtr zero = IntPtr.Zero;
                    int pdwCancelID = 0;
                    try
                    {
                        ((IOPCHDA_AsyncUpdate) base.m_server).DeleteAtTime(request2.RequestID, handles.Length, handles, fILETIMEs, out pdwCancelID, out zero);
                    }
                    catch (Exception exception)
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_AsyncUpdate.DeleteAtTime", exception);
                    }
                    this.UpdateResults(items, results, count, ref zero);
                    if (request2.Update(pdwCancelID, results))
                    {
                        request = null;
                        this.m_callback.CancelRequest(request2, null);
                        return this.GetIdentifiedResults(results);
                    }
                    request = request2;
                }
                return this.GetIdentifiedResults(results);
            }
        }

        public override void Dispose()
        {
            lock (this)
            {
                this.Unadvise();
                base.Dispose();
            }
        }

        private int[] GetAggregateIDs(Item[] items)
        {
            int[] numArray = new int[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                numArray[i] = 0;
                if (items[i].AggregateID != 0)
                {
                    numArray[i] = items[i].AggregateID;
                }
            }
            return numArray;
        }

        public Aggregate[] GetAggregates()
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                int pdwCount = 0;
                IntPtr zero = IntPtr.Zero;
                IntPtr ppszAggrName = IntPtr.Zero;
                IntPtr ppszAggrDesc = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Server) base.m_server).GetAggregates(out pdwCount, out zero, out ppszAggrName, out ppszAggrDesc);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Server.GetAggregates", exception);
                }
                if (pdwCount == 0)
                {
                    return new Aggregate[0];
                }
                int[] numArray = OpcCom.Interop.GetInt32s(ref zero, pdwCount, true);
                string[] strArray = OpcCom.Interop.GetUnicodeStrings(ref ppszAggrName, pdwCount, true);
                string[] strArray2 = OpcCom.Interop.GetUnicodeStrings(ref ppszAggrDesc, pdwCount, true);
                if (((numArray == null) || (strArray == null)) || (strArray2 == null))
                {
                    throw new InvalidResponseException();
                }
                Aggregate[] aggregateArray = new Aggregate[pdwCount];
                for (int i = 0; i < pdwCount; i++)
                {
                    aggregateArray[i] = new Aggregate();
                    aggregateArray[i].ID = numArray[i];
                    aggregateArray[i].Name = strArray[i];
                    aggregateArray[i].Description = strArray2[i];
                }
                return aggregateArray;
            }
        }

        public Opc.Hda.Attribute[] GetAttributes()
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                int pdwCount = 0;
                IntPtr zero = IntPtr.Zero;
                IntPtr ppszAttrName = IntPtr.Zero;
                IntPtr ppszAttrDesc = IntPtr.Zero;
                IntPtr ppvtAttrDataType = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Server) base.m_server).GetItemAttributes(out pdwCount, out zero, out ppszAttrName, out ppszAttrDesc, out ppvtAttrDataType);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Server.GetItemAttributes", exception);
                }
                if (pdwCount == 0)
                {
                    return new Opc.Hda.Attribute[0];
                }
                int[] numArray = OpcCom.Interop.GetInt32s(ref zero, pdwCount, true);
                string[] strArray = OpcCom.Interop.GetUnicodeStrings(ref ppszAttrName, pdwCount, true);
                string[] strArray2 = OpcCom.Interop.GetUnicodeStrings(ref ppszAttrDesc, pdwCount, true);
                short[] numArray2 = OpcCom.Interop.GetInt16s(ref ppvtAttrDataType, pdwCount, true);
                if (((numArray == null) || (strArray == null)) || ((strArray2 == null) || (numArray2 == null)))
                {
                    throw new InvalidResponseException();
                }
                Opc.Hda.Attribute[] attributeArray = new Opc.Hda.Attribute[pdwCount];
                for (int i = 0; i < pdwCount; i++)
                {
                    attributeArray[i] = new Opc.Hda.Attribute();
                    attributeArray[i].ID = numArray[i];
                    attributeArray[i].Name = strArray[i];
                    attributeArray[i].Description = strArray2[i];
                    attributeArray[i].DataType = OpcCom.Interop.GetType((VarEnum) Enum.ToObject(typeof(VarEnum), numArray2[i]));
                }
                return attributeArray;
            }
        }

        private int GetCount(ICollection[] collections)
        {
            int num = 0;
            if (collections != null)
            {
                foreach (ICollection is2 in collections)
                {
                    if (is2 != null)
                    {
                        num += is2.Count;
                    }
                }
            }
            return num;
        }

        private IdentifiedResult[] GetIdentifiedResults(ResultCollection[] results)
        {
            if ((results == null) || (results.Length == 0))
            {
                return new IdentifiedResult[0];
            }
            IdentifiedResult[] resultArray = new IdentifiedResult[results.Length];
            for (int i = 0; i < results.Length; i++)
            {
                resultArray[i] = new IdentifiedResult(results[i]);
                if ((results[i] == null) || (results[i].Count == 0))
                {
                    resultArray[i].ResultID = ResultID.Hda.S_NODATA;
                }
                else
                {
                    ResultID resultID = results[i][0].ResultID;
                    foreach (Result result in results[i])
                    {
                        if (resultID.Code != result.ResultID.Code)
                        {
                            resultID = ResultID.E_FAIL;
                            break;
                        }
                    }
                }
            }
            return resultArray;
        }

        private int GetInvalidHandle()
        {
            int num = 0;
            foreach (ItemIdentifier identifier in this.m_items.Values)
            {
                int serverHandle = (int) identifier.ServerHandle;
                if (num < serverHandle)
                {
                    num = serverHandle;
                }
            }
            return (num + 1);
        }

        private int[] GetServerHandles(ItemIdentifier[] items)
        {
            int invalidHandle = this.GetInvalidHandle();
            int[] numArray = new int[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                numArray[i] = invalidHandle;
                if ((items[i] != null) && (items[i].ServerHandle != null))
                {
                    ItemIdentifier identifier = (ItemIdentifier) this.m_items[items[i].ServerHandle];
                    if (identifier != null)
                    {
                        numArray[i] = (int) identifier.ServerHandle;
                    }
                }
            }
            return numArray;
        }

        public ServerStatus GetStatus()
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                OPCHDA_SERVERSTATUS pwStatus = OPCHDA_SERVERSTATUS.OPCHDA_INDETERMINATE;
                IntPtr zero = IntPtr.Zero;
                IntPtr pftStartTime = IntPtr.Zero;
                short pwMajorVersion = 0;
                short pwMinorVersion = 0;
                short pwBuildNumber = 0;
                int pdwMaxReturnValues = 0;
                string ppszStatusString = null;
                string ppszVendorInfo = null;
                try
                {
                    ((IOPCHDA_Server) base.m_server).GetHistorianStatus(out pwStatus, out zero, out pftStartTime, out pwMajorVersion, out pwMinorVersion, out pwBuildNumber, out pdwMaxReturnValues, out ppszStatusString, out ppszVendorInfo);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Server.GetHistorianStatus", exception);
                }
                ServerStatus status = new ServerStatus {
                    VendorInfo = ppszVendorInfo,
                    ProductVersion = string.Format("{0}.{1}.{2}", pwMajorVersion, pwMinorVersion, pwBuildNumber),
                    ServerState = (ServerState) pwStatus,
                    StatusInfo = ppszStatusString,
                    StartTime = DateTime.MinValue,
                    CurrentTime = DateTime.MinValue,
                    MaxReturnValues = pdwMaxReturnValues
                };
                if (pftStartTime != IntPtr.Zero)
                {
                    status.StartTime = OpcCom.Interop.GetFILETIME(pftStartTime);
                    Marshal.FreeCoTaskMem(pftStartTime);
                }
                if (zero != IntPtr.Zero)
                {
                    status.CurrentTime = OpcCom.Interop.GetFILETIME(zero);
                    Marshal.FreeCoTaskMem(zero);
                }
                return status;
            }
        }

        public ResultCollection[] Insert(ItemValueCollection[] items, bool replace)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new ResultCollection[0];
                }
                ResultCollection[] results = this.CreateResultCollections(items);
                int[] handles = null;
                object[] values = null;
                int[] qualities = null;
                DateTime[] timestamps = null;
                int count = this.MarshalValues(items, ref handles, ref values, ref qualities, ref timestamps);
                if (count == 0)
                {
                    return results;
                }
                OPCHDA_FILETIME[] fILETIMEs = OpcCom.Hda.Interop.GetFILETIMEs(timestamps);
                IntPtr zero = IntPtr.Zero;
                if (replace)
                {
                    try
                    {
                        ((IOPCHDA_SyncUpdate) base.m_server).InsertReplace(handles.Length, handles, fILETIMEs, values, qualities, out zero);
                        goto Label_00CE;
                    }
                    catch (Exception exception)
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_SyncUpdate.InsertReplace", exception);
                    }
                }
                try
                {
                    ((IOPCHDA_SyncUpdate) base.m_server).Insert(handles.Length, handles, fILETIMEs, values, qualities, out zero);
                }
                catch (Exception exception2)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncUpdate.Insert", exception2);
                }
            Label_00CE:
                this.UpdateResults(items, results, count, ref zero);
                return results;
            }
        }

        public IdentifiedResult[] Insert(ItemValueCollection[] items, bool replace, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            request = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                ResultCollection[] results = this.CreateResultCollections(items);
                int[] handles = null;
                object[] values = null;
                int[] qualities = null;
                DateTime[] timestamps = null;
                int count = this.MarshalValues(items, ref handles, ref values, ref qualities, ref timestamps);
                if (count == 0)
                {
                    return this.GetIdentifiedResults(results);
                }
                OPCHDA_FILETIME[] fILETIMEs = OpcCom.Hda.Interop.GetFILETIMEs(timestamps);
                Request request2 = this.m_callback.CreateRequest(requestHandle, callback);
                IntPtr zero = IntPtr.Zero;
                int pdwCancelID = 0;
                if (replace)
                {
                    try
                    {
                        ((IOPCHDA_AsyncUpdate) base.m_server).InsertReplace(request2.RequestID, handles.Length, handles, fILETIMEs, values, qualities, out pdwCancelID, out zero);
                        goto Label_00FD;
                    }
                    catch (Exception exception)
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_AsyncUpdate.InsertReplace", exception);
                    }
                }
                try
                {
                    ((IOPCHDA_AsyncUpdate) base.m_server).Insert(request2.RequestID, handles.Length, handles, fILETIMEs, values, qualities, out pdwCancelID, out zero);
                }
                catch (Exception exception2)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncUpdate.Insert", exception2);
                }
            Label_00FD:
                this.UpdateResults(items, results, count, ref zero);
                if (request2.Update(pdwCancelID, results))
                {
                    request = null;
                    this.m_callback.CancelRequest(request2, null);
                    return this.GetIdentifiedResults(results);
                }
                request = request2;
                return this.GetIdentifiedResults(results);
            }
        }

        public ResultCollection[] InsertAnnotations(AnnotationValueCollection[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new ResultCollection[0];
                }
                ResultCollection[] results = this.CreateResultCollections(items);
                int[] serverHandles = null;
                OPCHDA_ANNOTATION[] annotations = null;
                OPCHDA_FILETIME[] ftTimestamps = null;
                int count = this.MarshalAnnotatations(items, ref serverHandles, ref ftTimestamps, ref annotations);
                if (count != 0)
                {
                    IntPtr zero = IntPtr.Zero;
                    try
                    {
                        ((IOPCHDA_SyncAnnotations) base.m_server).Insert(serverHandles.Length, serverHandles, ftTimestamps, annotations, out zero);
                    }
                    catch (Exception exception)
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_SyncAnnotations.Insert", exception);
                    }
                    for (int i = 0; i < annotations.Length; i++)
                    {
                        OpcCom.Interop.GetFILETIMEs(ref annotations[i].ftTimeStamps, 1, true);
                        OpcCom.Interop.GetUnicodeStrings(ref annotations[i].szAnnotation, 1, true);
                        OpcCom.Interop.GetFILETIMEs(ref annotations[i].ftAnnotationTime, 1, true);
                        OpcCom.Interop.GetUnicodeStrings(ref annotations[i].szUser, 1, true);
                    }
                    this.UpdateResults(items, results, count, ref zero);
                }
                return results;
            }
        }

        public IdentifiedResult[] InsertAnnotations(AnnotationValueCollection[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            request = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                ResultCollection[] results = this.CreateResultCollections(items);
                int[] serverHandles = null;
                OPCHDA_ANNOTATION[] annotations = null;
                OPCHDA_FILETIME[] ftTimestamps = null;
                int count = this.MarshalAnnotatations(items, ref serverHandles, ref ftTimestamps, ref annotations);
                if (count != 0)
                {
                    Request request2 = this.m_callback.CreateRequest(requestHandle, callback);
                    IntPtr zero = IntPtr.Zero;
                    int pdwCancelID = 0;
                    try
                    {
                        ((IOPCHDA_AsyncAnnotations) base.m_server).Insert(request2.RequestID, serverHandles.Length, serverHandles, ftTimestamps, annotations, out pdwCancelID, out zero);
                    }
                    catch (Exception exception)
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_AsyncAnnotations.Insert", exception);
                    }
                    for (int i = 0; i < annotations.Length; i++)
                    {
                        OpcCom.Interop.GetFILETIMEs(ref annotations[i].ftTimeStamps, 1, true);
                        OpcCom.Interop.GetUnicodeStrings(ref annotations[i].szAnnotation, 1, true);
                        OpcCom.Interop.GetFILETIMEs(ref annotations[i].ftAnnotationTime, 1, true);
                        OpcCom.Interop.GetUnicodeStrings(ref annotations[i].szUser, 1, true);
                    }
                    this.UpdateResults(items, results, count, ref zero);
                    if (request2.Update(pdwCancelID, results))
                    {
                        request = null;
                        this.m_callback.CancelRequest(request2, null);
                        return this.GetIdentifiedResults(results);
                    }
                    request = request2;
                }
                return this.GetIdentifiedResults(results);
            }
        }

        private int MarshalAnnotatations(AnnotationValueCollection[] items, ref int[] serverHandles, ref OPCHDA_FILETIME[] ftTimestamps, ref OPCHDA_ANNOTATION[] annotations)
        {
            int count = this.GetCount(items);
            int[] numArray = this.GetServerHandles(items);
            serverHandles = new int[count];
            annotations = new OPCHDA_ANNOTATION[count];
            DateTime[] input = new DateTime[count];
            int index = 0;
            for (int i = 0; i < items.Length; i++)
            {
                for (int j = 0; j < items[i].Count; j++)
                {
                    serverHandles[index] = numArray[i];
                    input[index] = items[i][j].Timestamp;
                    annotations[index] = new OPCHDA_ANNOTATION();
                    annotations[index].dwNumValues = 1;
                    DateTime[] datetimes = new DateTime[] { input[j] };
                    annotations[index].ftTimeStamps = OpcCom.Interop.GetFILETIMEs(datetimes);
                    annotations[index].szAnnotation = OpcCom.Interop.GetUnicodeStrings(new string[] { items[i][j].Annotation });
                    datetimes = new DateTime[] { items[i][j].CreationTime };
                    annotations[index].ftAnnotationTime = OpcCom.Interop.GetFILETIMEs(datetimes);
                    annotations[index].szUser = OpcCom.Interop.GetUnicodeStrings(new string[] { items[i][j].User });
                    index++;
                }
            }
            ftTimestamps = OpcCom.Hda.Interop.GetFILETIMEs(input);
            return count;
        }

        private int MarshalTimestamps(ItemTimeCollection[] items, ref int[] handles, ref DateTime[] timestamps)
        {
            int count = this.GetCount(items);
            handles = new int[count];
            timestamps = new DateTime[count];
            int[] serverHandles = this.GetServerHandles(items);
            int index = 0;
            for (int i = 0; i < items.Length; i++)
            {
                foreach (DateTime time in items[i])
                {
                    handles[index] = serverHandles[i];
                    timestamps[index] = time;
                    index++;
                }
            }
            return count;
        }

        private int MarshalValues(ItemValueCollection[] items, ref int[] handles, ref object[] values, ref int[] qualities, ref DateTime[] timestamps)
        {
            int count = this.GetCount(items);
            handles = new int[count];
            timestamps = new DateTime[count];
            values = new object[count];
            qualities = new int[count];
            int[] serverHandles = this.GetServerHandles(items);
            int index = 0;
            for (int i = 0; i < items.Length; i++)
            {
                foreach (ItemValue value2 in items[i])
                {
                    handles[index] = serverHandles[i];
                    timestamps[index] = value2.Timestamp;
                    values[index] = OpcCom.Interop.GetVARIANT(value2.Value);
                    qualities[index] = value2.Quality.GetCode();
                    index++;
                }
            }
            return count;
        }

        public IdentifiedResult[] PlaybackProcessed(Time startTime, Time endTime, decimal resampleInterval, int numberOfIntervals, decimal updateInterval, Item[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            request = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                Request request2 = this.m_callback.CreateRequest(requestHandle, callback);
                int requestID = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = this.GetServerHandles(items);
                int[] aggregateIDs = this.GetAggregateIDs(items);
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = OpcCom.Hda.Interop.GetTime(endTime);
                OPCHDA_FILETIME fILETIME = OpcCom.Hda.Interop.GetFILETIME(resampleInterval);
                OPCHDA_FILETIME ftUpdateInterval = OpcCom.Hda.Interop.GetFILETIME(updateInterval);
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Playback) base.m_server).ReadProcessedWithUpdate(request2.RequestID, ref time, ref htEndTime, fILETIME, numberOfIntervals, ftUpdateInterval, serverHandles.Length, serverHandles, aggregateIDs, out pdwCancelID, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Playback.ReadProcessedWithUpdate", exception);
                }
                IdentifiedResult[] results = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    results[i] = new IdentifiedResult();
                }
                this.UpdateResults(items, results, ref zero);
                request2.Update(pdwCancelID, results);
                request = request2;
                return results;
            }
        }

        public IdentifiedResult[] PlaybackRaw(Time startTime, Time endTime, int maxValues, decimal updateInterval, decimal playbackDuration, ItemIdentifier[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            request = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                Request request2 = this.m_callback.CreateRequest(requestHandle, callback);
                int requestID = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = this.GetServerHandles(items);
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = OpcCom.Hda.Interop.GetTime(endTime);
                OPCHDA_FILETIME fILETIME = OpcCom.Hda.Interop.GetFILETIME(updateInterval);
                OPCHDA_FILETIME ftUpdateDuration = OpcCom.Hda.Interop.GetFILETIME(playbackDuration);
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Playback) base.m_server).ReadRawWithUpdate(request2.RequestID, ref time, ref htEndTime, maxValues, ftUpdateDuration, fILETIME, serverHandles.Length, serverHandles, out pdwCancelID, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Playback.ReadRawWithUpdate", exception);
                }
                IdentifiedResult[] results = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    results[i] = new IdentifiedResult();
                }
                this.UpdateResults(items, results, ref zero);
                request2.Update(pdwCancelID, results);
                request = request2;
                return results;
            }
        }

        public AnnotationValueCollection[] ReadAnnotations(Time startTime, Time endTime, ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new AnnotationValueCollection[0];
                }
                int[] serverHandles = this.GetServerHandles(items);
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = OpcCom.Hda.Interop.GetTime(endTime);
                IntPtr zero = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncAnnotations) base.m_server).Read(ref time, ref htEndTime, serverHandles.Length, serverHandles, out zero, out ppErrors);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncAnnotations.Read", exception);
                }
                AnnotationValueCollection[] results = OpcCom.Hda.Interop.GetAnnotationValueCollections(ref zero, items.Length, true);
                this.UpdateResults(items, results, ref ppErrors);
                this.UpdateActualTimes(results, time, htEndTime);
                return results;
            }
        }

        public IdentifiedResult[] ReadAnnotations(Time startTime, Time endTime, ItemIdentifier[] items, object requestHandle, ReadAnnotationsEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            request = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                Request request2 = this.m_callback.CreateRequest(requestHandle, callback);
                int requestID = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = this.GetServerHandles(items);
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = OpcCom.Hda.Interop.GetTime(endTime);
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncAnnotations) base.m_server).Read(request2.RequestID, ref time, ref htEndTime, serverHandles.Length, serverHandles, out pdwCancelID, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncAnnotations.Read", exception);
                }
                IdentifiedResult[] results = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    results[i] = new IdentifiedResult();
                }
                this.UpdateResults(items, results, ref zero);
                if (request2.Update(pdwCancelID, results))
                {
                    request = null;
                    this.m_callback.CancelRequest(request2, null);
                    return results;
                }
                this.UpdateActualTimes(new IActualTime[] { request2 }, time, htEndTime);
                request = request2;
                return results;
            }
        }

        public ItemValueCollection[] ReadAtTime(DateTime[] timestamps, ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new ItemValueCollection[0];
                }
                int[] serverHandles = this.GetServerHandles(items);
                OPCHDA_FILETIME[] fILETIMEs = OpcCom.Hda.Interop.GetFILETIMEs(timestamps);
                IntPtr zero = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncRead) base.m_server).ReadAtTime(fILETIMEs.Length, fILETIMEs, serverHandles.Length, serverHandles, out zero, out ppErrors);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncRead.ReadAtTime", exception);
                }
                ItemValueCollection[] results = OpcCom.Hda.Interop.GetItemValueCollections(ref zero, items.Length, true);
                this.UpdateResults(items, results, ref ppErrors);
                return results;
            }
        }

        public IdentifiedResult[] ReadAtTime(DateTime[] timestamps, ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            request = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                Request request2 = this.m_callback.CreateRequest(requestHandle, callback);
                int requestID = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = this.GetServerHandles(items);
                OPCHDA_FILETIME[] fILETIMEs = OpcCom.Hda.Interop.GetFILETIMEs(timestamps);
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncRead) base.m_server).ReadAtTime(request2.RequestID, fILETIMEs.Length, fILETIMEs, serverHandles.Length, serverHandles, out pdwCancelID, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.ReadAtTime", exception);
                }
                IdentifiedResult[] results = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    results[i] = new IdentifiedResult();
                }
                this.UpdateResults(items, results, ref zero);
                if (request2.Update(pdwCancelID, results))
                {
                    request = null;
                    this.m_callback.CancelRequest(request2, null);
                    return results;
                }
                request = request2;
                return results;
            }
        }

        public ItemAttributeCollection ReadAttributes(Time startTime, Time endTime, ItemIdentifier item, int[] attributeIDs)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (attributeIDs == null)
            {
                throw new ArgumentNullException("attributeIDs");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (attributeIDs.Length == 0)
                {
                    return new ItemAttributeCollection(item);
                }
                int[] serverHandles = this.GetServerHandles(new ItemIdentifier[] { item });
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = OpcCom.Hda.Interop.GetTime(endTime);
                IntPtr zero = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncRead) base.m_server).ReadAttribute(ref time, ref htEndTime, serverHandles[0], attributeIDs.Length, attributeIDs, out zero, out ppErrors);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncRead.ReadAttribute", exception);
                }
                AttributeValueCollection[] valuesArray = OpcCom.Hda.Interop.GetAttributeValueCollections(ref zero, attributeIDs.Length, true);
                ItemAttributeCollection attributes = this.UpdateResults(item, valuesArray, ref ppErrors);
                this.UpdateActualTimes(new IActualTime[] { attributes }, time, htEndTime);
                return attributes;
            }
        }

        public ResultCollection ReadAttributes(Time startTime, Time endTime, ItemIdentifier item, int[] attributeIDs, object requestHandle, ReadAttributesEventHandler callback, out IRequest request)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (attributeIDs == null)
            {
                throw new ArgumentNullException("attributeIDs");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            request = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (attributeIDs.Length == 0)
                {
                    return new ResultCollection();
                }
                Request request2 = this.m_callback.CreateRequest(requestHandle, callback);
                int requestID = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = this.GetServerHandles(new ItemIdentifier[] { item });
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = OpcCom.Hda.Interop.GetTime(endTime);
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncRead) base.m_server).ReadAttribute(request2.RequestID, ref time, ref htEndTime, serverHandles[0], attributeIDs.Length, attributeIDs, out pdwCancelID, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.ReadAttribute", exception);
                }
                ResultCollection results = new ResultCollection(item);
                this.UpdateResult(item, results, 0);
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref zero, attributeIDs.Length, true);
                if (numArray2 == null)
                {
                    throw new InvalidResponseException();
                }
                foreach (int num2 in numArray2)
                {
                    Result result = new Result(OpcCom.Interop.GetResultID(num2));
                    results.Add(result);
                }
                if (request2.Update(pdwCancelID, new ResultCollection[] { results }))
                {
                    request = null;
                    this.m_callback.CancelRequest(request2, null);
                    return results;
                }
                this.UpdateActualTimes(new IActualTime[] { request2 }, time, htEndTime);
                request = request2;
                return results;
            }
        }

        public ModifiedValueCollection[] ReadModified(Time startTime, Time endTime, int maxValues, ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new ModifiedValueCollection[0];
                }
                int[] serverHandles = this.GetServerHandles(items);
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = OpcCom.Hda.Interop.GetTime(endTime);
                IntPtr zero = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncRead) base.m_server).ReadModified(ref time, ref htEndTime, maxValues, serverHandles.Length, serverHandles, out zero, out ppErrors);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncRead.ReadModified", exception);
                }
                ModifiedValueCollection[] results = OpcCom.Hda.Interop.GetModifiedValueCollections(ref zero, items.Length, true);
                this.UpdateResults(items, results, ref ppErrors);
                this.UpdateActualTimes(results, time, htEndTime);
                return results;
            }
        }

        public IdentifiedResult[] ReadModified(Time startTime, Time endTime, int maxValues, ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            request = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                Request request2 = this.m_callback.CreateRequest(requestHandle, callback);
                int requestID = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = this.GetServerHandles(items);
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = OpcCom.Hda.Interop.GetTime(endTime);
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncRead) base.m_server).ReadModified(request2.RequestID, ref time, ref htEndTime, maxValues, serverHandles.Length, serverHandles, out pdwCancelID, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.ReadModified", exception);
                }
                IdentifiedResult[] results = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    results[i] = new IdentifiedResult();
                }
                this.UpdateResults(items, results, ref zero);
                if (request2.Update(pdwCancelID, results))
                {
                    request = null;
                    this.m_callback.CancelRequest(request2, null);
                    return results;
                }
                this.UpdateActualTimes(new IActualTime[] { request2 }, time, htEndTime);
                request = request2;
                return results;
            }
        }

        public ItemValueCollection[] ReadProcessed(Time startTime, Time endTime, decimal resampleInterval, Item[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new ItemValueCollection[0];
                }
                int[] serverHandles = this.GetServerHandles(items);
                int[] aggregateIDs = this.GetAggregateIDs(items);
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = OpcCom.Hda.Interop.GetTime(endTime);
                OPCHDA_FILETIME fILETIME = OpcCom.Hda.Interop.GetFILETIME(resampleInterval);
                IntPtr zero = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncRead) base.m_server).ReadProcessed(ref time, ref htEndTime, fILETIME, serverHandles.Length, serverHandles, aggregateIDs, out zero, out ppErrors);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncRead.ReadProcessed", exception);
                }
                ItemValueCollection[] results = OpcCom.Hda.Interop.GetItemValueCollections(ref zero, items.Length, true);
                this.UpdateResults(items, results, ref ppErrors);
                this.UpdateActualTimes(results, time, htEndTime);
                return results;
            }
        }

        public IdentifiedResult[] ReadProcessed(Time startTime, Time endTime, decimal resampleInterval, Item[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            request = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                Request request2 = this.m_callback.CreateRequest(requestHandle, callback);
                int requestID = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = this.GetServerHandles(items);
                int[] aggregateIDs = this.GetAggregateIDs(items);
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = OpcCom.Hda.Interop.GetTime(endTime);
                OPCHDA_FILETIME fILETIME = OpcCom.Hda.Interop.GetFILETIME(resampleInterval);
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncRead) base.m_server).ReadProcessed(request2.RequestID, ref time, ref htEndTime, fILETIME, serverHandles.Length, serverHandles, aggregateIDs, out pdwCancelID, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.ReadProcessed", exception);
                }
                IdentifiedResult[] results = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    results[i] = new IdentifiedResult();
                }
                this.UpdateResults(items, results, ref zero);
                if (request2.Update(pdwCancelID, results))
                {
                    request = null;
                    this.m_callback.CancelRequest(request2, null);
                    return results;
                }
                this.UpdateActualTimes(new IActualTime[] { request2 }, time, htEndTime);
                request = request2;
                return results;
            }
        }

        public ItemValueCollection[] ReadRaw(Time startTime, Time endTime, int maxValues, bool includeBounds, ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new ItemValueCollection[0];
                }
                int[] serverHandles = this.GetServerHandles(items);
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = OpcCom.Hda.Interop.GetTime(endTime);
                IntPtr zero = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncRead) base.m_server).ReadRaw(ref time, ref htEndTime, maxValues, includeBounds ? 1 : 0, serverHandles.Length, serverHandles, out zero, out ppErrors);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncRead.ReadRaw", exception);
                }
                ItemValueCollection[] results = OpcCom.Hda.Interop.GetItemValueCollections(ref zero, items.Length, true);
                this.UpdateResults(items, results, ref ppErrors);
                this.UpdateActualTimes(results, time, htEndTime);
                return results;
            }
        }

        public IdentifiedResult[] ReadRaw(Time startTime, Time endTime, int maxValues, bool includeBounds, ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            request = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                Request request2 = this.m_callback.CreateRequest(requestHandle, callback);
                int requestID = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = this.GetServerHandles(items);
                OPCHDA_TIME time = OpcCom.Hda.Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = OpcCom.Hda.Interop.GetTime(endTime);
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncRead) base.m_server).ReadRaw(request2.RequestID, ref time, ref htEndTime, maxValues, includeBounds ? 1 : 0, serverHandles.Length, serverHandles, out pdwCancelID, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.ReadRaw", exception);
                }
                IdentifiedResult[] results = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    results[i] = new IdentifiedResult();
                }
                this.UpdateResults(items, results, ref zero);
                if (request2.Update(pdwCancelID, results))
                {
                    request = null;
                    this.m_callback.CancelRequest(request2, null);
                    return results;
                }
                this.UpdateActualTimes(new IActualTime[] { request2 }, time, htEndTime);
                request = request2;
                return results;
            }
        }

        public IdentifiedResult[] ReleaseItems(ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                int[] serverHandles = this.GetServerHandles(items);
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Server) base.m_server).ReleaseItemHandles(items.Length, serverHandles, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Server.ReleaseItemHandles", exception);
                }
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref zero, items.Length, true);
                if (numArray2 == null)
                {
                    throw new InvalidResponseException();
                }
                IdentifiedResult[] resultArray = new IdentifiedResult[items.Length];
                for (int i = 0; i < resultArray.Length; i++)
                {
                    resultArray[i] = new IdentifiedResult(items[i]);
                    resultArray[i].ResultID = OpcCom.Interop.GetResultID(numArray2[i]);
                    if (resultArray[i].ResultID.Succeeded() && (items[i].ServerHandle != null))
                    {
                        ItemIdentifier identifier = (ItemIdentifier) this.m_items[items[i].ServerHandle];
                        if (identifier != null)
                        {
                            resultArray[i].ItemName = identifier.ItemName;
                            resultArray[i].ItemPath = identifier.ItemPath;
                            resultArray[i].ClientHandle = identifier.ClientHandle;
                            this.m_items.Remove(items[i].ServerHandle);
                        }
                    }
                }
                return resultArray;
            }
        }

        public ResultCollection[] Replace(ItemValueCollection[] items)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new ResultCollection[0];
                }
                ResultCollection[] results = this.CreateResultCollections(items);
                int[] handles = null;
                object[] values = null;
                int[] qualities = null;
                DateTime[] timestamps = null;
                int count = this.MarshalValues(items, ref handles, ref values, ref qualities, ref timestamps);
                if (count != 0)
                {
                    OPCHDA_FILETIME[] fILETIMEs = OpcCom.Hda.Interop.GetFILETIMEs(timestamps);
                    IntPtr zero = IntPtr.Zero;
                    try
                    {
                        ((IOPCHDA_SyncUpdate) base.m_server).Replace(handles.Length, handles, fILETIMEs, values, qualities, out zero);
                    }
                    catch (Exception exception)
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_SyncUpdate.Replace", exception);
                    }
                    this.UpdateResults(items, results, count, ref zero);
                }
                return results;
            }
        }

        public IdentifiedResult[] Replace(ItemValueCollection[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            request = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                ResultCollection[] results = this.CreateResultCollections(items);
                int[] handles = null;
                object[] values = null;
                int[] qualities = null;
                DateTime[] timestamps = null;
                int count = this.MarshalValues(items, ref handles, ref values, ref qualities, ref timestamps);
                if (count != 0)
                {
                    OPCHDA_FILETIME[] fILETIMEs = OpcCom.Hda.Interop.GetFILETIMEs(timestamps);
                    Request request2 = this.m_callback.CreateRequest(requestHandle, callback);
                    IntPtr zero = IntPtr.Zero;
                    int pdwCancelID = 0;
                    try
                    {
                        ((IOPCHDA_AsyncUpdate) base.m_server).Replace(request2.RequestID, handles.Length, handles, fILETIMEs, values, qualities, out pdwCancelID, out zero);
                    }
                    catch (Exception exception)
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_AsyncUpdate.Replace", exception);
                    }
                    this.UpdateResults(items, results, count, ref zero);
                    if (request2.Update(pdwCancelID, results))
                    {
                        request = null;
                        this.m_callback.CancelRequest(request2, null);
                        return this.GetIdentifiedResults(results);
                    }
                    request = request2;
                }
                return this.GetIdentifiedResults(results);
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

        private void UpdateActualTimes(IActualTime[] results, OPCHDA_TIME pStartTime, OPCHDA_TIME pEndTime)
        {
            DateTime fILETIME = OpcCom.Interop.GetFILETIME(OpcCom.Hda.Interop.Convert(pStartTime.ftTime));
            DateTime time2 = OpcCom.Interop.GetFILETIME(OpcCom.Hda.Interop.Convert(pEndTime.ftTime));
            foreach (IActualTime time3 in results)
            {
                time3.StartTime = fILETIME;
                time3.EndTime = time2;
            }
        }

        private void UpdateResult(ItemIdentifier item, ItemIdentifier result, int error)
        {
            result.ItemName = item.ItemName;
            result.ItemPath = item.ItemPath;
            result.ClientHandle = item.ClientHandle;
            result.ServerHandle = item.ServerHandle;
            if ((error >= 0) && (item.ServerHandle != null))
            {
                ItemIdentifier identifier = (ItemIdentifier) this.m_items[item.ServerHandle];
                if (identifier != null)
                {
                    result.ItemName = identifier.ItemName;
                    result.ItemPath = identifier.ItemPath;
                    result.ClientHandle = identifier.ClientHandle;
                }
            }
        }

        private ItemAttributeCollection UpdateResults(ItemIdentifier item, AttributeValueCollection[] attributes, ref IntPtr pErrors)
        {
            int[] numArray = OpcCom.Interop.GetInt32s(ref pErrors, attributes.Length, true);
            if ((attributes == null) || (numArray == null))
            {
                throw new InvalidResponseException();
            }
            for (int i = 0; i < attributes.Length; i++)
            {
                attributes[i].ResultID = OpcCom.Interop.GetResultID(numArray[i]);
            }
            ItemAttributeCollection result = new ItemAttributeCollection();
            foreach (AttributeValueCollection values in attributes)
            {
                result.Add(values);
            }
            this.UpdateResult(item, result, 0);
            return result;
        }

        private void UpdateResults(ItemIdentifier[] items, ItemIdentifier[] results, ref IntPtr pErrors)
        {
            int[] numArray = OpcCom.Interop.GetInt32s(ref pErrors, items.Length, true);
            if ((results == null) || (numArray == null))
            {
                throw new InvalidResponseException();
            }
            for (int i = 0; i < results.Length; i++)
            {
                this.UpdateResult(items[i], results[i], numArray[i]);
                if (typeof(IResult).IsInstanceOfType(results[i]))
                {
                    ((IResult) results[i]).ResultID = OpcCom.Interop.GetResultID(numArray[i]);
                }
            }
        }

        private void UpdateResults(ICollection[] items, ResultCollection[] results, int count, ref IntPtr pErrors)
        {
            int[] numArray = OpcCom.Interop.GetInt32s(ref pErrors, count, true);
            if (numArray == null)
            {
                throw new InvalidResponseException();
            }
            int num = 0;
            for (int i = 0; i < items.Length; i++)
            {
                for (int j = 0; j < items[i].Count; j++)
                {
                    if (num >= count)
                    {
                        break;
                    }
                    Result result = new Result(OpcCom.Interop.GetResultID(numArray[num++]));
                    results[i].Add(result);
                }
            }
        }

        public IdentifiedResult[] ValidateItems(ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }
                string[] pszItemID = new string[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null)
                    {
                        pszItemID[i] = items[i].ItemName;
                    }
                }
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Server) base.m_server).ValidateItemIDs(items.Length, pszItemID, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Server.ValidateItemIDs", exception);
                }
                int[] numArray = OpcCom.Interop.GetInt32s(ref zero, items.Length, true);
                if (numArray == null)
                {
                    throw new InvalidResponseException();
                }
                IdentifiedResult[] resultArray = new IdentifiedResult[items.Length];
                for (int j = 0; j < resultArray.Length; j++)
                {
                    resultArray[j] = new IdentifiedResult(items[j]);
                    resultArray[j].ResultID = OpcCom.Interop.GetResultID(numArray[j]);
                }
                return resultArray;
            }
        }
    }
}

