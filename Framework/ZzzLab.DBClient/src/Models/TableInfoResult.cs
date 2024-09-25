using System;
using System.Collections.Generic;

namespace ZzzLab.Data.Models
{
    public class TableInfoResult : TableInfo, ICopyable, ICloneable
    {
        public static TableInfoResult Create(TableInfo tableInfo = null)
            => new TableInfoResult(tableInfo);

        public List<string> RefPath { set; get; } = new List<string>();
        public virtual bool IsSuccess { set; get; } = true;
        public virtual string Message { set; get; } = string.Empty;

        public TableInfoResult(TableInfo tableInfo = null)
        {
            if (tableInfo != null) Set(tableInfo);
        }

        public TableInfoResult Set(TableInfo tableInfo)
        {
            DataBaseName = tableInfo.DataBaseName;
            SchemaName = tableInfo.SchemaName;
            TableName = tableInfo.TableName;
            DBLink = tableInfo.DBLink;
            TableType = tableInfo.TableType;
            Comment = tableInfo.Comment;
            CreatedDate = tableInfo.CreatedDate;
            UpdatedDate = tableInfo.UpdatedDate;

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

            target.RefPath = this.RefPath;
            target.IsSuccess = this.IsSuccess;
            target.Message = this.Message;

            return target;
        }

        public virtual TableInfoResult CopyFrom(TableInfoResult source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            base.CopyFrom(source);

            this.RefPath = source.RefPath;
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