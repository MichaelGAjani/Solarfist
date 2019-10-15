// FileInfo
// File:"CookieHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Cookie
// 1.ClearCookie(string key)
// 2.GetCookie(string key)
// 3.SetCookie(string key, string value, DateTime expires)
//
// File Lines:56

using System;
using System.Web;

namespace Jund.NETHelper.WebHelper
{
    public class CookieHelper
    {
        public static void ClearCookie(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }      
        public static string GetCookie(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
           
            if (cookie != null)
            {
                 return HttpUtility.UrlDecode(cookie.Value);
            }
            return String.Empty;
        }
        public static void SetCookie(string key, string value, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(key)
            {
                Value = value,
                Expires = expires
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
