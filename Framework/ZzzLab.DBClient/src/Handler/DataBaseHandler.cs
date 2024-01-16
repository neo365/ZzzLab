using System;
using System.Collections.Generic;
using System.Data;
using ZzzLab.Data.Configuration;

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
            this.Handler = GetDBHandler(this.Config.ServerType, this.Config.ConnectionString);
        }

        public static IDBHandler Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            ConnectionConfig config = DBClient.Connector[name] ?? throw new ArgumentOutOfRangeException(nameof(name));
            return new DataBaseHandler(config);
        }

        protected virtual IDBHandler GetDBHandler(DataBaseType server, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            return this.Config.ServerType switch
            {
                DataBaseType.Oracle => new OracleDBHandler(connectionString),
                DataBaseType.MSSql => new MSSqlDBHandler(connectionString),
                DataBaseType.PostgreSQL => new PostgreSQLDBHandler(connectionString),
                _ => throw new NotSupportedException(),
            };
        }

        #endregion Construct

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

        public bool BulkCopy(DataTable table)
            => this.Handler.BulkCopy(table);

        public bool BulkCopyFromFile(string tableName, string filePath, int offset = 0)
            => this.Handler.BulkCopyFromFile(tableName, filePath, offset);

        public void Vacuum(IDictionary<string, string> options = null)
            => this.Handler.Vacuum(options);

        public string GetQuery(string section, string label)
            => this.Handler.GetQuery(section, label);

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