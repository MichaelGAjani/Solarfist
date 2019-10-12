namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;

    [Serializable]
    public class BrowsePosition : IBrowsePosition, IDisposable, ICloneable
    {
        public virtual object Clone()
        {
            return (BrowsePosition) base.MemberwiseClone();
        }

        public virtual void Dispose()
        {
        }
    }
}

