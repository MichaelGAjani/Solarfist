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

namespace Jund.DesktopControlLibrary.GridControl.VariantControl
{
    public partial class VerticalGrid : DevExpress.XtraEditors.XtraUserControl
    {
        public string FilterString { get => this.vGridControl1.ActiveFilterString; set => this.vGridControl1.ActiveFilterString = value; }
        public bool ShowAllValuesInFilterPopup { get => this.vGridControl1.OptionsFilter.ShowAllValuesInFilterPopup; set => this.vGridControl1.OptionsFilter.ShowAllValuesInFilterPopup = value; }
        public bool ShowCustomFunctions { get => this.vGridControl1.OptionsFilter.ShowCustomFunctions== DevExpress.Utils.DefaultBoolean.True;
            set =>this.vGridControl1.OptionsFilter.ShowCustomFunctions = value? DevExpress.Utils.DefaultBoolean.True: DevExpress.Utils.DefaultBoolean.False; }
        public VerticalGrid()
        {
            InitializeComponent();
        }
    }
}
