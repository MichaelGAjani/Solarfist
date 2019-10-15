// FileInfo
// File:"ChineseCultureHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Chinese Culture
// 1.ChartoPinyin(string str)
// 2.ChineseYuan(decimal number)
//
// File Lines:120
using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Jund.NETHelper.CultureHelper
{
    /// <summary>
    /// 中国文化帮助类
    /// </summary>
    public class ChineseCultureHelper
    {
        /// <summary>
        /// 农历
        /// </summary>
        private static ChineseLunisolarCalendar chineseCalendar = new ChineseLunisolarCalendar();
        public enum Holiday
        {
            SpringFestival,//春节
            LanternFestival,//元宵
            ChingMingFestival,//清明
            DragonBoatFestival,//端午
            Tanabata,//七夕
            MidAutumnFestival,//中秋
            DoubleNinthFestival,//重阳
            ChineseNewYearsEve,//除夕
            NewYears,//元旦
            Labor,//劳动节
            NationalDay//国庆
        }

        private List<string> Celestial = new List<string>() { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
        private List<string> Terrestrial = new List<string>() { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

        private DateTime _chinese_date;
        private DateTime _date;
        public ChineseCultureHelper(DateTime date)
        {
            _chinese_date = chineseCalendar.ToDateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
            _date = date;
        }

        public DateTime ChineseDate => _chinese_date; 
        public int Year=> _chinese_date.Year; 
        public int Month => _chinese_date.Month; 
        public int Day =>_chinese_date.Day; 
        public bool IsFestival()
        {
            switch (Month)
            {
                case 1:return ChineseDate.Day <=6;//春节
                case 5:return ChineseDate.Day == 5;//端午
                case 8:return ChineseDate.Day == 15;//中秋
            }

            switch(_date.Month)
            {
                case 1:return _date.Day == 1;
                case 4:return _date.Day == 5;
                case 5:return _date.Day == 1;
                case 10:return _date.Day <= 7;
            }

            return ChineseDate.AddDays(1).Month == 1 && ChineseDate.AddDays(1).Day == 1;
        }
        public DateTime FirstDate  => chineseCalendar.MinSupportedDateTime;
        public DateTime LastDate => chineseCalendar.MaxSupportedDateTime;
        public int CelestialStem => chineseCalendar.GetCelestialStem(Year);
        public string CelestialChar => Celestial[CelestialStem - 1];      
        public int TerrestrialBranch => chineseCalendar.GetTerrestrialBranch(Year);       
        public string TerrestrialChar => Terrestrial[TerrestrialBranch - 1];        
        public int SexagenaryYear  => chineseCalendar.GetSexagenaryYear(ChineseDate); 
        public string SexagenaryChar => Celestial[(SexagenaryYear + 1) % 10] + Terrestrial[(SexagenaryYear + 1) % 12]; 
        public int IsLeapMonth => chineseCalendar.GetLeapMonth(Year); 
        public bool IsLeapYear => chineseCalendar.IsLeapYear(Year); 
        public int WeekOfYear => chineseCalendar.GetWeekOfYear(ChineseDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday); 
        public static List<string> ChartoPinyin(string str)
        {
            List<string> pinyin_list = new List<string>();

            foreach (char c in str)
            {
                ChineseChar ch = new ChineseChar(c);

                string pinyin = String.Empty;
                foreach (string s in ch.Pinyins)
                    pinyin += s;

                pinyin_list.Add(pinyin);
            }

            return pinyin_list;
        }
        public static string ChineseYuan(decimal number)
        {
            var s = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            return r;
        }
    }
}
