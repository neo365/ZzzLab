using System;
using System.Collections;
using System.Collections.Generic;
using ZzzLab.Data.Configuration;

namespace ZzzLab.Data
{
    public static class DBClient
    {
        /// <summary>
        /// DB 연결정보
        /// </summary>
        public static ConnectionCollection Connector
        {
            get
            {
                try
                {
                    return (ConnectionCollection)Configurator.Setting.DBConnector;
                }
                catch (Exception ex)
                {
                    Logger.Fatal(ex);
                }

                return null;
            }
        }

        /// <summary>
        /// Query 목록
        /// </summary>
        public static SQLCollection Queries
            => (SQLCollection)Configurator.Setting.DBQueries;

        public static string CreateConnectionString(ConnectionConfig config)
            => CreateConnectionString(
                config.ServerType,
                config.Host,
                config.Port,
                config.Database,
                config.UserId,
                config.Password,
                config.Timeout);

        public static string CreateConnectionString(DataBaseType serverType, string host, int port, string database, string userid, string password, int timeout)
        {
            switch (serverType)
            {
                case DataBaseType.PostgreSQL: return PostgreSQLDBHandler.CreateConnectionString(host, port, database, userid, password, timeout);
                case DataBaseType.MSSql: return MSSqlDBHandler.CreateConnectionString(host, port, database, userid, password, timeout);
                case DataBaseType.Oracle: return OracleDBHandler.CreateConnectionString(host, port, database, userid, password, timeout);
                default: throw new NotSupportedException();
            }
        }

        public static string GetQuery(string section, string label)
            => Queries.Get(section, label);

        public static bool SetQuery(string section, string label, string command)
            => SetQuery(SqlEntity.Create(section, label, command));

        public static bool SetQuery(params SqlEntity[] collection)
            => SetQuery((IEnumerable<SqlEntity>)collection);

        public static bool SetQuery(IEnumerable<SqlEntity> collection)
             => DBClientBuilder.BaseReader.QueryWriter(collection);
    }
}