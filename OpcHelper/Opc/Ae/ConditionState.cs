namespace Jund.OpcHelper.Opc.Ae
{
    using System;

    [Flags]
    public enum ConditionState
    {
        Acknowledged = 4,
        Active = 2,
        Enabled = 1
    }
}

