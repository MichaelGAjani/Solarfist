using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraEditors;

namespace ServerManager
{
    public partial class InstallForm : XtraForm
    {
        /// <summary>
        /// 服务路径
        /// </summary>
        public string Path
        {
            get
            {
                return this.txtPath.Text;
            }
            set
            {
                this.txtPath.Text = value;
            }
        }

        /// <summary>
        /// 安装服务
        /// </summary>
        public InstallForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 确定执行安装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!File.Exists(this.txtPath.Text))
            {
                MessageBox.Show("文件不存在");
            }
            else
            {
                base.DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 浏览选择要安装的服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            XtraOpenFileDialog dialog = new XtraOpenFileDialog
            {
                FileName = this.txtPath.Text,
                Filter = "Windows 服务(*.exe)|*.exe|所有文件(*.*)|*.*",
                FilterIndex = 0
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.txtPath.Text = dialog.FileName;
            }
        }

        #region 把要安装的服务拖动到路径输入框
        private void txtPath_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] data = e.Data.GetData(DataFormats.FileDrop) as string[];
                if ((data != null) && (data.Length != 0))
                {
                    string str = data[0];
                    this.txtPath.Text = str;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void txtPath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        #endregion
    }
}
