using System;
using System.DirectoryServices.AccountManagement;
using ZzzLab.Models.Auth;

namespace ZzzLab.Net.LDAP
{
    public class ADConnecter
    {
        public string Domain { get; }

        public string Container { get; }

        public ADConnecter(string domain, string container)
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                throw new ArgumentNullException(domain);
            }

            if (string.IsNullOrWhiteSpace(container))
            {
                throw new ArgumentNullException(container);
            }

            Domain = domain;
            Container = container;
        }

        public virtual bool IsValidateAccount(string userId, string password)
        {
            using (PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, Domain, Container))
            {
                return principalContext.ValidateCredentials(userId, password);
            }
        }

        public virtual UserEntity GetAccount(string userId, string password, string searchUser = null)
        {
            if (string.IsNullOrEmpty(searchUser))
            {
                searchUser = userId;
            }

            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, Domain, Container, userId, password))
            {
                UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, searchUser);
                if (userPrincipal == null) return null;

                return new UserEntity
                {
                    UserId = userPrincipal.SamAccountName,
                    CompanyName = userPrincipal.GetProperty("company"),
                    DeptName = userPrincipal.GetProperty("department"),
                    UserName = userPrincipal.DisplayName,
                    Email = userPrincipal.GetProperty("mail"),
                    Mobile = userPrincipal.GetProperty("mobile"),
                    WhenCreated = userPrincipal.GetProperty("whenCreated").ToDateTimeNullable(),
                    WhenChanged = userPrincipal.GetProperty("whenChanged").ToDateTimeNullable(),
                    LastLogOn = userPrincipal.LastLogon,
                    LastLogOff = userPrincipal.GetProperty("lastLogoff")?.ToDateTimeNullable(),
                    WhenPasswordChanged = userPrincipal.LastPasswordSet
                };
            }
        }

        public virtual bool TryGetAccount(string userId, string password, out UserEntity userInfo, out string message)
        {
            userInfo = null;
            message = null;
            try
            {
                userInfo = GetAccount(userId, password, userId);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return false;
        }
    }
}