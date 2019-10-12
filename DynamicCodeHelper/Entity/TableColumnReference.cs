using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DynamicCodeHelper.Entity
{
    /// <summary>
    /// 表的列引用外部数据
    /// </summary>
    public class TableColumnReference
    {
        int _id;
        int _reference_id;
        int _reference_text_column_id;
        int _reference_value_column_id;

        /// <summary>
        /// 表编号
        /// </summary>
        public int Id { get => _id; set => _id = value; }
        /// <summary>
        /// 外键表编号
        /// </summary>
        public int Reference_id { get => _reference_id; set => _reference_id = value; }
        /// <summary>
        /// 外键显示文本的列编号
        /// </summary>
        public int Reference_text_column_id { get => _reference_text_column_id; set => _reference_text_column_id = value; }
        /// <summary>
        /// 外键显示值的列编号
        /// </summary>
        public int Reference_value_column_id { get => _reference_value_column_id; set => _reference_value_column_id = value; }
    }
}
