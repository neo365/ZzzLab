using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;
using ZzzLab.Json;

namespace ZzzLab.Data.Models
{
    [DataContract]
    public class ConstraintInfo : ICopyable, ICloneable, IQuerySupport
    {
        [DataMember]
        public virtual string TableOwner { get; set; }

        [DataMember]
        public virtual string TableName { get; set; }

        [DataMember]
        public virtual string ConstraintOwner { get; set; }

        [Required]
        [DataMember]
        public virtual string ConstraintName { get; set; }

        [DataMember]
        public virtual string ConstraintType { get; set; }

        [DataMember]
        public virtual string ChangedDate { get; set; }

        [DataMember]
        public ConstraintColumn[] Columns { get; set; }

        [DataMember]
        public virtual string ColumnText
        {
            get
            {
                string result = string.Empty;
                if (Columns != null && Columns.Length > 0)
                {
                    foreach (ConstraintColumn column in Columns)
                    {
                        result += $", {(string.IsNullOrWhiteSpace(column.RefTableOwner) ? string.Empty : $"{column.RefTableOwner}.")}{(string.IsNullOrWhiteSpace(column.RefTableName) ? string.Empty : $"{ column.RefTableName}#")}{column.ColumnName}".Trim();
                    }
                }

                return result.TrimStart(',').Trim();
            }
        }

        public virtual ConstraintInfo Set(DataRow row)
        {
            this.TableOwner = row.ToStringNullable("TABLE_OWNER");
            this.TableName = row.ToString("TABLE_NAME");
            this.ConstraintOwner = row.ToStringNullable("CONSTRAINT_OWNER");
            this.ConstraintName = row.ToString("CONSTRAINT_NAME");
            this.ConstraintType = row.ToString("CONSTRAINT_TYPE");
            this.ChangedDate = row.ToDateTimeNullable("LAST_CHANGE")?.ToString("yyyy-MM-dd HH:mm:ss");

            return this;
        }

        #region IQuerySupport

        public virtual QueryParameterCollection GetParameters()
        {
            return new QueryParameterCollection
            {
                { "TABLE_OWNER", this.TableOwner },
                { "TABLE_NAME", this.TableName },
                { "CONSTRAINT_OWNER", this.ConstraintOwner },
                { "CONSTRAINT_NAME", this.ConstraintName },
                { "CONSTRAINT_TYPE", this.ConstraintType },
                { "COLUMNS",  (this.Columns != null && this.Columns.Length > 0 ? this.Columns?.ToJson() : null) },
                { "LAST_CHANGE", this.ChangedDate }
            };
        }

        #endregion IQuerySupport

        #region Override

        public override string ToString()
           => $"[{this.TableOwner}.{this.TableName} : {this.ConstraintType}] {this.ConstraintOwner}.{this.ConstraintName} : {this.ChangedDate}";

        #endregion Override

        #region ICopyable

        public virtual ConstraintInfo CopyTo(ConstraintInfo target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            target.TableOwner = this.TableOwner;
            target.TableName = this.TableName;
            target.ConstraintOwner = this.ConstraintOwner;
            target.ConstraintName = this.ConstraintName;
            target.ConstraintType = this.ConstraintType;
            target.ChangedDate = this.ChangedDate;

            List<ConstraintColumn> list = new List<ConstraintColumn>();

            if (this.Columns != null && this.Columns.Length > 0)
            {
                foreach (ConstraintColumn column in this.Columns)
                {
                    list.Add(column.Clone());
                }
            }

            target.Columns = list.ToArray();

            return target;
        }

