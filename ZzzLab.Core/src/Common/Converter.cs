using System.Collections.Generic;
using System.Linq;

namespace ZzzLab
{
    public static class Converter
    {
        public static IEnumerable<T> ToIEnumerable<T>(params T[] collection)
        {
            if (collection == null || collection.Any() == false) return Enumerable.Empty<T>();

            List<T> list = new List<T>(collection.Length);
            list.AddRange(collection);
            return list;
        }
    }
}