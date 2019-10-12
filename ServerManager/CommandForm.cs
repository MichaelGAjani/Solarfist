using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ServerManager
{
    public partial class CommandForm : XtraForm
    {
        /// <summary>
        /// 指令值
        /// </summary>
        public int CustomCommand
        {
            get
            {
                return Convert.ToInt32(this.txtCommand.Value);
            }
            set
            {
                this.txtCommand.Value = value;
            }
        }

        /// <summary>
        /// 发送指令
        /// </summary>
        public CommandForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 确定发送指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((this.CustomCommand < 0x80) && (this.CustomCommand > 0xff))
            {
                MessageBox.Show("命令值必须在 128 到 255 之间");
            }
            else
            {
                base.DialogResult = DialogResult.OK;
            }
        }
    }
}
