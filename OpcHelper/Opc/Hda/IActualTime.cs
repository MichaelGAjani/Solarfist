namespace Jund.OpcHelper.Opc.Hda
{
    using System;

    public interface IActualTime
    {
        DateTime EndTime { get; set; }

        DateTime StartTime { get; set; }
    }
}

