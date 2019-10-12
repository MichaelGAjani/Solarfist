using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.LogHelper
{
    public static class DBFileLog
    {
        public enum LogType
        {
            Info,
            Warn,
            Debug,
            Error,
            Fatal
        }
        public static void Log(LogType type,string message,Exception ex)
        {
            Jund.DatabaseHelper.DALHelper.SqliteService sql = new DatabaseHelper.DALHelper.SqliteService();

            StringBuilder builder = new StringBuilder();
            builder.Append("@LogTime");
            builder.Append(",@LogType");
            builder.Append(",@FuncName");
            builder.Append(",@MethodType");
            builder.Append(",@ModuleName");
            builder.Append(",@Message");
            builder.Append(",@Exception");

            List <StackInfo> list=StackHelper.GetStackInfo();

            foreach (StackInfo info in list)
            {
                sql.ClearParameters();
                sql.AddParameter("@LogTime", System.Data.DbType.DateTime, DateTime.Now);
                sql.AddParameter("@LogType", System.Data.DbType.String, type);
                sql.AddParameter("@FuncName", System.Data.DbType.String, info.FullName);
                sql.AddParameter("@MethodType", System.Data.DbType.String, info.MethodType);
                sql.AddParameter("@ModuleName", System.Data.DbType.String, info.MoudleName);
                sql.AddParameter("@Message", System.Data.DbType.String, list.IndexOf(info)==0?message:String.Empty);
                sql.AddParameter("@Exception", System.Data.DbType.String, list.IndexOf(info) == 0  &&ex != null ?ex.Message:String.Empty);

                string query = String.Format("Insert Into t_log ({0}) Values ({1})", builder.ToString().Replace("@", "").TrimStart(',').ToLower(), builder.ToString().TrimStart(',').ToLower());
                sql.ExecuteSqlReader(query).Close();
            }
        }
    }
}
