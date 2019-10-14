// FileInfo
// File:"DateTimeHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:DateTime Func
// 1.ConvertToDateTime(string str, string format)
// 2.GetLastDate(int year, int month)
// 3.DayOfWeek GetDayOfWeek(DateTime date)
// 4.GetDayOfYear(DateTime date)
// 5.GetLeapMonth(DateTime date)
// 6.GetWeekOfYear(DateTime date, DayOfWeek firstDayofWeek,bool full_week)
//
// File Lines:40

using System;
using System.Globalization;

namespace Jund.NETHelper.DataTypeHelper
{
    /// <summary>
    /// 时间扩展类
    /// </summary>
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
