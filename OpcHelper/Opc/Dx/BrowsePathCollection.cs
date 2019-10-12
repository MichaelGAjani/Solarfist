namespace Jund.OpcHelper.Opc.Dx
{
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class BrowsePathCollection : ArrayList
    {
        public BrowsePathCollection()
        {
        }

        public BrowsePathCollection(ICollection browsePaths)
        {
            if (browsePaths != null)
            {
                foreach (string str in browsePaths)
                {
                    this.Add(str);
                }
            }
        }

        public int Add(string browsePath)
        {
            return base.Add(browsePath);
        }

        public void Insert(int index, string browsePath)
        {
            if (browsePath == null)
            {
                throw new ArgumentNullException("browsePath");
            }
            base.Insert(index, browsePath);
        }

        public string[] ToArray()
        {
            return (string[]) this.ToArray(typeof(string));
        }

        public string this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = value;
            }
        }
    }
}

