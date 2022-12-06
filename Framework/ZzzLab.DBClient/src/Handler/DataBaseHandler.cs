using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using ZzzLab.Data.Configuration;
using ZzzLab.Data.Models;

namespace ZzzLab.Data
{
    public class DataBaseHandler : IDBHandler, IDisposable
    {
        public virtual bool IsDebug
        {
            set => this.Handler.IsDebug = value;
            get => this.Handler.IsDebug;
        }

        public ConnectionConfig Config { get; }

        public DataBaseType ServerType => this.Handler.ServerType;

        public string ConnectionString => this.Handler.ConnectionString;

        protected IDBHandler Handler;

        #region Construct

        protected DataBaseHandler(ConnectionConfig config)
        {
            this.Config = config ?? throw new ArgumentNullException(nameof(config));

            //if (string.IsNullOrWhiteSpace(this.Config.ConnectionString))
            //{
            //    this.Config.ConnectionString = DBClient.CreateConnectionString(config.ServerType, config.Host, config.Port.ToInt(), config.Database, config.UserId, config.Password);
            //}

            this.Handler = GetDBHandler(this.Config.ServerType, this.Config.ConnectionString);
        }

        public static IDBHandler Create(string name)
        {
            ConnectionConfig config = DBClient.Connector[name];

            if (config == null) throw new ArgumentOutOfRangeException(nameof(name));

            return new DataBaseHandler(config);
        }

        protected virtual IDBHandler GetDBHandler(DataBaseType server, string connectionString)
        {
            switch (this.Config.ServerType)
            {
                case DataBaseType.Oracle: return new OracleDBHandler(connectionString);
                case DataBaseType.MSSql: return new MSSqlDBHandler(connectionString);
                case DataBaseType.PostgreSQL: return new PostgreSQLDBHandler(connectionString);
                default: throw new NotSupportedException();
            }
        }

        #endregion Construct

        public IDbConnection CreateDBConnection()
            => this.Handler.CreateDBConnection();

        public IDbCommand CreateDBCommand()
            => this.Handler.CreateDBCommand();

        public void CrearDBConnection(IDbConnection connection)
            => this.Handler.CrearDBConnection(connection);

        public bool ConnectionTest()
            => this.Handler.ConnectionTest();

        public string GetVersion() 
            => this.Handler.GetVersion();

        public DataTable Select(string commandText)
            => this.Handler.Select(commandText);

        public DataTable Select(string commandText, QueryParameterCollection parameters)
            => this.Handler.Select(commandText, parameters);

        public DataTable Select(Query query)
            => this.Handler.Select(query);

        public DataRow SelectRow(string commandText)
            => this.Handler.SelectRow(commandText);

        public DataRow SelectRow(string commandText, QueryParameterCollection parameters)
            => this.Handler.SelectRow(commandText, parameters);

        public DataRow SelectRow(Query query)
            => this.Handler.SelectRow(query);

        public object SelectValue(string commandText)
            => this.Handler.SelectValue(commandText);

        public object SelectValue(string commandText, QueryParameterCollection parameters)
            => this.Handler.SelectValue(commandText, parameters);

        public object SelectValue(Query query)
            => this.Handler.SelectValue(query);

        public int Excute(params Query[] queries)
            => this.Handler.Excute(queries);

        public int Excute(string commandText)
            => this.Handler.Excute(commandText);

        public int Excute(string commandText, QueryParameterCollection parameters)
            => this.Handler.Excute(commandText, parameters);

        public int Excute(QueryCollection queries)
            => this.Handler.Excute(queries);

        public void Vacuum(IDictionary<string, string> options = null)
            => this.Handler.Vacuum(options);

        public string GetQuery(string section, string label)
            => this.Handler.GetQuery(section, label);

        public string GetQuery(string section, string label, Hashtable parameters)
            => this.Handler.GetQuery(section, label, parameters);

        public string GetQuery(string section, string label, string search)
            => this.Handler.GetQuery(section, label, search);

        public string GetQuery(string section, string label, string search, string order)
            => this.Handler.GetQuery(section, label, search, order);

        #region DB Schema

        /// <summary>
        /// 테이블의 정보를 가져온다.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TableInfo> GetTableList()
            => this.Handler.GetTableList();

        /// <summary>
        /// 지정된 테이블의 정보를 가져온다.
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="databaseName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public TableInfo GetTableInfo(string schemaName, string databaseName, string tableName)
            => this.Handler.GetTableInfo(schemaName, databaseName, tableName);

        /// <summary>
        /// 지정된 테이블의 컬럼정보를 가져온다.
        /// </summary>
        /// <param name="tableInfo">TableInfo</param>
        /// <returns></returns>
        public IEnumerable<TableColomn> GetTableColumns(TableInfo tableInfo)
            => this.Handler.GetTableColumns(tableInfo);

        /// <summary>
        /// 지정된 테이블의 컬럼정보를 가져온다.
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="databaseName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IEnumerable<TableColomn> GetTableColumns(string schemaName, string databaseName, string tableName)
            => GetTableColumns(GetTableInfo(schemaName, databaseName, tableName));

        #endregion DB Schema

        #region IDisposable

        public void Dispose()
        {
            this.Handler.Dispose();
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}