// FileInfo
// File:"WebHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Website
// 1.GetWebSitePath(string localPath)
// 2.WebSitePath
// 3.GetMapPath(string localPath)
//
// File Lines:39
using System.Web;

namespace Jund.NETHelper.WebPageHelper
{
    public class WebHelper
    {
        public enum SortType
        {
            Photo = 1,
            Article = 5,
            Diary = 7,
            Pic = 2,
            Music = 6,
            AddressList = 4,
            Favorite = 3
        }
        public static string GetWebSitePath(string localPath) => HttpContext.Current.Request.ApplicationPath + @"/" + localPath;      
        public static string WebSitePath=> System.Web.HttpContext.Current.Request.ApplicationPath;  
        public static string GetMapPath(string localPath)=> System.Web.HttpContext.Current.Server.MapPath(localPath);
    }
}
