using System.Collections.Generic;

namespace ZzzLab.Data.Models
{
    public class KeyInfo
    {
        public string Owner { get; set; }
        public string TableName { get; set; }
        public string KeyName { get; set; }
        public string KeyType { set; get; }
        public string TargetTable { set; get; }
        public IEnumerable<string> Columns { get; set; }
    }
}