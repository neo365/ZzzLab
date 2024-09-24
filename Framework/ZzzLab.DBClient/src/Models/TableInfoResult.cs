using System;
using System.Collections.Generic;

namespace ZzzLab.Data.Models
{
    public class TableInfoResult : TableInfo, ICopyable, ICloneable
    {
        public static TableInfoResult Create(TableInfo tableInfo = null)
            => new TableInfoResult(tableInfo);

        public List<string> SourcePath { set; get; } = new List<string>();
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

        public override object CopyFrom(object source)
        {
            throw new NotImplementedException();
        }

        public override object CopyTo(object target)
        {
            throw new NotImplementedException();
        }

        #endregion ICopyable

        #region ICloneable

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        #endregion ICloneable
    }
}