namespace Jund.OpcHelper.OpcCom.Ae
{
    using Opc;
    using Opc.Ae;
    using OpcCom;
    using OpcRcw.Ae;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    [Serializable]
    public class Server : OpcCom.Server, Opc.Ae.IServer, Opc.IServer, IDisposable
    {
        private object m_browser;
        private int m_handles;
        private Hashtable m_subscriptions;
        private bool m_supportsAE11;

        internal Server(URL url, object server) : base(url, server)
        {
            this.m_supportsAE11 = true;
            this.m_browser = null;
            this.m_handles = 1;
            this.m_subscriptions = new Hashtable();
            this.m_supportsAE11 = true;
            try
            {
                IOPCEventServer2 server1 = (IOPCEventServer2) server;
            }
            catch
            {
                this.m_supportsAE11 = false;
            }
        }

        public ResultID[] AcknowledgeCondition(string acknowledgerID, string comment, EventAcknowledgement[] conditions)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if ((conditions == null) || (conditions.Length == 0))
                {
                    return new ResultID[0];
                }
                int length = conditions.Length;
                string[] pszSource = new string[length];
                string[] pszConditionName = new string[length];
                OpcRcw.Ae.FILETIME[] pftActiveTime = new OpcRcw.Ae.FILETIME[length];
                int[] pdwCookie = new int[length];
                for (int i = 0; i < length; i++)
                {
                    pszSource[i] = conditions[i].SourceName;
                    pszConditionName[i] = conditions[i].ConditionName;
                    pftActiveTime[i] = OpcCom.Ae.Interop.Convert(OpcCom.Interop.GetFILETIME(conditions[i].ActiveTime));
                    pdwCookie[i] = conditions[i].Cookie;
                }
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer) base.m_server).AckCondition(conditions.Length, acknowledgerID, comment, pszSource, pszConditionName, pftActiveTime, pdwCookie, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.AckCondition", exception);
                }
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref zero, length, true);
                ResultID[] tidArray = new ResultID[length];
                for (int j = 0; j < length; j++)
                {
                    tidArray[j] = OpcCom.Ae.Interop.GetResultID(numArray2[j]);
                }
                return tidArray;
            }
        }

        public BrowseElement[] Browse(string areaID, BrowseType browseType, string browseFilter)
        {
            lock (this)
            {
                IBrowsePosition position = null;
                BrowseElement[] elementArray = this.Browse(areaID, browseType, browseFilter, 0, out position);
                if (position != null)
                {
                    position.Dispose();
                }
                return elementArray;
            }
        }

        public BrowseElement[] Browse(string areaID, BrowseType browseType, string browseFilter, int maxElements, out IBrowsePosition position)
        {
            position = null;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                this.InitializeBrowser();
                this.ChangeBrowsePosition(areaID);
                UCOMIEnumString enumerator = (UCOMIEnumString) this.CreateEnumerator(browseType, browseFilter);
                ArrayList elements = new ArrayList();
                if (this.FetchElements(browseType, maxElements, enumerator, elements) != 0)
                {
                    OpcCom.Interop.ReleaseServer(enumerator);
                }
                else
                {
                    position = new OpcCom.Ae.BrowsePosition(areaID, browseType, browseFilter, enumerator);
                }
                return (BrowseElement[]) elements.ToArray(typeof(BrowseElement));
            }
        }

        public BrowseElement[] BrowseNext(int maxElements, ref IBrowsePosition position)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (position == null)
                {
                    throw new ArgumentNullException("position");
                }
                this.InitializeBrowser();
                this.ChangeBrowsePosition(((OpcCom.Ae.BrowsePosition) position).AreaID);
                UCOMIEnumString enumerator = ((OpcCom.Ae.BrowsePosition) position).Enumerator;
                ArrayList elements = new ArrayList();
                if (this.FetchElements(((OpcCom.Ae.BrowsePosition) position).BrowseType, maxElements, enumerator, elements) != 0)
                {
                    position.Dispose();
                    position = null;
                }
                return (BrowseElement[]) elements.ToArray(typeof(BrowseElement));
            }
        }

        private void ChangeBrowsePosition(string areaID)
        {
            string szString = (areaID != null) ? areaID : "";
            try
            {
                ((IOPCEventAreaBrowser) this.m_browser).ChangeBrowsePosition(OPCAEBROWSEDIRECTION.OPCAE_BROWSE_TO, szString);
            }
            catch (Exception exception)
            {
                throw OpcCom.Interop.CreateException("IOPCEventAreaBrowser.ChangeBrowsePosition", exception);
            }
        }

        private object CreateEnumerator(BrowseType browseType, string browseFilter)
        {
            OPCAEBROWSETYPE dwBrowseFilterType = OpcCom.Ae.Interop.GetBrowseType(browseType);
            IEnumString ppIEnumString = null;
            try
            {
                ((IOPCEventAreaBrowser) this.m_browser).BrowseOPCAreas(dwBrowseFilterType, (browseFilter != null) ? browseFilter : "", out ppIEnumString);
            }
            catch (Exception exception)
            {
                throw OpcCom.Interop.CreateException("IOPCEventAreaBrowser.BrowseOPCAreas", exception);
            }
            if (ppIEnumString == null)
            {
                throw new InvalidResponseException("enumerator == null");
            }
            return ppIEnumString;
        }

        public ISubscription CreateSubscription(SubscriptionState state)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (state == null)
                {
                    throw new ArgumentNullException("state");
                }
                object ppUnk = null;
                Guid gUID = typeof(IOPCEventSubscriptionMgt).GUID;
                int pdwRevisedBufferTime = 0;
                int pdwRevisedMaxSize = 0;
                try
                {
                    ((IOPCEventServer) base.m_server).CreateEventSubscription(state.Active ? 1 : 0, state.BufferTime, state.MaxSize, ++this.m_handles, ref gUID, out ppUnk, out pdwRevisedBufferTime, out pdwRevisedMaxSize);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.CreateEventSubscription", exception);
                }
                state.BufferTime = pdwRevisedBufferTime;
                state.MaxSize = pdwRevisedMaxSize;
                OpcCom.Ae.Subscription subscription = new OpcCom.Ae.Subscription(state, ppUnk);
                subscription.ModifyState(0x20, state);
                this.m_subscriptions.Add(this.m_handles, subscription);
                return subscription;
            }
        }

        public ResultID[] DisableConditionByArea(string[] areas)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if ((areas == null) || (areas.Length == 0))
                {
                    return new ResultID[0];
                }
                IntPtr zero = IntPtr.Zero;
                int[] numArray = null;
                if (this.m_supportsAE11)
                {
                    try
                    {
                        ((IOPCEventServer2) base.m_server).DisableConditionByArea2(areas.Length, areas, out zero);
                    }
                    catch (Exception exception)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer2.DisableConditionByArea2", exception);
                    }
                    numArray = OpcCom.Interop.GetInt32s(ref zero, areas.Length, true);
                }
                else
                {
                    try
                    {
                        ((IOPCEventServer) base.m_server).DisableConditionByArea(areas.Length, areas);
                    }
                    catch (Exception exception2)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer.DisableConditionByArea", exception2);
                    }
                    numArray = new int[areas.Length];
                }
                ResultID[] tidArray = new ResultID[numArray.Length];
                for (int i = 0; i < numArray.Length; i++)
                {
                    tidArray[i] = OpcCom.Ae.Interop.GetResultID(numArray[i]);
                }
                return tidArray;
            }
        }

        public ResultID[] DisableConditionBySource(string[] sources)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if ((sources == null) || (sources.Length == 0))
                {
                    return new ResultID[0];
                }
                IntPtr zero = IntPtr.Zero;
                int[] numArray = null;
                if (this.m_supportsAE11)
                {
                    try
                    {
                        ((IOPCEventServer2) base.m_server).DisableConditionBySource2(sources.Length, sources, out zero);
                    }
                    catch (Exception exception)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer2.DisableConditionBySource2", exception);
                    }
                    numArray = OpcCom.Interop.GetInt32s(ref zero, sources.Length, true);
                }
                else
                {
                    try
                    {
                        ((IOPCEventServer) base.m_server).DisableConditionBySource(sources.Length, sources);
                    }
                    catch (Exception exception2)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer.DisableConditionBySource", exception2);
                    }
                    numArray = new int[sources.Length];
                }
                ResultID[] tidArray = new ResultID[numArray.Length];
                for (int i = 0; i < numArray.Length; i++)
                {
                    tidArray[i] = OpcCom.Ae.Interop.GetResultID(numArray[i]);
                }
                return tidArray;
            }
        }

        public override void Dispose()
        {
            lock (this)
            {
                if (this.m_browser != null)
                {
                    OpcCom.Interop.ReleaseServer(this.m_browser);
                    this.m_browser = null;
                }
                if (base.m_server != null)
                {
                    foreach (OpcCom.Ae.Subscription subscription in this.m_subscriptions.Values)
                    {
                        subscription.Dispose();
                    }
                    this.m_subscriptions.Clear();
                    OpcCom.Interop.ReleaseServer(base.m_server);
                    base.m_server = null;
                }
            }
        }

        public ResultID[] EnableConditionByArea(string[] areas)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if ((areas == null) || (areas.Length == 0))
                {
                    return new ResultID[0];
                }
                IntPtr zero = IntPtr.Zero;
                int[] numArray = null;
                if (this.m_supportsAE11)
                {
                    try
                    {
                        ((IOPCEventServer2) base.m_server).EnableConditionByArea2(areas.Length, areas, out zero);
                    }
                    catch (Exception exception)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer2.EnableConditionByArea2", exception);
                    }
                    numArray = OpcCom.Interop.GetInt32s(ref zero, areas.Length, true);
                }
                else
                {
                    try
                    {
                        ((IOPCEventServer) base.m_server).EnableConditionByArea(areas.Length, areas);
                    }
                    catch (Exception exception2)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer.EnableConditionByArea", exception2);
                    }
                    numArray = new int[areas.Length];
                }
                ResultID[] tidArray = new ResultID[numArray.Length];
                for (int i = 0; i < numArray.Length; i++)
                {
                    tidArray[i] = OpcCom.Ae.Interop.GetResultID(numArray[i]);
                }
                return tidArray;
            }
        }

        public ResultID[] EnableConditionBySource(string[] sources)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if ((sources == null) || (sources.Length == 0))
                {
                    return new ResultID[0];
                }
                IntPtr zero = IntPtr.Zero;
                int[] numArray = null;
                if (this.m_supportsAE11)
                {
                    try
                    {
                        ((IOPCEventServer2) base.m_server).EnableConditionBySource2(sources.Length, sources, out zero);
                    }
                    catch (Exception exception)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer2.EnableConditionBySource2", exception);
                    }
                    numArray = OpcCom.Interop.GetInt32s(ref zero, sources.Length, true);
                }
                else
                {
                    try
                    {
                        ((IOPCEventServer) base.m_server).EnableConditionBySource(sources.Length, sources);
                    }
                    catch (Exception exception2)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer.EnableConditionBySource", exception2);
                    }
                    numArray = new int[sources.Length];
                }
                ResultID[] tidArray = new ResultID[numArray.Length];
                for (int i = 0; i < numArray.Length; i++)
                {
                    tidArray[i] = OpcCom.Ae.Interop.GetResultID(numArray[i]);
                }
                return tidArray;
            }
        }

        private int FetchElements(BrowseType browseType, int maxElements, UCOMIEnumString enumerator, ArrayList elements)
        {
            string[] rgelt = new string[1];
            int celt = ((maxElements > 0) && ((maxElements - elements.Count) < rgelt.Length)) ? (maxElements - elements.Count) : rgelt.Length;
            int pceltFetched = 0;
            int num3 = enumerator.Next(celt, rgelt, out pceltFetched);
            while (num3 == 0)
            {
                for (int i = 0; i < pceltFetched; i++)
                {
                    BrowseElement element = new BrowseElement {
                        Name = rgelt[i],
                        QualifiedName = this.GetQualifiedName(rgelt[i], browseType),
                        NodeType = browseType
                    };
                    elements.Add(element);
                }
                if ((maxElements > 0) && (elements.Count >= maxElements))
                {
                    return num3;
                }
                celt = ((maxElements > 0) && ((maxElements - elements.Count) < rgelt.Length)) ? (maxElements - elements.Count) : rgelt.Length;
                num3 = enumerator.Next(celt, rgelt, out pceltFetched);
            }
            return num3;
        }

        public Condition GetConditionState(string sourceName, string conditionName, int[] attributeIDs)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer) base.m_server).GetConditionState((sourceName != null) ? sourceName : "", (conditionName != null) ? conditionName : "", (attributeIDs != null) ? attributeIDs.Length : 0, (attributeIDs != null) ? attributeIDs : new int[0], out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.GetConditionState", exception);
                }
                Condition[] conditionArray = OpcCom.Ae.Interop.GetConditions(ref zero, 1, true);
                for (int i = 0; i < conditionArray[0].Attributes.Count; i++)
                {
                    conditionArray[0].Attributes[i].ID = attributeIDs[i];
                }
                return conditionArray[0];
            }
        }

        public EnabledStateResult[] GetEnableStateByArea(string[] areas)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if ((areas == null) || (areas.Length == 0))
                {
                    return new EnabledStateResult[0];
                }
                if (!this.m_supportsAE11)
                {
                    EnabledStateResult[] resultArray = new EnabledStateResult[areas.Length];
                    for (int j = 0; j < resultArray.Length; j++)
                    {
                        resultArray[j] = new EnabledStateResult();
                        resultArray[j].Enabled = false;
                        resultArray[j].EffectivelyEnabled = false;
                        resultArray[j].ResultID = ResultID.E_FAIL;
                    }
                    return resultArray;
                }
                IntPtr zero = IntPtr.Zero;
                IntPtr pbEffectivelyEnabled = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer2) base.m_server).GetEnableStateByArea(areas.Length, areas, out zero, out pbEffectivelyEnabled, out ppErrors);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer2.GetEnableStateByArea", exception);
                }
                int[] numArray = OpcCom.Interop.GetInt32s(ref zero, areas.Length, true);
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref pbEffectivelyEnabled, areas.Length, true);
                int[] numArray3 = OpcCom.Interop.GetInt32s(ref ppErrors, areas.Length, true);
                EnabledStateResult[] resultArray2 = new EnabledStateResult[numArray3.Length];
                for (int i = 0; i < numArray3.Length; i++)
                {
                    resultArray2[i] = new EnabledStateResult();
                    resultArray2[i].Enabled = numArray[i] != 0;
                    resultArray2[i].EffectivelyEnabled = numArray2[i] != 0;
                    resultArray2[i].ResultID = OpcCom.Ae.Interop.GetResultID(numArray3[i]);
                }
                return resultArray2;
            }
        }

        public EnabledStateResult[] GetEnableStateBySource(string[] sources)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if ((sources == null) || (sources.Length == 0))
                {
                    return new EnabledStateResult[0];
                }
                if (!this.m_supportsAE11)
                {
                    EnabledStateResult[] resultArray = new EnabledStateResult[sources.Length];
                    for (int j = 0; j < resultArray.Length; j++)
                    {
                        resultArray[j] = new EnabledStateResult();
                        resultArray[j].Enabled = false;
                        resultArray[j].EffectivelyEnabled = false;
                        resultArray[j].ResultID = ResultID.E_FAIL;
                    }
                    return resultArray;
                }
                IntPtr zero = IntPtr.Zero;
                IntPtr pbEffectivelyEnabled = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer2) base.m_server).GetEnableStateBySource(sources.Length, sources, out zero, out pbEffectivelyEnabled, out ppErrors);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer2.GetEnableStateBySource", exception);
                }
                int[] numArray = OpcCom.Interop.GetInt32s(ref zero, sources.Length, true);
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref pbEffectivelyEnabled, sources.Length, true);
                int[] numArray3 = OpcCom.Interop.GetInt32s(ref ppErrors, sources.Length, true);
                EnabledStateResult[] resultArray2 = new EnabledStateResult[numArray3.Length];
                for (int i = 0; i < numArray3.Length; i++)
                {
                    resultArray2[i] = new EnabledStateResult();
                    resultArray2[i].Enabled = numArray[i] != 0;
                    resultArray2[i].EffectivelyEnabled = numArray2[i] != 0;
                    resultArray2[i].ResultID = OpcCom.Ae.Interop.GetResultID(numArray3[i]);
                }
                return resultArray2;
            }
        }

        private string GetQualifiedName(string name, BrowseType browseType)
        {
            string pszQualifiedAreaName = null;
            if (browseType == BrowseType.Area)
            {
                try
                {
                    ((IOPCEventAreaBrowser) this.m_browser).GetQualifiedAreaName(name, out pszQualifiedAreaName);
                    return pszQualifiedAreaName;
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventAreaBrowser.GetQualifiedAreaName", exception);
                }
            }
            try
            {
                ((IOPCEventAreaBrowser) this.m_browser).GetQualifiedSourceName(name, out pszQualifiedAreaName);
            }
            catch (Exception exception2)
            {
                throw OpcCom.Interop.CreateException("IOPCEventAreaBrowser.GetQualifiedSourceName", exception2);
            }
            return pszQualifiedAreaName;
        }

        public ServerStatus GetStatus()
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer) base.m_server).GetStatus(out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.GetStatus", exception);
                }
                return OpcCom.Ae.Interop.GetServerStatus(ref zero, true);
            }
        }

        private void InitializeBrowser()
        {
            if (this.m_browser == null)
            {
                object ppUnk = null;
                Guid gUID = typeof(IOPCEventAreaBrowser).GUID;
                try
                {
                    ((IOPCEventServer) base.m_server).CreateAreaBrowser(ref gUID, out ppUnk);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.CreateAreaBrowser", exception);
                }
                if (ppUnk == null)
                {
                    throw new InvalidResponseException("unknown == null");
                }
                this.m_browser = ppUnk;
            }
        }

        public int QueryAvailableFilters()
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                int pdwFilterMask = 0;
                try
                {
                    ((IOPCEventServer) base.m_server).QueryAvailableFilters(out pdwFilterMask);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.QueryAvailableFilters", exception);
                }
                return pdwFilterMask;
            }
        }

        public string[] QueryConditionNames(int eventCategory)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                int pdwCount = 0;
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer) base.m_server).QueryConditionNames(eventCategory, out pdwCount, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.QueryConditionNames", exception);
                }
                if (pdwCount == 0)
                {
                    return new string[0];
                }
                return OpcCom.Interop.GetUnicodeStrings(ref zero, pdwCount, true);
            }
        }

        public string[] QueryConditionNames(string sourceName)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                int pdwCount = 0;
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer) base.m_server).QuerySourceConditions(sourceName, out pdwCount, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.QuerySourceConditions", exception);
                }
                if (pdwCount == 0)
                {
                    return new string[0];
                }
                return OpcCom.Interop.GetUnicodeStrings(ref zero, pdwCount, true);
            }
        }

        public Opc.Ae.Attribute[] QueryEventAttributes(int eventCategory)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                int pdwCount = 0;
                IntPtr zero = IntPtr.Zero;
                IntPtr ppszAttrDescs = IntPtr.Zero;
                IntPtr ppvtAttrTypes = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer) base.m_server).QueryEventAttributes(eventCategory, out pdwCount, out zero, out ppszAttrDescs, out ppvtAttrTypes);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.QueryEventAttributes", exception);
                }
                if (pdwCount == 0)
                {
                    return new Opc.Ae.Attribute[0];
                }
                int[] numArray = OpcCom.Interop.GetInt32s(ref zero, pdwCount, true);
                string[] strArray = OpcCom.Interop.GetUnicodeStrings(ref ppszAttrDescs, pdwCount, true);
                short[] numArray2 = OpcCom.Interop.GetInt16s(ref ppvtAttrTypes, pdwCount, true);
                Opc.Ae.Attribute[] attributeArray = new Opc.Ae.Attribute[pdwCount];
                for (int i = 0; i < pdwCount; i++)
                {
                    attributeArray[i] = new Opc.Ae.Attribute();
                    attributeArray[i].ID = numArray[i];
                    attributeArray[i].Name = strArray[i];
                    attributeArray[i].DataType = OpcCom.Interop.GetType((VarEnum) numArray2[i]);
                }
                return attributeArray;
            }
        }

        public Category[] QueryEventCategories(int eventType)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                int pdwCount = 0;
                IntPtr zero = IntPtr.Zero;
                IntPtr ppszEventCategoryDescs = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer) base.m_server).QueryEventCategories(eventType, out pdwCount, out zero, out ppszEventCategoryDescs);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.QueryEventCategories", exception);
                }
                if (pdwCount == 0)
                {
                    return new Category[0];
                }
                int[] numArray = OpcCom.Interop.GetInt32s(ref zero, pdwCount, true);
                string[] strArray = OpcCom.Interop.GetUnicodeStrings(ref ppszEventCategoryDescs, pdwCount, true);
                Category[] categoryArray = new Category[pdwCount];
                for (int i = 0; i < pdwCount; i++)
                {
                    categoryArray[i] = new Category();
                    categoryArray[i].ID = numArray[i];
                    categoryArray[i].Name = strArray[i];
                }
                return categoryArray;
            }
        }

        public string[] QuerySubConditionNames(string conditionName)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                int pdwCount = 0;
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer) base.m_server).QuerySubConditionNames(conditionName, out pdwCount, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.QuerySubConditionNames", exception);
                }
                if (pdwCount == 0)
                {
                    return new string[0];
                }
                return OpcCom.Interop.GetUnicodeStrings(ref zero, pdwCount, true);
            }
        }

        public ItemUrl[] TranslateToItemIDs(string sourceName, int eventCategory, string conditionName, string subConditionName, int[] attributeIDs)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                IntPtr zero = IntPtr.Zero;
                IntPtr ppszNodeNames = IntPtr.Zero;
                IntPtr ppCLSIDs = IntPtr.Zero;
                int dwCount = (attributeIDs != null) ? attributeIDs.Length : 0;
                try
                {
                    ((IOPCEventServer) base.m_server).TranslateToItemIDs((sourceName != null) ? sourceName : "", eventCategory, (conditionName != null) ? conditionName : "", (subConditionName != null) ? subConditionName : "", dwCount, (dwCount > 0) ? attributeIDs : new int[0], out zero, out ppszNodeNames, out ppCLSIDs);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.TranslateToItemIDs", exception);
                }
                string[] strArray = OpcCom.Interop.GetUnicodeStrings(ref zero, dwCount, true);
                string[] strArray2 = OpcCom.Interop.GetUnicodeStrings(ref ppszNodeNames, dwCount, true);
                Guid[] guidArray = OpcCom.Interop.GetGUIDs(ref ppCLSIDs, dwCount, true);
                ItemUrl[] urlArray = new ItemUrl[dwCount];
                for (int i = 0; i < dwCount; i++)
                {
                    urlArray[i] = new ItemUrl();
                    urlArray[i].ItemName = strArray[i];
                    urlArray[i].ItemPath = null;
                    urlArray[i].Url.Scheme = "opcda";
                    urlArray[i].Url.HostName = strArray2[i];
                    urlArray[i].Url.Path = string.Format("{{{0}}}", guidArray[i]);
                }
                return urlArray;
            }
        }
    }
}

