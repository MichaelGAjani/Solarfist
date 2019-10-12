namespace Jund.OpcHelper.Opc.Cpx
{
    using Opc;
    using Opc.Da;
    using System;
    using System.Collections;

    public class ComplexTypeCache
    {
        private static Hashtable m_descriptions = new Hashtable();
        private static Hashtable m_dictionaries = new Hashtable();
        private static Hashtable m_items = new Hashtable();
        private static Opc.Da.Server m_server = null;

        public static ComplexItem GetComplexItem(BrowseElement element)
        {
            if (element == null)
            {
                return null;
            }
            lock (typeof(ComplexTypeCache))
            {
                return GetComplexItem(new ItemIdentifier(element.ItemPath, element.ItemName));
            }
        }

        public static ComplexItem GetComplexItem(ItemIdentifier itemID)
        {
            if (itemID == null)
            {
                return null;
            }
            lock (typeof(ComplexTypeCache))
            {
                ComplexItem item = new ComplexItem(itemID);
                try
                {
                    item.Update(m_server);
                }
                catch
                {
                    item = null;
                }
                m_items[itemID.Key] = item;
                return item;
            }
        }

        public static string GetTypeDescription(ItemIdentifier itemID)
        {
            if (itemID == null)
            {
                return null;
            }
            lock (typeof(ComplexTypeCache))
            {
                string str = null;
                ComplexItem complexItem = GetComplexItem(itemID);
                if (complexItem != null)
                {
                    str = (string) m_descriptions[complexItem.TypeItemID.Key];
                    if (str != null)
                    {
                        return str;
                    }
                    m_descriptions[complexItem.TypeItemID.Key] = str = complexItem.GetTypeDescription(m_server);
                }
                return str;
            }
        }

        public static string GetTypeDictionary(ItemIdentifier itemID)
        {
            if (itemID == null)
            {
                return null;
            }
            lock (typeof(ComplexTypeCache))
            {
                string typeDictionary = (string) m_dictionaries[itemID.Key];
                if (typeDictionary == null)
                {
                    ComplexItem complexItem = GetComplexItem(itemID);
                    if (complexItem != null)
                    {
                        typeDictionary = complexItem.GetTypeDictionary(m_server);
                    }
                }
                return typeDictionary;
            }
        }

        public static Opc.Da.Server Server
        {
            get
            {
                lock (typeof(ComplexTypeCache))
                {
                    return m_server;
                }
            }
            set
            {
                lock (typeof(ComplexTypeCache))
                {
                    m_server = value;
                    m_items.Clear();
                    m_dictionaries.Clear();
                    m_descriptions.Clear();
                }
            }
        }
    }
}

