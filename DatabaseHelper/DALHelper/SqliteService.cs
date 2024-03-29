//------------------------------------------------------------------------------
//
// Copyright (c) 2002-2008 CodeSmith Tools, LLC.  All rights reserved.
// 
// The terms of use for this software are contained in the file
// named sourcelicense.txt, which can be found in the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by the
// terms of this license.
// 
// You must not remove this notice, or any other, from this software.
//
//------------------------------------------------------------------------------

using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using System.Diagnostics;
using System.Configuration;
using System.Xml;
using System.Text;
using System.Globalization;
using System.Collections.Specialized;
using System.Data.SQLite;

namespace Jund.DatabaseHelper.DALHelper
{
	//[DebuggerStepThrough]
    public class SqliteService
    {
        #region Protected Member Variables
        protected string _connectionString = String.Empty;
        protected SQLiteParameterCollection _parameterCollection;
        protected ArrayList _parameters = new ArrayList();
        protected bool _isSingleRow = false;
        protected bool _convertEmptyValuesToDbNull = true;
        protected bool _convertMinValuesToDbNull = true;
        protected bool _convertMaxValuesToDbNull = false;
        protected bool _autoCloseConnection = true;
        protected SQLiteConnection _connection;
        protected SQLiteTransaction _transaction;
        protected int _commandTimeout = 30;
        #endregion Protected Member Variables

        #region Contructors
        public SqliteService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; 
            //@"Data Source=E:\BaiduYunDownload\FM\MySoccerDatabase\AjaniSoccerDB\bin\Debug\Data\fmgel.db3";
            //_connectionString =
            //ConfigurationManager.ConnectionStrings["MySoccer"].ConnectionString;
        }

        public SqliteService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqliteService(string server, string database, string user, string password)
        {
            this.ConnectionString = @"Data Source=" + database + ";User ID=" + user + ";Password=" + password + ";";
        }

        public SqliteService(SQLiteConnection connection)
        {
            this.Connection = connection;
            this.AutoCloseConnection = false;
        }
        #endregion Contructors

        #region Properties
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        public int CommandTimeout
        {
            get
            {
                return _commandTimeout;
            }
            set
            {
                _commandTimeout = value;
            }
        }

        public bool IsSingleRow
        {
            get
            {
                return _isSingleRow;
            }
            set
            {
                _isSingleRow = value;
            }
        }

        public bool AutoCloseConnection
        {
            get
            {
                return _autoCloseConnection;
            }
            set
            {
                _autoCloseConnection = value;
            }
        }

