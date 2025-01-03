using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;

namespace ZzzLab.Data.Models
{
    [DataContract]
    public class TableColumnInfo : ICopyable, ICloneable, IDataRowSupport<TableColumnInfo>
    {
        [DataMember]
        public virtual int OrderNo { get; set; } = 0;

        [Required]
        [DataMember]
        public virtual string ColumnName { get; set; } = string.Empty;

        [Required]
        [DataMember]
        public virtual string DataType { get; set; } = string.Empty;

        [Required]
        [DataMember]
        public virtual string DataLength { get; set; }

        [DataMember]
        public virtual string DataPrecision { get; set; }

        [DataMember]
        public virtual string DataScale { set; get; }

        [DataMember]
        public virtual string DataFormat
        {
            get => GetDataType();
        }

        [DataMember]
        public virtual string ConstraintType { get; set; }

        [Required]
        [DataMember]
        public virtual bool IsNullable { get; set; } = true;

        [DataMember]
        public virtual string NullableStr { get => (IsNullable ? "○" : string.Empty); }

        [DataMember]
        public virtual string Nullable
        {
            get => this.IsNullable.ToYN();
            set => this.IsNullable = value.ToBoolean();
        }

        [DataMember]
        public virtual string DataDefault { get; set; }

        [DataMember]
        public virtual string Comment { get; set; }

        public TableColumnInfo(TableColumnInfo info = null)
        {
            if (info != null) this.Set(info);
        }

        public TableColumnInfo Set(TableColumnInfo info)
            => this.CopyFrom(info);

        #region IDataRowSupport

        public virtual TableColumnInfo Set(DataRow row)
        {
            this.OrderNo = row.ToInt("COLUMN_ORDER");
            this.ColumnName = row.ToString("COLUMN_NAME");
            this.DataType = row.ToString("DATA_TYPE");
            this.DataLength = row.ToStringNullable("DATA_LENGTH", throwOnError: false);
            this.DataPrecision = row.ToStringNullable("DATA_PRECISION", throwOnError: false);
            this.DataScale = row.ToStringNullable("DATA_SCALE", throwOnError: false);
            this.ConstraintType = row.ToStringNullable("CONSTRAINT_TYPE", throwOnError: false);
            this.IsNullable = row.ToBooleanNullable("NULLABLE", throwOnError: false) ?? true;
            this.DataDefault = row.ToStringNullable("DATA_DEFAULT", throwOnError: false)?.Trim();
            this.Comment = row.ToStringNullable("COMMENTS", throwOnError: false);

            return this;
        }

        #endregion IDataRowSupport

        public string GetDataType()
        {
            switch (DataType.ToLower())
            {
                case "number":
                    string result = DataType;

                    if (string.IsNullOrWhiteSpace(DataPrecision) == false)
                    {
                        result += $"({DataPrecision}{(string.IsNullOrWhiteSpace(DataScale) || (DataScale.ToIntNullable() ?? 0) > 0 ? $", {DataScale}" : string.Empty)})";
                    }
                    return result;

                case "numeric":
                case "decimal":
                    return $"{DataType}({(string.IsNullOrWhiteSpace(DataPrecision) ? "*" : DataPrecision)}{(string.IsNullOrWhiteSpace(DataScale) || (DataScale.ToIntNullable() ?? 0) > 0 ? $", {DataScale}" : string.Empty)})";

                case "bit":
                case "bit varying":
                case "character varying":
                case "character":
                case "char":
                case "nchar": // check
                case "varchar":
                case "varchar2":
                case "nvarchar":
                case "nvarchar2":
                case "raw": // check
                case "urowid": // check
                case "binary": // check
                case "varbinary": // check
                    return $"{DataType}({DataLength})";

                case "float":
                    return $"{DataType}({DataPrecision})";

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
                    return DataType;

                default:
                    return DataType;
            }
        }

        public virtual string ToScript()
            => $"{ColumnName} {GetDataType()}{(string.IsNullOrWhiteSpace(DataDefault) ? string.Empty : $" default {DataDefault}")} {(IsNullable ? string.Empty : " not Null")}";

        #region Override

        public override string ToString()
            => $"{ColumnName} {GetDataType()}{(string.IsNullOrWhiteSpace(DataDefault) ? string.Empty : $" default {DataDefault}")} {(IsNullable ? string.Empty : " not Null")} : {Comment}";

        #endregion Override

        #region ICopyable

        public virtual TableColumnInfo CopyTo(TableColumnInfo target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            target.OrderNo = this.OrderNo;
            target.ColumnName = this.ColumnName;
            target.DataType = this.DataType;
            target.DataLength = this.DataLength;
            target.DataPrecision = this.DataPrecision;
            target.DataScale = this.DataScale;
            target.ConstraintType = this.ConstraintType;
            target.IsNullable = this.IsNullable;
            target.DataDefault = this.DataDefault;
            target.Comment = this.Comment;

            return target;
        }

        public virtual TableColumnInfo CopyFrom(TableColumnInfo source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.OrderNo = source.OrderNo;
            this.ColumnName = source.ColumnName;
            this.DataType = source.DataType;
            this.DataLength = source.DataLength;
            this.DataPrecision = source.DataPrecision;
            this.DataScale = source.DataScale;
            this.ConstraintType = source.ConstraintType;
            this.IsNullable = source.IsNullable;
            this.DataDefault = source.DataDefault;
            this.Comment = source.Comment;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((TableColumnInfo)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((TableColumnInfo)source);

        #endregion ICopyable

        #region ICloneable

        public virtual TableColumnInfo Clone()
            => CopyTo(new TableColumnInfo());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}