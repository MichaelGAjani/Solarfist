// FileInfo
// File:"AppSettingHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:App Setting File Operate
// 1.GetSettingString(string key)
// 2.WriteAppSettings(string key,string value)
// 3.ReadAppSettings(string key)
// 4.IntializeConfigurationFile(List<(string key,string value)> list,string file_name)
//
// File Lines:92

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Jund.NETHelper.FileHelper
{
    /// <summary>
    /// 配置文件操作类
    /// </summary>
    public class AppSettingHelper
    {
        public static string GetSettingString(string key) => new AppSettingsReader().GetValue(key, typeof(string)).ToString();
        public static void WriteAppSettings(string key,string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                AppSettingsSection appSettings = config.AppSettings;
                appSettings.Settings.Add(key, value);

                config.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (ConfigurationException ex)
            {
            }
        }
        public static string ReadAppSettings(string key)
        {
            try
            {
                Configuration config =ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                AppSettingsSection appSettings = (AppSettingsSection)config.GetSection("appSettings");

                if (appSettings.Settings.Count != 0)
                {
                    return appSettings.Settings.AllKeys.First(obj => obj == key);
                }
                else
                    return String.Empty;
            }
            catch (ConfigurationException ex)
            {
                return String.Empty;
            }
        }
        public static void IntializeConfigurationFile(List<(string key,string value)> list,string file_name)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<appSettings>");

            for(int i=0;i<list.Count;i++)
            {
                builder.AppendLine("< add key ='" + list[i].key + "' value='" + list[i].value + "'/>");
            }

            builder.AppendLine("</appSettings>");

            File.WriteAllText(file_name, builder.ToString());

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            AppSettingsSection appSettings = config.AppSettings;
            appSettings.File = file_name;

            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
