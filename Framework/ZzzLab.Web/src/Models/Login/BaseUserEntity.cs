namespace ZzzLab.Models.Auth
{
    public class BaseUserEntity : ICopyable, ICloneable
    {/// <summary>
     /// 사용자 아이디
     /// </summary>
        public string? UserId { set; get; }

        /// <summary>
        /// 회사코드
        /// </summary>
        public string? CompanyCode { set; get; }

        /// <summary>
        /// 회사명
        /// </summary>
        public string? CompanyName { get; set; }

        /// <summary>
        /// 부서코드
        /// </summary>
        public string? DeptCode { set; get; }

        /// <summary>
        /// 부서명
        /// </summary>
        public string? DeptName { set; get; }

        /// <summary>
        /// 사용자명
        /// </summary>
        public string? UserName { set; get; }

        /// <summary>
        /// 별칭
        /// </summary>
        public string? Nickname { set; get; }

        /// <summary>
        /// 휴대폰주소
        /// </summary>
        public string? Mobile { set; get; }

        /// <summary>
        /// 메일주소
        /// </summary>
        public string? Email { set; get; }

        /// <summary>
        /// 적용 그룹
        /// </summary>
        public string? AuthRole { set; get; }

        #region Extra

        /// <summary>
        /// Api Key
        /// </summary>
        public virtual string? ApiKey { set; get; }

        #endregion Extra

        #region ICopyable

        /// <summary>
        /// 현재 값을 타겟에 복사한다.
        /// </summary>
        /// <param name="target">destnation</param>
        /// <returns>복사된 destnation</returns>
        public virtual BaseUserEntity CopyTo(BaseUserEntity target)
        {
            ArgumentNullException.ThrowIfNull(target);

            target.UserId = this.UserId;
            target.CompanyCode = this.CompanyCode;
            target.CompanyName = this.CompanyName;
            target.DeptCode = this.DeptCode;
            target.DeptName = this.DeptName;
            target.UserName = this.UserName;
            target.Nickname = this.Nickname;
            target.Mobile = this.Mobile;
            target.Email = this.Email;
            target.ApiKey = this.ApiKey;
            target.AuthRole = this.AuthRole;

            return target;
        }

        public virtual BaseUserEntity CopyFrom(BaseUserEntity source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.UserId = source.UserId;
            this.CompanyCode = source.CompanyCode;
            this.CompanyName = source.CompanyName;
            this.DeptCode = source.DeptCode;
            this.DeptName = source.DeptName;
            this.UserName = source.UserName;
            this.Nickname = source.Nickname;
            this.Mobile = source.Mobile;
            this.Email = source.Email;
            this.ApiKey = source.ApiKey;
            this.AuthRole = source.AuthRole;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => this.CopyTo((BaseUserEntity)target);

        object ICopyable.CopyFrom(object source)
            => this.CopyFrom((BaseUserEntity)source);

        #endregion ICopyable

        #region ICloneable

        /// <summary>
        /// ICloneable 구현
        /// </summary>
        /// <returns></returns>
        public virtual BaseUserEntity Clone()
            => CopyTo(new BaseUserEntity());

        /// <summary>
        /// ICloneable 구현
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}