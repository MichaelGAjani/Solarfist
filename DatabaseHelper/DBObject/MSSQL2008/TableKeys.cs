using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBObject.MSSQL2008
{
   public class TableKeys:DBSimpleObject
    {
        int _unique_index_id;
        bool _is_system_named;

        public int Unique_index_id { get => _unique_index_id; set => _unique_index_id = value; }
        public bool Is_system_named { get => _is_system_named; set => _is_system_named = value; }
    }
}
