using System;
using System.Data;
using System.Runtime.Serialization;

namespace ZzzLab.Data.Models
{
    [DataContract]
    public class TableInfoResult : TableInfo, ICopyable, ICloneable
    {
        public static TableInfoResult Create(TableInfo tableInfo = null)
            => new TableInfoResult(tableInfo);

        [DataMember]
        public virtual bool IsSuccess { set; get; } = true;

        [DataMember]
        public virtual string Message { set; get; } = string.Empty;

        public TableInfoResult(TableInfo tableInfo = null)
        {
            if (tableInfo != null) this.Set(tableInfo);
        }

        public new TableInfoResult Set(TableInfo tableInfo)
        {
            base.CopyFrom(tableInfo);

            return this;
        }

        public new TableInfoResult Set(DataRow row)
        {
            base.Set(row);

            return this;
        }

        public TableInfoResult SetResult(string message)
        {
            IsSuccess = false;
            Message = message;
            return this;
        }

        #region ICopyable

        public virtual TableInfoResult CopyTo(TableInfoResult target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            base.CopyTo(target);

            target.IsSuccess = this.IsSuccess;
            target.Message = this.Message;

            return target;
        }

        public virtual TableInfoResult CopyFrom(TableInfoResult source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            base.CopyFrom(source);

            this.IsSuccess = source.IsSuccess;
            this.Message = source.Message;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((TableInfoResult)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((TableInfoResult)source);

        #endregion ICopyable

        #region ICloneable

        public new TableInfoResult Clone()
            => CopyTo(new TableInfoResult());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}