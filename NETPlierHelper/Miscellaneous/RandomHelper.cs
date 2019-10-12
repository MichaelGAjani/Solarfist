using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
