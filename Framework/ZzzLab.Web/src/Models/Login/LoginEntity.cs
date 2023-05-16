using System;

namespace ZzzLab.Models.Auth
{
    /// <summary>
    /// ����Ʈ Ŀ���� ����¡ ����
    /// </summary>
    public class LoginEntity : UserEntity, ICopyable, ICloneable
    {
        #region LoginLog

        /// <summary>
        /// �α��ο� ���� ������ȣ
        /// </summary>
        public virtual string? Uuid { set; get; }

        /// <summary>
        /// �α��� ip
        /// </summary>
        public virtual string? LoginIP { set; get; }

        /// <summary>
        /// User Agent ����
        /// </summary>
        public virtual string? UserAgent { set; get; }

        /// <summary>
        /// ������ �α���
        /// </summary>
        public virtual DateTimeOffset? LastLogOn { set; get; }

        /// <summary>
        /// ������ �α׿���
        /// </summary>
        public virtual DateTimeOffset? LastLogOff { set; get; }

        /// <summary>
        /// �α��� Ÿ��
        /// </summary>
        public string? LoginType { set; get; }

        /// <summary>
        /// �α��� �޸�
        /// </summary>
        public string? LoginMemo { set; get; }

        #endregion LoginLog

        #region ICopyable

        /// <summary>
        /// ��� ������ ���� �Ѵ�.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public LoginEntity CopyTo(LoginEntity target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            base.CopyTo(target);

            target.Uuid = this.Uuid;
            target.LoginIP = this.LoginIP;
            target.UserAgent = this.UserAgent;
            target.LastLogOn = this.LastLogOn;
            target.LastLogOff = this.LastLogOff;
            target.LoginType = this.LoginType;
            target.LoginMemo = this.LoginMemo;

            return target;
        }

        /// <summary>
        /// �ҽ��� ������ �����´�.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>

        public LoginEntity CopyFrom(LoginEntity source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            base.CopyFrom(source);

            this.Uuid = source.Uuid;
            this.LoginIP = source.LoginIP;
            this.UserAgent = source.UserAgent;
            this.LastLogOn = source.LastLogOn;
            this.LastLogOff = source.LastLogOff;
            this.LoginType = source.LoginType;
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
        /// ICloneable ����
        /// </summary>
        /// <returns>������ ��</returns>
        public new LoginEntity Clone()
            => CopyTo(new LoginEntity());

        /// <summary>
        /// ICloneable ����
        /// </summary>
        /// <returns>������ ��</returns>
        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}