namespace Jund.DesktopControlLibrary.GridControl.AccessoryControl
{
    partial class DataDetailsBoard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutEdit = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            ((System.ComponentModel.ISupportInitialize)(this.layoutEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutEdit
            // 
            this.layoutEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutEdit.Location = new System.Drawing.Point(0, 0);
            this.layoutEdit.Name = "layoutEdit";
            this.layoutEdit.Root = this.Root;
            this.layoutEdit.Size = new System.Drawing.Size(525, 768);
            this.layoutEdit.TabIndex = 0;
            this.layoutEdit.Text = "layoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(525, 768);
            this.Root.TextVisible = false;
            // 
            // DataDetailsBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutEdit);
            this.Name = "DataDetailsBoard";
            this.Size = new System.Drawing.Size(525, 768);
            ((System.ComponentModel.ISupportInitialize)(this.layoutEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutEdit;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
    }
}
