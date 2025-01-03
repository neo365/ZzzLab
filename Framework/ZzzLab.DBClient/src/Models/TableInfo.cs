using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;
using ZzzLab.Json;

namespace ZzzLab.Data.Models
{
    [DataContract]
    public class TableInfo : DataBaseInfo, ICopyable, ICloneable, IDataRowSupport<TableInfo>, IQuerySupport
    {
        // DataBase 종류
        [DataMember]
        public virtual DataBaseType ServerType { get; set; }

        [DataMember]
        public virtual string TableOwner { get; set; } = string.Empty;

        [Required]
        [DataMember]
        public virtual string TableName { get; set; } = string.Empty;

        [DataMember]
        public virtual string TableSpaceName { get; set; }

        [DataMember]
        public virtual string Comment { get; set; }

        [DataMember]
        public virtual long UsedSize { get; set; } = 0L;

        [DataMember]
        public virtual long Count { set; get; } = -1;

        [Required]
        [DataMember]
        public virtual TableColumnInfo[] Columns { set; get; } = null;

        [DataMember]
        public virtual IndexInfo[] Indexes { set; get; } = null;

        [DataMember]
        public virtual ConstraintInfo[] Keys { set; get; } = null;

        [DataMember]
        public virtual TriggerInfo[] Triggers { set; get; } = null;

        [DataMember]
        public virtual PrivilegeInfo[] Privileges { get; set; }

        [DataMember]
        public virtual ReferenceInfo[] References { get; set; }

        public virtual string FullName
            => $"{(string.IsNullOrWhiteSpace(TableOwner) ? string.Empty : $"{TableOwner}.")}{TableName}{(string.IsNullOrWhiteSpace(DbLink) ? string.Empty : "@" + DbLink)}";

        public virtual string ColumnText
        {
            get
            {
                string result = string.Empty;
                if (Columns != null && Columns.Length > 0)
                {
                    foreach (TableColumnInfo column in Columns)
                    {
                        result += $", {column.ColumnName}";
                    }
                }

                return result.TrimStart(',').Trim();
            }
        }

        public TableInfo(TableInfo info = null)
        {
            if (info != null) this.Set(info);
        }

        public TableInfo Set(TableInfo info)
            => this.CopyFrom(info);

        #region IDataRowSupport

        public new TableInfo Set(DataRow row)
        {
            base.Set(row);

            this.TableOwner = row.ToString("TABLE_OWNER");
            this.TableName = row.ToString("TABLE_NAME");
            this.TableSpaceName = row.ToStringNullable("TABLESPACE_NAME", throwOnError: false);
            this.Comment = row.ToStringNullable("COMMENTS", throwOnError: false);

            if (row.Table.Columns.Contains("USED_SIZE")) this.UsedSize = row.ToLongNullable("USED_SIZE", throwOnError: false) ?? 0L;
            if (row.Table.Columns.Contains("KEYS")) this.Keys = row.FromJsonNullable<ConstraintInfo[]>("KEYS", throwOnError: false);
            if (row.Table.Columns.Contains("COLUMNS")) this.Columns = row.FromJsonNullable<TableColumnInfo[]>("COLUMNS", throwOnError: false);
            if (row.Table.Columns.Contains("PRIVILEGES")) this.Privileges = row.FromJsonNullable<PrivilegeInfo[]>("PRIVILEGES", throwOnError: false);
            if (row.Table.Columns.Contains("NUM_ROWS")) this.Count = row.ToLongNullable("NUM_ROWS", throwOnError: false) ?? 0L;

            return this;
        }

        #endregion IDataRowSupport

        #region IQuerySupport

        public new QueryParameterCollection GetParameters()
        {
            QueryParameterCollection parameters = base.GetParameters();

            parameters.Set("TABLE_OWNER", this.TableOwner);
            parameters.Set("TABLE_NAME", this.TableName);
            parameters.Set("TABLESPACE_NAME", this.TableSpaceName);
            parameters.Set("USED_SIZE", this.UsedSize);
            parameters.Set("NUM_ROWS", this.Count);
            parameters.Set("COMMENTS", this.Comment);
            parameters.Set("COLUMNS", (this.Columns != null && this.Columns.Length > 0 ? this.Columns?.ToJson() : null));
            parameters.Set("KEYS", (this.Keys != null && this.Keys.Length > 0 ? this.Keys?.ToJson() : null));
            parameters.Set("PRIVILEGES", (this.Privileges != null && this.Privileges.Length > 0 ? this.Privileges?.ToJson() : null));

            return parameters;
        }

        #endregion IQuerySupport

        #region Override

        public override string ToString()
            => $"{FullName} : {Comment}";

        #endregion Override

        #region ICopyable

