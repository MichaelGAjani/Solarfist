namespace Jund.OpcHelper.Opc.Dx
{
    using System;

    [Serializable]
    public class SourceServer : ItemIdentifier
    {
        private bool m_defaultConnected;
        private bool m_defaultConnectedSpecified;
        private string m_description;
        private string m_name;
        private string m_serverType;
        private string m_serverURL;

        public SourceServer()
        {
            this.m_name = null;
            this.m_description = null;
            this.m_serverType = null;
            this.m_serverURL = null;
            this.m_defaultConnected = false;
            this.m_defaultConnectedSpecified = false;
        }

        public SourceServer(ItemIdentifier item) : base(item)
        {
            this.m_name = null;
            this.m_description = null;
            this.m_serverType = null;
            this.m_serverURL = null;
            this.m_defaultConnected = false;
            this.m_defaultConnectedSpecified = false;
        }

        public SourceServer(SourceServer server) : base((ItemIdentifier) server)
        {
            this.m_name = null;
            this.m_description = null;
            this.m_serverType = null;
            this.m_serverURL = null;
            this.m_defaultConnected = false;
            this.m_defaultConnectedSpecified = false;
            if (server != null)
            {
                this.m_name = server.m_name;
                this.m_description = server.m_description;
                this.m_serverType = server.m_serverType;
                this.m_serverURL = server.m_serverURL;
                this.m_defaultConnected = server.m_defaultConnected;
                this.m_defaultConnectedSpecified = server.m_defaultConnectedSpecified;
            }
        }

        public bool DefaultConnected
        {
            get
            {
                return this.m_defaultConnected;
            }
            set
            {
                this.m_defaultConnected = value;
            }
        }

        public bool DefaultConnectedSpecified
        {
            get
            {
                return this.m_defaultConnectedSpecified;
            }
            set
            {
                this.m_defaultConnectedSpecified = value;
            }
        }

        public string Description
        {
            get
            {
                return this.m_description;
            }
            set
            {
                this.m_description = value;
            }
        }

        public string Name
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

        public string ServerType
        {
            get
            {
                return this.m_serverType;
            }
            set
            {
                this.m_serverType = value;
            }
        }

        public string ServerURL
        {
            get
            {
                return this.m_serverURL;
            }
            set
            {
                this.m_serverURL = value;
            }
        }
    }
}

