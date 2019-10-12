namespace Jund.OpcHelper.Opc.Cpx
{
    using System;

    public class BinaryWriter : BinaryStream
    {
        public byte[] Write(ComplexValue namedValue, TypeDictionary dictionary, string typeName)
        {
            if (namedValue == null)
            {
                throw new ArgumentNullException("namedValue");
            }
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            if (typeName == null)
            {
                throw new ArgumentNullException("typeName");
            }
            Context context = base.InitializeContext(null, dictionary, typeName);
            int num = this.WriteType(context, namedValue);
            if (num == 0)
            {
                throw new InvalidDataToWriteException("Could not write value into buffer.");
            }
            context.Buffer = new byte[num];
            if (this.WriteType(context, namedValue) != num)
            {
                throw new InvalidDataToWriteException("Could not write value into buffer.");
            }
            return context.Buffer;
        }

        private int WriteArrayField(Context context, FieldType field, int fieldIndex, ComplexValue[] fieldValues, object fieldValue)
        {
            int num6;
            int index = context.Index;
            Array array = null;
            if (!fieldValue.GetType().IsArray)
            {
                throw new InvalidDataToWriteException("Array field value is not an array type.");
            }
            array = (Array) fieldValue;
            byte bitOffset = 0;
            if (!field.ElementCountSpecified)
            {
                if (field.ElementCountRef == null)
                {
                    if (field.FieldTerminator != null)
                    {
                        foreach (object obj4 in array)
                        {
                            int num8 = this.WriteField(context, field, fieldIndex, fieldValues, obj4, ref bitOffset);
                            if ((num8 == 0) && (bitOffset == 0))
                            {
                                break;
                            }
                            context.Index += num8;
                        }
                        byte[] terminator = base.GetTerminator(context, field);
                        if (context.Buffer != null)
                        {
                            for (int i = 0; i < terminator.Length; i++)
                            {
                                context.Buffer[context.Index + i] = terminator[i];
                            }
                        }
                        context.Index += terminator.Length;
                    }
                    goto Label_0217;
                }
                num6 = 0;
                foreach (object obj3 in array)
                {
                    int num7 = this.WriteField(context, field, fieldIndex, fieldValues, obj3, ref bitOffset);
                    if ((num7 == 0) && (bitOffset == 0))
                    {
                        break;
                    }
                    context.Index += num7;
                    num6++;
                }
            }
            else
            {
                int num3 = 0;
                foreach (object obj2 in array)
                {
                    if (num3 == field.ElementCount)
                    {
                        break;
                    }
                    int num4 = this.WriteField(context, field, fieldIndex, fieldValues, obj2, ref bitOffset);
                    if ((num4 == 0) && (bitOffset == 0))
                    {
                        break;
                    }
                    context.Index += num4;
                    num3++;
                }
                while (num3 < field.ElementCount)
                {
                    int num5 = this.WriteField(context, field, fieldIndex, fieldValues, null, ref bitOffset);
                    if ((num5 == 0) && (bitOffset == 0))
                    {
                        break;
                    }
                    context.Index += num5;
                    num3++;
                }
                goto Label_0217;
            }
            this.WriteReference(context, field, fieldIndex, fieldValues, field.ElementCountRef, num6);
        Label_0217:
            if (bitOffset != 0)
            {
                context.Index++;
            }
            return (context.Index - index);
        }

        private int WriteField(Context context, FloatingPoint field, object fieldValue)
        {
            int num2;
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
            if (buffer == null)
            {
                return num;
            }
            if ((buffer.Length - context.Index) < num)
            {
                throw new InvalidDataToWriteException("Unexpected end of buffer.");
            }
            byte[] bytes = null;
            if (str == "IEEE-754")
            {
                switch (num)
                {
                    case 4:
                        bytes = BitConverter.GetBytes(Convert.ToSingle(fieldValue));
                        goto Label_00D1;

                    case 8:
                        bytes = BitConverter.GetBytes(Convert.ToDouble(fieldValue));
                        goto Label_00D1;
                }
                bytes = (byte[]) fieldValue;
            }
            else
            {
                bytes = (byte[]) fieldValue;
            }
        Label_00D1:
            num2 = 0;
            while (num2 < bytes.Length)
            {
                buffer[context.Index + num2] = bytes[num2];
                num2++;
            }
            return num;
        }

        private int WriteField(Context context, Integer field, object fieldValue)
        {
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
            if (buffer == null)
            {
                return length;
            }
            if ((buffer.Length - context.Index) < length)
            {
                throw new InvalidDataToWriteException("Unexpected end of buffer.");
            }
            byte[] bytes = null;
            if (!signed)
            {
                switch (length)
                {
                    case 1:
                        bytes = new byte[] { Convert.ToByte(fieldValue) };
                        goto Label_0205;

                    case 2:
                        bytes = BitConverter.GetBytes(Convert.ToUInt16(fieldValue));
                        goto Label_0205;

                    case 4:
                        bytes = BitConverter.GetBytes(Convert.ToUInt32(fieldValue));
                        goto Label_0205;

                    case 8:
                        bytes = BitConverter.GetBytes(Convert.ToUInt64(fieldValue));
                        goto Label_0205;
                }
            }
            else
            {
                switch (length)
                {
                    case 1:
                    {
                        bytes = new byte[1];
                        sbyte num2 = Convert.ToSByte(fieldValue);
                        if (num2 >= 0)
                        {
                            bytes[0] = (byte) num2;
                        }
                        else
                        {
                            bytes[0] = (byte) ((0xff + num2) + 1);
                        }
                        goto Label_0205;
                    }
                    case 2:
                        bytes = BitConverter.GetBytes(Convert.ToInt16(fieldValue));
                        goto Label_0205;

                    case 4:
                        bytes = BitConverter.GetBytes(Convert.ToInt32(fieldValue));
                        goto Label_0205;

                    case 8:
                        bytes = BitConverter.GetBytes(Convert.ToInt64(fieldValue));
                        goto Label_0205;
                }
                bytes = (byte[]) fieldValue;
                goto Label_0205;
            }
            bytes = (byte[]) fieldValue;
        Label_0205:
            if (context.BigEndian)
            {
                base.SwapBytes(bytes, 0, length);
            }
            for (int i = 0; i < bytes.Length; i++)
            {
                buffer[context.Index + i] = bytes[i];
            }
            return length;
        }

        private int WriteField(Context context, TypeReference field, object fieldValue)
        {
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
            if (fieldValue.GetType() != typeof(ComplexValue))
            {
                throw new InvalidDataToWriteException("Instance of type is not the correct type.");
            }
            return this.WriteType(context, (ComplexValue) fieldValue);
        }

        private int WriteField(Context context, BitString field, object fieldValue, ref byte bitOffset)
        {
            byte[] buffer = context.Buffer;
            int num = field.LengthSpecified ? field.Length : 8;
            int num2 = ((num % 8) == 0) ? (num / 8) : ((num / 8) + 1);
            if (fieldValue.GetType() != typeof(byte[]))
            {
                throw new InvalidDataToWriteException("Wrong data type to write.");
            }
            byte[] buffer2 = (byte[]) fieldValue;
            if (buffer != null)
            {
                if ((buffer.Length - context.Index) < num2)
                {
                    throw new InvalidDataToWriteException("Unexpected end of buffer.");
                }
                int num3 = num;
                byte num4 = (bitOffset == 0) ? ((byte) 0xff) : ((byte) ((((int) 0x80) >> (bitOffset - 1)) - 1));
                for (int i = 0; (num3 >= 0) && (i < num2); i++)
                {
                    byte[] buffer3;
                    IntPtr ptr;
                    (buffer3 = buffer)[(int) (ptr = (IntPtr) (context.Index + i))] = (byte) (buffer3[(int) ptr] + ((byte) (((num4 & ((1 << (num3 & 0x1f)) - 1)) & buffer2[i]) << bitOffset)));
                    if ((num3 + bitOffset) <= 8)
                    {
                        break;
                    }
                    if (((context.Index + i) + 1) >= buffer.Length)
                    {
                        throw new InvalidDataToWriteException("Unexpected end of buffer.");
                    }
                    (buffer3 = buffer)[(int) (ptr = (IntPtr) ((context.Index + i) + 1))] = (byte) (buffer3[(int) ptr] + ((byte) (((~num4 & ((1 << (num3 & 0x1f)) - 1)) & buffer2[i]) >> (8 - bitOffset))));
                    if (num3 <= 8)
                    {
                        break;
                    }
                    num3 -= 8;
                }
            }
            num2 = (num + bitOffset) / 8;
            bitOffset = (byte) ((num + bitOffset) % 8);
            return num2;
        }

        private int WriteField(Context context, CharString field, int fieldIndex, ComplexValue[] fieldValues, object fieldValue)
        {
            byte[] bytes = context.Buffer;
            int length = field.CharWidthSpecified ? field.CharWidth : context.CharWidth;
            int count = field.LengthSpecified ? field.Length : -1;
            if (field.GetType() == typeof(Ascii))
            {
                length = 1;
            }
            else if (field.GetType() == typeof(Unicode))
            {
                length = 2;
            }
            byte[] buffer2 = null;
            if (count == -1)
            {
                if (length > 2)
                {
                    if (fieldValue.GetType() != typeof(byte[]))
                    {
                        throw new InvalidDataToWriteException("Field value is not a byte array.");
                    }
                    buffer2 = (byte[]) fieldValue;
                    count = buffer2.Length / length;
                }
                else
                {
                    if (fieldValue.GetType() != typeof(string))
                    {
                        throw new InvalidDataToWriteException("Field value is not a string.");
                    }
                    string str = (string) fieldValue;
                    count = str.Length + 1;
                    if (length == 1)
                    {
                        count = 1;
                        foreach (char ch in str)
                        {
                            count++;
                            if (BitConverter.GetBytes(ch)[1] != 0)
                            {
                                count++;
                            }
                        }
                    }
                }
            }
            if (field.CharCountRef != null)
            {
                this.WriteReference(context, field, fieldIndex, fieldValues, field.CharCountRef, count);
            }
            if (bytes != null)
            {
                if (buffer2 == null)
                {
                    string str2 = (string) fieldValue;
                    buffer2 = new byte[length * count];
                    int num3 = 0;
                    for (int j = 0; j < str2.Length; j++)
                    {
                        if (num3 >= buffer2.Length)
                        {
                            break;
                        }
                        byte[] buffer4 = BitConverter.GetBytes(str2[j]);
                        buffer2[num3++] = buffer4[0];
                        if ((length == 2) || (buffer4[1] != 0))
                        {
                            buffer2[num3++] = buffer4[1];
                        }
                    }
                }
                if ((bytes.Length - context.Index) < buffer2.Length)
                {
                    throw new InvalidDataToWriteException("Unexpected end of buffer.");
                }
                for (int i = 0; i < buffer2.Length; i++)
                {
                    bytes[context.Index + i] = buffer2[i];
                }
                if (context.BigEndian && (length > 1))
                {
                    for (int k = 0; k < buffer2.Length; k += length)
                    {
                        base.SwapBytes(bytes, context.Index + k, length);
                    }
                }
            }
            return (count * length);
        }

        private int WriteField(Context context, FieldType field, int fieldIndex, ComplexValue[] fieldValues, object fieldValue, ref byte bitOffset)
        {
            Type type = field.GetType();
            if ((type == typeof(Integer)) || type.IsSubclassOf(typeof(Integer)))
            {
                return this.WriteField(context, (Integer) field, fieldValue);
            }
            if ((type == typeof(FloatingPoint)) || type.IsSubclassOf(typeof(FloatingPoint)))
            {
                return this.WriteField(context, (FloatingPoint) field, fieldValue);
            }
            if ((type == typeof(CharString)) || type.IsSubclassOf(typeof(CharString)))
            {
                return this.WriteField(context, (CharString) field, fieldIndex, fieldValues, fieldValue);
            }
            if ((type == typeof(BitString)) || type.IsSubclassOf(typeof(BitString)))
            {
                return this.WriteField(context, (BitString) field, fieldValue, ref bitOffset);
            }
            if ((type != typeof(TypeReference)) && !type.IsSubclassOf(typeof(TypeReference)))
            {
                throw new NotImplementedException("Fields of type '" + type.ToString() + "' are not implemented yet.");
            }
            return this.WriteField(context, (TypeReference) field, fieldValue);
        }

        private void WriteReference(Context context, FieldType field, int fieldIndex, ComplexValue[] fieldValues, string fieldName, int count)
        {
            ComplexValue value2 = null;
            if (fieldName.Length == 0)
            {
                if ((fieldIndex > 0) && ((fieldIndex - 1) < fieldValues.Length))
                {
                    value2 = fieldValues[fieldIndex - 1];
                }
            }
            else
            {
                for (int i = 0; i < fieldIndex; i++)
                {
                    value2 = fieldValues[i];
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
            if (context.Buffer == null)
            {
                value2.Value = count;
            }
            if (!count.Equals(value2.Value))
            {
                throw new InvalidDataToWriteException("Reference field value and the actual array length are not equal.");
            }
        }

        private int WriteType(Context context, ComplexValue namedValue)
        {
            TypeDescription description = context.Type;
            int index = context.Index;
            ComplexValue[] fieldValues = null;
            if ((namedValue.Value == null) || (namedValue.Value.GetType() != typeof(ComplexValue[])))
            {
                throw new InvalidDataToWriteException("Type instance does not contain field values.");
            }
            fieldValues = (ComplexValue[]) namedValue.Value;
            if (fieldValues.Length != description.Field.Length)
            {
                throw new InvalidDataToWriteException("Type instance does not contain the correct number of fields.");
            }
            byte bitOffset = 0;
            for (int i = 0; i < description.Field.Length; i++)
            {
                FieldType field = description.Field[i];
                ComplexValue fieldValue = fieldValues[i];
                if ((bitOffset != 0) && (field.GetType() != typeof(BitString)))
                {
                    context.Index++;
                    bitOffset = 0;
                }
                int num4 = 0;
                if (base.IsArrayField(field))
                {
                    num4 = this.WriteArrayField(context, field, i, fieldValues, fieldValue.Value);
                }
                else if (field.GetType() == typeof(TypeReference))
                {
                    num4 = this.WriteField(context, (TypeReference) field, fieldValue);
                }
                else
                {
                    num4 = this.WriteField(context, field, i, fieldValues, fieldValue.Value, ref bitOffset);
                }
                if ((num4 == 0) && (bitOffset == 0))
                {
                    throw new InvalidDataToWriteException("Could not write field '" + field.Name + "' in type '" + description.TypeID + "'.");
                }
                context.Index += num4;
            }
            if (bitOffset != 0)
            {
                context.Index++;
            }
            return (context.Index - index);
        }
    }
}

