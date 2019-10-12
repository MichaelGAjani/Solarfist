using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jund.DatabaseHelper.DALHelper
{
    public class SqlService
    {
        object sqlQuery = new object();
        public ObjectEnum.DatabaseType databaseType = ObjectEnum.DatabaseType.SQLSERVER2008R2;

        #region Contructors
        public SqlService()
        {
            switch (databaseType)
            {
                case ObjectEnum.DatabaseType.MYSQL: sqlQuery = new MySqlService(); break;
                case ObjectEnum.DatabaseType.ORACLE9i: sqlQuery = new OracleService(); break;
                case ObjectEnum.DatabaseType.SQLITE: sqlQuery = new SqliteService(); break;
                case ObjectEnum.DatabaseType.SQLSERVER2008R2: sqlQuery = new MSSqlService(); break;
            }
        }
        public SqlService(ObjectEnum.DatabaseType databaseType)
        {
            switch(databaseType)
            {
                case ObjectEnum.DatabaseType.MYSQL:sqlQuery = new MySqlService();break;
                case ObjectEnum.DatabaseType.ORACLE9i:sqlQuery = new OracleService();break;
                case ObjectEnum.DatabaseType.SQLITE:sqlQuery = new SqliteService();break;
                case ObjectEnum.DatabaseType.SQLSERVER2008R2:sqlQuery = new MSSqlService();break;
            }
        }
        public SqlService(string connectionString)
        {
            switch (databaseType)
            {
                case ObjectEnum.DatabaseType.MYSQL: sqlQuery = new MySqlService(connectionString); break;
                case ObjectEnum.DatabaseType.ORACLE9i: sqlQuery = new OracleService(connectionString); break;
                case ObjectEnum.DatabaseType.SQLITE: sqlQuery = new SqliteService(connectionString); break;
                case ObjectEnum.DatabaseType.SQLSERVER2008R2: sqlQuery = new MSSqlService(connectionString); break;
            }
        }
        public SqlService(string server, string database, string user, string password)
        {
            switch (databaseType)
            {
                case ObjectEnum.DatabaseType.MYSQL: sqlQuery = new MySqlService(server,database,user,password); break;
                case ObjectEnum.DatabaseType.ORACLE9i: sqlQuery = new OracleService(server,database,user,password); break;
                case ObjectEnum.DatabaseType.SQLITE: sqlQuery = new SqliteService(server, database, user, password); break;
                case ObjectEnum.DatabaseType.SQLSERVER2008R2: sqlQuery = new MSSqlService(server, database, user, password); break;
            }            
        }
        public SqlService(string server, string database)
        {
                switch (databaseType)
                {
                    //case ObjectEnum.DatabaseType.MYSQL: sqlQuery = new MySqlService(connectionString); break;
                    //case ObjectEnum.DatabaseType.ORACLE9i: sqlQuery = new OracleService(connectionString); break;
                    //case ObjectEnum.DatabaseType.SQLITE: sqlQuery = new SqliteService(connectionString); break;
                    case ObjectEnum.DatabaseType.SQLSERVER2008R2: sqlQuery = new MSSqlService(server,database); break;
                }
        }
        public SqlService(object connection)
        {
            switch (databaseType)
            {
                case ObjectEnum.DatabaseType.MYSQL: sqlQuery = new MySqlService(connection as MySqlConnection); break;
                case ObjectEnum.DatabaseType.ORACLE9i: sqlQuery = new OracleService(connection as OracleConnection); break;
                case ObjectEnum.DatabaseType.SQLITE: sqlQuery = new SqliteService(connection as SQLiteConnection); break;
                case ObjectEnum.DatabaseType.SQLSERVER2008R2: sqlQuery = new MSSqlService(connection as SqlConnection); break;
            }
        }
#endregion Contructors

        #region Execute Methods
        public void ExecuteSql(string sql) => sqlQuery.GetType().InvokeMember("ExecuteSql", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, new object[] { sql });
        public object ExecuteSqlReader(string sql) => sqlQuery.GetType().InvokeMember("ExecuteSqlReader", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, new object[] { sql });
        public XmlReader ExecuteSqlXmlReader(string sql) => sqlQuery.GetType().InvokeMember("ExecuteSqlXmlReader", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, new object[] { sql }) as XmlReader;    
        public DataSet ExecuteSqlDataSet(string sql) => sqlQuery.GetType().InvokeMember("ExecuteSqlDataSet", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, new object[] { sql }) as DataSet;         
        public DataSet ExecuteSqlDataSet(string sql, string tableName) => sqlQuery.GetType().InvokeMember("ExecuteSqlDataSet", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, new object[] { sql ,tableName}) as DataSet;        
        public void ExecuteSqlDataSet(ref DataSet dataSet, string sql, string tableName) => sqlQuery.GetType().InvokeMember("ExecuteSqlDataSet", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, new object[] { dataSet,sql,tableName });       
        public DataSet ExecuteSPDataSet(string procedureName) => sqlQuery.GetType().InvokeMember("ExecuteSPDataSet", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, new object[] { procedureName }) as DataSet;       
        public DataSet ExecuteSPDataSet(string procedureName, string tableName) => sqlQuery.GetType().InvokeMember("ExecuteSPDataSet", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, new object[] { procedureName,tableName }) as DataSet;       
        public void ExecuteSPDataSet(ref DataSet dataSet, string procedureName, string tableName) => sqlQuery.GetType().InvokeMember("ExecuteSPDataSet", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, new object[] { dataSet,procedureName,tableName });         
        public void ExecuteSP(string procedureName) => sqlQuery.GetType().InvokeMember("ExecuteSP", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, new object[] { procedureName });         
        public object ExecuteSPReader(string procedureName) => sqlQuery.GetType().InvokeMember("ExecuteSPReader", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, new object[] { procedureName });        
        public XmlReader ExecuteSPXmlReader(string procedureName) => sqlQuery.GetType().InvokeMember("ExecuteSPXmlReader", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, new object[] { procedureName }) as XmlReader;

        #endregion Execute Methods

        #region Public Methods
        public void Connect() => sqlQuery.GetType().InvokeMember("Connect", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, null);    
        public void Disconnect() => sqlQuery.GetType().InvokeMember("Disconnect", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, null); 
        public void BeginTransaction() => sqlQuery.GetType().InvokeMember("BeginTransaction", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, null);  
        public void CommitTransaction() => sqlQuery.GetType().InvokeMember("CommitTransaction", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, null); 
        public void RollbackTransaction() => sqlQuery.GetType().InvokeMember("RollbackTransaction", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, null);
        public void Reset() => sqlQuery.GetType().InvokeMember("Reset", BindingFlags.Default | BindingFlags.InvokeMethod, null, sqlQuery, null);
        
        #endregion
    }
}
