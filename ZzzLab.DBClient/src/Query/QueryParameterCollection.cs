using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ZzzLab.Collections;

namespace ZzzLab.Data
{
    public sealed class QueryParameterCollection : CollectionsBase<QueryParameter>, ICloneable
    {
        public QueryParameter this[string name]
        {
            get { return this.Items.Find(x => x.Name.EqualsIgnoreCase(name)); }
            set
            {
                if (value == null) return;
                QueryParameter item = this.Items.Find(x => x.Name.EqualsIgnoreCase(name));

                if (item == null)
                {
                    this.Add(QueryParameter.Create(name, value.Value, value.Direction));
                }
                else
                {
                    item.Value = value.Value;
                    item.Direction = value.Direction;
                }
            }
        }

        public string[] Keys
        {
            get
            {
                List<string> list = new List<string>();

                foreach (QueryParameter item in Items)
                {
                    list.Add(item.Name);
                }

                return list.ToArray();
            }
        }

        public QueryParameterCollection() : base()
        {
            // Do Nothing
        }

        public QueryParameterCollection(IEnumerable<QueryParameter> collection) : this()
        {
            this.AddRange(collection);
        }

        public QueryParameterCollection(params QueryParameter[] collection) : this()
        {
            this.AddRange(collection);
        }

        public QueryParameterCollection(QueryParameter item) : this()
        {
            this.Add(item);
        }

        public static QueryParameterCollection Create()
            => new QueryParameterCollection();

        public static QueryParameterCollection Create(IEnumerable<QueryParameter> collection)
            => new QueryParameterCollection(collection);

        public static QueryParameterCollection Create(params QueryParameter[] collection)
            => new QueryParameterCollection(collection);

        public static QueryParameterCollection Create(QueryParameter item)
            => new QueryParameterCollection(item);

        public static QueryParameterCollection Create(string name, object value)
            => new QueryParameterCollection(QueryParameter.Create(name, value));

        public void Set(string name, object value)
        {
            QueryParameter item = this.Items.Find(x => x.Name.EqualsIgnoreCase(name));

            if (item == null) this.Add(QueryParameter.Create(name, value));
            else item.Value = value;
        }

        public override void Add(QueryParameter item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (this.Items.Any(x => x.Name.EqualsIgnoreCase(item.Name))) throw new DuplicateNameException(item.Name);

            base.Add(item);
        }

        public void Add(string name, object value, Direction direction = Direction.Input)
            => Add(QueryParameter.Create(name, value, direction));

        public override void AddRange(IEnumerable<QueryParameter> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (collection.Any() == false) return;

            foreach (QueryParameter item in collection)
            {
                if (this.Items.Any(x => x.Name.EqualsIgnoreCase(item.Name))) throw new DuplicateNameException(item.Name);
            }

            Items.AddRange(collection);
        }

        public override void Insert(int index, QueryParameter item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (this.Items.Any(x => x.Name.EqualsIgnoreCase(item.Name))) throw new DuplicateNameException(item.Name);

            this.Items.Insert(index, item);
        }

        public override bool Remove(QueryParameter item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            QueryParameter removeItem = this.Items.Find(x => x.Name.EqualsIgnoreCase(item.Name));
            if (removeItem == null) return false;

            return base.Remove(removeItem);
        }

        /// <summary>
        /// 지정된 이름을 가진 파라미터를 제거합니다.
        /// </summary>
        /// <param name="name">제거할 파라미터의 이름입니다. (대소문자구분X)</param>
        public void RemoveAt(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            QueryParameter removeItem = this.Items.Find(x => x.Name.EqualsIgnoreCase(name));
            if (removeItem == null) return;

            base.Remove(removeItem);
        }

        public bool Contains(string name)
            => this.Items.Any(x => x.Name.EqualsIgnoreCase(name));

        public override bool Contains(QueryParameter item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return Contains(item.Name);
        }

        public override int IndexOf(QueryParameter item)
            => this.Items.FindIndex(0, x => x.Name.EqualsIgnoreCase(item.Name));

        public int IndexOf(string name)
            => this.Items.FindIndex(0, x => x.Name.EqualsIgnoreCase(name));

        public QueryParameterCollection CopyTo(QueryParameterCollection target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            foreach (QueryParameter p in Items)
            {
                if (target.Any(x => x.Name.EqualsIgnoreCase(p.Name))) target.RemoveAt(p.Name);

                target.Add(p.Name, p.Value, p.Direction);
            }

            return target;
        }

        #region ICloneable

        public QueryParameterCollection Clone()
        {
            QueryParameterCollection collection = new QueryParameterCollection();

            foreach (QueryParameter p in Items)
            {
                collection.Add(p.Name, p.Value, p.Direction);
            }

            return collection;
        }

        object ICloneable.Clone() => this.Clone();

        #endregion ICloneable
    }
}