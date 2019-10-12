using DevExpress.XtraEditors.Mask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DynamicCodeHelper.Entity
{
    public class TableColumnRegular
    {
        int _id;
        MaskType _mask_type;
        string _mask;
        int _min_value;
        int _max_value;
        int _max_length;
        string _item;
        object _toggle_on_value;
        object _toggle_off_value;
        string _toggle_on_text;
        string _toggle_off_text;

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get => _id; set => _id = value; }
        /// <summary>
        /// 掩码类型
        /// </summary>
        public MaskType Mask_type { get => _mask_type; set => _mask_type = value; }
        /// <summary>
        /// 掩码
        /// </summary>
        public string Mask { get => _mask; set => _mask = value; }
        /// <summary>
        /// 最小值
        /// </summary>
        public int Min_value { get => _min_value; set => _min_value = value; }
        /// <summary>
        /// 最大值
        /// </summary>
        public int Max_value { get => _max_value; set => _max_value = value; }
        /// <summary>
        /// 最大长度
        /// </summary>
        public int Max_length { get => _max_length; set => _max_length = value; }
        public string ItemList { get => _item; set => _item = value; }
        public object Toggle_on_value { get => _toggle_on_value; set => _toggle_on_value = value; }
        public object Toggle_off_value { get => _toggle_off_value; set => _toggle_off_value = value; }
        public string Toggle_on_text { get => _toggle_on_text; set => _toggle_on_text = value; }
        public string Toggle_off_text { get => _toggle_off_text; set => _toggle_off_text = value; }
    }
}
