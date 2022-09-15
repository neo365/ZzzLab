using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ZzzLab.Collections;

namespace ZzzLab.Data
{
    public class QueryCollection : CollectionsBase<Query>, IEnumerable<Query>, IEnumerable, ICloneable
    {
        /// <summary>
        /// 모튼 쿼리셋의 타임아웃시간을 더한값이 전체 값이 된다.
        /// </summary>
        public int CommandTimeout
        {
            get
            {
                int timeout = 0;
                foreach (Query query in this)
                {
                    timeout += query.CommandTimeout;
                }

                return timeout;
            }
        }

        public QueryCollection()
        {
        }

        public QueryCollection(IEnumerable<Query> collection) : this()
        {
            if (collection != null && collection.Any()) this.AddRange(collection);
        }

        public static QueryCollection Create() => new QueryCollection(null);

        public static QueryCollection Create(params Query[] collection)
            => new QueryCollection((IEnumerable<Query>)collection);

        public static QueryCollection Create(IEnumerable<Query> collection)
            => new QueryCollection(collection);

        /// <summary>
        /// QuerySet을 추가 한다.
        /// </summary>
        /// <param name="item"></param>
        /// <exception cref="ArgumentNullException">item이 null인 경우</exception>
        public override void Add(Query item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            base.Add(item);
        }

        public void Add(string commandText, QueryParameterCollection parameters = null, CommandType commandType = CommandType.Text, int commandTimeout = Query.DEFATLT_COMMAND_TIMEOUT)
             => this.Add(Query.Create(commandText, parameters, commandType, commandTimeout));

        public void Add(string commandText)
            => this.Add(Query.Create(commandText));

        public override void AddRange(IEnumerable<Query> collection)
        {
            if (collection != null && collection.Any()) Items.AddRange(collection);
        }

        public void AddRange(IEnumerable<string> collection)
        {
            if (collection != null && collection.Any())
            {
                foreach (string commandText in collection)
                {
                    this.Add(Query.Create(commandText));
                }
            }
        }

        public override void Insert(int index, Query item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            this.Items.Insert(index, item);
        }

        public override int IndexOf(Query item)
            => throw new NotSupportedException();

        public override bool Contains(Query item)
            => throw new NotSupportedException();

        #region ICloneable

        public QueryCollection Clone()
        {
            QueryCollection collection = QueryCollection.Create();

            foreach (Query query in this.Items)
            {
                collection.Add(query.Clone());
            }

            return collection;
        }

        object ICloneable.Clone()
            => this.Clone();

        #endregion ICloneable
    }
}