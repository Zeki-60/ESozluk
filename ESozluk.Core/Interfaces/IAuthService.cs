using ESozluk.Domain.DTOs;
using ESozluk.Domain.Entities;

namespace ESozluk.Domain.Interfaces
{
    public interface IAuthService
    {
        LoginResponse Login(LoginRequest request);
        //void Register(RegisterRequest request); 
        void ForgotPassword(string email);
        void ResetPassword(ResetPasswordRequest request);
        int GetCurrentUserId();
        User? GetCurrentUser();

    }
}