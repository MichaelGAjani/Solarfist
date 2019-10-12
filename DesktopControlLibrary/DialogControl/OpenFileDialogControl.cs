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

namespace Jund.DesktopControlLibrary.DialogControl
{
    public partial class OpenFileDialogControl : Component
    {
        XtraOpenFileDialog dig = new XtraOpenFileDialog();

        [DisplayName("DefaultExtension")]
        public string DefaultEx { get => dig.DefaultExt; set => dig.DefaultExt = value; }
        [DisplayName("RestoreDirectory")]
        public bool RestoreDirectory { get => dig.RestoreDirectory; set =>dig.RestoreDirectory=value; }
        [DisplayName("Title")]
        public string Title { get => dig.Title; set => dig.Title = value; }
        [DisplayName("InitialDirectory")]
        public string InitialDirectory { get => dig.InitialDirectory; set => dig.InitialDirectory = value; }
        [DisplayName("FileName")]
        public string FileName { get => dig.FileName; set => dig.FileName = value; }
        [DisplayName("FileNames")]
        public List<string> FileNames { get => dig.FileNames.ToList();  }

        public OpenFileDialogControl()
        {
            dig.CheckFileExists = true;
            dig.CheckPathExists = true;
            dig.SupportMultiDottedExtensions = true;
        }
        public DialogResult ShowDialog()
        {
           return dig.ShowDialog();
        }

        public void OpenFile(string file)
        {
            dig.FileName = file;
            dig.OpenFile();
        }
    }
}
