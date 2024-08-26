using System;

namespace ZzzLab.Models.Auth
{
    public class UserEntity : ICopyable, ICloneable
    {
        /// <summary>
        /// 사용자 아이디
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// 회사코드
        /// </summary>
        public virtual string CompanyCode { get; set; }

        /// <summary>
        /// 회사명
        /// </summary>
        public virtual string CompanyName { get; set; }

        /// <summary>
        /// 부서코드
        /// </summary>
        public virtual string DeptCode { get; set; }

        /// <summary>
        /// 부서명
        /// </summary>
        public virtual string DeptName { get; set; }

        /// <summary>
        /// 사용자명
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 별칭
        /// </summary>
        public virtual string Nickname { get; set; }

        /// <summary>
        /// 휴대폰주소
        /// </summary>
        public virtual string Mobile { get; set; }

        /// <summary>
        /// 메일주소
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 적용 그룹
        /// </summary>
        public virtual string AuthRole { get; set; }

        /// <summary>
        /// 권한
        /// </summary>
        public virtual dynamic Auth { get; set; }

        /// <summary>
        /// 생성일
        /// </summary>
        public virtual DateTimeOffset? WhenCreated { get; set; }

        #region Login Logging

        /// <summary>
        /// 변경일
        /// </summary>
        public virtual DateTimeOffset? WhenChanged { get; set; }

        /// <summary>
        /// 마지막 로그인
        /// </summary>
        public virtual DateTimeOffset? LastLogOn { set; get; }

        /// <summary>
        /// 마지막 로그오프
        /// </summary>
        public virtual DateTimeOffset? LastLogOff { set; get; }

        /// <summary>
        /// 로그인 타입
        /// </summary>
        public virtual string LoginType { set; get; }

        #endregion Login Logging

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

        #region extra

        /// <summary>
        /// Api Key
        /// </summary>
        public virtual string ApiKey { set; get; }

        /// <summary>
        /// Profile Image
        /// </summary>
        public virtual string ProfileImageUrl { get; set; }

        /// <summary>
        /// 메모
        /// </summary>
        public virtual string Memo { set; get; }

        #endregion extra

        #region ICopyable

        /// <summary>
        /// 현재 값을 타겟에 복사한다.
        /// </summary>
        /// <param name="target">destnation</param>
        /// <returns>복사된 destnation</returns>
        public virtual UserEntity CopyTo(UserEntity target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

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
            target.Auth = this.Auth;
            target.WhenCreated = this.WhenCreated;
            target.WhenChanged = this.WhenChanged;
            target.LastLogOn = this.LastLogOn;
            target.LastLogOff = this.LastLogOff;
            target.LoginType = this.LoginType;
            target.WhenPasswordChanged = this.WhenPasswordChanged;
            target.ProfileImageUrl = this.ProfileImageUrl;
            target.Memo = this.Memo;

            return target;
        }

        public virtual UserEntity CopyFrom(UserEntity source)
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
            this.Auth = source.Auth;
            this.WhenCreated = source.WhenCreated;
            this.WhenChanged = source.WhenChanged;
            this.LastLogOn = source.LastLogOn;
            this.LastLogOff = source.LastLogOff;
            this.LoginType = source.LoginType;
            this.WhenPasswordChanged = source.WhenPasswordChanged;
            this.ProfileImageUrl = source.ProfileImageUrl;
            this.Memo = source.Memo;

            return this;
        }

        object ICopyable.CopyTo(object target)
            => CopyTo((UserEntity)target);

        object ICopyable.CopyFrom(object source)
            => CopyFrom((UserEntity)source);

        #endregion ICopyable

        #region ICloneable

        /// <summary>
        /// ICloneable 구현
        /// </summary>
        /// <returns></returns>
        public virtual UserEntity Clone()
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