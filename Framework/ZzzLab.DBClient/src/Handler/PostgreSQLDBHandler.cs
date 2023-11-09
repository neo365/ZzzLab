using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ZzzLab.Data
{
    public sealed class PostgreSQLDBHandler : DataBaseHandlerBase, IDBHandler, IDisposable
    {
        #region Construct

        public PostgreSQLDBHandler(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString?.Trim()))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.ConnectionString = connectionString;
            this.ServerType = DataBaseType.PostgreSQL;
        }

        #endregion Construct

        #region Connectionstring

        internal static string CreateConnectionString(string host, int port, string database, string userid, string password, int timeout = 15)
            => $"Host={host};Port={port};Database={database};User ID={userid};Password={password};Timeout={timeout}";

        #endregion Connectionstring

        #region Connection

        protected override IDbConnection CreateConnection()
            => this.CreateConnection();

        protected override void CrearConnection(IDbConnection conn)
            => this.CrearDBConnection(conn as NpgsqlConnection);

        private NpgsqlConnection CreateDBConnection()
            => new NpgsqlConnection(this.ConnectionString);

        private void CrearDBConnection(NpgsqlConnection conn)
        {
            if (conn == null) return;
            if (conn.State != ConnectionState.Closed) conn.Close();
            NpgsqlConnection.ClearPool(conn);
        }

        #endregion Connection

        #region Version

        public override string GetVersion()
            => SelectValue("SELECT VERSION()")?.ToString();

        #endregion Version

        #region Select

        public override object SelectValue(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            NpgsqlConnection conn = null;
            try
            {
                using (conn = CreateDBConnection())
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcuteSQL(query);
                        cmd.CommandType = query.CommandType;
                        cmd.CommandTimeout = query.CommandTimeout;
                        cmd.Parameters.Clear();
                        MappingQuery(cmd, query);
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

        public override DataTable Select(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            NpgsqlConnection conn = null;
            try
            {
                DataTable result = new DataTable();

                using (conn = CreateDBConnection())
                {
                    conn.Open();

                    using (NpgsqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcuteSQL(query);
                        cmd.CommandType = query.CommandType;
                        cmd.CommandTimeout = query.CommandTimeout;
                        cmd.Parameters.Clear();
                        MappingQuery(cmd, query);
                        cmd.Prepare();

                        using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows) result.Load(reader);
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

            NpgsqlConnection conn = null;

            try
            {
                int resultcount = 0;
                using (conn = CreateDBConnection())
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandTimeout = queries.CommandTimeout;
                        cmd.Prepare();

                        if (queries.Count > 1 && cmd.Transaction == null) cmd.Transaction = cmd.Connection.BeginTransaction();

                        try
                        {
                            foreach (Query q in queries)
                            {
                                query = q;

                                cmd.CommandText = ConvertToExcuteSQL(q);
                                cmd.CommandType = q.CommandType;
                                cmd.Parameters.Clear();
                                MappingQuery(cmd, q);
                                cmd.ExecuteNonQuery();
                                resultcount++;
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
            catch { 
            if(query != null) Logger.Debug(query?.ToString());    
                throw;
            }
            finally
            {
                CrearConnection(conn);
            }
        }

        #endregion Excute

        #region Vacuum

        public override void Vacuum(IDictionary<string, string> options = null)
        {
            NpgsqlConnection conn = null;

            try
            {
                string sql = "vacuum";
                if (options != null)
                {
                    if (options.ContainsKey("FULL"))
                    {
                        sql = "vacuum full " + (options.ContainsKey("TABLENAME") ? options["TABLENAME"] : "analyze");
                    }
                    else if (options.ContainsKey("VERBOSE"))
                    {
                        sql = "vacuum verbose " + (options.ContainsKey("TABLENAME") ? options["TABLENAME"] : "analyze");
                    }
                }

                using (conn = new NpgsqlConnection(this.ConnectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                CrearConnection(conn);
            }
        }

        #endregion Vacuum

        #region BulkCopy

        public override bool BulkCopy(DataTable table)
        {
            if (table == null || table.Rows.Count == 0) return true;

            string sql = $"COPY {table.TableName} ({string.Join(",", table.Columns.GetNames())}) FROM STDIN (FORMAT BINARY)";
            NpgsqlConnection conn = null;
            try
            {
                using (conn = CreateDBConnection())
                {
                    using (NpgsqlBinaryImporter writer = conn.BeginBinaryImport(sql))
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            writer.StartRow();

                            List<object> list = new List<object>();
                            foreach (DataColumn c in table.Columns)
                            {
                                list.Add(row[c.ColumnName]);
                            }

                            writer.Write(list.ToArray());
                        }

                        writer.Complete();
                    }
                }
                return true;
            }
            catch { throw; }
            finally
            {
                CrearConnection(conn);
            }
        }

        public override bool BulkCopyFromFile(string tableName, string filePath, int offset = 0)
            => throw new NotSupportedException();

        #endregion BulkCopy

        #region HELPER_FUNCTIONS

        private void MappingQuery(NpgsqlCommand cmd, Query query)
        {
            if (query == null) return;
            cmd.CommandText = ConvertToExcuteSQL(query);

            cmd.Parameters.Clear();

            if (query.Parameters == null || query.Parameters.Any() == false) return;

            foreach (var p in query.Parameters)
            {
                if (cmd.Parameters.Contains(p.Name)) continue;

                NpgsqlParameter dbParameter = new NpgsqlParameter()
                {
                    ParameterName = p.Name,
                    Value = (p.Value ?? DBNull.Value),
                    Direction = ToParameterDirection(p.Direction)
                };

                cmd.Parameters.Add(dbParameter);
            }
        }

        #endregion HELPER_FUNCTIONS
    }
}