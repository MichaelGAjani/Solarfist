using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.LogHelper
{
    public class StackInfo
    {
        string _full_name;
        string _method_type;
        string _moudle_name;

        public string FullName { get => _full_name; set => _full_name = value; }
        public string MethodType { get => _method_type; set => _method_type = value; }
        public string MoudleName { get => _moudle_name; set => _moudle_name = value; }
        public override string ToString()
        {
            return "Func Name:" + FullName + ";MethodType:" + MethodType + ";Moudle:" + MoudleName;
        }
    }
}
