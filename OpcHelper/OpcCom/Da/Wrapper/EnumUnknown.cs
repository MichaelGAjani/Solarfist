namespace Jund.OpcHelper.OpcCom.Da.Wrapper
{
    using OpcRcw.Comn;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using System.Threading;

    [CLSCompliant(false)]
    public class EnumUnknown : IEnumUnknown
    {
        private int m_index = 0;
        private ArrayList m_unknowns = new ArrayList();

        internal EnumUnknown(ICollection unknowns)
        {
            if (unknowns != null)
            {
                foreach (object obj2 in unknowns)
                {
                    this.m_unknowns.Add(obj2);
                }
            }
        }

        public void Clone(out IEnumUnknown ppenum)
        {
            EnumUnknown unknown;
            Monitor.Enter(unknown = this);
            try
            {
                ppenum = new EnumUnknown(this.m_unknowns);
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(unknown);
            }
        }

        public void RemoteNext(int celt, object[] rgelt, out int pceltFetched)
        {
            EnumUnknown unknown;
            Monitor.Enter(unknown = this);
            try
            {
                if (rgelt == null)
                {
                    throw new ExternalException("E_INVALIDARG", -2147024809);
                }
                pceltFetched = 0;
                if (this.m_index < this.m_unknowns.Count)
                {
                    for (int i = 0; (i < (this.m_unknowns.Count - this.m_index)) && (i < rgelt.Length); i++)
                    {
                        rgelt[i] = this.m_unknowns[this.m_index + i];
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
                Monitor.Exit(unknown);
            }
        }

        public void Reset()
        {
            EnumUnknown unknown;
            Monitor.Enter(unknown = this);
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
                Monitor.Exit(unknown);
            }
        }

        public void Skip(int celt)
        {
            EnumUnknown unknown;
            Monitor.Enter(unknown = this);
            try
            {
                this.m_index += celt;
                if (this.m_index > this.m_unknowns.Count)
                {
                    this.m_index = this.m_unknowns.Count;
                }
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(unknown);
            }
        }
    }
}

