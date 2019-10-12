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
using Jund.DynamicCodeHelper;
using Jund.DynamicCodeHelper.Entity;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using Jund.DesktopControlLibrary.DialogControl;
using DevExpress.XtraGrid;

namespace Jund.DesktopControlLibrary.GridControl
{
    public partial class StandardGridView : DevExpress.XtraEditors.XtraUserControl,BaseViewControlInterface
    {
        DynamicObjectHelper dynamicObject;
        Object focusData => this.gridView1.GetFocusedRow();
        string focusDisplayText => focusData.GetType().GetProperty(dynamicObject.ColumnList.Find(obj => obj.Id == dynamicObject.classDesc.Text_id).Column_name).GetValue(focusData).ToString();
        string focusId=> focusData.GetType().GetProperty(dynamicObject.ColumnList.Find(obj => obj.Id == dynamicObject.classDesc.Value_id).Column_name).GetValue(focusData).ToString();
        public StandardGridView()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;
        }

        public void BindData(DynamicObjectHelper dynamicObject)
        {
            #region 绑定数据源
            this.dynamicObject = dynamicObject;
            this.gridView1.Columns.Clear();

            this.gridControl1.DataSource = this.dynamicObject.DataList;
            #endregion

            this.BindViewCombo();//设定列表

            this.gridView1.RefreshData();

            this.gridView1.MoveFirst();
        }

        private void BindViewCombo()
        {
            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                this.gridView1.Columns[i].OptionsColumn.AllowEdit = false;

                string field_name = this.gridView1.Columns[i].FieldName.ToLower();

                #region 设定列标签
                TableColumnDesc desc = this.dynamicObject.ColumnList.Find(obj => obj.Column_name.ToLower() == field_name);

                this.gridView1.Columns[i].Caption = GlobeData.GetTableColumnDisplayName(desc.Id);
                #endregion

                #region BindCombo
                if (this.gridView1.Columns[i].FieldName == "Id")
                {
                    this.gridView1.Columns[i].VisibleIndex = 0;
                }
                else
                {
                    TableColumnReference columnReference = this.dynamicObject.ColumnReferenceList.Find(obj => obj.Id == desc.Id);

                    if (columnReference != null)
                    {
                        if (columnReference.Reference_id != null)
                        {
                            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repEditor = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
                            repEditor.AutoHeight = false;
                            repEditor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
                            repEditor.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo(GlobeData.GetColumnName(columnReference.Reference_value_column_id), "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo(GlobeData.GetColumnName(columnReference.Reference_text_column_id), "Name")});
                            repEditor.DisplayMember = GlobeData.GetColumnName(columnReference.Reference_text_column_id);
                            repEditor.Name = "repEditor" + field_name;
                            repEditor.ValueMember = GlobeData.GetColumnName(columnReference.Reference_value_column_id);
                            repEditor.DataSource = new DynamicObjectHelper(
                                GlobeData.classDescList.Find(obj => obj.Table_id == columnReference.Reference_id)).DataList;
                            this.gridView1.Columns[i].ColumnEdit = repEditor;
                        }
                    }
                }

                #endregion

                TableColumnRegular regular = this.dynamicObject.ColumnRegularList.Find(obj => obj.Id == desc.Id);

                if (regular != null && regular.Max_value != regular.Min_value)
                    BindCondtion(this.gridView1.Columns[i], regular.Min_value, regular.Max_value);
            }
        }
        private void BindCondtion(GridColumn column,int min,int max)
        {
            GridFormatRule gridFormatRuleMax = new DevExpress.XtraGrid.GridFormatRule();
            FormatConditionRuleValue formatConditionRuleValueMax = new DevExpress.XtraEditors.FormatConditionRuleValue();
            formatConditionRuleValueMax.Appearance.ForeColor = System.Drawing.Color.Red;
            formatConditionRuleValueMax.Appearance.Options.UseForeColor = true;
            formatConditionRuleValueMax.Condition = DevExpress.XtraEditors.FormatCondition.Greater;
            formatConditionRuleValueMax.Value1 = max;
            gridFormatRuleMax.Rule = formatConditionRuleValueMax;
            gridFormatRuleMax.Column = column;

            GridFormatRule gridFormatRuleMin = new DevExpress.XtraGrid.GridFormatRule();
            FormatConditionRuleValue formatConditionRuleValueMin= new DevExpress.XtraEditors.FormatConditionRuleValue();
            formatConditionRuleValueMin.Appearance.ForeColor = System.Drawing.Color.Red;
            formatConditionRuleValueMin.Appearance.Options.UseForeColor = true;
            formatConditionRuleValueMin.Condition = DevExpress.XtraEditors.FormatCondition.Less;
            formatConditionRuleValueMin.Value1 = min;
            gridFormatRuleMin.Rule = formatConditionRuleValueMin;
            gridFormatRuleMin.Column = column;
            this.gridView1.FormatRules.Add(gridFormatRuleMax);
            this.gridView1.FormatRules.Add(gridFormatRuleMin);
        }
        public void NewItem()
        {
            throw new NotImplementedException();
        }

        public void OpenEditItem()
        {
            throw new NotImplementedException();
        }

        public void RemoveItem()
        {
            throw new NotImplementedException();
        }

        public void RemoveSelectedItem()
        {
            throw new NotImplementedException();
        }

        public void RefreshDataList()
        {
            throw new NotImplementedException();
        }

        public void SetEditInRow(bool allowEdit)
        {
            this.gridView1.OptionsBehavior.Editable = allowEdit;
        }

        public void SetEditButtonMode(GridEditingMode mode)
        {
            this.gridView1.OptionsBehavior.EditingMode = mode;
        }

        public void ExportExcel()
        {
            using (SaveFileDialogControl saveFileDialog = new SaveFileDialogControl())
            {
                saveFileDialog.DefaultEx = "xlsx";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    this.gridView1.ExportToXlsx(saveFileDialog.FileName);
            }
        }

        public void ExportPDF()
        {
            using (SaveFileDialogControl saveFileDialog = new SaveFileDialogControl())
            {
                saveFileDialog.DefaultEx = "pdf";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    this.gridView1.ExportToPdf(saveFileDialog.FileName);
            }
        }

        public void ExportHtml()
        {
            using (SaveFileDialogControl saveFileDialog = new SaveFileDialogControl())
            {
                saveFileDialog.DefaultEx = "html";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    this.gridView1.ExportToHtml(saveFileDialog.FileName);
            }
        }

        public void ExportXml()
        {
            
        }

        public void SetMultiSelectMode(GridMultiSelectMode mode)
        {
            this.gridView1.OptionsSelection.MultiSelectMode = mode;
        }
    }
}
