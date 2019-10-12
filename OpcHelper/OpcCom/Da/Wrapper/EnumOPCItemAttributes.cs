namespace Jund.OpcHelper.OpcCom.Da.Wrapper
{
    using Opc.Da;
    using OpcCom;
    using OpcCom.Da;
    using OpcRcw.Da;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using System.Threading;

    [CLSCompliant(false)]
    public class EnumOPCItemAttributes : IEnumOPCItemAttributes
    {
        private int m_index = 0;
        private ArrayList m_items = new ArrayList();

        internal EnumOPCItemAttributes(ICollection items)
        {
            if (items != null)
            {
                foreach (ItemAttributes attributes in items)
                {
                    this.m_items.Add(attributes);
                }
            }
        }

        public void Clone(out IEnumOPCItemAttributes ppEnumItemAttributes)
        {
            EnumOPCItemAttributes attributes;
            Monitor.Enter(attributes = this);
            try
            {
                ppEnumItemAttributes = new EnumOPCItemAttributes(this.m_items);
            }
            catch (Exception exception)
            {
                throw OpcCom.Da.Wrapper.Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(attributes);
            }
        }

        public void Next(int celt, out IntPtr ppItemArray, out int pceltFetched)
        {
            EnumOPCItemAttributes attributes2;
            Monitor.Enter(attributes2 = this);
            try
            {
                pceltFetched = 0;
                ppItemArray = IntPtr.Zero;
                if (this.m_index < this.m_items.Count)
                {
                    pceltFetched = this.m_items.Count - this.m_index;
                    if (pceltFetched > celt)
                    {
                        pceltFetched = celt;
                    }
                    ppItemArray = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(OPCITEMATTRIBUTES)) * pceltFetched);
                    IntPtr ptr = ppItemArray;
                    for (int i = 0; i < pceltFetched; i++)
                    {
                        ItemAttributes attributes = (ItemAttributes) this.m_items[this.m_index + i];
                        OPCITEMATTRIBUTES structure = new OPCITEMATTRIBUTES {
                            szItemID = attributes.ItemID,
                            szAccessPath = attributes.AccessPath,
                            hClient = attributes.ClientHandle,
                            hServer = attributes.ServerHandle,
                            bActive = attributes.Active ? 1 : 0,
                            vtCanonicalDataType = (short) OpcCom.Interop.GetType(attributes.CanonicalDataType),
                            vtRequestedDataType = (short) OpcCom.Interop.GetType(attributes.RequestedDataType),
                            dwAccessRights = (int) OpcCom.Da.Interop.MarshalPropertyValue(Property.ACCESSRIGHTS, attributes.AccessRights),
                            dwBlobSize = 0,
                            pBlob = IntPtr.Zero,
                            dwEUType = (OPCEUTYPE) OpcCom.Da.Interop.MarshalPropertyValue(Property.EUTYPE, attributes.EuType),
                            vEUInfo = null
                        };
                        switch (attributes.EuType)
                        {
                            case euType.analog:
                                structure.vEUInfo = new double[] { attributes.MinValue, attributes.MaxValue };
                                break;

                            case euType.enumerated:
                                structure.vEUInfo = attributes.EuInfo;
                                break;
                        }
                        Marshal.StructureToPtr(structure, ptr, false);
                        ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OPCITEMATTRIBUTES)));
                    }
                    this.m_index += pceltFetched;
                }
            }
            catch (Exception exception)
            {
                throw OpcCom.Da.Wrapper.Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(attributes2);
            }
        }

        public void Reset()
        {
            EnumOPCItemAttributes attributes;
            Monitor.Enter(attributes = this);
            try
            {
                this.m_index = 0;
            }
            catch (Exception exception)
            {
                throw OpcCom.Da.Wrapper.Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(attributes);
            }
        }

        public void Skip(int celt)
        {
            EnumOPCItemAttributes attributes;
            Monitor.Enter(attributes = this);
            try
            {
                this.m_index += celt;
                if (this.m_index > this.m_items.Count)
                {
                    this.m_index = this.m_items.Count;
                }
            }
            catch (Exception exception)
            {
                throw OpcCom.Da.Wrapper.Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(attributes);
            }
        }

        public class ItemAttributes
        {
            public string AccessPath = null;
            public accessRights AccessRights = accessRights.readWritable;
            public bool Active = false;
            public Type CanonicalDataType = null;
            public int ClientHandle = -1;
            public string[] EuInfo = null;
            public euType EuType = euType.noEnum;
            public string ItemID = null;
            public double MaxValue = 0.0;
            public double MinValue = 0.0;
            public Type RequestedDataType = null;
            public int ServerHandle = -1;
        }
    }
}

