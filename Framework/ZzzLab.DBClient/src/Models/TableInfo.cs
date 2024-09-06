using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace ZzzLab.Data.Models
{
    public class TableInfo
    {
        public virtual string DataBaseName { get; set; } = string.Empty;

        public virtual string SchemaName { get; set; } = string.Empty;

        [Required]
        public virtual string TableName { get; set; } = string.Empty;

        public virtual string DBLink { set; get; }

        public virtual string FullName => $"{TableName}{(string.IsNullOrWhiteSpace(DBLink) ? string.Empty : "@" + DBLink)}";

        [Required]
        public virtual string TableType { get; set; } = string.Empty;

        public virtual string Comment { get; set; }

        public virtual string CreatedDate { get; set; }

        public virtual string UpdatedDate { get; set; }

        public IEnumerable<ColumnInfo> Columns { set; get; } = Enumerable.Empty<ColumnInfo>();

        public IEnumerable<IndexInfo> Indexes { set; get; } = Enumerable.Empty<IndexInfo>();

        public IEnumerable<IndexInfo> Keys { set; get; } = Enumerable.Empty<IndexInfo>();

        public virtual TableInfo Set(DataRow row)
        {
            this.DataBaseName = row.ToStringNullable("DATABASE_NAME");
            this.SchemaName = row.ToString("SCHEMA_NAME");
            this.TableName = row.ToString("TABLE_NAME");
            this.DBLink = row.ToStringNullable("DBLINK")?.TrimStart('@');
            this.TableType = row.ToString("TABLE_TYPE");

            this.Comment = row.ToStringNullable("COMMENTS");
            this.CreatedDate = row.ToDateTimeNullable("WHEN_CREATED")?.ToString("yyyy-MM-dd HH:mm:ss");
            this.UpdatedDate = row.ToDateTimeNullable("WHEN_UPDATED")?.ToString("yyyy-MM-dd HH:mm:ss");

            return this;
        }

        public override string ToString()
            => $"{SchemaName}.{FullName} : {Comment}";
    }
}