namespace Jund.OpcHelper.OpcCom.Dx
{
    using Opc;
    using Opc.Dx;
    using OpcCom;
    using OpcRcw.Dx;
    using System;
    using System.Runtime.InteropServices;

    public class Interop
    {
        internal static OpcRcw.Dx.DXConnection GetDXConnection(Opc.Dx.DXConnection input)
        {
            OpcRcw.Dx.DXConnection connection = new OpcRcw.Dx.DXConnection {
                dwMask = 0,
                szItemPath = null,
                szItemName = null,
                szVersion = null,
                dwBrowsePathCount = 0,
                pszBrowsePaths = IntPtr.Zero,
                szName = null,
                szDescription = null,
                szKeyword = null,
                bDefaultSourceItemConnected = 0,
                bDefaultTargetItemConnected = 0,
                bDefaultOverridden = 0,
                vDefaultOverrideValue = null,
                vSubstituteValue = null,
                bEnableSubstituteValue = 0,
                szTargetItemPath = null,
                szTargetItemName = null,
                szSourceServerName = null,
                szSourceItemPath = null,
                szSourceItemName = null,
                dwSourceItemQueueSize = 0,
                dwUpdateRate = 0,
                fltDeadBand = 0f,
                szVendorData = null
            };
            if (input.ItemName != null)
            {
                connection.dwMask |= 2;
                connection.szItemName = input.ItemName;
            }
            if (input.ItemPath != null)
            {
                connection.dwMask |= 1;
                connection.szItemPath = input.ItemPath;
            }
            if (input.Version != null)
            {
                connection.dwMask |= 4;
                connection.szVersion = input.Version;
            }
            if (input.BrowsePaths.Count > 0)
            {
                connection.dwMask |= 8;
                connection.dwBrowsePathCount = input.BrowsePaths.Count;
                connection.pszBrowsePaths = OpcCom.Interop.GetUnicodeStrings(input.BrowsePaths.ToArray());
            }
            if (input.Name != null)
            {
                connection.dwMask |= 0x10;
                connection.szName = input.Name;
            }
            if (input.Description != null)
            {
                connection.dwMask |= 0x20;
                connection.szDescription = input.Description;
            }
            if (input.Keyword != null)
            {
                connection.dwMask |= 0x40;
                connection.szKeyword = input.Keyword;
            }
            if (input.DefaultSourceItemConnectedSpecified)
            {
                connection.dwMask |= 0x80;
                connection.bDefaultSourceItemConnected = input.DefaultSourceItemConnected ? 1 : 0;
            }
            if (input.DefaultTargetItemConnectedSpecified)
            {
                connection.dwMask |= 0x100;
                connection.bDefaultTargetItemConnected = input.DefaultTargetItemConnected ? 1 : 0;
            }
            if (input.DefaultOverriddenSpecified)
            {
                connection.dwMask |= 0x200;
                connection.bDefaultOverridden = input.DefaultOverridden ? 1 : 0;
            }
            if (input.DefaultOverrideValue != null)
            {
                connection.dwMask |= 0x400;
                connection.vDefaultOverrideValue = input.DefaultOverrideValue;
            }
            if (input.SubstituteValue != null)
            {
                connection.dwMask |= 0x800;
                connection.vSubstituteValue = input.SubstituteValue;
            }
            if (input.EnableSubstituteValueSpecified)
            {
                connection.dwMask |= 0x1000;
                connection.bEnableSubstituteValue = input.EnableSubstituteValue ? 1 : 0;
            }
            if (input.TargetItemName != null)
            {
                connection.dwMask |= 0x4000;
                connection.szTargetItemName = input.TargetItemName;
            }
            if (input.TargetItemPath != null)
            {
                connection.dwMask |= 0x2000;
                connection.szTargetItemPath = input.TargetItemPath;
            }
            if (input.SourceServerName != null)
            {
                connection.dwMask |= 0x8000;
                connection.szSourceServerName = input.SourceServerName;
            }
            if (input.SourceItemName != null)
            {
                connection.dwMask |= 0x20000;
                connection.szSourceItemName = input.SourceItemName;
            }
            if (input.SourceItemPath != null)
            {
                connection.dwMask |= 0x10000;
                connection.szSourceItemPath = input.SourceItemPath;
            }
            if (input.SourceItemQueueSizeSpecified)
            {
                connection.dwMask |= 0x40000;
                connection.dwSourceItemQueueSize = input.SourceItemQueueSize;
            }
            if (input.UpdateRateSpecified)
            {
                connection.dwMask |= 0x80000;
                connection.dwUpdateRate = input.UpdateRate;
            }
            if (input.DeadbandSpecified)
            {
                connection.dwMask |= 0x100000;
                connection.fltDeadBand = input.Deadband;
            }
            if (input.VendorData != null)
            {
                connection.dwMask |= 0x200000;
                connection.szVendorData = input.VendorData;
            }
            return connection;
        }

