namespace Jund.OpcHelper.OpcCom.Hda
{
    using Opc;
    using Opc.Hda;
    using OpcCom;
    using OpcRcw.Hda;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public class Browser : IBrowser, IDisposable
    {
        private const int BLOCK_SIZE = 10;
        private IOPCHDA_Browser m_browser = null;
        private BrowseFilterCollection m_filters = new BrowseFilterCollection();
        private OpcCom.Hda.Server m_server = null;

        internal Browser(OpcCom.Hda.Server server, IOPCHDA_Browser browser, BrowseFilter[] filters, ResultID[] results)
        {
            if (browser == null)
            {
                throw new ArgumentNullException("browser");
            }
            this.m_server = server;
            this.m_browser = browser;
            if (filters != null)
            {
                ArrayList collection = new ArrayList();
                for (int i = 0; i < filters.Length; i++)
                {
                    if (results[i].Succeeded())
                    {
                        collection.Add(filters[i]);
                    }
                }
                this.m_filters = new BrowseFilterCollection(collection);
            }
        }

        public BrowseElement[] Browse(ItemIdentifier itemID)
        {
            IBrowsePosition position = null;
            BrowseElement[] elementArray = this.Browse(itemID, 0, out position);
            if (position != null)
            {
                position.Dispose();
            }
            return elementArray;
        }

        public BrowseElement[] Browse(ItemIdentifier itemID, int maxElements, out IBrowsePosition position)
        {
            position = null;
            if (maxElements <= 0)
            {
                maxElements = 0x7fffffff;
            }
            lock (this)
            {
                string szString = ((itemID != null) && (itemID.ItemName != null)) ? itemID.ItemName : "";
                try
                {
                    this.m_browser.ChangeBrowsePosition(OPCHDA_BROWSEDIRECTION.OPCHDA_BROWSE_DIRECT, szString);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Browser.ChangeBrowsePosition", exception);
                }
                EnumString enumerator = this.GetEnumerator(true);
                ArrayList list = this.FetchElements(enumerator, maxElements, true);
                if (list.Count >= maxElements)
                {
                    position = new OpcCom.Hda.BrowsePosition(szString, enumerator, false);
                    return (BrowseElement[]) list.ToArray(typeof(BrowseElement));
                }
                enumerator.Dispose();
                enumerator = this.GetEnumerator(false);
                ArrayList c = this.FetchElements(enumerator, maxElements - list.Count, false);
                if (c != null)
                {
                    list.AddRange(c);
                }
                if (list.Count >= maxElements)
                {
                    position = new OpcCom.Hda.BrowsePosition(szString, enumerator, true);
                    return (BrowseElement[]) list.ToArray(typeof(BrowseElement));
                }
                enumerator.Dispose();
                return (BrowseElement[]) list.ToArray(typeof(BrowseElement));
            }
        }

        public BrowseElement[] BrowseNext(int maxElements, ref IBrowsePosition position)
        {
            if ((position == null) || (position.GetType() != typeof(OpcCom.Hda.BrowsePosition)))
            {
                throw new ArgumentException("Not a valid browse position object.", "position");
            }
            if (maxElements <= 0)
            {
                maxElements = 0x7fffffff;
            }
            lock (this)
            {
                OpcCom.Hda.BrowsePosition position2 = (OpcCom.Hda.BrowsePosition) position;
                ArrayList list = new ArrayList();
                if (!position2.FetchingItems)
                {
                    list = this.FetchElements(position2.Enumerator, maxElements, true);
                    if (list.Count >= maxElements)
                    {
                        return (BrowseElement[]) list.ToArray(typeof(BrowseElement));
                    }
                    position2.Enumerator.Dispose();
                    position2.Enumerator = null;
                    position2.FetchingItems = true;
                    try
                    {
                        this.m_browser.ChangeBrowsePosition(OPCHDA_BROWSEDIRECTION.OPCHDA_BROWSE_DIRECT, position2.BranchPath);
                    }
                    catch (Exception exception)
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_Browser.ChangeBrowsePosition", exception);
                    }
                    position2.Enumerator = this.GetEnumerator(false);
                }
                ArrayList c = this.FetchElements(position2.Enumerator, maxElements - list.Count, false);
                if (c != null)
                {
                    list.AddRange(c);
                }
                if (list.Count < maxElements)
                {
                    position.Dispose();
                    position = null;
                }
                return (BrowseElement[]) list.ToArray(typeof(BrowseElement));
            }
        }

        public virtual void Dispose()
        {
            lock (this)
            {
                this.m_server = null;
                OpcCom.Interop.ReleaseServer(this.m_browser);
                this.m_browser = null;
            }
        }

        private ArrayList FetchElements(EnumString enumerator, int maxElements, bool isBranch)
        {
            ArrayList list = new ArrayList();
            while (list.Count < maxElements)
            {
                int count = 10;
                if ((list.Count + count) > maxElements)
                {
                    count = maxElements - list.Count;
                }
                string[] strArray = enumerator.Next(count);
                if ((strArray == null) || (strArray.Length == 0))
                {
                    break;
                }
                foreach (string str in strArray)
                {
                    BrowseElement element = new BrowseElement {
                        Name = str
                    };
                    try
                    {
                        string pszItemID = null;
                        this.m_browser.GetItemID(str, out pszItemID);
                        element.ItemName = pszItemID;
                        element.ItemPath = null;
                        element.HasChildren = isBranch;
                    }
                    catch
                    {
                    }
                    list.Add(element);
                }
            }
            IdentifiedResult[] resultArray = this.m_server.ValidateItems((ItemIdentifier[]) list.ToArray(typeof(ItemIdentifier)));
            if (resultArray != null)
            {
                for (int i = 0; i < resultArray.Length; i++)
                {
                    if (resultArray[i].ResultID.Succeeded())
                    {
                        ((BrowseElement) list[i]).IsItem = true;
                    }
                }
            }
            return list;
        }

        private EnumString GetEnumerator(bool isBranch)
        {
            EnumString str2;
            try
            {
                OPCHDA_BROWSETYPE dwBrowseType = isBranch ? OPCHDA_BROWSETYPE.OPCHDA_BRANCH : OPCHDA_BROWSETYPE.OPCHDA_LEAF;
                IEnumString ppIEnumString = null;
                this.m_browser.GetEnum(dwBrowseType, out ppIEnumString);
                str2 = new EnumString(ppIEnumString);
            }
            catch (Exception exception)
            {
                throw OpcCom.Interop.CreateException("IOPCHDA_Browser.GetEnum", exception);
            }
            return str2;
        }

        public BrowseFilterCollection Filters
        {
            get
            {
                lock (this)
                {
                    return (BrowseFilterCollection) this.m_filters.Clone();
                }
            }
        }
    }
}

