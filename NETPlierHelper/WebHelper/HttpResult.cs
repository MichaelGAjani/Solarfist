using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Jund.NETHelper.WebHelper
{
    public class HttpResult
    {
        string _Cookie = string.Empty;
        CookieCollection cookiecollection = null;
        private string html = string.Empty;
        private byte[] resultbyte = null;
        private WebHeaderCollection header = new WebHeaderCollection();

        public string Cookie { get => _Cookie; set => _Cookie = value; }
        public CookieCollection CookieCollection { get => cookiecollection; set => cookiecollection = value; }
        public string Html { get => html; set => html = value; }
        public byte[] ResultByte { get => resultbyte; set => resultbyte = value; }
        public WebHeaderCollection Header { get => header; set => header = value; }
    }
}
