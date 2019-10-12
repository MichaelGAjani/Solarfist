namespace Jund.OpcHelper.Opc
{
    using System;

    public interface IResult
    {
        string DiagnosticInfo { get; set; }

        Opc.ResultID ResultID { get; set; }
    }
}

