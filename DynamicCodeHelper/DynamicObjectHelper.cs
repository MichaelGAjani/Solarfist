using Jund.DatabaseHelper.DALHelper;
using Jund.DynamicCodeHelper.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Jund.DynamicCodeHelper.Entity.ObjectEnum;

namespace Jund.DynamicCodeHelper
{
    public class DynamicObjectHelper
    {
        /// <summary>
        /// 数据模型
        /// </summary>
        public object objclass { get; set; } = new object();
        /// <summary>
        /// 类(数据模型)描述
        /// </summary>
        public ClassDesc classDesc { get; set; } = new ClassDesc();
        /// <summary>
        /// 子类描述
        /// </summary>
        public List<ClassRelated> SubRelatedClass => GlobeData.classRelatedList.FindAll(obj => obj.Main_class_id == classDesc.Id);
        /// <summary>
        /// 类属性描述
        /// </summary>
        public List<TableColumnDesc> ColumnList => GlobeData.tableColumnDescList.FindAll(obj => obj.Class_name == classDesc.Class_name);
        /// <summary>
        /// 类界面显示
        /// </summary>
        public List<TableColumnDisplay> ColumnDisplayList => GlobeData.tableColumnDisplayList.FindAll(obj => ColumnList.Exists(obj2 => obj2.Id == obj.Id));
        /// <summary>
        /// 类属性ID引用外部对象
        /// </summary>
        public List<TableColumnReference> ColumnReferenceList => GlobeData.tableColumnReferenceList.FindAll(obj => ColumnList.Exists(obj2 => obj2.Id == obj.Id));
        /// <summary>
        /// 类属性规则
        /// </summary>
        public List<TableColumnRegular> ColumnRegularList => GlobeData.tableColumnRegularList.FindAll(obj => ColumnList.Exists(obj2 => obj2.Id == obj.Id));
        /// <summary>
        /// 类界面显示-指定语言
        /// </summary>
        public List<TableColumnDisplay> ColumnLanguageDisplayList => ColumnDisplayList.FindAll(obj => obj.Language_id == GlobeSetting.language);
        private List<(SaveDataType type,object data)> tempData{ get; set; }

        public DynamicObjectHelper(string className)
        {
            this.classDesc = GlobeData.classDescList.Find(obj=>obj.Class_name==className);
            objclass = NetCodeComplierHelper.ClassObject(className);
        }
        public DynamicObjectHelper(ClassDesc classDesc)
        {
            this.classDesc = classDesc;
            objclass = NetCodeComplierHelper.ClassObject(classDesc.Class_name);
        }
        public DynamicObjectHelper(ClassDesc classDesc, object objclass)
        {
            this.classDesc = classDesc;
            this.objclass = objclass;
        }

