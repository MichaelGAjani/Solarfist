﻿namespace Jund.OpcHelper.OpcCom.Cpx
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct ResultIDs
    {
        public const int E_TYPE_CHANGED = -1073478649;
        public const int E_FILTER_DUPLICATE = -1073478648;
        public const int E_FILTER_INVALID = -1073478647;
        public const int E_FILTER_ERROR = -1073478646;
        public const int S_FILTER_NO_DATA = 0x4040b;
    }
}

