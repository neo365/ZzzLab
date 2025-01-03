using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;
using ZzzLab.Json;

namespace ZzzLab.Data.Models
{
    [DataContract]
    public class ViewInfo : DataBaseInfo, ICopyable, ICloneable, IDataRowSupport<ViewInfo>, IQuerySupport
    {
        // DataBase 종류
        [DataMember]
        public virtual DataBaseType ServerType { get; set; }

        [DataMember]
        public virtual string ViewOwner { get; set; } = string.Empty;

        [Required]
        [DataMember]
        public virtual string ViewName { get; set; } = string.Empty;

        [DataMember]
        public virtual string ViewSource { get; set; } = string.Empty;

        [DataMember]
        public virtual string Comment { get; set; }

        [Required]
        [DataMember]
        public virtual TableColumnInfo[] Columns { set; get; } = null;

        [DataMember]
        public virtual PrivilegeInfo[] Privileges { get; set; } = null;

        [DataMember]
        public virtual ReferenceInfo[] References { get; set; }

        [DataMember]
        public virtual string FullName => $"{(string.IsNullOrWhiteSpace(ViewOwner) ? string.Empty : $"{ViewOwner}.")}{ViewName}{(string.IsNullOrWhiteSpace(DbLink) ? string.Empty : "@" + DbLink)}";

        [DataMember]
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

        public ViewInfo(ViewInfo info = null)
        {
            if (info != null) this.Set(info);
        }

        public ViewInfo Set(ViewInfo info)
            => this.CopyFrom(info);

        #region IDataRowSupport

        public new ViewInfo Set(DataRow row)
        {
            base.Set(row);

            this.ViewOwner = row.ToString("VIEW_OWNER");
            this.ViewName = row.ToString("VIEW_NAME");
            this.ViewSource = row.ToString("VIEW_SOURCE");
            this.Comment = row.ToStringNullable("COMMENTS", throwOnError: false);
            if (row.Table.Columns.Contains("COLUMNS")) this.Columns = row.FromJsonNullable<TableColumnInfo[]>("COLUMNS", throwOnError: false);
            if (row.Table.Columns.Contains("PRIVILEGES")) this.Privileges = row.FromJsonNullable<PrivilegeInfo[]>("PRIVILEGES", throwOnError: false);

            return this;
        }

        #endregion IDataRowSupport

        #region IQuerySupport

        public new QueryParameterCollection GetParameters()
        {
            QueryParameterCollection parameters = base.GetParameters();

            parameters.Set("VIEW_OWNER", this.ViewOwner);
            parameters.Set("VIEW_NAME", this.ViewName);
            parameters.Set("VIEW_SOURCE", this.ViewSource);
            parameters.Set("COMMENTS", this.Comment);
            parameters.Set("COLUMNS", (this.Columns != null && this.Columns.Length > 0 ? this.Columns?.ToJson() : null));
            parameters.Set("PRIVILEGES", (this.Privileges != null && this.Privileges.Length > 0 ? this.Privileges?.ToJson() : null));

            return parameters;
        }

        #endregion IQuerySupport

        #region Override

        public override string ToString()
            => $"{FullName} : {Comment}";

        #endregion Override

        #region ICopyable

        public virtual ViewInfo CopyTo(ViewInfo target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            base.CopyTo(target);

            target.ViewOwner = this.ViewOwner;
            target.ViewName = this.ViewName;
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

            List<PrivilegeInfo> privilegelist = new List<PrivilegeInfo>();

            if (this.Privileges != null && this.Privileges.Length > 0)
            {
                foreach (PrivilegeInfo item in this.Privileges)
                {
                    privilegelist.Add(item.Clone());
                }
            }

            target.Privileges = privilegelist.ToArray();

            List<ReferenceInfo> referencelist = new List<ReferenceInfo>();

            if (this.References != null && this.References.Length > 0)
            {
                foreach (ReferenceInfo item in this.References)
                {
                    referencelist.Add(item.Clone());
                }
            }

            target.References = referencelist.ToArray();

            return target;
        }

        public virtual ViewInfo CopyFrom(ViewInfo source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            base.CopyFrom(source);

            this.ViewOwner = source.ViewOwner;
            this.ViewName = source.ViewName;
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

            List<PrivilegeInfo> privilegelist = new List<PrivilegeInfo>();

            if (source.Privileges != null && source.Privileges.Length > 0)
            {
                foreach (PrivilegeInfo item in source.Privileges)
                {
                    privilegelist.Add(item.Clone());
                }
            }

            this.Privileges = privilegelist.ToArray();

            List<ReferenceInfo> referencelist = new List<ReferenceInfo>();

            if (source.References != null && source.References.Length > 0)
            {
                foreach (ReferenceInfo item in source.References)
                {
                    referencelist.Add(item.Clone());
                }
            }

            this.References = referencelist.ToArray();

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((ViewInfo)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((ViewInfo)source);

        #endregion ICopyable

        #region ICloneable

        public new ViewInfo Clone()
            => CopyTo(new ViewInfo());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}