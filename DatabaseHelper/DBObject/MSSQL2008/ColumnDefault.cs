using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBObject.MSSQL2008
{
    public class ColumnDefault:DBSimpleObject
    {
        int _parent_column_id;
        string _definition;
        bool _is_system_named;

        public int Parent_column_id { get => _parent_column_id; set => _parent_column_id = value; }
        public string Definition { get => _definition; set => _definition = value; }
        public bool Is_system_named { get => _is_system_named; set => _is_system_named = value; }
    }
}
