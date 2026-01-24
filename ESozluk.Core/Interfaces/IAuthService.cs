using ESozluk.Core.DTOs;

namespace ESozluk.Core.Interfaces
{
    public interface IAuthService
    {
        LoginResponse Login(LoginRequest request);
        //void Register(RegisterRequest request); 
        void ForgotPassword(string email);
        void ResetPassword(ResetPasswordRequest request);

    }
}