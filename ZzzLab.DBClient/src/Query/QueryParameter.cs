using System;

namespace ZzzLab.Data
{
    public sealed class QueryParameter : ICopyable, ICloneable
    {
        public string Name { set; get; }
        public object Value { internal set; get; }
        public Direction Direction { set; get; } = Direction.Input;

        internal QueryParameter(
            string name,
            object value,
            Direction? direction = Direction.Input
        )
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            this.Name = name.Trim().ToUpper();
            this.Direction = direction ?? Direction.Input;

            if (this.Direction.HasMask(Direction.Output)
                && (value == null || value == DBNull.Value))
            {
                throw new ArgumentNullException(nameof(value), "Output 은 데이터형이 명확해야됩니다. null 값을 넣지 마세요.");
            }

            this.Value = value ?? DBNull.Value;
        }

        /// <summary>
        /// Query Parameter를 정의 한다.
        /// </summary>
        /// <param name="name">파라미터명</param>
        /// <param name="value">값. class로 셋팅할 경우. json으로 출력한다.</param>
        /// <param name="direction">direction</param>
        /// <returns></returns>
        public static QueryParameter Create(string name, object value, Direction direction = Direction.Input)
            => new QueryParameter(name, value, direction);

        public override string ToString()
            => $"{this.Name}: {this.Value} {(this.Direction != Direction.Input ? $"({this.Direction})" : "")}";

        #region ICopyable

        public QueryParameter CopyTo(QueryParameter target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            target.Name = this.Name;
            target.Value = this.Value;
            target.Direction = this.Direction;

            return target;
        }

        public QueryParameter CopyFrom(QueryParameter source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.Name = source.Name;
            this.Value = source.Value;
            this.Direction = source.Direction;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((QueryParameter)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((QueryParameter)source);

        #endregion ICopyable

        #region ICloneable

        public QueryParameter Clone()
            => new QueryParameter(this.Name, this.Value, this.Direction);

        object ICloneable.Clone()
            => this.Clone();

        #endregion ICloneable
    }
}