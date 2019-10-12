namespace Jund.OpcHelper.OpcCom.Da.Wrapper
{
    using OpcRcw.Comn;
    using OpcRcw.Da;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using System.Threading;

    [CLSCompliant(false)]
    public class EnumString : OpcRcw.Comn.IEnumString, OpcRcw.Da.IEnumString
    {
        private int m_index = 0;
        private ArrayList m_strings = new ArrayList();

        internal EnumString(ICollection strings)
        {
            if (strings != null)
            {
                foreach (object obj2 in strings)
                {
                    this.m_strings.Add(obj2);
                }
            }
        }

        public void Clone(out OpcRcw.Comn.IEnumString ppenum)
        {
            EnumString str;
            Monitor.Enter(str = this);
            try
            {
                ppenum = new EnumString(this.m_strings);
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(str);
            }
        }

        public void Clone(out OpcRcw.Da.IEnumString ppenum)
        {
            EnumString str;
            Monitor.Enter(str = this);
            try
            {
                ppenum = new EnumString(this.m_strings);
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(str);
            }
        }

        public void RemoteNext(int celt, string[] rgelt, out int pceltFetched)
        {
            EnumString str;
            Monitor.Enter(str = this);
            try
            {
                if (rgelt == null)
                {
                    throw new ExternalException("E_INVALIDARG", -2147024809);
                }
                pceltFetched = 0;
                if (this.m_index < this.m_strings.Count)
                {
                    for (int i = 0; (i < (this.m_strings.Count - this.m_index)) && (i < rgelt.Length); i++)
                    {
                        rgelt[i] = (string) this.m_strings[this.m_index + i];
                        pceltFetched++;
                    }
                    this.m_index += pceltFetched;
                }
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(str);
            }
        }

        public void Reset()
        {
            EnumString str;
            Monitor.Enter(str = this);
            try
            {
                this.m_index = 0;
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(str);
            }
        }

        public void Skip(int celt)
        {
            EnumString str;
            Monitor.Enter(str = this);
            try
            {
                this.m_index += celt;
                if (this.m_index > this.m_strings.Count)
                {
                    this.m_index = this.m_strings.Count;
                }
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(str);
            }
        }
    }
}

