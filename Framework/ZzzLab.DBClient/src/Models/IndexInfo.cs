using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using ZzzLab;
using ZzzLab.Json;

namespace ZzzLab.Data.Models
{
    [DataContract]
    public class IndexInfo : DataBaseInfo, ICopyable, ICloneable, IQuerySupport
    {
        [DataMember]
        public virtual string TableOwner { get; set; }

        [Required]
        [DataMember]
        public virtual string TableName { get; set; }

        [DataMember]
        public virtual string IndexOwner { get; set; }

        [Required]
        [DataMember]
        public virtual string IndexName { get; set; }

        [DataMember]
        public virtual string IndexType { get; set; }

        [DataMember]
        public virtual string Uniqueness { get; set; }

        [DataMember]
        public virtual IndexColumn[] Columns { get; set; }

        [DataMember]
        public virtual string ColumnText
        {
            get
            {
                string result = string.Empty;
                if (Columns != null && Columns.Any())
                {
                    foreach (IndexColumn column in Columns)
                    {
                        result += $", {column.ColumnName}".Trim();
                    }
                }

                return result.TrimStart(',').Trim();
            }
        }

        [DataMember]
        public virtual string ColumnTextWithDescend
        {
            get
            {
                string result = string.Empty;
                if (Columns != null && Columns.Any())
                {
                    foreach (IndexColumn column in Columns)
                    {
                        result += $", {column.ColumnName} {(string.IsNullOrWhiteSpace(column.Descend) ? string.Empty : column.Descend)}".Trim();
                    }
                }

                return result.TrimStart(',').Trim();
            }
        }

        public new IndexInfo Set(DataRow row)
        {
            base.Set(row);

            this.TableOwner = row.ToStringNullable("TABLE_OWNER");
            this.TableName = row.ToString("TABLE_NAME");
            this.IndexOwner = row.ToStringNullable("INDEX_OWNER");
            this.IndexName = row.ToString("INDEX_NAME");
            this.IndexType = row.ToStringNullable("INDEX_TYPE");
            this.Uniqueness = row.ToStringNullable("UNIQUENESS");

            if (row.Table.Columns.Contains("COLUMNS")) this.Columns = row.FromJsonNullable<IndexColumn[]>("COLUMNS", throwOnError: false);

            return this;
        }

        #region IQuerySupport

        public new QueryParameterCollection GetParameters()
        {
            QueryParameterCollection parameters = base.GetParameters();

            parameters.Set("DB_NAME", this.Source);
            parameters.Set("TABLE_OWNER", this.TableOwner);
            parameters.Set("TABLE_NAME", this.TableName);
            parameters.Set("INDEX_OWNER", this.IndexOwner);
            parameters.Set("INDEX_NAME", this.IndexName);
            parameters.Set("INDEX_TYPE", this.IndexType);
            parameters.Set("UNIQUENESS", this.Uniqueness);
            parameters.Set("COLUMNS", (this.Columns != null && this.Columns.Length > 0 ? this.Columns?.ToJson() : null));

            return parameters;
        }

        #endregion IQuerySupport

        #region Override

        public override string ToString()
           => $"[{this.TableOwner}.{this.TableName}] {this.IndexOwner}.{this.IndexName} ({this.IndexType} : {this.Uniqueness}) => {this.ColumnTextWithDescend}";

        #endregion Override

        #region ICopyable

        public virtual IndexInfo CopyTo(IndexInfo target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            target.TableOwner = this.TableOwner;
            target.TableName = this.TableName;
            target.IndexOwner = this.IndexOwner;
            target.IndexName = this.IndexName;
            target.IndexType = this.IndexType;
            target.Uniqueness = this.Uniqueness;

            List<IndexColumn> list = new List<IndexColumn>();

            if (this.Columns != null && this.Columns.Length > 0)
            {
                foreach (IndexColumn column in this.Columns)
                {
                    list.Add(column.Clone());
                }
            }

            target.Columns = list.ToArray();

            return target;
        }

