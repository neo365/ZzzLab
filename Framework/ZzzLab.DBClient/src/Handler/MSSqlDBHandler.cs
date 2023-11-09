using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ZzzLab.Data
{
    public sealed class MSSqlDBHandler : DataBaseHandlerBase, IDBHandler, IDisposable
    {
        #region Construct

        public MSSqlDBHandler(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString?.Trim()))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.ConnectionString = connectionString;
            this.ServerType = DataBaseType.MSSql;
        }

        #endregion Construct

        #region Connectionstring

        internal static string CreateConnectionString(string host, int port, string database, string userid, string password, int timeout = 15)
            => $"Data Source={host},{port};Initial Catalog={database}; User ID={userid}; Password={password};Connect Timeout={timeout}";

        #endregion Connectionstring

        #region Connection

        protected override IDbConnection CreateConnection()
            => this.CreateDBConnection();

        protected override void CrearConnection(IDbConnection conn)
            => this.CrearDBConnection(conn as SqlConnection);

        private SqlConnection CreateDBConnection()
            => new SqlConnection(this.ConnectionString);

        private void CrearDBConnection(SqlConnection conn)
        {
            if (conn == null) return;
            if (conn.State != ConnectionState.Closed) conn.Close();
            SqlConnection.ClearPool(conn);
        }

        #endregion Connection

        #region Select

        public override object SelectValue(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            SqlConnection conn = null;
            try
            {
                using (conn = CreateDBConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
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

            SqlConnection conn = null;

            try
            {
                DataTable result = new DataTable();

                using (conn = CreateDBConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcuteSQL(query);
                        cmd.CommandType = query.CommandType;
                        cmd.CommandTimeout = query.CommandTimeout;

                        MappingQuery(cmd, query);

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

            SqlConnection conn = null;

            try
            {
                int resultcount = 0;
                using (conn = CreateDBConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandTimeout = queries.CommandTimeout;

                        if (queries.Count > 1 && cmd.Transaction == null) cmd.Transaction = cmd.Connection.BeginTransaction();

                        try
                        {
                            cmd.Prepare();

                            foreach (Query q in queries)
                            {
                                query = q;
                                cmd.CommandText = ConvertToExcuteSQL(q);
                                cmd.CommandType = q.CommandType;
                                cmd.Parameters.Clear();
                                MappingQuery(cmd, q);

                                if (q.Parameters != null
                                    && q.Parameters.Any(x => x.Direction.HasMask(Direction.Output))
                                    && (q.CommandText.ContainsIgnoreCase("OUTPUT ") || q.CommandText.ContainsIgnoreCase("SELECT "))
                                )
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
                                        foreach (SqlParameter p in cmd.Parameters)
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
            SqlConnection conn = null;
            try
            {
                using (conn = CreateDBConnection())
                {
                    conn.Open();

                    using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
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

        private void MappingQuery(SqlCommand cmd, Query query)
        {
            if (query == null) return;
            cmd.CommandText = ConvertToExcuteSQL(query);
            cmd.Parameters.Clear();

            if (query.Parameters == null || query.Parameters.Any() == false) return;

            foreach (var p in query.Parameters)
            {
                if (cmd.Parameters.Contains(p.Name)) continue;

                SqlParameter dbParameter = new SqlParameter()
                {
                    ParameterName = p.Name,
                    Value = (p.Value ?? DBNull.Value),
                    Direction = ToParameterDirection(p.Direction)
                };

                cmd.Parameters.Add(dbParameter);
            }
        }

        public static bool MSSQLBulkcopy(string filepath,
                string tablename,
                string server,
                string userid,
                string password,
                string rowDemiter,
                string columnDemiter,
                bool hasHeader,
                int offset,
                out string message
        )
        {
            message = "";

            ConvertExtension.DosToUnix(filepath);
            ConvertExtension.UnixToDos(filepath);

            int TotalCount = File.ReadAllLines(filepath, Encoding.Default).Length + offset;

            if (hasHeader) TotalCount--;

            ProcessStartInfo startinfo = new ProcessStartInfo()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "bcp.exe",
                Arguments = $"{tablename} in {filepath} -r\"{rowDemiter}\"  -t\"{columnDemiter}\" -S{server} -U{userid} -P{password} -c ",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process pricess = Process.Start(startinfo);

            if (pricess == null) return false;

            pricess.WaitForExit(0);

            if (pricess.StandardOutput != null)
            {
                using (StreamReader reader = pricess.StandardOutput)
                {
                    string output = reader.ReadToEnd();
                    if (output.IndexOf("rows copied.") < 0 && output.IndexOf("개 행이 복사되었습니다.") < 0)
                    {
                        message = output;
                        return false;
                    }
                    else if (output.IndexOf(string.Format("{0} rows copied.", TotalCount)) < 0
                        && output.IndexOf(string.Format("{0}개 행이 복사되었습니다.", TotalCount)) < 0)
                    {
                        message = "복사된 수가 일치 하지 않습니다.\n";
                        message += "예상수 ;" + TotalCount + "\n";
                        message += output;
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion HELPER_FUNCTIONS
    }
}