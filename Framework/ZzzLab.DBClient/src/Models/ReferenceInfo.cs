using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;

namespace ZzzLab.Data.Models
{
    public class ReferenceInfo : ICopyable, ICloneable, IDataRowSupport<ReferenceInfo>
    {
        [Required]
        [DataMember]
        public virtual string Source { get; set; }

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
        public virtual string ObjectSource { get; set; }

        [Required]
        [DataMember]
        public virtual string Status { get; set; }

        public ReferenceInfo(ReferenceInfo info = null)
        {
            if (info != null) this.Set(info);
        }

        public ReferenceInfo Set(ReferenceInfo info)
            => this.CopyFrom(info);

        #region IDataRowSupport

        public ReferenceInfo Set(DataRow row)
        {
            this.Source = row.ToString("DB_NAME");
            this.ObjectType = row.ToString("OBJECT_TYPE");
            this.ObjectOwner = row.ToString("OBJECT_OWNER");
            this.ObjectName = row.ToString("OBJECT_NAME");
            this.ObjectSource = row.ToStringNullable("OBJECT_SOURCE", throwOnError: false);
            this.Status = row.ToString("STATUS");

            return this;
        }

        #endregion IDataRowSupport

        #region Override

        public override string ToString()
           => $"[{this.ObjectType}] {this.ObjectOwner}.{this.ObjectName}";

        #endregion Override

        #region ICopyable

        public virtual ReferenceInfo CopyTo(ReferenceInfo target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            target.Source = this.Source;
            target.ObjectType = this.ObjectType;
            target.ObjectOwner = this.ObjectOwner;
            target.ObjectName = this.ObjectName;
            target.ObjectSource = this.ObjectSource;
            target.Status = this.Status;

            return target;
        }

        public virtual ReferenceInfo CopyFrom(ReferenceInfo source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.Source = source.Source;
            this.ObjectType = source.ObjectType;
            this.ObjectOwner = source.ObjectOwner;
            this.ObjectName = source.ObjectName;
            this.ObjectSource = source.ObjectSource;
            this.Status = source.Status;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((ReferenceInfo)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((ReferenceInfo)source);

        #endregion ICopyable

        #region ICloneable

        public ReferenceInfo Clone()
            => CopyTo(new ReferenceInfo());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}