        public virtual TableInfo CopyTo(TableInfo target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            base.CopyTo(target);

            target.TableOwner = this.TableOwner;
            target.TableName = this.TableName;
            target.TableSpaceName = this.TableSpaceName;
            target.UsedSize = this.UsedSize;
            target.Count = this.Count;
            target.Comment = this.Comment;

            List<TableColumnInfo> Columnlist = new List<TableColumnInfo>();

            if (this.Columns != null && this.Columns.Length > 0)
            {
                foreach (TableColumnInfo item in this.Columns)
                {
                    Columnlist.Add(item.Clone());
                }
            }

            target.Columns = Columnlist.ToArray();

            List<ConstraintInfo> keyList = new List<ConstraintInfo>();

            if (this.Keys != null && this.Keys.Length > 0)
            {
                foreach (ConstraintInfo item in this.Keys)
                {
                    keyList.Add(item.Clone());
                }
            }

            target.Keys = keyList.ToArray();

            List<IndexInfo> indexlist = new List<IndexInfo>();

            if (this.Indexes != null && this.Indexes.Length > 0)
            {
                foreach (IndexInfo item in this.Indexes)
                {
                    indexlist.Add(item.Clone());
                }
            }

            target.Indexes = indexlist.ToArray();

            List<TriggerInfo> triggerlist = new List<TriggerInfo>();

            if (this.Triggers != null && this.Triggers.Length > 0)
            {
                foreach (TriggerInfo item in this.Triggers)
                {
                    triggerlist.Add(item.Clone());
                }
            }

            target.Triggers = triggerlist.ToArray();

            List<PrivilegeInfo> privilegelist = new List<PrivilegeInfo>();

            if (this.Privileges != null && this.Privileges.Length > 0)
            {
                foreach (PrivilegeInfo item in this.Privileges)
                {
                    privilegelist.Add(item.Clone());
                }
            }

            target.Privileges = privilegelist.ToArray();

            List<ReferenceInfo> ReferenceList = new List<ReferenceInfo>();

            if (this.Privileges != null && this.Privileges.Length > 0)
            {
                foreach (ReferenceInfo item in this.References)
                {
                    ReferenceList.Add(item.Clone());
                }
            }

            target.References = ReferenceList.ToArray();

            return target;
        }

        public virtual TableInfo CopyFrom(TableInfo source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            base.CopyFrom(source);

            this.TableOwner = source.TableOwner;
            this.TableName = source.TableName;
            this.TableSpaceName = source.TableSpaceName;
            this.UsedSize = source.UsedSize;
            this.Count = source.Count;
            this.Comment = source.Comment;

            List<TableColumnInfo> Columnlist = new List<TableColumnInfo>();

            if (source.Columns != null && source.Columns.Length > 0)
            {
                foreach (TableColumnInfo item in source.Columns)
                {
                    Columnlist.Add(item.Clone());
                }
            }

            this.Columns = Columnlist.ToArray();

            List<ConstraintInfo> keyList = new List<ConstraintInfo>();

            if (source.Keys != null && source.Keys.Length > 0)
            {
                foreach (ConstraintInfo item in source.Keys)
                {
                    keyList.Add(item.Clone());
                }
            }

            this.Keys = keyList.ToArray();

            List<IndexInfo> indexlist = new List<IndexInfo>();

            if (source.Indexes != null && source.Indexes.Length > 0)
            {
                foreach (IndexInfo item in source.Indexes)
                {
                    indexlist.Add(item.Clone());
                }
            }

            this.Indexes = indexlist.ToArray();

            List<TriggerInfo> triggerlist = new List<TriggerInfo>();

            if (source.Triggers != null && source.Triggers.Length > 0)
            {
                foreach (TriggerInfo item in source.Triggers)
                {
                    triggerlist.Add(item.Clone());
                }
            }

            this.Triggers = triggerlist.ToArray();

            List<PrivilegeInfo> privilegelist = new List<PrivilegeInfo>();

            if (source.Privileges != null && source.Privileges.Length > 0)
            {
                foreach (PrivilegeInfo item in source.Privileges)
                {
                    privilegelist.Add(item.Clone());
                }
            }

            this.Privileges = privilegelist.ToArray();

            List<ReferenceInfo> ReferenceList = new List<ReferenceInfo>();

            if (source.Privileges != null && source.Privileges.Length > 0)
            {
                foreach (ReferenceInfo item in source.References)
                {
                    ReferenceList.Add(item.Clone());
                }
            }

            this.References = ReferenceList.ToArray();

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((TableInfo)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((TableInfo)source);

        #endregion ICopyable

        #region ICloneable

        public new TableInfo Clone()
            => CopyTo(new TableInfo());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}