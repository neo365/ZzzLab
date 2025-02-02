using System;
using System.Collections.Generic;
using System.Data;
using ZzzLab.Data.Configuration;
using ZzzLab.Data.Handler;

namespace ZzzLab.Data
{
    public class DataBaseHandler : IDBHandler, IDisposable
    {
        public virtual bool IsDebug
        {
            set => this.Handler.IsDebug = value;
            get => this.Handler.IsDebug;
        }

        public virtual ConnectionConfig Config { get; }

        public virtual DataBaseType ServerType => this.Handler.ServerType;

        public virtual string ConnectionString => this.Handler.ConnectionString;

        public virtual string Group { protected set; get; }
        public virtual string Name { protected set; get; }
        public virtual string AliasName { protected set; get; }

        protected IDBHandler Handler;

        #region Construct

        protected DataBaseHandler(ConnectionConfig config)
        {
            this.Config = config ?? throw new ArgumentNullException(nameof(config));
            this.Handler = GetDBHandler(this.Config.ServerType, this.Config.ConnectionString);

            switch (config.ServerType)
            {
                case DataBaseType.Oracle:
                    this.Handler = new OracleDBHandler(Config.ConnectionString);
                    break;

                case DataBaseType.MSSql:
                    this.Handler = new MSSqlDBHandler(Config.ConnectionString);
                    break;

                case DataBaseType.PostgreSQL:
                    this.Handler = new PostgreSQLDBHandler(Config.ConnectionString);
                    break;

                case DataBaseType.MariaDB:
                case DataBaseType.MySql:
                    this.Handler = new MySqlDBHandler(Config.ConnectionString);
                    break;

                case DataBaseType.SQLite:
                    this.Handler = new SQLiteDBHandler(Config.ConnectionString);
                    break;

                default:
                    throw new NotSupportedException();
            };

            this.Group = config.Group;
            this.Name = config.Name;
            this.AliasName = config.AliasName;
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

            switch (server)
            {
                case DataBaseType.Oracle: return new OracleDBHandler(connectionString);
                case DataBaseType.MSSql: return new MSSqlDBHandler(connectionString);
                case DataBaseType.PostgreSQL: return new PostgreSQLDBHandler(connectionString);
                case DataBaseType.MariaDB: return new MySqlDBHandler(connectionString);
                case DataBaseType.MySql: return new MySqlDBHandler(connectionString);
                case DataBaseType.SQLite:  return new SQLiteDBHandler(connectionString);
            };

            throw new NotSupportedException();
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

        public int Excute(QueriesCollection queries)
            => this.Handler.Excute(queries);

        public bool BulkCopy(DataTable table)
            => this.Handler.BulkCopy(table);

        public bool BulkCopyFromFile(string tableName, string filePath, int offset = 0)
            => this.Handler.BulkCopyFromFile(tableName, filePath, offset);

        public void Vacuum(IDictionary<string, string> options = null)
            => this.Handler.Vacuum(options);

        public string GetQuery(string section, string label)
            => this.Handler.GetQuery(section, label);

        public string GetQuery(string section, string label, QueryParameterCollection parameters)
            => this.Handler.GetQuery(section, label, parameters);

        public string MakePagingQuery(string query, int pageNum, int pageSize)
            => this.Handler.MakePagingQuery(query, pageNum, pageSize);

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