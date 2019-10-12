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

namespace Jund.DesktopControlLibrary.LoginControl
{
    public partial class LoginForm : XtraUserControl
    {
        [DisplayName("LoginTitle")]
        public string Login_title { get => this.lblLoginTitle.Text; set => this.lblLoginTitle.Text = value; }
        [DisplayName("WelcomeTitle")]
        public string Welcome_title { get => this.lblWelcomeTitle.Text; set => this.lblWelcomeTitle.Text = value; }
        [DisplayName("UserNameTextboxTooltip")]
        public string Username_title { get => this.txtUserName.Properties.NullValuePrompt; set => this.txtUserName.Properties.NullValuePrompt = value; }
        [DisplayName("PasswordTextboxTooltip")]
        public string Password_title { get => this.txtPassword.Properties.NullValuePrompt; set => this.txtPassword.Properties.NullValuePrompt = value; }
        [DisplayName("LoginButtonCaptain")]
        public string Login_button_title { get => this.btnLogin.Text; set => this.btnLogin.Text = value; }
        [DisplayName("CancelButtonCaptain")]
        public string Cancel_button_title { get => this.btnCancel.Text; set => this.btnCancel.Text = value; }
        [DisplayName("UserInfoTable")]
        public string User_table{ get; set; }
        [DisplayName("UserLoginIdColumn")]
        public string User_loginId_column { get; set; }
        [DisplayName("UserPasswordColumn")]
        public string User_password_column { get; set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void txtPassword_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (txtPassword.Properties.PasswordChar == '*')
                txtPassword.Properties.PasswordChar = new char();
            else
                txtPassword.Properties.PasswordChar = '*';
        }
    }
}