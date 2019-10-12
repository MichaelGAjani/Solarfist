namespace Jund.OpcHelper.Opc.Ae
{
    using Opc;
    using System;
    using System.Runtime.InteropServices;

    public interface IServer : Opc.IServer, IDisposable
    {
        ResultID[] AcknowledgeCondition(string acknowledgerID, string comment, EventAcknowledgement[] conditions);
        BrowseElement[] Browse(string areaID, BrowseType browseType, string browseFilter);
        BrowseElement[] Browse(string areaID, BrowseType browseType, string browseFilter, int maxElements, out IBrowsePosition position);
        BrowseElement[] BrowseNext(int maxElements, ref IBrowsePosition position);
        ISubscription CreateSubscription(SubscriptionState state);
        ResultID[] DisableConditionByArea(string[] areas);
        ResultID[] DisableConditionBySource(string[] sources);
        ResultID[] EnableConditionByArea(string[] areas);
        ResultID[] EnableConditionBySource(string[] sources);
        Condition GetConditionState(string sourceName, string conditionName, int[] attributeIDs);
        EnabledStateResult[] GetEnableStateByArea(string[] areas);
        EnabledStateResult[] GetEnableStateBySource(string[] sources);
        ServerStatus GetStatus();
        int QueryAvailableFilters();
        string[] QueryConditionNames(int eventCategory);
        string[] QueryConditionNames(string sourceName);
        Opc.Ae.Attribute[] QueryEventAttributes(int eventCategory);
        Category[] QueryEventCategories(int eventType);
        string[] QuerySubConditionNames(string conditionName);
        ItemUrl[] TranslateToItemIDs(string sourceName, int eventCategory, string conditionName, string subConditionName, int[] attributeIDs);
    }
}

