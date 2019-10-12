using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBInfoHelper.MSSQL2008
{
    public class SqlDQLHelper
    {
        public object GetTableData(object obj)
        {
            return obj.GetType().InvokeMember("GetDataList", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, null);
        }
        public List<object> GetDataList(object obj)
        {
            object data = this.GetTableData(obj);

            List<object> list = new List<object>();

            int count = Convert.ToInt32(data.GetType().GetProperty("Count").GetValue(data));

            for(int i=0;i<count;i++)
            {
                object itm= data.GetType().InvokeMember("get_Item", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, new object[] { i });

                list.Add(itm);
            }

            return list;

        }
    }
}
