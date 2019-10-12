using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Navigation;

namespace Jund.DesktopControlLibrary.GridControl.AccessoryControl
{
    public partial class FilterBoard : DevExpress.XtraEditors.XtraUserControl
    {
        public string FilterString;
        public FilterBoard()
        {
            InitializeComponent();

            this.Dock = DockStyle.Left;
        }

        public void AddFilterItem(AccordionControlElement group,string captain,string filterString)
        {
            AccordionControlElement item = new AccordionControlElement();
            item.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            item.Text = captain;
            item.Tag = filterString;
            item.Click += Item_Click;
        }
        public void AddCustomFilterItem(string captain, string filterString)
        {
            AccordionControlElement item = new AccordionControlElement();
            item.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            item.Text = captain;
            item.Tag = filterString;
            item.Click += Item_Click;
            this.accordionControlElement4.Elements.Add(item);
        }
        public void AddCustomDataGroupItem(string captain, string filterString)
        {
            AccordionControlElement item = new AccordionControlElement();
            item.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            item.Text = captain;
            item.Tag = filterString;
            item.Click += Item_Click;
            this.accordionControlElement5.Elements.Add(item);
        }
        public AccordionControlElement AddFilterGroup(string captain)
        {
            AccordionControlElement item = new AccordionControlElement();
            item.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            item.Text = captain;
            this.accordionControlElement1.Elements.Add(item);

            return item;
        }

        private void Item_Click(object sender, EventArgs e)
        {
            this.FilterString=(sender as AccordionControlElement).Tag.ToString();
        }
    }
}
