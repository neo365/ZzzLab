using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;

namespace ZzzLab.Data.Models
{
    [DataContract]
    public class SynonymInfo : DataBaseInfo, ICopyable, ICloneable
    {
        [Required]
        [DataMember]
        public virtual string TableOwner { get; set; }

        [Required]
        [DataMember]
        public virtual string TableName { get; set; }

        public virtual string TableLink { get; set; }

        [Required]
        [DataMember]
        public virtual string SynonymOwner { get; set; }

        [Required]
        [DataMember]
        public virtual string SynonymName { get; set; }

        public SynonymInfo(SynonymInfo info = null)
        {
            if (info != null) this.Set(info);
        }

        public SynonymInfo Set(SynonymInfo info)
            => this.CopyFrom(info);

        public new SynonymInfo Set(DataRow row)
        {
            base.Set(row);

            this.TableOwner = row.ToStringNullable("TABLE_OWNER");
            this.TableName = row.ToString("TABLE_NAME");
            this.TableLink = row.ToStringNullable("TABLE_LINK");
            this.SynonymOwner = row.ToStringNullable("SYNONYM_OWNER");
            this.SynonymName = row.ToString("SYNONYM_NAME");

            return this;
        }

        #region IQuerySupport

        public new QueryParameterCollection GetParameters()
        {
            QueryParameterCollection parameters = base.GetParameters();

            parameters.Set("TABLE_OWNER", this.TableOwner);
            parameters.Set("TABLE_NAME", this.TableName);
            parameters.Set("TABLE_LINK", this.TableLink);
            parameters.Set("SYNONYM_OWNER", this.SynonymOwner);
            parameters.Set("SYNONYM_NAME", this.SynonymName);

            return parameters;
        }

        #endregion IQuerySupport

        #region Override

        public override string ToString()
           => $"[{TableOwner}.{TableName} {(string.IsNullOrWhiteSpace(this.TableLink) ? string.Empty : $"@{this.TableLink}")}] {SynonymOwner}.{SynonymName}";

        #endregion Override

        #region ICopyable

        public virtual SynonymInfo CopyTo(SynonymInfo target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            base.CopyTo(target);

            target.TableOwner = this.TableOwner;
            target.TableName = this.TableName;
            target.SynonymOwner = this.SynonymOwner;
            target.SynonymName = this.SynonymName;
            target.TableLink = this.TableLink;

            return target;
        }

        public virtual SynonymInfo CopyFrom(SynonymInfo source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            base.CopyFrom(source);

            this.TableOwner = source.TableOwner;
            this.TableName = source.TableName;
            this.SynonymOwner = source.SynonymOwner;
            this.SynonymName = source.SynonymName;
            this.TableLink = source.TableLink;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((SynonymInfo)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((SynonymInfo)source);

        #endregion ICopyable

        #region ICloneable

        public new SynonymInfo Clone()
            => CopyTo(new SynonymInfo());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}