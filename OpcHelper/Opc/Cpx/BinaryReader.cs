namespace Jund.OpcHelper.Opc.Cpx
{
    using Opc;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public class BinaryReader : BinaryStream
    {
        public ComplexValue Read(byte[] buffer, TypeDictionary dictionary, string typeName)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            if (typeName == null)
            {
                throw new ArgumentNullException("typeName");
            }
            Context context = base.InitializeContext(buffer, dictionary, typeName);
            ComplexValue complexValue = null;
            if (this.ReadType(context, out complexValue) == 0)
            {
                throw new InvalidSchemaException("Type '" + typeName + "' not found in dictionary.");
            }
            return complexValue;
        }

        private int ReadArrayField(Context context, FieldType field, int fieldIndex, ArrayList fieldValues, out object fieldValue)
        {
            fieldValue = null;
            int index = context.Index;
            ArrayList list = new ArrayList();
            object obj2 = null;
            byte bitOffset = 0;
            if (field.ElementCountSpecified)
            {
                for (int i = 0; i < field.ElementCount; i++)
                {
                    int num4 = this.ReadField(context, field, fieldIndex, fieldValues, out obj2, ref bitOffset);
                    if ((num4 == 0) && (bitOffset == 0))
                    {
                        break;
                    }
                    list.Add(obj2);
                    context.Index += num4;
                }
            }
            else if (field.ElementCountRef != null)
            {
                int num5 = this.ReadReference(context, field, fieldIndex, fieldValues, field.ElementCountRef);
                for (int j = 0; j < num5; j++)
                {
                    int num7 = this.ReadField(context, field, fieldIndex, fieldValues, out obj2, ref bitOffset);
                    if ((num7 == 0) && (bitOffset == 0))
                    {
                        break;
                    }
                    list.Add(obj2);
                    context.Index += num7;
                }
            }
            else if (field.FieldTerminator != null)
            {
                byte[] terminator = base.GetTerminator(context, field);
                while (context.Index < context.Buffer.Length)
                {
                    bool flag = true;
                    for (int k = 0; k < terminator.Length; k++)
                    {
                        if (terminator[k] != context.Buffer[context.Index + k])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        context.Index += terminator.Length;
                        break;
                    }
                    int num9 = this.ReadField(context, field, fieldIndex, fieldValues, out obj2, ref bitOffset);
                    if ((num9 == 0) && (bitOffset == 0))
                    {
                        break;
                    }
                    list.Add(obj2);
                    context.Index += num9;
                }
            }
            if (bitOffset != 0)
            {
                context.Index++;
            }
            System.Type type = null;
            foreach (object obj3 in list)
            {
                if (type == null)
                {
                    type = obj3.GetType();
                }
                else if (type != obj3.GetType())
                {
                    type = typeof(object);
                    break;
                }
            }
            fieldValue = list.ToArray(type);
            return (context.Index - index);
        }

        private int ReadField(Context context, FloatingPoint field, out object fieldValue)
        {
            fieldValue = null;
            byte[] buffer = context.Buffer;
            int num = field.LengthSpecified ? field.Length : 4;
            string str = (field.FloatFormat != null) ? field.FloatFormat : context.FloatFormat;
            if (field.GetType() == typeof(Opc.Cpx.Single))
            {
                num = 4;
                str = "IEEE-754";
            }
            else if (field.GetType() == typeof(Opc.Cpx.Double))
            {
                num = 8;
                str = "IEEE-754";
            }
            if ((buffer.Length - context.Index) < num)
            {
                throw new InvalidDataInBufferException("Unexpected end of buffer.");
            }
            byte[] buffer2 = new byte[num];
            for (int i = 0; i < num; i++)
            {
                buffer2[i] = buffer[context.Index + i];
            }
            if (str == "IEEE-754")
            {
                switch (num)
                {
                    case 4:
                        fieldValue = BitConverter.ToSingle(buffer2, 0);
                        return num;

                    case 8:
                        fieldValue = BitConverter.ToDouble(buffer2, 0);
                        return num;
                }
                fieldValue = buffer2;
                return num;
            }
            fieldValue = buffer2;
            return num;
        }

        private int ReadField(Context context, Integer field, out object fieldValue)
        {
            fieldValue = null;
            byte[] buffer = context.Buffer;
            int length = field.LengthSpecified ? field.Length : 4;
            bool signed = field.Signed;
            if (field.GetType() == typeof(Int8))
            {
                length = 1;
                signed = true;
            }
            else if (field.GetType() == typeof(Opc.Cpx.Int16))
            {
                length = 2;
                signed = true;
            }
            else if (field.GetType() == typeof(Opc.Cpx.Int32))
            {
                length = 4;
                signed = true;
            }
            else if (field.GetType() == typeof(Opc.Cpx.Int64))
            {
                length = 8;
                signed = true;
            }
            else if (field.GetType() == typeof(UInt8))
            {
                length = 1;
                signed = false;
            }
            else if (field.GetType() == typeof(Opc.Cpx.UInt16))
            {
                length = 2;
                signed = false;
            }
            else if (field.GetType() == typeof(Opc.Cpx.UInt32))
            {
                length = 4;
                signed = false;
            }
            else if (field.GetType() == typeof(Opc.Cpx.UInt64))
            {
                length = 8;
                signed = false;
            }
            if ((buffer.Length - context.Index) < length)
            {
                throw new InvalidDataInBufferException("Unexpected end of buffer.");
            }
            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                bytes[i] = buffer[context.Index + i];
            }
            if (context.BigEndian)
            {
                base.SwapBytes(bytes, 0, length);
            }
            if (!signed)
            {
                switch (length)
                {
                    case 1:
                        fieldValue = bytes[0];
                        return length;

                    case 2:
                        fieldValue = BitConverter.ToUInt16(bytes, 0);
                        return length;

                    case 4:
                        fieldValue = BitConverter.ToUInt32(bytes, 0);
                        return length;

                    case 8:
                        fieldValue = BitConverter.ToUInt64(bytes, 0);
                        return length;
                }
            }
            else
            {
                switch (length)
                {
                    case 1:
                        if (bytes[0] >= 0x80)
                        {
                            fieldValue = (sbyte) -bytes[0];
                            return length;
                        }
                        fieldValue = (sbyte) bytes[0];
                        return length;

                    case 2:
                        fieldValue = BitConverter.ToInt16(bytes, 0);
                        return length;

                    case 4:
                        fieldValue = BitConverter.ToInt32(bytes, 0);
                        return length;

                    case 8:
                        fieldValue = BitConverter.ToInt64(bytes, 0);
                        return length;
                }
                fieldValue = bytes;
                return length;
            }
            fieldValue = bytes;
            return length;
        }

        private int ReadField(Context context, TypeReference field, out object fieldValue)
        {
            fieldValue = null;
            foreach (TypeDescription description in context.Dictionary.TypeDescription)
            {
                if (description.TypeID == field.TypeID)
                {
                    context.Type = description;
                    if (description.DefaultBigEndianSpecified)
                    {
                        context.BigEndian = description.DefaultBigEndian;
                    }
                    if (description.DefaultCharWidthSpecified)
                    {
                        context.CharWidth = description.DefaultCharWidth;
                    }
                    if (description.DefaultStringEncoding != null)
                    {
                        context.StringEncoding = description.DefaultStringEncoding;
                    }
                    if (description.DefaultFloatFormat != null)
                    {
                        context.FloatFormat = description.DefaultFloatFormat;
                    }
                    break;
                }
            }
            if (context.Type == null)
            {
                throw new InvalidSchemaException("Reference type '" + field.TypeID + "' not found.");
            }
            ComplexValue complexValue = null;
            int num = this.ReadType(context, out complexValue);
            if (num == 0)
            {
                fieldValue = null;
            }
            fieldValue = complexValue;
            return num;
        }

        private int ReadField(Context context, BitString field, out object fieldValue, ref byte bitOffset)
        {
            fieldValue = null;
            byte[] buffer = context.Buffer;
            int num = field.LengthSpecified ? field.Length : 8;
            int num2 = ((num % 8) == 0) ? (num / 8) : ((num / 8) + 1);
            if ((buffer.Length - context.Index) < num2)
            {
                throw new InvalidDataInBufferException("Unexpected end of buffer.");
            }
            byte[] buffer2 = new byte[num2];
            int num3 = num;
            byte num4 = (byte) ~((((int) 1) << bitOffset) - 1);
            for (int i = 0; (num3 >= 0) && (i < num2); i++)
            {
                byte[] buffer3;
                IntPtr ptr;
                buffer2[i] = (byte) ((num4 & buffer[context.Index + i]) >> bitOffset);
                if ((num3 + bitOffset) <= 8)
                {
                    (buffer3 = buffer2)[(int) (ptr = (IntPtr) i)] = (byte) (buffer3[(int) ptr] & ((byte) ((((int) 1) << num3) - 1)));
                    break;
                }
                if (((context.Index + i) + 1) >= buffer.Length)
                {
                    throw new InvalidDataInBufferException("Unexpected end of buffer.");
                }
                (buffer3 = buffer2)[(int) (ptr = (IntPtr) i)] = (byte) (buffer3[(int) ptr] + ((byte) ((~num4 & buffer[(context.Index + i) + 1]) << (8 - bitOffset))));
                if (num3 <= 8)
                {
                    (buffer3 = buffer2)[(int) (ptr = (IntPtr) i)] = (byte) (buffer3[(int) ptr] & ((byte) ((((int) 1) << num3) - 1)));
                    break;
                }
                num3 -= 8;
            }
            fieldValue = buffer2;
            num2 = (num + bitOffset) / 8;
            bitOffset = (byte) ((num + bitOffset) % 8);
            return num2;
        }

        private int ReadField(Context context, CharString field, int fieldIndex, ArrayList fieldValues, out object fieldValue)
        {
            fieldValue = null;
            byte[] buffer = context.Buffer;
            int length = field.CharWidthSpecified ? field.CharWidth : context.CharWidth;
            int num2 = field.LengthSpecified ? field.Length : -1;
            if (field.GetType() == typeof(Ascii))
            {
                length = 1;
            }
            else if (field.GetType() == typeof(Unicode))
            {
                length = 2;
            }
            if (field.CharCountRef != null)
            {
                num2 = this.ReadReference(context, field, fieldIndex, fieldValues, field.CharCountRef);
            }
            if (num2 == -1)
            {
                num2 = 0;
                for (int i = context.Index; i < ((context.Buffer.Length - length) + 1); i += length)
                {
                    num2++;
                    bool flag = true;
                    for (int j = 0; j < length; j++)
                    {
                        if (context.Buffer[i + j] != 0)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                }
            }
            if ((buffer.Length - context.Index) < (length * num2))
            {
                throw new InvalidDataInBufferException("Unexpected end of buffer.");
            }
            if (length > 2)
            {
                byte[] bytes = new byte[num2 * length];
                for (int k = 0; k < (num2 * length); k++)
                {
                    bytes[k] = buffer[context.Index + k];
                }
                if (context.BigEndian)
                {
                    for (int m = 0; m < bytes.Length; m += length)
                    {
                        base.SwapBytes(bytes, 0, length);
                    }
                }
                fieldValue = bytes;
            }
            else
            {
                char[] chArray = new char[num2];
                for (int n = 0; n < num2; n++)
                {
                    if (length == 1)
                    {
                        chArray[n] = System.Convert.ToChar(buffer[context.Index + n]);
                    }
                    else
                    {
                        byte[] buffer3 = new byte[] { buffer[context.Index + (2 * n)], buffer[(context.Index + (2 * n)) + 1] };
                        if (context.BigEndian)
                        {
                            base.SwapBytes(buffer3, 0, 2);
                        }
                        chArray[n] = BitConverter.ToChar(buffer3, 0);
                    }
                }
                fieldValue = new string(chArray).TrimEnd(new char[1]);
            }
            return (num2 * length);
        }

        private int ReadField(Context context, FieldType field, int fieldIndex, ArrayList fieldValues, out object fieldValue, ref byte bitOffset)
        {
            fieldValue = null;
            System.Type type = field.GetType();
            if ((type == typeof(Integer)) || type.IsSubclassOf(typeof(Integer)))
            {
                return this.ReadField(context, (Integer) field, out fieldValue);
            }
            if ((type == typeof(FloatingPoint)) || type.IsSubclassOf(typeof(FloatingPoint)))
            {
                return this.ReadField(context, (FloatingPoint) field, out fieldValue);
            }
            if ((type == typeof(CharString)) || type.IsSubclassOf(typeof(CharString)))
            {
                return this.ReadField(context, (CharString) field, fieldIndex, fieldValues, out fieldValue);
            }
            if ((type == typeof(BitString)) || type.IsSubclassOf(typeof(BitString)))
            {
                return this.ReadField(context, (BitString) field, out fieldValue, ref bitOffset);
            }
            if (type != typeof(TypeReference))
            {
                throw new NotImplementedException("Fields of type '" + type.ToString() + "' are not implemented yet.");
            }
            return this.ReadField(context, (TypeReference) field, out fieldValue);
        }

        private int ReadReference(Context context, FieldType field, int fieldIndex, ArrayList fieldValues, string fieldName)
        {
            ComplexValue value2 = null;
            if (fieldName.Length == 0)
            {
                if ((fieldIndex > 0) && ((fieldIndex - 1) < fieldValues.Count))
                {
                    value2 = (ComplexValue) fieldValues[fieldIndex - 1];
                }
            }
            else
            {
                for (int i = 0; i < fieldIndex; i++)
                {
                    value2 = (ComplexValue) fieldValues[i];
                    if (value2.Name == fieldName)
                    {
                        break;
                    }
                    value2 = null;
                }
            }
            if (value2 == null)
            {
                throw new InvalidSchemaException("Referenced field not found (" + fieldName + ").");
            }
            return System.Convert.ToInt32(value2.Value);
        }

        private int ReadType(Context context, out ComplexValue complexValue)
        {
            complexValue = null;
            TypeDescription description = context.Type;
            int index = context.Index;
            byte bitOffset = 0;
            ArrayList fieldValues = new ArrayList();
            for (int i = 0; i < description.Field.Length; i++)
            {
                FieldType field = description.Field[i];
                ComplexValue value2 = new ComplexValue {
                    Name = ((field.Name != null) && (field.Name.Length != 0)) ? field.Name : ("[" + i.ToString() + "]"),
                    Type = null,
                    Value = null
                };
                if ((bitOffset != 0) && (field.GetType() != typeof(BitString)))
                {
                    context.Index++;
                    bitOffset = 0;
                }
                int num4 = 0;
                if (base.IsArrayField(field))
                {
                    num4 = this.ReadArrayField(context, field, i, fieldValues, out value2.Value);
                }
                else if (field.GetType() == typeof(TypeReference))
                {
                    object fieldValue = null;
                    num4 = this.ReadField(context, (TypeReference) field, out fieldValue);
                    value2.Name = field.Name;
                    value2.Type = ((ComplexValue) fieldValue).Type;
                    value2.Value = ((ComplexValue) fieldValue).Value;
                }
                else
                {
                    num4 = this.ReadField(context, field, i, fieldValues, out value2.Value, ref bitOffset);
                }
                if ((num4 == 0) && (bitOffset == 0))
                {
                    throw new InvalidDataInBufferException("Could not read field '" + field.Name + "' in type '" + description.TypeID + "'.");
                }
                context.Index += num4;
                if (value2.Type == null)
                {
                    value2.Type = Opc.Convert.ToString(value2.Value.GetType());
                }
                fieldValues.Add(value2);
            }
            if (bitOffset != 0)
            {
                context.Index++;
            }
            complexValue = new ComplexValue();
            complexValue.Name = description.TypeID;
            complexValue.Type = description.TypeID;
            complexValue.Value = (ComplexValue[]) fieldValues.ToArray(typeof(ComplexValue));
            return (context.Index - index);
        }
    }
}

