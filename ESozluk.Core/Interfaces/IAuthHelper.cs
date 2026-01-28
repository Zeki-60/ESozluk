// ESozluk.Domain/Interfaces/IAuthHelper.cs
namespace ESozluk.Domain.Interfaces
{
    public interface IAuthHelper
    {
        string GenerateToken(int userId, string email, List<string> roles);
        bool VerifyPassword(string password, string passwordHash, string passwordSalt);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    }
}