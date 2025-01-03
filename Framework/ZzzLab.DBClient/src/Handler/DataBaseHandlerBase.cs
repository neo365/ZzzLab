﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ZzzLab.Data
{
    public abstract class DataBaseHandlerBase : IDBHandler, IDisposable
    {
        #region property

        public virtual bool IsDebug { set; get; } = false;

        /// <summary>
        /// 연결문자열
        /// </summary>

        public virtual string ConnectionString { protected set; get; }

        /// <summary>
        /// 서버 종류
        /// </summary>
        public virtual DataBaseType ServerType { protected set; get; }

        /// <summary>
        /// 서버그룹
        /// </summary>
        public virtual string Group { internal set; get; }

        /// <summary>
        /// 서버명(코드)
        /// </summary>
        public virtual string Name { internal set; get; }

        /// <summary>
        /// 서버별칭
        /// </summary>
        public virtual string AliasName { internal set; get; }

        #endregion property

        #region Connection

        protected abstract IDbConnection CreateConnection();

        protected abstract void CrearConnection(IDbConnection conn);

        #endregion Connection

        #region version

        public virtual string GetVersion()
            => string.Empty;

        #endregion version

        #region ConnectionTest

        public virtual bool ConnectionTest()
        {
            IDbConnection conn = null;
            try
            {
                using (conn = CreateConnection())
                {
                    conn.Open();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                CrearConnection(conn);
            }

            return false;
        }

        #endregion ConnectionTest

        #region Select

        public virtual object SelectValue(string query)
            => SelectValue(Query.Create(query));

        public virtual object SelectValue(string query, QueryParameterCollection parameters)
            => SelectValue(Query.Create(query, parameters));

        public abstract object SelectValue(Query query);

        public virtual DataTable Select(string commandText)
            => Select(Query.Create(commandText));

        public virtual DataTable Select(string commandText, QueryParameterCollection parameters)
            => Select(Query.Create(commandText, parameters));

        public abstract DataTable Select(Query query);

        public virtual DataRow SelectRow(string commandText)
            => SelectRow(Query.Create(commandText));

        public virtual DataRow SelectRow(string commandText, QueryParameterCollection parameters)
            => SelectRow(Query.Create(commandText, parameters));

        public virtual DataRow SelectRow(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            DataTable table = Select(query);

            if (table != null && table.Rows.Count > 0) return table.Rows[0];

            return null;
        }

        #endregion Select

        #region Execute

        public virtual int Excute(string commandText)
            => Excute(Query.Create(commandText));

        public virtual int Excute(string commandText, QueryParameterCollection parameters)
            => Excute(Query.Create(commandText, parameters));

        public virtual int Excute(params Query[] queries)
            => Excute(QueriesCollection.Create(queries));

        public abstract int Excute(QueriesCollection queries);

        #endregion Execute

        #region BulkCopy

        /// <summary>
        /// BulkCopy 지원
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public virtual bool BulkCopy(DataTable table)
             => throw new NotSupportedException();

        public virtual bool BulkCopyFromFile(string tableName, string filePath, int offset = 0)
            => throw new NotSupportedException();

        #endregion BulkCopy

        public virtual void Vacuum(IDictionary<string, string> options = null)
            => throw new NotSupportedException();

        public virtual string GetQuery(string section, string label)
            => DBClient.Queries.Get(section, label);

        public virtual string GetQuery(string section, string label, QueryParameterCollection parameters)
        {
            string sql = DBClient.Queries.Get(section, label);

            if (parameters != null && parameters.Count > 0)
            {
                foreach (QueryParameter p in parameters)
                {
                    sql = sql.ReplaceIgnoreCase("#{{" + p.Name + "}}", p.Value?.ToString());
                    //sql = sql.ReplaceIgnoreCase("${{" + p.Name + "}}", $"'{p.Value?.ToString()}'");
                }
            }

            return sql;
        }

        public abstract string MakePagingQuery(string query, int pageNum, int pageSize);

        #region Utils

        protected virtual string StripParameter(string parameter)
        {
            int endIdx = 0;

            for (int i = 0; i < parameter.Length; i++)
            {
                if (((parameter[i] >= 'a' && parameter[i] <= 'z')
                    || (parameter[i] >= 'A' && parameter[i] <= 'Z')
                    || (parameter[i] >= '0' && parameter[i] <= '9')
                    || (parameter[i] == '_')
                    || ValidUtils.IsHangle(parameter[i])
                    || (parameter[i] == '-')) == false)
                {
                    endIdx = i;
                    break;
                }
                endIdx = i;
            }

            return parameter.Substring(0, endIdx);
        }

        protected char GetParameterChar()
        {
            switch (this.ServerType)
            {
                case DataBaseType.Oracle: return ':';
                case DataBaseType.MSSql: return '@';
                case DataBaseType.PostgreSQL: return '@';
                case DataBaseType.MySql: return '@';
                case DataBaseType.MariaDB: return '@';
                case DataBaseType.SQLite: return '@';
            }
            throw new NotImplementedException("Not Support DBServer");
        }

        protected virtual string ConvertToExcuteSQL(Query query)
        {
            char parameterChar = GetParameterChar();
            string commandText = CheckCommand(this.ServerType, query.CommandText, query.Parameters);

            string[] sqls = commandText.Split('\'');

            StringBuilder sb = new StringBuilder(string.Empty);

            for (int i = 0; i < sqls.Length; i++)
            {
                if (i % 2 == 0)
                {
                    string newsql = sqls[i];
                    if (this.ServerType != DataBaseType.PostgreSQL)
                    {
                        newsql = newsql.Replace(':', parameterChar);
                    }
                    //newsql = newsql.Replace('@', parameterChar); // Oracle problem

                    sb.Append(newsql);
                }
                else sb.Append($"'{sqls[i]}'");
            }

            sb.Append(' ');

            return sb.ToString();
        }

        protected virtual string CheckCommand(DataBaseType serverType, string commandText, QueryParameterCollection parameters)
        {
            if (parameters != null && parameters.Any())
            {
                foreach (QueryParameter parameter in parameters)
                    commandText = commandText.ReplaceIgnoreCase("${{" + parameter.Name + "}}", $"{GetParameterChar()}{parameter.Name}");
            }

            if (commandText.Trim().StartsWithIgnoreCaseOr("MERGE ")
                && commandText.Trim().EndsWith(";") == false)
            {
                return commandText + ";";
            }

            return commandText.TrimEnd(';', ' ');
        }

        protected ParameterDirection ToParameterDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Input: return ParameterDirection.Input;
                case Direction.Output: return ParameterDirection.Output;
                case Direction.InputOutput: return ParameterDirection.InputOutput;
                case Direction.ReturnValue: return ParameterDirection.ReturnValue;
                default:
                    break;
            }

            return ParameterDirection.Input;
        }

        #endregion Utils

        #region IDispose

        protected bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                disposedValue = true;
            }
        }

        public virtual void Dispose()
        {
            Dispose(disposing: true);
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        #endregion IDispose
    }
}