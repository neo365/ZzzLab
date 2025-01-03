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

        public string Get(string section, string label)
        {
            if (string.IsNullOrWhiteSpace(section)) throw new ArgumentNullException(nameof(section));
            if (string.IsNullOrWhiteSpace(label)) throw new ArgumentNullException(nameof(label));

            return this.Items.FirstOrDefault(x => x.Section.EqualsIgnoreCase(section) && x.Label.EqualsIgnoreCase(label))?.Command ?? throw new NotFoundException($"Query Not Found: [{section}] {label}");
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

                SqlEntity item = this.Items.Find(x => x.Section.EqualsIgnoreCase(query.Section) && x.Label.EqualsIgnoreCase(query.Label));

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
            SqlEntity query = this.Items.Find(x => x.Section.EqualsIgnoreCase(item.Section) && x.Label.EqualsIgnoreCase(item.Label));

            if (query != null) return Items.Remove(query);

            return true;
        }

        public bool RemoveAt(string section, string label)
        {
            SqlEntity item = this.Items.Find(x => x.Section.EqualsIgnoreCase(section) && x.Label.EqualsIgnoreCase(label));

            if (item != null) return Items.Remove(item);

            return true;
        }

        public override void RemoveAt(int index)
            => throw new NotSupportedException();

        public override void Clear()
            => Items.Clear();

        public override bool Contains(SqlEntity item)
            => this.Items.Any(x => x.Section.EqualsIgnoreCase(item.Section) && x.Label.EqualsIgnoreCase(item.Label));
    }
}