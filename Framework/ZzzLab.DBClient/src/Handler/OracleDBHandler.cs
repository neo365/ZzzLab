using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

#if TRACE

#endif

using System.Linq;

namespace ZzzLab.Data
{
    public sealed class OracleDBHandler : DataBaseHandlerBase, IDisposable, IDBHandler
    {
        #region Construct

        public OracleDBHandler(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString?.Trim()))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.ConnectionString = connectionString;
            this.ServerType = DataBaseType.Oracle;
        }

        #endregion Construct

        #region Connectionstring

        internal static string CreateConnectionString(string host, int port, string serviceName, string userid, string password, int timeout = 15)
            => $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port}))(CONNECT_DATA=(SERVICE_NAME={serviceName})));User Id ={userid}; Password={password};;Connection Timeout={timeout};";

        #endregion Connectionstring

        #region Version

        public override string GetVersion()
            => SelectValue("SELECT DISTINCT VERSION FROM SYS.PRODUCT_COMPONENT_VERSION")?.ToString();

        #endregion Version

        #region Connection

        protected override IDbConnection CreateConnection()
            => this.CreateDBConnection();

        protected override void CrearConnection(IDbConnection conn)
            => this.CrearDBConnection(conn as OracleConnection);

        private OracleConnection CreateDBConnection()
            => new OracleConnection(this.ConnectionString);

        private void CrearDBConnection(OracleConnection conn)
        {
            if (conn == null) return;
            if (conn.State != ConnectionState.Closed) conn.Close();
            OracleConnection.ClearPool(conn);
        }

        #endregion Connection

        #region Select

        public override object SelectValue(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            OracleConnection conn = null;
            try
            {
                using (conn = CreateDBConnection())
                {
                    conn.Open();
                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcuteSQL(query);
                        cmd.CommandType = query.CommandType;
                        cmd.CommandTimeout = query.CommandTimeout;

                        MappingQuery(cmd, query);
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

            OracleConnection conn = null;

            try
            {
                DataTable result = new DataTable();

                using (conn = CreateDBConnection())
                {
                    conn.Open();

                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcuteSQL(query);
                        cmd.CommandType = query.CommandType;
                        cmd.CommandTimeout = query.CommandTimeout;

                        if (query.Parameters != null && query.Parameters.Count > 0)
                        {
                            cmd.BindByName = true;
                            MappingQuery(cmd, query);
                        }

                        cmd.InitialLOBFetchSize = -1;

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

        public override int Excute(QueriesCollection queries)
        {
            if (queries == null || queries.Any() == false) return 0;

            Query query = null;
            OracleConnection conn = null;
            try
            {
                int resultcount = 0;
                using (conn = CreateDBConnection())
                {
                    conn.Open();
                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandTimeout = queries.CommandTimeout;

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
                                cmd.Prepare();

                                if (q.Parameters != null
                                     && q.Parameters.Any(x => x.Direction.HasMask(Direction.Output)))
                                {
                                    DataTable result = new DataTable();
                                    using (var reader = cmd.ExecuteReader())
                                    {
                                        if (reader.HasRows) result.Load(reader);
                                        if (reader.IsClosed == false) reader.Close();

                                        if (result != null && result.Rows.Count > 0)
                                        {
                                            DataRow row = result.Rows[0];
                                            foreach (DataColumn column in result.Columns)
                                            {
                                                if (q.Parameters.Contains(column.ColumnName) == true
                                                    && q.Parameters[column.ColumnName].Direction.HasMask(Direction.Output))
                                                {
                                                    q.Parameters[column.ColumnName].Value = row[column.ColumnName];
                                                    continue;
                                                }

                                                string columnName = column.ColumnName.ToPascalCase();

                                                if (q.Parameters.Contains(columnName) == true
                                                    && q.Parameters[columnName].Direction.HasMask(Direction.Output))
                                                {
                                                    q.Parameters[columnName].Value = row[column.ColumnName];
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    cmd.ExecuteNonQuery();

                                    if (q.Parameters != null
                                       && q.Parameters.Any(x => x.Direction.HasMask(Direction.ReturnValue))
                                    )
                                    {
                                        foreach (OracleParameter p in cmd.Parameters)
                                        {
                                            if (q.Parameters.Contains(p.ParameterName)
                                                && q.Parameters[p.ParameterName].Direction.HasMask(Direction.ReturnValue))
                                            {
                                                q.Parameters[p.ParameterName].Value = p.Value;
                                                continue;
                                            }

                                            string parameterName = p.ParameterName.ToPascalCase();

                                            if (q.Parameters.Contains(parameterName)
                                                && q.Parameters[parameterName].Direction.HasMask(Direction.ReturnValue))
                                            {
                                                q.Parameters[parameterName].Value = p.Value;
                                                continue;
                                            }
                                        }
                                    }
                                }
                                resultcount++;
                            }

                            cmd.Transaction?.Commit(); // 트랜잭션commit
                        }
                        catch
                        {
                            Logger.Debug(query.ToString());
                            cmd.Transaction?.Rollback(); // 에러발생시rollback
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

        #region BulkCopy

        public override bool BulkCopy(DataTable table)
        {
            if (table == null || table.Rows.Count == 0) return true;

            OracleConnection conn = null;
            try
            {
                using (conn = CreateDBConnection())
                {
                    conn.Open();

                    using (OracleBulkCopy bulkcopy = new OracleBulkCopy(conn))
                    {
                        bulkcopy.DestinationTableName = table.TableName;
                        foreach (var column in table.Columns)
                        {
                            bulkcopy.ColumnMappings.Add(column.ToString(), column.ToString());
                        }
                        bulkcopy.WriteToServer(table);
                    }

                    conn.Close();
                }

                return true;
            }
            catch { throw; }
            finally
            {
                CrearConnection(conn);
            }
        }

        #endregion BulkCopy

        #region HELPER_FUNCTIONS

        private void MappingQuery(OracleCommand cmd, Query query)
        {
            if (query == null) return;

            cmd.BindByName = true;
            cmd.CommandText = ConvertToExcuteSQL(query);

            cmd.Parameters.Clear();

            if (query.Parameters == null || query.Parameters.Any() == false) return;

            foreach (var p in query.Parameters)
            {
                if (cmd.Parameters.Contains(p.Name)) continue;

                OracleParameter dbParameter = new OracleParameter()
                {
                    ParameterName = p.Name,
                    Value = (p.Value ?? DBNull.Value),
                    Direction = ToParameterDirection(p.Direction)
                };

                if (dbParameter.Value is string str)
                {
                    if (str.Length > 4000) dbParameter.OracleDbType = OracleDbType.Clob;
                }
                else if (dbParameter.Value is byte[] bytes)
                {
                    dbParameter.OracleDbType = OracleDbType.Blob;
                    dbParameter.Size = bytes.Length;
                }

                cmd.Parameters.Add(dbParameter);
            }
        }

        #endregion HELPER_FUNCTIONS
    }
}