using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.NETHelper.WebHelper
{
    public class HttpProxy
    {
        private string proxyUsername = string.Empty;
        private string proxyPassword = string.Empty;
        private string proxyIP = string.Empty;

        public string ProxyUsername { get => proxyUsername; set => proxyUsername = value; }
        public string ProxyPassword { get => proxyPassword; set => proxyPassword = value; }
        public string ProxyIP { get => proxyIP; set => proxyIP = value; }
    }
}
