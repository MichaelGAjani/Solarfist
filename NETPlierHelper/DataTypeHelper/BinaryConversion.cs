// FileInfo
// File:"BinaryConversion.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:BinaryConversion
// 1.DecimaltoBinary(int dec)
// 2.DecimaltoHex(int dec)
// 3.BinarytoDecimal(string bin)
// 4.BinarytoHex(string bin)
// 5.HextoBinary(int hex)
// 6.HextoDecimal(int hex)
//
// File Lines:68
using System;

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
