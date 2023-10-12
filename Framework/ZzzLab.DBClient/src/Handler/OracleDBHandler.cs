using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

#if TRACE

#endif

using System.Linq;
using ZzzLab.Data.Models;

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

        internal static string CreateConnectionString(string host, int port, string serviceName, string userid, string password, int timeout = 60)
            => $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port}))(CONNECT_DATA=(SERVICE_NAME={serviceName})));User Id ={userid}; Password={password};";

        #endregion Connectionstring

        #region Version

        public override string GetVersion()
            => SelectValue("SELECT DISTINCT VERSION FROM SYS.PRODUCT_COMPONENT_VERSION")?.ToString();

        #endregion Version

        #region Connection

        private OracleConnection CreateConnection()
            => new OracleConnection(this.ConnectionString);

        private void CrearConnection(OracleConnection conn)
        {
            if (conn == null) return;
            if (conn.State != ConnectionState.Closed) conn.Close();
            OracleConnection.ClearPool(conn);
        }

        public override IDbConnection CreateDBConnection()
            => CreateConnection();

        #endregion Connection

        #region Select

        public override object SelectValue(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            OracleConnection conn = null;
            try
            {
                using (conn = CreateConnection())
                {
                    conn.Open();
                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcutSQL(query);
                        cmd.CommandType = query.CommandType;
                        cmd.CommandTimeout = query.CommandTimeout;

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

            OracleConnection conn = null;

            try
            {
                DataTable result = new DataTable();

                using (conn = CreateConnection())
                {
                    conn.Open();

                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcutSQL(query);
                        cmd.CommandType = query.CommandType;
                        cmd.CommandTimeout = query.CommandTimeout;

                        if (query.Parameters != null && query.Parameters.Count > 0)
                        {
                            cmd.BindByName = true;
                            FormatValue(cmd, query);
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

        public override int Excute(QueryCollection queries)
        {
            if (queries == null || queries.Any() == false) return 0;

            Query currentQuery = null;

            OracleConnection conn = null;
            try
            {
                int resultcount = 0;
                using (conn = CreateConnection())
                {
                    conn.Open();
                    using (OracleCommand cmd = conn.CreateCommand())
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
                                     && query.Parameters.Any(x => x.Direction.HasMask(Direction.Output)))
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
                                        foreach (OracleParameter p in cmd.Parameters)
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

        #region HELPER_FUNCTIONS

        private void FormatValue(OracleCommand cmd, Query query)
        {
            if (query == null) return;

            cmd.BindByName = true;
            cmd.CommandText = ConvertToExcutSQL(query);

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

        /// <summary>
        /// 지정된 테이블의 정보를 가져온다.
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="databaseName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override TableInfo GetTableInfo(string schemaName, string databaseName, string tableName)
        {
            string query = TABLE_LIST_QUERY.ReplaceIgnoreCase("{WHERE_TABLE}", " AND TAB.OWNER = @SCHEMA_NAME AND TAB.TABLE_NAME = @TABLE_NAME ")
                                           .ReplaceIgnoreCase("{WHERE_VIEW}", " AND TAB.OWNER = @SCHEMA_NAME AND TAB.VIEW_NAME = @TABLE_NAME ");

            QueryParameterCollection parameters = new QueryParameterCollection
            {
                { "SCHEMA_NAME", schemaName },
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
                    , '' AS DATABASE_NAME
                    , COL.TABLE_NAME
                    , COL.COLUMN_ID  AS COLUMN_ORDER
                    , COL.COLUMN_NAME
                    , COL.DATA_TYPE
                    , COL.DATA_LENGTH
                    , COL.DATA_PRECISION
                    , CON.CONSTRAINT_TYPE
                    , CON.OWNER AS REF_SCHEMA_NAME
                    , '' AS REF_DATABASE_NAME
                    , CON.REF_TABLE_NAME
                    , '' AS CON.REF_COLUMN_NAME
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
                TableColomn item = new TableColomn(tableInfo);
                item.Set(row);

                list.Add(item);
            }

            return list;
        }

        #endregion DB Schema
    }
}