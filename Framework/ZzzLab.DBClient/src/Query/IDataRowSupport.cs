using System.Data;

namespace ZzzLab.Data
{
    internal interface IDataRowSupport<T> where T : class
    {
        T Set(DataRow row);
    }
}