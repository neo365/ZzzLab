using System.Collections.Generic;
using ZzzLab.Configuration;

namespace ZzzLab.Data.Configuration
{
    public interface IDBConfigurationLoader : IConfigurationLoader<ConnectionConfig>
    {
        IEnumerable<SqlEntity> QueryReader();

        bool QueryWriter(params SqlEntity[] collection);

        bool QueryWriter(IEnumerable<SqlEntity> collection);
    }
}