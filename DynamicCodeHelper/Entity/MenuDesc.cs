using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DynamicCodeHelper.Entity
{
    public class MenuDesc
    {
        int _id;
        bool _is_root;
        string _captain;
        int _class_id;
        int _external_program_id;
        int _upper_id;
        int _idx;
        bool _visible;
        bool _system_menu;

        public int Id { get => _id; set => _id = value; }
        /// <summary>
        /// 根节点菜单
        /// </summary>
        public bool Is_root { get => _is_root; set => _is_root = value; }
        public string Captain { get => _captain; set => _captain = value; }
        /// <summary>
        /// 对应的对象
        /// </summary>
        public int Class_id { get => _class_id; set => _class_id = value; }
        /// <summary>
        /// 对应的外部程序
        /// </summary>
        public int External_program_id { get => _external_program_id; set => _external_program_id = value; }
        public int Upper_id { get => _upper_id; set => _upper_id = value; }
        public int Idx { get => _idx; set => _idx = value; }
        public bool Visible { get => _visible; set => _visible = value; }
        public bool System_menu { get => _system_menu; set => _system_menu = value; }
    }
}
