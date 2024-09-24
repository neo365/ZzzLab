using System.Collections.Generic;
using System.Data;

namespace ZzzLab.Data.Models
{
    public class ConstraintInfo
    {
        public string Owner { get; set; }
        public string TableName { get; set; }
        public string ConstraintName { get; set; }
        public string ConstraintType { set; get; }
        public IEnumerable<string> Columns { get; set; }

        public virtual ConstraintInfo Set(DataRow row)
        {
            this.Owner = row.ToString("OWNER");
            this.TableName = row.ToString("TABLE_NAME");
            this.ConstraintName = row.ToString("CONSTRAINT_NAME");
            this.ConstraintType = row.ToString("CONSTRAINT_TYPE");

            return this;
        }
    }
}