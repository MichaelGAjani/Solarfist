using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DynamicCodeHelper.Entity
{
    public class TableColumnDesc
    {
        int _id;
        string _table_name;
        string _class_name;
        string _column_name;
        int _column_type;
        Entity.ObjectEnum.ColumnEditType _column_edit_type;
        int _column_idx;
        int _group_idx;
        int _sub_group_idx;
        bool _visible;
        bool _is_readonly;      

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get => _id; set => _id = value; }
        /// <summary>
        /// 表名
        /// </summary>
        public string Table_name { get => _table_name; set => _table_name = value; }
        /// <summary>
        /// 类名称
        /// </summary>
        public string Class_name { get => _class_name; set => _class_name = value; }
        /// <summary>
        /// 列名称
        /// </summary>
        public string Column_name { get => _column_name; set => _column_name = value; }
        /// <summary>
        /// 列数据类型
        /// </summary>
        public int Column_type { get => _column_type; set => _column_type = value; }
        /// <summary>
        /// 列编辑器类型
        /// </summary>
        public Entity.ObjectEnum.ColumnEditType Column_edit_type { get => _column_edit_type; set => _column_edit_type = value; }
        /// <summary>
        /// 列顺序序号
        /// </summary>
        public int Column_idx { get => _column_idx; set => _column_idx = value; }
        /// <summary>
        /// 分组序号
        /// </summary>
        public int Group_idx { get => _group_idx; set => _group_idx = value; }
        /// <summary>
        /// 子分组序号
        /// </summary>
        public int Sub_group_idx { get => _sub_group_idx; set => _sub_group_idx = value; }
        /// <summary>
        /// 可见
        /// </summary>
        public bool Visible { get => _visible; set => _visible = value; }
       
        /// <summary>
        /// 只读
        /// </summary>
        public bool Is_readonly { get => _is_readonly; set => _is_readonly = value; }
    }
}
