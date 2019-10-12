namespace Jund.OpcHelper.Opc
{
    using System;

    public interface IFactory : IDisposable
    {
        IServer CreateInstance(URL url, ConnectData connectData);
    }
}

