using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using DevExpress.XtraEditors;

namespace ServerManager
{
    public partial class PropertyForm : XtraForm
    {
        #region 服务属性
        /// <summary>
        /// 登录账户
        /// </summary>
        public string Account
        {
            get
            {
                return this.cmbLoginType.EditValue.ToString();
            }
            set
            {
                this.cmbLoginType.EditValue = value;
            }
        }

        /// <summary>
        /// 路径
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
        /// 描述
        /// </summary>
        public string Disc
        {
            get
            {
                return this.txtDisc.Text;
            }
            set
            {
                this.txtDisc.Text = value;
            }
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this.lblDisplayName.Text;
            }
            set
            {
                this.lblDisplayName.Text = value;
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string ServiceName
        {
            get
            {
                return this.lblName.Text;
            }
            set
            {
                this.lblName.Text = value;
            }
        }

        /// <summary>
        /// 服务启动模式
        /// </summary>
        public ServiceStartMode StartType
        {
            get
            {
                return (ServiceStartMode)this.cmbStartType.EditValue;
            }
            set
            {
                this.cmbStartType.EditValue = value;
            }
        }
        #endregion

        /// <summary>
        /// 下拉框选项集合
        /// </summary>
        private class ListItem
        {
            public string Display { get; set; }

            public object Value { get; set; }
        }

        /// <summary>
        /// 服务属性查看
        /// </summary>
        public PropertyForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设定下拉框
        /// </summary>
        private void InitList()
        {
            List<ListItem> list = new List<ListItem>();
            ListItem item = new ListItem
            {
                Display = "自动",
                Value = ServiceStartMode.Automatic
            };
            list.Add(item);
            ListItem item2 = new ListItem
            {
                Display = "手动",
                Value = ServiceStartMode.Manual
            };
            list.Add(item2);
            ListItem item3 = new ListItem
            {
                Display = "禁用",
                Value = ServiceStartMode.Disabled
            };
            list.Add(item3);
            this.cmbStartType.Properties.DisplayMember = "Display";
            this.cmbStartType.Properties.ValueMember = "Value";
            this.cmbStartType.Properties.DataSource = list;
            this.cmbStartType.ItemIndex = 0;
            List<ListItem> list2 = new List<ListItem>();
            ListItem item4 = new ListItem
            {
                Display = "本地服务",
                Value = @"NT AUTHORITY\LocalService"
            };
            list2.Add(item4);
            ListItem item5 = new ListItem
            {
                Display = "网络服务",
                Value = @"NT AUTHORITY\NetworkService"
            };
            list2.Add(item5);
            ListItem item6 = new ListItem
            {
                Display = "本地系统",
                Value = "LocalSystem"
            };
            list2.Add(item6);
            this.cmbLoginType.Properties.DisplayMember = "Display";
            this.cmbLoginType.Properties.ValueMember = "Value";
            this.cmbLoginType.Properties.DataSource = list2;
            this.cmbLoginType.ItemIndex = 0;
        }

        private void PropertyForm_Load(object sender, EventArgs e)
        {
            this.InitList();
        }
    }
}
