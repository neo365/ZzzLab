namespace ZzzLab.Models.Auth
{
    public class UserEntity : BaseUserEntity, ICopyable, ICloneable
    {
        #region Extra

        /// <summary>
        /// 메모
        /// </summary>
        public virtual string? Memo { set; get; }

        #endregion Extra

        /// <summary>
        /// 생성일
        /// </summary>
        public virtual DateTimeOffset? WhenCreated { set; get; }

        /// <summary>
        /// 최종정보 변경일
        /// </summary>
        public virtual DateTimeOffset? WhenChanged { set; get; }

        /// <summary>
        /// 마지막 패스워드 변경일
        /// </summary>
        public virtual DateTimeOffset? WhenPasswordChanged { set; get; }

        /// <summary>
        /// 계정 만료일
        /// </summary>
        public virtual DateTimeOffset? WhenExpired { set; get; }

        /// <summary>
        /// 계정 만료 여부
        /// </summary>
        public virtual bool IsExpired => DateTime.Now > WhenExpired;

        /// <summary>
        /// 로그인 가능여부
        /// </summary>
        public virtual bool IsLogin { set; get; }

        /// <summary>
        /// 사용여부
        /// </summary>
        public virtual bool IsUsed { set; get; }

        /// <summary>
        /// 로그인 가능여부
        /// </summary>
        public virtual bool LoginEnabled => (IsExpired == false && IsLogin && IsUsed);

        #region ICopyable

        /// <summary>
        /// 현재 값을 타겟에 복사한다.
        /// </summary>
        /// <param name="target">destnation</param>
        /// <returns>복사된 destnation</returns>
        public virtual UserEntity CopyTo(UserEntity target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            base.CopyTo(target);

            target.Memo = this.Memo;
            target.WhenCreated = this.WhenCreated;
            target.WhenChanged = this.WhenChanged;
            target.WhenPasswordChanged = this.WhenPasswordChanged;
            target.WhenExpired = this.WhenExpired;
            target.IsLogin = this.IsLogin;
            target.IsUsed = this.IsUsed;

            return target;
        }

        public UserEntity CopyFrom(UserEntity source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            base.CopyFrom(source);

            this.Memo = source.Memo;
            this.WhenCreated = source.WhenCreated;
            this.WhenChanged = source.WhenChanged;
            this.WhenPasswordChanged = source.WhenPasswordChanged;
            this.WhenExpired = source.WhenExpired;
            this.IsLogin = source.IsLogin;
            this.IsUsed = source.IsUsed;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((UserEntity)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((UserEntity)source);

        #endregion ICopyable

        #region ICloneable

        /// <summary>
        /// ICloneable 구현
        /// </summary>
        /// <returns></returns>
        public new UserEntity Clone()
            => CopyTo(new UserEntity());

        /// <summary>
        /// ICloneable 구현
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}