// FileInfo
// File:"SessionHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Session
// 1.SetSession(string name, object val)
// 2.ClearSession()
// 3.RemoveSession(string name)
// 4.AddSession(string key, string value, int time)
// 5.GetSession(string name) 
// 6.DeleteSession(string name)
// 7.ChangeSessionTime(int time)
//
// File Lines:47

using System.Web;

namespace Jund.NETHelper.WebHelper
{
    public class SessionHelper
    {
        public static void SetSession(string name, object val)
        {
            HttpContext.Current.Session.Remove(name);
            HttpContext.Current.Session.Add(name, val);
        }
        public static void ClearSession()=> HttpContext.Current.Session.Clear();
        public static void RemoveSession(string name)=> HttpContext.Current.Session.Remove(name);
        public static void AddSession(string key, string value, int time)
        {
            HttpContext.Current.Session[key] = value;
            HttpContext.Current.Session.Timeout = time;
        }

        public static string GetSession(string name) => HttpContext.Current.Session[name].ToString();        
        public static void DeleteSession(string name) => HttpContext.Current.Session[name] = null;
        public static void ChangeSessionTime(int time)=> HttpContext.Current.Session.Timeout = time;
    }
}
