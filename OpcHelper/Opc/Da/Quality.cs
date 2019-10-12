namespace Jund.OpcHelper.Opc.Da
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Quality
    {
        private qualityBits m_qualityBits;
        private limitBits m_limitBits;
        private byte m_vendorBits;
        public static readonly Quality Good;
        public static readonly Quality Bad;
        public qualityBits QualityBits
        {
            get
            {
                return this.m_qualityBits;
            }
            set
            {
                this.m_qualityBits = value;
            }
        }
        public limitBits LimitBits
        {
            get
            {
                return this.m_limitBits;
            }
            set
            {
                this.m_limitBits = value;
            }
        }
        public byte VendorBits
        {
            get
            {
                return this.m_vendorBits;
            }
            set
            {
                this.m_vendorBits = value;
            }
        }
        public short GetCode()
        {
            ushort num = 0;
            num = (ushort) (num | ((ushort) this.QualityBits));
            num = (ushort) (num | ((ushort) this.LimitBits));
            num = (ushort) (num | ((ushort) (this.VendorBits << 8)));
            if (num > 0x7fff)
            {
                return (short) -(0x10000 - num);
            }
            return (short) num;
        }

        public void SetCode(short code)
        {
            this.m_qualityBits = ((qualityBits) code) & (qualityBits.goodLocalOverride | qualityBits.badWaitingForInitialData | qualityBits.badConfigurationError);
            this.m_limitBits = ((limitBits) code) & limitBits.constant;
            this.m_vendorBits = (byte) ((code & -253) >> 8);
        }

        public static bool operator ==(Quality a, Quality b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Quality a, Quality b)
        {
            return !a.Equals(b);
        }

        public Quality(qualityBits quality)
        {
            this.m_qualityBits = quality;
            this.m_limitBits = limitBits.none;
            this.m_vendorBits = 0;
        }

        public Quality(short code)
        {
            this.m_qualityBits = ((qualityBits) code) & (qualityBits.goodLocalOverride | qualityBits.badWaitingForInitialData | qualityBits.badConfigurationError);
            this.m_limitBits = ((limitBits) code) & limitBits.constant;
            this.m_vendorBits = (byte) ((code & -253) >> 8);
        }

        public override string ToString()
        {
            string str = this.QualityBits.ToString();
            if (this.LimitBits != limitBits.none)
            {
                str = str + string.Format("[{0}]", this.LimitBits.ToString());
            }
            if (this.VendorBits != 0)
            {
                str = str + string.Format(":{0,0:X}", this.VendorBits);
            }
            return str;
        }

        public override bool Equals(object target)
        {
            if ((target == null) || (target.GetType() != typeof(Quality)))
            {
                return false;
            }
            Quality quality = (Quality) target;
            if (this.QualityBits != quality.QualityBits)
            {
                return false;
            }
            if (this.LimitBits != quality.LimitBits)
            {
                return false;
            }
            if (this.VendorBits != quality.VendorBits)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return this.GetCode();
        }

        static Quality()
        {
            Good = new Quality(qualityBits.good);
            Bad = new Quality(qualityBits.bad);
        }
    }
}

