namespace Jund.OpcHelper.Opc.Ae
{
    using Opc;
    using System;
    using System.Reflection;

    [Serializable]
    public class AttributeCollection : WriteableCollection
    {
        internal AttributeCollection() : base(null, typeof(int))
        {
        }

        internal AttributeCollection(int[] attributeIDs) : base(attributeIDs, typeof(int))
        {
        }

        public int[] ToArray()
        {
            return (int[]) this.Array.ToArray(typeof(int));
        }

        public int this[int index]
        {
            get
            {
                return (int) this.Array[index];
            }
        }
    }
}

