namespace ZzzLab.Models.Auth
{
    /// <summary>
    /// 사이트 커스터 마이징 정보
    /// </summary>
    public class LoginEntity : UserEntity, ICopyable, ICloneable
    {
        #region LoginLog

        /// <summary>
        /// 로그인에 대한 고유번호
        /// </summary>
        public virtual string? Uuid { set; get; }

        /// <summary>
        /// 로그인 ip
        /// </summary>
        public virtual string? LoginIP { set; get; }

        /// <summary>
        /// User Agent 정보
        /// </summary>
        public virtual string? UserAgent { set; get; }

        /// <summary>
        /// 로그인 메모
        /// </summary>
        public string? LoginMemo { set; get; }

        #endregion LoginLog

        #region ICopyable

        /// <summary>
        /// 대상에 내용을 복사 한다.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public LoginEntity CopyTo(LoginEntity target)
        {
            ArgumentNullException.ThrowIfNull(target);

            base.CopyTo(target);

            target.Uuid = this.Uuid;
            target.LoginIP = this.LoginIP;
            target.UserAgent = this.UserAgent;
            target.LoginMemo = this.LoginMemo;

            return target;
        }

        /// <summary>
        /// 소스의 내용을 가져온다.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>

        public LoginEntity CopyFrom(LoginEntity source)
        {
            ArgumentNullException.ThrowIfNull(source);

            base.CopyFrom(source);

            this.Uuid = source.Uuid;
            this.LoginIP = source.LoginIP;
            this.UserAgent = source.UserAgent;
            this.LoginMemo = source.LoginMemo;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((LoginEntity)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((LoginEntity)source);

        #endregion ICopyable

        #region ICloneable

        /// <summary>
        /// ICloneable 구현
        /// </summary>
        /// <returns>복제된 값</returns>
        public new LoginEntity Clone()
            => CopyTo(new LoginEntity());

        /// <summary>
        /// ICloneable 구현
        /// </summary>
        /// <returns>복제된 값</returns>
        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}