namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class Factory : IFactory, IDisposable, ISerializable, ICloneable
    {
        private System.Type m_systemType;
        private bool m_useRemoting;

        protected Factory(SerializationInfo info, StreamingContext context)
        {
            this.m_systemType = null;
            this.m_useRemoting = false;
            this.m_useRemoting = info.GetBoolean("UseRemoting");
            this.m_systemType = (System.Type) info.GetValue("SystemType", typeof(System.Type));
        }

        public Factory(System.Type systemType, bool useRemoting)
        {
            this.m_systemType = null;
            this.m_useRemoting = false;
            this.m_systemType = systemType;
            this.m_useRemoting = useRemoting;
        }

        public virtual object Clone()
        {
            return base.MemberwiseClone();
        }

        public virtual IServer CreateInstance(URL url, ConnectData connectData)
        {
            if (!this.m_useRemoting)
            {
                return (IServer) Activator.CreateInstance(this.m_systemType, new object[] { url, connectData });
            }
            return (IServer) Activator.GetObject(this.m_systemType, url.ToString());
        }

        public virtual void Dispose()
        {
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UseRemoting", this.m_useRemoting);
            info.AddValue("SystemType", this.m_systemType);
        }

        protected System.Type SystemType
        {
            get
            {
                return this.m_systemType;
            }
            set
            {
                this.m_systemType = value;
            }
        }

        protected bool UseRemoting
        {
            get
            {
                return this.m_useRemoting;
            }
            set
            {
                this.m_useRemoting = value;
            }
        }

        private class Names
        {
            internal const string SYSTEM_TYPE = "SystemType";
            internal const string USE_REMOTING = "UseRemoting";
        }
    }
}

