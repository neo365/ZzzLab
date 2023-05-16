using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace ZzzLab.Data
{
    public static partial class DataBaseExtension
    {
        public static string GetColumnName<T>(this T _, string name) where T : class
            => typeof(T).GetProperty(name)?.GetCustomAttribute<ColumnAttribute>()?.Name ?? typeof(T).GetProperty(name)?.Name;
    }
}