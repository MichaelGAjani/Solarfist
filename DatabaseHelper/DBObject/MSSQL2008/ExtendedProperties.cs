using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBObject.MSSQL2008
{
    public class ExtendedProperties
    {
        int _class;
        string _class_desc;
        int _major_id;
        int _minor_id;
        string _name;
        string _value;

        public int Class { get => _class; set => _class = value; }
        public string Class_desc { get => _class_desc; set => _class_desc = value; }
        public int Major_id { get => _major_id; set => _major_id = value; }
        public int Minor_id { get => _minor_id; set => _minor_id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Value { get => _value; set => _value = value; }
    }
}