        public virtual ConstraintInfo CopyFrom(ConstraintInfo source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.TableOwner = source.TableOwner;
            this.TableName = source.TableName;
            this.ConstraintOwner = source.ConstraintOwner;
            this.ConstraintName = source.ConstraintName;
            this.ConstraintType = source.ConstraintType;
            this.ChangedDate = source.ChangedDate;

            List<ConstraintColumn> list = new List<ConstraintColumn>();

            if (source.Columns != null && source.Columns.Length > 0)
            {
                foreach (ConstraintColumn column in source.Columns)
                {
                    list.Add(column.Clone());
                }
            }

            this.Columns = list.ToArray();

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((ConstraintInfo)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((ConstraintInfo)source);

        #endregion ICopyable

        #region ICloneable

        public virtual ConstraintInfo Clone()
            => CopyTo(new ConstraintInfo());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }

    public class ConstraintColumn : ICopyable, ICloneable, IQuerySupport
    {
        [DataMember]
        public virtual string TableOwner { get; set; }

        [DataMember]
        public virtual string TableName { get; set; }

        [DataMember]
        public virtual string ConstraintOwner { get; set; }

        [Required]
        [DataMember]
        public virtual string ConstraintName { get; set; }

        [DataMember]
        public virtual string ConstraintType { get; set; }

        [DataMember]
        public virtual int OrderNo { get; set; }

        [Required]
        [DataMember]
        public virtual string ColumnName { get; set; }

        [DataMember]
        public string RefTableOwner { set; get; }

        [DataMember]
        public string RefTableName { set; get; }

        [DataMember]
        public string RefColumnName { set; get; }

        public virtual ConstraintColumn Set(DataRow row)
        {
            this.TableOwner = row.ToStringNullable("TABLE_OWNER");
            this.TableName = row.ToString("TABLE_NAME");
            this.ConstraintOwner = row.ToStringNullable("CONSTRAINT_OWNER");
            this.ConstraintName = row.ToString("CONSTRAINT_NAME");
            this.ConstraintType = row.ToString("CONSTRAINT_TYPE");
            this.OrderNo = row.ToInt("ORDER_NO");
            this.ColumnName = row.ToString("COLUMN_NAME");
            this.RefTableOwner = row.ToStringNullable("R_TABLE_OWNER");
            this.RefTableName = row.ToStringNullable("R_TABLE_NAME");
            this.RefColumnName = row.ToStringNullable("R_COLUMN_NAME");

            return this;
        }

        #region IQuerySupport

        public virtual QueryParameterCollection GetParameters()
        {
            return new QueryParameterCollection
            {
                { "TABLE_OWNER", this.TableOwner },
                { "TABLE_NAME", this.TableName },
                { "CONSTRAINT_OWNER", this.ConstraintOwner },
                { "CONSTRAINT_NAME", this.ConstraintName },
                { "CONSTRAINT_TYPE", this.ConstraintType },
                { "ORDER_NO", this.OrderNo },
                { "COLUMN_NAME", this.ColumnName },
                { "R_TABLE_OWNER", this.RefTableOwner },
                { "R_TABLE_NAME", this.RefTableName },
                { "R_COLUMN_NAME", this.RefColumnName },
            };
        }

        #endregion IQuerySupport

        #region Override

        public override string ToString()
           => $"[{this.TableOwner}.{this.TableName} : {this.ConstraintType}] {this.ConstraintOwner}.{this.ConstraintName} : {this.ColumnName} => {this.RefTableOwner}{this.RefTableName} : {this.RefColumnName}";

        #endregion Override

        #region ICopyable

        public virtual ConstraintColumn CopyTo(ConstraintColumn target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            target.TableOwner = this.TableOwner;
            target.TableName = this.TableName;
            target.ConstraintOwner = this.ConstraintOwner;
            target.ConstraintName = this.ConstraintName;
            target.ConstraintType = this.ConstraintType;
            target.OrderNo = this.OrderNo;
            target.ColumnName = this.ColumnName;
            target.RefTableOwner = this.RefTableOwner;
            target.RefTableName = this.RefTableName;
            target.RefColumnName = this.RefColumnName;

            return target;
        }

        public virtual ConstraintColumn CopyFrom(ConstraintColumn source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.TableOwner = source.TableOwner;
            this.TableName = source.TableName;
            this.ConstraintOwner = source.ConstraintOwner;
            this.ConstraintName = source.ConstraintName;
            this.ConstraintType = source.ConstraintType;
            this.OrderNo = source.OrderNo;
            this.ColumnName = source.ColumnName;
            this.RefTableOwner = source.RefTableOwner;
            this.RefTableName = source.RefTableName;
            this.RefColumnName = source.RefColumnName;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((ConstraintColumn)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((ConstraintColumn)source);

        #endregion ICopyable

        #region ICloneable

        public virtual ConstraintColumn Clone()
            => CopyTo(new ConstraintColumn());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}