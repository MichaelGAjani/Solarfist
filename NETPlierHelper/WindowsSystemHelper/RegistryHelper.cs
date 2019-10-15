// FileInfo
// File:"RegistryHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Registry
// 1.CreateRegistryFolder(RegistryRootEnum root, string path, string folder)
// 2.DeleteRegistryFolder(RegistryRootEnum root, string path, string folder)
// 3.DeleteKeyValue(RegistryRootEnum root, string path, string key)
// 4.GetRegistryFolderCount(RegistryRootEnum root, string path)
// 5.GetRegistryFolderNames(RegistryRootEnum root, string path)
// 6.GetRootKey(RegistryRootEnum Root)
// 7.GetRegistryKeyValue(RegistryRootEnum root, string path, string key)
// 8.GetRegistryKeyValueCount(RegistryRootEnum root, string path)
// 9.GetSettingNames(RegistryRootEnum root, string path)
// 10.SetRegistryValue(RegistryRootEnum Root, string Path, string Key, string Value)
// 11.KeyExist(RegistryRootEnum root, string path, string folder, string key)
// 12.KeyClose(RegistryKey rootKey)
//
// File Lines:228
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static void CreateRegistryFolder(RegistryRootEnum root, string path, string folder)
        {
            RegistryKey rootKey = GetRootKey(root);
            if (KeyExist(root, path, folder, String.Empty))
            {
                try
                {
                    rootKey = rootKey.OpenSubKey(path, true);
                    rootKey.CreateSubKey(folder).Close();
                    KeyClose(rootKey);
                }
                catch (System.Security.SecurityException ex)
                {
                    throw ex;
                }
                catch (UnauthorizedAccessException ex)
                {
                    throw ex;
                }
            }
        }
        public static void DeleteRegistryFolder(RegistryRootEnum root, string path, string folder)
        {
            RegistryKey rootKey = GetRootKey(root);
            if (KeyExist(root, path, folder, String.Empty))
            {
                try
                {
                    rootKey = rootKey.OpenSubKey(path, true);
                    rootKey.DeleteSubKey(folder, false);
                    KeyClose(rootKey);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public static void DeleteKeyValue(RegistryRootEnum root, string path, string key)
        {
            RegistryKey rootKey = GetRootKey(root);
            if (KeyExist(root, path, String.Empty, key))
            {
                try
                {
                    rootKey = rootKey.OpenSubKey(path, true);
                    rootKey.DeleteValue(key, false);
                    KeyClose(rootKey);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public static int GetRegistryFolderCount(RegistryRootEnum root, string path)
        {
            int subKeyCount = 0;
            RegistryKey rootKey = GetRootKey(root);
            if (KeyExist(root, path, String.Empty, String.Empty))
            {
                rootKey = rootKey.OpenSubKey(path, false);
                subKeyCount = rootKey.SubKeyCount;
                KeyClose(rootKey);
            }
            return subKeyCount;
        }
        public static List<string> GetRegistryFolderNames(RegistryRootEnum root, string path)
        {
            string[] subKeyNames = null;
            RegistryKey rootKey = GetRootKey(root);
            if (KeyExist(root, path, String.Empty, String.Empty))
            {
                rootKey = rootKey.OpenSubKey(path, false);
                subKeyNames = rootKey.GetSubKeyNames();
                KeyClose(rootKey);
            }
            return subKeyNames.ToList();
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
        public static string GetRegistryKeyValue(RegistryRootEnum root, string path, string key)
        {
            string value = String.Empty;
            RegistryKey rootKey = GetRootKey(root);
            if (KeyExist(root, path, String.Empty, key))
            {
                rootKey = rootKey.OpenSubKey(path, true);
                value = rootKey.GetValue(key, String.Empty).ToString();
                KeyClose(rootKey);
            }
            return value;
        }
        public static int GetRegistryKeyValueCount(RegistryRootEnum root, string path)
        {
            int valueCount = 0;
            RegistryKey rootKey = GetRootKey(root);
            if (KeyExist(root, path, String.Empty, String.Empty))
            {
                rootKey = rootKey.OpenSubKey(path, false);
                valueCount = rootKey.ValueCount;
                KeyClose(rootKey);
            }
            return valueCount;
        }
        public static List<string> GetSettingNames(RegistryRootEnum root, string path)
        {
            string[] valueNames = null;
            RegistryKey rootKey = GetRootKey(root);
            if (KeyExist(root, path, String.Empty, String.Empty))
            {
                rootKey = rootKey.OpenSubKey(path, true);
                valueNames = rootKey.GetValueNames();
                KeyClose(rootKey);
            }
            return valueNames.ToList();
        }
        public static void SetRegistryValue(RegistryRootEnum Root, string Path, string Key, string Value)
        {
            RegistryKey rootKey = GetRootKey(Root);
            if ((rootKey == null) || (Path == string.Empty))
            {
                return;
            }
            rootKey = rootKey.OpenSubKey(Path, true);
            rootKey.SetValue(Key, Value);
            rootKey.Close();
            rootKey = null;
        }
        public static bool KeyExist(RegistryRootEnum root, string path, string folder, string key)
        {
            bool flag = false;
            RegistryKey rootKey = GetRootKey(root);
            if (rootKey == null)
                throw new Exception("Registry is not exist!");
            if ((path == string.Empty) && (folder == string.Empty))
                throw new Exception("Path or folder is not exist!");

            if (key != String.Empty)
            {
                rootKey = rootKey.OpenSubKey(path, true);
                if (rootKey == null) return false;
                flag = rootKey.GetValueNames().ToList().Exists(obj => obj == key);

                rootKey.Close();
                rootKey = null;
                return flag;
            }
            return true;
        }
        private static void KeyClose(RegistryKey rootKey) => rootKey.Close();
    }
}
