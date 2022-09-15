using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Collections
{
    public static class ObservableCollectionsExtension
    {
        public static T Find<T>(this ObservableCollection<T> collection, Func<T, bool> predicate) => collection.FirstOrDefault(predicate);

        public static IEnumerable<T> FindAll<T>(this ObservableCollection<T> collection, Func<T, bool> predicate) => collection.Where(predicate);

        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static int Remove<T>(this ObservableCollection<T> collection, params T[] items)
        {
            int count = 0;
            foreach (T item in items)
            {
                collection.Remove(item);
                count++;
            }

            return count;
        }

        public static int RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> predicate)
        {
            IEnumerable<T> items = collection.Where(predicate);

            if (items != null && items.Any()) return collection.RemoveAll(items.ToArray());

            return 0;
        }

        public static int RemoveAll<T>(this ObservableCollection<T> collection, params T[] items)
        {
            int count = 0;

            if (items != null && items.Length > 0)
            {
                foreach (T item in items)
                {
                    collection.Remove(item);
                    count++;
                }
            }

            return count;
        }

        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        {
            var sortableList = new List<T>(collection);
            sortableList.Sort(comparison);

            for (int i = 0; i < sortableList.Count; i++)
            {
                collection.Move(collection.IndexOf(sortableList[i]), i);
            }
        }

        public static T[] ToArray<T>(this ObservableCollection<T> collection)
            => ToArray<T>(collection, null);

        public static T[] ToArray<T>(this ObservableCollection<T> collection, Func<T, bool> predicate)
        {
            IEnumerable<T> items;
            if (predicate != null) items = collection.Where(predicate);
            else items = collection.ToList().ToArray();

            return items.ToArray<T>();
        }
    }
}