using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.NETHelper.WindowsSystemHelper
{
    /// <summary>
    /// Registry operation method class library
    /// </summary>
    public class RegistryHelper
    {
        public enum RegistryRootEnum
        {
            HKEY_CLASSES_ROOT,
            HKEY_CURRENT_USER,
            HKEY_LOCAL_MACHINE,
            HKEY_USERS,
            HKEY_CURRENT_CONFIG,
            HKEY_DYN_DATA,
            HKEY_PERFORMENCE_DATA
        }

        // Methods
        /// <summary>
        /// Create Registry Folder
        /// </summary>
        /// <param name="Root">Root Folder</param>
        /// <param name="Path">Path</param>
        /// <param name="Folder">Folder name</param>
        /// <returns></returns>
        public static bool CreateRegistryFolder(RegistryRootEnum Root, string Path, string Folder)
        {
            using (RegistryKey rootKey = GetRootKey(Root))
            {
                if (((rootKey == null) || !(Path != string.Empty)) || !(Folder != string.Empty))
                {
                    return false;
                }
                try
                {
                    rootKey.OpenSubKey(Path, true).CreateSubKey(Folder).Close();
                    return true;
                }
                catch(System.Security.SecurityException ex)
                {
                    throw ex;
                }
                catch (UnauthorizedAccessException ex)
                {
                    throw ex;
                }
            }
        }

        public static bool DeleteRegFolder(RegistryRootEnum Root, string Path, string Folder)
        {
            bool flag = false;
            RegistryKey rootKey = GetRootKey(Root);
            if (((rootKey == null) || !(Path != string.Empty)) || !(Folder != string.Empty))
            {
                return flag;
            }
            try
            {
                rootKey = rootKey.OpenSubKey(Path, true);
                rootKey.DeleteSubKey(Folder, false);
                rootKey.Close();
                rootKey = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DeleteSetting(RegistryRootEnum Root, string Path, string Key)
        {
            bool flag = false;
            RegistryKey rootKey = GetRootKey(Root);
            if (((rootKey == null) || !(Path != string.Empty)) || !(Key != string.Empty))
            {
                return flag;
            }
            try
            {
                rootKey = rootKey.OpenSubKey(Path, true);
                rootKey.DeleteValue(Key, false);
                rootKey.Close();
                rootKey = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool FolderExists(RegistryRootEnum Root, string Path, string Folder)
        {
            bool flag = false;
            RegistryKey rootKey = GetRootKey(Root);
            if (((rootKey == null) || !(Path != string.Empty)) || !(Folder != string.Empty))
            {
                return flag;
            }
            try
            {
                Path = (Path.Substring(Path.Length - 1) == @"\") ? Path : (Path + @"\");
                rootKey.OpenSubKey(Path + Folder, false).Close();
                rootKey = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static int GetRegFolderCount(RegistryRootEnum Root, string Path)
        {
            int subKeyCount = 0;
            RegistryKey rootKey = GetRootKey(Root);
            if ((rootKey != null) && (Path != string.Empty))
            {
                try
                {
                    rootKey = rootKey.OpenSubKey(Path, false);
                    subKeyCount = rootKey.SubKeyCount;
                    rootKey.Close();
                    rootKey = null;
                }
                catch
                {
                    subKeyCount = 0;
                }
            }
            return subKeyCount;
        }

        public static string[] GetRegFolderNames(RegistryRootEnum Root, string Path)
        {
            string[] subKeyNames = null;
            RegistryKey rootKey = GetRootKey(Root);
            if ((rootKey != null) && (Path != string.Empty))
            {
                try
                {
                    rootKey = rootKey.OpenSubKey(Path, false);
                    subKeyNames = rootKey.GetSubKeyNames();
                    rootKey.Close();
                    rootKey = null;
                }
                catch
                {
                    subKeyNames = null;
                }
            }
            return subKeyNames;
        }

        public static RegistryKey GetRootKey(RegistryRootEnum Root)
        {
            switch (Root)
            {
                case RegistryRootEnum.HKEY_CLASSES_ROOT:
                    return Registry.ClassesRoot;

                case RegistryRootEnum.HKEY_CURRENT_USER:
                    return Registry.CurrentUser;

                case RegistryRootEnum.HKEY_LOCAL_MACHINE:
                    return Registry.LocalMachine;

                case RegistryRootEnum.HKEY_USERS:
                    return Registry.Users;

                case RegistryRootEnum.HKEY_CURRENT_CONFIG:
                    return Registry.CurrentConfig;

                case RegistryRootEnum.HKEY_DYN_DATA:
                    return Registry.DynData;

                case RegistryRootEnum.HKEY_PERFORMENCE_DATA:
                    return Registry.PerformanceData;
            }
            return null;
        }

        public static RegistryRootEnum GetRootKey(string Root)
        {
            RegistryRootEnum enum2 = RegistryRootEnum.HKEY_LOCAL_MACHINE;
            switch (Root.ToUpper())
            {
                case "HKEY_LOCAL_MATCHINE":
                    return RegistryRootEnum.HKEY_LOCAL_MACHINE;

                case "HKEY_CURRENT_USER":
                    return RegistryRootEnum.HKEY_CURRENT_USER;

                case "HKEY_CURRENT_CONFIG":
                    return RegistryRootEnum.HKEY_CURRENT_CONFIG;

                case "HKEY_CLASSES_ROOT":
                    return RegistryRootEnum.HKEY_CLASSES_ROOT;

                case "HKEY_USERS":
                    return RegistryRootEnum.HKEY_USERS;

                case "HKEY_DYN_DATA":
                    return RegistryRootEnum.HKEY_DYN_DATA;

                case "HKEY_PERFORMENCE_DATA":
                    return RegistryRootEnum.HKEY_PERFORMENCE_DATA;
            }
            return enum2;
        }

        public static string GetSetting(RegistryRootEnum Root, string Path, string Key, string Default)
        {
            string str = Default;
            RegistryKey rootKey = GetRootKey(Root);
            if ((rootKey != null) && (Path != string.Empty))
            {
                try
                {
                    rootKey = rootKey.OpenSubKey(Path, true);
                    str = rootKey.GetValue(Key, Default).ToString();
                    rootKey.Close();
                    rootKey = null;
                }
                catch
                {
                    str = Default;
                }
            }
            return str;
        }

        public static bool GetSetting(RegistryRootEnum Root, string Path, string Key, ref string Value, string Default)
        {
            bool flag = false;
            RegistryKey rootKey = GetRootKey(Root);
            if ((rootKey == null) || !(Path != string.Empty))
            {
                return flag;
            }
            try
            {
                rootKey = rootKey.OpenSubKey(Path, true);
                Value = rootKey.GetValue(Key, Default).ToString();
                rootKey.Close();
                rootKey = null;
                return true;
            }
            catch
            {
                Value = Default;
                return false;
            }
        }

        public static int GetSettingCount(RegistryRootEnum Root, string Path)
        {
            int valueCount = 0;
            RegistryKey rootKey = GetRootKey(Root);
            if ((rootKey != null) && (Path != string.Empty))
            {
                try
                {
                    rootKey = rootKey.OpenSubKey(Path, false);
                    valueCount = rootKey.ValueCount;
                    rootKey.Close();
                    rootKey = null;
                }
                catch
                {
                    valueCount = 0;
                }
            }
            return valueCount;
        }

        public static string[] GetSettingNames(RegistryRootEnum Root, string Path)
        {
            string[] valueNames = null;
            RegistryKey rootKey = GetRootKey(Root);
            if ((rootKey != null) && (Path != string.Empty))
            {
                try
                {
                    rootKey = rootKey.OpenSubKey(Path, false);
                    valueNames = rootKey.GetValueNames();
                    rootKey.Close();
                    rootKey = null;
                }
                catch
                {
                    valueNames = null;
                }
            }
            return valueNames;
        }

        public static bool SetSetting(RegistryRootEnum Root, string Path, string Key, string Value)
        {
            bool flag = false;
            RegistryKey rootKey = GetRootKey(Root);
            if ((rootKey == null) || !(Path != string.Empty))
            {
                return flag;
            }
            try
            {
                rootKey = rootKey.OpenSubKey(Path, true);
                rootKey.SetValue(Key, Value);
                rootKey.Close();
                rootKey = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SettingExists(RegistryRootEnum Root, string Path, string Key)
        {
            bool flag = false;
            RegistryKey rootKey = GetRootKey(Root);
            if ((rootKey != null) && (Path != string.Empty))
            {
                try
                {
                    rootKey = rootKey.OpenSubKey(Path, true);
                    foreach (string str in rootKey.GetValueNames())
                    {
                        if (str == Key)
                        {
                            flag = true;
                            break;
                        }
                    }
                    rootKey.Close();
                    rootKey = null;
                }
                catch
                {
                    flag = false;
                }
            }
            return flag;
        }
    }
}