        public virtual IndexInfo CopyFrom(IndexInfo source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.TableOwner = source.TableOwner;
            this.TableName = source.TableName;
            this.IndexOwner = source.IndexOwner;
            this.IndexName = source.IndexName;
            this.IndexType = source.IndexType;
            this.Uniqueness = source.Uniqueness;

            List<IndexColumn> list = new List<IndexColumn>();

            if (source.Columns != null && source.Columns.Length > 0)
            {
                foreach (IndexColumn column in source.Columns)
                {
                    list.Add(column.Clone());
                }
            }

            this.Columns = list.ToArray();

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((IndexInfo)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((IndexInfo)source);

        #endregion ICopyable

        #region ICloneable

        public new IndexInfo Clone()
            => CopyTo(new IndexInfo());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}

[DataContract]
public class IndexColumn : ICopyable, ICloneable
{
    [DataMember]
    protected virtual string TableOwner { get; set; }

    [DataMember]
    protected virtual string TableName { get; set; }

    [DataMember]
    protected virtual string IndexOwner { get; set; }

    [DataMember]
    protected virtual string IndexName { get; set; }

    [DataMember]
    public virtual int OrderNo { get; set; }

    [Required]
    [DataMember]
    public virtual string ColumnName { get; set; }

    public string _ColumnNameRef = string.Empty;

    [DataMember]
    public virtual string ColumnNameRef
    {
        get => string.IsNullOrWhiteSpace(_ColumnNameRef) ? ColumnName : _ColumnNameRef;
        set => _ColumnNameRef = value;
    }

    [DataMember]
    public virtual string Descend { get; set; } = "NONE";

    public virtual IndexColumn Set(DataRow row)
    {
        this.TableOwner = row.ToStringNullable("TABLE_OWNER");
        this.TableName = row.ToStringNullable("TABLE_NAME");
        this.IndexOwner = row.ToStringNullable("INDEX_OWNER");
        this.IndexName = row.ToString("INDEX_NAME");
        this.OrderNo = row.ToInt("ORDER_NO");
        this.ColumnName = row.ToString("COLUMN_NAME");
        this.ColumnNameRef = row.ToStringNullable("COLUMN_NAME_REF");
        this.Descend = row.ToStringNullable("DESCEND");

        return this;
    }

    #region Override

    public override string ToString()
       => $"{this.ColumnNameRef} {this.Descend}";

    #endregion Override

    #region ICopyable

    public virtual IndexColumn CopyTo(IndexColumn target)
    {
        if (target == null) throw new ArgumentNullException(nameof(target));

        target.TableOwner = this.TableOwner;
        target.TableName = this.TableOwner;
        target.IndexOwner = this.IndexOwner;
        target.IndexName = this.IndexName;
        target.OrderNo = this.OrderNo;
        target.ColumnName = this.ColumnName;
        target.ColumnNameRef = this.ColumnNameRef;
        target.Descend = this.Descend;

        return target;
    }

    public virtual IndexColumn CopyFrom(IndexColumn source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        this.TableOwner = source.TableOwner;
        this.TableName = source.TableOwner;
        this.IndexOwner = source.IndexOwner;
        this.IndexName = source.IndexName;
        this.OrderNo = source.OrderNo;
        this.ColumnName = source.ColumnName;
        this.ColumnNameRef = source.ColumnNameRef;
        this.Descend = source.Descend;

        return this;
    }

    object ICopyable.CopyTo(object target)
        => this.CopyTo((IndexColumn)target);

    object ICopyable.CopyFrom(object source)
        => this.CopyFrom((IndexColumn)source);

    #endregion ICopyable

    #region ICloneable

    public virtual IndexColumn Clone()
        => CopyTo(new IndexColumn());

    object ICloneable.Clone()
        => Clone();

    #endregion ICloneable
}