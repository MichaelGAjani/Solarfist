namespace Jund.OpcHelper.Opc.Ae
{
    using Opc;
    using System;
    using System.Reflection;

    public class ItemUrlCollection : ReadOnlyCollection
    {
        public ItemUrlCollection() : base(new ItemUrl[0])
        {
        }

        public ItemUrlCollection(ItemUrl[] itemUrls) : base(itemUrls)
        {
        }

        public ItemUrl[] ToArray()
        {
            return (ItemUrl[]) Opc.Convert.Clone(this.Array);
        }

        public ItemUrl this[int index]
        {
            get
            {
                return (ItemUrl) this.Array.GetValue(index);
            }
        }
    }
}

