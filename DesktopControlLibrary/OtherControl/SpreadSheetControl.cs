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
using System.IO;
using DevExpress.XtraBars.Ribbon;

namespace Jund.DesktopControlLibrary.OtherControl
{
    public partial class SpreadSheetControl : DevExpress.XtraEditors.XtraUserControl
    {
        public RibbonControl ExcelMenu { get => ribbonControl1; set=>ribbonControl1=value; }
        public SpreadSheetControl()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;
        }

        public void LoadFile(string file)
        {
            this.spreadsheetControl1.LoadDocument(file);
        }

        public void LoadFile()
        {
            DialogControl.OpenFileDialogControl openFileDialog = new DialogControl.OpenFileDialogControl();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                this.spreadsheetControl1.LoadDocument(openFileDialog.FileName);
        }

        public void SaveFile()
        {
            this.spreadsheetControl1.SaveDocument();
        }

        public void SaveFileAs()
        {
            DialogControl.SaveFileDialogControl saveFileDialog = new DialogControl.SaveFileDialogControl();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                this.spreadsheetControl1.SaveDocument(saveFileDialog.FileName);
        }
    }
}
