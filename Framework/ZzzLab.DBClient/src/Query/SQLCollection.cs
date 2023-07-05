using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ZzzLab.Collections;

namespace ZzzLab.Data
{
    public sealed class SQLCollection : CollectionsBase<SqlEntity>, IEnumerable<SqlEntity>, IEnumerable
    {
        public SQLCollection()
        {
        }

        public string Get(string section, string label, Hashtable parameters = null)
        {
            if (string.IsNullOrWhiteSpace(section)) throw new ArgumentNullException(nameof(section));
            if (string.IsNullOrWhiteSpace(label)) throw new ArgumentNullException(nameof(label));

            SqlEntity item = this.Items.Find(x => x.Section.EndsWithIgnoreCase(section) && x.Label.EndsWithIgnoreCase(label)) ?? throw new NotFoundException($"Query Not Found: [{section}] {label}");
            string query = item.Command;

            if (parameters != null && parameters.Count > 0)
            {
                foreach (string key in parameters.Keys)
                {
                    string keyvalue = "--{" + key.ToLower() + "}";
                    string value = parameters[key]?.ToString();

                    if (query.Contains(keyvalue)) query = query.Replace(keyvalue, value);
                }
            }

            query = query.Replace("--{orderby}", string.Empty)
                         .Replace("--{search}", string.Empty)
                         .Replace("--{output}", string.Empty);

            return query;
        }

        public void Add(string section, string label, string command)
            => Add(SqlEntity.Create(section, label, command));

        public override void AddRange(IEnumerable<SqlEntity> collection)
        {
            foreach (SqlEntity query in collection)
            {
                if (string.IsNullOrWhiteSpace(query.Section)) throw new ArgumentNullException(nameof(query.Section));
                if (string.IsNullOrWhiteSpace(query.Label)) throw new ArgumentNullException(nameof(query.Label));
                if (string.IsNullOrWhiteSpace(query.Command)) throw new ArgumentNullException(nameof(query.Command));

                SqlEntity item = this.Items.Find(x => x.Section.EndsWithIgnoreCase(query.Section) && x.Label.EndsWithIgnoreCase(query.Label));

                if (item == null) this.Items.Add(query);
                else item.Set(item.Command);
            }
        }

        public override void Insert(int index, SqlEntity item)
            => throw new NotSupportedException();

        public override int IndexOf(SqlEntity item)
            => throw new NotSupportedException();

        public override bool Remove(SqlEntity item)
        {
            SqlEntity query = this.Items.Find(x => x.Section.EndsWithIgnoreCase(item.Section) && x.Label.EndsWithIgnoreCase(item.Label));

            if (query != null) return Items.Remove(query);

            return true;
        }

        public bool RemoveAt(string section, string label)
        {
            SqlEntity item = this.Items.Find(x => x.Section.EndsWithIgnoreCase(section) && x.Label.EndsWithIgnoreCase(label));

            if (item != null) return Items.Remove(item);

            return true;
        }

        public override void RemoveAt(int index)
            => throw new NotSupportedException();

        public override void Clear()
            => Items.Clear();

        public override bool Contains(SqlEntity item)
            => this.Items.Any(x => x.Section.EndsWithIgnoreCase(item.Section) && x.Label.EndsWithIgnoreCase(item.Label));
    }
}