        internal static Opc.Dx.DXConnection GetDXConnection(OpcRcw.Dx.DXConnection input, bool deallocate)
        {
            Opc.Dx.DXConnection connection = new Opc.Dx.DXConnection {
                ItemPath = null,
                ItemName = null,
                Version = null
            };
            connection.BrowsePaths.Clear();
            connection.Name = null;
            connection.Description = null;
            connection.Keyword = null;
            connection.DefaultSourceItemConnected = false;
            connection.DefaultSourceItemConnectedSpecified = false;
            connection.DefaultTargetItemConnected = false;
            connection.DefaultTargetItemConnectedSpecified = false;
            connection.DefaultOverridden = false;
            connection.DefaultOverriddenSpecified = false;
            connection.DefaultOverrideValue = null;
            connection.SubstituteValue = null;
            connection.EnableSubstituteValue = false;
            connection.EnableSubstituteValueSpecified = false;
            connection.TargetItemPath = null;
            connection.TargetItemName = null;
            connection.SourceServerName = null;
            connection.SourceItemPath = null;
            connection.SourceItemName = null;
            connection.SourceItemQueueSize = 0;
            connection.SourceItemQueueSizeSpecified = false;
            connection.UpdateRate = 0;
            connection.UpdateRateSpecified = false;
            connection.Deadband = 0f;
            connection.DeadbandSpecified = false;
            connection.VendorData = null;
            if ((input.dwMask & 2) != 0)
            {
                connection.ItemName = input.szItemName;
            }
            if ((input.dwMask & 1) != 0)
            {
                connection.ItemPath = input.szItemPath;
            }
            if ((input.dwMask & 4) != 0)
            {
                connection.Version = input.szVersion;
            }
            if ((input.dwMask & 8) != 0)
            {
                string[] c = OpcCom.Interop.GetUnicodeStrings(ref input.pszBrowsePaths, input.dwBrowsePathCount, deallocate);
                if (c != null)
                {
                    connection.BrowsePaths.AddRange(c);
                }
            }
            if ((input.dwMask & 0x10) != 0)
            {
                connection.Name = input.szName;
            }
            if ((input.dwMask & 0x20) != 0)
            {
                connection.Description = input.szDescription;
            }
            if ((input.dwMask & 0x40) != 0)
            {
                connection.Keyword = input.szKeyword;
            }
            if ((input.dwMask & 0x80) != 0)
            {
                connection.DefaultSourceItemConnected = input.bDefaultSourceItemConnected != 0;
                connection.DefaultSourceItemConnectedSpecified = true;
            }
            if ((input.dwMask & 0x100) != 0)
            {
                connection.DefaultTargetItemConnected = input.bDefaultTargetItemConnected != 0;
                connection.DefaultTargetItemConnectedSpecified = true;
            }
            if ((input.dwMask & 0x200) != 0)
            {
                connection.DefaultOverridden = input.bDefaultOverridden != 0;
                connection.DefaultOverriddenSpecified = true;
            }
            if ((input.dwMask & 0x400) != 0)
            {
                connection.DefaultOverrideValue = input.vDefaultOverrideValue;
            }
            if ((input.dwMask & 0x800) != 0)
            {
                connection.SubstituteValue = input.vSubstituteValue;
            }
            if ((input.dwMask & 0x1000) != 0)
            {
                connection.EnableSubstituteValue = input.bEnableSubstituteValue != 0;
                connection.EnableSubstituteValueSpecified = true;
            }
            if ((input.dwMask & 0x4000) != 0)
            {
                connection.TargetItemName = input.szTargetItemName;
            }
            if ((input.dwMask & 0x2000) != 0)
            {
                connection.TargetItemPath = input.szTargetItemPath;
            }
            if ((input.dwMask & 0x8000) != 0)
            {
                connection.SourceServerName = input.szSourceServerName;
            }
            if ((input.dwMask & 0x20000) != 0)
            {
                connection.SourceItemName = input.szSourceItemName;
            }
            if ((input.dwMask & 0x10000) != 0)
            {
                connection.SourceItemPath = input.szSourceItemPath;
            }
            if ((input.dwMask & 0x40000) != 0)
            {
                connection.SourceItemQueueSize = input.dwSourceItemQueueSize;
                connection.SourceItemQueueSizeSpecified = true;
            }
            if ((input.dwMask & 0x80000) != 0)
            {
                connection.UpdateRate = input.dwUpdateRate;
                connection.UpdateRateSpecified = true;
            }
            if ((input.dwMask & 0x100000) != 0)
            {
                connection.Deadband = input.fltDeadBand;
                connection.DeadbandSpecified = true;
            }
            if ((input.dwMask & 0x200000) != 0)
            {
                connection.VendorData = input.szVendorData;
            }
            return connection;
        }

