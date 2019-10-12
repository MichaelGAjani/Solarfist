namespace ServerManager
{
    partial class PropertyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new DevExpress.XtraEditors.LabelControl();
            this.lblName = new DevExpress.XtraEditors.LabelControl();
            this.label3 = new DevExpress.XtraEditors.LabelControl();
            this.lblDisplayName = new DevExpress.XtraEditors.LabelControl();
            this.label5 = new DevExpress.XtraEditors.LabelControl();
            this.label7 = new DevExpress.XtraEditors.LabelControl();
            this.label8 = new DevExpress.XtraEditors.LabelControl();
            this.txtDisc = new DevExpress.XtraEditors.MemoEdit();
            this.label2 = new DevExpress.XtraEditors.LabelControl();
            this.txtPath = new DevExpress.XtraEditors.TextEdit();
            this.cmbStartType = new DevExpress.XtraEditors.LookUpEdit();
            this.cmbLoginType = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDisc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStartType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLoginType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 413);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(415, 58);
            this.panel1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(317, 19);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消(&C)";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(224, 19);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(88, 28);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定(&O)";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(26, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "服务名称";
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(108, 28);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(31, 14);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "label2";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(26, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "显示名称";
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.Location = new System.Drawing.Point(108, 63);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(31, 14);
            this.lblDisplayName.TabIndex = 5;
            this.lblDisplayName.Text = "label4";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(26, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 14);
            this.label5.TabIndex = 6;
            this.label5.Text = "描述";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(26, 337);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 14);
            this.label7.TabIndex = 8;
            this.label7.Text = "启动类型";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(26, 375);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 14);
            this.label8.TabIndex = 10;
            this.label8.Text = "登录账号";
            // 
            // txtDisc
            // 
            this.txtDisc.Location = new System.Drawing.Point(110, 98);
            this.txtDisc.Name = "txtDisc";
            this.txtDisc.Properties.ReadOnly = true;
            this.txtDisc.Size = new System.Drawing.Size(294, 149);
            this.txtDisc.TabIndex = 12;
            this.txtDisc.TabStop = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(26, 272);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 14);
            this.label2.TabIndex = 13;
            this.label2.Text = "路径";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(110, 264);
            this.txtPath.Name = "txtPath";
            this.txtPath.Properties.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(294, 20);
            this.txtPath.TabIndex = 14;
            this.txtPath.TabStop = false;
            // 
            // cmbStartType
            // 
            this.cmbStartType.Location = new System.Drawing.Point(110, 334);
            this.cmbStartType.Name = "cmbStartType";
            this.cmbStartType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbStartType.Properties.NullText = "";
            this.cmbStartType.Properties.PopupSizeable = false;
            this.cmbStartType.Size = new System.Drawing.Size(294, 20);
            this.cmbStartType.TabIndex = 9;
            // 
            // cmbLoginType
            // 
            this.cmbLoginType.Location = new System.Drawing.Point(110, 372);
            this.cmbLoginType.Name = "cmbLoginType";
            this.cmbLoginType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLoginType.Properties.NullText = "";
            this.cmbLoginType.Properties.PopupSizeable = false;
            this.cmbLoginType.Size = new System.Drawing.Size(294, 20);
            this.cmbLoginType.TabIndex = 11;
            // 
            // PropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 471);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDisc);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblDisplayName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cmbStartType);
            this.Controls.Add(this.cmbLoginType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PropertyForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "服务属性";
            this.Load += new System.EventHandler(this.PropertyForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtDisc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStartType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLoginType.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panel1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.LabelControl label1;
        private DevExpress.XtraEditors.LabelControl lblName;
        private DevExpress.XtraEditors.LabelControl label3;
        private DevExpress.XtraEditors.LabelControl lblDisplayName;
        private DevExpress.XtraEditors.LabelControl label5;
        private DevExpress.XtraEditors.LabelControl label7;
        private DevExpress.XtraEditors.LabelControl label8;
        private DevExpress.XtraEditors.MemoEdit txtDisc;
        private DevExpress.XtraEditors.LabelControl label2;
        private DevExpress.XtraEditors.TextEdit txtPath;
        private DevExpress.XtraEditors.LookUpEdit cmbStartType;
        private DevExpress.XtraEditors.LookUpEdit cmbLoginType;
    }
}