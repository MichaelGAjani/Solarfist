namespace Jund.OpcHelper.OpcCom.Hda
{
    using Opc.Da;
    using Opc.Hda;
    using OpcCom;
    using OpcRcw.Hda;
    using System;
    using System.Runtime.InteropServices;

    public class Interop
    {
        internal static FILETIME Convert(OPCHDA_FILETIME input)
        {
            return new FILETIME { dwLowDateTime = input.dwLowDateTime, dwHighDateTime = input.dwHighDateTime };
        }

        internal static OPCHDA_FILETIME Convert(FILETIME input)
        {
            return new OPCHDA_FILETIME { dwLowDateTime = input.dwLowDateTime, dwHighDateTime = input.dwHighDateTime };
        }

        internal static AnnotationValueCollection GetAnnotationValueCollection(OPCHDA_ANNOTATION input, bool deallocate)
        {
            AnnotationValueCollection values = new AnnotationValueCollection {
                ClientHandle = input.hClient
            };
            DateTime[] timeArray = OpcCom.Interop.GetFILETIMEs(ref input.ftTimeStamps, input.dwNumValues, deallocate);
            string[] strArray = OpcCom.Interop.GetUnicodeStrings(ref input.szAnnotation, input.dwNumValues, deallocate);
            DateTime[] timeArray2 = OpcCom.Interop.GetFILETIMEs(ref input.ftAnnotationTime, input.dwNumValues, deallocate);
            string[] strArray2 = OpcCom.Interop.GetUnicodeStrings(ref input.szUser, input.dwNumValues, deallocate);
            for (int i = 0; i < input.dwNumValues; i++)
            {
                AnnotationValue value2 = new AnnotationValue {
                    Timestamp = timeArray[i],
                    Annotation = strArray[i],
                    CreationTime = timeArray2[i],
                    User = strArray2[i]
                };
                values.Add(value2);
            }
            return values;
        }

        internal static AnnotationValueCollection GetAnnotationValueCollection(IntPtr pInput, bool deallocate)
        {
            AnnotationValueCollection annotationValueCollection = null;
            if (pInput != IntPtr.Zero)
            {
                annotationValueCollection = GetAnnotationValueCollection((OPCHDA_ANNOTATION) Marshal.PtrToStructure(pInput, typeof(OPCHDA_ANNOTATION)), deallocate);
                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCHDA_ANNOTATION));
                }
            }
            return annotationValueCollection;
        }

        internal static AnnotationValueCollection[] GetAnnotationValueCollections(ref IntPtr pInput, int count, bool deallocate)
        {
            AnnotationValueCollection[] valuesArray = null;
            if ((pInput != IntPtr.Zero) && (count > 0))
            {
                valuesArray = new AnnotationValueCollection[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    valuesArray[i] = GetAnnotationValueCollection(ptr, deallocate);
                    ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OPCHDA_ANNOTATION)));
                }
                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }
            return valuesArray;
        }

        internal static AttributeValueCollection GetAttributeValueCollection(OPCHDA_ATTRIBUTE input, bool deallocate)
        {
            AttributeValueCollection values = new AttributeValueCollection {
                AttributeID = input.dwAttributeID
            };
            object[] objArray = OpcCom.Interop.GetVARIANTs(ref input.vAttributeValues, input.dwNumValues, deallocate);
            DateTime[] timeArray = OpcCom.Interop.GetFILETIMEs(ref input.ftTimeStamps, input.dwNumValues, deallocate);
            for (int i = 0; i < input.dwNumValues; i++)
            {
                AttributeValue value2 = new AttributeValue {
                    Value = objArray[i],
                    Timestamp = timeArray[i]
                };
                values.Add(value2);
            }
            return values;
        }

        internal static AttributeValueCollection GetAttributeValueCollection(IntPtr pInput, bool deallocate)
        {
            AttributeValueCollection attributeValueCollection = null;
            if (pInput != IntPtr.Zero)
            {
                attributeValueCollection = GetAttributeValueCollection((OPCHDA_ATTRIBUTE) Marshal.PtrToStructure(pInput, typeof(OPCHDA_ATTRIBUTE)), deallocate);
                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCHDA_ATTRIBUTE));
                }
            }
            return attributeValueCollection;
        }

        internal static AttributeValueCollection[] GetAttributeValueCollections(ref IntPtr pInput, int count, bool deallocate)
        {
            AttributeValueCollection[] valuesArray = null;
            if ((pInput != IntPtr.Zero) && (count > 0))
            {
                valuesArray = new AttributeValueCollection[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    valuesArray[i] = GetAttributeValueCollection(ptr, deallocate);
                    ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OPCHDA_ATTRIBUTE)));
                }
                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }
            return valuesArray;
        }

        internal static OPCHDA_FILETIME GetFILETIME(decimal input)
        {
            return new OPCHDA_FILETIME { dwHighDateTime = (int) ((((long) (input * 10000000M)) & -4294967296L) >> 0x20), dwLowDateTime = (int) (((ulong) (input * 10000000M)) & 0xffffffffL) };
        }

        internal static OPCHDA_FILETIME[] GetFILETIMEs(DateTime[] input)
        {
            OPCHDA_FILETIME[] opchda_filetimeArray = null;
            if (input != null)
            {
                opchda_filetimeArray = new OPCHDA_FILETIME[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    opchda_filetimeArray[i] = Convert(OpcCom.Interop.GetFILETIME(input[i]));
                }
            }
            return opchda_filetimeArray;
        }

        internal static ItemValueCollection GetItemValueCollection(OPCHDA_ITEM input, bool deallocate)
        {
            ItemValueCollection values = new ItemValueCollection {
                ClientHandle = input.hClient,
                AggregateID = input.haAggregate
            };
            object[] objArray = OpcCom.Interop.GetVARIANTs(ref input.pvDataValues, input.dwCount, deallocate);
            DateTime[] timeArray = OpcCom.Interop.GetFILETIMEs(ref input.pftTimeStamps, input.dwCount, deallocate);
            int[] numArray = OpcCom.Interop.GetInt32s(ref input.pdwQualities, input.dwCount, deallocate);
            for (int i = 0; i < input.dwCount; i++)
            {
                Opc.Hda.ItemValue value2 = new Opc.Hda.ItemValue {
                    Value = objArray[i],
                    Timestamp = timeArray[i],
                    Quality = new Opc.Da.Quality((short) (numArray[i] & 0xffff)),
                    HistorianQuality = ((Opc.Hda.Quality) numArray[i]) & ((Opc.Hda.Quality) (unchecked((int) 0xffff0000L)))
                };
                values.Add(value2);
            }
            return values;
        }

        internal static ItemValueCollection GetItemValueCollection(IntPtr pInput, bool deallocate)
        {
            ItemValueCollection itemValueCollection = null;
            if (pInput != IntPtr.Zero)
            {
                itemValueCollection = GetItemValueCollection((OPCHDA_ITEM) Marshal.PtrToStructure(pInput, typeof(OPCHDA_ITEM)), deallocate);
                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCHDA_ITEM));
                }
            }
            return itemValueCollection;
        }

        internal static ItemValueCollection[] GetItemValueCollections(ref IntPtr pInput, int count, bool deallocate)
        {
            ItemValueCollection[] valuesArray = null;
            if ((pInput != IntPtr.Zero) && (count > 0))
            {
                valuesArray = new ItemValueCollection[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    valuesArray[i] = GetItemValueCollection(ptr, deallocate);
                    ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OPCHDA_ITEM)));
                }
                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }
            return valuesArray;
        }

        internal static ModifiedValueCollection GetModifiedValueCollection(OPCHDA_MODIFIEDITEM input, bool deallocate)
        {
            ModifiedValueCollection values = new ModifiedValueCollection {
                ClientHandle = input.hClient
            };
            object[] objArray = OpcCom.Interop.GetVARIANTs(ref input.pvDataValues, input.dwCount, deallocate);
            DateTime[] timeArray = OpcCom.Interop.GetFILETIMEs(ref input.pftTimeStamps, input.dwCount, deallocate);
            int[] numArray = OpcCom.Interop.GetInt32s(ref input.pdwQualities, input.dwCount, deallocate);
            DateTime[] timeArray2 = OpcCom.Interop.GetFILETIMEs(ref input.pftModificationTime, input.dwCount, deallocate);
            int[] numArray2 = OpcCom.Interop.GetInt32s(ref input.pEditType, input.dwCount, deallocate);
            string[] strArray = OpcCom.Interop.GetUnicodeStrings(ref input.szUser, input.dwCount, deallocate);
            for (int i = 0; i < input.dwCount; i++)
            {
                ModifiedValue value2 = new ModifiedValue {
                    Value = objArray[i],
                    Timestamp = timeArray[i],
                    Quality = new Opc.Da.Quality((short) (numArray[i] & 0xffff)),
                    HistorianQuality = ((Opc.Hda.Quality) numArray[i]) & ((Opc.Hda.Quality) (unchecked((int) 0xffff0000L))),
                    ModificationTime = timeArray2[i],
                    EditType = (EditType) numArray2[i],
                    User = strArray[i]
                };
                values.Add((Opc.Hda.ItemValue) value2);
            }
            return values;
        }

        internal static ModifiedValueCollection GetModifiedValueCollection(IntPtr pInput, bool deallocate)
        {
            ModifiedValueCollection modifiedValueCollection = null;
            if (pInput != IntPtr.Zero)
            {
                modifiedValueCollection = GetModifiedValueCollection((OPCHDA_MODIFIEDITEM) Marshal.PtrToStructure(pInput, typeof(OPCHDA_MODIFIEDITEM)), deallocate);
                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCHDA_MODIFIEDITEM));
                }
            }
            return modifiedValueCollection;
        }

        internal static ModifiedValueCollection[] GetModifiedValueCollections(ref IntPtr pInput, int count, bool deallocate)
        {
            ModifiedValueCollection[] valuesArray = null;
            if ((pInput != IntPtr.Zero) && (count > 0))
            {
                valuesArray = new ModifiedValueCollection[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    valuesArray[i] = GetModifiedValueCollection(ptr, deallocate);
                    ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OPCHDA_MODIFIEDITEM)));
                }
                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }
            return valuesArray;
        }

        internal static OPCHDA_TIME GetTime(Time input)
        {
            OPCHDA_TIME opchda_time = new OPCHDA_TIME();
            if (input != null)
            {
                opchda_time.ftTime = Convert(OpcCom.Interop.GetFILETIME(input.AbsoluteTime));
                opchda_time.szTime = input.IsRelative ? input.ToString() : "";
                opchda_time.bString = input.IsRelative ? 1 : 0;
                return opchda_time;
            }
            opchda_time.ftTime = Convert(OpcCom.Interop.GetFILETIME(DateTime.MinValue));
            opchda_time.szTime = "";
            opchda_time.bString = 1;
            return opchda_time;
        }
    }
}

