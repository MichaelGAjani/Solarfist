namespace Jund.OpcHelper.OpcCom
{
    using Opc;
    using Opc.Da;
    using System;
    using System.Globalization;
    using System.Net;
    using System.Runtime.InteropServices;

    public class Interop
    {
        private const uint CLSCTX_INPROC_HANDLER = 2;
        private const uint CLSCTX_INPROC_SERVER = 1;
        private const uint CLSCTX_LOCAL_SERVER = 4;
        private const uint CLSCTX_REMOTE_SERVER = 0x10;
        private const uint EOAC_ACCESS_CONTROL = 4;
        private const uint EOAC_APPID = 8;
        private const uint EOAC_CLOAKING = 0x10;
        private const uint EOAC_MUTUAL_AUTH = 1;
        private const uint EOAC_NONE = 0;
        private const uint EOAC_SECURE_REFS = 2;
        private static readonly DateTime FILETIME_BaseTime = new DateTime(0x641, 1, 1);
        private const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;
        private const uint FORMAT_MESSAGE_IGNORE_INSERTS = 0x200;
        private static readonly Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");
        private const uint LEVEL_SERVER_INFO_100 = 100;
        private const uint LEVEL_SERVER_INFO_101 = 0x65;
        internal const int LOCALE_SYSTEM_DEFAULT = 0x800;
        internal const int LOCALE_USER_DEFAULT = 0x400;
        private static bool m_preserveUTC = false;
        private const int MAX_COMPUTERNAME_LENGTH = 0x1f;
        private const int MAX_MESSAGE_LENGTH = 0x400;
        private const int MAX_PREFERRED_LENGTH = -1;
        private const uint RPC_C_AUTHN_DCE_PRIVATE = 1;
        private const uint RPC_C_AUTHN_DCE_PUBLIC = 2;
        private const uint RPC_C_AUTHN_DEC_PUBLIC = 4;
        private const uint RPC_C_AUTHN_DEFAULT = uint.MaxValue;
        private const uint RPC_C_AUTHN_DIGEST = 0x15;
        private const uint RPC_C_AUTHN_DPA = 0x11;
        private const uint RPC_C_AUTHN_GSS_KERBEROS = 0x10;
        private const uint RPC_C_AUTHN_GSS_NEGOTIATE = 9;
        private const uint RPC_C_AUTHN_GSS_SCHANNEL = 14;
        private const uint RPC_C_AUTHN_LEVEL_CALL = 3;
        private const uint RPC_C_AUTHN_LEVEL_CONNECT = 2;
        private const uint RPC_C_AUTHN_LEVEL_DEFAULT = 0;
        private const uint RPC_C_AUTHN_LEVEL_NONE = 1;
        private const uint RPC_C_AUTHN_LEVEL_PKT = 4;
        private const uint RPC_C_AUTHN_LEVEL_PKT_INTEGRITY = 5;
        private const uint RPC_C_AUTHN_LEVEL_PKT_PRIVACY = 6;
        private const uint RPC_C_AUTHN_MQ = 100;
        private const uint RPC_C_AUTHN_MSN = 0x12;
        private const uint RPC_C_AUTHN_NONE = 0;
        private const uint RPC_C_AUTHN_WINNT = 10;
        private const uint RPC_C_AUTHZ_DCE = 2;
        private const uint RPC_C_AUTHZ_DEFAULT = uint.MaxValue;
        private const uint RPC_C_AUTHZ_NAME = 1;
        private const uint RPC_C_AUTHZ_NONE = 0;
        private const uint RPC_C_IMP_LEVEL_ANONYMOUS = 1;
        private const uint RPC_C_IMP_LEVEL_DELEGATE = 4;
        private const uint RPC_C_IMP_LEVEL_IDENTIFY = 2;
        private const uint RPC_C_IMP_LEVEL_IMPERSONATE = 3;
        private const uint SEC_WINNT_AUTH_IDENTITY_ANSI = 1;
        private const uint SEC_WINNT_AUTH_IDENTITY_UNICODE = 2;
        private const uint SV_TYPE_SERVER = 2;
        private const uint SV_TYPE_WORKSTATION = 1;
        private const int VARIANT_SIZE = 0x10;

        [DllImport("ole32.dll")]
        private static extern void CoCreateInstanceEx(ref Guid clsid, [MarshalAs(UnmanagedType.IUnknown)] object punkOuter, uint dwClsCtx, [In] ref COSERVERINFO pServerInfo, uint dwCount, [In, Out] MULTI_QI[] pResults);
        [DllImport("ole32.dll")]
        private static extern void CoGetClassObject([MarshalAs(UnmanagedType.LPStruct)] Guid clsid, uint dwClsContext, [In] ref COSERVERINFO pServerInfo, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);
        [DllImport("ole32.dll")]
        private static extern int CoInitializeSecurity(IntPtr pSecDesc, int cAuthSvc, SOLE_AUTHENTICATION_SERVICE[] asAuthSvc, IntPtr pReserved1, uint dwAuthnLevel, uint dwImpLevel, IntPtr pAuthList, uint dwCapabilities, IntPtr pReserved3);
        public static Exception CreateException(string message, Exception e)
        {
            return CreateException(message, Marshal.GetHRForException(e));
        }

        public static Exception CreateException(string message, int code)
        {
            return new ResultIDException(GetResultID(code), message);
        }

        public static object CreateInstance(Guid clsid, string hostName, NetworkCredential credential)
        {
            ServerInfo info = new ServerInfo();
            COSERVERINFO pServerInfo = info.Allocate(hostName, credential);
            GCHandle handle = GCHandle.Alloc(IID_IUnknown, GCHandleType.Pinned);
            MULTI_QI[] pResults = new MULTI_QI[1];
            pResults[0].iid = handle.AddrOfPinnedObject();
            pResults[0].pItf = null;
            pResults[0].hr = 0;
            try
            {
                uint dwClsCtx = 5;
                if (((hostName != null) && (hostName.Length > 0)) && (hostName != "localhost"))
                {
                    dwClsCtx = 20;
                }
                CoCreateInstanceEx(ref clsid, null, dwClsCtx, ref pServerInfo, 1, pResults);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
                info.Deallocate();
            }
            if (pResults[0].hr != 0)
            {
                throw new ExternalException("CoCreateInstanceEx: " + GetSystemMessage((int) pResults[0].hr));
            }
            return pResults[0].pItf;
        }

        public static object CreateInstanceWithLicenseKey(Guid clsid, string hostName, NetworkCredential credential, string licenseKey)
        {
            ServerInfo info = new ServerInfo();
            COSERVERINFO pServerInfo = info.Allocate(hostName, credential);
            object ppvObject = null;
            IClassFactory2 pProxy = null;
            try
            {
                uint dwClsContext = 5;
                if ((hostName != null) && (hostName.Length > 0))
                {
                    dwClsContext = 20;
                }
                object ppv = null;
                CoGetClassObject(clsid, dwClsContext, ref pServerInfo, typeof(IClassFactory2).GUID, out ppv);
                pProxy = (IClassFactory2) ppv;
                IClientSecurity security = (IClientSecurity) pProxy;
                uint pAuthnSvc = 0;
                uint pAuthzSvc = 0;
                string pServerPrincName = "";
                uint pAuthnLevel = 0;
                uint pImpLevel = 0;
                IntPtr zero = IntPtr.Zero;
                uint pCapabilities = 0;
                security.QueryBlanket(pProxy, ref pAuthnSvc, ref pAuthzSvc, ref pServerPrincName, ref pAuthnLevel, ref pImpLevel, ref zero, ref pCapabilities);
                pAuthnSvc = uint.MaxValue;
                pAuthnLevel = 2;
                security.SetBlanket(pProxy, pAuthnSvc, pAuthzSvc, pServerPrincName, pAuthnLevel, pImpLevel, zero, pCapabilities);
                pProxy.CreateInstanceLic(null, null, IID_IUnknown, licenseKey, out ppvObject);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                info.Deallocate();
            }
            return ppvObject;
        }

        public static string[] EnumComputers()
        {
            IntPtr ptr;
            int entriesread = 0;
            int totalentries = 0;
            int num3 = NetServerEnum(IntPtr.Zero, 100, out ptr, -1, out entriesread, out totalentries, 3, IntPtr.Zero, IntPtr.Zero);
            if (num3 != 0)
            {
                throw new ApplicationException("NetApi Error = " + string.Format("0x{0,0:X}", num3));
            }
            string[] strArray = new string[entriesread];
            IntPtr ptr2 = ptr;
            for (int i = 0; i < entriesread; i++)
            {
                SERVER_INFO_100 server_info_ = (SERVER_INFO_100) Marshal.PtrToStructure(ptr2, typeof(SERVER_INFO_100));
                strArray[i] = server_info_.sv100_name;
                ptr2 = (IntPtr) (ptr2.ToInt32() + Marshal.SizeOf(typeof(SERVER_INFO_100)));
            }
            NetApiBufferFree(ptr);
            return strArray;
        }

        [DllImport("Kernel32.dll")]
        private static extern int FormatMessageW(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, IntPtr lpBuffer, int nSize, IntPtr Arguments);
        public static string GetComputerName()
        {
            string str = null;
            int lpnSize = 0x20;
            IntPtr lpBuffer = Marshal.AllocCoTaskMem(lpnSize * 2);
            if (GetComputerNameW(lpBuffer, ref lpnSize) != 0)
            {
                str = Marshal.PtrToStringUni(lpBuffer, lpnSize);
            }
            Marshal.FreeCoTaskMem(lpBuffer);
            return str;
        }

        [DllImport("Kernel32.dll")]
        private static extern int GetComputerNameW(IntPtr lpBuffer, ref int lpnSize);
        public static FILETIME GetFILETIME(DateTime datetime)
        {
            FILETIME filetime;
            if (datetime <= FILETIME_BaseTime)
            {
                filetime.dwHighDateTime = 0;
                filetime.dwLowDateTime = 0;
                return filetime;
            }
            long ticks = 0L;
            if (m_preserveUTC)
            {
                ticks = datetime.Subtract(new TimeSpan(FILETIME_BaseTime.Ticks)).Ticks;
            }
            else
            {
                ticks = datetime.ToUniversalTime().Subtract(new TimeSpan(FILETIME_BaseTime.Ticks)).Ticks;
            }
            filetime.dwHighDateTime = (int) (((ulong) (ticks >> 0x20)) & 0xffffffffL);
            filetime.dwLowDateTime = (int) (((ulong) ticks) & 0xffffffffL);
            return filetime;
        }

        public static DateTime GetFILETIME(IntPtr pFiletime)
        {
            if (pFiletime == IntPtr.Zero)
            {
                return DateTime.MinValue;
            }
            return GetFILETIME((FILETIME) Marshal.PtrToStructure(pFiletime, typeof(FILETIME)));
        }

        public static DateTime GetFILETIME(FILETIME filetime)
        {
            long dwHighDateTime = filetime.dwHighDateTime;
            if (dwHighDateTime < 0L)
            {
                dwHighDateTime += 0x100000000L;
            }
            long ticks = dwHighDateTime << 0x20;
            dwHighDateTime = filetime.dwLowDateTime;
            if (dwHighDateTime < 0L)
            {
                dwHighDateTime += 0x100000000L;
            }
            ticks += dwHighDateTime;
            if (ticks == 0L)
            {
                return DateTime.MinValue;
            }
            if (m_preserveUTC)
            {
                return FILETIME_BaseTime.Add(new TimeSpan(ticks));
            }
            return FILETIME_BaseTime.Add(new TimeSpan(ticks)).ToLocalTime();
        }

        public static IntPtr GetFILETIMEs(DateTime[] datetimes)
        {
            int num = (datetimes != null) ? datetimes.Length : 0;
            if (num <= 0)
            {
                return IntPtr.Zero;
            }
            IntPtr ptr = Marshal.AllocCoTaskMem(num * Marshal.SizeOf(typeof(FILETIME)));
            IntPtr ptr2 = ptr;
            for (int i = 0; i < num; i++)
            {
                Marshal.StructureToPtr(GetFILETIME(datetimes[i]), ptr2, false);
                ptr2 = (IntPtr) (ptr2.ToInt32() + Marshal.SizeOf(typeof(FILETIME)));
            }
            return ptr;
        }

        public static DateTime[] GetFILETIMEs(ref IntPtr pArray, int size, bool deallocate)
        {
            if ((pArray == IntPtr.Zero) || (size <= 0))
            {
                return null;
            }
            DateTime[] timeArray = new DateTime[size];
            IntPtr pFiletime = pArray;
            for (int i = 0; i < size; i++)
            {
                timeArray[i] = GetFILETIME(pFiletime);
                pFiletime = (IntPtr) (pFiletime.ToInt32() + Marshal.SizeOf(typeof(FILETIME)));
            }
            if (deallocate)
            {
                Marshal.FreeCoTaskMem(pArray);
                pArray = IntPtr.Zero;
            }
            return timeArray;
        }

        public static Guid[] GetGUIDs(ref IntPtr pInput, int size, bool deallocate)
        {
            if ((pInput == IntPtr.Zero) || (size <= 0))
            {
                return null;
            }
            Guid[] guidArray = new Guid[size];
            IntPtr ptr = pInput;
            for (int i = 0; i < size; i++)
            {
                GUID guid = (GUID) Marshal.PtrToStructure(pInput, typeof(GUID));
                guidArray[i] = new Guid(guid.Data1, guid.Data2, guid.Data3, guid.Data4);
                ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(GUID)));
            }
            if (deallocate)
            {
                Marshal.FreeCoTaskMem(pInput);
                pInput = IntPtr.Zero;
            }
            return guidArray;
        }

        public static IntPtr GetInt16s(short[] input)
        {
            IntPtr zero = IntPtr.Zero;
            if (input != null)
            {
                zero = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(short)) * input.Length);
                Marshal.Copy(input, 0, zero, input.Length);
            }
            return zero;
        }

        public static short[] GetInt16s(ref IntPtr pArray, int size, bool deallocate)
        {
            if ((pArray == IntPtr.Zero) || (size <= 0))
            {
                return null;
            }
            short[] destination = new short[size];
            Marshal.Copy(pArray, destination, 0, size);
            if (deallocate)
            {
                Marshal.FreeCoTaskMem(pArray);
                pArray = IntPtr.Zero;
            }
            return destination;
        }

        public static IntPtr GetInt32s(int[] input)
        {
            IntPtr zero = IntPtr.Zero;
            if (input != null)
            {
                zero = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * input.Length);
                Marshal.Copy(input, 0, zero, input.Length);
            }
            return zero;
        }

        public static int[] GetInt32s(ref IntPtr pArray, int size, bool deallocate)
        {
            if ((pArray == IntPtr.Zero) || (size <= 0))
            {
                return null;
            }
            int[] destination = new int[size];
            Marshal.Copy(pArray, destination, 0, size);
            if (deallocate)
            {
                Marshal.FreeCoTaskMem(pArray);
                pArray = IntPtr.Zero;
            }
            return destination;
        }

        internal static string GetLocale(int input)
        {
            string name;
            try
            {
                if (((input == 0x800) || (input == 0x400)) || (input == 0))
                {
                    return CultureInfo.InvariantCulture.Name;
                }
                name = new CultureInfo(input).Name;
            }
            catch
            {
                throw new ExternalException("Invalid LCID", -2147024809);
            }
            return name;
        }

        internal static int GetLocale(string input)
        {
            if ((input == null) || (input == ""))
            {
                return 0;
            }
            CultureInfo currentCulture = null;
            try
            {
                currentCulture = new CultureInfo(input);
            }
            catch
            {
                currentCulture = CultureInfo.CurrentCulture;
            }
            return currentCulture.LCID;
        }

        internal static int GetResultID(ResultID input)
        {
            if ((input.Name != null) && (input.Name.Namespace == "http://opcfoundation.org/DataAccess/"))
            {
                if (input == ResultID.S_OK)
                {
                    return 0;
                }
                if (input == ResultID.E_FAIL)
                {
                    return -2147467259;
                }
                if (input == ResultID.E_INVALIDARG)
                {
                    return -2147024809;
                }
                if (input == ResultID.Da.E_BADTYPE)
                {
                    return -1073479676;
                }
                if (input == ResultID.Da.E_READONLY)
                {
                    return -1073479674;
                }
                if (input == ResultID.Da.E_WRITEONLY)
                {
                    return -1073479674;
                }
                if (input == ResultID.Da.E_RANGE)
                {
                    return -1073479669;
                }
                if (input == ResultID.E_OUTOFMEMORY)
                {
                    return -2147024882;
                }
                if (input == ResultID.E_NOTSUPPORTED)
                {
                    return -2147467262;
                }
                if (input == ResultID.Da.E_INVALIDHANDLE)
                {
                    return -1073479679;
                }
                if (input == ResultID.Da.E_UNKNOWN_ITEM_NAME)
                {
                    return -1073479673;
                }
                if (input == ResultID.Da.E_INVALID_ITEM_NAME)
                {
                    return -1073479672;
                }
                if (input == ResultID.Da.E_INVALID_ITEM_PATH)
                {
                    return -1073479672;
                }
                if (input == ResultID.Da.E_UNKNOWN_ITEM_PATH)
                {
                    return -1073479670;
                }
                if (input == ResultID.Da.E_INVALID_FILTER)
                {
                    return -1073479671;
                }
                if (input == ResultID.Da.S_UNSUPPORTEDRATE)
                {
                    return 0x4000d;
                }
                if (input == ResultID.Da.S_CLAMP)
                {
                    return 0x4000e;
                }
                if (input == ResultID.Da.E_INVALID_PID)
                {
                    return -1073479165;
                }
                if (input == ResultID.Da.E_NO_ITEM_DEADBAND)
                {
                    return -1073478655;
                }
                if (input == ResultID.Da.E_NO_ITEM_BUFFERING)
                {
                    return -1073478654;
                }
                if (input == ResultID.Da.E_NO_WRITEQT)
                {
                    return -1073478650;
                }
                if (input == ResultID.Da.E_INVALIDCONTINUATIONPOINT)
                {
                    return -1073478653;
                }
                if (input == ResultID.Da.S_DATAQUEUEOVERFLOW)
                {
                    return 0x40404;
                }
            }
            else if ((input.Name != null) && (input.Name.Namespace == "http://opcfoundation.org/ComplexData/"))
            {
                if (input == ResultID.Cpx.E_TYPE_CHANGED)
                {
                    return -1073478649;
                }
                if (input == ResultID.Cpx.E_FILTER_DUPLICATE)
                {
                    return -1073478648;
                }
                if (input == ResultID.Cpx.E_FILTER_INVALID)
                {
                    return -1073478647;
                }
                if (input == ResultID.Cpx.E_FILTER_ERROR)
                {
                    return -1073478646;
                }
                if (input == ResultID.Cpx.S_FILTER_NO_DATA)
                {
                    return 0x4040b;
                }
            }
            else if ((input.Name != null) && (input.Name.Namespace == "http://opcfoundation.org/HistoricalDataAccess/"))
            {
                if (input == ResultID.Hda.E_MAXEXCEEDED)
                {
                    return -1073475583;
                }
                if (input == ResultID.Hda.S_NODATA)
                {
                    return 0x40041002;
                }
                if (input == ResultID.Hda.S_MOREDATA)
                {
                    return 0x40041003;
                }
                if (input == ResultID.Hda.E_INVALIDAGGREGATE)
                {
                    return -1073475580;
                }
                if (input == ResultID.Hda.S_CURRENTVALUE)
                {
                    return 0x40041005;
                }
                if (input == ResultID.Hda.S_EXTRADATA)
                {
                    return 0x40041006;
                }
                if (input == ResultID.Hda.E_UNKNOWNATTRID)
                {
                    return -1073475576;
                }
                if (input == ResultID.Hda.E_NOT_AVAIL)
                {
                    return -1073475575;
                }
                if (input == ResultID.Hda.E_INVALIDDATATYPE)
                {
                    return -1073475574;
                }
                if (input == ResultID.Hda.E_DATAEXISTS)
                {
                    return -1073475573;
                }
                if (input == ResultID.Hda.E_INVALIDATTRID)
                {
                    return -1073475572;
                }
                if (input == ResultID.Hda.E_NODATAEXISTS)
                {
                    return -1073475571;
                }
                if (input == ResultID.Hda.S_INSERTED)
                {
                    return 0x4004100e;
                }
                if (input == ResultID.Hda.S_REPLACED)
                {
                    return 0x4004100f;
                }
            }
            if ((input.Name != null) && (input.Name.Namespace == "http://opcfoundation.org/DataExchange/"))
            {
                if (input == ResultID.Dx.E_PERSISTING)
                {
                    return -1073477888;
                }
                if (input == ResultID.Dx.E_NOITEMLIST)
                {
                    return -1073477887;
                }
                if (input == ResultID.Dx.E_SERVER_STATE)
                {
                    return -1073477885;
                }
                if (input == ResultID.Dx.E_VERSION_MISMATCH)
                {
                    return -1073477885;
                }
                if (input == ResultID.Dx.E_UNKNOWN_ITEM_PATH)
                {
                    return -1073477884;
                }
                if (input == ResultID.Dx.E_UNKNOWN_ITEM_NAME)
                {
                    return -1073477883;
                }
                if (input == ResultID.Dx.E_INVALID_ITEM_PATH)
                {
                    return -1073477882;
                }
                if (input == ResultID.Dx.E_INVALID_ITEM_NAME)
                {
                    return -1073477881;
                }
                if (input == ResultID.Dx.E_INVALID_NAME)
                {
                    return -1073477880;
                }
                if (input == ResultID.Dx.E_DUPLICATE_NAME)
                {
                    return -1073477879;
                }
                if (input == ResultID.Dx.E_INVALID_BROWSE_PATH)
                {
                    return -1073477878;
                }
                if (input == ResultID.Dx.E_INVALID_SERVER_URL)
                {
                    return -1073477877;
                }
                if (input == ResultID.Dx.E_INVALID_SERVER_TYPE)
                {
                    return -1073477876;
                }
                if (input == ResultID.Dx.E_UNSUPPORTED_SERVER_TYPE)
                {
                    return -1073477875;
                }
                if (input == ResultID.Dx.E_CONNECTIONS_EXIST)
                {
                    return -1073477874;
                }
                if (input == ResultID.Dx.E_TOO_MANY_CONNECTIONS)
                {
                    return -1073477873;
                }
                if (input == ResultID.Dx.E_OVERRIDE_BADTYPE)
                {
                    return -1073477872;
                }
                if (input == ResultID.Dx.E_OVERRIDE_RANGE)
                {
                    return -1073477871;
                }
                if (input == ResultID.Dx.E_SUBSTITUTE_BADTYPE)
                {
                    return -1073477870;
                }
                if (input == ResultID.Dx.E_SUBSTITUTE_RANGE)
                {
                    return -1073477869;
                }
                if (input == ResultID.Dx.E_INVALID_TARGET_ITEM)
                {
                    return -1073477868;
                }
                if (input == ResultID.Dx.E_UNKNOWN_TARGET_ITEM)
                {
                    return -1073477867;
                }
                if (input == ResultID.Dx.E_TARGET_ALREADY_CONNECTED)
                {
                    return -1073477866;
                }
                if (input == ResultID.Dx.E_UNKNOWN_SERVER_NAME)
                {
                    return -1073477865;
                }
                if (input == ResultID.Dx.E_UNKNOWN_SOURCE_ITEM)
                {
                    return -1073477864;
                }
                if (input == ResultID.Dx.E_INVALID_SOURCE_ITEM)
                {
                    return -1073477863;
                }
                if (input == ResultID.Dx.E_INVALID_QUEUE_SIZE)
                {
                    return -1073477862;
                }
                if (input == ResultID.Dx.E_INVALID_DEADBAND)
                {
                    return -1073477861;
                }
                if (input == ResultID.Dx.E_INVALID_CONFIG_FILE)
                {
                    return -1073477860;
                }
                if (input == ResultID.Dx.E_PERSIST_FAILED)
                {
                    return -1073477859;
                }
                if (input == ResultID.Dx.E_TARGET_FAULT)
                {
                    return -1073477858;
                }
                if (input == ResultID.Dx.E_TARGET_NO_ACCESSS)
                {
                    return -1073477857;
                }
                if (input == ResultID.Dx.E_SOURCE_SERVER_FAULT)
                {
                    return -1073477856;
                }
                if (input == ResultID.Dx.E_SOURCE_SERVER_NO_ACCESSS)
                {
                    return -1073477855;
                }
                if (input == ResultID.Dx.E_SUBSCRIPTION_FAULT)
                {
                    return -1073477854;
                }
                if (input == ResultID.Dx.E_SOURCE_ITEM_BADRIGHTS)
                {
                    return -1073477853;
                }
                if (input == ResultID.Dx.E_SOURCE_ITEM_BAD_QUALITY)
                {
                    return -1073477852;
                }
                if (input == ResultID.Dx.E_SOURCE_ITEM_BADTYPE)
                {
                    return -1073477851;
                }
                if (input == ResultID.Dx.E_SOURCE_ITEM_RANGE)
                {
                    return -1073477850;
                }
                if (input == ResultID.Dx.E_SOURCE_SERVER_NOT_CONNECTED)
                {
                    return -1073477849;
                }
                if (input == ResultID.Dx.E_SOURCE_SERVER_TIMEOUT)
                {
                    return -1073477848;
                }
                if (input == ResultID.Dx.E_TARGET_ITEM_DISCONNECTED)
                {
                    return -1073477847;
                }
                if (input == ResultID.Dx.E_TARGET_NO_WRITES_ATTEMPTED)
                {
                    return -1073477846;
                }
                if (input == ResultID.Dx.E_TARGET_ITEM_BADTYPE)
                {
                    return -1073477845;
                }
                if (input == ResultID.Dx.E_TARGET_ITEM_RANGE)
                {
                    return -1073477844;
                }
                if (input == ResultID.Dx.S_TARGET_SUBSTITUTED)
                {
                    return 0x40780;
                }
                if (input == ResultID.Dx.S_TARGET_OVERRIDEN)
                {
                    return 0x40781;
                }
                if (input == ResultID.Dx.S_CLAMP)
                {
                    return 0x40782;
                }
            }
            else if (input.Code == -1)
            {
                if (input.Succeeded())
                {
                    return 1;
                }
                return -2147467259;
            }
            return input.Code;
        }

        internal static ResultID GetResultID(int input)
        {
            switch (input)
            {
                case -2147352571:
                    return new ResultID(ResultID.Da.E_BADTYPE, (long) input);

                case -2147352566:
                    return new ResultID(ResultID.Da.E_RANGE, (long) input);

                case -2147217401:
                    return new ResultID(ResultID.Hda.W_NOFILTER, (long) input);

                case -2147467262:
                    return new ResultID(ResultID.E_NOTSUPPORTED, (long) input);

                case -2147467259:
                    return new ResultID(ResultID.E_FAIL, (long) input);

                case -2147024882:
                    return new ResultID(ResultID.E_OUTOFMEMORY, (long) input);

                case -2147024809:
                    return new ResultID(ResultID.E_INVALIDARG, (long) input);

                case -1073479679:
                    return new ResultID(ResultID.Da.E_INVALIDHANDLE, (long) input);

                case -1073479676:
                    return new ResultID(ResultID.Da.E_BADTYPE, (long) input);

                case -1073479673:
                    return new ResultID(ResultID.Da.E_UNKNOWN_ITEM_NAME, (long) input);

                case -1073479672:
                    return new ResultID(ResultID.Da.E_INVALID_ITEM_NAME, (long) input);

                case -1073479671:
                    return new ResultID(ResultID.Da.E_INVALID_FILTER, (long) input);

                case -1073479670:
                    return new ResultID(ResultID.Da.E_UNKNOWN_ITEM_PATH, (long) input);

                case -1073479669:
                    return new ResultID(ResultID.Da.E_RANGE, (long) input);

                case -1073479165:
                    return new ResultID(ResultID.Da.E_INVALID_PID, (long) input);

                case -1073479164:
                    return new ResultID(ResultID.Ae.E_INVALIDTIME, (long) input);

                case -1073479163:
                    return new ResultID(ResultID.Ae.E_BUSY, (long) input);

                case -1073479162:
                    return new ResultID(ResultID.Ae.E_NOINFO, (long) input);

                case -1073478655:
                    return new ResultID(ResultID.Da.E_NO_ITEM_DEADBAND, (long) input);

                case -1073478654:
                    return new ResultID(ResultID.Da.E_NO_ITEM_BUFFERING, (long) input);

                case -1073478653:
                    return new ResultID(ResultID.Da.E_INVALIDCONTINUATIONPOINT, (long) input);

                case -1073478650:
                    return new ResultID(ResultID.Da.E_NO_WRITEQT, (long) input);

                case -1073478649:
                    return new ResultID(ResultID.Cpx.E_TYPE_CHANGED, (long) input);

                case -1073478648:
                    return new ResultID(ResultID.Cpx.E_FILTER_DUPLICATE, (long) input);

                case -1073478647:
                    return new ResultID(ResultID.Cpx.E_FILTER_INVALID, (long) input);

                case -1073478646:
                    return new ResultID(ResultID.Cpx.E_FILTER_ERROR, (long) input);

                case -1073477888:
                    return new ResultID(ResultID.Dx.E_PERSISTING, (long) input);

                case -1073477887:
                    return new ResultID(ResultID.Dx.E_NOITEMLIST, (long) input);

                case -1073477886:
                    return new ResultID(ResultID.Dx.E_VERSION_MISMATCH, (long) input);

                case -1073477885:
                    return new ResultID(ResultID.Dx.E_VERSION_MISMATCH, (long) input);

                case -1073477884:
                    return new ResultID(ResultID.Dx.E_UNKNOWN_ITEM_PATH, (long) input);

                case -1073477883:
                    return new ResultID(ResultID.Dx.E_UNKNOWN_ITEM_NAME, (long) input);

                case -1073477882:
                    return new ResultID(ResultID.Dx.E_INVALID_ITEM_PATH, (long) input);

                case -1073477881:
                    return new ResultID(ResultID.Dx.E_INVALID_ITEM_NAME, (long) input);

                case -1073477880:
                    return new ResultID(ResultID.Dx.E_INVALID_NAME, (long) input);

                case -1073477879:
                    return new ResultID(ResultID.Dx.E_DUPLICATE_NAME, (long) input);

                case -1073477878:
                    return new ResultID(ResultID.Dx.E_INVALID_BROWSE_PATH, (long) input);

                case -1073477877:
                    return new ResultID(ResultID.Dx.E_INVALID_SERVER_URL, (long) input);

                case -1073477876:
                    return new ResultID(ResultID.Dx.E_INVALID_SERVER_TYPE, (long) input);

                case -1073477875:
                    return new ResultID(ResultID.Dx.E_UNSUPPORTED_SERVER_TYPE, (long) input);

                case -1073477874:
                    return new ResultID(ResultID.Dx.E_CONNECTIONS_EXIST, (long) input);

                case -1073477873:
                    return new ResultID(ResultID.Dx.E_TOO_MANY_CONNECTIONS, (long) input);

                case -1073477872:
                    return new ResultID(ResultID.Dx.E_OVERRIDE_BADTYPE, (long) input);

                case -1073477871:
                    return new ResultID(ResultID.Dx.E_OVERRIDE_RANGE, (long) input);

                case -1073477870:
                    return new ResultID(ResultID.Dx.E_SUBSTITUTE_BADTYPE, (long) input);

                case -1073477869:
                    return new ResultID(ResultID.Dx.E_SUBSTITUTE_RANGE, (long) input);

                case -1073477868:
                    return new ResultID(ResultID.Dx.E_INVALID_TARGET_ITEM, (long) input);

                case -1073477867:
                    return new ResultID(ResultID.Dx.E_UNKNOWN_TARGET_ITEM, (long) input);

                case -1073477866:
                    return new ResultID(ResultID.Dx.E_TARGET_ALREADY_CONNECTED, (long) input);

                case -1073477865:
                    return new ResultID(ResultID.Dx.E_UNKNOWN_SERVER_NAME, (long) input);

                case -1073477864:
                    return new ResultID(ResultID.Dx.E_UNKNOWN_SOURCE_ITEM, (long) input);

                case -1073477863:
                    return new ResultID(ResultID.Dx.E_INVALID_SOURCE_ITEM, (long) input);

                case -1073477862:
                    return new ResultID(ResultID.Dx.E_INVALID_QUEUE_SIZE, (long) input);

                case -1073477861:
                    return new ResultID(ResultID.Dx.E_INVALID_DEADBAND, (long) input);

                case -1073477860:
                    return new ResultID(ResultID.Dx.E_INVALID_CONFIG_FILE, (long) input);

                case -1073477859:
                    return new ResultID(ResultID.Dx.E_PERSIST_FAILED, (long) input);

                case -1073477858:
                    return new ResultID(ResultID.Dx.E_TARGET_FAULT, (long) input);

                case -1073477857:
                    return new ResultID(ResultID.Dx.E_TARGET_NO_ACCESSS, (long) input);

                case -1073477856:
                    return new ResultID(ResultID.Dx.E_SOURCE_SERVER_FAULT, (long) input);

                case -1073477855:
                    return new ResultID(ResultID.Dx.E_SOURCE_SERVER_NO_ACCESSS, (long) input);

                case -1073477854:
                    return new ResultID(ResultID.Dx.E_SUBSCRIPTION_FAULT, (long) input);

                case -1073477853:
                    return new ResultID(ResultID.Dx.E_SOURCE_ITEM_BADRIGHTS, (long) input);

                case -1073477852:
                    return new ResultID(ResultID.Dx.E_SOURCE_ITEM_BAD_QUALITY, (long) input);

                case -1073477851:
                    return new ResultID(ResultID.Dx.E_SOURCE_ITEM_BADTYPE, (long) input);

                case -1073477850:
                    return new ResultID(ResultID.Dx.E_SOURCE_ITEM_RANGE, (long) input);

                case -1073477849:
                    return new ResultID(ResultID.Dx.E_SOURCE_SERVER_NOT_CONNECTED, (long) input);

                case -1073477848:
                    return new ResultID(ResultID.Dx.E_SOURCE_SERVER_TIMEOUT, (long) input);

                case -1073477847:
                    return new ResultID(ResultID.Dx.E_TARGET_ITEM_DISCONNECTED, (long) input);

                case -1073477846:
                    return new ResultID(ResultID.Dx.E_TARGET_NO_WRITES_ATTEMPTED, (long) input);

                case -1073477845:
                    return new ResultID(ResultID.Dx.E_TARGET_ITEM_BADTYPE, (long) input);

                case -1073477844:
                    return new ResultID(ResultID.Dx.E_TARGET_ITEM_RANGE, (long) input);

                case -1073475583:
                    return new ResultID(ResultID.Hda.E_MAXEXCEEDED, (long) input);

                case -1073475580:
                    return new ResultID(ResultID.Hda.E_INVALIDAGGREGATE, (long) input);

                case -1073475576:
                    return new ResultID(ResultID.Hda.E_UNKNOWNATTRID, (long) input);

                case -1073475575:
                    return new ResultID(ResultID.Hda.E_NOT_AVAIL, (long) input);

                case -1073475574:
                    return new ResultID(ResultID.Hda.E_INVALIDDATATYPE, (long) input);

                case -1073475573:
                    return new ResultID(ResultID.Hda.E_DATAEXISTS, (long) input);

                case -1073475572:
                    return new ResultID(ResultID.Hda.E_INVALIDATTRID, (long) input);

                case -1073475571:
                    return new ResultID(ResultID.Hda.E_NODATAEXISTS, (long) input);

                case 0x4000d:
                    return new ResultID(ResultID.Da.S_UNSUPPORTEDRATE, (long) input);

                case 0x4000e:
                    return new ResultID(ResultID.Da.S_CLAMP, (long) input);

                case 0:
                    return new ResultID(ResultID.S_OK, (long) input);

                case 0x40200:
                    return new ResultID(ResultID.Ae.S_ALREADYACKED, (long) input);

                case 0x40201:
                    return new ResultID(ResultID.Ae.S_INVALIDBUFFERTIME, (long) input);

                case 0x40202:
                    return new ResultID(ResultID.Ae.S_INVALIDMAXSIZE, (long) input);

                case 0x40203:
                    return new ResultID(ResultID.Ae.S_INVALIDKEEPALIVETIME, (long) input);

                case 0x40404:
                    return new ResultID(ResultID.Da.S_DATAQUEUEOVERFLOW, (long) input);

                case 0x4040b:
                    return new ResultID(ResultID.Cpx.S_FILTER_NO_DATA, (long) input);

                case 0x40780:
                    return new ResultID(ResultID.Dx.S_TARGET_SUBSTITUTED, (long) input);

                case 0x40781:
                    return new ResultID(ResultID.Dx.S_TARGET_OVERRIDEN, (long) input);

                case 0x40782:
                    return new ResultID(ResultID.Dx.S_CLAMP, (long) input);

                case 0x40041002:
                    return new ResultID(ResultID.Hda.S_NODATA, (long) input);

                case 0x40041003:
                    return new ResultID(ResultID.Hda.S_MOREDATA, (long) input);

                case 0x40041005:
                    return new ResultID(ResultID.Hda.S_CURRENTVALUE, (long) input);

                case 0x40041006:
                    return new ResultID(ResultID.Hda.S_EXTRADATA, (long) input);

                case 0x4004100e:
                    return new ResultID(ResultID.Hda.S_INSERTED, (long) input);

                case 0x4004100f:
                    return new ResultID(ResultID.Hda.S_REPLACED, (long) input);
            }
            if ((input & 0x7fff0000) == 0x10000)
            {
                return new ResultID(ResultID.E_NETWORK_ERROR, (long) input);
            }
            if (input >= 0)
            {
                return new ResultID(ResultID.S_FALSE, (long) input);
            }
            return new ResultID(ResultID.E_FAIL, (long) input);
        }

        public static string GetSystemMessage(int error)
        {
            IntPtr lpBuffer = Marshal.AllocCoTaskMem(0x400);
            FormatMessageW(0x1000, IntPtr.Zero, error, 0, lpBuffer, 0x3ff, IntPtr.Zero);
            string str = Marshal.PtrToStringUni(lpBuffer);
            Marshal.FreeCoTaskMem(lpBuffer);
            if ((str != null) && (str.Length > 0))
            {
                return str;
            }
            return string.Format("0x{0,0:X}", error);
        }

        internal static System.Type GetType(VarEnum input)
        {
            switch (input)
            {
                case VarEnum.VT_EMPTY:
                    return null;

                case VarEnum.VT_I2:
                    return typeof(short);

                case VarEnum.VT_I4:
                    return typeof(int);

                case VarEnum.VT_R4:
                    return typeof(float);

                case VarEnum.VT_R8:
                    return typeof(double);

                case VarEnum.VT_CY:
                    return typeof(decimal);

                case VarEnum.VT_DATE:
                    return typeof(DateTime);

                case VarEnum.VT_BSTR:
                    return typeof(string);

                case VarEnum.VT_BOOL:
                    return typeof(bool);

                case VarEnum.VT_I1:
                    return typeof(sbyte);

                case VarEnum.VT_UI1:
                    return typeof(byte);

                case VarEnum.VT_UI2:
                    return typeof(ushort);

                case VarEnum.VT_UI4:
                    return typeof(uint);

                case VarEnum.VT_I8:
                    return typeof(long);

                case VarEnum.VT_UI8:
                    return typeof(ulong);

                case (VarEnum.VT_ARRAY | VarEnum.VT_I2):
                    return typeof(short[]);

                case (VarEnum.VT_ARRAY | VarEnum.VT_I4):
                    return typeof(int[]);

                case (VarEnum.VT_ARRAY | VarEnum.VT_R4):
                    return typeof(float[]);

                case (VarEnum.VT_ARRAY | VarEnum.VT_R8):
                    return typeof(double[]);

                case (VarEnum.VT_ARRAY | VarEnum.VT_CY):
                    return typeof(decimal[]);

                case (VarEnum.VT_ARRAY | VarEnum.VT_DATE):
                    return typeof(DateTime[]);

                case (VarEnum.VT_ARRAY | VarEnum.VT_BSTR):
                    return typeof(string[]);

                case (VarEnum.VT_ARRAY | VarEnum.VT_BOOL):
                    return typeof(bool[]);

                case (VarEnum.VT_ARRAY | VarEnum.VT_VARIANT):
                    return typeof(object[]);

                case (VarEnum.VT_ARRAY | VarEnum.VT_I1):
                    return typeof(sbyte[]);

                case (VarEnum.VT_ARRAY | VarEnum.VT_UI1):
                    return typeof(byte[]);

                case (VarEnum.VT_ARRAY | VarEnum.VT_UI2):
                    return typeof(ushort[]);

                case (VarEnum.VT_ARRAY | VarEnum.VT_UI4):
                    return typeof(uint[]);

                case (VarEnum.VT_ARRAY | VarEnum.VT_I8):
                    return typeof(long[]);

                case (VarEnum.VT_ARRAY | VarEnum.VT_UI8):
                    return typeof(ulong[]);
            }
            return Opc.Type.ILLEGAL_TYPE;
        }

        internal static VarEnum GetType(System.Type input)
        {
            if (input != null)
            {
                if (input == typeof(sbyte))
                {
                    return VarEnum.VT_I1;
                }
                if (input == typeof(byte))
                {
                    return VarEnum.VT_UI1;
                }
                if (input == typeof(short))
                {
                    return VarEnum.VT_I2;
                }
                if (input == typeof(ushort))
                {
                    return VarEnum.VT_UI2;
                }
                if (input == typeof(int))
                {
                    return VarEnum.VT_I4;
                }
                if (input == typeof(uint))
                {
                    return VarEnum.VT_UI4;
                }
                if (input == typeof(long))
                {
                    return VarEnum.VT_I8;
                }
                if (input == typeof(ulong))
                {
                    return VarEnum.VT_UI8;
                }
                if (input == typeof(float))
                {
                    return VarEnum.VT_R4;
                }
                if (input == typeof(double))
                {
                    return VarEnum.VT_R8;
                }
                if (input == typeof(decimal))
                {
                    return VarEnum.VT_CY;
                }
                if (input == typeof(bool))
                {
                    return VarEnum.VT_BOOL;
                }
                if (input == typeof(DateTime))
                {
                    return VarEnum.VT_DATE;
                }
                if (input == typeof(string))
                {
                    return VarEnum.VT_BSTR;
                }
                if (input == typeof(object))
                {
                    return VarEnum.VT_EMPTY;
                }
                if (input == typeof(sbyte[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_I1);
                }
                if (input == typeof(byte[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_UI1);
                }
                if (input == typeof(short[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_I2);
                }
                if (input == typeof(ushort[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_UI2);
                }
                if (input == typeof(int[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_I4);
                }
                if (input == typeof(uint[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_UI4);
                }
                if (input == typeof(long[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_I8);
                }
                if (input == typeof(ulong[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_UI8);
                }
                if (input == typeof(float[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_R4);
                }
                if (input == typeof(double[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_R8);
                }
                if (input == typeof(decimal[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_CY);
                }
                if (input == typeof(bool[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_BOOL);
                }
                if (input == typeof(DateTime[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_DATE);
                }
                if (input == typeof(string[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_BSTR);
                }
                if (input == typeof(object[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_VARIANT);
                }
                if (input == Opc.Type.ILLEGAL_TYPE)
                {
                    return (VarEnum) System.Enum.ToObject(typeof(VarEnum), 0x7fff);
                }
                if (input == typeof(System.Type))
                {
                    return VarEnum.VT_I2;
                }
                if (input == typeof(Quality))
                {
                    return VarEnum.VT_I2;
                }
                if (input == typeof(accessRights))
                {
                    return VarEnum.VT_I4;
                }
                if (input == typeof(euType))
                {
                    return VarEnum.VT_I4;
                }
            }
            return VarEnum.VT_EMPTY;
        }

        public static IntPtr GetUnicodeStrings(string[] values)
        {
            int length = (values != null) ? values.Length : 0;
            if (length <= 0)
            {
                return IntPtr.Zero;
            }
            IntPtr zero = IntPtr.Zero;
            int[] source = new int[length];
            for (int i = 0; i < length; i++)
            {
                source[i] = (int) Marshal.StringToCoTaskMemUni(values[i]);
            }
            zero = Marshal.AllocCoTaskMem(values.Length * Marshal.SizeOf(typeof(IntPtr)));
            Marshal.Copy(source, 0, zero, length);
            return zero;
        }

        public static string[] GetUnicodeStrings(ref IntPtr pArray, int size, bool deallocate)
        {
            if ((pArray == IntPtr.Zero) || (size <= 0))
            {
                return null;
            }
            int[] destination = new int[size];
            Marshal.Copy(pArray, destination, 0, size);
            string[] strArray = new string[size];
            for (int i = 0; i < size; i++)
            {
                IntPtr ptr = (IntPtr) destination[i];
                strArray[i] = Marshal.PtrToStringUni(ptr);
                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(ptr);
                }
            }
            if (deallocate)
            {
                Marshal.FreeCoTaskMem(pArray);
                pArray = IntPtr.Zero;
            }
            return strArray;
        }

        public static object GetVARIANT(object source)
        {
            if ((source == null) || (source.GetType() == null))
            {
                return null;
            }
            if (source.GetType() != typeof(decimal[]))
            {
                return source;
            }
            decimal[] numArray = (decimal[]) source;
            object[] objArray = new object[numArray.Length];
            for (int i = 0; i < numArray.Length; i++)
            {
                try
                {
                    objArray[i] = numArray[i];
                }
                catch (Exception)
                {
                    objArray[i] = (double) 1.0 / (double) 0.0;
                }
            }
            return objArray;
        }

        public static IntPtr GetVARIANTs(object[] values, bool preprocess)
        {
            int num = (values != null) ? values.Length : 0;
            if (num <= 0)
            {
                return IntPtr.Zero;
            }
            IntPtr ptr = Marshal.AllocCoTaskMem(num * 0x10);
            IntPtr pDstNativeVariant = ptr;
            for (int i = 0; i < num; i++)
            {
                if (preprocess)
                {
                    Marshal.GetNativeVariantForObject(GetVARIANT(values[i]), pDstNativeVariant);
                }
                else
                {
                    Marshal.GetNativeVariantForObject(values[i], pDstNativeVariant);
                }
                pDstNativeVariant = (IntPtr) (pDstNativeVariant.ToInt32() + 0x10);
            }
            return ptr;
        }

        public static object[] GetVARIANTs(ref IntPtr pArray, int size, bool deallocate)
        {
            if ((pArray == IntPtr.Zero) || (size <= 0))
            {
                return null;
            }
            object[] objArray = new object[size];
            IntPtr pSrcNativeVariant = pArray;
            for (int i = 0; i < size; i++)
            {
                try
                {
                    objArray[i] = Marshal.GetObjectForNativeVariant(pSrcNativeVariant);
                    if (deallocate)
                    {
                        VariantClear(pSrcNativeVariant);
                    }
                }
                catch (Exception)
                {
                    objArray[i] = null;
                }
                pSrcNativeVariant = (IntPtr) (pSrcNativeVariant.ToInt32() + 0x10);
            }
            if (deallocate)
            {
                Marshal.FreeCoTaskMem(pArray);
                pArray = IntPtr.Zero;
            }
            return objArray;
        }

        public static void InitializeSecurity()
        {
            int errorCode = CoInitializeSecurity(IntPtr.Zero, -1, null, IntPtr.Zero, 1, 2, IntPtr.Zero, 0, IntPtr.Zero);
            if (errorCode != 0)
            {
                throw new ExternalException("CoInitializeSecurity: " + GetSystemMessage(errorCode), errorCode);
            }
        }

        [DllImport("Netapi32.dll")]
        private static extern int NetApiBufferFree(IntPtr buffer);
        [DllImport("Netapi32.dll")]
        private static extern int NetServerEnum(IntPtr servername, uint level, out IntPtr bufptr, int prefmaxlen, out int entriesread, out int totalentries, uint servertype, IntPtr domain, IntPtr resume_handle);
        public static void ReleaseServer(object server)
        {
            if ((server != null) && server.GetType().IsCOMObject)
            {
                Marshal.ReleaseComObject(server);
            }
        }

        [DllImport("oleaut32.dll")]
        public static extern void VariantClear(IntPtr pVariant);

        public static bool PreserveUTC
        {
            get
            {
                lock (typeof(Interop))
                {
                    return m_preserveUTC;
                }
            }
            set
            {
                lock (typeof(Interop))
                {
                    m_preserveUTC = value;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        private struct COAUTHIDENTITY
        {
            public IntPtr User;
            public uint UserLength;
            public IntPtr Domain;
            public uint DomainLength;
            public IntPtr Password;
            public uint PasswordLength;
            public uint Flags;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        private struct COAUTHINFO
        {
            public uint dwAuthnSvc;
            public uint dwAuthzSvc;
            public IntPtr pwszServerPrincName;
            public uint dwAuthnLevel;
            public uint dwImpersonationLevel;
            public IntPtr pAuthIdentityData;
            public uint dwCapabilities;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        private struct COSERVERINFO
        {
            public uint dwReserved1;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pwszName;
            public IntPtr pAuthInfo;
            public uint dwReserved2;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        private struct GUID
        {
            public int Data1;
            public short Data2;
            public short Data3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
            public byte[] Data4;
        }

        [ComImport, Guid("B196B28F-BAB4-101A-B69C-00AA00341D07"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IClassFactory2
        {
            void CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object punkOuter, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);
            void LockServer([MarshalAs(UnmanagedType.Bool)] bool fLock);
            void GetLicInfo([In, Out] ref Interop.LICINFO pLicInfo);
            void RequestLicKey(int dwReserved, [MarshalAs(UnmanagedType.BStr)] string pbstrKey);
            void CreateInstanceLic([MarshalAs(UnmanagedType.IUnknown)] object punkOuter, [MarshalAs(UnmanagedType.IUnknown)] object punkReserved, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.BStr)] string bstrKey, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObject);
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000013D-0000-0000-C000-000000000046")]
        private interface IClientSecurity
        {
            void QueryBlanket([MarshalAs(UnmanagedType.IUnknown)] object pProxy, ref uint pAuthnSvc, ref uint pAuthzSvc, [MarshalAs(UnmanagedType.LPWStr)] ref string pServerPrincName, ref uint pAuthnLevel, ref uint pImpLevel, ref IntPtr pAuthInfo, ref uint pCapabilities);
            void SetBlanket([MarshalAs(UnmanagedType.IUnknown)] object pProxy, uint pAuthnSvc, uint pAuthzSvc, [MarshalAs(UnmanagedType.LPWStr)] string pServerPrincName, uint pAuthnLevel, uint pImpLevel, IntPtr pAuthInfo, uint pCapabilities);
            void CopyProxy([MarshalAs(UnmanagedType.IUnknown)] object pProxy, [MarshalAs(UnmanagedType.IUnknown)] out object ppCopy);
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        private struct LICINFO
        {
            public int cbLicInfo;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fRuntimeKeyAvail;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fLicVerified;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        private struct MULTI_QI
        {
            public IntPtr iid;
            [MarshalAs(UnmanagedType.IUnknown)]
            public object pItf;
            public uint hr;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        private struct SERVER_INFO_100
        {
            public uint sv100_platform_id;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string sv100_name;
        }

        private class ServerInfo
        {
            private GCHandle m_hAuthInfo;
            private GCHandle m_hDomain;
            private GCHandle m_hIdentity;
            private GCHandle m_hPassword;
            private GCHandle m_hUserName;

            public Interop.COSERVERINFO Allocate(string hostName, NetworkCredential credential)
            {
                string userName = null;
                string password = null;
                string domain = null;
                if (credential != null)
                {
                    userName = credential.UserName;
                    password = credential.Password;
                    domain = credential.Domain;
                }
                this.m_hUserName = GCHandle.Alloc(userName, GCHandleType.Pinned);
                this.m_hPassword = GCHandle.Alloc(password, GCHandleType.Pinned);
                this.m_hDomain = GCHandle.Alloc(domain, GCHandleType.Pinned);
                this.m_hIdentity = new GCHandle();
                if ((userName != null) && (userName != string.Empty))
                {
                    Interop.COAUTHIDENTITY coauthidentity = new Interop.COAUTHIDENTITY {
                        User = this.m_hUserName.AddrOfPinnedObject(),
                        UserLength = (userName != null) ? ((uint) userName.Length) : 0,
                        Password = this.m_hPassword.AddrOfPinnedObject(),
                        PasswordLength = (password != null) ? ((uint) password.Length) : 0,
                        Domain = this.m_hDomain.AddrOfPinnedObject(),
                        DomainLength = (domain != null) ? ((uint) domain.Length) : 0,
                        Flags = 2
                    };
                    this.m_hIdentity = GCHandle.Alloc(coauthidentity, GCHandleType.Pinned);
                }
                Interop.COAUTHINFO coauthinfo = new Interop.COAUTHINFO {
                    dwAuthnSvc = 10,
                    dwAuthzSvc = 0,
                    pwszServerPrincName = IntPtr.Zero,
                    dwAuthnLevel = 2,
                    dwImpersonationLevel = 3,
                    pAuthIdentityData = this.m_hIdentity.IsAllocated ? this.m_hIdentity.AddrOfPinnedObject() : IntPtr.Zero,
                    dwCapabilities = 0
                };
                this.m_hAuthInfo = GCHandle.Alloc(coauthinfo, GCHandleType.Pinned);
                return new Interop.COSERVERINFO { pwszName = hostName, pAuthInfo = (credential != null) ? this.m_hAuthInfo.AddrOfPinnedObject() : IntPtr.Zero, dwReserved1 = 0, dwReserved2 = 0 };
            }

            public void Deallocate()
            {
                if (this.m_hUserName.IsAllocated)
                {
                    this.m_hUserName.Free();
                }
                if (this.m_hPassword.IsAllocated)
                {
                    this.m_hPassword.Free();
                }
                if (this.m_hDomain.IsAllocated)
                {
                    this.m_hDomain.Free();
                }
                if (this.m_hIdentity.IsAllocated)
                {
                    this.m_hIdentity.Free();
                }
                if (this.m_hAuthInfo.IsAllocated)
                {
                    this.m_hAuthInfo.Free();
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        private struct SOLE_AUTHENTICATION_SERVICE
        {
            public uint dwAuthnSvc;
            public uint dwAuthzSvc;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pPrincipalName;
            public int hr;
        }
    }
}

