using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ServerManager.Entity;
using System.ServiceProcess;

namespace ServerManager
{
    public partial class MainForm : DevExpress.XtraBars.ToolbarForm.ToolbarForm
    {
        private int lastCommandValue = 0x80;//服务指令初始默认值128

        List<Service> service_list = new List<Service>();
        List<ServiceController> service_controller_list
        {
            get => System.ServiceProcess.ServiceController.GetServices().ToList();
        }

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取选定的服务
        /// </summary>
        /// <returns></returns>
        private Service GetSelectedService()
        {
            return this.viewService.GetFocusedRow() as Service;
        }

        #region 更新画面上按钮可用性
        /// <summary>
        /// 更新画面上按钮可用性
        /// </summary>
        private void UpdaterButtonStatus()
        {
            Service selectedService = this.GetSelectedService();
            if (selectedService == null)//没有选定服务，设定按钮不可用
            {
                this.btnUnInstall.Enabled = false;
                this.btnStart.Enabled = false;
                this.btnStop.Enabled = false;
                this.btnPause.Enabled = false;
                this.btnResume.Enabled = false;
                this.btnRestart.Enabled = false;
                this.btnProperty.Enabled = false;
                this.btnCommand.Enabled = false;
            }
            else
            {
                this.btnUnInstall.Enabled = this.btnStart.Enabled;
                this.btnStart.Enabled = selectedService.CanStart;
                this.btnStop.Enabled = selectedService.CanStop;
                this.btnPause.Enabled = selectedService.CanPause;
                this.btnResume.Enabled = selectedService.CanContinue;
                this.btnRestart.Enabled = selectedService.CanRestart;
                this.btnProperty.Enabled = true;
                this.btnCommand.Enabled = selectedService.CanExecuteCommand;
            }
        }
        #endregion

        #region 加载服务列表
        /// <summary>
        /// 载入服务列表
        /// </summary>
        private void LoadService()
        {
            this.service_list.Clear();

            foreach (ServiceController srv in this.service_controller_list)
            {
                Service service = new Service(srv);
                this.service_list.Add(service);
            }

            this.gridControl1.DataSource = this.service_list;
            this.viewService.RefreshData();
        }
        #endregion

