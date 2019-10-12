namespace Jund.OpcHelper.Opc.Dx
{
    using Opc;
    using Opc.Da;
    using System;
    using System.Runtime.InteropServices;

    public interface IServer : Opc.Da.IServer, Opc.IServer, IDisposable
    {
        GeneralResponse AddDXConnections(DXConnection[] connections);
        GeneralResponse AddSourceServers(SourceServer[] servers);
        GeneralResponse CopyDefaultSourceServerAttributes(bool configToStatus, Opc.Dx.ItemIdentifier[] servers);
        GeneralResponse CopyDXConnectionDefaultAttributes(bool configToStatus, string browsePath, DXConnection[] connectionMasks, bool recursive, out ResultID[] errors);
        GeneralResponse DeleteDXConnections(string browsePath, DXConnection[] connectionMasks, bool recursive, out ResultID[] errors);
        GeneralResponse DeleteSourceServers(Opc.Dx.ItemIdentifier[] servers);
        SourceServer[] GetSourceServers();
        GeneralResponse ModifyDXConnections(DXConnection[] connections);
        GeneralResponse ModifySourceServers(SourceServer[] servers);
        DXConnection[] QueryDXConnections(string browsePath, DXConnection[] connectionMasks, bool recursive, out ResultID[] errors);
        string ResetConfiguration(string configurationVersion);
        GeneralResponse UpdateDXConnections(string browsePath, DXConnection[] connectionMasks, bool recursive, DXConnection connectionDefinition, out ResultID[] errors);
    }
}

