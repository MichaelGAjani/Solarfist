namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void ReadValuesEventHandler(IRequest request, ItemValueCollection[] results);
}