        public SQLiteConnection Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value;
                this.ConnectionString = _connection.ConnectionString;
            }
        }

        public SQLiteTransaction Transaction
        {
            get
            {
                return _transaction;
            }
            set
            {
                _transaction = value;
            }
        }

        public bool ConvertEmptyValuesToDbNull
        {
            get
            {
                return _convertEmptyValuesToDbNull;
            }
            set
            {
                _convertEmptyValuesToDbNull = value;
            }
        }

        public bool ConvertMinValuesToDbNull
        {
            get
            {
                return _convertMinValuesToDbNull;
            }
            set
            {
                _convertMinValuesToDbNull = value;
            }
        }

        public bool ConvertMaxValuesToDbNull
        {
            get
            {
                return _convertMaxValuesToDbNull;
            }
            set
            {
                _convertMaxValuesToDbNull = value;
            }
        }

        public SQLiteParameterCollection Parameters
        {
            get
            {
                return _parameterCollection;
            }
        }

        public int ReturnValue
        {
            get
            {
                if (_parameterCollection.Contains("@ReturnValue"))
                {
                    return (int)_parameterCollection["@ReturnValue"].Value;
                }
                else
                {
                    throw new Exception("You must call the AddReturnValueParameter method before executing your request.");
                }
            }
        }
        #endregion Properties

        #region Execute Methods
        public void ExecuteSql(string sql)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            this.Connect();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = sql;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();
        }

        public SQLiteDataReader ExecuteSqlReader(string sql)
        {
            SQLiteDataReader reader;
            SQLiteCommand cmd = new SQLiteCommand();
            this.Connect();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = sql;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.Text;
            this.CopyParameters(cmd);

            CommandBehavior behavior = CommandBehavior.Default;

            if (this.AutoCloseConnection) behavior = behavior | CommandBehavior.CloseConnection;
            if (_isSingleRow) behavior = behavior | CommandBehavior.SingleRow;

            reader = cmd.ExecuteReader(behavior);
            cmd.Dispose();

            return reader;
        }

        public DataSet ExecuteSqlDataSet(string sql)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            this.Connect();
            SQLiteDataAdapter da = new SQLiteDataAdapter();
            DataSet ds = new DataSet();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            this.CopyParameters(cmd);
            da.SelectCommand = cmd;

            da.Fill(ds);
            da.Dispose();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();

            return ds;
        }

        public DataSet ExecuteSqlDataSet(string sql, string tableName)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            this.Connect();
            SQLiteDataAdapter da = new SQLiteDataAdapter();
            DataSet ds = new DataSet();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            da.SelectCommand = cmd;

            da.Fill(ds, tableName);
            da.Dispose();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();

            return ds;
        }

        public void ExecuteSqlDataSet(ref DataSet dataSet, string sql, string tableName)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            this.Connect();
            SQLiteDataAdapter da = new SQLiteDataAdapter();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            da.SelectCommand = cmd;

            da.Fill(dataSet, tableName);
            da.Dispose();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();
        }

        public DataSet ExecuteSPDataSet(string procedureName)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            this.Connect();
            SQLiteDataAdapter da = new SQLiteDataAdapter();
            DataSet ds = new DataSet();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = procedureName;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            this.CopyParameters(cmd);

            da.SelectCommand = cmd;

            da.Fill(ds);

            _parameterCollection = cmd.Parameters;
            da.Dispose();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();

            return ds;
        }

        public DataSet ExecuteSPDataSet(string procedureName, string tableName)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            this.Connect();
            SQLiteDataAdapter da = new SQLiteDataAdapter();
            DataSet ds = new DataSet();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = procedureName;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            this.CopyParameters(cmd);

            da.SelectCommand = cmd;

            da.Fill(ds, tableName);

            _parameterCollection = cmd.Parameters;
            da.Dispose();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();

            return ds;
        }

        public void ExecuteSPDataSet(ref DataSet dataSet, string procedureName, string tableName)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            this.Connect();
            SQLiteDataAdapter da = new SQLiteDataAdapter();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = procedureName;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            this.CopyParameters(cmd);

            da.SelectCommand = cmd;

            da.Fill(dataSet, tableName);

            _parameterCollection = cmd.Parameters;
            da.Dispose();
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();
        }

        public void ExecuteSP(string procedureName)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            this.Connect();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = procedureName;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            this.CopyParameters(cmd);

            cmd.ExecuteNonQuery();

            _parameterCollection = cmd.Parameters;
            cmd.Dispose();

            if (this.AutoCloseConnection) this.Disconnect();
        }

        public SQLiteDataReader ExecuteSPReader(string procedureName)
        {
            SQLiteDataReader reader;
            SQLiteCommand cmd = new SQLiteCommand();
            this.Connect();

            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandText = procedureName;
            cmd.Connection = _connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            this.CopyParameters(cmd);

            CommandBehavior behavior = CommandBehavior.Default;

            if (this.AutoCloseConnection) behavior = behavior | CommandBehavior.CloseConnection;
            if (_isSingleRow) behavior = behavior | CommandBehavior.SingleRow;

            reader = cmd.ExecuteReader(behavior);

            _parameterCollection = cmd.Parameters;
            cmd.Dispose();

            return reader;
        }
        #endregion Execute Methods

        #region AddParameter
        public SQLiteParameter AddParameter(string name, DbType type, object value)
        {
            SQLiteParameter prm = new SQLiteParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.DbType = type;
            prm.Value = this.PrepareSqlValue(value);

            _parameters.Add(prm);

            return prm;
        }

        public SQLiteParameter AddParameter(string name, DbType type, object value, bool convertZeroToDBNull)
        {
            SQLiteParameter prm = new SQLiteParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.DbType = type;
            prm.Value = this.PrepareSqlValue(value, convertZeroToDBNull);

            _parameters.Add(prm);

            return prm;
        }

        //public SQLiteParameter AddParameter(string name, DbType type, object value, bool convertZeroToDBNull)
        //{
        //    SQLiteParameter prm = new SQLiteParameter();
        //    prm.Direction = ParameterDirection.Input;
        //    prm.ParameterName = name;
        //    prm.DbType = type;
        //    prm.Value = this.PrepareSqlValue(value, convertZeroToDBNull);

        //    _parameters.Add(prm);

        //    return prm;
        //}

        public SQLiteParameter AddParameter(string name, DbType type, object value, int size)
        {
            SQLiteParameter prm = new SQLiteParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.DbType = type;
            prm.Size = size;
            prm.Value = this.PrepareSqlValue(value);

            _parameters.Add(prm);

            return prm;
        }

        public SQLiteParameter AddParameter(string name, DbType type, object value, ParameterDirection direction)
        {
            SQLiteParameter prm = new SQLiteParameter();
            prm.Direction = direction;
            prm.ParameterName = name;
            prm.DbType = type;
            prm.Value = this.PrepareSqlValue(value);

            _parameters.Add(prm);

            return prm;
        }

        public SQLiteParameter AddParameter(string name, DbType type, object value, int size, ParameterDirection direction)
        {
            SQLiteParameter prm = new SQLiteParameter();
            prm.Direction = direction;
            prm.ParameterName = name;
            prm.DbType = type;
            prm.Size = size;
            prm.Value = this.PrepareSqlValue(value);

            _parameters.Add(prm);

            return prm;
        }

        public void AddParameter(SQLiteParameter parameter)
        {
            _parameters.Add(parameter);
        }
        #endregion AddParameter

        #region Specialized AddParameter Methods
        public SQLiteParameter AddOutputParameter(string name, DbType type)
        {
            SQLiteParameter prm = new SQLiteParameter();
            prm.Direction = ParameterDirection.Output;
            prm.ParameterName = name;
            prm.DbType = type;

            _parameters.Add(prm);

            return prm;
        }

        public SQLiteParameter AddOutputParameter(string name, DbType type, int size)
        {
            SQLiteParameter prm = new SQLiteParameter();
            prm.Direction = ParameterDirection.Output;
            prm.ParameterName = name;
            prm.DbType = type;
            prm.Size = size;

            _parameters.Add(prm);

            return prm;
        }

        public SQLiteParameter AddReturnValueParameter()
        {
            SQLiteParameter prm = new SQLiteParameter();
            prm.Direction = ParameterDirection.ReturnValue;
            prm.ParameterName = "@ReturnValue";
            prm.DbType = DbType.Int32;

            _parameters.Add(prm);

            return prm;
        }

        public SQLiteParameter AddStreamParameter(string name, Stream value)
        {
            return this.AddStreamParameter(name, value, DbType.Object);
        }

        public SQLiteParameter AddStreamParameter(string name, Stream value, DbType type)
        {
            SQLiteParameter prm = new SQLiteParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.DbType = type;

            value.Position = 0;
            byte[] data = new byte[value.Length];
            value.Read(data, 0, (int)value.Length);
            prm.Value = data;

            _parameters.Add(prm);

            return prm;
        }

        public SQLiteParameter AddTextParameter(string name, string value)
        {
            SQLiteParameter prm = new SQLiteParameter();
            prm.Direction = ParameterDirection.Input;
            prm.ParameterName = name;
            prm.DbType = DbType.String;
            prm.Value = this.PrepareSqlValue(value);

            _parameters.Add(prm);

            return prm;
        }
        #endregion Specialized AddParameter Methods

        #region Private Methods
        public void ClearParameters()
        {
            this._parameters.Clear();
        }

        public void RemoveParameters(SQLiteParameter para)
        {
            this._parameters.Remove(para);
        }

        public SQLiteParameter FindParameter(string name)
        {
            SQLiteParameter pm = null;

            foreach (SQLiteParameter para in _parameters)
            {
                if (para.ParameterName.Equals(name))
                {
                    pm = para;

                    break;
                }
            }

            return pm;
        }

        private void CopyParameters(SQLiteCommand command)
        {
            for (int i = 0; i < _parameters.Count; i++)
            {
                command.Parameters.Add(_parameters[i]);
            }
        }

        private object PrepareSqlValue(object value)
        {
            return this.PrepareSqlValue(value, false);
        }

        private object PrepareSqlValue(object value, bool convertZeroToDBNull)
        {
            if (value is String)
            {
                if (this.ConvertEmptyValuesToDbNull && (string)value == String.Empty)
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is Guid)
            {
                if (this.ConvertEmptyValuesToDbNull && (Guid)value == Guid.Empty)
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is DateTime)
            {
                if ((this.ConvertMinValuesToDbNull && (DateTime)value == DateTime.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (DateTime)value == DateTime.MaxValue))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is Int16)
            {
                if ((this.ConvertMinValuesToDbNull && (Int16)value == Int16.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (Int16)value == Int16.MaxValue)
                    || (convertZeroToDBNull && (Int16)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is Int32)
            {
                if ((this.ConvertMinValuesToDbNull && (Int32)value == Int32.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (Int32)value == Int32.MaxValue)
                    || (convertZeroToDBNull && (Int32)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is Int64)
            {
                if ((this.ConvertMinValuesToDbNull && (Int64)value == Int64.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (Int64)value == Int64.MaxValue)
                    || (convertZeroToDBNull && (Int64)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is Single)
            {
                if ((this.ConvertMinValuesToDbNull && (Single)value == Single.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (Single)value == Single.MaxValue)
                    || (convertZeroToDBNull && (Single)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is Double)
            {
                if ((this.ConvertMinValuesToDbNull && (Double)value == Double.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (Double)value == Double.MaxValue)
                    || (convertZeroToDBNull && (Double)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else if (value is Decimal)
            {
                if ((this.ConvertMinValuesToDbNull && (Decimal)value == Decimal.MinValue)
                    || (this.ConvertMaxValuesToDbNull && (Decimal)value == Decimal.MaxValue)
                    || (convertZeroToDBNull && (Decimal)value == 0))
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }
            }
            else
            {
                return value;
            }
        }

        private Hashtable ParseConfigString(string config)
        {
            Hashtable attributes = new Hashtable(10, new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), new CaseInsensitiveComparer(CultureInfo.InvariantCulture));
            string[] keyValuePairs = config.Split(';');
            for (int i = 0; i < keyValuePairs.Length; i++)
            {
                string[] keyValuePair = keyValuePairs[i].Split('=');
                if (keyValuePair.Length == 2)
                {
                    attributes.Add(keyValuePair[0].Trim(), keyValuePair[1].Trim());
                }
                else
                {
                    attributes.Add(keyValuePairs[i].Trim(), null);
                }
            }

            return attributes;
        }
        #endregion Private Methods

        #region Public Methods
        public void Connect()
        {
            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
            }
            else
            {
                if (_connectionString != String.Empty)
                {
                    StringCollection initKeys = new StringCollection();
                    initKeys.AddRange(new string[] { "ARITHABORT", "ANSI_NULLS", "ANSI_WARNINGS", "ARITHIGNORE", "ANSI_DEFAULTS", "ANSI_NULL_DFLT_OFF", "ANSI_NULL_DFLT_ON", "ANSI_PADDING", "ANSI_WARNINGS" });

                    StringBuilder initStatements = new StringBuilder();
                    StringBuilder connectionString = new StringBuilder();

                    Hashtable attribs = this.ParseConfigString(_connectionString);
                    foreach (string key in attribs.Keys)
                    {
                        if (initKeys.Contains(key.Trim().ToUpper()))
                        {
                            initStatements.AppendFormat("SET {0} {1};", key, attribs[key]);
                        }
                        else if (key.Trim().Length > 0)
                        {
                            connectionString.AppendFormat("{0}={1};", key, attribs[key]);
                        }
                    }

                    _connection = new SQLiteConnection(connectionString.ToString());
                    _connection.Open();

                    if (initStatements.Length > 0)
                    {
                        SQLiteCommand cmd = new SQLiteCommand();
                        cmd.CommandTimeout = this.CommandTimeout;
                        cmd.CommandText = initStatements.ToString();
                        cmd.Connection = _connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
                else
                {
                    throw new InvalidOperationException("You must set a connection object or specify a connection string before calling Connect.");
                }
            }
        }

        public void Disconnect()
        {
            if ((_connection != null) && (_connection.State != ConnectionState.Closed))
            {
                _connection.Close();
            }

            if (_connection != null) _connection.Dispose();
            if (_transaction != null) _transaction.Dispose();

            _transaction = null;
            _connection = null;
        }

        public void BeginTransaction()
        {
            if (_connection != null)
            {
                _transaction = _connection.BeginTransaction();
            }
            else
            {
                throw new InvalidOperationException("You must have a valid connection object before calling BeginTransaction.");
            }
        }

        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                try
                {
                    _transaction.Commit();
                }
                catch (Exception)
                {
                    // TODO: We need to handle this situation.  Maybe just write a log entry or something.
                    throw;
                }
            }
            else
            {
                throw new InvalidOperationException("You must call BeginTransaction before calling CommitTransaction.");
            }
        }

        public void RollbackTransaction()
        {

            if (_transaction != null)
            {
                try
                {
                    _transaction.Rollback();
                }
                catch (Exception)
                {
                    // TODO: We need to handle this situation.  Maybe just write a log entry or something.
                    throw;
                }
            }
            else
            {
                throw new InvalidOperationException("You must call BeginTransaction before calling RollbackTransaction.");
            }
        }

        public void Reset()
        {
            if (_parameters != null)
            {
                _parameters.Clear();
            }

            if (_parameterCollection != null)
            {
                _parameterCollection = null;
            }
        }
        #endregion
    }
}
