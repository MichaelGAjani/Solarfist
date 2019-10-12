namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;

    [Serializable]
    public class ConnectData : ISerializable, ICredentials
    {
        private NetworkCredential m_credentials;
        private string m_licenseKey;
        private WebProxy m_proxy;

        public ConnectData(NetworkCredential credentials)
        {
            this.m_credentials = null;
            this.m_proxy = null;
            this.m_licenseKey = null;
            this.m_credentials = credentials;
            this.m_proxy = null;
        }

        public ConnectData(NetworkCredential credentials, WebProxy proxy)
        {
            this.m_credentials = null;
            this.m_proxy = null;
            this.m_licenseKey = null;
            this.m_credentials = credentials;
            this.m_proxy = proxy;
        }

        protected ConnectData(SerializationInfo info, StreamingContext context)
        {
            this.m_credentials = null;
            this.m_proxy = null;
            this.m_licenseKey = null;
            string userName = info.GetString("UN");
            string password = info.GetString("PW");
            string domain = info.GetString("DO");
            string address = info.GetString("PU");
            info.GetString("LK");
            if (domain != null)
            {
                this.m_credentials = new NetworkCredential(userName, password, domain);
            }
            else
            {
                this.m_credentials = new NetworkCredential(userName, password);
            }
            if (address != null)
            {
                this.m_proxy = new WebProxy(address);
            }
            else
            {
                this.m_proxy = null;
            }
        }

        public NetworkCredential GetCredential(Uri uri, string authenticationType)
        {
            if (this.m_credentials != null)
            {
                return new NetworkCredential(this.m_credentials.UserName, this.m_credentials.Password, this.m_credentials.Domain);
            }
            return null;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (this.m_credentials != null)
            {
                info.AddValue("UN", this.m_credentials.UserName);
                info.AddValue("PW", this.m_credentials.Password);
                info.AddValue("DO", this.m_credentials.Domain);
            }
            else
            {
                info.AddValue("UN", null);
                info.AddValue("PW", null);
                info.AddValue("DO", null);
            }
            if (this.m_proxy != null)
            {
                info.AddValue("PU", this.m_proxy.Address);
            }
            else
            {
                info.AddValue("PU", null);
            }
        }

        public IWebProxy GetProxy()
        {
            if (this.m_proxy != null)
            {
                return this.m_proxy;
            }
            return WebProxy.GetDefaultProxy();
        }

        public void SetProxy(WebProxy proxy)
        {
            this.m_proxy = proxy;
        }

        public NetworkCredential Credentials
        {
            get
            {
                return this.m_credentials;
            }
            set
            {
                this.m_credentials = value;
            }
        }

        public string LicenseKey
        {
            get
            {
                return this.m_licenseKey;
            }
            set
            {
                this.m_licenseKey = value;
            }
        }

        private class Names
        {
            internal const string DOMAIN = "DO";
            internal const string LICENSE_KEY = "LK";
            internal const string PASSWORD = "PW";
            internal const string PROXY_URI = "PU";
            internal const string USER_NAME = "UN";
        }
    }
}

