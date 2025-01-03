using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;

namespace ZzzLab.Data.Models
{
    [DataContract]
    public class ObjectInfo : DataBaseInfo, ICopyable, ICloneable, IQuerySupport
    {
        [Required]
        [DataMember]
        public virtual string ObjectOwner { get; set; }

        [Required]
        [DataMember]
        public virtual string ObjectName { get; set; }

        [Required]
        [DataMember]
        public virtual string ObjectType { get; set; }

        [DataMember]
        public virtual string TableOwner { get; set; }

        [DataMember]
        public virtual string TableName { get; set; }

        public ObjectInfo(ObjectInfo info = null) : base(info)
        {
            if (info != null) this.Set(info);
        }

        public ObjectInfo Set(ObjectInfo info)
            => this.CopyFrom(info);

        public new ObjectInfo Set(DataRow row)
        {
            base.Set(row);

            this.ObjectOwner = row.ToStringNullable("OBJECT_OWNER");
            this.ObjectName = row.ToString("OBJECT_NAME");
            this.ObjectType = row.ToString("OBJECT_TYPE");

            this.TableOwner = row.ToStringNullable("TABLE_OWNER", throwOnError: false);
            this.TableName = row.ToStringNullable("TABLE_NAME", throwOnError: false);

            return this;
        }

        #region IQuerySupport

        public new QueryParameterCollection GetParameters()
        {
            QueryParameterCollection parameters = base.GetParameters();

            parameters.Set("OBJECT_TYPE", this.ObjectType);
            parameters.Set("OBJECT_OWNER", this.ObjectOwner);
            parameters.Set("OBJECT_NAME", this.ObjectName);
            parameters.Set("TABLE_OWNER", this.TableOwner);
            parameters.Set("TABLE_NAME", this.TableName);

            return parameters;
        }

        #endregion IQuerySupport

        #region Override

        public override string ToString()
           => $"[{ObjectType}] {ObjectOwner}.{ObjectName} : {this.UpdatedDate ?? this.CreatedDate}";

        #endregion Override

        #region ICopyable

        public virtual ObjectInfo CopyTo(ObjectInfo target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            base.CopyTo(target);

            target.ObjectOwner = this.ObjectOwner;
            target.ObjectName = this.ObjectName;
            target.ObjectType = this.ObjectType;
            target.TableOwner = this.TableOwner;
            target.TableName = this.TableName;

            return target;
        }

        public virtual ObjectInfo CopyFrom(ObjectInfo source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            base.CopyFrom(source);
            this.ObjectOwner = source.ObjectOwner;
            this.ObjectName = source.ObjectName;
            this.ObjectType = source.ObjectType;
            this.TableOwner = source.TableOwner;
            this.TableName = source.TableName;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((ObjectInfo)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((ObjectInfo)source);

        #endregion ICopyable

        #region ICloneable

        public new ObjectInfo Clone()
            => CopyTo(new ObjectInfo());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}