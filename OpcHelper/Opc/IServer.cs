namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Runtime.CompilerServices;

    public interface IServer : IDisposable
    {
        event ServerShutdownEventHandler ServerShutdown;

        string GetErrorText(string locale, ResultID resultID);
        string GetLocale();
        string[] GetSupportedLocales();
        string SetLocale(string locale);
    }
}

