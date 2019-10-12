using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jund.NETHelper.FileHelper.XmlFile
{
   public class XmlNodeObject
    {
        int _level;
        XmlNodeType _type;
        string _name;
        string _value;

        List<XmlNodeObject> _child_list = new List<XmlNodeObject>();

        public XmlNodeObject(int level, XmlNodeType type, string name, string value)
        {
            _level = level;
            _type = type;
            _name = name;
            _value = value;
        }

        public int Level { get => _level; set => _level = value; }
        public string Name { get => _name; set => _name = value; }
        public string Value { get => _value; set => _value = value; }
        public List<XmlNodeObject> Child_list { get => _child_list; set => _child_list = value; }
        public XmlNodeType NodeType { get => _type; set => _type = value; }
    }
}
