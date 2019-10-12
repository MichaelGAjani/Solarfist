namespace ServerManager
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolbarFormControl1 = new DevExpress.XtraBars.ToolbarForm.ToolbarFormControl();
            this.toolbarFormManager1 = new DevExpress.XtraBars.ToolbarForm.ToolbarFormManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.btnInstall = new DevExpress.XtraBars.BarButtonItem();
            this.btnUnInstall = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.btnStart = new DevExpress.XtraBars.BarButtonItem();
            this.btnStop = new DevExpress.XtraBars.BarButtonItem();
            this.btnPause = new DevExpress.XtraBars.BarButtonItem();
            this.btnResume = new DevExpress.XtraBars.BarButtonItem();
            this.btnRestart = new DevExpress.XtraBars.BarButtonItem();
            this.btnProperty = new DevExpress.XtraBars.BarButtonItem();
            this.btnCommand = new DevExpress.XtraBars.BarButtonItem();
            this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem13 = new DevExpress.XtraBars.BarButtonItem();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.viewService = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.toolbarFormControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolbarFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewService)).BeginInit();
            this.SuspendLayout();
            // 
            // toolbarFormControl1
            // 
            this.toolbarFormControl1.Location = new System.Drawing.Point(0, 0);
            this.toolbarFormControl1.Manager = this.toolbarFormManager1;
            this.toolbarFormControl1.Name = "toolbarFormControl1";
            this.toolbarFormControl1.Size = new System.Drawing.Size(1453, 30);
            this.toolbarFormControl1.TabIndex = 0;
            this.toolbarFormControl1.TabStop = false;
            this.toolbarFormControl1.TitleItemLinks.Add(this.barSubItem1);
            this.toolbarFormControl1.TitleItemLinks.Add(this.btnStart, true);
            this.toolbarFormControl1.TitleItemLinks.Add(this.btnStop);
            this.toolbarFormControl1.TitleItemLinks.Add(this.btnPause);
            this.toolbarFormControl1.TitleItemLinks.Add(this.btnResume);
            this.toolbarFormControl1.TitleItemLinks.Add(this.btnRestart);
            this.toolbarFormControl1.TitleItemLinks.Add(this.btnProperty, true);
            this.toolbarFormControl1.TitleItemLinks.Add(this.btnCommand);
            this.toolbarFormControl1.TitleItemLinks.Add(this.btnRefresh);
            this.toolbarFormControl1.TitleItemLinks.Add(this.barSubItem2);
            this.toolbarFormControl1.ToolbarForm = this;
            // 
            // toolbarFormManager1
            // 
            this.toolbarFormManager1.DockControls.Add(this.barDockControlTop);
            this.toolbarFormManager1.DockControls.Add(this.barDockControlBottom);
            this.toolbarFormManager1.DockControls.Add(this.barDockControlLeft);
            this.toolbarFormManager1.DockControls.Add(this.barDockControlRight);
            this.toolbarFormManager1.Form = this;
            this.toolbarFormManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1,
            this.btnInstall,
            this.btnUnInstall,
            this.barButtonItem3,
            this.btnStart,
            this.btnStop,
            this.btnPause,
            this.btnResume,
            this.btnRestart,
            this.btnProperty,
            this.btnCommand,
            this.btnRefresh,
            this.barSubItem2,
            this.barButtonItem13});
            this.toolbarFormManager1.MaxItemId = 17;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 30);
            this.barDockControlTop.Manager = this.toolbarFormManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1453, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 627);
            this.barDockControlBottom.Manager = this.toolbarFormManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1453, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 30);
            this.barDockControlLeft.Manager = this.toolbarFormManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 597);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1453, 30);
            this.barDockControlRight.Manager = this.toolbarFormManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 597);
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "Service";
            this.barSubItem1.Id = 1;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnInstall),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnUnInstall),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // btnInstall
            // 
            this.btnInstall.Caption = "Install Service";
            this.btnInstall.Id = 2;
            this.btnInstall.ItemAppearance.Disabled.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnInstall.ItemAppearance.Disabled.Options.UseFont = true;
            this.btnInstall.ItemAppearance.Hovered.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnInstall.ItemAppearance.Hovered.Options.UseFont = true;
            this.btnInstall.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnInstall.ItemAppearance.Normal.Options.UseFont = true;
            this.btnInstall.ItemAppearance.Pressed.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnInstall.ItemAppearance.Pressed.Options.UseFont = true;
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnInstall_Click);
            // 
            // btnUnInstall
            // 
            this.btnUnInstall.Caption = "UnInstall Service";
            this.btnUnInstall.Id = 3;
            this.btnUnInstall.ItemAppearance.Disabled.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnUnInstall.ItemAppearance.Disabled.Options.UseFont = true;
            this.btnUnInstall.ItemAppearance.Hovered.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnUnInstall.ItemAppearance.Hovered.Options.UseFont = true;
            this.btnUnInstall.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnUnInstall.ItemAppearance.Normal.Options.UseFont = true;
            this.btnUnInstall.ItemAppearance.Pressed.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnUnInstall.ItemAppearance.Pressed.Options.UseFont = true;
            this.btnUnInstall.Name = "btnUnInstall";
            this.btnUnInstall.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUnInstall_Click);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "Service List";
            this.barButtonItem3.Id = 4;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // btnStart
            // 
            this.btnStart.Caption = "Start";
            this.btnStart.Id = 6;
            this.btnStart.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.ImageOptions.Image")));
            this.btnStart.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnStart.ImageOptions.LargeImage")));
            this.btnStart.ItemAppearance.Disabled.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnStart.ItemAppearance.Disabled.Options.UseFont = true;
            this.btnStart.ItemAppearance.Hovered.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnStart.ItemAppearance.Hovered.Options.UseFont = true;
            this.btnStart.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnStart.ItemAppearance.Normal.Options.UseFont = true;
            this.btnStart.ItemAppearance.Pressed.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnStart.ItemAppearance.Pressed.Options.UseFont = true;
            this.btnStart.Name = "btnStart";
            this.btnStart.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnStart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStrat_Click);
            // 
            // btnStop
            // 
            this.btnStop.Caption = "Stop";
            this.btnStop.Id = 7;
            this.btnStop.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.ImageOptions.Image")));
            this.btnStop.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnStop.ImageOptions.LargeImage")));
            this.btnStop.ItemAppearance.Disabled.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnStop.ItemAppearance.Disabled.Options.UseFont = true;
            this.btnStop.ItemAppearance.Hovered.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnStop.ItemAppearance.Hovered.Options.UseFont = true;
            this.btnStop.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnStop.ItemAppearance.Normal.Options.UseFont = true;
            this.btnStop.ItemAppearance.Pressed.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnStop.ItemAppearance.Pressed.Options.UseFont = true;
            this.btnStop.Name = "btnStop";
            this.btnStop.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnStop.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStop_Click);
            // 
            // btnPause
            // 
            this.btnPause.Caption = "Suspend";
            this.btnPause.Id = 8;
            this.btnPause.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnPause.ImageOptions.Image")));
            this.btnPause.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnPause.ImageOptions.LargeImage")));
            this.btnPause.ItemAppearance.Disabled.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnPause.ItemAppearance.Disabled.Options.UseFont = true;
            this.btnPause.ItemAppearance.Hovered.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnPause.ItemAppearance.Hovered.Options.UseFont = true;
            this.btnPause.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnPause.ItemAppearance.Normal.Options.UseFont = true;
            this.btnPause.ItemAppearance.Pressed.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnPause.ItemAppearance.Pressed.Options.UseFont = true;
            this.btnPause.Name = "btnPause";
            this.btnPause.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnPause.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPause_Click);
            // 
            // btnResume
            // 
            this.btnResume.Caption = "Resume";
            this.btnResume.Id = 9;
            this.btnResume.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnResume.ImageOptions.Image")));
            this.btnResume.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnResume.ImageOptions.LargeImage")));
            this.btnResume.ItemAppearance.Disabled.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnResume.ItemAppearance.Disabled.Options.UseFont = true;
            this.btnResume.ItemAppearance.Hovered.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnResume.ItemAppearance.Hovered.Options.UseFont = true;
            this.btnResume.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnResume.ItemAppearance.Normal.Options.UseFont = true;
            this.btnResume.ItemAppearance.Pressed.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnResume.ItemAppearance.Pressed.Options.UseFont = true;
            this.btnResume.Name = "btnResume";
            this.btnResume.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnResume.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnResume_Click);
            // 
            // btnRestart
            // 
            this.btnRestart.Caption = "Restart";
            this.btnRestart.Id = 10;
            this.btnRestart.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnRestart.ImageOptions.Image")));
            this.btnRestart.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnRestart.ImageOptions.LargeImage")));
            this.btnRestart.ItemAppearance.Disabled.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRestart.ItemAppearance.Disabled.Options.UseFont = true;
            this.btnRestart.ItemAppearance.Hovered.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRestart.ItemAppearance.Hovered.Options.UseFont = true;
            this.btnRestart.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRestart.ItemAppearance.Normal.Options.UseFont = true;
            this.btnRestart.ItemAppearance.Pressed.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRestart.ItemAppearance.Pressed.Options.UseFont = true;
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnRestart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRestart_Click);
            // 
            // btnProperty
            // 
            this.btnProperty.Caption = "Property";
            this.btnProperty.Id = 11;
            this.btnProperty.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnProperty.ImageOptions.Image")));
            this.btnProperty.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnProperty.ImageOptions.LargeImage")));
            this.btnProperty.ItemAppearance.Disabled.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnProperty.ItemAppearance.Disabled.Options.UseFont = true;
            this.btnProperty.ItemAppearance.Hovered.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnProperty.ItemAppearance.Hovered.Options.UseFont = true;
            this.btnProperty.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnProperty.ItemAppearance.Normal.Options.UseFont = true;
            this.btnProperty.ItemAppearance.Pressed.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnProperty.ItemAppearance.Pressed.Options.UseFont = true;
            this.btnProperty.Name = "btnProperty";
            this.btnProperty.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnProperty.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnProperty_Click);
            // 
            // btnCommand
            // 
            this.btnCommand.Caption = "Command";
            this.btnCommand.Id = 13;
            this.btnCommand.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCommand.ImageOptions.Image")));
            this.btnCommand.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnCommand.ImageOptions.LargeImage")));
            this.btnCommand.ItemAppearance.Disabled.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnCommand.ItemAppearance.Disabled.Options.UseFont = true;
            this.btnCommand.ItemAppearance.Hovered.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnCommand.ItemAppearance.Hovered.Options.UseFont = true;
            this.btnCommand.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnCommand.ItemAppearance.Normal.Options.UseFont = true;
            this.btnCommand.ItemAppearance.Pressed.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnCommand.ItemAppearance.Pressed.Options.UseFont = true;
            this.btnCommand.Name = "btnCommand";
            this.btnCommand.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnCommand.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCommand_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Caption = "Refresh";
            this.btnRefresh.Id = 14;
            this.btnRefresh.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.ImageOptions.Image")));
            this.btnRefresh.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnRefresh.ImageOptions.LargeImage")));
            this.btnRefresh.ItemAppearance.Disabled.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ItemAppearance.Disabled.Options.UseFont = true;
            this.btnRefresh.ItemAppearance.Hovered.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ItemAppearance.Hovered.Options.UseFont = true;
            this.btnRefresh.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ItemAppearance.Normal.Options.UseFont = true;
            this.btnRefresh.ItemAppearance.Pressed.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ItemAppearance.Pressed.Options.UseFont = true;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRefresh_Click);
            // 
            // barSubItem2
            // 
            this.barSubItem2.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barSubItem2.Caption = "Help";
            this.barSubItem2.Id = 15;
            this.barSubItem2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem13)});
            this.barSubItem2.Name = "barSubItem2";
            // 
            // barButtonItem13
            // 
            this.barButtonItem13.Caption = "About";
            this.barButtonItem13.Id = 16;
            this.barButtonItem13.Name = "barButtonItem13";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 30);
            this.gridControl1.MainView = this.viewService;
            this.gridControl1.MenuManager = this.toolbarFormManager1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1453, 597);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewService});
            // 
            // viewService
            // 
            this.viewService.GridControl = this.gridControl1;
            this.viewService.Name = "viewService";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1453, 627);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Controls.Add(this.toolbarFormControl1);
            this.Name = "MainForm";
            this.Text = "MainForm1";
            this.ToolbarFormControl = this.toolbarFormControl1;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.toolbarFormControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolbarFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewService)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.ToolbarForm.ToolbarFormControl toolbarFormControl1;
        private DevExpress.XtraBars.ToolbarForm.ToolbarFormManager toolbarFormManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem btnInstall;
        private DevExpress.XtraBars.BarButtonItem btnUnInstall;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem btnStart;
        private DevExpress.XtraBars.BarButtonItem btnStop;
        private DevExpress.XtraBars.BarButtonItem btnPause;
        private DevExpress.XtraBars.BarButtonItem btnResume;
        private DevExpress.XtraBars.BarButtonItem btnRestart;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView viewService;
        private DevExpress.XtraBars.BarButtonItem btnProperty;
        private DevExpress.XtraBars.BarButtonItem btnCommand;
        private DevExpress.XtraBars.BarButtonItem btnRefresh;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem13;
    }
}