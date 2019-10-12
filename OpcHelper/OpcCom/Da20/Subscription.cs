namespace Jund.OpcHelper.OpcCom.Da20
{
    using Opc;
    using Opc.Da;
    using OpcCom;
    using OpcCom.Da;
    using OpcRcw.Da;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public class Subscription : OpcCom.Da.Subscription
    {
        internal Subscription(object group, SubscriptionState state, int filters) : base(group, state, filters)
        {
        }

        protected override IdentifiedResult[] BeginRead(ItemIdentifier[] itemIDs, Item[] items, int requestID, out int cancelID)
        {
            IdentifiedResult[] resultArray2;
            try
            {
                int[] phServer = new int[itemIDs.Length];
                for (int i = 0; i < itemIDs.Length; i++)
                {
                    phServer[i] = (int) itemIDs[i].ServerHandle;
                }
                IntPtr zero = IntPtr.Zero;
                ((IOPCAsyncIO2) base.m_group).Read(itemIDs.Length, phServer, requestID, out cancelID, out zero);
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref zero, itemIDs.Length, true);
                IdentifiedResult[] resultArray = new IdentifiedResult[itemIDs.Length];
                for (int j = 0; j < itemIDs.Length; j++)
                {
                    resultArray[j] = new IdentifiedResult(itemIDs[j]);
                    resultArray[j].ResultID = OpcCom.Interop.GetResultID(numArray2[j]);
                    resultArray[j].DiagnosticInfo = null;
                    if (numArray2[j] == -1073479674)
                    {
                        resultArray[j].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                    }
                }
                resultArray2 = resultArray;
            }
            catch (Exception exception)
            {
                throw OpcCom.Interop.CreateException("IOPCAsyncIO2.Read", exception);
            }
            return resultArray2;
        }

        protected override IdentifiedResult[] BeginWrite(ItemIdentifier[] itemIDs, ItemValue[] items, int requestID, out int cancelID)
        {
            cancelID = 0;
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            IdentifiedResult[] resultArray = new IdentifiedResult[itemIDs.Length];
            for (int i = 0; i < itemIDs.Length; i++)
            {
                resultArray[i] = new IdentifiedResult(itemIDs[i]);
                resultArray[i].ResultID = ResultID.S_OK;
                resultArray[i].DiagnosticInfo = null;
                if (items[i].QualitySpecified || items[i].TimestampSpecified)
                {
                    resultArray[i].ResultID = ResultID.Da.E_NO_WRITEQT;
                    resultArray[i].DiagnosticInfo = null;
                }
                else
                {
                    list.Add(resultArray[i]);
                    list2.Add(OpcCom.Interop.GetVARIANT(items[i].Value));
                }
            }
            if (list.Count != 0)
            {
                try
                {
                    int[] phServer = new int[list.Count];
                    for (int j = 0; j < list.Count; j++)
                    {
                        phServer[j] = (int) ((IdentifiedResult) list[j]).ServerHandle;
                    }
                    IntPtr zero = IntPtr.Zero;
                    ((IOPCAsyncIO2) base.m_group).Write(list.Count, phServer, (object[]) list2.ToArray(typeof(object)), requestID, out cancelID, out zero);
                    int[] numArray2 = OpcCom.Interop.GetInt32s(ref zero, list.Count, true);
                    for (int k = 0; k < list.Count; k++)
                    {
                        IdentifiedResult result = (IdentifiedResult) list[k];
                        result.ResultID = OpcCom.Interop.GetResultID(numArray2[k]);
                        result.DiagnosticInfo = null;
                        if (numArray2[k] == -1073479674)
                        {
                            resultArray[k].ResultID = new ResultID(ResultID.Da.E_READONLY, -1073479674L);
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCAsyncIO2.Write", exception);
                }
            }
            return resultArray;
        }

        public override bool GetEnabled()
        {
            bool flag;
            lock (this)
            {
                if (base.m_group == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    int pbEnable = 0;
                    ((IOPCAsyncIO2) base.m_group).GetEnable(out pbEnable);
                    flag = pbEnable != 0;
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCAsyncIO2.GetEnable", exception);
                }
            }
            return flag;
        }

        private void Read(ItemValueResult[] items, bool cache)
        {
            if (items.Length != 0)
            {
                int[] phServer = new int[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    phServer[i] = (int) items[i].ServerHandle;
                }
                IntPtr zero = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCSyncIO) base.m_group).Read(cache ? OPCDATASOURCE.OPC_DS_CACHE : OPCDATASOURCE.OPC_DS_DEVICE, items.Length, phServer, out zero, out ppErrors);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCSyncIO.Read", exception);
                }
                ItemValue[] valueArray = OpcCom.Da.Interop.GetItemValues(ref zero, items.Length, true);
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref ppErrors, items.Length, true);
                for (int j = 0; j < items.Length; j++)
                {
                    items[j].ResultID = OpcCom.Interop.GetResultID(numArray2[j]);
                    items[j].DiagnosticInfo = null;
                    if (numArray2[j] == -1073479674)
                    {
                        items[j].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                    }
                    if (items[j].ResultID.Succeeded())
                    {
                        items[j].Value = valueArray[j].Value;
                        items[j].Quality = valueArray[j].Quality;
                        items[j].QualitySpecified = valueArray[j].QualitySpecified;
                        items[j].Timestamp = valueArray[j].Timestamp;
                        items[j].TimestampSpecified = valueArray[j].TimestampSpecified;
                    }
                }
            }
        }

        protected override ItemValueResult[] Read(ItemIdentifier[] itemIDs, Item[] items)
        {
            ItemValueResult[] resultArray = new ItemValueResult[itemIDs.Length];
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            for (int i = 0; i < itemIDs.Length; i++)
            {
                resultArray[i] = new ItemValueResult(itemIDs[i]);
                if (items[i].MaxAgeSpecified && ((items[i].MaxAge < 0) || (items[i].MaxAge == 0x7fffffff)))
                {
                    list.Add(resultArray[i]);
                }
                else
                {
                    list2.Add(resultArray[i]);
                }
            }
            if (list.Count > 0)
            {
                this.Read((ItemValueResult[]) list.ToArray(typeof(ItemValueResult)), true);
            }
            if (list2.Count > 0)
            {
                this.Read((ItemValueResult[]) list2.ToArray(typeof(ItemValueResult)), false);
            }
            return resultArray;
        }

        public override void Refresh()
        {
            lock (this)
            {
                if (base.m_group == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    int pdwCancelID = 0;
                    ((IOPCAsyncIO2) base.m_group).Refresh2(OPCDATASOURCE.OPC_DS_CACHE, ++base.m_counter, out pdwCancelID);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCAsyncIO2.RefreshMaxAge", exception);
                }
            }
        }

        public override void SetEnabled(bool enabled)
        {
            lock (this)
            {
                if (base.m_group == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    ((IOPCAsyncIO2) base.m_group).SetEnable(enabled ? 1 : 0);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCAsyncIO2.SetEnable", exception);
                }
            }
        }

        protected override IdentifiedResult[] Write(ItemIdentifier[] itemIDs, ItemValue[] items)
        {
            IdentifiedResult[] resultArray = new IdentifiedResult[itemIDs.Length];
            ArrayList list = new ArrayList(itemIDs.Length);
            ArrayList list2 = new ArrayList(itemIDs.Length);
            for (int i = 0; i < items.Length; i++)
            {
                resultArray[i] = new IdentifiedResult(itemIDs[i]);
                if (items[i].QualitySpecified || items[i].TimestampSpecified)
                {
                    resultArray[i].ResultID = ResultID.Da.E_NO_WRITEQT;
                    resultArray[i].DiagnosticInfo = null;
                }
                else
                {
                    list.Add(resultArray[i]);
                    list2.Add(items[i]);
                }
            }
            if (list.Count != 0)
            {
                int[] phServer = new int[list.Count];
                object[] pItemValues = new object[list.Count];
                for (int j = 0; j < phServer.Length; j++)
                {
                    phServer[j] = (int) ((IdentifiedResult) list[j]).ServerHandle;
                    pItemValues[j] = OpcCom.Interop.GetVARIANT(((ItemValue) list2[j]).Value);
                }
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCSyncIO) base.m_group).Write(list.Count, phServer, pItemValues, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCSyncIO.Write", exception);
                }
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref zero, list.Count, true);
                for (int k = 0; k < list.Count; k++)
                {
                    IdentifiedResult result = (IdentifiedResult) list[k];
                    result.ResultID = OpcCom.Interop.GetResultID(numArray2[k]);
                    result.DiagnosticInfo = null;
                    if (numArray2[k] == -1073479674)
                    {
                        resultArray[k].ResultID = new ResultID(ResultID.Da.E_READONLY, -1073479674L);
                    }
                }
            }
            return resultArray;
        }
    }
}

