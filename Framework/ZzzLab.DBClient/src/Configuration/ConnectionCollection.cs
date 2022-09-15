using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ZzzLab.Collections;

namespace ZzzLab.Data.Configuration
{
    public class ConnectionCollection : CollectionsBase<ConnectionConfig>, IEnumerable<ConnectionConfig>, IEnumerable, ICloneable
    {
        public ConnectionCollection() : base()
        {
        }

        public ConnectionConfig this[string name]
        {
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                ConnectionConfig item = Items.Find(x => x.Name.EqualsIgnoreCase(name));

                if (item == null) this.Add(value);
                else item = value;
            }
            get => Items.Find(x => x.Name.EqualsIgnoreCase(name));
        }

        public string[] AllNames
        {
            get
            {
                List<string> list = new List<string>();

                foreach (ConnectionConfig s in this.Items)
                {
                    list.Add(s.Name);
                }

                return list.ToArray();
            }
        }

        public override void Add(ConnectionConfig item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (Items.Any(x => x.Name.EqualsIgnoreCase(item.Name))) throw new DuplicateItemException(item.Name);

            base.Add(item);
        }

        public override void AddRange(IEnumerable<ConnectionConfig> collection)
        {
            foreach (ConnectionConfig item in collection)
            {
                if (Items.Any(x => x.Name.EqualsIgnoreCase(item.Name))) throw new DuplicateItemException(item.Name);
            }

            Items.AddRange(collection);
        }

        public override void Insert(int index, ConnectionConfig item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (Items.Any(x => x.Name.EqualsIgnoreCase(item.Name))) throw new DuplicateItemException(item.Name);

            Items.Insert(index, item);
        }

        public void RemoveAt(string name)
            => base.Remove(Items.Find(x => x.Name.EqualsIgnoreCase(name)));

        public override bool Contains(ConnectionConfig item)
            => Contains(item.Name);

        public bool Contains(string name)
            => Items.Any(x => x.Name.EqualsIgnoreCase(name));

        public override int IndexOf(ConnectionConfig item)
            => Items.IndexOf(item);

        #region ICloneable

        public ConnectionCollection Clone()
        {
            return null;
        }

        object ICloneable.Clone()
            => this.Clone();

        #endregion ICloneable
    }
}