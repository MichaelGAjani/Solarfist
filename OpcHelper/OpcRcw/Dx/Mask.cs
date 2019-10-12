namespace Jund.OpcHelper.OpcRcw.Dx
{
    using System;

    public enum Mask
    {
        All = 0x7fffffff,
        BrowsePaths = 8,
        DeadBand = 0x100000,
        DefaultOverridden = 0x200,
        DefaultOverrideValue = 0x400,
        DefaultSourceItemConnected = 0x80,
        DefaultSourceServerConnected = 0x1000000,
        DefaultTargetItemConnected = 0x100,
        Description = 0x20,
        EnableSubstituteValue = 0x1000,
        ItemName = 2,
        ItemPath = 1,
        Keyword = 0x40,
        Name = 0x10,
        None = 0,
        ServerType = 0x400000,
        ServerURL = 0x800000,
        SourceItemName = 0x20000,
        SourceItemPath = 0x10000,
        SourceItemQueueSize = 0x40000,
        SourceServerName = 0x8000,
        SubstituteValue = 0x800,
        TargetItemName = 0x4000,
        TargetItemPath = 0x2000,
        UpdateRate = 0x80000,
        VendorData = 0x200000,
        Version = 4
    }
}

