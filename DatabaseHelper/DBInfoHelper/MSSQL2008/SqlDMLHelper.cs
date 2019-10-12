using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Jund.DatabaseHelper.DALHelper;
using System.Data;
using System.Data.SqlClient;

namespace Jund.DatabaseHelper.DBInfoHelper.MSSQL2008
{
    public class SqlDMLHelper
    {
        public bool InsertData(object obj,string table="")
        {
            if (table == String.Empty)
                table = this.GetTableName(obj);

            PropertyInfo[] propertys = obj.GetType().GetProperties();

            MSSqlService sql = new MSSqlService();
            StringBuilder queryParameters = new StringBuilder();

            foreach (PropertyInfo prop in propertys)
            {
                if (prop.CanWrite)//&&prop.Name.ToLower()!="id")
                {
                    string data_type = prop.PropertyType.Name.ToLower();

                    if (data_type == "list`1")
                        continue;

                    switch (data_type)
                    {
                        case "string": sql.AddParameter("@" + prop.Name, SqlDbType.VarChar, prop.GetValue(obj, null)); queryParameters.Append(",@" + prop.Name); break;
                        case "int32": sql.AddParameter("@" + prop.Name, SqlDbType.Int, prop.GetValue(obj, null)); queryParameters.Append(",@" + prop.Name); break;
                        case "decimal": sql.AddParameter("@" + prop.Name, SqlDbType.Decimal, prop.GetValue(obj, null)); queryParameters.Append(",@" + prop.Name); break;
                        case "datetime": sql.AddParameter("@" + prop.Name, SqlDbType.DateTime, prop.GetValue(obj, null)); queryParameters.Append(",@" + prop.Name); break;
                        case "int64": sql.AddParameter("@" + prop.Name, SqlDbType.BigInt, prop.GetValue(obj, null)); queryParameters.Append(",@" + prop.Name); break;
                        case "boolean": sql.AddParameter("@" + prop.Name, SqlDbType.Bit, prop.GetValue(obj, null)); queryParameters.Append(",@" + prop.Name); break;
                        case "double": sql.AddParameter("@" + prop.Name, SqlDbType.Float, prop.GetValue(obj, null)); queryParameters.Append(",@" + prop.Name); break;
                        case "byte": sql.AddParameter("@" + prop.Name, SqlDbType.SmallInt, prop.GetValue(obj, null)); queryParameters.Append(",@" + prop.Name); break;
                    }
                }
            }

            string query = String.Format("Insert Into [" + table + "] ({0}) Values ({1})", queryParameters.ToString().Replace("@", "").TrimStart(',').ToLower(), queryParameters.ToString().TrimStart(',').ToLower());

            SqlDataReader reader = sql.ExecuteSqlReader(query);

            int record = reader.RecordsAffected;

            reader.Close();

            return record > 0;
        }
        public bool InsertData(object obj)
        {
            return Convert.ToBoolean(obj.GetType().InvokeMember("InsertData", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, null));
        }
        public bool UpdateData(object obj,string table="")
        {
            if (table == String.Empty)
                table = this.GetTableName(obj);

            PropertyInfo[] propertys = obj.GetType().GetProperties();

            MSSqlService sql = new MSSqlService();
            StringBuilder queryParameters = new StringBuilder();

            foreach (PropertyInfo prop in propertys)
            {
                if (prop.CanWrite)
                {
                    string data_type = prop.PropertyType.Name.ToLower();

                    switch (data_type)
                    {
                        case "string":
                            sql.AddParameter("@" + prop.Name.ToLower(), SqlDbType.VarChar, prop.GetValue(obj, null));
                            queryParameters.Append("," + prop.Name.ToLower() + "=@" + prop.Name.ToLower()); break;
                        case "int32":
                            sql.AddParameter("@" + prop.Name.ToLower(), SqlDbType.Int, prop.GetValue(obj, null));
                            queryParameters.Append("," + prop.Name.ToLower() + "=@" + prop.Name.ToLower()); break;
                        case "decimal":
                            sql.AddParameter("@" + prop.Name.ToLower(), SqlDbType.Decimal, prop.GetValue(obj, null));
                            queryParameters.Append("," + prop.Name.ToLower() + "=@" + prop.Name.ToLower()); break;
                        case "datetime":
                            sql.AddParameter("@" + prop.Name.ToLower(), SqlDbType.DateTime, prop.GetValue(obj, null));
                            queryParameters.Append("," + prop.Name.ToLower() + "=@" + prop.Name.ToLower()); break;
                        case "int64":
                            sql.AddParameter("@" + prop.Name.ToLower(), SqlDbType.BigInt, prop.GetValue(obj, null));
                            queryParameters.Append("," + prop.Name.ToLower() + "=@" + prop.Name.ToLower()); break;
                        case "boolean":
                            sql.AddParameter("@" + prop.Name.ToLower(), SqlDbType.Bit, prop.GetValue(obj, null));
                            queryParameters.Append("," + prop.Name.ToLower() + "=@" + prop.Name.ToLower()); break;
                        case "double":
                            sql.AddParameter("@" + prop.Name.ToLower(), SqlDbType.Float, prop.GetValue(obj, null));
                            queryParameters.Append("," + prop.Name.ToLower() + "=@" + prop.Name.ToLower()); break;
                        case "byte":
                            sql.AddParameter("@" + prop.Name.ToLower(), SqlDbType.SmallInt, prop.GetValue(obj, null));
                            queryParameters.Append("," + prop.Name.ToLower() + "=@" + prop.Name.ToLower()); break;
                    }
                }
            }

            string query = String.Format("Update [" + table + "] Set {0} Where id = @id", queryParameters.ToString().TrimStart(','));

            SqlDataReader reader = sql.ExecuteSqlReader(query);

            int record = reader.RecordsAffected;

            reader.Close();

            return record > 0;
        }     
        public bool UpdateData(object obj)
        {
            return Convert.ToBoolean(obj.GetType().InvokeMember("UpdateData", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, null));
        }
        public void DeleteData(object obj)
        {
            obj.GetType().InvokeMember("DeleteData", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, null).ToString();
        }
        public string GetTableName(object obj)
        {
            return obj.GetType().InvokeMember("GetTableName", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, null).ToString();
        }

        public void InsertData(List<object> list)
        {
            MSSqlService sql = new MSSqlService();

            sql.BeginTransaction();
            sql.AutoCloseConnection = false;

            foreach(object obj in list)
            {
                obj.GetType().InvokeMember("InsertData", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, new object[] {sql }).ToString();
            }

            sql.CommitTransaction();
        }
        public void UpdateData(List<object> list)
        {
            MSSqlService sql = new MSSqlService();

            sql.BeginTransaction();
            sql.AutoCloseConnection = false;

            foreach (object obj in list)
            {
                obj.GetType().InvokeMember("UpdateData", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, new object[] { sql }).ToString();
            }

            sql.CommitTransaction();
        }
        public void DeleteData(List<object> list)
        {
            MSSqlService sql = new MSSqlService();

            sql.BeginTransaction();
            sql.AutoCloseConnection = false;

            foreach (object obj in list)
            {
                obj.GetType().InvokeMember("DeleteData", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, new object[] { sql }).ToString();
            }

            sql.CommitTransaction();
        }
    }
}
