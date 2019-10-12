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
using DevExpress.XtraLayout;
using System.Reflection;
using DevExpress.XtraEditors.Mask;
using Jund.DynamicCodeHelper.Entity;
using Jund.DynamicCodeHelper;

namespace Jund.DesktopControlLibrary.GridControl.AccessoryControl
{
    public partial class DataDetailsBoard : DevExpress.XtraEditors.XtraUserControl
    {
        BindingSource bindingSource = new System.Windows.Forms.BindingSource();
        DynamicObjectHelper dynamicObject;

        List<BaseViewControl> subclassviewList = new List<BaseViewControl>();
        public DataDetailsBoard()
        {
            InitializeComponent();

            bindingSource.CurrentItemChanged += BindingSource_CurrentItemChanged;
        }

        public void BindNewEmptyData()
        {
            dynamicObject.Create();
            BindData(dynamicObject);
        }

        public LayoutControlGroup CreateLayoutGroup(string text)
        {
            LayoutControlGroup group1 = new LayoutControlGroup();
            group1.Text = text;
            group1.ExpandButtonVisible = true;

            return group1;
        }

        #region CreateItem
        /// <summary>
        /// 创建文本框
        /// </summary>
        /// <param name="captain">标签</param>
        /// <param name="prop">类属性</param>
        /// <param name="max_length">长度</param>
        /// <param name="mask_type">掩码类型</param>
        /// <param name="mask">掩码</param>
        /// <param name="is_readonly">只读</param>
        /// <returns></returns>
        public LayoutControlItem CreateTextItem(string captain, string prop, int max_length, MaskType mask_type, string mask, bool is_readonly = false)
        {
            LayoutControlItem item = new LayoutControlItem();
            TextEdit textEdit = new TextEdit();
            textEdit.Properties.ReadOnly = is_readonly;
            textEdit.DataBindings.Add("EditValue", bindingSource, prop);
            textEdit.Properties.MaxLength = max_length;
            textEdit.Properties.Mask.MaskType = mask_type;
            textEdit.Properties.Mask.EditMask = mask;
            textEdit.Properties.NullValuePrompt = captain;
            textEdit.Properties.NullValuePromptShowForEmptyValue = true;
            textEdit.Properties.ShowNullValuePromptWhenFocused = true;
            item.Control = textEdit;
            item.Text = captain;

            return item;
        }
        /// <summary>
        /// 创建备注控件
        /// </summary>
        /// <param name="captain">标签</param>
        /// <param name="prop">属性</param>
        /// <param name="max_length">最大长度</param>
        /// <param name="is_readonly">只读</param>
        /// <returns></returns>
        public LayoutControlItem CreateMemoItem(string captain, string prop, int max_length, bool is_readonly = false)
        {
            LayoutControlItem item = new LayoutControlItem();
            MemoEdit textEdit = new MemoEdit();
            textEdit.Properties.ReadOnly = is_readonly;
            textEdit.DataBindings.Add("EditValue", bindingSource, prop);
            textEdit.Properties.MaxLength = max_length;
            textEdit.Properties.AcceptsReturn = true;
            textEdit.Properties.AcceptsTab = true;
            textEdit.Properties.WordWrap = true;
            textEdit.Properties.NullValuePrompt = captain;
            textEdit.Properties.NullValuePromptShowForEmptyValue = true;
            textEdit.Properties.ShowNullValuePromptWhenFocused = true;
            item.Control = textEdit;
            item.Text = captain;

            return item;
        }
        /// <summary>
        /// 创建计算器
        /// </summary>
        /// <param name="captain">标签</param>
        /// <param name="prop">属性</param>
        /// <param name="max_length">最大长度</param>
        /// <param name="precision">小数位数</param>
        /// <returns></returns>
        public LayoutControlItem CreateCalcItem(string captain, string prop, int max_length = 10, int precision = 8, bool is_readonly = false)
        {
            LayoutControlItem item = new LayoutControlItem();

            if (max_length < precision) max_length = precision;

            CalcEdit calcEdit = new CalcEdit();
            calcEdit.DataBindings.Add("EditValue", bindingSource, prop);
            calcEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Clear)});
            calcEdit.Properties.MaxLength = max_length;
            calcEdit.Properties.Precision = precision;
            calcEdit.Properties.ReadOnly = is_readonly;
            calcEdit.Properties.NullValuePrompt = captain;
            calcEdit.Properties.NullValuePromptShowForEmptyValue = true;
            calcEdit.Properties.ShowNullValuePromptWhenFocused = true;

            item.Control = calcEdit;
            item.Text = captain;

            return item;
        }
        /// <summary>
        /// 创建勾选下拉框
        /// </summary>
        /// <param name="captain">标签</param>
        /// <param name="prop">属性</param>
        /// <param name="item_list">下拉列表</param>
        /// <param name="select_all_captain">选择所有项按钮的标签</param>
        /// <param name="separator_char">分隔符</param>
        /// <returns></returns>
        public LayoutControlItem CreateCheckedComboItem(string captain, string prop, List<string> item_list, string select_all_captain = "(Select All)", char separator_char = ',', bool is_readonly = false)
        {
            LayoutControlItem item = new LayoutControlItem();

            CheckedComboBoxEdit checkedComboBoxEdit = new CheckedComboBoxEdit();
            checkedComboBoxEdit.DataBindings.Add("EditValue", bindingSource, prop);
            checkedComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            foreach (string str in item_list)
            {
                checkedComboBoxEdit.Properties.Items.Add(str);
            }
            checkedComboBoxEdit.Properties.DropDownRows = 20;
            checkedComboBoxEdit.Properties.ReadOnly = is_readonly;
            checkedComboBoxEdit.Properties.NullValuePrompt = captain;
            checkedComboBoxEdit.Properties.NullValuePromptShowForEmptyValue = true;
            checkedComboBoxEdit.Properties.SelectAllItemCaption = select_all_captain;
            checkedComboBoxEdit.Properties.SeparatorChar = separator_char;
            checkedComboBoxEdit.Properties.ShowNullValuePromptWhenFocused = true;

            item.Control = checkedComboBoxEdit;
            item.Text = captain;

            return item;
        }
        public LayoutControlItem CreateComboItem(string captain, string prop,
            object data, string display_name = "Value", string value_name = "Id", bool is_readonly = false)
        {
            LayoutControlItem item = new LayoutControlItem();

            LookUpEdit textEdit = new LookUpEdit();
            textEdit.DataBindings.Add("EditValue", bindingSource, prop);
            textEdit.Properties.DataSource = data;
            textEdit.Properties.DisplayMember = display_name;
            textEdit.Properties.ValueMember = value_name;
            textEdit.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(display_name));
            textEdit.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(value_name));
            textEdit.Properties.Columns[1].Visible = false;
            textEdit.Properties.ShowHeader = false;
            textEdit.Properties.NullText = "Please Select";
            textEdit.Properties.DropDownRows = 20;
            textEdit.Properties.ReadOnly = is_readonly;
            textEdit.Properties.NullValuePrompt = captain;
            textEdit.Properties.NullValuePromptShowForEmptyValue = true;
            textEdit.Properties.ShowNullValuePromptWhenFocused = true;
            item.Control = textEdit;
            item.Text = captain;

            return item;
        }
        /// <summary>
        /// 创建转动控件
        /// </summary>
        /// <param name="captain">标签</param>
        /// <param name="prop">属性</param>
        /// <param name="max">最大值</param>
        /// <param name="min">最小值</param>
        /// <param name="increment">每次转动值</param>
        /// <param name="float_value">是否小数</param>
        /// <param name="is_readonly">只读</param>
        /// <returns></returns>
        public LayoutControlItem CreateSpinItem(string captain, string prop, decimal max = 20, decimal min = 0, int increment = 1,
            bool float_value = false, bool is_readonly = false)
        {
            LayoutControlItem item = new LayoutControlItem();

            SpinEdit textEdit = new SpinEdit();
            textEdit.DataBindings.Add("EditValue", bindingSource, prop);
            textEdit.Properties.IsFloatValue = float_value;
            textEdit.Properties.MinValue = min;
            textEdit.Properties.MaxValue = max;
            textEdit.Properties.Increment = increment;
            textEdit.Properties.ReadOnly = is_readonly;
            textEdit.Properties.NullValuePrompt = captain;
            textEdit.Properties.NullValuePromptShowForEmptyValue = true;
            textEdit.Properties.ShowNullValuePromptWhenFocused = true;
            item.Control = textEdit;
            item.Text = captain;
            item.Enabled = !is_readonly;

            return item;
        }
        public LayoutControlItem CreateSearchFindItem(string captain, string prop,
            object data, string display_name = "Value", string value_name = "Id", bool is_readonly = false)
        {
            LayoutControlItem item = new LayoutControlItem();

            DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            searchLookUpEdit1View.OptionsView.EnableAppearanceEvenRow = true;
            searchLookUpEdit1View.OptionsView.ShowAutoFilterRow = true;
            searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            searchLookUpEdit1View.OptionsView.ColumnAutoWidth = false;

            SearchLookUpEdit textEdit = new SearchLookUpEdit();
            textEdit.DataBindings.Add("EditValue", bindingSource, prop);
            textEdit.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            textEdit.Properties.DataSource = data;
            textEdit.Properties.DisplayMember = display_name;
            textEdit.Properties.ValueMember = value_name;
            textEdit.Properties.ReadOnly = is_readonly;
            textEdit.Properties.NullValuePrompt = captain;
            textEdit.Properties.NullValuePromptShowForEmptyValue = true;
            textEdit.Properties.ShowNullValuePromptWhenFocused = true;
            textEdit.Properties.View = searchLookUpEdit1View;
            item.Control = textEdit;
            item.Text = captain;

            return item;
        }
        /// <summary>
        /// 创建日期控件
        /// </summary>
        /// <param name="captain">标签</param>
        /// <param name="prop">属性</param>
        /// <returns></returns>
        public LayoutControlItem CreateDateItem(string captain, string prop, bool is_readonly = false)
        {
            LayoutControlItem item = new LayoutControlItem();

            DateEdit textEdit = new DateEdit();
            textEdit.DataBindings.Add("EditValue", bindingSource, prop);
            textEdit.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.TouchUI;
            textEdit.Properties.ReadOnly = is_readonly;
            textEdit.Properties.NullValuePrompt = captain;
            textEdit.Properties.NullValuePromptShowForEmptyValue = true;
            textEdit.Properties.ShowNullValuePromptWhenFocused = true;
            item.Control = textEdit;
            item.Text = captain;

            return item;
        }
        /// <summary>
        /// 创建时间控件
        /// </summary>
        /// <param name="captain">标签</param>
        /// <param name="prop">属性</param>
        /// <returns></returns>
        public LayoutControlItem CreateTimeItem(string captain, string prop, bool is_readonly = false)
        {
            LayoutControlItem item = new LayoutControlItem();

            TimeEdit textEdit = new TimeEdit();
            textEdit.DataBindings.Add("EditValue", bindingSource, prop);
            textEdit.Properties.TimeEditStyle = DevExpress.XtraEditors.Repository.TimeEditStyle.TouchUI;
            textEdit.Properties.ReadOnly = is_readonly;
            textEdit.Properties.NullValuePrompt = captain;
            textEdit.Properties.NullValuePromptShowForEmptyValue = true;
            textEdit.Properties.ShowNullValuePromptWhenFocused = true;
            item.Control = textEdit;
            item.Text = captain;

            return item;
        }
        public LayoutControlItem CreateCheckItem(string captain, string prop, bool is_readonly = false)
        {
            LayoutControlItem item = new LayoutControlItem();

            CheckEdit textEdit = new CheckEdit();
            textEdit.DataBindings.Add("EditValue", bindingSource, prop);
            textEdit.Text = captain;
            textEdit.ReadOnly = is_readonly;
            item.Control = textEdit;
            item.Text = captain;

            return item;
        }
        /// <summary>
        /// 创建单选控件
        /// </summary>
        /// <param name="captain">标签</param>
        /// <param name="prop">属性</param>
        /// <param name="off_value">不选时的值</param>
        /// <param name="on_value">勾选时的值</param>
        /// <param name="off_text">不选时的文本</param>
        /// <param name="on_text">勾选时的文本</param>
        /// <param name="is_readonly">只读</param>
        /// <returns></returns>
        public LayoutControlItem CreateToggleSwitchItem(string captain, string prop, object off_value, object on_value, string off_text = "Off", string on_text = "On", bool is_readonly = false)
        {
            LayoutControlItem item = new LayoutControlItem();

            ToggleSwitch toggleSwitch = new ToggleSwitch();
            toggleSwitch.DataBindings.Add("EditValue", bindingSource, prop);
            toggleSwitch.Properties.OffText = off_text;
            toggleSwitch.Properties.OnText = on_text;
            toggleSwitch.Properties.ValueOff = off_value;
            toggleSwitch.Properties.ValueOn = on_value;
            toggleSwitch.Properties.ReadOnly = is_readonly;

            item.Control = toggleSwitch;
            item.Text = captain;

            return item;
        }
        public LayoutControlItem CreateRadioItem(string captain, string prop, int column_count, List<Tuple<int,string>> item_list, string mask, bool is_readonly = false)
        {            
            LayoutControlItem item = new LayoutControlItem();
            RadioGroup textEdit = new RadioGroup();
            textEdit.Properties.ReadOnly = is_readonly;
            textEdit.DataBindings.Add("EditValue", bindingSource, prop);
            textEdit.Properties.Columns = column_count;
            foreach (Tuple<int, string> val in item_list)
                textEdit.Properties.Items.Add(new DevExpress.XtraEditors.Controls.RadioGroupItem(val.Item1, val.Item2));
            item.Control = textEdit;
            item.Text = captain;

            return item;
        }
        public LayoutControlItem CreateListBoxItem(string captain, string prop, object data, string display_name = "Value", string value_name = "Id", bool is_readonly = false)
        {
            LayoutControlItem item = new LayoutControlItem();
            CheckedListBoxControl textEdit = new CheckedListBoxControl();
            textEdit.Enabled = !is_readonly;
            textEdit.DataBindings.Add("EditValue", bindingSource, prop);
            textEdit.DataSource = data;
            textEdit.DisplayMember = display_name;
            textEdit.ValueMember = value_name;
            textEdit.SelectionMode = SelectionMode.MultiSimple;
            item.Control = textEdit;
            item.Text = captain;

            return item;
        }
        public LayoutControlItem CreateTrackItem(string captain, string prop, int min_value, int max_value, bool show_lable=false, bool is_readonly = false)
        {
            LayoutControlItem item = new LayoutControlItem();
            TrackBarControl textEdit = new TrackBarControl();
            int step = max_value - min_value > 5 ? 5 : 1;
            textEdit.Properties.ReadOnly = is_readonly;
            textEdit.DataBindings.Add("EditValue", bindingSource, prop);
            textEdit.Properties.Minimum = min_value;
            textEdit.Properties.Maximum = max_value;
            for (int i = min_value; i <= max_value; i += step)
                textEdit.Properties.Labels.Add(new DevExpress.XtraEditors.Repository.TrackBarLabel(i.ToString(), i));
            item.Control = textEdit;
            item.Text = captain;

            return item;
        }
        public LayoutControlItem CreateRangeTrackItem(string captain, string prop, int min_value, int max_value, bool show_lable = false, bool is_readonly = false)
        {
            LayoutControlItem item = new LayoutControlItem();
            RangeTrackBarControl textEdit = new RangeTrackBarControl();
            int step = max_value - min_value > 5 ? 5 : 1;
            textEdit.Properties.ReadOnly = is_readonly;
            textEdit.DataBindings.Add("EditValue", bindingSource, prop);
            textEdit.Properties.Minimum = min_value;
            textEdit.Properties.Maximum = max_value;
            for (int i = min_value; i <= max_value; i += step)
                textEdit.Properties.Labels.Add(new DevExpress.XtraEditors.Repository.TrackBarLabel(i.ToString(), i));
            item.Control = textEdit;
            item.Text = captain;

            return item;
        }
        #endregion

        public LayoutControlItem CreateGridListViewItem(ClassDesc subclass)
        {
            LayoutControlItem item = new LayoutControlItem();
            BaseViewControl baseViewControl = new BaseViewControl();
            baseViewControl.BindData(new DynamicObjectHelper(subclass));
            baseViewControl.HideFilter();
            baseViewControl.SimpleEditor();
            item.Control = baseViewControl;
            item.TextVisible = false;
            item.SizeConstraintsType = SizeConstraintsType.Custom;
            item.ControlMinSize = new Size(0, 800);

            subclassviewList.Add(baseViewControl);

            return item;
        }
        public void BindData(object objclass)
        {
            dynamicObject.objclass = objclass;
            BindData(dynamicObject);
        }
        public void BindData(DynamicObjectHelper dynamicObject)
        {
            if (dynamicObject.objclass != null)
            {
                this.bindingSource.DataSource = dynamicObject.objclass;
            }

            foreach (BaseViewControl baseViewControl in subclassviewList)
            {
                int column_id = dynamicObject.SubRelatedClass.Find(obj => obj.Sub_class_id == baseViewControl.dynamicObject.classDesc.Id).Main_class_column_id;
                string column_name = dynamicObject.ColumnList.Find(obj => obj.Id == column_id).Column_name;
                object id = dynamicObject.objclass.GetType().GetProperty(column_name).GetValue(dynamicObject.objclass);

                baseViewControl.RefreshView(id);
            }
                
        }
        public void BindDataChanged()
        {

        }
        public void CreateLayoutEditor(DynamicObjectHelper dynamicObject)
        {
            if (dynamicObject.classDesc == null)
                return;

            this.dynamicObject = dynamicObject;
            bindingSource.DataSource = dynamicObject.objclass;

            int group_idx = 1;

            while (true)
            {
                List<TableColumnDesc> list = dynamicObject.ColumnList.FindAll(obj => obj.Group_idx == group_idx);

                if (list.Count > 0)
                {
                    string group_text = String.Empty;

                    TableColumnDisplay display = dynamicObject.ColumnLanguageDisplayList.Find(obj => obj.Id == list[0].Id);
                    if (display != null)
                    {
                        group_text = display.Column_display_name;
                    }
                    LayoutControlGroup grp = this.CreateLayoutGroup(group_text);

                    #region 子Group
                    (int min, int max) tuple = (list.Min(obj => obj.Sub_group_idx), list.Min(obj => obj.Sub_group_idx));

                    if (tuple.min > 0)
                    {
                        for (int k = tuple.min; k <= tuple.max; k++)
                        {
                            List<TableColumnDesc> sub_list = list.FindAll(obj => obj.Sub_group_idx == k);

                            grp.Add(this.CreateLayoutGroup(sub_list, dynamicObject.ColumnLanguageDisplayList, dynamicObject.ColumnReferenceList, dynamicObject.ColumnRegularList));
                        }
                    }
                    #endregion

                    #region 单个Group
                    else
                    {
                        grp = this.CreateLayoutGroup(list, dynamicObject.ColumnLanguageDisplayList, dynamicObject.ColumnReferenceList, dynamicObject.ColumnRegularList);
                    }
                    #endregion

                    this.layoutEdit.AddGroup(grp);
                }
                else
                    break;

                group_idx++;
            }

            List<ClassRelated> sub_class = GlobeData.classRelatedList.FindAll(obj => obj.Main_class_id == dynamicObject.classDesc.Id);
            foreach (ClassRelated sub in sub_class)
            {
                ClassDesc subclass = GlobeData.classDescList.Find(obj => obj.Id == sub.Sub_class_id);

                LayoutControlGroup grp1 = this.CreateLayoutGroup(subclass.Class_name);
                LayoutControlItem item = this.CreateGridListViewItem(subclass);                
                grp1.AddItem(item);
                layoutEdit.AddGroup(grp1);
            }
        }
        private LayoutControlGroup CreateLayoutGroup(List<TableColumnDesc> column_list,List<TableColumnDisplay> display_list,List<TableColumnReference> reference_list,List<TableColumnRegular> rule_list)
        {
            string group_text = String.Empty;
            string sub_group_text = String.Empty;

            TableColumnDisplay display = display_list.Find(obj => obj.Id == column_list[0].Id);
            if(display!=null)
            {
                group_text = display.Column_display_name;
                sub_group_text = display.Sub_group_display_name;
            }
            LayoutControlGroup grp = this.CreateLayoutGroup(sub_group_text==String.Empty?group_text:sub_group_text);

            foreach (TableColumnDesc col in column_list)
            {
                TableColumnDisplay column_display = display_list.Find(obj => obj.Id == col.Id);
                TableColumnReference column_reference = reference_list.Find(obj => obj.Id == col.Id);
                TableColumnRegular column_rule=rule_list.Find(obj => obj.Id == col.Id);

                string column_text = column_display!=null?column_display.Column_display_name:col.Column_name;
                string column_ref_value = GlobeData.tableColumnDescList.Find(obj => obj.Column_idx == column_reference.Reference_value_column_id).Column_name;
                string column_ref_text = GlobeData.tableColumnDescList.Find(obj => obj.Column_idx == column_reference.Reference_text_column_id).Column_name;
                var ref_data=new object();

                LayoutControlItem itm=new LayoutControlItem();

                if(column_reference != null)
                {
                    ClassDesc tmp = GlobeData.classDescList.Find(obj => obj.Id == column_reference.Reference_id);

                    if (tmp != null)
                    {
                        object ref_class = GlobeData.assembly.CreateInstance(tmp.Class_name);
                        ref_data = ref_class.GetType().InvokeMember("GetDataList", BindingFlags.Default | BindingFlags.InvokeMethod, null, ref_class, null);
                    }
                }

                    
                switch(col.Column_edit_type)
                {
                    case ObjectEnum.ColumnEditType.Calc:
                        itm = this.CreateCalcItem(column_text, col.Column_name, 10, 8, col.Is_readonly);break;
                    case ObjectEnum.ColumnEditType.Check:
                        itm = this.CreateCheckItem(column_text, col.Column_name, col.Is_readonly);break;
                    case ObjectEnum.ColumnEditType.CheckedCombo:
                        itm = this.CreateCheckedComboItem(column_text, col.Column_name, column_rule != null ? column_rule.ItemList.Split(',').ToList(): new List<String>(), "(Select All)", ',', col.Is_readonly);break;
                    case ObjectEnum.ColumnEditType.Combo:
                        itm = this.CreateComboItem(column_text, col.Column_name, ref_data, column_ref_text,
                        column_ref_value,
                        col.Is_readonly);break;
                    case ObjectEnum.ColumnEditType.Date:
                        itm = this.CreateDateItem(column_text, col.Column_name, col.Is_readonly);break;
                    case ObjectEnum.ColumnEditType.Grid:break;
                    case ObjectEnum.ColumnEditType.LookUp:
                        itm = this.CreateSearchFindItem(column_text, col.Column_name,
                        ref_data,
                        column_ref_text,
                        column_ref_value,
                        col.Is_readonly);break;
                    case ObjectEnum.ColumnEditType.Memo:
                        itm = this.CreateMemoItem(column_text, col.Column_name, column_rule != null ? column_rule.Max_length : 65535, col.Is_readonly);break;
                    case ObjectEnum.ColumnEditType.Spin:
                        itm = this.CreateSpinItem(column_text, col.Column_name,
                            column_rule != null ? column_rule.Max_value : int.MaxValue,
                            column_rule != null ? column_rule.Min_value : int.MinValue,
                            1, false, col.Is_readonly);break;
                    case ObjectEnum.ColumnEditType.Text:
                        itm = this.CreateTextItem(column_text, col.Column_name, column_rule != null ? column_rule.Max_length : 65535, 
                            column_rule!=null?column_rule.Mask_type:MaskType.None,
                            column_rule!=null?column_rule.Mask:String.Empty,
                            col.Is_readonly); break;
                    case ObjectEnum.ColumnEditType.Time:
                        itm=this.CreateTimeItem(column_text, col.Column_name, col.Is_readonly); break;
                    case ObjectEnum.ColumnEditType.Toggle:
                        itm = this.CreateToggleSwitchItem(column_text, col.Column_name,
                            column_rule != null ? column_rule.Toggle_off_value : false,
                            column_rule != null ? column_rule.Toggle_on_value : true,
                            column_rule != null ? column_rule.Toggle_off_text : "Off",
                            column_rule != null ? column_rule.Toggle_on_text : "ON",
                            col.Is_readonly);break;
                }

                grp.AddItem(itm);
            }

            return grp;
        }
        public void SaveData()
        {
            var obj = this.bindingSource.DataSource;

            obj.GetType().InvokeMember("SaveData", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, null);
        }
        public void EndEdit()
        {
            this.bindingSource.EndEdit();
        }
        private void BindingSource_CurrentItemChanged(object sender, EventArgs e)
        {
            // if (!SplashGlobeInfo.is_load)
            this.BindDataChanged();
        }
    }
}
