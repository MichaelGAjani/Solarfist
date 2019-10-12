using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.NETHelper.DataTypeHelper
{
    public static class DateTimeHelper
    {        
        public static DateTime ConvertToDateTime(string str, string format) => DateTime.ParseExact(str, format, System.Globalization.CultureInfo.CurrentCulture);
        public static DateTime GetLastDate(int year, int month) => new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
        public static DayOfWeek GetDayOfWeek(DateTime date) => CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
        public static int GetDayOfYear(DateTime date) => CultureInfo.InvariantCulture.Calendar.GetDayOfYear(date);
        public static int GetLeapMonth(DateTime date) => CultureInfo.InvariantCulture.Calendar.GetLeapMonth(date.Year);
        public static int GetWeekOfYear(DateTime date, DayOfWeek firstDayofWeek,bool full_week) => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, full_week?CalendarWeekRule.FirstFullWeek: CalendarWeekRule.FirstDay, firstDayofWeek);
    }
}
