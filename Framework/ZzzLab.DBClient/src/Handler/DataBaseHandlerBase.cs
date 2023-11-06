using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ZzzLab.Data
{
    public abstract class DataBaseHandlerBase : IDBHandler, IDisposable
    {
        #region property

        public virtual bool IsDebug { set; get; } = false;

        public virtual string ConnectionString { protected set; get; }
        public virtual DataBaseType ServerType { protected set; get; }

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
            => Excute(QueryCollection.Create(queries));

        public abstract int Excute(QueryCollection queries);

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

        public string GetQuery(string section, string label)
            => DBClient.Queries.Get(section, label);

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
                default:
                    throw new NotImplementedException("Not Support DBServer");
            }
        }

        protected virtual string ConvertToExcuteSQL(Query query)
        {
            char parameterChar = GetParameterChar();
            string commandText = CheckCommand(query.CommandText);

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
                    newsql = newsql.Replace('@', parameterChar);

                    sb.Append(newsql);
                }
                else sb.Append($"'{sqls[i]}'");
            }

            sb.Append(' ');

            return sb.ToString();
        }

        protected virtual string CheckCommand(string commandText)
        {
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