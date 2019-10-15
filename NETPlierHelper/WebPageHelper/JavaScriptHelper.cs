// FileInfo
// File:"JavaScriptHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Java Script
//
// File Lines:111
using System.Web;
using System.Web.UI.WebControls;

namespace Jund.NETHelper.WebPageHelper
{
    /// <summary>
    /// 客户端脚本输出
    /// </summary>
    public class JavaScriptHelper
    {
        static string js_alert = "<script language=javascript>alert('{0}');</script>";
        static string js_alert_redirect = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";
        static string js_alert_history = @"<Script language='JavaScript'>alert('{0}');history.go({1});</Script>";
        static string js_alert_parent = "<script language=javascript>alert('{0}');window.top.location.replace('{1}')</script>";
        static string js_parent = "<script language=javascript>window.top.location.replace('{0}')</script>";
        static string js_close = @"<Script language='JavaScript'>parent.opener=null;window.close();  </Script>";
        static string js_history = @"<Script language='JavaScript'>history.go({0});  </Script>";
        static string js_refresh = @"<Script language='JavaScript'> opener.location.reload(); </Script>";
        public static void AlertAndRedirect(string message, string toURL)
        {            
            HttpContext.Current.Response.Write(string.Format(js_alert_redirect, message, toURL));
            HttpContext.Current.Response.End();
        }
        public static void AlertAndRedirect(string message, int value)
        {           
            HttpContext.Current.Response.Write(string.Format(js_alert_history, message, value));
            HttpContext.Current.Response.End();
        }
        public static void AlertAndParentUrl(string message, string toURL)=> HttpContext.Current.Response.Write(string.Format(js_alert_parent, message, toURL));      
        public static void ParentRedirect(string ToUrl)=> HttpContext.Current.Response.Write(string.Format(js_parent, ToUrl));
        public static void Alert(string message)=> HttpContext.Current.Response.Write(string.Format(js_alert, message));
        public static void RegisterScriptBlock(System.Web.UI.Page page, string scriptString) => page.ClientScript.RegisterStartupScript(page.GetType(), "scriptblock", "<script type='text/javascript'>" + scriptString + "</script>");
        public static string ShowModelWindowScript(string wid, string title, int width, int height, string url)=>
            string.Format("setTimeout(\"showModalWindow('{0}','{1}',{2},{3},'{4}')\",100);", wid, title, width, height, url);
        public static void ShowCilentConfirm(WebControl control, string eventName, string title, int width, int height, string message)=>
             control.Attributes[eventName] = string.Format("return showConfirm('{0}',{1},{2},'{3}','{4}');", title, width, height, message, control.ClientID);
        public static void GoHistory(int value)
        {
            HttpContext.Current.Response.Write(string.Format(js_history, value));
            HttpContext.Current.Response.End();
        }
        public static void CloseWindow()
        {
            HttpContext.Current.Response.Write(js_close);
            HttpContext.Current.Response.End();
        }
        public static void RefreshParentForm(string url)
        {
            string js = @"<script>try{top.location=""" + url + @"""}catch(e){location=""" + url + @"""}</script>";
            HttpContext.Current.Response.Write(js);
        }
        public static void RefreshOpenForm()=> HttpContext.Current.Response.Write(js_refresh);
        public static void JavaScriptLocationHref(string url)
        {
            string js = @"<Script language='JavaScript'>
                    window.location.replace('{0}');
                  </Script>";
            js = string.Format(js, url);
            HttpContext.Current.Response.Write(js);
        }
        public static void ShowModalDialogWindow(string webFormUrl, int width, int height, int top, int left)
        {
            string features = "dialogWidth:" + width.ToString() + "px"
                + ";dialogHeight:" + height.ToString() + "px"
                + ";dialogLeft:" + left.ToString() + "px"
                + ";dialogTop:" + top.ToString() + "px"
                + ";center:yes;help=no;resizable:no;status:no;scroll=yes";
            ShowModalDialogWindow(webFormUrl, features);
        }
        public static void ShowModalDialogWindow(string webFormUrl, string features)
        {
            string js = ShowModalDialogJavascript(webFormUrl, features);
            HttpContext.Current.Response.Write(js);
        }
        public static string ShowModalDialogJavascript(string webFormUrl, string features)
        {
            string js = @"<script language=javascript>							
							showModalDialog('" + webFormUrl + "','','" + features + "');</script>";
            return js;
        }
        public static void OpenWebFormSize(string url, int width, int heigth, int top, int left)
        {
            string js = @"<Script language='JavaScript'>window.open('" + url + @"','','height=" + heigth + ",width=" + width + ",top=" + top + ",left=" + left + ",location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');</Script>";

            HttpContext.Current.Response.Write(js);
        }

        public static void JavaScriptExitIfream(string url)
        {
            string js = @"<Script language='JavaScript'>
                    parent.window.location.replace('{0}');
                  </Script>";
            js = string.Format(js, url);
            HttpContext.Current.Response.Write(js);
        }
    }
}