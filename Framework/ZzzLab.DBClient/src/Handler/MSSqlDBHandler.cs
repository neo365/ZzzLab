using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ZzzLab.Data.Models;

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

        internal static string CreateConnectionString(string host, int port, string database, string userid, string password)
            => $"Data Source={host},{port};Network Library=DBMSSOCN;Initial Catalog = {database}; User ID ={userid}; Password={password};";

        #endregion Connectionstring

        #region Connection

        private SqlConnection CreateConnection()
            => new SqlConnection(this.ConnectionString);

        private void CrearConnection(SqlConnection conn)
        {
            if (conn == null) return;
            if (conn.State != ConnectionState.Closed) conn.Close();
            SqlConnection.ClearPool(conn);
        }

        public override IDbConnection CreateDBConnection()
            => CreateConnection();

        #endregion Connection

        #region Select

        public override object SelectValue(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            SqlConnection conn = null;
            try
            {
                using (conn = CreateConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcutSQL(query);
                        cmd.CommandType = query.CommandType;
                        cmd.CommandTimeout = query.CommandTimeout;
                        cmd.Parameters.Clear();

                        // prepare the command, which is significantly faster
                        cmd.Prepare();

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

        public override DataTable Select(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            SqlConnection conn = null;

            try
            {
                DataTable result = new DataTable();

                using (conn = CreateConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcutSQL(query);
                        cmd.CommandType = query.CommandType;
                        cmd.CommandTimeout = query.CommandTimeout;

                        FormatValue(cmd, query);

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

            Query currentQuery = null;

            SqlConnection conn = null;

            try
            {
                int resultcount = 0;
                using (conn = CreateConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandTimeout = queries.CommandTimeout;

                        if (queries.Count > 1 && cmd.Transaction == null) cmd.Transaction = cmd.Connection.BeginTransaction();

                        try
                        {
                            foreach (Query query in queries)
                            {
                                currentQuery = query;

                                cmd.CommandText = ConvertToExcutSQL(query);
                                cmd.CommandType = query.CommandType;
                                cmd.Parameters.Clear();
                                FormatValue(cmd, query);
                                cmd.Prepare();

                                if (query.Parameters != null
                                    && query.Parameters.Any(x => x.Direction.HasMask(Direction.Output))
                                    && (query.CommandText.ContainsIgnoreCase("OUTPUT ") || query.CommandText.ContainsIgnoreCase("SELECT "))
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
                                                if (query.Parameters.Contains(column.ColumnName) == true
                                                    && query.Parameters[column.ColumnName].Direction.HasMask(Direction.Output))
                                                {
                                                    query.Parameters[column.ColumnName].Value = row[column.ColumnName];
                                                    continue;
                                                }

                                                string columnName = column.ColumnName.ToPascalCase();

                                                if (query.Parameters.Contains(columnName) == true
                                                    && query.Parameters[columnName].Direction.HasMask(Direction.Output))
                                                {
                                                    query.Parameters[columnName].Value = row[column.ColumnName];
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    cmd.ExecuteNonQuery();

                                    if (query.Parameters != null
                                       && query.Parameters.Any(x => x.Direction.HasMask(Direction.ReturnValue))
                                    )
                                    {
                                        foreach (SqlParameter p in cmd.Parameters)
                                        {
                                            if (query.Parameters.Contains(p.ParameterName)
                                                && query.Parameters[p.ParameterName].Direction.HasMask(Direction.ReturnValue))
                                            {
                                                query.Parameters[p.ParameterName].Value = p.Value;
                                                continue;
                                            }

                                            string parameterName = p.ParameterName.ToPascalCase();

                                            if (query.Parameters.Contains(parameterName)
                                                && query.Parameters[parameterName].Direction.HasMask(Direction.ReturnValue))
                                            {
                                                query.Parameters[parameterName].Value = p.Value;
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

        public bool BulkCopy(DataTable table)
        {
            SqlConnection conn = null;
            try
            {
                using (conn = CreateConnection())
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

        private void FormatValue(SqlCommand cmd, Query query)
        {
            if (query == null) return;

            cmd.CommandText = ConvertToExcutSQL(query);

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

        #region DB Schema

        private const string TABLE_LIST_QUERY = @"
                    SELECT DISTINCT
                        TAB.OWNER AS SCHEMA_NAME,
                        TAB.TABLE_NAME,
                        TAB.TABLE_TYPE,
                        TAB.COMMENTS
                    FROM (
                    SELECT
                        TAB.OWNER,
                        TAB.TABLE_NAME,
                        'TABLE' AS TABLE_TYPE,
                        COM.COMMENTS
                    FROM ALL_TABLES TAB
                        LEFT OUTER JOIN ALL_TAB_COMMENTS COM
                            ON TAB.OWNER = COM.OWNER AND TAB.TABLE_NAME = COM.TABLE_NAME
                    WHERE TAB.OWNER NOT IN ('SYS', 'SYSTEM')
                      AND INSTR( TAB.TABLE_NAME, '$') = 0
                    {WHERE_TABLE}
                    UNION
                    SELECT
                        TAB.OWNER,
                        TAB.VIEW_NAME AS TABLE_NAME,
                        'VIEW' AS TABLE_TYPE,
                        COM.COMMENTS
                    FROM ALL_VIEWS TAB
                        LEFT OUTER JOIN ALL_TAB_COMMENTS COM
                            ON TAB.OWNER = COM.OWNER AND TAB.VIEW_NAME = COM.TABLE_NAME
                    WHERE TAB.OWNER NOT IN ('SYS', 'SYSTEM')
                      AND INSTR( TAB.VIEW_NAME, '$') = 0
                     {WHERE_VIEW}) TAB
                    WHERE 1 = 1
                    ORDER BY TAB.OWNER, TAB.TABLE_NAME";

        public override IEnumerable<TableInfo> GetTableList()
        {
            DataTable table = this.Select(TABLE_LIST_QUERY.RemoveIgnoreCase("{WHERE_TABLE}", "{WHERE_VIEW}"));

            if (table == null || table.Rows.Count == 0) return Enumerable.Empty<TableInfo>();

            List<TableInfo> list = new List<TableInfo>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(new TableInfo().Set(row));
            }

            return list;
        }

        public override TableInfo GetTableInfo(string schemaName, string databaseName, string tableName)
        {
            string query = TABLE_LIST_QUERY.ReplaceIgnoreCase("{WHERE_TABLE}", " AND TAB.OWNER = @SCHEMA_NAME AND TAB.TABLE_NAME = @TABLE_NAME ")
                                           .ReplaceIgnoreCase("{WHERE_VIEW}", " AND TAB.OWNER = @SCHEMA_NAME AND TAB.VIEW_NAME = @TABLE_NAME ");

            QueryParameterCollection parameters = new QueryParameterCollection
            {
                { "SCHEMA_NAME",schemaName },
                { "DATABASE_NAME", databaseName },
                { "TABLE_NAME", tableName }
            };

            DataRow row = this.SelectRow(query, parameters);

            if (row == null) return null;

            return new TableInfo().Set(row);
        }

        private const string TABLE_COLUMN_QUERY = @"
                SELECT
                      COL.OWNER AS SCHEMA_NAME
                    , COL.TABLE_NAME
                    , COL.COLUMN_ID  AS COLUMN_ORDER
                    , COL.COLUMN_NAME
                    , COL.DATA_TYPE
                    , COL.DATA_LENGTH
                    , COL.DATA_PRECISION
                    , CON.CONSTRAINT_TYPE
                    , CON.REF_OWNER
                    , CON.REF_TABLE_NAME
                    , COL.NULLABLE
                    , COL.DATA_DEFAULT
                    , COM.COMMENTS
                FROM ALL_TAB_COLUMNS COL
                    LEFT OUTER JOIN ALL_COL_COMMENTS COM
                        ON COL.OWNER = COM.OWNER AND COL.TABLE_NAME = COM.TABLE_NAME AND COL.COLUMN_NAME = COM.COLUMN_NAME AND COM.COMMENTS IS NOT NULL
                    LEFT OUTER JOIN  (
                      SELECT DISTINCT
                          CON.OWNER
                        , CON.TABLE_NAME
                        , COL.COLUMN_NAME
                        , LISTAGG(CON.CONSTRAINT_TYPE, ',') WITHIN GROUP (ORDER BY CON.CONSTRAINT_TYPE ASC) AS CONSTRAINT_TYPE
                        , CON.R_OWNER AS REF_OWNER
                        , REF.TABLE_NAME AS REF_TABLE_NAME
                      FROM ALL_CONSTRAINTS  CON
                         LEFT OUTER JOIN ALL_CONS_COLUMNS COL
                             ON CON.OWNER = COL.OWNER
                            AND CON.TABLE_NAME = COL.TABLE_NAME
                            AND CON.CONSTRAINT_NAME = COL.CONSTRAINT_NAME
                         LEFT OUTER JOIN (
                             SELECT DISTINCT
                                OWNER,
                                TABLE_NAME,
                                CONSTRAINT_NAME,
                                CONSTRAINT_TYPE
                            FROM ALL_CONSTRAINTS CON
                            WHERE 1 = 1
                              AND OWNER = :SCHEMA_NAME
                              AND TABLE_NAME = :TABLE_NAME
                         ) REF
                            ON REF.CONSTRAINT_NAME = CON.R_CONSTRAINT_NAME
                    WHERE CON.CONSTRAINT_TYPE IN ('P', 'R', 'U')
                      AND COL.OWNER NOT IN ('SYS', 'SYSTEM')
                      AND INSTR( COL.TABLE_NAME, '$') = 0
                    GROUP BY CON.OWNER, CON.TABLE_NAME, COL.COLUMN_NAME, CON.R_OWNER, REF.TABLE_NAME
                    ) CON
                      ON  COL.OWNER = CON.OWNER AND COL.TABLE_NAME = CON.TABLE_NAME AND COL.COLUMN_NAME = CON.COLUMN_NAME
                WHERE 1 = 1
                  AND COL.OWNER NOT IN ('SYS', 'SYSTEM')
                  AND INSTR( COL.TABLE_NAME, '$') = 0
                  AND COL.OWNER = :SCHEMA_NAME
                  AND COL.TABLE_NAME = :TABLE_NAME
                ORDER BY COL.OWNER, COL.TABLE_NAME, COL.COLUMN_ID";

        /// <summary>
        /// 테이블의 컬럼정보를 가져온다.
        /// </summary>
        /// <param name="tableInfo">TableInfo</param>
        /// <returns></returns>
        public override IEnumerable<TableColomn> GetTableColumns(TableInfo tableInfo)
        {
            QueryParameterCollection parameters = new QueryParameterCollection
            {
                { "SCHEMA_NAME", tableInfo.SchemaName },
                { "DATABASE_NAME", tableInfo.DataBaseName },
                { "TABLE_NAME", tableInfo.TableName }
            };

            DataTable table = this.Select(TABLE_COLUMN_QUERY, parameters);

            if (table == null || table.Rows.Count == 0) return Enumerable.Empty<TableColomn>();

            List<TableColomn> list = new List<TableColomn>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(new TableColomn().Set(row));
            }

            return list;
        }

        #endregion DB Schema
    }
}