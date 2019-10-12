using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBObject.MSSQL2008
{
    public class TableCK:DBSimpleObject
    {
        bool _is_disabled;
        bool _is_not_for_replication;
        bool _is_not_trusted;
        int _parent_column_id;
        string _definition;
        bool _uses_database_collation;
        bool _is_system_named;

        public bool Is_disabled { get => _is_disabled; set => _is_disabled = value; }
        public bool Is_not_for_replication { get => _is_not_for_replication; set => _is_not_for_replication = value; }
        public bool Is_not_trusted { get => _is_not_trusted; set => _is_not_trusted = value; }
        public int Parent_column_id { get => _parent_column_id; set => _parent_column_id = value; }
        public string Definition { get => _definition; set => _definition = value; }
        public bool Uses_database_collation { get => _uses_database_collation; set => _uses_database_collation = value; }
        public bool Is_system_named { get => _is_system_named; set => _is_system_named = value; }
    }
}
