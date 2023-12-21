using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;

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

        protected override IDbConnection CreateConnection()
        {
            return this.ServerType switch
            {
                DataBaseType.PostgreSQL => new NpgsqlConnection(this.ConnectionString),
                DataBaseType.MSSql => new SqlConnection(this.ConnectionString),
                DataBaseType.Oracle => new OracleConnection(this.ConnectionString),
                DataBaseType.MySql => new MySqlConnection(this.ConnectionString),
                DataBaseType.MariaDB => new MySqlConnection(this.ConnectionString),
                DataBaseType.SQLite => new SQLiteConnection(this.ConnectionString),
                _ => throw new NotSupportedException(),
            };
        }

        protected override void CrearConnection(IDbConnection conn)
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
                using (conn = CreateConnection())
                {
                    conn.Open();
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcuteSQL(query);
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

                using (conn = CreateConnection())
                {
                    conn.Open();

                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcuteSQL(query);
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

            Query query = null;
            IDbConnection conn = null;

            try
            {
                int resultcount = 0;
                using (conn = CreateConnection())
                {
                    conn.Open();
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandTimeout = queries.CommandTimeout;

                        if (queries.Count > 1)
                        {
                            cmd.Transaction ??= cmd.Connection?.BeginTransaction();
                        }

                        try
                        {
                            foreach (Query q in queries)
                            {
                                query = q;

                                cmd.CommandText = ConvertToExcuteSQL(q);
                                cmd.CommandType = q.CommandType;
                                cmd.Parameters.Clear();
                                FormatValue(cmd, q);
                                cmd.Prepare();

                                resultcount += cmd.ExecuteNonQuery();
                            }

                            cmd.Transaction?.Commit(); // 트랜잭션commit
                        }
                        catch
                        {
                            cmd.Transaction?.Rollback(); // 에러발생시rollback
                            cmd.Cancel();
                            throw;
                        }
                    }
                }

                return resultcount;
            }
            catch
            {
                if (query != null) Logger.Debug(query.ToString());
                throw;
            }
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
            cmd.CommandText = ConvertToExcuteSQL(query);

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
    }
}