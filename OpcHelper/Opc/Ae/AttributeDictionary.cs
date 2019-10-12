namespace Jund.OpcHelper.Opc.Ae
{
    using Opc;
    using System;
    using System.Reflection;

    [Serializable]
    public class AttributeDictionary : WriteableDictionary
    {
        public AttributeDictionary() : base(null, typeof(int), typeof(Opc.Ae.AttributeCollection))
        {
        }

        public AttributeDictionary(int[] categoryIDs) : base(null, typeof(int), typeof(Opc.Ae.AttributeCollection))
        {
            for (int i = 0; i < categoryIDs.Length; i++)
            {
                this.Add(categoryIDs[i], null);
            }
        }

        public virtual void Add(int key, int[] value)
        {
            if (value != null)
            {
                base.Add(key, new Opc.Ae.AttributeCollection(value));
            }
            else
            {
                base.Add(key, new Opc.Ae.AttributeCollection());
            }
        }

        public Opc.Ae.AttributeCollection this[int categoryID]
        {
            get
            {
                return (Opc.Ae.AttributeCollection) base[categoryID];
            }
            set
            {
                if (value != null)
                {
                    base[categoryID] = value;
                }
                else
                {
                    base[categoryID] = new Opc.Ae.AttributeCollection();
                }
            }
        }
    }
}

