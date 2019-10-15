using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jund.NETHelper.WindowsSystemHelper
{
    class PerfomanceInfo_CPU
    {
        string nameCore;
        float totalCore;
        float kernelCore;
        float userCore;

        public string Name { get { return nameCore; } }
        public float Total { get { return totalCore; } }
        public float Kernel { get { return kernelCore; } }
        public float User { get { return userCore; } }

        public PerfomanceInfo_CPU(string name, float total, float kernel, float user)
        {
            nameCore = name;
            totalCore = total;
            kernelCore = kernel;
            userCore = user;
        }
    }
    class PerfomanceInfo_OS
    {
        string nameCore;
        int processesCore;
        int threadsCore;

        public string Name { get { return nameCore; } }
        public int Processes { get { return processesCore; } }
        public int Threads { get { return threadsCore; } }

        public PerfomanceInfo_OS(string name, int processes, int threads)
        {
            nameCore = name;
            processesCore = processes;
            threadsCore = threads;
        }
    }
    class MemoryPerfomanceInfo
    {
        string nameCore;
        int totalCore;
        int freeCore;

        public string Name { get { return nameCore; } }
        public int Total { get { return totalCore; } }
        public int Free { get { return freeCore; } }
        public MemoryPerfomanceInfo(string name, int total, int free)
        {
            nameCore = name;
            totalCore = total;
            freeCore = free;
        }
    }
    public sealed class WMIService : IDisposable
    {       
        public static WMIService GetInstance(string path)
        {
            return new WMIService(string.IsNullOrEmpty(path) ? "//./root/cimv2" : path);
        }

        bool connectedCore = false;
        ManagementScope scopeCore;
        Dictionary<string, ManagementObjectCollection> queryCacheCore;

        Dictionary<string, ManagementObjectCollection> QueryCache { get { return queryCacheCore; } }
        public bool Connected { get { return connectedCore; } }
        public ManagementScope Scope { get { return scopeCore; } }

        WMIService(string path)
        {
            queryCacheCore = new Dictionary<string, ManagementObjectCollection>();
            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = ImpersonationLevel.Impersonate;
            options.Authentication = AuthenticationLevel.Packet;
            this.scopeCore = new ManagementScope(path, options);
            try
            {
                Scope.Connect();
                connectedCore = Scope.IsConnected;
            }
            catch { connectedCore = false; }
        }
        ManagementObjectCollection GetManagementObjectCollection(string queryString)
        {
            ManagementObjectCollection result = null;
            ObjectQuery query = new ObjectQuery(queryString);
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query))
            {
                result = searcher.Get();
            }
            return result;
        }
        public void Dispose()
        {
            connectedCore = false;
            if (queryCacheCore != null)
            {
                foreach (KeyValuePair<string, ManagementObjectCollection> pair in queryCacheCore)
                {
                    if (pair.Value != null) pair.Value.Dispose();
                }
                queryCacheCore.Clear();
                queryCacheCore = null;
            }
            scopeCore = null;
        }
        public ManagementObjectCollection GetObjectCollection(string queryString, bool allowQueryCaching)
        {
            ManagementObjectCollection result = null;
            if (allowQueryCaching) QueryCache.TryGetValue(queryString, out result);
            if (result == null)
            {
                result = GetManagementObjectCollection(queryString);
                if (allowQueryCaching)
                {
                    if (QueryCache.ContainsKey(queryString)) QueryCache[queryString] = result;
                    else QueryCache.Add(queryString, result);
                }
            }
            return result;
        }
        public ManagementObject[] GetObjects(string queryString, bool allowQueryCaching)
        {
            ManagementObject[] result = new ManagementObject[0];
            ManagementObjectCollection collection = GetObjectCollection(queryString, allowQueryCaching);
            int count = collection.Count;
            if (collection != null && count > 0)
            {
                result = new ManagementObject[count];
                collection.CopyTo(result, 0);
            }
            return result;
        }
    }
}
