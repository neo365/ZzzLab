using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ZzzLab.Data.Models
{
    public class TableInfo
    {
        [Required]
        public string DataBaseName { get; set; }

        [Required]
        public virtual string SchemaName { set; get; }

        [Required]
        public virtual string TableName { set; get; }

        [Required]
        public virtual string TableType { set; get; }

        public virtual string TableComment { set; get; }

        public virtual TableInfo Set(DataRow row)
        {
            this.DataBaseName = row.ToString("DATABASE_NAME");
            this.SchemaName = row.ToString("SCHEMA_NAME");
            this.TableName = row.ToString("TABLE_NAME");
            this.TableType = row.ToString("TABLE_TYPE");
            this.TableComment = row.ToStringNullable("COMMENTS");

            return this;
        }
    }
}