using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBObject.MSSQL2008
{
    public class TableFK:DBSimpleObject
    {
        int _object_id;
        int _referenced_object_id;
        int _key_index_id;
        bool _is_disabled;
        bool _is_not_for_replication;
        bool _is_not_trusted;
        int _delete_referential_action;
        string _delete_referential_action_desc;
        int _update_referential_action;
        string _update_referential_action_desc;
        bool _is_system_named;

        //public int Id => this.Object_id;
        public int Referenced_object_id { get => _referenced_object_id; set => _referenced_object_id = value; }
        public int Key_index_id { get => _key_index_id; set => _key_index_id = value; }
        public bool Is_disabled { get => _is_disabled; set => _is_disabled = value; }
        public bool Is_not_for_replication { get => _is_not_for_replication; set => _is_not_for_replication = value; }
        public bool Is_not_trusted { get => _is_not_trusted; set => _is_not_trusted = value; }
        public int Delete_referential_action { get => _delete_referential_action; set => _delete_referential_action = value; }
        public string Delete_referential_action_desc { get => _delete_referential_action_desc; set => _delete_referential_action_desc = value; }
        public int Update_referential_action { get => _update_referential_action; set => _update_referential_action = value; }
        public string Update_referential_action_desc { get => _update_referential_action_desc; set => _update_referential_action_desc = value; }
        public bool Is_system_named { get => _is_system_named; set => _is_system_named = value; }
        public int Object_id { get => _object_id; set => _object_id = value; }
    }
}
