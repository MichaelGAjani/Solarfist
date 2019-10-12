namespace Jund.OpcHelper.OpcCom.Da
{
    using Opc;
    using Opc.Da;
    using OpcCom;
    using OpcRcw.Da;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class Interop
    {
        internal static System.Runtime.InteropServices.FILETIME Convert(OpcRcw.Da.FILETIME input)
        {
            return new System.Runtime.InteropServices.FILETIME { dwLowDateTime = input.dwLowDateTime, dwHighDateTime = input.dwHighDateTime };
        }

        internal static OpcRcw.Da.FILETIME Convert(System.Runtime.InteropServices.FILETIME input)
        {
            return new OpcRcw.Da.FILETIME { dwLowDateTime = input.dwLowDateTime, dwHighDateTime = input.dwHighDateTime };
        }

        internal static OPCBROWSEELEMENT GetBrowseElement(BrowseElement input, bool propertiesRequested)
        {
            OPCBROWSEELEMENT opcbrowseelement = new OPCBROWSEELEMENT();
            if (input != null)
            {
                opcbrowseelement.szName = input.Name;
                opcbrowseelement.szItemID = input.ItemName;
                opcbrowseelement.dwFlagValue = 0;
                opcbrowseelement.ItemProperties = GetItemProperties(input.Properties);
                if (input.IsItem)
                {
                    opcbrowseelement.dwFlagValue |= 2;
                }
                if (input.HasChildren)
                {
                    opcbrowseelement.dwFlagValue |= 1;
                }
            }
            return opcbrowseelement;
        }

        internal static BrowseElement GetBrowseElement(IntPtr pInput, bool deallocate)
        {
            BrowseElement element = null;
            if (pInput != IntPtr.Zero)
            {
                OPCBROWSEELEMENT opcbrowseelement = (OPCBROWSEELEMENT) Marshal.PtrToStructure(pInput, typeof(OPCBROWSEELEMENT));
                element = new BrowseElement {
                    Name = opcbrowseelement.szName,
                    ItemPath = null,
                    ItemName = opcbrowseelement.szItemID,
                    IsItem = (opcbrowseelement.dwFlagValue & 2) != 0,
                    HasChildren = (opcbrowseelement.dwFlagValue & 1) != 0,
                    Properties = GetItemProperties(ref opcbrowseelement.ItemProperties, deallocate)
                };
                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCBROWSEELEMENT));
                }
            }
            return element;
        }

        internal static IntPtr GetBrowseElements(BrowseElement[] input, bool propertiesRequested)
        {
            IntPtr zero = IntPtr.Zero;
            if ((input != null) && (input.Length > 0))
            {
                zero = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(OPCBROWSEELEMENT)) * input.Length);
                IntPtr ptr = zero;
                for (int i = 0; i < input.Length; i++)
                {
                    Marshal.StructureToPtr(GetBrowseElement(input[i], propertiesRequested), ptr, false);
                    ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OPCBROWSEELEMENT)));
                }
            }
            return zero;
        }

        internal static BrowseElement[] GetBrowseElements(ref IntPtr pInput, int count, bool deallocate)
        {
            BrowseElement[] elementArray = null;
            if ((pInput != IntPtr.Zero) && (count > 0))
            {
                elementArray = new BrowseElement[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    elementArray[i] = GetBrowseElement(ptr, deallocate);
                    ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OPCBROWSEELEMENT)));
                }
                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }
            return elementArray;
        }

        internal static OPCBROWSEFILTER GetBrowseFilter(browseFilter input)
        {
            switch (input)
            {
                case browseFilter.all:
                    return OPCBROWSEFILTER.OPC_BROWSE_FILTER_ALL;

                case browseFilter.branch:
                    return OPCBROWSEFILTER.OPC_BROWSE_FILTER_BRANCHES;

                case browseFilter.item:
                    return OPCBROWSEFILTER.OPC_BROWSE_FILTER_ITEMS;
            }
            return OPCBROWSEFILTER.OPC_BROWSE_FILTER_ALL;
        }

        internal static browseFilter GetBrowseFilter(OPCBROWSEFILTER input)
        {
            switch (input)
            {
                case OPCBROWSEFILTER.OPC_BROWSE_FILTER_ALL:
                    return browseFilter.all;

                case OPCBROWSEFILTER.OPC_BROWSE_FILTER_BRANCHES:
                    return browseFilter.branch;

                case OPCBROWSEFILTER.OPC_BROWSE_FILTER_ITEMS:
                    return browseFilter.item;
            }
            return browseFilter.all;
        }

        internal static IntPtr GetHRESULTs(IResult[] results)
        {
            int[] source = new int[results.Length];
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i] != null)
                {
                    source[i] = OpcCom.Interop.GetResultID(results[i].ResultID);
                }
                else
                {
                    source[i] = -1073479679;
                }
            }
            IntPtr destination = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * results.Length);
            Marshal.Copy(source, 0, destination, results.Length);
            return destination;
        }

        internal static OPCITEMPROPERTIES GetItemProperties(ItemProperty[] input)
        {
            OPCITEMPROPERTIES opcitemproperties = new OPCITEMPROPERTIES();
            if ((input != null) && (input.Length > 0))
            {
                opcitemproperties.hrErrorID = 0;
                opcitemproperties.dwReserved = 0;
                opcitemproperties.dwNumProperties = input.Length;
                opcitemproperties.pItemProperties = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(OPCITEMPROPERTY)) * input.Length);
                bool flag = false;
                IntPtr pItemProperties = opcitemproperties.pItemProperties;
                for (int i = 0; i < input.Length; i++)
                {
                    Marshal.StructureToPtr(GetItemProperty(input[i]), pItemProperties, false);
                    pItemProperties = (IntPtr) (pItemProperties.ToInt32() + Marshal.SizeOf(typeof(OPCITEMPROPERTY)));
                    if (input[i].ResultID.Failed())
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    opcitemproperties.hrErrorID = 1;
                }
            }
            return opcitemproperties;
        }

        internal static ItemProperty[] GetItemProperties(ref OPCITEMPROPERTIES input, bool deallocate)
        {
            ItemProperty[] propertyArray = null;
            if (input.dwNumProperties > 0)
            {
                propertyArray = new ItemProperty[input.dwNumProperties];
                IntPtr pItemProperties = input.pItemProperties;
                for (int i = 0; i < propertyArray.Length; i++)
                {
                    propertyArray[i] = GetItemProperty(pItemProperties, deallocate);
                    pItemProperties = (IntPtr) (pItemProperties.ToInt32() + Marshal.SizeOf(typeof(OPCITEMPROPERTY)));
                }
                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(input.pItemProperties);
                    input.pItemProperties = IntPtr.Zero;
                }
            }
            return propertyArray;
        }

        internal static OPCITEMPROPERTY GetItemProperty(ItemProperty input)
        {
            OPCITEMPROPERTY opcitemproperty = new OPCITEMPROPERTY();
            if (input != null)
            {
                opcitemproperty.dwPropertyID = input.ID.Code;
                opcitemproperty.szDescription = input.Description;
                opcitemproperty.vtDataType = (short) OpcCom.Interop.GetType(input.DataType);
                opcitemproperty.vValue = MarshalPropertyValue(input.ID, input.Value);
                opcitemproperty.wReserved = 0;
                opcitemproperty.hrErrorID = OpcCom.Interop.GetResultID(input.ResultID);
                PropertyDescription description = PropertyDescription.Find(input.ID);
                if (description != null)
                {
                    opcitemproperty.vtDataType = (short) OpcCom.Interop.GetType(description.Type);
                }
                if (input.ResultID == ResultID.Da.E_WRITEONLY)
                {
                    opcitemproperty.hrErrorID = -1073479674;
                }
            }
            return opcitemproperty;
        }

        internal static ItemProperty GetItemProperty(IntPtr pInput, bool deallocate)
        {
            ItemProperty property = null;
            if (pInput != IntPtr.Zero)
            {
                OPCITEMPROPERTY opcitemproperty = (OPCITEMPROPERTY) Marshal.PtrToStructure(pInput, typeof(OPCITEMPROPERTY));
                property = new ItemProperty {
                    ID = GetPropertyID(opcitemproperty.dwPropertyID),
                    Description = opcitemproperty.szDescription,
                    DataType = OpcCom.Interop.GetType((VarEnum) opcitemproperty.vtDataType),
                    ItemPath = null,
                    ItemName = opcitemproperty.szItemID,
                    Value = UnmarshalPropertyValue(property.ID, opcitemproperty.vValue),
                    ResultID = OpcCom.Interop.GetResultID(opcitemproperty.hrErrorID)
                };
                if (opcitemproperty.hrErrorID == -1073479674)
                {
                    property.ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                }
                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCITEMPROPERTY));
                }
            }
            return property;
        }

        internal static IntPtr GetItemPropertyCollections(ItemPropertyCollection[] input)
        {
            IntPtr zero = IntPtr.Zero;
            if ((input != null) && (input.Length > 0))
            {
                zero = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(OPCITEMPROPERTIES)) * input.Length);
                IntPtr ptr = zero;
                for (int i = 0; i < input.Length; i++)
                {
                    OPCITEMPROPERTIES structure = new OPCITEMPROPERTIES();
                    if (input[i].Count > 0)
                    {
                        structure = GetItemProperties((ItemProperty[]) input[i].ToArray(typeof(ItemProperty)));
                    }
                    structure.hrErrorID = OpcCom.Interop.GetResultID(input[i].ResultID);
                    Marshal.StructureToPtr(structure, ptr, false);
                    ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OPCITEMPROPERTIES)));
                }
            }
            return zero;
        }

        internal static ItemPropertyCollection[] GetItemPropertyCollections(ref IntPtr pInput, int count, bool deallocate)
        {
            ItemPropertyCollection[] propertysArray = null;
            if ((pInput != IntPtr.Zero) && (count > 0))
            {
                propertysArray = new ItemPropertyCollection[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    OPCITEMPROPERTIES input = (OPCITEMPROPERTIES) Marshal.PtrToStructure(ptr, typeof(OPCITEMPROPERTIES));
                    propertysArray[i] = new ItemPropertyCollection();
                    propertysArray[i].ItemPath = null;
                    propertysArray[i].ItemName = null;
                    propertysArray[i].ResultID = OpcCom.Interop.GetResultID(input.hrErrorID);
                    ItemProperty[] itemProperties = GetItemProperties(ref input, deallocate);
                    if (itemProperties != null)
                    {
                        propertysArray[i].AddRange(itemProperties);
                    }
                    if (deallocate)
                    {
                        Marshal.DestroyStructure(ptr, typeof(OPCITEMPROPERTIES));
                    }
                    ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OPCITEMPROPERTIES)));
                }
                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }
            return propertysArray;
        }

        internal static int[] GetItemResults(ref IntPtr pInput, int count, bool deallocate)
        {
            int[] numArray = null;
            if ((pInput != IntPtr.Zero) && (count > 0))
            {
                numArray = new int[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    OPCITEMRESULT opcitemresult = (OPCITEMRESULT) Marshal.PtrToStructure(ptr, typeof(OPCITEMRESULT));
                    numArray[i] = opcitemresult.hServer;
                    if (deallocate)
                    {
                        Marshal.FreeCoTaskMem(opcitemresult.pBlob);
                        opcitemresult.pBlob = IntPtr.Zero;
                        Marshal.DestroyStructure(ptr, typeof(OPCITEMRESULT));
                    }
                    ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                }
                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }
            return numArray;
        }

        internal static IntPtr GetItemStates(ItemValueResult[] input)
        {
            IntPtr zero = IntPtr.Zero;
            if ((input != null) && (input.Length > 0))
            {
                zero = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(OPCITEMSTATE)) * input.Length);
                IntPtr ptr = zero;
                for (int i = 0; i < input.Length; i++)
                {
                    OPCITEMSTATE structure = new OPCITEMSTATE {
                        hClient = System.Convert.ToInt32(input[i].ClientHandle),
                        vDataValue = input[i].Value,
                        wQuality = input[i].QualitySpecified ? input[i].Quality.GetCode() : ((short) 0),
                        ftTimeStamp = Convert(OpcCom.Interop.GetFILETIME(input[i].Timestamp)),
                        wReserved = 0
                    };
                    Marshal.StructureToPtr(structure, ptr, false);
                    ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OPCITEMSTATE)));
                }
            }
            return zero;
        }

        internal static ItemValue[] GetItemValues(ref IntPtr pInput, int count, bool deallocate)
        {
            ItemValue[] valueArray = null;
            if ((pInput != IntPtr.Zero) && (count > 0))
            {
                valueArray = new ItemValue[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    OPCITEMSTATE opcitemstate = (OPCITEMSTATE) Marshal.PtrToStructure(ptr, typeof(OPCITEMSTATE));
                    valueArray[i] = new ItemValue();
                    valueArray[i].ClientHandle = opcitemstate.hClient;
                    valueArray[i].Value = opcitemstate.vDataValue;
                    valueArray[i].Quality = new Quality(opcitemstate.wQuality);
                    valueArray[i].QualitySpecified = true;
                    valueArray[i].Timestamp = OpcCom.Interop.GetFILETIME(Convert(opcitemstate.ftTimeStamp));
                    valueArray[i].TimestampSpecified = valueArray[i].Timestamp != DateTime.MinValue;
                    if (deallocate)
                    {
                        Marshal.DestroyStructure(ptr, typeof(OPCITEMSTATE));
                    }
                    ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OPCITEMSTATE)));
                }
                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }
            return valueArray;
        }

        internal static OPCITEMDEF[] GetOPCITEMDEFs(Item[] input)
        {
            OPCITEMDEF[] opcitemdefArray = null;
            if (input != null)
            {
                opcitemdefArray = new OPCITEMDEF[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    opcitemdefArray[i] = new OPCITEMDEF();
                    opcitemdefArray[i].szItemID = input[i].ItemName;
                    opcitemdefArray[i].szAccessPath = input[i].ItemPath;
                    opcitemdefArray[i].bActive = input[i].ActiveSpecified ? (input[i].Active ? 1 : 0) : 1;
                    opcitemdefArray[i].vtRequestedDataType = (short) OpcCom.Interop.GetType(input[i].ReqType);
                    opcitemdefArray[i].hClient = 0;
                    opcitemdefArray[i].dwBlobSize = 0;
                    opcitemdefArray[i].pBlob = IntPtr.Zero;
                }
            }
            return opcitemdefArray;
        }

        internal static OPCITEMVQT[] GetOPCITEMVQTs(ItemValue[] input)
        {
            OPCITEMVQT[] opcitemvqtArray = null;
            if (input != null)
            {
                opcitemvqtArray = new OPCITEMVQT[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    opcitemvqtArray[i] = new OPCITEMVQT();
                    DateTime datetime = input[i].TimestampSpecified ? input[i].Timestamp : DateTime.MinValue;
                    opcitemvqtArray[i].vDataValue = OpcCom.Interop.GetVARIANT(input[i].Value);
                    opcitemvqtArray[i].bQualitySpecified = input[i].QualitySpecified ? 1 : 0;
                    opcitemvqtArray[i].wQuality = input[i].QualitySpecified ? input[i].Quality.GetCode() : ((short) 0);
                    opcitemvqtArray[i].bTimeStampSpecified = input[i].TimestampSpecified ? 1 : 0;
                    opcitemvqtArray[i].ftTimeStamp = Convert(OpcCom.Interop.GetFILETIME(datetime));
                }
            }
            return opcitemvqtArray;
        }

        public static PropertyID GetPropertyID(int input)
        {
            foreach (FieldInfo info in typeof(Property).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                PropertyID yid = (PropertyID) info.GetValue(typeof(PropertyID));
                if (input == yid.Code)
                {
                    return yid;
                }
            }
            return new PropertyID(input);
        }

        internal static int[] GetPropertyIDs(PropertyID[] propertyIDs)
        {
            ArrayList list = new ArrayList();
            if (propertyIDs != null)
            {
                foreach (PropertyID yid in propertyIDs)
                {
                    list.Add(yid.Code);
                }
            }
            return (int[]) list.ToArray(typeof(int));
        }

        internal static PropertyID[] GetPropertyIDs(int[] propertyIDs)
        {
            ArrayList list = new ArrayList();
            if (propertyIDs != null)
            {
                foreach (int num in propertyIDs)
                {
                    list.Add(GetPropertyID(num));
                }
            }
            return (PropertyID[]) list.ToArray(typeof(PropertyID));
        }

        internal static ServerStatus GetServerStatus(ref IntPtr pInput, bool deallocate)
        {
            ServerStatus status = null;
            if (pInput != IntPtr.Zero)
            {
                OPCSERVERSTATUS opcserverstatus = (OPCSERVERSTATUS) Marshal.PtrToStructure(pInput, typeof(OPCSERVERSTATUS));
                status = new ServerStatus {
                    VendorInfo = opcserverstatus.szVendorInfo,
                    ProductVersion = string.Format("{0}.{1}.{2}", opcserverstatus.wMajorVersion, opcserverstatus.wMinorVersion, opcserverstatus.wBuildNumber),
                    ServerState = (serverState) opcserverstatus.dwServerState,
                    StatusInfo = null,
                    StartTime = OpcCom.Interop.GetFILETIME(Convert(opcserverstatus.ftStartTime)),
                    CurrentTime = OpcCom.Interop.GetFILETIME(Convert(opcserverstatus.ftCurrentTime)),
                    LastUpdateTime = OpcCom.Interop.GetFILETIME(Convert(opcserverstatus.ftLastUpdateTime))
                };
                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCSERVERSTATUS));
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }
            return status;
        }

        internal static OPCSERVERSTATUS GetServerStatus(ServerStatus input, int groupCount)
        {
            OPCSERVERSTATUS opcserverstatus = new OPCSERVERSTATUS();
            if (input != null)
            {
                opcserverstatus.szVendorInfo = input.VendorInfo;
                opcserverstatus.wMajorVersion = 0;
                opcserverstatus.wMinorVersion = 0;
                opcserverstatus.wBuildNumber = 0;
                opcserverstatus.dwServerState = (OPCSERVERSTATE) input.ServerState;
                opcserverstatus.ftStartTime = Convert(OpcCom.Interop.GetFILETIME(input.StartTime));
                opcserverstatus.ftCurrentTime = Convert(OpcCom.Interop.GetFILETIME(input.CurrentTime));
                opcserverstatus.ftLastUpdateTime = Convert(OpcCom.Interop.GetFILETIME(input.LastUpdateTime));
                opcserverstatus.dwBandWidth = -1;
                opcserverstatus.dwGroupCount = groupCount;
                opcserverstatus.wReserved = 0;
                if (input.ProductVersion == null)
                {
                    return opcserverstatus;
                }
                string[] strArray = input.ProductVersion.Split(new char[] { '.' });
                if (strArray.Length > 0)
                {
                    try
                    {
                        opcserverstatus.wMajorVersion = System.Convert.ToInt16(strArray[0]);
                    }
                    catch
                    {
                        opcserverstatus.wMajorVersion = 0;
                    }
                }
                if (strArray.Length > 1)
                {
                    try
                    {
                        opcserverstatus.wMinorVersion = System.Convert.ToInt16(strArray[1]);
                    }
                    catch
                    {
                        opcserverstatus.wMinorVersion = 0;
                    }
                }
                opcserverstatus.wBuildNumber = 0;
                for (int i = 2; i < strArray.Length; i++)
                {
                    try
                    {
                        opcserverstatus.wBuildNumber = (short) ((opcserverstatus.wBuildNumber * 100) + System.Convert.ToInt16(strArray[i]));
                    }
                    catch
                    {
                        opcserverstatus.wBuildNumber = 0;
                        return opcserverstatus;
                    }
                }
            }
            return opcserverstatus;
        }

        internal static object MarshalPropertyValue(PropertyID propertyID, object input)
        {
            if (input == null)
            {
                return null;
            }
            try
            {
                if (propertyID == Property.DATATYPE)
                {
                    return (short) OpcCom.Interop.GetType((System.Type) input);
                }
                if (propertyID == Property.ACCESSRIGHTS)
                {
                    switch (((accessRights) input))
                    {
                        case accessRights.readable:
                            return 1;

                        case accessRights.writable:
                            return 2;

                        case accessRights.readWritable:
                            return 3;
                    }
                    return null;
                }
                if (propertyID == Property.EUTYPE)
                {
                    switch (((euType) input))
                    {
                        case euType.noEnum:
                            return OPCEUTYPE.OPC_NOENUM;

                        case euType.analog:
                            return OPCEUTYPE.OPC_ANALOG;

                        case euType.enumerated:
                            return OPCEUTYPE.OPC_ENUMERATED;
                    }
                    return null;
                }
                if (propertyID == Property.QUALITY)
                {
                    Quality quality = (Quality) input;
                    return quality.GetCode();
                }
                if (!(propertyID == Property.TIMESTAMP) || (input.GetType() != typeof(DateTime)))
                {
                    return input;
                }
                DateTime time = (DateTime) input;
                if (time != DateTime.MinValue)
                {
                    return time.ToUniversalTime();
                }
                return time;
            }
            catch
            {
            }
            return input;
        }

        internal static object UnmarshalPropertyValue(PropertyID propertyID, object input)
        {
            if (input == null)
            {
                return null;
            }
            try
            {
                if (propertyID == Property.DATATYPE)
                {
                    return OpcCom.Interop.GetType((VarEnum) System.Convert.ToUInt16(input));
                }
                if (propertyID == Property.ACCESSRIGHTS)
                {
                    switch (System.Convert.ToInt32(input))
                    {
                        case 1:
                            return accessRights.readable;

                        case 2:
                            return accessRights.writable;

                        case 3:
                            return accessRights.readWritable;
                    }
                    return null;
                }
                if (propertyID == Property.EUTYPE)
                {
                    switch (((OPCEUTYPE) input))
                    {
                        case OPCEUTYPE.OPC_NOENUM:
                            return euType.noEnum;

                        case OPCEUTYPE.OPC_ANALOG:
                            return euType.analog;

                        case OPCEUTYPE.OPC_ENUMERATED:
                            return euType.enumerated;
                    }
                    return null;
                }
                if (propertyID == Property.QUALITY)
                {
                    return new Quality(System.Convert.ToInt16(input));
                }
                if (!(propertyID == Property.TIMESTAMP) || (input.GetType() != typeof(DateTime)))
                {
                    return input;
                }
                DateTime time = (DateTime) input;
                if (time != DateTime.MinValue)
                {
                    return time.ToLocalTime();
                }
                return time;
            }
            catch
            {
            }
            return input;
        }
    }
}

