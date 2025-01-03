using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;

namespace ZzzLab.Data.Models
{
    [DataContract]
    public class PrivilegeInfo : ICopyable, ICloneable, IDataRowSupport<PrivilegeInfo>
    {
        [Required]
        [DataMember]
        public virtual string ObjectType { get; set; }

        [Required]
        [DataMember]
        public virtual string ObjectOwner { get; set; }

        [Required]
        [DataMember]
        public virtual string ObjectName { get; set; }

        [DataMember]
        public virtual string Grantee { get; set; }

        [DataMember]
        public virtual string[] Privileges { get; set; }

        [DataMember]
        public virtual bool Grantable { get; set; }

        [DataMember]
        public virtual string GrantableStr { get => (Grantable ?  "○" : string.Empty); }

        [DataMember]
        public virtual bool HasSelect { get => (Privileges != null && Privileges.Length > 0 && this.Privileges.Contains("SELECT")); }

        [DataMember]
        public virtual string SelectStr { get => (Privileges != null && Privileges.Length > 0 && this.Privileges.Contains("SELECT") ? "○" : string.Empty); }

        [DataMember]
        public virtual bool HasInsert { get => (Privileges != null && Privileges.Length > 0 && this.Privileges.Contains("INSERT")); }

        [DataMember]
        public virtual string InsertStr { get => (Privileges != null && Privileges.Length > 0 && this.Privileges.Contains("INSERT") ? "○" : string.Empty); }

        [DataMember]
        public virtual bool HasUpdate { get => (Privileges != null && Privileges.Length > 0 && this.Privileges.Contains("UPDATE")); }

        [DataMember]
        public virtual string UpdateStr { get => (Privileges != null && Privileges.Length > 0 && this.Privileges.Contains("UPDATE") ? "○" : string.Empty); }

        [DataMember]
        public virtual bool HasDelete { get => (Privileges != null && Privileges.Length > 0 && this.Privileges.Contains("DELETE")); }


        [DataMember]
        public virtual string DeleteStr { get => (Privileges != null && Privileges.Length > 0 && this.Privileges.Contains("DELETE") ? "○" : string.Empty); }

        [DataMember]
        public virtual bool HasExecute { get => (Privileges != null && Privileges.Length > 0 && this.Privileges.Contains("EXECUTE")); }

        [DataMember]
        public virtual string ExecuteStr { get => (Privileges != null && Privileges.Length > 0 && this.Privileges.Contains("EXECUTE") ? "○" : string.Empty); }

        public PrivilegeInfo(PrivilegeInfo info = null)
        {
            if (info != null) this.Set(info);
        }

        public PrivilegeInfo Set(PrivilegeInfo info)
            => this.CopyFrom(info);

        public PrivilegeInfo Set(DataRow row)
        {
            this.ObjectType = row.ToString("OBJECT_TYPE");
            this.ObjectOwner = row.ToString("OBJECT_OWNER");
            this.ObjectName = row.ToString("OBJECT_NAME");
            this.Grantee = row.ToStringNullable("GRANTEE");
            this.Grantable = row.ToBoolean("GRANTABLE");

            string privilege = row.ToStringNullable("PRIVILEGE", throwOnError: false);

            if (string.IsNullOrWhiteSpace(privilege) == false) this.AddPrivileges(privilege);

            return this;
        }

        public PrivilegeInfo AddPrivileges(string pri)
        {
            if (string.IsNullOrWhiteSpace(pri)) throw new ArgumentNullException(nameof(pri));

            if (Privileges == null || Privileges.Length == 0) this.Privileges = pri.ToArray();
            else
            {
                List<string> list = new List<string>();

                if (this.Privileges.Contains("SELECT") || pri.EqualsIgnoreCase("SELECT")) list.Add("SELECT");
                if (this.Privileges.Contains("INSERT") || pri.EqualsIgnoreCase("INSERT")) list.Add("INSERT");
                if (this.Privileges.Contains("UPDATE") || pri.EqualsIgnoreCase("UPDATE")) list.Add("UPDATE");
                if (this.Privileges.Contains("DELETE") || pri.EqualsIgnoreCase("DELETE")) list.Add("DELETE");
                if (this.Privileges.Contains("EXECUTE") || pri.EqualsIgnoreCase("EXECUTE")) list.Add("EXECUTE");

                this.Privileges = list.ToArray();
            }
            return this;
        }

        #region Override

        public override string ToString()
           => $"[{this.ObjectType}] {this.ObjectOwner}.{this.ObjectName} => {this.Privileges.Concat()} : {this.Grantable.ToYN()}";

        #endregion Override

        #region ICopyable

        public virtual PrivilegeInfo CopyTo(PrivilegeInfo target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            target.ObjectType = this.ObjectType;
            target.ObjectOwner = this.ObjectOwner;
            target.ObjectName = this.ObjectName;
            target.Grantee = this.Grantee;
            target.Privileges = this.Privileges;
            target.Grantable = this.Grantable;

            return target;
        }

        public virtual PrivilegeInfo CopyFrom(PrivilegeInfo source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.ObjectType = source.ObjectType;
            this.ObjectOwner = source.ObjectOwner;
            this.ObjectName = source.ObjectName;
            this.Grantee = source.Grantee;
            this.Privileges = source.Privileges;
            this.Grantable = source.Grantable;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((PrivilegeInfo)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((PrivilegeInfo)source);

        #endregion ICopyable

        #region ICloneable

        public PrivilegeInfo Clone()
            => CopyTo(new PrivilegeInfo());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}