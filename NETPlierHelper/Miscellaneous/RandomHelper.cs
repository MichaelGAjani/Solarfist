// FileInfo
// File:"RandomHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Random
// 1.GetRandomString(int length)
// 2.GetRandomItem(List<object> list)
// 3.GetRandomNumber(int min, int max)
// 4.GetRandomFloatNumber(int min, int max, int dec_places)
// 5.GetRandomDate(int min_year,int max_year)
//
// File Lines:92
using System;
using System.Collections.Generic;

namespace Jund.NETHelper.Miscellaneous
{
    public static class RandomHelper
    {
        static Random rnd = new Random();
        static string randomstr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxry1234567890";

        public static string GetRandomString(int length)
        {
            string result = String.Empty;

            while (result.Length < length)
                result += randomstr[rnd.Next(0, randomstr.Length)];

            return result;
        }
        public static object GetRandomItem(List<object> list) => list[rnd.Next(0, list.Count)];
        public static int GetRandomNumber(int min, int max) => rnd.Next(min, max + 1);
        public static double GetRandomFloatNumber(int min, int max, int dec_places) => Math.Round(Convert.ToDouble(rnd.Next(min, max)) + rnd.NextDouble(), dec_places);
        public static DateTime GetRandomDate(int min_year,int max_year)
        {
            DateTime min_date = new DateTime(min_year, 1, 1);
            DateTime max_date = new DateTime(max_year, 12, 31);

            return min_date.AddDays(rnd.Next(0, (max_date - min_date).Days));
        }
    }
}
