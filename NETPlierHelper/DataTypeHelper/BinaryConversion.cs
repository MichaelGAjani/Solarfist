using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.NETHelper.DataTypeHelper
{
    /// <summary>
    /// 进制转换帮助类
    /// </summary>
    public static class BinaryConversion
    {
        /// <summary>
        /// 十进制转二进制
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static string DecimaltoBinary(int dec) => Convert.ToString(dec, 2);
        /// <summary>
        /// 十进制转十六进制
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static string DecimaltoHex(int dec) => Convert.ToString(dec, 16);
        /// <summary>
        /// 二进制转十进制
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static int BinarytoDecimal(string bin) => Convert.ToInt32(bin, 2);
        /// <summary>
        /// 二进制转十六进制
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static string BinarytoHex(string bin) => string.Format("{0:x}", Convert.ToInt32(bin, 2));
        /// <summary>
        /// 十六进制转二进制
        /// </summary>
        /// <param name="hex">十六进制数：0x</param>
        /// <returns></returns>
        public static string HextoBinary(int hex) => Convert.ToString(hex, 2);
        /// <summary>
        /// 
        /// </summary十六进制转十进制>
        /// <param name="hex">十六进制数：0x</param>
        /// <returns></returns>
        public static string HextoDecimal(int hex) => Convert.ToString(hex, 10);
    }
}
