using ZzzLab.Models.Auth;

namespace ZzzLab.AspCore.Models
{
    public class LoginUser : BaseUserEntity, ICopyable, ICloneable
    {
        public UserAuth? Auth { set; get; }

        /// <summary>
        /// 전달할 주요사항
        /// </summary>
        public string? Message { set; get; }

        public LoginUser Set(object obj)
        {
            if (obj != null && obj is BaseUserEntity source) base.CopyFrom(source);

            return this;
        }

        #region ICopyable

        public LoginUser CopyTo(LoginUser dest)
        {
            base.CopyTo(dest);

            dest.AuthRole = this.AuthRole;
            dest.Auth = this.Auth;
            dest.Message = this.Message;

            return dest;
        }

        public LoginUser CopyFrom(LoginUser source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            base.CopyTo(source);
            this.Auth = source.Auth;

            return this;
        }

        object ICopyable.CopyTo(object obj)
            => CopyTo((LoginUser)obj);

        object ICopyable.CopyFrom(object obj)
            => CopyFrom((LoginUser)obj);


        #endregion ICopyable

        #region ICloneable

        public new LoginUser Clone()
            => CopyTo(new LoginUser());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}