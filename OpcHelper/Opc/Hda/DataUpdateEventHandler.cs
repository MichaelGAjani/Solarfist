namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void DataUpdateEventHandler(IRequest request, ItemValueCollection[] results);
}

