namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Text;

    [Serializable]
    public class Time
    {
        private DateTime m_absoluteTime;
        private RelativeTime m_baseTime;
        private TimeOffsetCollection m_offsets;

        public Time()
        {
            this.m_absoluteTime = DateTime.MinValue;
            this.m_baseTime = RelativeTime.Now;
            this.m_offsets = new TimeOffsetCollection();
        }

        public Time(DateTime time)
        {
            this.m_absoluteTime = DateTime.MinValue;
            this.m_baseTime = RelativeTime.Now;
            this.m_offsets = new TimeOffsetCollection();
            this.AbsoluteTime = time;
        }

        public Time(string time)
        {
            this.m_absoluteTime = DateTime.MinValue;
            this.m_baseTime = RelativeTime.Now;
            this.m_offsets = new TimeOffsetCollection();
            Time time2 = Parse(time);
            this.m_absoluteTime = DateTime.MinValue;
            this.m_baseTime = time2.m_baseTime;
            this.m_offsets = time2.m_offsets;
        }

        private static string BaseTypeToString(RelativeTime baseTime)
        {
            switch (baseTime)
            {
                case RelativeTime.Now:
                    return "NOW";

                case RelativeTime.Second:
                    return "SECOND";

                case RelativeTime.Minute:
                    return "MINUTE";

                case RelativeTime.Hour:
                    return "HOUR";

                case RelativeTime.Day:
                    return "DAY";

                case RelativeTime.Week:
                    return "WEEK";

                case RelativeTime.Month:
                    return "MONTH";

                case RelativeTime.Year:
                    return "YEAR";
            }
            throw new ArgumentOutOfRangeException("baseTime", baseTime.ToString(), "Invalid value for relative base time.");
        }

        public static Time Parse(string buffer)
        {
            buffer = buffer.Trim();
            Time time = new Time();
            bool flag = false;
            foreach (RelativeTime time2 in Enum.GetValues(typeof(RelativeTime)))
            {
                string str = BaseTypeToString(time2);
                if (buffer.StartsWith(str))
                {
                    buffer = buffer.Substring(str.Length).Trim();
                    time.BaseTime = time2;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                time.AbsoluteTime = System.Convert.ToDateTime(buffer);
                return time;
            }
            if (buffer.Length > 0)
            {
                time.Offsets.Parse(buffer);
            }
            return time;
        }

        public DateTime ResolveTime()
        {
            if (!this.IsRelative)
            {
                return this.m_absoluteTime;
            }
            DateTime utcNow = DateTime.UtcNow;
            int year = utcNow.Year;
            int month = utcNow.Month;
            int day = utcNow.Day;
            int hour = utcNow.Hour;
            int minute = utcNow.Minute;
            int second = utcNow.Second;
            int millisecond = utcNow.Millisecond;
            switch (this.BaseTime)
            {
                case RelativeTime.Second:
                    millisecond = 0;
                    break;

                case RelativeTime.Minute:
                    second = 0;
                    millisecond = 0;
                    break;

                case RelativeTime.Hour:
                    minute = 0;
                    second = 0;
                    millisecond = 0;
                    break;

                case RelativeTime.Day:
                case RelativeTime.Week:
                    hour = 0;
                    minute = 0;
                    second = 0;
                    millisecond = 0;
                    break;

                case RelativeTime.Month:
                    day = 0;
                    hour = 0;
                    minute = 0;
                    second = 0;
                    millisecond = 0;
                    break;

                case RelativeTime.Year:
                    month = 0;
                    day = 0;
                    hour = 0;
                    minute = 0;
                    second = 0;
                    millisecond = 0;
                    break;
            }
            utcNow = new DateTime(year, month, day, hour, minute, second, millisecond);
            if ((this.BaseTime == RelativeTime.Week) && (utcNow.DayOfWeek != DayOfWeek.Sunday))
            {
                utcNow = utcNow.AddDays((double) (0-utcNow.DayOfWeek));
            }
            foreach (TimeOffset offset in this.Offsets)
            {
                switch (offset.Type)
                {
                    case RelativeTime.Second:
                        utcNow = utcNow.AddSeconds((double) offset.Value);
                        break;

                    case RelativeTime.Minute:
                        utcNow = utcNow.AddMinutes((double) offset.Value);
                        break;

                    case RelativeTime.Hour:
                        utcNow = utcNow.AddHours((double) offset.Value);
                        break;

                    case RelativeTime.Day:
                        utcNow = utcNow.AddDays((double) offset.Value);
                        break;

                    case RelativeTime.Week:
                        utcNow = utcNow.AddDays((double) (offset.Value * 7));
                        break;

                    case RelativeTime.Month:
                        utcNow = utcNow.AddMonths(offset.Value);
                        break;

                    case RelativeTime.Year:
                        utcNow = utcNow.AddYears(offset.Value);
                        break;
                }
            }
            return utcNow;
        }

        public override string ToString()
        {
            if (!this.IsRelative)
            {
                return Opc.Convert.ToString(this.m_absoluteTime);
            }
            StringBuilder builder = new StringBuilder(0x100);
            builder.Append(BaseTypeToString(this.BaseTime));
            builder.Append(this.Offsets.ToString());
            return builder.ToString();
        }

        public DateTime AbsoluteTime
        {
            get
            {
                return this.m_absoluteTime;
            }
            set
            {
                this.m_absoluteTime = value;
            }
        }

        public RelativeTime BaseTime
        {
            get
            {
                return this.m_baseTime;
            }
            set
            {
                this.m_baseTime = value;
            }
        }

        public bool IsRelative
        {
            get
            {
                return (this.m_absoluteTime == DateTime.MinValue);
            }
            set
            {
                this.m_absoluteTime = DateTime.MinValue;
            }
        }

        public TimeOffsetCollection Offsets
        {
            get
            {
                return this.m_offsets;
            }
        }
    }
}

