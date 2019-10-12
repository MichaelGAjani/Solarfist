using DevExpress.Skins;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Jund.DesktopControlLibrary.MainForm
{
    public partial class MainModernToolbarForm : DevExpress.XtraBars.ToolbarForm.ToolbarForm
    {
        public MainModernToolbarForm()
        {
            InitializeComponent();
        }

        public void AddMenu(string captain)
        {
            BarSubItem menu = new BarSubItem();
            menu.Caption = captain;
            this.toolbarFormManager1.Items.Add(menu);
        }


        private void menuAbout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }


    }
}
