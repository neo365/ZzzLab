using System;

namespace ZzzLab.Data
{
    public sealed class SqlEntity : ICopyable, ICloneable
    {
        public string Section { set; get; }

        public string Label { set; get; }

        public string Command { private set; get; }

        private SqlEntity()
        {
        }

        private SqlEntity(string section, string label, string command) : this()
        {
            if (string.IsNullOrWhiteSpace(section)) throw new ArgumentNullException(nameof(section));
            if (string.IsNullOrWhiteSpace(label)) throw new ArgumentNullException(nameof(label));
            if (string.IsNullOrWhiteSpace(command)) throw new ArgumentNullException(nameof(command));

            this.Section = section;
            this.Label = label;
            this.Set(command);
        }

        public static SqlEntity Create(string section, string label, string command)
            => new SqlEntity(section, label, command);

        public void Set(string command)
        {
            if (string.IsNullOrWhiteSpace(command)) throw new ArgumentNullException(nameof(command));
            this.Command = SQLUtils.Formatter(command).Trim().TrimEnd(';');
        }

        public override string ToString()
            => $"{Section} | {Label} | {Command}";

        #region ICopyable

        public SqlEntity CopyTo(SqlEntity target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            target.Section = this.Section;
            target.Label = this.Label;
            target.Command = this.Command;

            return target;
        }

        public SqlEntity CopyFrom(SqlEntity source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.Section = source.Section;
            this.Label = source.Label;
            this.Command = source.Command;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((SqlEntity)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((SqlEntity)source);

        #endregion ICopyable

        #region ICloneable

        public SqlEntity Clone()
            => this.CopyTo(new SqlEntity());

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }

        #endregion ICloneable
    }
}