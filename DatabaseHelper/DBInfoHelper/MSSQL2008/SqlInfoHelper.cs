using Jund.DatabaseHelper.DBObject.MSSQL2008;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBInfoHelper.MSSQL2008
{
    public class SqlInfoHelper
    {
        string query_db = "SELECT * FROM Master..SysDatabases ORDER BY Name";
        string query_object = "SELECT * FROM SysObjects Where XType=@xtype ORDER BY Name";
        string query_table_column = "SELECT * FROM SysColumns WHERE id=@id";
        string query_column = "SELECT * FROM SysColumns ORDER BY id";
        string query_sp_parameter = "SELECT * FROM [sys].[parameters] WHERE object_id=@id";
        string query_table_fk = "SELECT * FROM [sys].[foreign_keys] WHERE parent_object_id=@id";
        string query_table_ck = "SELECT * FROM [sys].[check_constraints] WHERE parent_object_id=@id";
        string query_table_keys = "SELECT * FROM [sys].[key_constraints] WHERE parent_object_id=@id";
        string query_table_index = "SELECT * FROM [sys].[indexes] WHERE object_id=@id";
        string query_column_default = "SELECT * FROM [sys].[default_constraints] WHERE parent_object_id=@id";
        string query_object_extend = "SELECT * FROM [sys].[extended_properties] WHERE major_id=@id";

        public enum DBObject
        {
            S,
            U,
            V,
            C,
            F,
            P,
            UQ,
            D,
            PK
        }

        public List<DatabaseHelper.DBObject.MSSQL2008.Database> GetDatabaseInfo()
        {
            Jund.DatabaseHelper.DALHelper.MSSqlService sql = new DALHelper.MSSqlService();

            DataTable dt = sql.ExecuteSqlDataSet(query_db).Tables[0];

            return Jund.NETHelper.ConvertHelper.ListDataTableConvent<DatabaseHelper.DBObject.MSSQL2008.Database>.ConvertDataTableToList(dt);
        }

        public List<DBAllObjec> GetObjectInfo(DBObject type)
        {
            Jund.DatabaseHelper.DALHelper.MSSqlService sql = new DALHelper.MSSqlService();
            sql.AddParameter("@xtype", SqlDbType.VarChar, type.ToString());

            DataTable dt = sql.ExecuteSqlDataSet(query_object).Tables[0];

            return Jund.NETHelper.ConvertHelper.ListDataTableConvent<DBAllObjec>.ConvertDataTableToList(dt);
        }

        public List<TableColumn> GetColumnInfo()
        {
            Jund.DatabaseHelper.DALHelper.MSSqlService sql = new DALHelper.MSSqlService();

            DataTable dt = sql.ExecuteSqlDataSet(query_column).Tables[0];

            return Jund.NETHelper.ConvertHelper.ListDataTableConvent<TableColumn>.ConvertDataTableToList(dt);
        }

        public List<TableColumn> GetTableColumnInfo(int table_id)
        {
            Jund.DatabaseHelper.DALHelper.MSSqlService sql = new DALHelper.MSSqlService();
            sql.AddParameter("@id", SqlDbType.Int, table_id);

            DataTable dt = sql.ExecuteSqlDataSet(query_table_column).Tables[0];

            return Jund.NETHelper.ConvertHelper.ListDataTableConvent<TableColumn>.ConvertDataTableToList(dt);
        }

        public List<SPParameter> GetSPParameterInfo(int sp_id)
        {
            Jund.DatabaseHelper.DALHelper.MSSqlService sql = new DALHelper.MSSqlService();
            sql.AddParameter("@id", SqlDbType.Int, sp_id);

            DataTable dt = sql.ExecuteSqlDataSet(query_sp_parameter).Tables[0];

            return Jund.NETHelper.ConvertHelper.ListDataTableConvent<SPParameter>.ConvertDataTableToList(dt);
        }

        public List<TableFK> GetTableFKInfo(int table_id)
        {
            Jund.DatabaseHelper.DALHelper.MSSqlService sql = new DALHelper.MSSqlService();
            sql.AddParameter("@id", SqlDbType.Int, table_id);

            DataTable dt = sql.ExecuteSqlDataSet(query_table_fk).Tables[0];

            return Jund.NETHelper.ConvertHelper.ListDataTableConvent<TableFK>.ConvertDataTableToList(dt);
        }
        public List<TableCK> GetTableCKInfo(int table_id)
        {
            Jund.DatabaseHelper.DALHelper.MSSqlService sql = new DALHelper.MSSqlService();
            sql.AddParameter("@id", SqlDbType.Int, table_id);

            DataTable dt = sql.ExecuteSqlDataSet(query_table_ck).Tables[0];

            return Jund.NETHelper.ConvertHelper.ListDataTableConvent<TableCK>.ConvertDataTableToList(dt);
        }
        public List<TableKeys> GetTableKeysInfo(int table_id)
        {
            Jund.DatabaseHelper.DALHelper.MSSqlService sql = new DALHelper.MSSqlService();
            sql.AddParameter("@id", SqlDbType.Int, table_id);

            DataTable dt = sql.ExecuteSqlDataSet(query_table_keys).Tables[0];

            return Jund.NETHelper.ConvertHelper.ListDataTableConvent<TableKeys>.ConvertDataTableToList(dt);
        }
        public List<TableIndex> GetTableIndexInfo(int table_id)
        {
            Jund.DatabaseHelper.DALHelper.MSSqlService sql = new DALHelper.MSSqlService();
            sql.AddParameter("@id", SqlDbType.Int, table_id);

            DataTable dt = sql.ExecuteSqlDataSet(query_table_index).Tables[0];

            return Jund.NETHelper.ConvertHelper.ListDataTableConvent<TableIndex>.ConvertDataTableToList(dt);
        }
        public List<ColumnDefault> GetColumnDefaultInfo(int table_id)
        {
            Jund.DatabaseHelper.DALHelper.MSSqlService sql = new DALHelper.MSSqlService();
            sql.AddParameter("@id", SqlDbType.Int, table_id);

            DataTable dt = sql.ExecuteSqlDataSet(query_column_default).Tables[0];

            return Jund.NETHelper.ConvertHelper.ListDataTableConvent<ColumnDefault>.ConvertDataTableToList(dt);
        }
        public List<ExtendedProperties> GetExtendedPropertiesInfo(int table_id)
        {
            Jund.DatabaseHelper.DALHelper.MSSqlService sql = new DALHelper.MSSqlService();
            sql.AddParameter("@id", SqlDbType.Int, table_id);

            DataTable dt = sql.ExecuteSqlDataSet(query_object_extend).Tables[0];

            return Jund.NETHelper.ConvertHelper.ListDataTableConvent<ExtendedProperties>.ConvertDataTableToList(dt);
        }
    }
}
