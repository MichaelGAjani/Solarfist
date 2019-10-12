using Jund.DatabaseHelper.DALHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBInfoHelper.MSSQL2008
{
    public class SqlDDLHelper:ISqlDDL
    {
        string sql_drop_db = "DROP DATABASE ";
        string sql_drop_queue = "DROP QUEUE ExpenseQueue";
        string sql_drop_procedure = "DROP PROCEDURE IF EXISTS ";
        string sql_drop_table = "DROP TABLE IF EXISTS ";
        string sql_drop_view = "DROP VIEW IF EXISTS ";
        string sql_drop_column = "DROP VIEW IF EXISTS ";
        string sql_drop_constraint = "DROP CONSTRAINT IF EXISTS ";
        string sql_stop_queue = "ALTER QUEUE ExpenseQueue WITH STATUS = OFF";
        string sql_start_queue = "ALTER QUEUE ExpenseQueue WITH STATUS = ON";

        public class TableColumnInfo
        {
            string _name;
            DataType _data_type;
            bool _is_null;
            bool _is_primary_key;
            bool _is_unique;
            string _default_value;
            string _fk_table;
            string _fk_column;
            string _compute_string;
            /// <summary>
            /// 列名称
            /// </summary>
            public string Name { get => _name; set => _name = value; }
            /// <summary>
            /// 列数据类型
            /// </summary>
            public DataType Data_type { get => _data_type; set => _data_type = value; }
            public bool Is_null { get => _is_null; set => _is_null = value; }
            public bool Is_primary_key { get => _is_primary_key; set => _is_primary_key = value; }
            public bool Is_unique { get => _is_unique; set => _is_unique = value; }
            public string Default_value { get => _default_value; set => _default_value = value; }
            public string Fk_table { get => _fk_table; set => _fk_table = value; }
            public string Fk_column { get => _fk_column; set => _fk_column = value; }
            /// <summary>
            /// 计算列
            /// </summary>
            public string Compute_string { get => _compute_string; set => _compute_string = value; }
        }
        public enum DataType
        {
            Bigint,
            Int,
            Smallint,
            Tinyint,
            Bit,
            Decimal,
            Numeric,
            Float,
            Real,
            Money,
            Smallmoney,
            Date,
            Datetime,
            Datetime2,
            Char,
            Nchar,
            Varchar,
            Nvarchar,
            Text,
            Ntext,
            Binary,
            Varbinary,
            Image
        }

        /// <summary>
        /// 附加数据库
        /// </summary>
        /// <param name="db_name">数据库名称</param>
        /// <param name="db_file_path">数据库数据文件路径</param>
        public void AttachDatabase(string db_name, string db_file_path)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("USE master;");
            builder.AppendLine("GO");
            builder.AppendLine("CREATE DATABASE " + db_name);
            builder.AppendLine("ON(FILENAME = '" + db_file_path + "')");
            builder.AppendLine("FOR ATTACH;");
            builder.AppendLine("GO");

            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(builder.ToString());
        }
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="db_name">数据库名称</param>
        public void CreateDatabase(string db_name)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("USE master;\r\n");
            builder.Append("GO\r\n");
            builder.Append("IF DB_ID(N'mytest') IS NOT NULL\r\n");
            builder.Append("DROP DATABASE " + db_name + ";\r\n");
            builder.Append("GO\r\n");
            builder.Append("CREATE DATABASE " + db_name + ";\r\n");
            builder.Append("GOr\n");

            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(builder.ToString());
        }
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="db_name">数据库名称</param>
        /// <param name="path">文件目录</param>
        /// <param name="size">初始空间</param>
        /// <param name="max_size">最大空间</param>
        /// <param name="growth">增长空间</param>
        public void CreateDatabase(string db_name, string path, int size = 64, int max_size = 1, int growth = 10)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("USE master;");
            builder.AppendLine("GO");
            builder.AppendLine("CREATE DATABASE " + db_name);
            builder.AppendLine("ON");
            builder.AppendLine(" (NAME = " + db_name + "_dat,");
            builder.AppendLine("FILENAME ='" + path + @"\" + db_name + "_dat.mdf,");
            builder.AppendLine("SIZE=" + size.ToString() + "MB,");
            builder.AppendLine("MAXSIZE=" + max_size.ToString() + "MB,");
            builder.AppendLine("FILEGROWTH = " + growth.ToString() + "%)");
            builder.AppendLine("LOG ON");
            builder.AppendLine(" (NAME = " + db_name + "_log,");
            builder.AppendLine("FILENAME ='" + path + @"\" + db_name + "_dat.ldf,");
            builder.AppendLine("SIZE=" + size.ToString() + "MB,");
            builder.AppendLine("MAXSIZE=" + max_size.ToString() + "MB,");
            builder.AppendLine("FILEGROWTH = " + growth.ToString() + "%);");
            builder.AppendLine("GO");

            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(builder.ToString());
        }
        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="db_list">数据库列表</param>
        public void DropDatabase(List<string> db_list)
        {
            StringBuilder builder = new StringBuilder();

            foreach (string db in db_list)
            {
                builder.Append(db + ",");
            }

            string drop_obj = builder.ToString().TrimEnd(',');

            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(sql_drop_db + drop_obj);
        }
        /// <summary>
        /// 创建队列
        /// </summary>
        /// <param name="procedure_name">存储过程名称</param>
        /// <param name="max_queue">队列数量上限</param>
        /// <param name="sql_user">数据库用户名</param>
        public void CreateQueue(string procedure_name, int max_queue, string sql_user)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("CREATE QUEUE ExpenseQueue");
            builder.AppendLine("WITH STATUS = ON,  ");
            builder.AppendLine("ACTIVATION(");
            builder.AppendLine("PROCEDURE_NAME = " + procedure_name + ",");
            builder.AppendLine("MAX_QUEUE_READERS = " + max_queue.ToString() + ",");
            builder.AppendLine("EXECUTE AS '" + sql_user + "');");

            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(builder.ToString());
        }
        /// <summary>
        /// 删除队列
        /// </summary>
        public void DropQueue()
        {
            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(sql_drop_queue);
        }
        /// <summary>
        /// 停止队列
        /// </summary>
        public void StopQueue()
        {
            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(sql_stop_queue);
        }
        /// <summary>
        /// 修改队列
        /// </summary>
        /// <param name="procedure_name">存储过程名称</param>
        /// <param name="max_queue">最大队列</param>
        public void AlterQueue(string procedure_name, int max_queue)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("ALTER QUEUE ExpenseQueue");
            builder.AppendLine("WITH ACTIVATION(");
            builder.AppendLine("PROCEDURE_NAME = " + procedure_name + ",MAX_QUEUE_READERS=" + max_queue.ToString() + ",EXECUTE AS SELF)");

            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(builder.ToString());
        }
        /// <summary>
        /// 开始队列
        /// </summary>
        public void StartQueue()
        {
            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(sql_start_queue);
        }
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="table_name">表名</param>
        /// <param name="list">列集合</param>
        public void CreateTable(string table_name, List<TableColumnInfo> list)
        {
            List<string> pk_list = new List<string>();

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("CREATE TABLE " + table_name);
            builder.AppendLine("(");
            foreach (TableColumnInfo column in list)
            {
                if (column.Compute_string != String.Empty)
                    builder.AppendLine(column.Name + " AS " + column.Compute_string + ",");
                else
                {
                    builder.AppendLine(CreateColumnString(column));

                    if (column.Is_unique)
                        builder.AppendLine("UNIQUE NONCLUSTERED");
                    if (column.Fk_table != String.Empty)
                        builder.AppendLine(CreateFKString(column));
                    if (column.Default_value != String.Empty)
                        builder.AppendLine(CreateDefaultValueString(table_name, column));
                    if (column.Is_primary_key)
                        pk_list.Add(column.Name);
                    builder.Append(",");
                }
            }
            builder.AppendLine(CreatePKString(table_name, pk_list));
            builder.AppendLine(")");
            builder.AppendLine("ON [PRIMARY];");

            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(builder.ToString());
        }
        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="table_list">表集合</param>
        public void DropTable(List<string> table_list)
        {
            StringBuilder builder = new StringBuilder();

            foreach (string table in table_list)
            {
                builder.Append(table + ",");
            }

            string drop_obj = builder.ToString().TrimEnd(',');

            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(sql_drop_table + drop_obj);
        }
        public void AddColumnToTable(string table_name, List<TableColumnInfo> list)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("ALTER TABLE " + table_name);
            builder.AppendLine("ADD");
            foreach (TableColumnInfo column in list)
            {
                if (column.Compute_string != String.Empty)
                    builder.AppendLine(column.Name + " AS " + column.Compute_string + ",");
                else
                {
                    builder.AppendLine(CreateColumnString(column));

                    if (column.Is_unique)
                        builder.AppendLine("UNIQUE NONCLUSTERED");
                    if (column.Fk_table != String.Empty)
                        builder.AppendLine(CreateFKString(column));
                    if (column.Default_value != String.Empty)
                        builder.AppendLine(CreateDefaultValueString(table_name, column));

                    builder.Append(",");
                }
            }

            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(builder.ToString().TrimEnd(','));
        }
        public void AlterColumnToTable(string table_name, List<TableColumnInfo> list)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("ALTER TABLE " + table_name);
            builder.AppendLine("ALTER");
            foreach (TableColumnInfo column in list)
            {
                if (column.Compute_string != String.Empty)
                    builder.AppendLine(column.Name + " AS " + column.Compute_string + ",");
                else
                {
                    builder.AppendLine(CreateColumnString(column));

                    if (column.Is_unique)
                        builder.AppendLine("UNIQUE NONCLUSTERED");
                    if (column.Fk_table != String.Empty)
                        builder.AppendLine(CreateFKString(column));
                    if (column.Default_value != String.Empty)
                        builder.AppendLine(CreateDefaultValueString(table_name, column));

                    builder.Append(",");
                }
            }

            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(builder.ToString().TrimEnd(','));
        }
        public void DropColumn(string table_name, List<string> column_list)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("ALTER TABLE " + table_name);
            builder.AppendLine(sql_drop_column);

            foreach (string column in column_list)
            {
                builder.Append(column + ",");
            }

            string drop_obj = builder.ToString().TrimEnd(',');

            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(drop_obj);

        }
        public void DropConstraint(string table_name, List<string> constraint_list)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("ALTER TABLE " + table_name);
            builder.AppendLine(sql_drop_constraint);

            foreach (string constraint in constraint_list)
            {
                builder.Append(constraint + ",");
            }

            string drop_obj = builder.ToString().TrimEnd(',');

            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(drop_obj);

        }
        private string CreateColumnString(TableColumnInfo column)
        {
            return column.Name + " " + column.Data_type.ToString().ToLower() + " " + (column.Is_null ? "NULL" : "NOT NULL");
        }
        private string CreateFKString(TableColumnInfo column)
        {
            return "REFERENCES " + column.Fk_table + "(" + column.Fk_column + ")";
        }
        private string CreateDefaultValueString(string table_name, TableColumnInfo column)
        {
            return "CONSTRAINT DF_" + table_name + "_" + column.Name + " DEFAULT(" + column.Default_value + ")";
        }
        private string CreatePKString(string table_name, List<string> pk_column_list)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string column in pk_column_list)
                builder.Append(column + "_");

            string pk_name = builder.ToString().TrimEnd('_');
            return "CONSTRAINT PK_" + table_name + "_" + pk_name + "\r\n" +
               "PRIMARY KEY CLUSTERED(" + pk_name.Replace("_", ",") + ")\r\n" +
               "WITH(IGNORE_DUP_KEY = OFF)";
        }
        public void DropView(List<string> view_list)
        {
            StringBuilder builder = new StringBuilder();

            foreach (string view in view_list)
            {
                builder.Append(view + ",");
            }

            string drop_obj = builder.ToString().TrimEnd(',');

            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(sql_drop_view + drop_obj);
        }
        public void DropProceduce(List<string> proc_list)
        {
            StringBuilder builder = new StringBuilder();

            foreach (string proc in proc_list)
            {
                builder.Append(proc + ",");
            }

            string drop_obj = builder.ToString().TrimEnd(',');

            MSSqlService sql = new MSSqlService();

            sql.ExecuteSql(sql_drop_procedure + drop_obj);
        }
    }
}
