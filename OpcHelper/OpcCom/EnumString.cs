namespace Jund.OpcHelper.OpcCom
{
    using OpcRcw.Comn;
    using System;

    public class EnumString : IDisposable
    {
        private IEnumString m_enumerator = null;

        public EnumString(object enumerator)
        {
            this.m_enumerator = (IEnumString) enumerator;
        }

        public EnumString Clone()
        {
            IEnumString ppenum = null;
            this.m_enumerator.Clone(out ppenum);
            return new EnumString(ppenum);
        }

        public void Dispose()
        {
            Interop.ReleaseServer(this.m_enumerator);
            this.m_enumerator = null;
        }

        public string[] Next(int count)
        {
            try
            {
                string[] rgelt = new string[count];
                int pceltFetched = 0;
                this.m_enumerator.RemoteNext(rgelt.Length, rgelt, out pceltFetched);
                if (pceltFetched == 0)
                {
                    return new string[0];
                }
                if (pceltFetched == count)
                {
                    return rgelt;
                }
                string[] strArray2 = new string[pceltFetched];
                for (int i = 0; i < pceltFetched; i++)
                {
                    strArray2[i] = rgelt[i];
                }
                return strArray2;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Reset()
        {
            this.m_enumerator.Reset();
        }

        public void Skip(int count)
        {
            this.m_enumerator.Skip(count);
        }
    }
}

