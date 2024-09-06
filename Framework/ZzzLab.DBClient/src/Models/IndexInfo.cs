using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using ZzzLab;

namespace ZzzLab.Data.Models
{
    public class IndexInfo
    {
        public virtual string Owner { get; set; }

        [Required]
        public virtual string TableName { get; set; }

        [Required]
        public virtual string IndexName { get; set; }

        public virtual string IndexType { get; set; }

        public virtual string Uniqueness { get; set; }

        public virtual IEnumerable<IndexColumn> Columns { get; set; }

        public virtual string ColumnNames {
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
            this.Owner = row.ToString("OWNER");
            this.IndexName = row.ToString("INDEX_NAME");
            this.IndexType = row.ToString("INDEX_TYPE");
            this.TableName = row.ToString("TABLE_NAME");
            this.Uniqueness = row.ToString("UNIQUENESS");

            return this;
        }
    }
}

public class IndexColumn
{
    protected virtual string Owner { get; set; }
    protected virtual string TableName { get; set; }
    protected virtual string IndexName { get; set; }

    public virtual int OrderNo { get; set; }

    [Required]
    public virtual string ColumnName { get; set; }

    [Required]
    public virtual string ColumnNameRef { get; set; }

    public virtual string Descend { get; set; } = "ASC";

    public virtual IndexColumn Set(DataRow row)
    {
        this.Owner = row.ToString("OWNER");
        this.TableName = row.ToString("TABLE_NAME");
        this.IndexName = row.ToString("INDEX_NAME");
        this.OrderNo = row.ToInt("ORDER_NO");
        this.ColumnName = row.ToString("COLUMN_NAME");
        this.ColumnNameRef = row.ToString("COLUMN_NAME_REF");
        this.Descend = row.ToString("DESCEND");

        return this;
    }
}