using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DynamicCodeHelper.Entity
{
    /// <summary>
    /// 对象关系
    /// </summary>
    public class ClassRelated
    {
        int _auto_id;
        int _main_class_id;
        int _sub_class_id;
        int _main_class_column_id;
        int _sub_class_column_id;

        public int Auto_id { get => _auto_id; set => _auto_id = value; }
        /// <summary>
        /// 主对象编号
        /// </summary>
        public int Main_class_id { get => _main_class_id; set => _main_class_id = value; }
        /// <summary>
        /// 从对象关系
        /// </summary>
        public int Sub_class_id { get => _sub_class_id; set => _sub_class_id = value; }
        /// <summary>
        /// 主键
        /// </summary>
        public int Main_class_column_id { get => _main_class_column_id; set => _main_class_column_id = value; }
        /// <summary>
        /// 外键
        /// </summary>
        public int Sub_class_column_id { get => _sub_class_column_id; set => _sub_class_column_id = value; }
    }
}