        public List<object> GetList(DataTable table)
        {
            List<object> list = new List<object>();

            object data=objclass.GetType().InvokeMember("GetList", BindingFlags.Default | BindingFlags.InvokeMethod, null, objclass, new object[] { table });

            int count=Convert.ToInt32(data.GetType().GetProperty("Count").GetValue(data));

            for(int i=0;i<count;i++)
            {
                object item=data.GetType().InvokeMember("get_Item", BindingFlags.Default | BindingFlags.InvokeMethod, null, data, new object[] { i });

                list.Add(item);
            }

            return list;
        }
        public List<object> DataList => GetList(this.GetDataTable());
        public object ExecObjectFunc(string funcName, List<object> parameters) => objclass.GetType().InvokeMember(funcName,
            BindingFlags.Default | BindingFlags.InvokeMethod, null, objclass, parameters.ToArray());
        public object GetValue(string attribute) => objclass.GetType().GetProperty(attribute).GetValue(objclass);
        public void SetValue((string attribute, object value) tuple) => objclass.GetType().GetProperty(tuple.attribute).SetValue(objclass, tuple.value);
        public void Create()=> objclass = NetCodeComplierHelper.ClassObject(classDesc.Class_name);
        public void Insert()
        {
            //PropertyInfo[] propertys = objclass.GetType().GetProperties();

            //var sql = new object();
            //switch(GlobeSetting.dbtype)
            //{
            //    case ObjectEnum.DatabaseType.MySql:sql = new MySqlService();break;
            //}
            //StringBuilder queryParameters = new StringBuilder();

            //foreach (PropertyInfo prop in propertys)
            //{
            //    if (prop.CanWrite)//&&prop.Name.ToLower()!="id")
            //    {
            //        string data_type = prop.PropertyType.Name.ToLower();

            //        if (data_type == "list`1" || data_type == "clubsub")
            //            continue;

            //        switch (data_type)
            //        {
            //            case "string": sql.AddParameter("@" + prop.Name, DbType.String, prop.GetValue(obj, null)); queryParameters.Append(",@" + prop.Name); break;
            //            case "int32": sql.AddParameter("@" + prop.Name, DbType.Int32, prop.GetValue(obj, null)); queryParameters.Append(",@" + prop.Name); break;
            //            case "decimal": sql.AddParameter("@" + prop.Name, DbType.Decimal, prop.GetValue(obj, null)); queryParameters.Append(",@" + prop.Name); break;
            //            case "datetime": sql.AddParameter("@" + prop.Name, DbType.DateTime, prop.GetValue(obj, null)); queryParameters.Append(",@" + prop.Name); break;
            //            case "int64": sql.AddParameter("@" + prop.Name, DbType.Int64, prop.GetValue(obj, null)); queryParameters.Append(",@" + prop.Name); break;
            //            case "boolean": sql.AddParameter("@" + prop.Name, DbType.Boolean, prop.GetValue(obj, null)); queryParameters.Append(",@" + prop.Name); break;
            //            case "double": sql.AddParameter("@" + prop.Name, DbType.Double, prop.GetValue(obj, null)); queryParameters.Append(",@" + prop.Name); break;
            //        }
            //    }
            //}

            //string query = String.Format("Insert Into [" + table + "] ({0}) Values ({1})", queryParameters.ToString().Replace("@", "").TrimStart(',').ToLower(), queryParameters.ToString().TrimStart(',').ToLower());

            //SQLiteDataReader reader = sql.ExecuteSqlReader(query);

            //int record = reader.RecordsAffected;

            //reader.Close();

            //return record > 0;
        }
        public void Update()
        {
            //PropertyInfo[] propertys = obj.GetType().GetProperties();

            //SQLiteService sql = new SQLiteService();
            //StringBuilder queryParameters = new StringBuilder();

            //foreach (PropertyInfo prop in propertys)
            //{
            //    if (prop.CanWrite)
            //    {
            //        string data_type = prop.PropertyType.Name.ToLower();

            //        switch (data_type)
            //        {
            //            case "string":
            //                sql.AddParameter("@" + prop.Name.ToLower(), DbType.String, prop.GetValue(obj, null));
            //                queryParameters.Append("," + prop.Name.ToLower() + "=@" + prop.Name.ToLower()); break;
            //            case "int32":
            //                sql.AddParameter("@" + prop.Name.ToLower(), DbType.Int32, prop.GetValue(obj, null));
            //                queryParameters.Append("," + prop.Name.ToLower() + "=@" + prop.Name.ToLower()); break;
            //            case "decimal":
            //                sql.AddParameter("@" + prop.Name.ToLower(), DbType.Decimal, prop.GetValue(obj, null));
            //                queryParameters.Append("," + prop.Name.ToLower() + "=@" + prop.Name.ToLower()); break;
            //            case "datetime":
            //                sql.AddParameter("@" + prop.Name.ToLower(), DbType.DateTime, prop.GetValue(obj, null));
            //                queryParameters.Append("," + prop.Name.ToLower() + "=@" + prop.Name.ToLower()); break;
            //            case "int64":
            //                sql.AddParameter("@" + prop.Name.ToLower(), DbType.Int64, prop.GetValue(obj, null));
            //                queryParameters.Append("," + prop.Name.ToLower() + "=@" + prop.Name.ToLower()); break;
            //            case "boolean":
            //                sql.AddParameter("@" + prop.Name.ToLower(), DbType.Boolean, prop.GetValue(obj, null));
            //                queryParameters.Append("," + prop.Name.ToLower() + "=@" + prop.Name.ToLower()); break;
            //            case "double":
            //                sql.AddParameter("@" + prop.Name.ToLower(), DbType.Double, prop.GetValue(obj, null));
            //                queryParameters.Append("," + prop.Name.ToLower() + "=@" + prop.Name.ToLower()); break;
            //        }
            //    }
            //}

            //string query = String.Format("Update [" + table + "] Set {0} Where id = @id", queryParameters.ToString().TrimStart(','));

            //SQLiteDataReader reader = sql.ExecuteSqlReader(query);

            //int record = reader.RecordsAffected;

            //reader.Close();

            //return record > 0;
        }
        public void Delete()
        {

        }
        public void SaveData()
        {

        }
        public DataTable GetDataTable()
        {
            string query = "SELECT * FROM " + objclass.ToString();

            return new DataTable();
        }
        public void GetByID()
        {

        }
        public void GetByQueryString()
        {

        }
        
    }
}
