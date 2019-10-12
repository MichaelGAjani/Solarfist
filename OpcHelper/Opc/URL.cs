namespace Jund.OpcHelper.Opc
{
    using System;

    [Serializable]
    public class URL : ICloneable
    {
        private string m_hostName;
        private string m_path;
        private int m_port;
        private string m_scheme;

        public URL()
        {
            this.m_scheme = null;
            this.m_hostName = null;
            this.m_port = 0;
            this.m_path = null;
            this.Scheme = "http";
            this.HostName = "localhost";
            this.Port = 0;
            this.Path = null;
        }

        public URL(string url)
        {
            this.m_scheme = null;
            this.m_hostName = null;
            this.m_port = 0;
            this.m_path = null;
            this.Scheme = "http";
            this.HostName = "localhost";
            this.Port = 0;
            this.Path = null;
            string str = url;
            int index = str.IndexOf("://");
            if (index >= 0)
            {
                this.Scheme = str.Substring(0, index);
                str = str.Substring(index + 3);
            }
            index = str.IndexOfAny(new char[] { ':', '/' });
            if (index < 0)
            {
                this.Path = str;
            }
            else
            {
                this.HostName = str.Substring(0, index);
                if (str[index] == ':')
                {
                    str = str.Substring(index + 1);
                    index = str.IndexOf("/");
                    string str2 = null;
                    if (index >= 0)
                    {
                        str2 = str.Substring(0, index);
                        str = str.Substring(index + 1);
                    }
                    else
                    {
                        str2 = str;
                        str = "";
                    }
                    try
                    {
                        this.Port = System.Convert.ToUInt16(str2);
                    }
                    catch
                    {
                        this.Port = 0;
                    }
                }
                else
                {
                    str = str.Substring(index + 1);
                }
                this.Path = str;
            }
        }

        public virtual object Clone()
        {
            return base.MemberwiseClone();
        }

        public override bool Equals(object target)
        {
            URL url = null;
            if ((target != null) && (target.GetType() == typeof(URL)))
            {
                url = (URL) target;
            }
            if ((target != null) && (target.GetType() == typeof(string)))
            {
                url = new URL((string) target);
            }
            if (url == null)
            {
                return false;
            }
            if (url.Path != this.Path)
            {
                return false;
            }
            if (url.Scheme != this.Scheme)
            {
                return false;
            }
            if (url.HostName != this.HostName)
            {
                return false;
            }
            if (url.Port != this.Port)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            string str = ((this.HostName == null) || (this.HostName == "")) ? "localhost" : this.HostName;
            if (this.Port > 0)
            {
                return string.Format("{0}://{1}:{2}/{3}", new object[] { this.Scheme, str, this.Port, this.Path });
            }
            return string.Format("{0}://{1}/{2}", new object[] { this.Scheme, str, this.Path });
        }

        public string HostName
        {
            get
            {
                return this.m_hostName;
            }
            set
            {
                this.m_hostName = value;
            }
        }

        public string Path
        {
            get
            {
                return this.m_path;
            }
            set
            {
                this.m_path = value;
            }
        }

        public int Port
        {
            get
            {
                return this.m_port;
            }
            set
            {
                this.m_port = value;
            }
        }

        public string Scheme
        {
            get
            {
                return this.m_scheme;
            }
            set
            {
                this.m_scheme = value;
            }
        }
    }
}

