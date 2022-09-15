using ZzzLab.Models.Auth;

namespace ZzzLab.Web.Auth
{
    public interface IUserRepository<T> where T : UserEntity
    {
        bool ValidateCredentials(string? userId, string? password);

        T? FindByUserId(string? userId);

        bool TryFindByUserId(string? userId, out T? loginInfo, out string? message);

        string GenerateToken(T login);
    }
}