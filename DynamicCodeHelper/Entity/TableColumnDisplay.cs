using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DynamicCodeHelper.Entity
{
    public class TableColumnDisplay
    {
        int _id;
        Entity.ObjectEnum.LanguageCode _language_id;
        string _column_display_name;
        string _group_display_name;
        string _sub_group_display_name;

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get => _id; set => _id = value; }
        /// <summary>
        /// 语言编号
        /// </summary>
        public Entity.ObjectEnum.LanguageCode Language_id { get => _language_id; set => _language_id = value; }
        /// <summary>
        /// 列显示的文本
        /// </summary>
        public string Column_display_name { get => _column_display_name; set => _column_display_name = value; }
        /// <summary>
        /// 组显示的文本
        /// </summary>
        public string Group_display_name { get => _group_display_name; set => _group_display_name = value; }
        /// <summary>
        /// 子组显示的文本
        /// </summary>
        public string Sub_group_display_name { get => _sub_group_display_name; set => _sub_group_display_name = value; }
    }
}
