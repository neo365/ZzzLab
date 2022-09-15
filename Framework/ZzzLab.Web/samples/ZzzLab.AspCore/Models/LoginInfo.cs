using System.Data;
using ZzzLab.Models.Auth;

namespace ZzzLab.AspCore.Models
{
    public class LoginInfo : LoginEntity, ICopyable, ICloneable
    {
        public LoginInfo Set(DataRow row)
        {
            if (row != null)
            {
                this.Uuid = row.ToStringNullable("uuid");

                this.UserId = row.ToStringNullable("user_id");
                this.UserName = row.ToStringNullable("user_name");
                this.NickName = row.ToStringNullable("nick_name");
                this.CompanyCode = row.ToStringNullable("company_code");
                this.CompanyName = row.ToStringNullable("company_name");
                this.DeptCode = row.ToStringNullable("dept_code");
                this.DeptName = row.ToStringNullable("dept_name");
                this.Mobile = row.ToStringNullable("mobile");
                this.Email = row.ToStringNullable("email");

                this.WhenCreated = row.ToDateTimeNullable("When_Created");
                this.WhenChanged = row.ToDateTimeNullable("When_Changed");
                this.WhenPasswordChanged = row.ToDateTimeNullable("When_Password_Changed");
                this.WhenExpired = row.ToDateTimeNullable("When_Expired");

                this.IsLogin = row.ToBoolean("login_yn");
                this.IsUsed = row.ToBoolean("used_yn");

                this.Memo = row.ToStringNullable("user_memo");
                this.ApiKey = row.ToStringNullable("api_key");

                this.LastLogOn = row.ToDateTimeNullable("Last_LogOn");
                this.LastLogOff = row.ToDateTimeNullable("Last_LogOff");
                this.LoginIP = row.ToStringNullable("login_ip");
                this.UserAgent = row.ToStringNullable("user_agent");
                this.LoginType = row.ToStringNullable("login_type");
                this.LoginMemo = row.ToStringNullable("login_memo");

                this.AuthRole = row.ToStringNullable("auth_role");
            }

            return this;
        }

        #region ICopyable

        public LoginInfo CopyTo(LoginInfo dest)
        {
            base.CopyTo(dest);

            return dest;
        }

        public LoginInfo CopyFrom(LoginInfo source)
        {
            base.CopyFrom(source);

            return this;
        }

        object ICopyable.CopyTo(object obj)
            => CopyTo((LoginInfo)obj);

        object ICopyable.CopyFrom(object obj)
            => CopyFrom((LoginInfo)obj);

        #endregion ICopyable

        #region ICloneable

        public new LoginInfo Clone()
            => CopyTo(new LoginInfo());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}