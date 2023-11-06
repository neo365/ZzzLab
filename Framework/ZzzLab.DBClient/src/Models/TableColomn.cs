using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ZzzLab.Data.Models
{
    public class TableColomn
    {
        public virtual int OrderNo { set; get; }

        [Required]
        public virtual string Name { set; get; }

        [Required]
        public virtual string DataType { set; get; }

        public virtual string DataLength { set; get; }

        public virtual string DataPrecision { set; get; }

        public virtual string DataScale { set; get; }

        public virtual string ConstraintType { set; get; }

        public virtual string RefSchemaName { set; get; }

        public virtual string RefDataBaseName { get; private set; }

        public virtual string RefTableName { set; get; }

        public virtual string RefColumnName { get; private set; }

        [Required]
        public virtual bool IsNullable { set; get; }

        public virtual string DataDefault { set; get; }

        public virtual string Comment { set; get; }

        public virtual string ColumnType
        {
            get
            {
                switch (DataType.ToLower())
                {
                    case "bit":
                    case "bit varying":
                    case "character varying":
                    case "character":
                    case "char":
                    case "nchar":
                    case "varchar":
                    case "varchar2":
                    case "nvarchar":
                    case "nvarchar2":
                    case "raw":
                    case "urowid":
                    case "binary":
                    case "varbinary":
                        return $"{DataType}({DataLength})";

                    case "float":
                        return $"{DataType}({DataPrecision})";

                    case "number":
                    case "numeric":
                    case "decimal":
                        return $"{DataType}({(string.IsNullOrWhiteSpace(DataPrecision) ? "*" : DataPrecision)}{(string.IsNullOrWhiteSpace(DataScale) || (DataScale.ToIntNullable() ?? 0) > 0 ? $", {DataScale}" : string.Empty)})";

                    //ntext, text및 image 데이터 형식은 SQL Server이후 버전에서 제거됩니다. 향후 개발 작업에서는 이 데이터 형식을 사용하지 않도록 하고 현재 이 데이터 형식을 사용하는 애플리케이션은 수정하세요. 대신 nvarchar(max), varchar(max)및 varbinary(max) 를 사용합니다.
                    case "text":
                        //return "text";
                        return $"varchar(MAX)";

                    case "ntext":
                        //return "ntext";
                        return $"nvarchar(MAX)";

                    case "image":
                        //return "image";
                        return $"varbinary(MAX)";

                    case "time":
                    case "timestamp":
                        return string.IsNullOrWhiteSpace(this.DataPrecision) ? $"{DataType}" : $"{DataType}({DataPrecision})";

                    case "time with time zone":
                    case "timestamp with time zone":
                        return string.IsNullOrWhiteSpace(this.DataPrecision) ? $"{DataType} with time zone" : $"{DataType}({DataPrecision}) with time zone";

                    case "boolean":
                    case "box":
                    case "bytea":
                    case "cidr":
                    case "circle":
                    case "double precision":
                    case "inet":
                    case "integer":
                    case "line":
                    case "lseg":
                    case "macaddr":
                    case "path":
                    case "point":
                    case "polygon":
                    case "serial":
                    case "tsquery":
                    case "tsvector":
                    case "txid_snapshot":
                    case "uuid":
                    case "xml":
                    case "bigserial":                    
                    case "int":
                    case "smallint":
                    case "tinyint":
                    case "bigint":
                    case "long":
                    case "binary_float":
                    case "binary_double":
                    case "real":
                    case "money":
                    case "smallmoney":
                    case "date":
                    case "datetime":
                    case "datetime2":
                    case "smalldatetime":
                    case "datetimeoffset":
                    case "long raw":
                    case "rowid":
                    case "clob":
                    case "nclob":
                    case "blob":
                    case "bfile":
                        return $"{DataType})";

                    default:
                        return $"{DataType})";
                }
            }
        }

        public TableColomn()
        {
        }

        public virtual TableColomn Set(DataRow row)
        {
            this.OrderNo = row.ToIntNullable("COLUMN_ORDER") ?? 0;
            this.Name = row.ToString("COLUMN_NAME");
            this.DataType = row.ToString("DATA_TYPE");
            this.DataLength = row.ToStringNullable("DATA_LENGTH");
            this.DataPrecision = row.ToStringNullable("DATA_PRECISION");
            this.DataScale = row.ToStringNullable("DATA_SCALE");
            this.ConstraintType = row.ToStringNullable("CONSTRAINT_TYPE");
            this.RefSchemaName = row.ToStringNullable("REF_SCHEMA_NAME");
            this.RefDataBaseName = row.ToStringNullable("REF_DATABASE_NAME");
            this.RefTableName = row.ToStringNullable("REF_TABLE_NAME");
            this.RefColumnName = row.ToStringNullable("REF_COLUMN_NAME");
            this.IsNullable = row.ToBooleanNullable("NULLABLE") ?? true;
            this.DataDefault = row.ToStringNullable("DATA_DEFAULT");
            this.Comment = row.ToStringNullable("COMMENTS");

            return this;
        }
    }
}