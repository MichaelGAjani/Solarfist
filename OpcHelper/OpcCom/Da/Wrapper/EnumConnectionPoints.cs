namespace Jund.OpcHelper.OpcCom.Da.Wrapper
{
    using OpcRcw.Comn;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using System.Threading;

    [CLSCompliant(false)]
    public class EnumConnectionPoints : IEnumConnectionPoints
    {
        private ArrayList m_connectionPoints = new ArrayList();
        private int m_index = 0;

        internal EnumConnectionPoints(ICollection connectionPoints)
        {
            if (connectionPoints != null)
            {
                foreach (IConnectionPoint point in connectionPoints)
                {
                    this.m_connectionPoints.Add(point);
                }
            }
        }

        public void Clone(out IEnumConnectionPoints ppenum)
        {
            EnumConnectionPoints points;
            Monitor.Enter(points = this);
            try
            {
                ppenum = new EnumConnectionPoints(this.m_connectionPoints);
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(points);
            }
        }

        public void RemoteNext(int cConnections, IConnectionPoint[] ppCP, out int pcFetched)
        {
            EnumConnectionPoints points;
            Monitor.Enter(points = this);
            try
            {
                if ((ppCP == null) || (ppCP.Length < cConnections))
                {
                    throw new ExternalException("E_INVALIDARG", -2147024809);
                }
                pcFetched = 0;
                if (this.m_index < this.m_connectionPoints.Count)
                {
                    for (int i = 0; (i < (this.m_connectionPoints.Count - this.m_index)) && (i < cConnections); i++)
                    {
                        ppCP[i] = (IConnectionPoint) this.m_connectionPoints[this.m_index + i];
                        pcFetched++;
                    }
                    this.m_index += pcFetched;
                }
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(points);
            }
        }

        public void Reset()
        {
            EnumConnectionPoints points;
            Monitor.Enter(points = this);
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
                Monitor.Exit(points);
            }
        }

        public void Skip(int cConnections)
        {
            EnumConnectionPoints points;
            Monitor.Enter(points = this);
            try
            {
                this.m_index += cConnections;
                if (this.m_index > this.m_connectionPoints.Count)
                {
                    this.m_index = this.m_connectionPoints.Count;
                }
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(points);
            }
        }
    }
}

