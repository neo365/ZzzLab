using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ZzzLab.Data.Models;

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

        internal static string CreateConnectionString(string host, int port, string database, string userid, string password, int timeout = 60)
            => $"Host={host};Port={port};Database={database};User ID={userid};Password={password};Timeout={timeout}";

        #endregion Connectionstring

        #region Connection

        private NpgsqlConnection CreateConnection()
            => new NpgsqlConnection(this.ConnectionString);

        private void CrearConnection(NpgsqlConnection conn)
        {
            if (conn == null) return;
            if (conn.State != ConnectionState.Closed) conn.Close();
            NpgsqlConnection.ClearPool(conn);
        }

        public override IDbConnection CreateDBConnection()
            => CreateConnection();

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
                using (conn = CreateConnection())
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = conn.CreateCommand())
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

        public override DataTable Select(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            NpgsqlConnection conn = null;
            try
            {
                DataTable result = new DataTable();

                using (conn = CreateConnection())
                {
                    conn.Open();

                    using (NpgsqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ConvertToExcutSQL(query);
                        cmd.CommandType = query.CommandType;
                        cmd.CommandTimeout = query.CommandTimeout;
                        cmd.Parameters.Clear();
                        FormatValue(cmd, query);
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

            Query currentQuery = null;

            NpgsqlConnection conn = null;

            try
            {
                int resultcount = 0;
                using (conn = CreateConnection())
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = conn.CreateCommand())
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
                                        foreach (NpgsqlParameter p in cmd.Parameters.Cast<NpgsqlParameter>())
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

        public bool BulkCopy(DataTable table)
        {
            string sql = $"COPY {table.TableName} ({string.Join(",", table.Columns.GetNames())}) FROM STDIN (FORMAT BINARY)";
            NpgsqlConnection conn = null;
            try
            {
                using (conn = new NpgsqlConnection(this.ConnectionString))
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

        #endregion BulkCopy

        #region HELPER_FUNCTIONS

        private void FormatValue(NpgsqlCommand cmd, Query query)
        {
            if (query == null) return;
            cmd.CommandText = ConvertToExcutSQL(query);

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

        #region DB Schema

        private const string TABLE_LIST_QUERY = @"
                    SELECT
                        table_schema as SCHEMA_NAME,
                        table_name AS TABLE_NAME,
	                    CASE WHEN table_type = 'BASE TABLE' THEN 'TABLE'
		                     ELSE table_type
	                    END AS TABLE_TYPE,
	                    (SELECT DESCRIPTION FROM PG_DESCRIPTION PD WHERE PD.OBJOID = (SELECT RELID FROM PG_STAT_ALL_TABLES PS WHERE PS.RELNAME  = table_name) AND PD.OBJSUBID = 0) AS COMMENTS
                    FROM information_schema.tables
                    WHERE table_schema NOT IN ('pg_catalog', 'information_schema')
                    ORDER BY table_type";

        /// <summary>
        /// 테이블의 정보를 가져온다.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<TableInfo> GetTableList()
        {
            DataTable table = this.Select(TABLE_LIST_QUERY);

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
            string query = $"SELECT * FROM {TABLE_LIST_QUERY} WHERE SCHEMA_NAME @SCHEMA_NAME AND TABLE_NAME = @TABLE_NAME ";

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
	ISC.TABLE_SCHEMA AS SCHEMA_NAME,
    ISC.TABLE_CATALOG AS DATABASE_NAME,
	ISC.TABLE_NAME AS TABLE_NAME,
	ISC.ORDINAL_POSITION AS COLUMN_ORDER,
	ISC.COLUMN_NAME AS COLUMN_NAME,
	CASE WHEN UPPER(UDT_NAME) = 'BPCHAR' THEN 'CHAR'
	     WHEN UPPER(UDT_NAME) = 'INT4' THEN 'INT'
		 ELSE UDT_NAME
	END AS DATA_TYPE,
	ISC.CHARACTER_MAXIMUM_LENGTH AS DATA_LENGTH,
	ISC.NUMERIC_SCALE AS DATA_PRECISION,
	TCC.CONSTRAINT_TYPE AS CONSTRAINT_TYPE,
    TCC.REF_CATALOG AS REF_DATABASE_NAME,
    TCC.REF_SCHEMA_NAME AS REF_SCHEMA_NAME,
    TCC.REF_TABLE_NAME AS REF_TABLE_NAME,
    TCC.REF_COLUMN_NAME AS REF_COLUMN_NAME,
	CASE WHEN ISC.is_nullable = 'NO' THEN 'N'
	     ELSE 'Y'
	END AS NULLABLE,
	CASE WHEN LastIndexOf(COLUMN_DEFAULT, '::') < 0 THEN COLUMN_DEFAULT
		 ELSE substr(COLUMN_DEFAULT, 0, LastIndexOf(COLUMN_DEFAULT, '::') + 1)
	END AS DATA_DEFAULT,
	( SELECT DESCRIPTION FROM PG_DESCRIPTION
	                    WHERE OBJOID = (SELECT RELID FROM PG_STAT_ALL_TABLES PS WHERE SCHEMANAME NOT IN ('pg_catalog', 'pg_toast', 'information_schema') AND PS.RELNAME  = ISC.TABLE_NAME)
	                    AND  OBJSUBID = ISC.ORDINAL_POSITION
	) AS DESCRIPTION
FROM INFORMATION_SCHEMA.COLUMNS ISC
	LEFT JOIN (
		SELECT
		    CATALOG,
		    SCHEMA_NAME,
		    TABLE_NAME,
			COLUMN_NAME,
			string_agg(CONSTRAINT_TYPE , ',' ORDER BY CONSTRAINT_TYPE) AS CONSTRAINT_TYPE,
			string_agg(REF_CATALOG, ',') AS REF_CATALOG,
			string_agg(REF_SCHEMA_NAME, ',') AS REF_SCHEMA_NAME,
		    string_agg(REF_TABLE_NAME, ',') AS REF_TABLE_NAME,
		    string_agg(REF_COLUMN_NAME, ',') AS REF_COLUMN_NAME
		FROM (
			SELECT
			    TC.CONSTRAINT_CATALOG AS CATALOG,
			    TC.CONSTRAINT_SCHEMA AS SCHEMA_NAME,
                TC.TABLE_NAME AS TABLE_NAME,
				CASE WHEN TC.CONSTRAINT_TYPE = 'PRIMARY KEY' THEN 'P'
					WHEN TC.CONSTRAINT_TYPE = 'FOREIGN KEY' THEN 'R'
					WHEN TC.CONSTRAINT_TYPE = 'UNIQUE' THEN 'U'
					ELSE TC.CONSTRAINT_TYPE
				END AS CONSTRAINT_TYPE,
				KCU.COLUMN_NAME,
				REF.REF_CATALOG AS REF_CATALOG,
				REF.REF_SCHEMA_NAME AS REF_SCHEMA_NAME,
                REF.REF_TABLE_NAME AS REF_TABLE_NAME,
                REF.REF_COLUMN_NAME AS REF_COLUMN_NAME
			FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC
				LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU
					 ON TC.CONSTRAINT_CATALOG = KCU.CONSTRAINT_CATALOG
					AND TC.CONSTRAINT_SCHEMA = KCU.CONSTRAINT_SCHEMA
					AND TC.CONSTRAINT_NAME = KCU.CONSTRAINT_NAME
				LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC
					 ON TC.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG
					AND TC.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA
					AND TC.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
				LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CCU
					 ON RC.UNIQUE_CONSTRAINT_CATALOG = CCU.CONSTRAINT_CATALOG
					AND RC.UNIQUE_CONSTRAINT_SCHEMA = CCU.CONSTRAINT_SCHEMA
					AND RC.UNIQUE_CONSTRAINT_NAME = CCU.CONSTRAINT_NAME
                LEFT JOIN (
                        SELECT
                            RC.CONSTRAINT_CATALOG,
                            RC.CONSTRAINT_SCHEMA,
                            RC.CONSTRAINT_NAME,
                            KCU.CONSTRAINT_CATALOG AS REF_CATALOG,
                            KCU.CONSTRAINT_SCHEMA AS REF_SCHEMA_NAME,
                            KCU.TABLE_NAME AS REF_TABLE_NAME,
                            KCU.COLUMN_NAME AS REF_COLUMN_NAME
                            FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC
                        LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU  ON RC.UNIQUE_CONSTRAINT_NAME = KCU.CONSTRAINT_NAME
                    ) REF
                         ON RC.CONSTRAINT_CATALOG = REF.CONSTRAINT_CATALOG
                        AND RC.CONSTRAINT_SCHEMA = REF.CONSTRAINT_SCHEMA
                        AND RC.CONSTRAINT_NAME = REF.CONSTRAINT_NAME
			WHERE 1 = 1
			  AND TC.TABLE_SCHEMA NOT IN ('pg_catalog', 'information_schema')
			  AND KCU.COLUMN_NAME is not null
              AND TC.TABLE_SCHEMA = @SCHEMA_NAME
              AND TC.TABLE_NAME = @TABLE_NAME
		) TC
		GROUP BY CATALOG, SCHEMA_NAME, TABLE_NAME, COLUMN_NAME
	) TCC
		ON ISC.TABLE_SCHEMA = TCC.SCHEMA_NAME AND ISC.TABLE_NAME = TCC.TABLE_NAME AND ISC.COLUMN_NAME = TCC.COLUMN_NAME
WHERE ISC.TABLE_SCHEMA NOT IN ('pg_catalog', 'information_schema')
  AND ISC.TABLE_SCHEMA = @SCHEMA_NAME
  AND ISC.TABLE_NAME = @TABLE_NAME
ORDER BY ISC.TABLE_SCHEMA, ISC.TABLE_CATALOG, ISC.TABLE_NAME, ISC.ORDINAL_POSITION";

        /// <summary>
        /// 지정된 테이블의 컬럼정보를 가져온다.
        /// </summary>
        /// <param name="tableInfo">TableInfo</param>
        /// <returns></returns>
        public override IEnumerable<TableColomn> GetTableColumns(TableInfo tableInfo)
        {
            QueryParameterCollection parameters = new QueryParameterCollection
            {
                { "SCHEMA_NAME", tableInfo.SchemaName },
                { "TABLE_NAME", tableInfo.TableName }
            };

            DataTable table = this.Select(TABLE_COLUMN_QUERY, parameters);

            if (table == null || table.Rows.Count == 0) return Enumerable.Empty<TableColomn>();

            List<TableColomn> list = new List<TableColomn>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(new TableColomn(tableInfo).Set(row));
            }

            return list;
        }

        #endregion DB Schema
    }
}