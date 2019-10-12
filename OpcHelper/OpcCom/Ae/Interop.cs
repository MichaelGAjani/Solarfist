namespace Jund.OpcHelper.OpcCom.Ae
{
    using Opc;
    using Opc.Ae;
    using Opc.Da;
    using OpcCom;
    using OpcRcw.Ae;
    using System;
    using System.Runtime.InteropServices;

    public class Interop
    {
        internal static System.Runtime.InteropServices.FILETIME Convert(OpcRcw.Ae.FILETIME input)
        {
            return new System.Runtime.InteropServices.FILETIME { dwLowDateTime = input.dwLowDateTime, dwHighDateTime = input.dwHighDateTime };
        }

        internal static OpcRcw.Ae.FILETIME Convert(System.Runtime.InteropServices.FILETIME input)
        {
            return new OpcRcw.Ae.FILETIME { dwLowDateTime = input.dwLowDateTime, dwHighDateTime = input.dwHighDateTime };
        }

        internal static OPCAEBROWSETYPE GetBrowseType(BrowseType input)
        {
            switch (input)
            {
                case BrowseType.Area:
                    return OPCAEBROWSETYPE.OPC_AREA;

                case BrowseType.Source:
                    return OPCAEBROWSETYPE.OPC_SOURCE;
            }
            return OPCAEBROWSETYPE.OPC_AREA;
        }

        internal static Condition[] GetConditions(ref IntPtr pInput, int count, bool deallocate)
        {
            Condition[] conditionArray = null;
            if ((pInput != IntPtr.Zero) && (count > 0))
            {
                conditionArray = new Condition[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    OPCCONDITIONSTATE opcconditionstate = (OPCCONDITIONSTATE) Marshal.PtrToStructure(ptr, typeof(OPCCONDITIONSTATE));
                    conditionArray[i] = new Condition();
                    conditionArray[i].State = opcconditionstate.wState;
                    conditionArray[i].Quality = new Quality(opcconditionstate.wQuality);
                    conditionArray[i].Comment = opcconditionstate.szComment;
                    conditionArray[i].AcknowledgerID = opcconditionstate.szAcknowledgerID;
                    conditionArray[i].CondLastActive = OpcCom.Interop.GetFILETIME(Convert(opcconditionstate.ftCondLastActive));
                    conditionArray[i].CondLastInactive = OpcCom.Interop.GetFILETIME(Convert(opcconditionstate.ftCondLastInactive));
                    conditionArray[i].SubCondLastActive = OpcCom.Interop.GetFILETIME(Convert(opcconditionstate.ftSubCondLastActive));
                    conditionArray[i].LastAckTime = OpcCom.Interop.GetFILETIME(Convert(opcconditionstate.ftLastAckTime));
                    conditionArray[i].ActiveSubCondition.Name = opcconditionstate.szActiveSubCondition;
                    conditionArray[i].ActiveSubCondition.Definition = opcconditionstate.szASCDefinition;
                    conditionArray[i].ActiveSubCondition.Severity = opcconditionstate.dwASCSeverity;
                    conditionArray[i].ActiveSubCondition.Description = opcconditionstate.szASCDescription;
                    string[] strArray = OpcCom.Interop.GetUnicodeStrings(ref opcconditionstate.pszSCNames, opcconditionstate.dwNumSCs, deallocate);
                    int[] numArray = OpcCom.Interop.GetInt32s(ref opcconditionstate.pdwSCSeverities, opcconditionstate.dwNumSCs, deallocate);
                    string[] strArray2 = OpcCom.Interop.GetUnicodeStrings(ref opcconditionstate.pszSCDefinitions, opcconditionstate.dwNumSCs, deallocate);
                    string[] strArray3 = OpcCom.Interop.GetUnicodeStrings(ref opcconditionstate.pszSCDescriptions, opcconditionstate.dwNumSCs, deallocate);
                    conditionArray[i].SubConditions.Clear();
                    if (opcconditionstate.dwNumSCs > 0)
                    {
                        for (int j = 0; j < strArray.Length; j++)
                        {
                            SubCondition condition = new SubCondition {
                                Name = strArray[j],
                                Severity = numArray[j],
                                Definition = strArray2[j],
                                Description = strArray3[j]
                            };
                            conditionArray[i].SubConditions.Add(condition);
                        }
                    }
                    object[] objArray = OpcCom.Interop.GetVARIANTs(ref opcconditionstate.pEventAttributes, opcconditionstate.dwNumEventAttrs, deallocate);
                    int[] numArray2 = OpcCom.Interop.GetInt32s(ref opcconditionstate.pErrors, opcconditionstate.dwNumEventAttrs, deallocate);
                    conditionArray[i].Attributes.Clear();
                    if (opcconditionstate.dwNumEventAttrs > 0)
                    {
                        for (int k = 0; k < objArray.Length; k++)
                        {
                            AttributeValue value2 = new AttributeValue {
                                ID = 0,
                                Value = objArray[k],
                                ResultID = GetResultID(numArray2[k])
                            };
                            conditionArray[i].Attributes.Add(value2);
                        }
                    }
                    if (deallocate)
                    {
                        Marshal.DestroyStructure(ptr, typeof(OPCCONDITIONSTATE));
                    }
                    ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OPCCONDITIONSTATE)));
                }
                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }
            return conditionArray;
        }

        internal static EventNotification GetEventNotification(ONEVENTSTRUCT input)
        {
            EventNotification notification = new EventNotification {
                SourceID = input.szSource,
                Time = OpcCom.Interop.GetFILETIME(Convert(input.ftTime)),
                Severity = input.dwSeverity,
                Message = input.szMessage,
                EventType = (EventType) input.dwEventType,
                EventCategory = input.dwEventCategory,
                ChangeMask = input.wChangeMask,
                NewState = input.wNewState,
                Quality = new Quality(input.wQuality),
                ConditionName = input.szConditionName,
                SubConditionName = input.szSubconditionName,
                AckRequired = input.bAckRequired != 0,
                ActiveTime = OpcCom.Interop.GetFILETIME(Convert(input.ftActiveTime)),
                Cookie = input.dwCookie,
                ActorID = input.szActorID
            };
            object[] attributes = OpcCom.Interop.GetVARIANTs(ref input.pEventAttributes, input.dwNumEventAttrs, false);
            notification.SetAttributes(attributes);
            return notification;
        }

        internal static EventNotification[] GetEventNotifications(ONEVENTSTRUCT[] input)
        {
            EventNotification[] notificationArray = null;
            if ((input != null) && (input.Length > 0))
            {
                notificationArray = new EventNotification[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    notificationArray[i] = GetEventNotification(input[i]);
                }
            }
            return notificationArray;
        }

        internal static ResultID GetResultID(int input)
        {
            if (input == -1073479165)
            {
                return ResultID.Ae.E_INVALIDBRANCHNAME;
            }
            return OpcCom.Interop.GetResultID(input);
        }

        internal static Opc.Ae.ServerStatus GetServerStatus(ref IntPtr pInput, bool deallocate)
        {
            Opc.Ae.ServerStatus status = null;
            if (pInput != IntPtr.Zero)
            {
                OPCEVENTSERVERSTATUS opceventserverstatus = (OPCEVENTSERVERSTATUS) Marshal.PtrToStructure(pInput, typeof(OPCEVENTSERVERSTATUS));
                status = new Opc.Ae.ServerStatus {
                    VendorInfo = opceventserverstatus.szVendorInfo,
                    ProductVersion = string.Format("{0}.{1}.{2}", opceventserverstatus.wMajorVersion, opceventserverstatus.wMinorVersion, opceventserverstatus.wBuildNumber),
                    ServerState = (ServerState) opceventserverstatus.dwServerState,
                    StatusInfo = null,
                    StartTime = OpcCom.Interop.GetFILETIME(Convert(opceventserverstatus.ftStartTime)),
                    CurrentTime = OpcCom.Interop.GetFILETIME(Convert(opceventserverstatus.ftCurrentTime)),
                    LastUpdateTime = OpcCom.Interop.GetFILETIME(Convert(opceventserverstatus.ftLastUpdateTime))
                };
                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCEVENTSERVERSTATUS));
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }
            return status;
        }
    }
}

