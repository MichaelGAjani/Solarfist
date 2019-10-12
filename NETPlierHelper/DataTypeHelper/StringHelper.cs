using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.NETHelper.DataTypeHelper
{
    public static class StringHelper
    {
        public enum DateTimeFormat
        {
            d,
            f,
            F,
            g,
            G,
            M,
            R,
            s,
            t,
            T,
            u,
            U,
            Y
        }
        /// <summary>
        /// 把字符串按分隔符转换成数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <param name="toLower"></param>
        /// <returns></returns>
        public static List<string> ConvertToListBySeparator(string str, char separator, bool toLower) =>( toLower?str.ToLower():str).Split(separator).ToList();
        /// <summary>
        /// 把字符串数组按分隔符合成字符串
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ConvertListToStringBySeparator(List<string> list, char separator)
        {
            StringBuilder builder = new StringBuilder();

            foreach (string s in list)
                builder.Append(s + separator.ToString());

            return builder.ToString().TrimEnd(separator);
        }
        /// <summary>
        /// 把数组按分隔符合成字符串
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ConvertListToStringBySeparator(List<int> list, char separator)
        {
            StringBuilder builder = new StringBuilder();

            foreach (int s in list)
                builder.Append(s.ToString() + separator.ToString());

            return builder.ToString().TrimEnd(separator);
        }
        /// <summary>
        /// 半角转全角
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HalfChartToFullChar(string str)
        {
            string result = string.Empty;
            foreach(char c in str)
            {
                if(c<127) result.Append<char>((char)(c + 65248));
               else if (c == 32) result.Append<char>((char)12288);
            }
           
            return result;
        }
        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FullChartToHalfChar(string str)
        {
            string result = string.Empty;
            foreach (char c in str)
            {
                if (c < 65375&&c> 65280) result.Append<char>((char)(c -
 65248));
                else if (c == 12288) result.Append<char>((char)32);
            }

            return result;
        }
        public static string GetFormatDateTime(DateTime date, string format) => string.Format(format, date);
        public static string GetFormatDateTime(DateTime date, DateTimeFormat format) => string.Format("{0:" + format + "}", date);       

    }
}
