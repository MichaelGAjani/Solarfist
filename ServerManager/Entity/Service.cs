using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Reflection;
using System.ServiceProcess;

namespace ServerManager.Entity
{
    public class Service
    {
        private const int actionTimeout = 30;//服务执行超时时间
        private ServiceController controller;
        #region 服务所在注册表
        private const string Registry_ServiceAccountValueName = "ObjectName";
        private const string Registry_ServiceDescriptionValueName = "Description";
        private const string Registry_ServicePathValueName = "ImagePath";
        private const string Registry_ServicesKeyName = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\";
        private const string Registry_ServiceStartTypeValueName = "Start";
        #endregion

        public Service(ServiceController controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }
            this.controller = controller;
        }

        public Service(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            this.controller = new ServiceController(name);
        }

        /// <summary>
        /// 服务暂停后重启
        /// </summary>
        public void Continue()
        {
            if (!this.controller.CanPauseAndContinue)
            {
                throw new InvalidOperationException();
            }
            if (this.controller.Status == ServiceControllerStatus.Paused)
            {
                this.controller.Continue();
                this.controller.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30.0));
            }
        }

        /// <summary>
        /// 执行服务指令
        /// </summary>
        /// <param name="command"></param>
        public void ExecuteCommand(int command)
        {
            if (this.controller.Status == ServiceControllerStatus.Running)
            {
                this.controller.ExecuteCommand(command);
            }
        }

        /// <summary>
        /// 判断服务是否存在
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool Exists(string serviceName)
        {
            foreach (ServiceController controller in ServiceController.GetServices())
            {
                if (string.Compare(controller.ServiceName, serviceName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取服务登录类型
        /// </summary>
        /// <param name="account">服务登录账号</param>
        /// <returns></returns>
        private string GetAccountText(ServiceAccount account)
        {
            switch (account)
            {
                case ServiceAccount.LocalService:
                    return "本地服务";

                case ServiceAccount.NetworkService:
                    return "网络服务";

                case ServiceAccount.LocalSystem:
                    return "本地系统";

                case ServiceAccount.User:
                    throw new NotImplementedException();
            }
            throw new NotSupportedException();
        }

        private static InstallContext GetInstallContext()
        {
            InstallContext context = new InstallContext();
            context.Parameters.Add("LogToConsole", string.Empty);
            return context;
        }

        /// <summary>
        /// 获取服务安装类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static Type[] GetInstallerTypes(Assembly assembly)
        {
            ArrayList list = new ArrayList();
            Module[] modules = assembly.GetModules();
            for (int i = 0; i < modules.Length; i++)
            {
                Type[] types = modules[i].GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    if ((typeof(Installer).IsAssignableFrom(types[j]) && !types[j].IsAbstract) && (types[j].IsPublic && ((RunInstallerAttribute)TypeDescriptor.GetAttributes(types[j])[typeof(RunInstallerAttribute)]).RunInstaller))
                    {
                        list.Add(types[j]);
                    }
                }
            }
            return (Type[])list.ToArray(typeof(Type));
        }

        /// <summary>
        /// 获取服务名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetServiceNames(string path)
        {
            Type[] installerTypes = GetInstallerTypes(Assembly.LoadFrom(path));
            List<string> list = new List<string>();
            foreach (Type type in installerTypes)
            {
                using (Installer installer = (Installer)Activator.CreateInstance(type, BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance, null, new object[0], null))
                {
                    foreach (Installer installer2 in installer.Installers)
                    {
                        if (installer2 is ServiceInstaller)
                        {
                            list.Add(((ServiceInstaller)installer2).ServiceName);
                        }
                    }
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 获取服务启动方式
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetStartTypeText(ServiceStartMode type)
        {
            switch (type)
            {
                case ServiceStartMode.Automatic:
                    return "自动";

                case ServiceStartMode.Manual:
                    return "手动";

                case ServiceStartMode.Disabled:
                    return "禁用";
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// 获取服务状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private string GetStatusText(ServiceControllerStatus status)
        {
            switch (status)
            {
                case ServiceControllerStatus.Stopped:
                    return "未运行";

                case ServiceControllerStatus.StartPending:
                    return "正在启动";

                case ServiceControllerStatus.StopPending:
                    return "正在停止";

                case ServiceControllerStatus.Running:
                    return "正在运行";

                case ServiceControllerStatus.ContinuePending:
                    return "即将继续";

                case ServiceControllerStatus.PausePending:
                    return "即将暂停";

                case ServiceControllerStatus.Paused:
                    return "已暂停";
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// 安装服务
        /// </summary>
        /// <param name="path"></param>
        public static void Install(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = System.IO.Path.GetFullPath(path);
                IDictionary stateSaver = new Hashtable();
                installer.Install(stateSaver);
                installer.Commit(stateSaver);
            }
        }

        /// <summary>
        /// 暂停服务
        /// </summary>
        public void Pause()
        {
            if (!this.controller.CanPauseAndContinue)
            {
                throw new InvalidOperationException();
            }
            if (this.controller.Status == ServiceControllerStatus.Running)
            {
                this.controller.Pause();
                this.controller.WaitForStatus(ServiceControllerStatus.Paused, TimeSpan.FromSeconds(30.0));
            }
        }

        /// <summary>
        /// 重启服务
        /// </summary>
        public void Restart()
        {
            this.Stop();
            this.Start();
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start()
        {
            if (this.controller.Status == ServiceControllerStatus.Stopped)
            {
                this.controller.Start();
                this.controller.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30.0));
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            if (!this.controller.CanStop)
            {
                throw new InvalidOperationException();
            }
            if (this.controller.Status == ServiceControllerStatus.Running)
            {
                this.controller.Stop();
                this.controller.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30.0));
            }
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="path"></param>
        public static void Uninstall(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = System.IO.Path.GetFullPath(path);
                installer.Uninstall(null);
            }
        }

        #region 服务属性
        /// <summary>
        /// 登录用户名
        /// </summary>
        public string Account
        {
            get
            {
                return Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\" + this.Name, "ObjectName", string.Empty));
            }
            set
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\" + this.Name, "ObjectName", value, RegistryValueKind.String);
            }
        }

        /// <summary>
        /// 可以继续
        /// </summary>
        public bool CanContinue
        {
            get
            {
                return (this.controller.CanPauseAndContinue && (this.controller.Status == ServiceControllerStatus.Paused));
            }
        }

        /// <summary>
        /// 可以发送指令
        /// </summary>
        public bool CanExecuteCommand
        {
            get
            {
                return (this.controller.Status == ServiceControllerStatus.Running);
            }
        }

        /// <summary>
        /// 可以暂停
        /// </summary>
        public bool CanPause
        {
            get
            {
                return (this.controller.CanPauseAndContinue && (this.controller.Status == ServiceControllerStatus.Running));
            }
        }

        /// <summary>
        /// 可以重启
        /// </summary>
        public bool CanRestart
        {
            get
            {
                return (this.controller.CanStop && (this.controller.Status == ServiceControllerStatus.Running));
            }
        }

        /// <summary>
        /// 可以开始
        /// </summary>
        public bool CanStart
        {
            get
            {
                return (this.controller.Status == ServiceControllerStatus.Stopped);
            }
        }

        /// <summary>
        /// 可以停止
        /// </summary>
        public bool CanStop
        {
            get
            {
                return (this.controller.CanStop && (this.controller.Status == ServiceControllerStatus.Running));
            }
        }

        /// <summary>
        /// 完全路径
        /// </summary>
        public string FullPath
        {
            get
            {
                return Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\" + this.Name, "Description", string.Empty));
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get
            {
                return Convert.ToString(Registry.GetValue(@"HKEY_USERS\S-1-5-21-2580061664-1423693276-1067424087-1001\Software\Classes\Local Settings\MuiCache\c6\AAF68885\" , this.FullPath, string.Empty));
            }
        }

        /// <summary>
        /// 显示的名称
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this.controller.DisplayName;
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return this.controller.ServiceName;
            }
        }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path
        {
            get
            {
                string str = Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\" + this.Name, "ImagePath", string.Empty));
                if (str.StartsWith("\""))
                {
                    str = str.Substring(1);
                }
                if (str.EndsWith("\""))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                return str;
            }
        }

        /// <summary>
        /// 启动类型
        /// </summary>
        public ServiceStartMode StartType
        {
            get
            {
                object obj2 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\" + this.Name, "Start", null);
                if (obj2 == null)
                {
                    throw new NotSupportedException();
                }
                return (ServiceStartMode)((int)obj2);
            }
            set
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\" + this.Name, "Start", (int)value, RegistryValueKind.DWord);
            }
        }

        /// <summary>
        /// 启动类型描述
        /// </summary>
        public string StartTypeText
        {
            get
            {
                return this.GetStartTypeText(this.StartType);
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public ServiceControllerStatus Status
        {
            get
            {
                return this.controller.Status;
            }
        }

        /// <summary>
        /// 状态描述
        /// </summary>
        public string StatusText
        {
            get
            {
                return this.GetStatusText(this.controller.Status);
            }
        }
        #endregion
    }
}
