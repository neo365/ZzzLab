using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace ZzzLab.Data.Models
{
    public class TableInfo
    {
        [Required]
        public string DataBaseName { get; set; }

        [Required]
        public virtual string Schema { set; get; }

        [Required]
        public virtual string Name { set; get; }

        [Required]
        public virtual string TableType { set; get; }

        public virtual string Comment { set; get; }

        public IEnumerable<TableColomn> Columns { set; get; } = Enumerable.Empty<TableColomn>();

        public virtual TableInfo Set(DataRow row)
        {
            this.DataBaseName = row.ToString("DATABASE_NAME");
            this.Schema = row.ToString("SCHEMA_NAME");
            this.Name = row.ToString("TABLE_NAME");
            this.TableType = row.ToString("TABLE_TYPE");
            this.Comment = row.ToStringNullable("COMMENTS");

            return this;
        }
    }
}