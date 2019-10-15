// FileInfo
// File:"AspxPageHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Aspx Page
// 1.GetCurrentPage()
// 2.GetPageName()
// 3.GetQueryString(string key)
// 4.Redirect(string url)
//
// File Lines:33

using System.Web;
using System.Web.UI;

namespace Jund.NETHelper.WebPageHelper
{
    public class AspxPageHelper
    {
        public static Page GetCurrentPage()=> (Page)HttpContext.Current.Handler;
        public static string GetPageName() => GetCurrentPage().Title;    
        public static string GetQueryString(string key) => HttpContext.Current.Request.QueryString[key].Trim();
        public void Redirect(string url)=> GetCurrentPage().Response.Redirect(url);      
    }
}
