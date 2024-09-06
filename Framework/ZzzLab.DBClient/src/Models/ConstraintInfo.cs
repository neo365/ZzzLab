using System.Collections.Generic;

namespace ZzzLab.Data.Models
{
    public class ConstraintInfo
    {
        public string Owner { get; set; }
        public string TableName { get; set; }
        public string ConstraintName { get; set; }
        public string ConstraintType { set; get; }
        public IEnumerable<string> Columns { get; set; }
    }
}