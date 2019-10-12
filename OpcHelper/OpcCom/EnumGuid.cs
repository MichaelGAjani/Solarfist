namespace Jund.OpcHelper.OpcCom
{
    using OpcRcw.Comn;
    using System;
    using System.Collections;

    public class EnumGuid
    {
        private IEnumGUID m_enumerator = null;

        public EnumGuid(object server)
        {
            this.m_enumerator = (IEnumGUID) server;
        }

        public EnumGuid Clone()
        {
            IEnumGUID ppenum = null;
            this.m_enumerator.Clone(out ppenum);
            return new EnumGuid(ppenum);
        }

        public Guid[] GetAll()
        {
            this.Reset();
            ArrayList list = new ArrayList();
            while (true)
            {
                Guid[] c = this.Next(1);
                if (c == null)
                {
                    break;
                }
                list.AddRange(c);
            }
            return (Guid[]) list.ToArray(typeof(Guid));
        }

        public object GetEnumerator()
        {
            return this.m_enumerator;
        }

        public Guid[] Next(int count)
        {
            Guid[] rgelt = new Guid[count];
            int pceltFetched = 0;
            try
            {
                this.m_enumerator.Next(rgelt.Length, rgelt, out pceltFetched);
            }
            catch (Exception)
            {
                return null;
            }
            if (pceltFetched == 0)
            {
                return null;
            }
            Guid[] guidArray2 = new Guid[pceltFetched];
            for (int i = 0; i < pceltFetched; i++)
            {
                guidArray2[i] = rgelt[i];
            }
            return guidArray2;
        }

        public void Release()
        {
            Interop.ReleaseServer(this.m_enumerator);
            this.m_enumerator = null;
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

