using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;

namespace ZzzLab.Data.Models
{
    [DataContract]
    public class TriggerInfo : DataBaseInfo, ICopyable, ICloneable, IDataRowSupport<TriggerInfo>, IQuerySupport
    {
        [DataMember]
        public virtual string TableOwner { get; set; }

        [Required]
        [DataMember]
        public virtual string TableName { get; set; }

        [DataMember]
        public virtual string TriggerOwner { get; set; }

        [Required]
        [DataMember]
        public virtual string TriggerName { get; set; }

        [DataMember]
        public virtual string TriggerType { set; get; }

        [DataMember]
        public virtual string TriggeringEvent { set; get; }

        [DataMember]
        public virtual string WhenClause { set; get; }

        [DataMember]
        public virtual string Description { set; get; }

        [DataMember]
        public virtual string ActionType { set; get; }

        [DataMember]
        public virtual string TriggerBody { set; get; }

        public TriggerInfo(TriggerInfo info = null)
        {
            if (info != null) this.Set(info);
        }

        public TriggerInfo Set(TriggerInfo info)
            => this.CopyFrom(info);

        #region IDataRowSupport

        public new TriggerInfo Set(DataRow row)
        {
            base.Set(row);

            this.TableOwner = row.ToStringNullable("TABLE_OWNER");
            this.TableName = row.ToString("TABLE_NAME");
            this.TriggerOwner = row.ToStringNullable("TRIGGER_OWNER")?.TrimStart('@');
            this.TriggerName = row.ToStringNullable("TRIGGER_NAME");
            this.TriggerType = row.ToStringNullable("TRIGGER_TYPE");
            this.TriggeringEvent = row.ToStringNullable("TRIGGERING_EVENT");
            this.WhenClause = row.ToStringNullable("WHEN_CLAUSE");
            this.Status = row.ToStringNullable("STATUS");
            this.Description = row.ToStringNullable("DESCRIPTION");
            this.ActionType = row.ToStringNullable("ACTION_TYPE");
            this.TriggerBody = row.ToStringNullable("TRIGGER_BODY");

            return this;
        }

        #endregion IDataRowSupport

        #region IQuerySupport

        public new QueryParameterCollection GetParameters()
        {
            QueryParameterCollection parameters = base.GetParameters();

            parameters.Set("TABLE_OWNER", this.TableOwner);
            parameters.Set("TABLE_NAME", this.TableName);
            parameters.Set("TRIGGER_OWNER", this.TriggerOwner);
            parameters.Set("TRIGGER_NAME", this.TriggerName);
            parameters.Set("TRIGGER_TYPE", this.TriggerType);
            parameters.Set("TRIGGERING_EVENT", this.TriggeringEvent);
            parameters.Set("WHEN_CLAUSE", this.WhenClause);
            parameters.Set("DESCRIPTION", this.Description);
            parameters.Set("ACTION_TYPE", this.ActionType);
            parameters.Set("TRIGGER_BODY", this.TriggerBody);

            return parameters;
        }

        #endregion IQuerySupport

        #region Override

        public override string ToString()
           => $"[{this.TableOwner}.{this.TableName}] {this.TriggerOwner}.{this.TriggerName} ({this.TriggerType})";

        #endregion Override

        #region ICopyable

        public virtual TriggerInfo CopyTo(TriggerInfo target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            base.CopyTo(target);

            target.TableOwner = this.TableOwner;
            target.TableName = this.TableName;
            target.TriggerOwner = this.TriggerOwner;
            target.TriggerName = this.TriggerName;
            target.TriggerType = this.TriggerType;
            target.TriggeringEvent = this.TriggeringEvent;
            target.WhenClause = this.WhenClause;
            //target.Status = this.Status;
            target.Description = this.Description;
            target.ActionType = this.ActionType;
            target.TriggerBody = this.TriggerBody;

            return target;
        }

        public virtual TriggerInfo CopyFrom(TriggerInfo source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            base.CopyFrom(source);

            this.TableOwner = source.TableOwner;
            this.TableName = source.TableName;
            this.TriggerOwner = source.TriggerOwner;
            this.TriggerName = source.TriggerName;
            this.TriggerType = source.TriggerType;
            this.TriggeringEvent = source.TriggeringEvent;
            this.WhenClause = source.WhenClause;
            //this.Status = source.Status;
            this.Description = source.Description;
            this.ActionType = source.ActionType;
            this.TriggerBody = source.TriggerBody;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((TriggerInfo)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((TriggerInfo)source);

        #endregion ICopyable

        #region ICloneable

        public new TriggerInfo Clone()
            => CopyTo(new TriggerInfo());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}