using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DynamicCodeHelper.Entity
{
    public class ClassDesc
    {
        int _id;
        int _table_id;
        string _table_name;
        string _class_name;
        int _class_type;
        bool _can_create;
        bool _can_modify;
        bool _can_remove;
        bool _visible;
        int _value_id;
        int _text_id;

        /// <summary>
        /// 对象编号
        /// </summary>
        public int Id { get => _id; set => _id = value; }
        /// <summary>
        /// 表编号
        /// </summary>
        public int Table_id { get => _table_id; set => _table_id = value; }
        /// <summary>
        /// 表名称
        /// </summary>
        public string Table_name { get => _table_name; set => _table_name = value; }
        /// <summary>
        /// 类名称
        /// </summary>
        public string Class_name { get => _class_name; set => _class_name = value; }
        /// <summary>
        /// 对象类型
        /// </summary>
        public int Class_type { get => _class_type; set => _class_type = value; }
        /// <summary>
        /// 可以创建
        /// </summary>
        public bool Can_create { get => _can_create; set => _can_create = value; }
        /// <summary>
        /// 可以编辑
        /// </summary>
        public bool Can_modify { get => _can_modify; set => _can_modify = value; }
        /// <summary>
        /// 可以移除
        /// </summary>
        public bool Can_remove { get => _can_remove; set => _can_remove = value; }
        /// <summary>
        /// 可见
        /// </summary>
        public bool Visible { get => _visible; set => _visible = value; }
        public int Value_id { get => _value_id; set => _value_id = value; }
        public int Text_id { get => _text_id; set => _text_id = value; }
    }
}
