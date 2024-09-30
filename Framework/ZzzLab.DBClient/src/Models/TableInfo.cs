using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace ZzzLab.Data.Models
{
    public class TableInfo : ICopyable, ICloneable
    {
        public virtual string TableOwner { get; set; } = string.Empty;

        [Required]
        public virtual string TableName { get; set; } = string.Empty;

        public virtual string DBLink { set; get; }

        public virtual string FullName => $"{TableName}{(string.IsNullOrWhiteSpace(DBLink) ? string.Empty : "@" + DBLink)}";

        [Required]
        public virtual string TableType { get; set; } = string.Empty;

        public virtual string Comment { get; set; }

        public virtual string CreatedDate { get; set; }

        public virtual string UpdatedDate { get; set; }

        public virtual IEnumerable<ColumnInfo> Columns { set; get; } = Enumerable.Empty<ColumnInfo>();

        public virtual IEnumerable<IndexInfo> Indexes { set; get; } = Enumerable.Empty<IndexInfo>();

        public virtual IEnumerable<ConstraintInfo> Keys { set; get; } = Enumerable.Empty<ConstraintInfo>();

        public virtual IEnumerable<TriggerInfo> Triggers { set; get; } = Enumerable.Empty<TriggerInfo>();

        public virtual string Source { get; set; }

        public virtual TableInfo Set(DataRow row)
        {
            this.TableOwner = row.ToString("TABLE_OWNER");
            this.TableName = row.ToString("TABLE_NAME");
            this.DBLink = row.ToStringNullable("DBLINK")?.TrimStart('@');
            this.TableType = row.ToString("TABLE_TYPE");

            this.Comment = row.ToStringNullable("COMMENTS");
            this.CreatedDate = row.ToDateTimeNullable("WHEN_CREATED")?.ToString("yyyy-MM-dd HH:mm:ss");
            this.UpdatedDate = row.ToDateTimeNullable("WHEN_UPDATED")?.ToString("yyyy-MM-dd HH:mm:ss");

            return this;
        }

        public override string ToString()
            => $"{TableOwner}.{FullName} : {Comment}";

        #region ICopyable

        public virtual TableInfo CopyTo(TableInfo target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            target.TableOwner = this.TableOwner;
            target.TableName = this.TableName;
            target.DBLink = this.DBLink;
            target.TableType = this.TableType;

            target.Comment = this.Comment;
            target.CreatedDate = this.CreatedDate;
            target.UpdatedDate = this.UpdatedDate;

            target.Source = this.Source;

            return target;
        }

        public virtual TableInfo CopyFrom(TableInfo source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.TableOwner = source.TableOwner;
            this.TableName = source.TableName;
            this.DBLink = source.DBLink;
            this.TableType = source.TableType;

            this.Comment = source.Comment;
            this.CreatedDate = source.CreatedDate;
            this.UpdatedDate = source.UpdatedDate;

            this.Source = source.Source;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((TableInfo)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((TableInfo)source);

        #endregion ICopyable

        #region ICloneable

        public virtual TableInfo Clone()
            => CopyTo(new TableInfo());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}