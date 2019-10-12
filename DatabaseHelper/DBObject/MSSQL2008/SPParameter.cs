using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBObject.MSSQL2008
{
    public class SPParameter
    {
        int _object_id;
        string _name;
        int _parameter_id;
        int _system_type_id;
        int _user_type_id;
        int _max_length;
        int _precision;
        int _scale;
        bool _is_output;
        bool _is_cursor_ref;
        bool _has_default_value;
        bool _is_xml_document;
        string _default_value;
        int _xml_collection_id;
        bool _is_readonly;
        bool _is_nullable;

        public int Object_id { get => _object_id; set => _object_id = value; }
        public string Name { get => _name; set => _name = value; }
        public int Parameter_id { get => _parameter_id; set => _parameter_id = value; }
        public int System_type_id { get => _system_type_id; set => _system_type_id = value; }
        public int User_type_id { get => _user_type_id; set => _user_type_id = value; }
        public int Max_length { get => _max_length; set => _max_length = value; }
        public int Precision { get => _precision; set => _precision = value; }
        public int Scale { get => _scale; set => _scale = value; }
        public bool Is_output { get => _is_output; set => _is_output = value; }
        public bool Is_cursor_ref { get => _is_cursor_ref; set => _is_cursor_ref = value; }
        public bool Has_default_value { get => _has_default_value; set => _has_default_value = value; }
        public bool Is_xml_document { get => _is_xml_document; set => _is_xml_document = value; }
        public string Default_value { get => _default_value; set => _default_value = value; }
        public int Xml_collection_id { get => _xml_collection_id; set => _xml_collection_id = value; }
        public bool Is_readonly { get => _is_readonly; set => _is_readonly = value; }
        public bool Is_nullable { get => _is_nullable; set => _is_nullable = value; }
    }
}
