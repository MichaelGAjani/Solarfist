using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBObject.MSSQL2008
{
    public class DBSimpleObject
    {
        int _object_id;
        string _name;
        int _principal_id;
        int _schema_id;
        int _parent_object_id;
        string _type;
        string _type_desc;
        DateTime _create_date;
        DateTime _modify_date;
        bool _is_ms_shipped;
        bool _is_published;
        bool _is_schema_published;

        public int Object_id { get => _object_id; set => _object_id = value; }
        public string Name { get => _name; set => _name = value; }
        public int Principal_id { get => _principal_id; set => _principal_id = value; }
        public int Schema_id { get => _schema_id; set => _schema_id = value; }
        public int Parent_object_id { get => _parent_object_id; set => _parent_object_id = value; }
        public string Type { get => _type; set => _type = value; }
        public string Type_desc { get => _type_desc; set => _type_desc = value; }
        public DateTime Create_date { get => _create_date; set => _create_date = value; }
        public DateTime Modify_date { get => _modify_date; set => _modify_date = value; }
        public bool Is_ms_shipped { get => _is_ms_shipped; set => _is_ms_shipped = value; }
        public bool Is_published { get => _is_published; set => _is_published = value; }
        public bool Is_schema_published { get => _is_schema_published; set => _is_schema_published = value; }
    }
}
