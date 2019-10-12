namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Xml;

    public class Convert
    {
        public static object ChangeType(object source, System.Type newType)
        {
            if (source == null)
            {
                if ((newType != null) && newType.IsValueType)
                {
                    return Activator.CreateInstance(newType);
                }
                return null;
            }
            if (((newType == null) || (newType == typeof(object))) || (newType == source.GetType()))
            {
                return Clone(source);
            }
            System.Type type = source.GetType();
            if (type.IsArray && newType.IsArray)
            {
                ArrayList list = new ArrayList(((Array) source).Length);
                foreach (object obj2 in (Array) source)
                {
                    list.Add(ChangeType(obj2, newType.GetElementType()));
                }
                return list.ToArray(newType.GetElementType());
            }
            if (!type.IsArray && newType.IsArray)
            {
                ArrayList list2 = new ArrayList(1);
                list2.Add(ChangeType(source, newType.GetElementType()));
                return list2.ToArray(newType.GetElementType());
            }
            if ((type.IsArray && !newType.IsArray) && (((Array) source).Length == 1))
            {
                return System.Convert.ChangeType(((Array) source).GetValue(0), newType);
            }
            if (type.IsArray && (newType == typeof(string)))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("{ ");
                int num = 0;
                foreach (object obj3 in (Array) source)
                {
                    builder.AppendFormat("{0}", ChangeType(obj3, typeof(string)));
                    num++;
                    if (num < ((Array) source).Length)
                    {
                        builder.Append(" | ");
                    }
                }
                builder.Append(" }");
                return builder.ToString();
            }
            if (newType.IsEnum)
            {
                if (type != typeof(string))
                {
                    return Enum.ToObject(newType, source);
                }
                if ((((string) source).Length > 0) && char.IsDigit((string) source, 0))
                {
                    return Enum.ToObject(newType, System.Convert.ToInt32(source));
                }
                return Enum.Parse(newType, (string) source);
            }
            if (newType != typeof(bool))
            {
                return System.Convert.ChangeType(source, newType);
            }
            if (typeof(string).IsInstanceOfType(source))
            {
                string s = (string) source;
                if ((s.Length > 0) && (((s[0] == '+') || (s[0] == '-')) || char.IsDigit(s, 0)))
                {
                    return System.Convert.ToBoolean(System.Convert.ToInt32(source));
                }
            }
            return System.Convert.ToBoolean(source);
        }

        public static object Clone(object source)
        {
            object obj2;
            if (source == null)
            {
                return null;
            }
            if (source.GetType().IsValueType)
            {
                return source;
            }
            if (source.GetType().IsArray || (source.GetType() == typeof(Array)))
            {
                Array array = (Array) ((Array) source).Clone();
                for (int i = 0; i < array.Length; i++)
                {
                    array.SetValue(Clone(array.GetValue(i)), i);
                }
                return array;
            }
            try
            {
                obj2 = ((ICloneable) source).Clone();
            }
            catch
            {
                throw new NotSupportedException("Object cannot be cloned.");
            }
            return obj2;
        }

        public static bool Compare(object a, object b)
        {
            if ((a == null) || (b == null))
            {
                return ((a == null) && (b == null));
            }
            System.Type type = a.GetType();
            System.Type type2 = b.GetType();
            if (type != type2)
            {
                return false;
            }
            if (!type.IsArray || !type2.IsArray)
            {
                return a.Equals(b);
            }
            Array array = (Array) a;
            Array array2 = (Array) b;
            if (array.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array.Length; i++)
            {
                if (!Compare(array.GetValue(i), array2.GetValue(i)))
                {
                    return false;
                }
            }
            return true;
        }

        private static char ConvertCase(char c, bool caseSensitive)
        {
            if (!caseSensitive)
            {
                return char.ToUpper(c);
            }
            return c;
        }

        public static bool IsEmpty(Array array)
        {
            if (array != null)
            {
                return (array.Length == 0);
            }
            return true;
        }

        public static bool IsEmpty(string target)
        {
            if (target != null)
            {
                return (target.Length == 0);
            }
            return true;
        }

        public static bool IsValid(Array array)
        {
            return ((array != null) && (array.Length > 0));
        }

        public static bool IsValid(string target)
        {
            return ((target != null) && (target.Length > 0));
        }

        public static bool Match(string target, string pattern, bool caseSensitive)
        {
            if ((pattern != null) && (pattern.Length != 0))
            {
                if ((target == null) || (target.Length == 0))
                {
                    return false;
                }
                if (caseSensitive)
                {
                    if (target == pattern)
                    {
                        return true;
                    }
                }
                else if (target.ToLower() == pattern.ToLower())
                {
                    return true;
                }
                int startIndex = 0;
                int num2 = 0;
                while ((num2 < target.Length) && (startIndex < pattern.Length))
                {
                    char ch;
                    char ch3;
                    char ch2 = ConvertCase(pattern[startIndex++], caseSensitive);
                    if (startIndex > pattern.Length)
                    {
                        return (num2 >= target.Length);
                    }
                    switch (ch2)
                    {
                        case '?':
                        {
                            if (num2 >= target.Length)
                            {
                                return false;
                            }
                            if ((startIndex >= pattern.Length) && (num2 < (target.Length - 1)))
                            {
                                return false;
                            }
                            num2++;
                            continue;
                        }
                        case '[':
                        {
                            ch = ConvertCase(target[num2++], caseSensitive);
                            if (num2 > target.Length)
                            {
                                return false;
                            }
                            ch3 = '\0';
                            if (pattern[startIndex] != '!')
                            {
                                goto Label_01A5;
                            }
                            startIndex++;
                            ch2 = ConvertCase(pattern[startIndex++], caseSensitive);
                            while (startIndex < pattern.Length)
                            {
                                switch (ch2)
                                {
                                    case ']':
                                    {
                                        continue;
                                    }
                                    case '-':
                                        ch2 = ConvertCase(pattern[startIndex], caseSensitive);
                                        if ((startIndex > pattern.Length) || (ch2 == ']'))
                                        {
                                            return false;
                                        }
                                        if ((ch >= ch3) && (ch <= ch2))
                                        {
                                            return false;
                                        }
                                        break;
                                }
                                ch3 = ch2;
                                if (ch == ch2)
                                {
                                    return false;
                                }
                                ch2 = ConvertCase(pattern[startIndex++], caseSensitive);
                            }
                            continue;
                        }
                        case '#':
                            ch = target[num2++];
                            if (char.IsDigit(ch))
                            {
                                continue;
                            }
                            return false;

                        case '*':
                            goto Label_00BC;

                        default:
                            goto Label_0242;
                    }
                Label_009E:
                    if (Match(target.Substring(num2++), pattern.Substring(startIndex), caseSensitive))
                    {
                        return true;
                    }
                Label_00BC:
                    if (num2 < target.Length)
                    {
                        goto Label_009E;
                    }
                    return Match(target, pattern.Substring(startIndex), caseSensitive);
                Label_01A5:
                    ch2 = ConvertCase(pattern[startIndex++], caseSensitive);
                    while (startIndex < pattern.Length)
                    {
                        if (ch2 == ']')
                        {
                            return false;
                        }
                        if (ch2 == '-')
                        {
                            ch2 = ConvertCase(pattern[startIndex], caseSensitive);
                            if ((startIndex > pattern.Length) || (ch2 == ']'))
                            {
                                return false;
                            }
                            if ((ch >= ch3) && (ch <= ch2))
                            {
                                break;
                            }
                        }
                        ch3 = ch2;
                        if (ch == ch2)
                        {
                            break;
                        }
                        ch2 = ConvertCase(pattern[startIndex++], caseSensitive);
                    }
                    while ((startIndex < pattern.Length) && (ch2 != ']'))
                    {
                        ch2 = pattern[startIndex++];
                    }
                    continue;
                Label_0242:
                    if (ConvertCase(target[num2++], caseSensitive) != ch2)
                    {
                        return false;
                    }
                    if ((startIndex >= pattern.Length) && (num2 < (target.Length - 1)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static string ToString(object source)
        {
            if (source == null)
            {
                return "";
            }
            System.Type type = source.GetType();
            if (type == typeof(DateTime))
            {
                if (((DateTime) source) == DateTime.MinValue)
                {
                    return string.Empty;
                }
                DateTime time = (DateTime) source;
                if (time.Millisecond > 0)
                {
                    return time.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                return time.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (type == typeof(XmlQualifiedName))
            {
                return ((XmlQualifiedName) source).Name;
            }
            if (type.FullName == "System.RuntimeType")
            {
                return ((System.Type) source).Name;
            }
            if (type == typeof(byte[]))
            {
                byte[] buffer = (byte[]) source;
                StringBuilder builder = new StringBuilder(buffer.Length * 3);
                for (int i = 0; i < buffer.Length; i++)
                {
                    builder.Append(buffer[i].ToString("X2"));
                    builder.Append(" ");
                }
                return builder.ToString();
            }
            if (type.IsArray)
            {
                return string.Format("{0}[{1}]", type.GetElementType().Name, ((Array) source).Length);
            }
            if (type == typeof(Array))
            {
                return string.Format("Object[{0}]", ((Array) source).Length);
            }
            return source.ToString();
        }
    }
}

