namespace Jund.OpcHelper.Opc
{
    using System;

    public interface IDiscovery : IDisposable
    {
        string[] EnumerateHosts();
        Server[] GetAvailableServers(Specification specification);
        Server[] GetAvailableServers(Specification specification, string host, ConnectData connectData);
    }
}

