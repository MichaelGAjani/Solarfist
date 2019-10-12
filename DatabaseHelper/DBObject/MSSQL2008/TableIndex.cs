using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBObject.MSSQL2008
{
    public class TableIndex
    {
       int  _object_id;
      string _name;
      int _index_id;
      int _type;
      string _type_desc;
      bool _is_unique;
      int _data_space_id;
      bool _ignore_dup_key;
      bool _is_primary_key;
      bool _is_unique_constraint;
      int _fill_factor;
      bool _is_padded;
      bool _is_disabled;
      bool _is_hypothetical;
      bool _allow_row_locks;
      bool _allow_page_locks;
      bool _has_filter;
      string _filter_definition;
        bool _auto_created;
        bool _optimize_for_sequential_key;

        public int Object_id { get => _object_id; set => _object_id = value; }
        public string Name { get => _name; set => _name = value; }
        public int Index_id { get => _index_id; set => _index_id = value; }
        public int Type { get => _type; set => _type = value; }
        public string Type_desc { get => _type_desc; set => _type_desc = value; }
        public bool Is_unique { get => _is_unique; set => _is_unique = value; }
        public int Data_space_id { get => _data_space_id; set => _data_space_id = value; }
        public bool Ignore_dup_key { get => _ignore_dup_key; set => _ignore_dup_key = value; }
        public bool Is_primary_key { get => _is_primary_key; set => _is_primary_key = value; }
        public bool Is_unique_constraint { get => _is_unique_constraint; set => _is_unique_constraint = value; }
        public int Fill_factor { get => _fill_factor; set => _fill_factor = value; }
        public bool Is_padded { get => _is_padded; set => _is_padded = value; }
        public bool Is_disabled { get => _is_disabled; set => _is_disabled = value; }
        public bool Is_hypothetical { get => _is_hypothetical; set => _is_hypothetical = value; }
        public bool Allow_row_locks { get => _allow_row_locks; set => _allow_row_locks = value; }
        public bool Allow_page_locks { get => _allow_page_locks; set => _allow_page_locks = value; }
        public bool Has_filter { get => _has_filter; set => _has_filter = value; }
        public string Filter_definition { get => _filter_definition; set => _filter_definition = value; }
        public bool Auto_created { get => _auto_created; set => _auto_created = value; }
        public bool Optimize_for_sequential_key { get => _optimize_for_sequential_key; set => _optimize_for_sequential_key = value; }
    }
}
