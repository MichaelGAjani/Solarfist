namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), ComConversionLoss]
    public struct CommandDescription
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szDescription;
        public int bIsGlobal;
        public double dblExecutionTime;
        public int dwNoOfEventDefinitions;
        [ComConversionLoss]
        public IntPtr pEventDefinitions;
        public int dwNoOfStateDefinitions;
        [ComConversionLoss]
        public IntPtr pStateDefinitions;
        public int dwNoOfActionDefinitions;
        [ComConversionLoss]
        public IntPtr pActionDefinitions;
        public int dwNoOfTransitions;
        [ComConversionLoss]
        public IntPtr pTransitions;
        public int dwNoOfInArguments;
        [ComConversionLoss]
        public IntPtr pInArguments;
        public int dwNoOfOutArguments;
        [ComConversionLoss]
        public IntPtr pOutArguments;
        public int dwNoOfSupportedControls;
        [ComConversionLoss]
        public IntPtr pszSupportedControls;
        public int dwNoOfAndDependencies;
        [ComConversionLoss]
        public IntPtr pszAndDependencies;
        public int dwNoOfOrDependencies;
        [ComConversionLoss]
        public IntPtr pszOrDependencies;
        public int dwNoOfNotDependencies;
        [ComConversionLoss]
        public IntPtr pszNotDependencies;
    }
}

