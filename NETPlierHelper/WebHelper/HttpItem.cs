using System;
using System.Net;

namespace Jund.NETHelper.WebHelper
{
    public class HttpItem
    {
        string _URL;
        HttpMethod _Method =  HttpMethod.Get;
        int _Timeout = 100000;
        int _ReadWriteTimeout = 30000;
        string _Accept = "text/html, application/xhtml+xml, */*";
        string _ContentType = "text/html";
        string _UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
        string _Encoding = string.Empty;
        private PostDataType _PostDataType = PostDataType.String;
        string _PostData;
        private byte[] _PostDataByte = null;
        CookieCollection cookiecollection = null;
        string _Cookie = string.Empty;
        string _Referer = string.Empty;
        string _CertificatePath = string.Empty;
        private Boolean isLower = true;
        private Boolean allowAutoRedirect = true;
        private int connectionLimit = 1024;
        private HttpProxy _httpProxy = new HttpProxy();
        private ResultType _resultType = ResultType.String;

        public string URL { get => _URL; set => _URL = value; }
        public HttpMethod Method { get => _Method; set => _Method = value; }
        public int Timeout { get => _Timeout; set => _Timeout = value; }
        public int ReadWriteTimeout { get => _ReadWriteTimeout; set => _ReadWriteTimeout = value; }
        public string Accept { get => _Accept; set => _Accept = value; }
        public string ContentType { get => _ContentType; set => _ContentType = value; }
        public string UserAgent { get => _UserAgent; set => _UserAgent = value; }
        public string Encoding { get => _Encoding; set => _Encoding = value; }
        public PostDataType PostDataType { get => _PostDataType; set => _PostDataType = value; }
        public string PostData { get => _PostData; set => _PostData = value; }
        public byte[] PostDataByte { get => _PostDataByte; set => _PostDataByte = value; }
        public CookieCollection CookieCollection { get => cookiecollection; set => cookiecollection = value; }
        public string Cookie { get => _Cookie; set => _Cookie = value; }
        public string Referer { get => _Referer; set => _Referer = value; }
        public string CertificatePath { get => _CertificatePath; set => _CertificatePath = value; }
        public bool IsLower { get => isLower; set => isLower = value; }
        public bool AllowAutoRedirect { get => allowAutoRedirect; set => allowAutoRedirect = value; }
        public int ConnectionLimit { get => connectionLimit; set => connectionLimit = value; }
        public HttpProxy HttpProxy { get => _httpProxy; set => _httpProxy = value; }
        public ResultType ResultType { get => _resultType; set => _resultType = value; }
    }
}
