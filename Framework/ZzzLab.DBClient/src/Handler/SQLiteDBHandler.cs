using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace ZzzLab.Data.Handler
{
    public sealed class SQLiteDBHandler : DataBaseHandlerBase, IDBHandler, IDisposable
    {
        #region Construct

        public SQLiteDBHandler(string connectionString, string aliasName = null)
        {
            if (string.IsNullOrEmpty(connectionString?.Trim()))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.ConnectionString = connectionString;
            this.ServerType = DataBaseType.SQLite;
            this.AliasName = aliasName;
        }

        #endregion Construct

        #region Connectionstring

        internal static string CreateConnectionString(string datasource, string password = null, bool isJournalMode = true, int timeout = 5)
        {
            if (string.IsNullOrEmpty(datasource)) throw new ArgumentNullException(nameof(datasource));

            SQLiteConnectionStringBuilder connectionBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = datasource
            };

            if (isJournalMode) connectionBuilder.JournalMode = SQLiteJournalModeEnum.Wal;
            if (string.IsNullOrWhiteSpace(password) == false) connectionBuilder.Password = password;

            connectionBuilder.FailIfMissing = false;
            connectionBuilder.Pooling = true;
            connectionBuilder.BusyTimeout = timeout * 1000;// 5sec

            return connectionBuilder.ToString();
        }

        #endregion Connectionstring

        #region Version

        public override string GetVersion()
            => SelectValue("select sqlite_version()")?.ToString();

        #endregion Version

        #region Connection

        protected override IDbConnection CreateConnection()
            => this.CreateDBConnection();

        protected override void CrearConnection(IDbConnection conn)
            => this.CrearDBConnection(conn as SQLiteConnection);

        private SQLiteConnection CreateDBConnection()
            => new SQLiteConnection(this.ConnectionString);

        private void CrearDBConnection(SQLiteConnection conn)
        {
            if (conn == null) return;
            if (conn.State != ConnectionState.Closed) conn.Close();
            SQLiteConnection.ClearPool(conn);
        }

        #endregion Connection

        public override object SelectValue(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            SQLiteConnection conn = null;
            try
            {
                using (conn = CreateDBConnection())
                {
                    conn.Open();
                    using (SQLiteCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcuteSQL(query);
                        cmd.CommandType = query.CommandType;
                        cmd.CommandTimeout = query.CommandTimeout;
                        cmd.Parameters.Clear();
                        cmd.Prepare();  // prepare the command, which is significantly faster
                        FormatValue(cmd, query);

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

            SQLiteConnection conn = null;
            try
            {
                DataTable result = new DataTable();

                using (conn = CreateDBConnection())
                {
                    conn.Open();

                    using (SQLiteCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcuteSQL(query);
                        cmd.CommandType = query.CommandType;
                        cmd.CommandTimeout = query.CommandTimeout;
                        cmd.Prepare();  // prepare the command, which is significantly faster

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

        public override int Excute(QueriesCollection queries)
        {
            if (queries == null || queries.Any() == false) return 0;

            Query query = null;
            SQLiteConnection conn = null;

            try
            {
                int resultcount = 0;
                using (conn = CreateDBConnection())
                {
                    conn.Open();
                    using (SQLiteCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandTimeout = queries.CommandTimeout;

                        if (queries.Count > 1)
                        {
                            cmd.Transaction = cmd.Transaction ?? cmd.Connection?.BeginTransaction();
                        }

                        cmd.Prepare();  // prepare the command, which is significantly faster

                        try
                        {
                            foreach (Query q in queries)
                            {
                                query = q;

                                cmd.CommandText = ConvertToExcuteSQL(q);
                                cmd.CommandType = q.CommandType;
                                cmd.Parameters.Clear();
                                FormatValue(cmd, q);

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

        public override string MakePagingQuery(string query, int pageNum, int pageSize)
        {
            if (pageNum <= 0) return query;

            return $"SELECT * FROM (SELECT * FROM ({query}) LIMIT (({pageNum} - 1) * {pageSize}), {pageSize};";
        }

        #region HELPER_FUNCTIONS

        private void FormatValue(SQLiteCommand cmd, Query query)
        {
            if (query == null) return;
            cmd.CommandText = ConvertToExcuteSQL(query);

            cmd.Parameters.Clear();

            if (query.Parameters == null || query.Parameters.Any() == false) return;

            foreach (var p in query.Parameters)
            {
                if (cmd.Parameters.Contains(p.Name)) continue;

                SQLiteParameter dbParameter = cmd.CreateParameter();
                dbParameter.ParameterName = p.Name;
                dbParameter.Direction = ToParameterDirection(p.Direction);
                dbParameter.Value = (p.Value ?? DBNull.Value);

                cmd.Parameters.Add(dbParameter);
            }
        }

        #endregion HELPER_FUNCTIONS
    }
}