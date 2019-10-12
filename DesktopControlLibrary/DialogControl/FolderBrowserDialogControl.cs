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
using static System.Environment;

namespace Jund.DesktopControlLibrary.DialogControl
{
    public partial class FolderBrowserDialogControl : Component
    {
        XtraFolderBrowserDialog dig = new XtraFolderBrowserDialog();

        [DisplayName("RootFolder")]
        public SpecialFolder RootFolder { get => dig.RootFolder; set => dig.RootFolder = value; }
        [DisplayName("SelectedPath")]
        public string SelectedPath { get => dig.SelectedPath; set => dig.SelectedPath = value; }
        [DisplayName("Title")]
        public string Title { get => dig.Title; set => dig.Title = value; }
        public FolderBrowserDialogControl()
        {
            dig.DialogStyle = DevExpress.Utils.CommonDialogs.FolderBrowserDialogStyle.Wide;
            dig.ShowNewFolderButton = true;
        }

        public void ShowDialog()
        {
            dig.ShowDialog();
        }
    }
}
