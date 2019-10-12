namespace Jund.OpcHelper.Opc.Hda
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Text;

    [Serializable]
    public class TimeOffsetCollection : ArrayList
    {
        public int Add(TimeOffset value)
        {
            return this.Add(value);
        }

        public int Add(int value, RelativeTime type)
        {
            TimeOffset offset = new TimeOffset {
                Value = value,
                Type = type
            };
            return base.Add(offset);
        }

        public bool Contains(TimeOffset value)
        {
            return this.Contains(value);
        }

        public void CopyTo(TimeOffset[] array, int index)
        {
            this.CopyTo(array, index);
        }

        private static TimeOffset CreateOffset(bool positive, int magnitude, string units)
        {
            foreach (RelativeTime time in Enum.GetValues(typeof(RelativeTime)))
            {
                if ((time != RelativeTime.Now) && (units == TimeOffset.OffsetTypeToString(time)))
                {
                    return new TimeOffset { Value = positive ? magnitude : -magnitude, Type = time };
                }
            }
            throw new ArgumentOutOfRangeException("units", units, "String is not a valid offset time type.");
        }

        public int IndexOf(TimeOffset value)
        {
            return this.IndexOf(value);
        }

        public void Insert(int index, TimeOffset value)
        {
            this.Insert(index, value);
        }

        public void Parse(string buffer)
        {
            this.Clear();
            bool positive = true;
            int magnitude = 0;
            string units = "";
            int num2 = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                if ((buffer[i] == '+') || (buffer[i] == '-'))
                {
                    if (num2 == 3)
                    {
                        this.Add(CreateOffset(positive, magnitude, units));
                        magnitude = 0;
                        units = "";
                        num2 = 0;
                    }
                    if (num2 != 0)
                    {
                        throw new FormatException("Unexpected token encountered while parsing relative time string.");
                    }
                    positive = buffer[i] == '+';
                    num2 = 1;
                }
                else if (char.IsDigit(buffer, i))
                {
                    if (num2 == 3)
                    {
                        this.Add(CreateOffset(positive, magnitude, units));
                        magnitude = 0;
                        units = "";
                        num2 = 0;
                    }
                    if (((num2 != 0) && (num2 != 1)) && (num2 != 2))
                    {
                        throw new FormatException("Unexpected token encountered while parsing relative time string.");
                    }
                    magnitude *= 10;
                    magnitude += Convert.ToInt32((int) (buffer[i] - '0'));
                    num2 = 2;
                }
                else if (!char.IsWhiteSpace(buffer, i))
                {
                    if ((num2 != 2) && (num2 != 3))
                    {
                        throw new FormatException("Unexpected token encountered while parsing relative time string.");
                    }
                    units = units + buffer[i];
                    num2 = 3;
                }
            }
            if (num2 == 3)
            {
                this.Add(CreateOffset(positive, magnitude, units));
                num2 = 0;
            }
            if (num2 != 0)
            {
                throw new FormatException("Unexpected end of string encountered while parsing relative time string.");
            }
        }

        public void Remove(TimeOffset value)
        {
            this.Remove(value);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(0x100);
            foreach (TimeOffset offset in this)
            {
                if (offset.Value >= 0)
                {
                    builder.Append("+");
                }
                builder.AppendFormat("{0}", offset.Value);
                builder.Append(TimeOffset.OffsetTypeToString(offset.Type));
            }
            return builder.ToString();
        }

        public TimeOffset this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = value;
            }
        }
    }
}

