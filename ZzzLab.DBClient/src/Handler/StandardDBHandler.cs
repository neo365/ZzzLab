using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ZzzLab.Data.Models;

namespace ZzzLab.Data.Handler
{
    public class StandardDBHandler : DataBaseHandlerBase, IDBHandler, IDisposable
    {
        public StandardDBHandler(DataBaseType serverType, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString?.Trim()))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.ConnectionString = connectionString;
            this.ServerType = serverType;
        }

        public override IDbConnection CreateDBConnection()
        {
            switch (this.ServerType)
            {
                case DataBaseType.PostgreSQL: return new NpgsqlConnection(this.ConnectionString);
                case DataBaseType.MSSql: return new SqlConnection(this.ConnectionString);
                case DataBaseType.Oracle: return new OracleConnection(this.ConnectionString);
                default: throw new NotSupportedException();
            }
        }

        private void CrearConnection(IDbConnection conn)
        {
            if (conn == null) return;
            if (conn.State != ConnectionState.Closed) conn.Close();
        }

        public override object SelectValue(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            IDbConnection conn = null;
            try
            {
                using (conn = CreateDBConnection())
                {
                    conn.Open();
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcutSQL(query);
                        cmd.CommandType = query.CommandType;
                        cmd.CommandTimeout = query.CommandTimeout;
                        cmd.Parameters.Clear();
                        FormatValue(cmd, query);
                        // prepare the command, which is significantly faster
                        cmd.Prepare();

                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch { throw; }
            finally
            {
                CrearConnection(conn);
            }
        }

        #region Select

        public override DataTable Select(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            IDbConnection conn = null;
            try
            {
                DataTable result = new DataTable();

                using (conn = CreateDBConnection())
                {
                    conn.Open();

                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcutSQL(query);
                        cmd.CommandType = query.CommandType;
                        cmd.CommandTimeout = query.CommandTimeout;

                        FormatValue(cmd, query);

                        using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (((System.Data.Common.DbDataReader)reader).HasRows) result.Load(reader);

                            if (reader.IsClosed == false) reader.Close();
                        }
                    }

                    conn.Close();
                }

                return (result != null && result.Rows.Count > 0 ? result : null);
            }
            catch { throw; }
            finally
            {
                CrearConnection(conn);
            }
        }

        #endregion Select

        #region Excute

        public override int Excute(QueryCollection queries)
        {
            if (queries == null || queries.Any() == false) return 0;

            //Query currentQuery = null;

            IDbConnection conn = null;

            try
            {
                int resultcount = 0;
                using (conn = CreateDBConnection())
                {
                    conn.Open();
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandTimeout = queries.CommandTimeout;

                        if (queries.Count > 1)
                        {
                            if (cmd.Transaction == null) cmd.Transaction = cmd.Connection?.BeginTransaction();
                        }

                        try
                        {
                            foreach (Query query in queries)
                            {
                                //currentQuery = query;

                                cmd.CommandText = ConvertToExcutSQL(query);
                                cmd.CommandType = query.CommandType;
                                cmd.Parameters.Clear();
                                FormatValue(cmd, query);
                                cmd.Prepare();

                                resultcount += cmd.ExecuteNonQuery();
                            }

                            if (cmd.Transaction != null) cmd.Transaction.Commit(); // 트랜잭션commit
                        }
                        catch
                        {
                            if (cmd.Transaction != null) cmd.Transaction.Rollback(); // 에러발생시rollback
                            cmd.Cancel();
                            throw;
                        }
                    }
                }

                return resultcount;
            }
            catch { throw; }
            finally
            {
                CrearConnection(conn);
            }
        }

        #endregion Excute

        #region HELPER_FUNCTIONS

        private void FormatValue(IDbCommand cmd, Query query)
        {
            if (query == null) return;
            cmd.CommandText = ConvertToExcutSQL(query);

            cmd.Parameters.Clear();

            if (query.Parameters == null || query.Parameters.Any() == false) return;

            foreach (var p in query.Parameters)
            {
                if (cmd.Parameters.Contains(p.Name)) continue;

                IDbDataParameter dbParameter = cmd.CreateParameter();
                dbParameter.ParameterName = p.Name;
                dbParameter.Direction = ToParameterDirection(p.Direction);
                dbParameter.Value = (p.Value ?? DBNull.Value);

                cmd.Parameters.Add(dbParameter);
            }
        }

        #endregion HELPER_FUNCTIONS

        #region DB Schema

        public override IEnumerable<TableInfo> GetTableList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 지정된 테이블의 정보를 가져온다.
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="databaseName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override TableInfo GetTableInfo(string schemaName, string databaseName, string tableName)
            => throw new NotImplementedException();

        /// <summary>
        /// 지정된 테이블의 컬럼정보를 가져온다.
        /// </summary>
        /// <param name="tableInfo">TableInfo</param>
        /// <returns></returns>
        public override IEnumerable<TableColomn> GetTableColumns(TableInfo tableInfo)
            => throw new NotImplementedException();

        #endregion DB Schema
    }
}