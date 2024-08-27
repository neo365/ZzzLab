using System.Collections.Generic;

namespace ZzzLab.Data.Models
{
    public class IndexInfo
    {
        public string Owner { get; set; }
        public string TableName { get; set; }
        public string IndexName { get; set; }
        public IEnumerable<IndexColumn> Columns { get; set; }
    }

    public class IndexColumn
    {
        public int OederNo { get; set; }
        public string ColumnName { get; set; }
        public string ColumnNameRef { get; set; }
        public string Descend { get; set; }
    }
}