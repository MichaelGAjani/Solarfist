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
using Jund.DynamicCodeHelper;
using System.Reflection;

namespace Jund.DesktopControlLibrary.GridControl
{
    public partial class BaseViewControl : DevExpress.XtraEditors.XtraUserControl
    {
        public DynamicObjectHelper dynamicObject;
        public Control grid;
        public BaseViewControl()
        {
            InitializeComponent();
        }
        public void HideFilter() => this.filterBoard1.Visible = false;
        public void SimpleEditor() { this.splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both; this.splitContainerControl1.Horizontal = false; }
        public void BindData(DynamicObjectHelper dynamicObject)
        {
            this.dynamicObject = dynamicObject;
            this.splitContainerControl1.Panel1.Controls.Clear();
            this.splitContainerControl1.Panel1.Controls.Add(grid);
            grid.GetType().InvokeMember("BindData", BindingFlags.Default | BindingFlags.InvokeMethod, null, grid, new object[] { this.dynamicObject });
            this.dataDetailsBoard1.CreateLayoutEditor(this.dynamicObject);
        }
        public void RefreshView(object id)
        {

        }

        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnRemove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}
