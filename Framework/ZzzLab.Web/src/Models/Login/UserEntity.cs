namespace ZzzLab.Models.Auth
{
    public class UserEntity : BaseUserEntity, ICopyable, ICloneable
    {
        #region Extra

        /// <summary>
        /// �޸�
        /// </summary>
        public virtual string? Memo { set; get; }

        #endregion Extra

        /// <summary>
        /// ������
        /// </summary>
        public virtual DateTimeOffset? WhenCreated { set; get; }

        /// <summary>
        /// �������� ������
        /// </summary>
        public virtual DateTimeOffset? WhenChanged { set; get; }

        /// <summary>
        /// ������ �н����� ������
        /// </summary>
        public virtual DateTimeOffset? WhenPasswordChanged { set; get; }

        /// <summary>
        /// ���� ������
        /// </summary>
        public virtual DateTimeOffset? WhenExpired { set; get; }

        /// <summary>
        /// ���� ���� ����
        /// </summary>
        public virtual bool IsExpired => DateTime.Now > WhenExpired;

        /// <summary>
        /// �α��� ���ɿ���
        /// </summary>
        public virtual bool IsLogin { set; get; }

        /// <summary>
        /// ��뿩��
        /// </summary>
        public virtual bool IsUsed { set; get; }

        /// <summary>
        /// �α��� ���ɿ���
        /// </summary>
        public virtual bool LoginEnabled => (IsExpired == false && IsLogin && IsUsed);

        #region ICopyable

        /// <summary>
        /// ���� ���� Ÿ�ٿ� �����Ѵ�.
        /// </summary>
        /// <param name="target">destnation</param>
        /// <returns>����� destnation</returns>
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
        /// ICloneable ����
        /// </summary>
        /// <returns></returns>
        public new UserEntity Clone()
            => CopyTo(new UserEntity());

        /// <summary>
        /// ICloneable ����
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable
    }
}