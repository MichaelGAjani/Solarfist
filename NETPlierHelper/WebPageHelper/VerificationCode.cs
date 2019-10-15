
// FileInfo
// File:"VerificationCode.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:VerificationCode
// 1.CreateNumberCode(int length)
// 2.CreateMixedCode(int length)
// 3.CreateCharCode(int length)
// 4.CreateCode(int Length, char[] list)
//
// File Lines:52
using System;

namespace Jund.NETHelper.WebPageHelper
{
    /// <summary>
    /// 验证码类
    /// </summary>
    public static class VerificationCode
    {
		/// <summary>
		/// create a random key
		/// </summary>
		static readonly Random Random = new Random(~unchecked((int)DateTime.Now.Ticks));
        static readonly char[] NumberList = {'1','2','3','4','5','6','7','8','9'};
        static readonly char[] CharList = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        static readonly char[] MixedList = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' }; //remove I & O

        public static string CreateNumberCode(int length)=> CreateCode(length, NumberList);
        public static string CreateMixedCode(int length)=> CreateCode(length, MixedList);
		public static string CreateCharCode(int length)=> CreateCode(length, CharList);
		private static string CreateCode(int Length, char[] list)
		{
			char[] Pattern = list;
			string result = string.Empty;

			for (int i = 0; i < Length; i++)
			{
				result += Pattern[Random.Next(0, Pattern.Length)];
			}
			return result;
		}
    }
}