namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;
    using System.Runtime.CompilerServices;

    public delegate ItemPropertyCollection[] GetPropertiesAsyncDelegate(ItemIdentifier[] itemIDs, PropertyID[] propertyIDs, string itemPath, bool returnValues);
}

