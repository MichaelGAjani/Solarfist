// FileInfo
// File:"RegaxHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Regax Match
// 1.IsNumeric(string value)
// 2.Int(string value) 
// 3.IsFloat(string value) 
// 4.IsUnsign(string value)
// 5.IsAlphanumeric(string value) 
// 6.IsLatin(string value) 
// 7.IsIPv4(string value) 
// 8.IsEmailAddress(string value) 
// 9.IsURL(string value) 
// 10.IsValidRegisterName(string value) 
// 11.IsPassword(string value, int min_char = 6, int max_char = 32)
// 12.IsPostCode(string value) 
// 13.sTelphone(string value)
//
// File Lines:52
using System.Text.RegularExpressions;

namespace Jund.NETHelper.RegularHelper
{
    /// <summary>
    /// 正则表达式验证类
    /// </summary>
    public static class RegaxHelper
    {
        public static bool IsNumeric(string value)=> Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        public static bool IsInt(string value)=> Regex.IsMatch(value, @"^(\+|\-)?\d+$");
        public static bool IsFloat(string value) => Regex.IsMatch(value, @"^(\+|\-)?(\d*\.\d+)$");
        public static bool IsUnsign(string value)=> Regex.IsMatch(value, @"^\d*[.]?\d*$");
        public static bool IsAlphanumeric(string value) => Regex.IsMatch(value, @"^([\w\.\+\-]|\s)*$");
        public static bool IsLatin(string value) => Regex.IsMatch(value, @"^[a-zA-Z\'\-\s]$");
        public static bool IsIPv4(string value) => Regex.IsMatch(value, @"^([0-2]?[0-9]?[0-9]\.){3}[0-2]?[0-9]?[0-9]$");
        public static bool IsEmailAddress(string value) => Regex.IsMatch(value, @"^[A-Za-z0-9_\-\.]+@(([A - Za - z0 - 9\-])+\.)+([A - Za - z\-])+$");
        public static bool IsURL(string value) => Regex.IsMatch(value, @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*$");
        public static bool IsValidRegisterName(string value) => Regex.IsMatch(value, @"^[a-zA-Z]{1}([a-zA-Z0-9]|[._]){4,19}*$");
        public static bool IsPassword(string value,int min_char=6,int max_char=32) => Regex.IsMatch(value, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{"+min_char.ToString()+","+max_char.ToString()+"}$");
        public static bool IsPostCode(string value) => Regex.IsMatch(value, @"^\d{6}$");
        public static bool IsTelphone(string value) => Regex.IsMatch(value, @"^\d[2]{2}|\d{4}-?\d{7,8}$");
    }
}
