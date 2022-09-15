using System;
using System.Collections;
using System.Collections.Generic;

namespace ZzzLab.Collections
{
    public abstract class CollectionsBase<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>
    {
        protected virtual List<T> Items { get; }

        public virtual T this[int index]
        {
            set
            {
                if (index <= Items.Count) throw new ArgumentOutOfRangeException(nameof(index));
                Items[index] = value;
            }
            get
            {
                if (index <= Items.Count) throw new ArgumentOutOfRangeException(nameof(index));
                return Items[index];
            }
        }

        public int Count => Items.Count;

        public bool IsReadOnly => false;

        protected CollectionsBase()
        {
            Items = new List<T>();
        }

        public virtual void Add(T item)
            => this.AddRange(item);

        public virtual void AddRange(params T[] collection)
            => AddRange((IEnumerable<T>)collection);

        public virtual void AddRange(IEnumerable<T> collection)
            => Items.AddRange(collection);

        public abstract void Insert(int index, T item);

        public virtual bool Remove(T item)
            => Items.Remove(item);

        public virtual void RemoveAt(int index)
            => Items.RemoveAt(index);

        public virtual void Clear()
            => Items.Clear();

        public abstract int IndexOf(T item);

        public abstract bool Contains(T item);

        public virtual void CopyTo(T[] array, int arrayIndex)
            => Items.CopyTo(array, arrayIndex);

        #region IEnumerable

        public virtual IEnumerator<T> GetEnumerator()
            => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        #endregion IEnumerable
    }
}