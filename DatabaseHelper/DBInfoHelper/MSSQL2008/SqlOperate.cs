using Jund.DatabaseHelper.DALHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBInfoHelper.MSSQL2008
{
    public class SqlOperate
    {
        string sp_addextendedproperty = "sp_addextendedproperty";
        string sp_databases = "sp_databases";
        string sp_datatype_info = "sp_datatype_info";
        string sp_delete_backuphistory = "sp_delete_backuphistory";
        string sp_depends = "sp_depends";
        string sp_dropextendedproperty = "sp_dropextendedproperty";
        string sp_help = "sp_help";
        string sp_monitor = "sp_monitor";
        string sp_procoption = "sp_procoption";
        string sp_updateextendedproperty = "sp_updateextendedproperty";
        string sp_bindefault = "sp_bindefault";

        public enum TableObject
        {
            column_privileges,
            columns,
            fkeys,
            pkeys,
            special_columns,
            table_privileges,
            tables,
            statistics
        }
        public enum StoredProcedureObject
        {
            stored_procedures,
            sproc_columns
        }
        public enum DBObjectExtendedProperty
        {
            Database,
            Table,
            View,
            Column
        }
        public DataTable ExecTableObjectInfo(TableObject type,string table_name,string table_owner)
        {
            MSSqlService sql = new MSSqlService();

            sql.AddParameter("@table_name", SqlDbType.VarChar, table_name);
            sql.AddParameter("@table_owner", SqlDbType.VarChar, table_owner);

            return sql.ExecuteSPDataSet("sp_"+type.ToString()).Tables[0];
        }
        public DataTable ExecSPObjectInfo(StoredProcedureObject type, string procedure_name, string procedure_owner)
        {
            MSSqlService sql = new MSSqlService();

            sql.AddParameter("@procedure_name", SqlDbType.VarChar, procedure_name);
            sql.AddParameter("@procedure_owner", SqlDbType.VarChar, procedure_owner);

            return sql.ExecuteSPDataSet("sp_" + type.ToString()).Tables[0];
        }

        public DataTable ExecDBInfo()
        {
            MSSqlService sql = new MSSqlService();

            return sql.ExecuteSPDataSet(sp_databases).Tables[0];
        }
        public DataTable ExecDataTypeInfo()
        {
            MSSqlService sql = new MSSqlService();

            return sql.ExecuteSPDataSet(sp_datatype_info).Tables[0];
        }
        public DataTable ExecObjectDependInfo(string obj_name)
        {
            MSSqlService sql = new MSSqlService();

            sql.AddParameter("@objname", SqlDbType.VarChar, obj_name);

            return sql.ExecuteSPDataSet(sp_depends).Tables[0];
        }
        public DataSet ExecObjectInfo(string obj_name="")
        {
            MSSqlService sql = new MSSqlService();

           if(obj_name!=String.Empty) sql.AddParameter("@objname", SqlDbType.VarChar, obj_name);

            return sql.ExecuteSPDataSet(sp_help);
        }
        public DataSet ExecMonitorDatabaseInfo()
        {
            MSSqlService sql = new MSSqlService();

            return sql.ExecuteSPDataSet(sp_monitor);
        }

        public bool AddExtendedProperty(DBObjectExtendedProperty type,string value,string object_name,string column_name="",string property_name= "MS_Description")
        {
            MSSqlService sql = new MSSqlService();
            sql.AddParameter("@name", SqlDbType.VarChar, property_name);
            sql.AddParameter("@value", SqlDbType.VarChar, value);
            sql.AddParameter("@level0type", SqlDbType.VarChar, "user");
            sql.AddParameter("@level0name", SqlDbType.VarChar, "dbo");
            sql.AddParameter("@level1type", SqlDbType.VarChar, type.ToString());
            sql.AddParameter("@level1name", SqlDbType.VarChar, object_name);

            if (type == DBObjectExtendedProperty.Column)
            {
                sql.AddParameter("@level2type", SqlDbType.VarChar, "column");
                sql.AddParameter("@level2name", SqlDbType.VarChar, column_name);
            }

            try
            {
                sql.ExecuteSP(sp_addextendedproperty);

                return true;
            }
            catch
            {
                return false;
            }
           
        }
        public bool DropExtendedProperty(DBObjectExtendedProperty type, string object_name, string column_name = "", string property_name = "MS_Description")
        {
            MSSqlService sql = new MSSqlService();
            sql.AddParameter("@name", SqlDbType.VarChar, property_name);
            sql.AddParameter("@level0type", SqlDbType.VarChar, "user");
            sql.AddParameter("@level0name", SqlDbType.VarChar, "dbo");
            sql.AddParameter("@level1type", SqlDbType.VarChar, type.ToString());
            sql.AddParameter("@level1name", SqlDbType.VarChar, object_name);

            if (type == DBObjectExtendedProperty.Column)
            {
                sql.AddParameter("@level2type", SqlDbType.VarChar, "column");
                sql.AddParameter("@level2name", SqlDbType.VarChar, column_name);
            }

            try
            {
                sql.ExecuteSP(sp_dropextendedproperty);

                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool UpdateExtendedProperty(DBObjectExtendedProperty type, string value, string object_name, string column_name = "", string property_name = "MS_Description")
        {
            MSSqlService sql = new MSSqlService();
            sql.AddParameter("@name", SqlDbType.VarChar, property_name);
            sql.AddParameter("@value", SqlDbType.VarChar, value);
            sql.AddParameter("@level0type", SqlDbType.VarChar, "user");
            sql.AddParameter("@level0name", SqlDbType.VarChar, "dbo");
            sql.AddParameter("@level1type", SqlDbType.VarChar, type.ToString());
            sql.AddParameter("@level1name", SqlDbType.VarChar, object_name);

            if (type == DBObjectExtendedProperty.Column)
            {
                sql.AddParameter("@level2type", SqlDbType.VarChar, "column");
                sql.AddParameter("@level2name", SqlDbType.VarChar, column_name);
            }

            try
            {
                sql.ExecuteSP(sp_updateextendedproperty);

                return true;
            }
            catch
            {
                return false;
            }

        }
        public void DeleteBackupHistory(DateTime date)
        {
            MSSqlService sql = new MSSqlService();

            sql.AddParameter("@oldest_date", SqlDbType.DateTime, date);

            sql.ExecuteSP(sp_delete_backuphistory);
        }
        public void SetStoredProcedureExec(string sp_name,bool exec)
        {
            MSSqlService sql = new MSSqlService();

            sql.AddParameter("@ProcName", SqlDbType.VarChar, sp_name);
            sql.AddParameter("@OptionName", SqlDbType.VarChar, "start_up");
            sql.AddParameter("@OptionValue", SqlDbType.VarChar, exec?"on":"off");

            sql.ExecuteSP(sp_procoption);
        }

        public void BindDefault(string db_name,string table_name,string column_name,string default_value)
        {
            MSSqlService sql = new MSSqlService();

            sql.AddParameter("@defname", SqlDbType.VarChar, default_value);
            sql.AddParameter("@objname", SqlDbType.VarChar, db_name+"."+table_name+"."+column_name);

            sql.ExecuteSP(sp_bindefault);
        }
    }
}
