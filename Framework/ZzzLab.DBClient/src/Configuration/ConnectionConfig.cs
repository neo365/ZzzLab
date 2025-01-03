using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace ZzzLab.Data.Configuration
{
    public class ConnectionConfig : ICopyable, ICloneable
    {
        public virtual string Group { set; get; } = string.Empty;

        public virtual string Name { set; get; } = string.Empty;

        public virtual string AliasName { set; get; }

        public virtual string Host { set; get; }

        public virtual int Port { set; get; } = 0;

        public virtual string Database { set; get; }

        public virtual string UserId { set; get; }

        public virtual string Password { set; get; }

        public virtual int Timeout { set; get; } = 15;

        [JsonConverter(typeof(StringEnumConverter))]
        public virtual DataBaseType ServerType { set; get; }

        public virtual bool JournalMode { set; get; } = true;

        public virtual string ConnectionString { set; get; }

        public virtual string GetConnectionString()
            => DBClient.CreateConnectionString(this.ServerType, this.Host, this.Port, this.Database, this.UserId, this.Password, this.Timeout, this.JournalMode);

        public override string ToString()
                => $"{Name} | {ConnectionString}";

        #region ICopyable

        public ConnectionConfig CopyTo(ConnectionConfig target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            target.Name = this.Name;
            target.AliasName = this.AliasName;
            target.Host = this.Host;
            target.Port = this.Port;
            target.Database = this.Database;
            target.UserId = this.UserId;
            target.Password = this.Password;
            target.ServerType = this.ServerType;
            target.JournalMode = this.JournalMode;
            target.ConnectionString = this.ConnectionString;

            return target;
        }

        public ConnectionConfig CopyFrom(ConnectionConfig source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.Name = source.Name;
            this.AliasName = source.AliasName;
            this.Host = source.Host;
            this.Port = source.Port;
            this.Database = source.Database;
            this.UserId = source.UserId;
            this.Password = source.Password;
            this.ServerType = source.ServerType;
            this.JournalMode = source.JournalMode;
            this.ConnectionString = source.ConnectionString;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((ConnectionConfig)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((ConnectionConfig)source);

        #endregion ICopyable

        #region ICloneable

        public ConnectionConfig Clone()
            => CopyTo(new ConnectionConfig());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}