namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.Serialization;

    [Serializable]
    public class Server : IServer, IDisposable, ISerializable, ICloneable
    {
        private ConnectData m_connectData;
        protected IFactory m_factory;
        private string m_locale;
        private string m_name;
        protected ResourceManager m_resourceManager;
        protected IServer m_server;
        private string[] m_supportedLocales;
        private URL m_url;

        public event ServerShutdownEventHandler ServerShutdown
        {
            add
            {
                this.m_server.ServerShutdown += value;
            }
            remove
            {
                this.m_server.ServerShutdown -= value;
            }
        }

        public Server(Factory factory, URL url)
        {
            this.m_server = null;
            this.m_url = null;
            this.m_factory = null;
            this.m_connectData = null;
            this.m_name = null;
            this.m_locale = null;
            this.m_supportedLocales = null;
            this.m_resourceManager = null;
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            this.m_factory = (IFactory) factory.Clone();
            this.m_server = null;
            this.m_url = null;
            this.m_name = null;
            this.m_supportedLocales = null;
            this.m_resourceManager = new ResourceManager("Opc.Resources.Strings", Assembly.GetExecutingAssembly());
            if (url != null)
            {
                this.SetUrl(url);
            }
        }

        protected Server(SerializationInfo info, StreamingContext context)
        {
            this.m_server = null;
            this.m_url = null;
            this.m_factory = null;
            this.m_connectData = null;
            this.m_name = null;
            this.m_locale = null;
            this.m_supportedLocales = null;
            this.m_resourceManager = null;
            this.m_name = info.GetString("Name");
            this.m_url = (URL) info.GetValue("Url", typeof(URL));
            this.m_factory = (IFactory) info.GetValue("Factory", typeof(IFactory));
        }

        public virtual object Clone()
        {
            Server server = (Server) base.MemberwiseClone();
            server.m_server = null;
            server.m_supportedLocales = null;
            server.m_locale = null;
            server.m_resourceManager = new ResourceManager("Opc.Resources.Strings", Assembly.GetExecutingAssembly());
            return server;
        }

        public virtual void Connect()
        {
            this.Connect(this.m_url, null);
        }

        public virtual void Connect(ConnectData connectData)
        {
            this.Connect(this.m_url, connectData);
        }

        public virtual void Connect(URL url, ConnectData connectData)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            if (this.m_server != null)
            {
                throw new AlreadyConnectedException();
            }
            this.SetUrl(url);
            try
            {
                this.m_server = this.m_factory.CreateInstance(url, connectData);
                this.m_connectData = connectData;
                this.GetSupportedLocales();
                this.SetLocale(this.m_locale);
            }
            catch (Exception exception)
            {
                if (this.m_server != null)
                {
                    try
                    {
                        this.Disconnect();
                    }
                    catch
                    {
                    }
                }
                throw exception;
            }
        }

        public virtual void Disconnect()
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            this.m_server.Dispose();
            this.m_server = null;
        }

        public virtual void Dispose()
        {
            if (this.m_factory != null)
            {
                this.m_factory.Dispose();
                this.m_factory = null;
            }
            if (this.m_server != null)
            {
                try
                {
                    this.Disconnect();
                }
                catch
                {
                }
                this.m_server = null;
            }
        }

        public virtual Server Duplicate()
        {
            Server server = (Server) Activator.CreateInstance(base.GetType(), new object[] { this.m_factory, this.m_url });
            server.m_connectData = this.m_connectData;
            server.m_locale = this.m_locale;
            return server;
        }

        public static string FindBestLocale(string requestedLocale, string[] supportedLocales)
        {
            try
            {
                foreach (string str in supportedLocales)
                {
                    if (str == requestedLocale)
                    {
                        return requestedLocale;
                    }
                }
                CultureInfo info = new CultureInfo(requestedLocale);
                foreach (string str2 in supportedLocales)
                {
                    try
                    {
                        CultureInfo info2 = new CultureInfo(str2);
                        if (info.Parent.Name == info2.Name)
                        {
                            return info2.Name;
                        }
                    }
                    catch
                    {
                    }
                }
                return (((supportedLocales != null) && (supportedLocales.Length > 0)) ? supportedLocales[0] : "");
            }
            catch
            {
                return (((supportedLocales != null) && (supportedLocales.Length > 0)) ? supportedLocales[0] : "");
            }
        }

        public virtual string GetErrorText(string locale, ResultID resultID)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return this.m_server.GetErrorText((locale == null) ? this.m_locale : locale, resultID);
        }

        public virtual string GetLocale()
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            this.m_locale = this.m_server.GetLocale();
            return this.m_locale;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", this.m_name);
            info.AddValue("Url", this.m_url);
            info.AddValue("Factory", this.m_factory);
        }

        protected string GetString(string name)
        {
            CultureInfo culture = null;
            try
            {
                culture = new CultureInfo(this.Locale);
            }
            catch
            {
                culture = new CultureInfo("");
            }
            try
            {
                return this.m_resourceManager.GetString(name, culture);
            }
            catch
            {
                return null;
            }
        }

        public virtual string[] GetSupportedLocales()
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            this.m_supportedLocales = this.m_server.GetSupportedLocales();
            return this.SupportedLocales;
        }

        public virtual string SetLocale(string locale)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            try
            {
                this.m_locale = this.m_server.SetLocale(locale);
            }
            catch
            {
                string str = FindBestLocale(locale, this.m_supportedLocales);
                if (str != locale)
                {
                    this.m_server.SetLocale(str);
                }
                this.m_locale = str;
            }
            return this.m_locale;
        }

        protected void SetUrl(URL url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            if (this.m_server != null)
            {
                throw new AlreadyConnectedException();
            }
            this.m_url = (URL) url.Clone();
            string str = "";
            if (this.m_url.HostName != null)
            {
                str = this.m_url.HostName.ToLower();
                switch (str)
                {
                    case "localhost":
                    case "127.0.0.1":
                        str = "";
                        break;
                }
            }
            if (this.m_url.Port != 0)
            {
                str = str + string.Format(".{0}", this.m_url.Port);
            }
            if (str != "")
            {
                str = str + ".";
            }
            if (this.m_url.Scheme != "http")
            {
                string path = this.m_url.Path;
                int length = path.LastIndexOf('/');
                if (length != -1)
                {
                    path = path.Substring(0, length);
                }
                str = str + path;
            }
            else
            {
                string str3 = this.m_url.Path;
                int num2 = str3.LastIndexOf('.');
                if (num2 != -1)
                {
                    str3 = str3.Substring(0, num2);
                }
                while (str3.IndexOf('/') != -1)
                {
                    str3 = str3.Replace('/', '-');
                }
                str = str + str3;
            }
            this.m_name = str;
        }

        public virtual bool IsConnected
        {
            get
            {
                return (this.m_server != null);
            }
        }

        public virtual string Locale
        {
            get
            {
                return this.m_locale;
            }
        }

        public virtual string Name
        {
            get
            {
                return this.m_name;
            }
            set
            {
                this.m_name = value;
            }
        }

        public virtual string[] SupportedLocales
        {
            get
            {
                if (this.m_supportedLocales == null)
                {
                    return null;
                }
                return (string[]) this.m_supportedLocales.Clone();
            }
        }

        public virtual URL Url
        {
            get
            {
                if (this.m_url == null)
                {
                    return null;
                }
                return (URL) this.m_url.Clone();
            }
            set
            {
                this.SetUrl(value);
            }
        }

        private class Names
        {
            internal const string FACTORY = "Factory";
            internal const string NAME = "Name";
            internal const string URL = "Url";
        }
    }
}

