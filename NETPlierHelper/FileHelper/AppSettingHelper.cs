using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.NETHelper.FileHelper
{
    public class AppSettingHelper
    {
        public static string GetSettingString(string key) => new AppSettingsReader().GetValue(key, typeof(string)).ToString();
        public static void WriteAppSettings(string key,string value)
        {
            try
            {
                // Get the application configuration file.
                System.Configuration.Configuration config =
                   ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // Add the key/value pair to the appSettings 
                // section.
                // config.AppSettings.Settings.Add(keyName, value);
                System.Configuration.AppSettingsSection appSettings = config.AppSettings;
                appSettings.Settings.Add(key, value);

                // Save the configuration file.
                config.Save(ConfigurationSaveMode.Modified);

                // Force a reload in memory of the changed section.
                // This to read the section with the
                // updated values.
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception e)
            {
            }
        }
        public static string ReadAppSettings(string key)
        {
            try
            {

                // Get the configuration file.
                System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // Get the appSettings section.
                System.Configuration.AppSettingsSection appSettings =
                    (System.Configuration.AppSettingsSection)config.GetSection("appSettings");

                // Get the auxiliary file name.
                Console.WriteLine("Auxiliary file: {0}", config.AppSettings.File);


                // Get the settings collection (key/value pairs).
                if (appSettings.Settings.Count != 0)
                {
                    return appSettings.Settings.AllKeys.First(obj => obj == key);
                }
                else
                    return String.Empty;
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }
        public static void IntializeConfigurationFile(List<Tuple<string,string>> list,string file_name)
        {
            // Create a set of unique key/value pairs to store in
            // the appSettings section of an auxiliary configuration
            // file.
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<appSettings>");

            for(int i=0;i<list.Count;i++)
            {
                builder.AppendLine("< add key ='" + list[i].Item1 + "' value='" + list[i].Item2 + "'/>");
            }

            builder.AppendLine("</appSettings>");

            // Create an auxiliary configuration file and store the
            // appSettings defined before.
            // Note creating a file at run-time is just for demo 
            // purposes to run this example.
            File.WriteAllText(file_name, builder.ToString());

            // Get the current configuration associated
            // with the application.
            System.Configuration.Configuration config =
               ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // Associate the auxiliary with the default
            // configuration file. 
            System.Configuration.AppSettingsSection appSettings = config.AppSettings;
            appSettings.File = file_name;

            // Save the configuration file.
            config.Save(ConfigurationSaveMode.Modified);

            // Force a reload in memory of the 
            // changed section.
            ConfigurationManager.RefreshSection("appSettings");

        }
    }
}
