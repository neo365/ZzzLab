using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using ZzzLab.Json;

namespace ZzzLab.Data.Models
{
    [DataContract]
    public class SourceInfo : DataBaseInfo, ICopyable, ICloneable
    {
        [DataMember]
        public virtual string SourceType { set; get; }

        [DataMember]
        public virtual string SourceOwner { set; get; }

        [DataMember]
        public virtual string SourceName { set; get; }

        [DataMember]
        public virtual string Header { set; get; }

        [DataMember]
        public virtual int HeaderLineNum { set; get; }

        [DataMember]
        public virtual string Body { set; get; }

        [DataMember]
        public virtual int BodyLineNum { set; get; }

        [DataMember]
        public virtual PrivilegeInfo[] Privileges { get; set; } = null;

        [DataMember]
        public virtual ReferenceInfo[] References { get; set; }

        public SourceInfo(SourceInfo info = null) : base(info)
        {
            if (info != null) this.Set(info);
        }

        public SourceInfo Set(SourceInfo info)
            => this.CopyFrom(info);

        public new SourceInfo Set(DataRow row)
        {
            base.Set(row);

            this.SourceType = row.ToString("SOURCE_TYPE");
            this.SourceOwner = row.ToString("SOURCE_OWNER");
            this.SourceName = row.ToString("SOURCE_NAME");
            this.Header = row.ToStringNullable("SOURCE_HEADER", throwOnError: false);
            this.Body = row.ToStringNullable("SOURCE_BODY", throwOnError: false);
            this.BodyLineNum = row.ToIntNullable("SOURCE_BODY_LINENUM", throwOnError: false) ?? 0;

            if (row.Table.Columns.Contains("PRIVILEGES")) this.Privileges = row.FromJsonNullable<PrivilegeInfo[]>("PRIVILEGES", throwOnError: false);

            return this;
        }

        #region IQuerySupport

        public new QueryParameterCollection GetParameters()
        {
            QueryParameterCollection parameters = base.GetParameters();

            parameters.Set("SOURCE_TYPE", this.SourceType);
            parameters.Set("SOURCE_OWNER", this.SourceOwner);
            parameters.Set("SOURCE_NAME", this.SourceName);
            parameters.Set("SOURCE_HEADER", this.Header);
            parameters.Set("SOURCE_HEADER_LINENUM", this.HeaderLineNum);
            parameters.Set("SOURCE_BODY", this.Body);
            parameters.Set("SOURCE_BODY_LINENUM", this.BodyLineNum);
            parameters.Set("PRIVILEGES", (this.Privileges != null && this.Privileges.Length > 0 ? this.Privileges?.ToJson() : null));

            return parameters;
        }

        #endregion IQuerySupport

        #region Override

        public override string ToString()
           => $"[{SourceType}] {SourceOwner}.{SourceName} : {BodyLineNum}";

        #endregion Override

        #region ICopyable

        public virtual SourceInfo CopyTo(SourceInfo target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            base.CopyTo(target);

            target.SourceOwner = this.SourceOwner;
            target.SourceName = this.SourceName;
            target.SourceType = this.SourceType;
            target.Header = this.Header;
            target.HeaderLineNum = this.HeaderLineNum;
            target.Body = this.Body;
            target.BodyLineNum += this.BodyLineNum;

            List<PrivilegeInfo> privilegelist = new List<PrivilegeInfo>();

            if (this.Privileges != null && this.Privileges.Length > 0)
            {
                foreach (PrivilegeInfo item in this.Privileges)
                {
                    privilegelist.Add(item.Clone());
                }
            }

            target.Privileges = privilegelist.ToArray();

            List<ReferenceInfo> Referenceslist = new List<ReferenceInfo>();

            if (this.Privileges != null && this.Privileges.Length > 0)
            {
                foreach (ReferenceInfo item in this.References)
                {
                    Referenceslist.Add(item.Clone());
                }
            }

            target.References = Referenceslist.ToArray();

            return target;
        }

        public virtual SourceInfo CopyFrom(SourceInfo source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            base.CopyFrom(source);

            this.SourceOwner = source.SourceOwner;
            this.SourceName = source.SourceName;
            this.SourceType = source.SourceType;

            this.Header = source.Header;
            this.HeaderLineNum = source.HeaderLineNum;
            this.Body = source.Body;
            this.BodyLineNum = source.BodyLineNum;

            List<PrivilegeInfo> privilegelist = new List<PrivilegeInfo>();

            if (source.Privileges != null && source.Privileges.Length > 0)
            {
                foreach (PrivilegeInfo item in source.Privileges)
                {
                    privilegelist.Add(item.Clone());
                }
            }

            this.Privileges = privilegelist.ToArray();


            List<ReferenceInfo> Referenceslist = new List<ReferenceInfo>();

            if (source.Privileges != null && source.Privileges.Length > 0)
            {
                foreach (ReferenceInfo item in source.References)
                {
                    Referenceslist.Add(item.Clone());
                }
            }

            this.References = Referenceslist.ToArray();

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((SourceInfo)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((SourceInfo)source);

        #endregion ICopyable

        #region ICloneable

        public new SourceInfo Clone()
            => CopyTo(new SourceInfo());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}