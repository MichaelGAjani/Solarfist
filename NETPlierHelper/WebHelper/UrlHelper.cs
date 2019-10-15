// FileInfo
// File:"UrlHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Url
// 1.AddParameter(string url, string paramName, string value)
// 2.UpdateParameter(string url, string paramName, string value)
// 3.Domain(string url) 
// 4.ParseUrl(string url)
//
// File Lines:92
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Jund.NETHelper.WebHelper
{
    /// <summary>
    /// URLµÄ²Ù×÷Àà
    /// </summary>
    public class UrlHelper
    {
        public static string AddParameter(string url, string paramName, string value)
        {
            Uri uri = new Uri(url);
            string eval = HttpContext.Current.Server.UrlEncode(value);

            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            builder.Append(string.IsNullOrEmpty(uri.Query) ? "?" : "&");
            builder.Append(paramName);
            builder.Append("=");
            builder.Append(eval);

            return builder.ToString();
        }
        public static string UpdateParameter(string url, string paramName, string value)
        {
            List<(string key, string value)> list = ParseUrl(url);
            string domain = Domain(url);

            int index = list.FindIndex(obj => obj.key == paramName);
            if (index > -1)
            {
                (string key, string value) paraGroup = list[index];
                paraGroup.value = value;
                list[index] = paraGroup;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(domain);

            if(list.Count>0)
            {
                builder.Append("?");
                foreach((string key, string value)  para in list)
                {
                    builder.Append(para.key + "=" + para.value);
                }
            }

            return builder.ToString();
        }

        public static string Domain(string url) => new Uri(url).Host;
        public static List<(string key, string value)> ParseUrl(string url)
        {
            List<(string key, string value)> list = new List<(string key, string value)>();

            Uri uri = new Uri(url);
            string query = uri.Query.Replace("?", "");
            string[] parameterList = query.Split('&');
            foreach(string parameter in parameterList)
            {
                string[] para = parameter.Split('=');
                (string key, string value) tuple = (para[0], para[1]);
                list.Add(tuple);
            }

            return list;
        }
    }
}
