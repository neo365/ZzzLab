using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ZzzLab.Data.Models
{
    public class TableColomn : TableInfo
    {
        public string ColumnOrder { set; get; }

        [Required]
        public string ColumnName { set; get; }

        [Required]
        public string DataType { set; get; }

        public string DataLength { set; get; }

        public string DataPrecision { set; get; }

        public string ConstraintType { set; get; }

        public string RefSchemaName { set; get; }

        public string RefDataBaseName { get; private set; }

        public string RefTableName { set; get; }

        public string RefColumnName { get; private set; }

        [Required]
        public string Nullable { set; get; }

        public string DataDefault { set; get; }

        public string ColumnComment { set; get; }

        public TableColomn()
        {
        }

        public TableColomn(TableInfo table) : this()
        {
            this.SchemaName = table.SchemaName;
            this.TableName = table.TableName;
            this.TableType = table.TableType;
            this.TableComment = table.TableComment;
        }

        public new TableColomn Set(DataRow row)
        {
            this.DataBaseName = row.ToString("DATABASE_NAME");
            this.SchemaName = row.ToString("SCHEMA_NAME");
            this.TableName = row.ToString("TABLE_NAME");
            this.ColumnOrder = row.ToString("COLUMN_ORDER");
            this.ColumnName = row.ToString("COLUMN_NAME");
            this.DataType = row.ToString("DATA_TYPE");
            this.DataLength = row.ToStringNullable("DATA_LENGTH");
            this.DataPrecision = row.ToStringNullable("DATA_PRECISION");
            this.ConstraintType = row.ToStringNullable("CONSTRAINT_TYPE");
            this.RefSchemaName = row.ToStringNullable("REF_SCHEMA_NAME");
            this.RefDataBaseName = row.ToStringNullable("REF_DATABASE_NAME");
            this.RefTableName = row.ToStringNullable("REF_TABLE_NAME");
            this.RefColumnName = row.ToStringNullable("REF_COLUMN_NAME");
            this.Nullable = row.ToStringNullable("NULLABLE");
            this.DataDefault = row.ToStringNullable("DATA_DEFAULT");
            this.ColumnComment = row.ToStringNullable("COMMENTS");

            return this;
        }
    }
}