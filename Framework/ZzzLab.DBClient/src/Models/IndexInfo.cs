using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using ZzzLab;

namespace ZzzLab.Data.Models
{
    public class IndexInfo
    {
        public virtual string TableOwner { get; set; }

        [Required]
        public virtual string TableName { get; set; }

        public virtual string IndexOwner { get; set; }

        [Required]
        public virtual string IndexName { get; set; }

        public virtual string IndexType { get; set; }

        public virtual string Uniqueness { get; set; }

        public virtual IEnumerable<IndexColumn> Columns { get; set; }

        public virtual string ColumnNames
        {
            get
            {
                if (Columns == null) return null;

                string indexColumnNames = string.Empty;

                foreach (IndexColumn Column in Columns)
                {
                    indexColumnNames += $", {Column.ColumnName}";
                }

                return indexColumnNames.TrimStart(',').Trim();
            }
        }

        public virtual IndexInfo Set(DataRow row)
        {
            this.TableOwner = row.ToString("TABLE_OWNER");
            this.TableName = row.ToString("TABLE_NAME");
            this.IndexOwner = row.ToString("INDEX_OWNER");
            this.IndexName = row.ToString("INDEX_NAME");
            this.IndexType = row.ToString("INDEX_TYPE");
            this.Uniqueness = row.ToString("UNIQUENESS");

            return this;
        }
    }
}

public class IndexColumn
{
    protected virtual string TableOwner { get; set; }
    protected virtual string TableName { get; set; }
    protected virtual string IndexName { get; set; }

    public virtual int OrderNo { get; set; }

    [Required]
    public virtual string ColumnName { get; set; }

    public string _ColumnNameRef = string.Empty;

    public virtual string ColumnNameRef
    {
        get => string.IsNullOrWhiteSpace(_ColumnNameRef) ? ColumnName : _ColumnNameRef;
        set => _ColumnNameRef = value;
    }

    public virtual string Descend { get; set; } = "NONE";

    public virtual IndexColumn Set(DataRow row)
    {
        this.TableOwner = row.ToString("OWNER");
        this.TableName = row.ToString("TABLE_NAME");
        this.IndexName = row.ToString("INDEX_NAME");
        this.OrderNo = row.ToInt("ORDER_NO");
        this.ColumnName = row.ToString("COLUMN_NAME");
        this.ColumnNameRef = row.ToStringNullable("COLUMN_NAME_REF");
        this.Descend = row.ToStringNullable("DESCEND");

        return this;
    }
}