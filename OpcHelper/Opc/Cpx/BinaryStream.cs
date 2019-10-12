namespace Jund.OpcHelper.Opc.Cpx
{
    using System;

    public class BinaryStream
    {
        protected BinaryStream()
        {
        }

        internal byte[] GetTerminator(Context context, FieldType field)
        {
            if (field.FieldTerminator == null)
            {
                throw new InvalidSchemaException(field.Name + " is not a terminated group.");
            }
            string str = Convert.ToString(field.FieldTerminator).ToUpper();
            byte[] buffer = new byte[str.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(str.Substring(i * 2, 2), 0x10);
            }
            return buffer;
        }

        internal Context InitializeContext(byte[] buffer, TypeDictionary dictionary, string typeName)
        {
            Context context = new Context(buffer) {
                Dictionary = dictionary,
                Type = null,
                BigEndian = dictionary.DefaultBigEndian,
                CharWidth = dictionary.DefaultCharWidth,
                StringEncoding = dictionary.DefaultStringEncoding,
                FloatFormat = dictionary.DefaultFloatFormat
            };
            foreach (TypeDescription description in dictionary.TypeDescription)
            {
                if (description.TypeID == typeName)
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
                throw new InvalidSchemaException("Type '" + typeName + "' not found in dictionary.");
            }
            return context;
        }

        internal bool IsArrayField(FieldType field)
        {
            if (field.ElementCountSpecified)
            {
                if ((field.ElementCountRef != null) || (field.FieldTerminator != null))
                {
                    throw new InvalidSchemaException("Multiple array size attributes specified for field '" + field.Name + " '.");
                }
                return true;
            }
            if (field.ElementCountRef != null)
            {
                if (field.FieldTerminator != null)
                {
                    throw new InvalidSchemaException("Multiple array size attributes specified for field '" + field.Name + " '.");
                }
                return true;
            }
            return (field.FieldTerminator != null);
        }

        internal void SwapBytes(byte[] bytes, int index, int length)
        {
            for (int i = 0; i < (length / 2); i++)
            {
                byte num2 = bytes[((index + length) - 1) - i];
                bytes[((index + length) - 1) - i] = bytes[index + i];
                bytes[index + i] = num2;
            }
        }
    }
}

