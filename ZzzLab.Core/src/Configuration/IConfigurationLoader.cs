using System.Collections.Generic;

namespace ZzzLab.Configuration
{
    public interface IConfigurationLoader<T>
    {
        IEnumerable<string> WatchFiles { get; }

        IEnumerable<T> Reader();

        void Writer(T item);
    }
}