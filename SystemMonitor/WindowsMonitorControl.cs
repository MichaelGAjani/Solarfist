using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Management;
using DevExpress.Utils;
using DevExpress.XtraGauges.Win.Gauges.Circular;
using System.Threading;

namespace SystemMonitor
{
    public partial class WindowsMonitorControl : DevExpress.XtraEditors.XtraUserControl
    {
        public WindowsMonitorControl()
        {
            //InitializeComponent();

            this.Dock = DockStyle.Fill;
            buffer = new PerfomanceInfo_CPU[3];
            using (WaitDialogForm dlg = new WaitDialogForm("Please Wait", "Connecting WMI Service ..."))
            {
                wmiService = WMIService.GetInstance(null);
                InitializeComponent();
                updateSpeed.Value = 6;
                if (wmiService.Connected)
                {
                    string[] processors = GetProcessorNames(wmiService);
                    DashboardGauge.Labels["processorName"].Text = processors[0];
                    DashboardGauge.Labels["osName"].Text = GetOSName(wmiService);
                    DashboardGauge.Scales["memoryTotal"].MaxValue = GetTotalMemorySizeMB(wmiService);
                    DashboardGauge.Scales["hddTotal"].MaxValue = GetTotalHDDSizeGB(wmiService);
                    this.pollingTimer = new System.Threading.Timer(OnTimerCallback, null, 1000, 300);
                    OnTimerCallback(null);
                }
            }
        }

        static int stateCounter = 0;
        static int lockTimerCounter = 0;

        static int GetTotalMemorySizeMB(WMIService wmiService)
        {
            ManagementObject[] collection = wmiService.GetObjects("Select TotalVisibleMemorySize From Win32_OperatingSystem", true);
            return (collection.Length == 1) ? (int)((UInt64)collection[0].Properties["TotalVisibleMemorySize"].Value / 1024) : 4096;
        }
        static int GetFreeMemorySizeMB(WMIService wmiService)
        {
            ManagementObject[] collection = wmiService.GetObjects("Select FreePhysicalMemory From Win32_OperatingSystem", false);
            return (collection.Length == 1) ? (int)((UInt64)collection[0].Properties["FreePhysicalMemory"].Value / 1024) : 4096;
        }
        static int GetTotalHDDSizeGB(WMIService wmiService)
        {
            ManagementObject[] collection = wmiService.GetObjects("Select Size From Win32_LogicalDisk ", true);
            UInt64 _size = 0;
            for (int i = 0; i < collection.Length; i++)
            {
                PropertyData pData = collection[i].Properties["Size"];
                _size += ((pData != null && pData.Value != null) ? (UInt64)pData.Value : 0u);
            }
            return (int)(_size >> 30);
        }
        static int GetFreeHDDSizeGB(WMIService wmiService)
        {
            ManagementObject[] collection = wmiService.GetObjects("Select FreeSpace From Win32_LogicalDisk ", false);
            UInt64 _size = 0;
            for (int i = 0; i < collection.Length; i++)
            {
                PropertyData pData = collection[i].Properties["FreeSpace"];
                _size += ((pData != null && pData.Value != null) ? (UInt64)pData.Value : 0u);
            }
            return (int)(_size >> 30);
        }
        static string GetOSName(WMIService wmiService)
        {
            ManagementObject[] collection = wmiService.GetObjects("Select Caption From Win32_OperatingSystem", true);
            return (collection.Length == 1) ? (string)collection[0].Properties["Caption"].Value : string.Empty;
        }
        static string[] GetProcessorNames(WMIService wmiService)
        {
            ManagementObject[] collection = wmiService.GetObjects("Select Name From Win32_Processor", true);
            string[] result = new string[collection.Length];
            for (int i = 0; i < collection.Length; i++)
            {
                result[i] = (string)collection[i].Properties["Name"].Value;
            }
            return result;
        }
        static PerfomanceInfo_CPU[] GetPerfomanceInfo_CPU(WMIService wmiService)
        {
            PerfomanceInfo_CPU[] result = new PerfomanceInfo_CPU[0];
            try
            {
                ManagementObject[] collection = wmiService.GetObjects(
                        "SELECT Name,PercentProcessorTime,PercentPrivilegedTime,PercentUserTime " +
                        "FROM Win32_PerfFormattedData_PerfOS_Processor " +
                        "WHERE Name=\'_Total\'",
                        false
                    );
                result = new PerfomanceInfo_CPU[collection.Length];
                for (int i = 0; i < collection.Length; i++)
                {
                    result[i] = new PerfomanceInfo_CPU(
                            (string)collection[i].Properties["Name"].Value,
                            (float)(UInt64)collection[i].Properties["PercentProcessorTime"].Value,
                            (float)(UInt64)collection[i].Properties["PercentPrivilegedTime"].Value,
                            (float)(UInt64)collection[i].Properties["PercentUserTime"].Value
                        );
                }
            }
            catch { }
            return result;
        }
        static PerfomanceInfo_OS[] GetPerfomanceInfo_OS(WMIService wmiService)
        {
            PerfomanceInfo_OS[] result = new PerfomanceInfo_OS[0];
            try
            {
                ManagementObject[] collection = wmiService.GetObjects(
                        "SELECT Name,Processes,Threads " +
                        "FROM Win32_PerfFormattedData_PerfOS_System",
                        false
                    );
                result = new PerfomanceInfo_OS[collection.Length];
                for (int i = 0; i < collection.Length; i++)
                {
                    result[i] = new PerfomanceInfo_OS(
                            (string)collection[i].Properties["Name"].Value,
                            (int)(UInt32)collection[i].Properties["Processes"].Value,
                            (int)(UInt32)collection[i].Properties["Threads"].Value
                        );
                }
            }
            catch { }
            return result;
        }