        #region 服务部署
        /// <summary>
        /// 安装服务
        /// </summary>
        private void InstallService()
        {
            try
            {
                InstallForm form = new InstallForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string path = form.Path;//服务文件路径
                    base.UseWaitCursor = true;
                    Service.Install(path);//执行安装
                    MessageBox.Show("服务安装成功");

                    //try
                    //{
                    //    foreach (string str2 in Service.GetServiceNames(path))
                    //    {
                    //        this.srvConfig.Services.Add(str2);
                    //        this.srvConfig.SaveService(str2);
                    //    }
                    //    //this.SaveConfig();//保存配置文件                        
                    //}
                    //catch (Exception exception)
                    //{
                    //    Console.WriteLine(exception.Message);
                    //}
                    this.LoadService();
                }
            }
            catch (Exception ex)
            {
                this.LogMessage("安装服务失败:" + ex.Message);
                //this.ShowLogView();
            }
            finally
            {
                base.UseWaitCursor = false;
            }
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        private void UninstallService()
        {
            Service selectedService = this.GetSelectedService();
            if ((selectedService != null) && (MessageBox.Show(string.Format("你确定要卸载服务“{0}”吗？", selectedService.DisplayName), "卸载服务", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK))
            {
                try
                {
                    base.UseWaitCursor = true;
                    Service.Uninstall(selectedService.Path);//执行卸载
                    MessageBox.Show("服务卸载成功");

                    try
                    {
                        this.LoadService();
                    }
                    catch
                    {
                    }
                }
                catch (Exception exception)
                {
                    this.LogMessage("卸载服务失败:" + exception.Message);
                }
                finally
                {
                    base.UseWaitCursor = false;
                }
            }
        }

        #endregion

        #region 服务操作
        /// <summary>
        /// 启动服务
        /// </summary>
        private void StartService()
        {
            try
            {
                Service selectedService = this.GetSelectedService();
                if ((selectedService != null) && selectedService.CanStart)
                {
                    base.UseWaitCursor = true;
                    selectedService.Start();
                    this.LoadService();
                }
            }
            catch (Exception exception)
            {
                this.LogMessage("启动服务失败:" + exception.Message);
            }
            finally
            {
                base.UseWaitCursor = false;
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        private void StopService()
        {
            try
            {
                Service selectedService = this.GetSelectedService();
                if ((selectedService != null) && selectedService.CanStop)
                {
                    base.UseWaitCursor = true;
                    selectedService.Stop();
                    this.LoadService();
                }
            }
            catch (Exception exception)
            {
                this.LogMessage("停止服务失败:" + exception.Message);
            }
            finally
            {
                base.UseWaitCursor = false;
            }
        }

        /// <summary>
        /// 暂停服务
        /// </summary>
        private void PauseService()
        {
            try
            {
                Service selectedService = this.GetSelectedService();
                if ((selectedService != null) && selectedService.CanPause)
                {
                    base.UseWaitCursor = true;
                    selectedService.Pause();
                    this.LoadService();
                }
            }
            catch (Exception exception)
            {
                this.LogMessage("暂停服务失败:" + exception.Message);
            }
            finally
            {
                base.UseWaitCursor = false;
            }
        }

        /// <summary>
        /// 恢复服务
        /// </summary>
        private void ResumeService()
        {
            try
            {
                Service selectedService = this.GetSelectedService();
                if ((selectedService != null) && selectedService.CanContinue)
                {
                    base.UseWaitCursor = true;
                    selectedService.Continue();
                    this.LoadService();
                }
            }
            catch (Exception exception)
            {
                this.LogMessage("恢复服务失败:" + exception.Message);
            }
            finally
            {
                base.UseWaitCursor = false;
            }
        }

        /// <summary>
        /// 重启服务
        /// </summary>
        private void RestartService()
        {
            try
            {
                Service selectedService = this.GetSelectedService();
                if ((selectedService != null) && selectedService.CanRestart)
                {
                    base.UseWaitCursor = true;
                    selectedService.Restart();
                    this.LoadService();
                }
            }
            catch (Exception exception)
            {
                this.LogMessage("重启服务失败:" + exception.Message);
            }
            finally
            {
                base.UseWaitCursor = false;
            }
        }
        #endregion

        #region 服务指令
        /// <summary>
        /// 发送指令
        /// </summary>
        private void ExecuteCommand()
        {
            try
            {
                Service selectedService = this.GetSelectedService();
                if (selectedService != null)
                {
                    CommandForm form = new CommandForm
                    {
                        CustomCommand = this.lastCommandValue
                    };
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        int customCommand = form.CustomCommand;
                        selectedService.ExecuteCommand(customCommand);
                        this.lastCommandValue = customCommand;
                    }
                }
            }
            catch (Exception exception)
            {
                this.LogMessage("发送命令失败:" + exception.Message);
            }
        }
        #endregion

        #region 显示服务属性
        /// <summary>
        /// 显示服务属性
        /// </summary>
        private void ShowServiceProperty()
        {
            try
            {
                Service selectedService = this.GetSelectedService();
                if (selectedService != null)
                {
                    PropertyForm form = new PropertyForm
                    {
                        ServiceName = selectedService.Name,
                        DisplayName = selectedService.DisplayName,
                        Path = selectedService.Path,
                        Disc = selectedService.Description,
                        StartType = selectedService.StartType,
                        Account = selectedService.Account
                    };
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        selectedService.StartType = form.StartType;
                        selectedService.Account = form.Account;
                        this.LoadService();
                    }
                }
            }
            catch (Exception exception)
            {
                this.LogMessage(exception.Message);
            }
        }
        #endregion

        private void LogMessage(string msg)
        {
            XtraMessageBox.Show(msg);
        }
        /// <summary>
        /// 关闭窗体
        /// </summary>
        private void CloseForm()
        {
            if (MessageBox.Show("你确定要关闭服务管理器吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Application.ExitThread();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.LoadService();
        }

        #region 按钮
        /// <summary>
        /// 安装服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInstall_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.InstallService();
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnInstall_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.UninstallService();
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStrat_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.StartService();
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.StopService();
        }

        /// <summary>
        /// 暂停服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPause_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.PauseService();
        }

        /// <summary>
        /// 恢复服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResume_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ResumeService();
        }

        /// <summary>
        /// 重启服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRestart_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.RestartService();
        }

        /// <summary>
        /// 显示服务属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProperty_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ShowServiceProperty();
        }


        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommand_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ExecuteCommand();
        }

        /// <summary>
        /// 刷新服务列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.LoadService();
        }



        /// <summary>
        /// 关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAbout_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }

        #endregion
    }
}