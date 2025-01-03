using System;
using System.Data;
using System.Runtime.Serialization;

namespace ZzzLab.Data.Models
{
    [DataContract]
    public class DataBaseInfo : ICopyable, ICloneable, IDataRowSupport<DataBaseInfo>, IQuerySupport
    {
        [DataMember]
        public virtual string Source { get; set; }

        [DataMember]
        public virtual string DbLink { get; set; }

        [DataMember]
        public virtual string Status { set; get; }

        [DataMember]
        public virtual string CreatedDate { get; set; }

        [DataMember]
        public virtual string UpdatedDate { get; set; }

        [DataMember]
        public virtual string SyncedDate { get; set; }

        [DataMember]
        public virtual string Remark { get; set; }

        [DataMember]
        public virtual bool IsUsed { set; get; }

        public DataBaseInfo(DataBaseInfo info = null)
        {
            if (info != null) this.Set(info);
        }

        public DataBaseInfo Set(DataBaseInfo info)
            => this.CopyFrom(info);

        #region IDataRowSupport

        public virtual DataBaseInfo Set(DataRow row)
        {
            this.Source = row.ToStringNullable("DB_NAME", throwOnError: false);
            this.DbLink = row.ToStringNullable("DB_LINK", throwOnError: false)?.TrimStart('@');
            if (row.Table.Columns.Contains("STATUS")) this.Status = row.ToStringNullable("STATUS", throwOnError: false) ?? "VALID";
            if (row.Table.Columns.Contains("CREATED_DT")) this.CreatedDate = row.ToDateTimeNullable("CREATED_DT", throwOnError: false)?.ToString("yyyy-MM-dd HH:mm:ss");
            if (row.Table.Columns.Contains("UPDATED_DT")) this.UpdatedDate = row.ToDateTimeNullable("UPDATED_DT", throwOnError: false)?.ToString("yyyy-MM-dd HH:mm:ss");
            this.SyncedDate = row.ToDateTimeNullable("SYNCED_DT", throwOnError: false)?.ToString("yyyy-MM-dd HH:mm:ss");
            if (row.Table.Columns.Contains("REMARK")) this.Remark = row.ToStringNullable("REMARK", throwOnError: false);
            if (row.Table.Columns.Contains("USED_YN")) this.IsUsed = row.ToBooleanNullable("USED_YN", throwOnError: false) ?? true;

            return this;
        }

        #endregion IDataRowSupport

        #region IQuerySupport

        public virtual QueryParameterCollection GetParameters()
        {
            return new QueryParameterCollection
            {
                { "DB_NAME", this.Source },
                { "DB_LINK", this.DbLink },
                { "STATUS", this.Status },
                { "CREATED_DT", this.CreatedDate?.ToDateTimeNullable() },
                { "UPDATED_DT", this.UpdatedDate?.ToDateTimeNullable() },
                { "SYNCED_DT", this.SyncedDate?.ToDateTimeNullable() },
                { "REMARK", this.Remark },
                { "USED_YN", this.IsUsed.ToYN() }
            };
        }

        #endregion IQuerySupport

        #region ICopyable

        public virtual DataBaseInfo CopyTo(DataBaseInfo target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            target.Source = this.Source;
            target.DbLink = this.DbLink;

            target.Status = this.Status;
            target.CreatedDate = this.CreatedDate;
            target.UpdatedDate = this.UpdatedDate;
            target.SyncedDate = this.SyncedDate;
            target.Remark = this.Remark;
            target.IsUsed = this.IsUsed;

            return target;
        }

        public virtual DataBaseInfo CopyFrom(DataBaseInfo source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.Source = source.Source;
            this.DbLink = source.DbLink;

            this.Status = source.Status;
            this.CreatedDate = source.CreatedDate;
            this.UpdatedDate = source.UpdatedDate;
            this.SyncedDate = source.SyncedDate;
            this.Remark = source.Remark;
            this.IsUsed = source.IsUsed;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((DataBaseInfo)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((DataBaseInfo)source);

        #endregion ICopyable

        #region ICloneable

        public virtual DataBaseInfo Clone()
            => CopyTo(new DataBaseInfo());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}