namespace Jund.OpcHelper.Opc.Da
{
    using System;

    [Serializable]
    public class BrowseFilters : ICloneable
    {
        private browseFilter m_browseFilter = browseFilter.all;
        private string m_elementNameFilter = null;
        private int m_maxElementsReturned = 0;
        private PropertyID[] m_propertyIDs = null;
        private bool m_returnAllProperties = false;
        private bool m_returnPropertyValues = false;
        private string m_vendorFilter = null;

        public virtual object Clone()
        {
            BrowseFilters filters = (BrowseFilters) base.MemberwiseClone();
            filters.PropertyIDs = (this.PropertyIDs != null) ? ((PropertyID[]) this.PropertyIDs.Clone()) : null;
            return filters;
        }

        public browseFilter BrowseFilter
        {
            get
            {
                return this.m_browseFilter;
            }
            set
            {
                this.m_browseFilter = value;
            }
        }

        public string ElementNameFilter
        {
            get
            {
                return this.m_elementNameFilter;
            }
            set
            {
                this.m_elementNameFilter = value;
            }
        }

        public int MaxElementsReturned
        {
            get
            {
                return this.m_maxElementsReturned;
            }
            set
            {
                this.m_maxElementsReturned = value;
            }
        }

        public PropertyID[] PropertyIDs
        {
            get
            {
                return this.m_propertyIDs;
            }
            set
            {
                this.m_propertyIDs = value;
            }
        }

        public bool ReturnAllProperties
        {
            get
            {
                return this.m_returnAllProperties;
            }
            set
            {
                this.m_returnAllProperties = value;
            }
        }

        public bool ReturnPropertyValues
        {
            get
            {
                return this.m_returnPropertyValues;
            }
            set
            {
                this.m_returnPropertyValues = value;
            }
        }

        public string VendorFilter
        {
            get
            {
                return this.m_vendorFilter;
            }
            set
            {
                this.m_vendorFilter = value;
            }
        }
    }
}

