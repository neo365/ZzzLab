using System;
using System.Data;
using System.Linq;

namespace ZzzLab.Data
{
    /// <summary>
    /// Query + Parameter 로 이루어진 Query Set
    /// </summary>
    public sealed class Query : IQuery, ICopyable, ICloneable
    {
        internal const int DEFATLT_COMMAND_TIMEOUT = 30;

        /// <summary>
        /// 실행될 SQL 쿼리
        /// </summary>
        public string CommandText { private set; get; }

        /// <summary>
        /// Procedure 여부
        /// </summary>
        public CommandType CommandType { private set; get; }

        /// <summary>
        /// 쿼리 실행시간. 기본값 30초.
        /// </summary>
        public int CommandTimeout { set; get; } = DEFATLT_COMMAND_TIMEOUT; // 30 sec

        /// <summary>
        /// Parameters
        /// </summary>
        public QueryParameterCollection Parameters { private set; get; } = QueryParameterCollection.Create();

        /// <summary>
        /// Query + Parameter 로 이루어진 Query Set
        /// </summary>
        /// <param name="commandText">쿼리</param>
        /// <param name="parameters">쿼리 파라미터 변수</param>
        /// <param name="commandType">System.Data.CommandType</param>
        /// <param name="commandTimeout">쿼리 Timeout 시간. 기본값 30초</param>
        /// <exception cref="ArgumentNullException">쿼리가 null인경우</exception>
        private Query(string commandText, QueryParameterCollection parameters = null, CommandType commandType = CommandType.Text, int commandTimeout = DEFATLT_COMMAND_TIMEOUT)
        {
            if (string.IsNullOrWhiteSpace(commandText)) throw new ArgumentNullException(nameof(commandText));

            this.CommandText = commandText.TrimEnd(' ', ';');
            this.CommandType = commandType;
            this.CommandTimeout = (commandTimeout <= 0 ? Query.DEFATLT_COMMAND_TIMEOUT : commandTimeout);

            if (parameters != null && parameters.Count > 0)
            {
                if (parameters.Any(x => x.Direction != Direction.Input))
                {
                    this.Parameters = parameters;
                }
                else this.Parameters = parameters.Clone();
            }
        }

        /// <summary>
        /// Query + Parameter 로 이루어진 Query Set
        /// </summary>
        /// <param name="commandText">쿼리</param>
        /// <param name="parameters">쿼리 파라미터 변수</param>
        /// <param name="commandType">System.Data.CommandType</param>
        /// <param name="commandTimeout">쿼리 Timeout 시간. 기본값 30초</param>
        /// <exception cref="ArgumentNullException">쿼리가 null인경우</exception>
        /// <returns></returns>
        public static Query Create(string commandText, QueryParameterCollection parameters = null, CommandType commandType = CommandType.Text, int commandTimeout = DEFATLT_COMMAND_TIMEOUT)
            => new Query(commandText, parameters, commandType, commandTimeout);

        /// <summary>
        ///  Query Set 을 변수가 적용된 sql 쿼리로 출력한다.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string commandText = CommandText.Replace(")", " ) ")
                                            .Replace("(", " ( ")
                                            .Replace("\n", " \n");
            commandText += " ";

            QueryParameterCollection parameters = Parameters.Clone();

            if (parameters.Count > 0)
            {
                foreach (string key in parameters.Keys)
                {
                    try
                    {
                        string tmpStr = string.Empty;
                        if (parameters[key] != null)
                        {
                            object obj = parameters[key].Value;
                            tmpStr = (obj == null ? "null" : $"'{obj}'");
                        }

                        commandText = commandText.ReplaceIgnoreCase("@" + key + " ", tmpStr + " ")
                                                 .ReplaceIgnoreCase("@" + key + ",", tmpStr + ",")
                                                 .ReplaceIgnoreCase("@" + key + ";", tmpStr + ";");
                    }
                    catch { }
                }
            }

            return commandText;
        }

        #region ICopyable

        public Query CopyTo(Query target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            target.CommandText = this.CommandText;
            target.CommandTimeout = this.CommandTimeout;
            target.Parameters = this.Parameters.Clone();

            return target;
        }

        public Query CopyFrom(Query source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.CommandText = source.CommandText;
            this.CommandTimeout = source.CommandTimeout;
            this.Parameters = source.Parameters.Clone();

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((Query)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((Query)source);

        #endregion ICopyable

        #region ICloneable

        public Query Clone()
            => new Query(this.CommandText, this.Parameters, this.CommandType, this.CommandTimeout);

        object ICloneable.Clone()
            => this.Clone();

        #endregion ICloneable
    }
}