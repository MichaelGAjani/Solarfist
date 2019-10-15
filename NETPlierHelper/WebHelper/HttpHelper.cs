// FileInfo
// File:"HttpHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Http Post&Get
// 1.HttpGet(string Url, string postData)
// 2.HttpPost(string url, string postData)
// 3.GetHttpRequestData(HttpItem objhttpitem)
// 4.SetRequest
// 5.SetCertificate
// 6.SetCookie
// 7.SetPostData
// 8.SetProxy
// 9.CheckValidationResult
// 10.GetHtml
//
// File Lines:251
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Jund.NETHelper.WebHelper
{
    public class HttpHelper
    {
        HttpResult result = new HttpResult();
        HttpWebResponse response = null;
        HttpWebRequest request = null;
        Encoding encoding = Encoding.Default;
        HttpItem objhttpItem = new HttpItem();

        public HttpHelper ()
        {

        }
        public HttpHelper(HttpItem objhttpItem)
        {
            this.objhttpItem = objhttpItem;
        }

        public static string HttpGet(string Url, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postData == "" ? "" : "?") + postData);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        public static string HttpPost(string url, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = System.Text.Encoding.GetEncoding("utf-8");
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }
        private HttpResult GetHttpRequestData(HttpItem objhttpitem)
        {
            try
            {
                #region 得到请求的response
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    result.Header = response.Headers;
                    if (response.Cookies != null)
                    {
                        result.CookieCollection = response.Cookies;
                    }
                    if (response.Headers["set-cookie"] != null)
                    {
                        result.Cookie = response.Headers["set-cookie"];
                    }

                    Stream stream = response.GetResponseStream();

                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    //是否返回Byte类型数据
                    if (objhttpitem.ResultType == ResultType.Byte)
                    {
                        result.ResultByte = bytes;
                    }
                    result.Html = encoding.GetString(bytes);

                    stream.Close();
                }
                #endregion
            }
            catch (WebException ex)
            {
                result.Html = "String Error";
                response = (HttpWebResponse)ex.Response;
            }
            if (objhttpitem.IsLower)
            {
                result.Html = result.Html.ToLower();
            }
            return result;
        }
        private void SetRequest()
        {
            SetCertificate();
            SetProxy();
            request.Method = objhttpItem.Method.ToString();
            request.Timeout = objhttpItem.Timeout;
            request.ReadWriteTimeout = objhttpItem.ReadWriteTimeout;
            request.Accept = objhttpItem.Accept;
            request.ContentType = objhttpItem.ContentType;
            request.UserAgent = objhttpItem.UserAgent;
            SetCookie();
            request.Referer = objhttpItem.Referer;
            request.AllowAutoRedirect = objhttpItem.AllowAutoRedirect;
            SetPostData();
            if (objhttpItem.ConnectionLimit > 0)
            {
                request.ServicePoint.ConnectionLimit = objhttpItem.ConnectionLimit;
            }
        }
        private void SetCertificate()
        {
            if (!string.IsNullOrEmpty(objhttpItem.CertificatePath))
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                request = (HttpWebRequest)WebRequest.Create(objhttpItem.URL);
                X509Certificate objx509 = new X509Certificate(objhttpItem.CertificatePath);
                request.ClientCertificates.Add(objx509);
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create(objhttpItem.URL);
            }
        }
        private void SetCookie()
        {
            if (!string.IsNullOrEmpty(objhttpItem.Cookie))
            {
                request.Headers[HttpRequestHeader.Cookie] = objhttpItem.Cookie;
            }
            //设置Cookie
            if (objhttpItem.CookieCollection != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(objhttpItem.CookieCollection);
            }
        }
        private void SetPostData()
        {
            if (request.Method.Trim().ToLower().Contains("post"))
            {
                switch (objhttpItem.PostDataType)
                {
                    case PostDataType.Byte:
                        if (objhttpItem.PostDataByte != null && objhttpItem.PostDataByte.Length > 0)
                        {
                            request.ContentLength = objhttpItem.PostDataByte.Length;
                            request.GetRequestStream().Write(objhttpItem.PostDataByte, 0, objhttpItem.PostDataByte.Length);
                        }
                        break;
                    case PostDataType.FilePath:
                        StreamReader reader = new StreamReader(objhttpItem.PostData, encoding);
                        byte[] fileBuffer = Encoding.Default.GetBytes(reader.ReadToEnd());
                        reader.Close();
                        request.ContentLength = fileBuffer.Length;
                        request.GetRequestStream().Write(fileBuffer, 0, fileBuffer.Length);
                        break;
                    default:
                        if (!string.IsNullOrEmpty(objhttpItem.PostData))
                        {
                            byte[] buffer = Encoding.Default.GetBytes(objhttpItem.PostData);
                            request.ContentLength = buffer.Length;
                            request.GetRequestStream().Write(buffer, 0, buffer.Length);
                        }
                        break;
                }
            }
        }
        private void SetProxy()
        {
            if (string.IsNullOrEmpty(objhttpItem.HttpProxy.ProxyUsername) && string.IsNullOrEmpty(objhttpItem.HttpProxy.ProxyPassword) && string.IsNullOrEmpty(objhttpItem.HttpProxy.ProxyIP))
                return;
            WebProxy myProxy = new WebProxy(objhttpItem.HttpProxy.ProxyIP, false);
            //建议连接
            myProxy.Credentials = new NetworkCredential(objhttpItem.HttpProxy.ProxyUsername, objhttpItem.HttpProxy.ProxyPassword);
            //给当前请求对象
            request.Proxy = myProxy;
            //设置安全凭证
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
        }
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            // 总是接受    
            return true;
        }
        public HttpResult GetHtml()
        {
            //准备参数
            SetRequest();
            //调用专门读取数据的类
            return GetHttpRequestData(objhttpItem);
        }
    }
}
