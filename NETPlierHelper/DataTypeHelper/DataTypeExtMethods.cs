// FileInfo
// File:"DataTypeExtMethodst.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:DataType Ext Methods
// 1.Base64EncodeBytes(this byte[] inputBytes)
// 2.EncodeBitmapToString(string bitmapFilePath)
// 3.MakeBase64EncodedStringForMime(string base64Encoded)
// 4.Base64DecodeString(this string inputStr)
// 5.AsciiEncodeBytes(byte[] asciiCharacterArray)
// 6.UnicodeEncodeBytes(byte[] unicodeCharacterArray)
// 7.AsciiDecodeString(string asciiCharacters)
// 8.UnicodeDecodeString(string unicodeCharacters)
// 9.DeterminingStringIsValidNumber(string str, out double result)
// 10.RoundUp(double valueToRound)
// 11.RoundDown(double valueToRound)
// 12.AddNarrowingChecked(this long lhs, long rhs)
// 13.InitialLetterUppercase(this string str)
//
// File Lines:100
using System;
using System.IO;
using System.Text;

namespace Jund.NETHelper.DataTypeHelper
{
    /// <summary>
    /// 数据类型拓展方法帮助类
    /// </summary>
    public static class DataTypeExtMethods
    {
        /// <summary>
        /// 数组转字符串
        /// </summary>
        /// <param name="inputBytes">数组</param>
        /// <returns></returns>
        public static string Base64EncodeBytes(this byte[] inputBytes) => (Convert.ToBase64String(inputBytes));
        /// <summary>
        /// 图片转字符串
        /// </summary>
        /// <param name="bitmapFilePath">图片路径</param>
        /// <returns></returns>
        public static string EncodeBitmapToString(string bitmapFilePath)
        {
            byte[] image = null;
            FileStream fstrm = new FileStream(bitmapFilePath, FileMode.Open, FileAccess.Read);
            using (BinaryReader reader = new BinaryReader(fstrm))
            {
                image = new byte[reader.BaseStream.Length];
                for (int i = 0; i < reader.BaseStream.Length; i++)
                    image[i] = reader.ReadByte();
            }
            return image.Base64EncodeBytes();
        }
        public static string MakeBase64EncodedStringForMime(string base64Encoded)
        {
            StringBuilder originalStr = new StringBuilder(base64Encoded);
            StringBuilder newStr = new StringBuilder();
            const int mimeBoundary = 76;
            int cntr = 1;
            while ((cntr * mimeBoundary) < (originalStr.Length - 1))
            {
                newStr.AppendLine(originalStr.ToString(((cntr - 1) * mimeBoundary), mimeBoundary));
                cntr++;
            }
            if (((cntr - 1) * mimeBoundary) < (originalStr.Length - 1))
            {
                newStr.AppendLine(originalStr.ToString(((cntr - 1) * mimeBoundary), ((originalStr.Length) - ((cntr - 1) * mimeBoundary))));

            }
            return newStr.ToString();
        }
        public static byte[] Base64DecodeString(this string inputStr)=> Convert.FromBase64String(inputStr);
        public static string AsciiEncodeBytes(byte[] asciiCharacterArray) => Encoding.ASCII.GetString(asciiCharacterArray);
        public static string UnicodeEncodeBytes(byte[] unicodeCharacterArray) => Encoding.Unicode.GetString(unicodeCharacterArray);
        public static byte[] AsciiDecodeString(string asciiCharacters) => Encoding.ASCII.GetBytes(asciiCharacters);
        public static byte[] UnicodeDecodeString(string unicodeCharacters) => Encoding.Unicode.GetBytes(unicodeCharacters);
        public static bool DeterminingStringIsValidNumber(string str, out double result)
        {
            result = 0;
            return (double.TryParse(str, System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.CurrentInfo, out result));
        }
        public static double RoundUp(double valueToRound) => Math.Floor(valueToRound + 0.5);
        public static double RoundDown(double valueToRound)
        {
            double floorValue = Math.Floor(valueToRound);
            if ((valueToRound - floorValue) > .5) return (floorValue + 1); else return (floorValue);
        }
        public static int AddNarrowingChecked(this long lhs, long rhs) => checked((int)(lhs + rhs));
        public static string InitialLetterUppercase(this string str) => str.ToCharArray()[0].ToString().ToUpper() + str.Remove(0, 1);
    }
}