        internal static OpcRcw.Dx.DXConnection[] GetDXConnections(Opc.Dx.DXConnection[] input)
        {
            OpcRcw.Dx.DXConnection[] connectionArray = null;
            if ((input != null) && (input.Length > 0))
            {
                connectionArray = new OpcRcw.Dx.DXConnection[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    connectionArray[i] = GetDXConnection(input[i]);
                }
            }
            return connectionArray;
        }

        internal static Opc.Dx.DXConnection[] GetDXConnections(ref IntPtr pInput, int count, bool deallocate)
        {
            Opc.Dx.DXConnection[] connectionArray = null;
            if ((pInput != IntPtr.Zero) && (count > 0))
            {
                connectionArray = new Opc.Dx.DXConnection[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    OpcRcw.Dx.DXConnection input = (OpcRcw.Dx.DXConnection) Marshal.PtrToStructure(ptr, typeof(OpcRcw.Dx.DXConnection));
                    connectionArray[i] = GetDXConnection(input, deallocate);
                    if (deallocate)
                    {
                        Marshal.DestroyStructure(ptr, typeof(OpcRcw.Dx.DXConnection));
                    }
                    ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OpcRcw.Dx.DXConnection)));
                }
                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }
            return connectionArray;
        }

        internal static GeneralResponse GetGeneralResponse(DXGeneralResponse input, bool deallocate)
        {
            return new GeneralResponse(input.szConfigurationVersion, GetIdentifiedResults(ref input.pIdentifiedResults, input.dwCount, deallocate));
        }

        internal static Opc.Dx.IdentifiedResult[] GetIdentifiedResults(ref IntPtr pInput, int count, bool deallocate)
        {
            Opc.Dx.IdentifiedResult[] resultArray = null;
            if ((pInput != IntPtr.Zero) && (count > 0))
            {
                resultArray = new Opc.Dx.IdentifiedResult[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    OpcRcw.Dx.IdentifiedResult result = (OpcRcw.Dx.IdentifiedResult) Marshal.PtrToStructure(ptr, typeof(OpcRcw.Dx.IdentifiedResult));
                    resultArray[i] = new Opc.Dx.IdentifiedResult();
                    resultArray[i].ItemName = result.szItemName;
                    resultArray[i].ItemPath = result.szItemPath;
                    resultArray[i].Version = result.szVersion;
                    resultArray[i].ResultID = OpcCom.Interop.GetResultID(result.hResultCode);
                    if (deallocate)
                    {
                        Marshal.DestroyStructure(ptr, typeof(OpcRcw.Dx.IdentifiedResult));
                    }
                    ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OpcRcw.Dx.IdentifiedResult)));
                }
                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }
            return resultArray;
        }

        internal static OpcRcw.Dx.ItemIdentifier[] GetItemIdentifiers(Opc.Dx.ItemIdentifier[] input)
        {
            OpcRcw.Dx.ItemIdentifier[] identifierArray = null;
            if ((input != null) && (input.Length > 0))
            {
                identifierArray = new OpcRcw.Dx.ItemIdentifier[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    identifierArray[i] = new OpcRcw.Dx.ItemIdentifier();
                    identifierArray[i].szItemName = input[i].ItemName;
                    identifierArray[i].szItemPath = input[i].ItemPath;
                    identifierArray[i].szVersion = input[i].Version;
                }
            }
            return identifierArray;
        }

        internal static ResultID[] GetResultIDs(ref IntPtr pInput, int count, bool deallocate)
        {
            ResultID[] tidArray = null;
            if ((pInput != IntPtr.Zero) && (count > 0))
            {
                tidArray = new ResultID[count];
                int[] numArray = OpcCom.Interop.GetInt32s(ref pInput, count, deallocate);
                for (int i = 0; i < count; i++)
                {
                    tidArray[i] = OpcCom.Interop.GetResultID(numArray[i]);
                }
            }
            return tidArray;
        }

        internal static OpcRcw.Dx.SourceServer[] GetSourceServers(Opc.Dx.SourceServer[] input)
        {
            OpcRcw.Dx.SourceServer[] serverArray = null;
            if ((input != null) && (input.Length > 0))
            {
                serverArray = new OpcRcw.Dx.SourceServer[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    serverArray[i] = new OpcRcw.Dx.SourceServer();
                    serverArray[i].dwMask = 0x7fffffff;
                    serverArray[i].szItemName = input[i].ItemName;
                    serverArray[i].szItemPath = input[i].ItemPath;
                    serverArray[i].szVersion = input[i].Version;
                    serverArray[i].szName = input[i].Name;
                    serverArray[i].szDescription = input[i].Description;
                    serverArray[i].szServerType = input[i].ServerType;
                    serverArray[i].szServerURL = input[i].ServerURL;
                    serverArray[i].bDefaultSourceServerConnected = input[i].DefaultConnected ? 1 : 0;
                }
            }
            return serverArray;
        }

        internal static Opc.Dx.SourceServer[] GetSourceServers(ref IntPtr pInput, int count, bool deallocate)
        {
            Opc.Dx.SourceServer[] serverArray = null;
            if ((pInput != IntPtr.Zero) && (count > 0))
            {
                serverArray = new Opc.Dx.SourceServer[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    OpcRcw.Dx.SourceServer server = (OpcRcw.Dx.SourceServer) Marshal.PtrToStructure(ptr, typeof(OpcRcw.Dx.SourceServer));
                    serverArray[i] = new Opc.Dx.SourceServer();
                    serverArray[i].ItemName = server.szItemName;
                    serverArray[i].ItemPath = server.szItemPath;
                    serverArray[i].Version = server.szVersion;
                    serverArray[i].Name = server.szName;
                    serverArray[i].Description = server.szDescription;
                    serverArray[i].ServerType = server.szServerType;
                    serverArray[i].ServerURL = server.szServerURL;
                    serverArray[i].DefaultConnected = server.bDefaultSourceServerConnected != 0;
                    ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OpcRcw.Dx.SourceServer)));
                }
                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }
            return serverArray;
        }
    }
}