        PerfomanceInfo_CPU[] buffer;
        WMIService wmiService;
        System.Threading.Timer pollingTimer;

        void OnTimerCallback(object state)
        {
            if (Interlocked.CompareExchange(ref lockTimerCounter, 1, 0) == 0)
            {
                UpdateData();
                stateCounter++;
                Interlocked.Add(ref lockTimerCounter, -1);
            }
        }
        void InvokeUpdate(Action action)
        {
            if (IsHandleCreated && DashboardGauge != null)
                BeginInvoke(action);
        }
        void UpdateData()
        {
            if (wmiService != null)
            {
                UpdatePerfomanceData_CPU();
                if (stateCounter % 5 == 0) UpdatePerfomanceData_OS();
                if (stateCounter % 10 == 0) UpdatePerfomanceData_Memory();
                if (stateCounter % 20 == 0) UpdatePerfomanceData_HDD();
            }
        }
        void UpdatePerfomanceData_CPU()
        {
            PerfomanceInfo_CPU[] infos = GetPerfomanceInfo_CPU(wmiService);
            if (infos.Length == 1)
            {
                PerfomanceInfo_CPU info = GetBufferedPerfomanceInfo(infos[0]);
                InvokeUpdate(() =>
                {
                    DashboardGauge.Scales["cpuTotal"].Value = info.Total;
                    DashboardGauge.Scales["cpuUser"].Value = info.Kernel + info.User;
                    DashboardGauge.Scales["cpuKernel"].Value = info.Kernel;
                });
            }
        }
        void UpdatePerfomanceData_OS()
        {
            PerfomanceInfo_OS[] infos = GetPerfomanceInfo_OS(wmiService);
            if (infos.Length == 1)
            {
                InvokeUpdate(() =>
                {
                    DashboardGauge.Scales["osThreads"].Value = infos[0].Threads;
                    DashboardGauge.Scales["osProcesses"].Value = infos[0].Processes;
                });
            }
        }
        void UpdatePerfomanceData_Memory()
        {
            InvokeUpdate(() =>
            {
                DashboardGauge.Scales["memoryTotal"].Value = GetFreeMemorySizeMB(wmiService);
            });
        }
        void UpdatePerfomanceData_HDD()
        {
            InvokeUpdate(() =>
            {
                DashboardGauge.Scales["hddTotal"].Value = GetFreeHDDSizeGB(wmiService);
            });
        }
        void OnUpdateSpeedChanged(object sender, EventArgs e)
        {
            int speed = 50 + (updateSpeed.Properties.Maximum - updateSpeed.Value) * 50;
            if (pollingTimer != null) pollingTimer.Change(0, speed);
        }
        PerfomanceInfo_CPU GetBufferedPerfomanceInfo(PerfomanceInfo_CPU currentValue)
        {
            for (int i = 1; i < buffer.Length; i++) buffer[i - 1] = buffer[i];
            buffer[buffer.Length - 1] = currentValue;

            float total = 0; float kernel = 0; float user = 0;
            int n = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != null)
                {
                    total += buffer[i].Total;
                    kernel += buffer[i].Kernel;
                    user += buffer[i].User;
                    n++;
                }
            }
            return new PerfomanceInfo_CPU(currentValue.Name, total / (float)n, kernel / (float)n, user / (float)n);
        }
        protected CircularGauge DashboardGauge
        {
            get { return gaugeControl1.Gauges[0] as CircularGauge; }
        }
       
    }
}
