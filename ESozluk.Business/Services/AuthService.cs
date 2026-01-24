using AutoMapper;
using ESozluk.Business.Utilities.Security;
using ESozluk.Core.DTOs;
using ESozluk.Core.Entities;
using ESozluk.Core.Exceptions;
using ESozluk.Core.Interfaces;

namespace ESozluk.Business.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly AuthHelper _authHelper;
    private readonly IMapper _mapper;
    private readonly IMailService _mailService;


    public AuthService(IUserRepository userRepository, AuthHelper authHelper,IMapper mapper,IMailService mailService)
    {
        _userRepository = userRepository;
        _authHelper = authHelper;
        _mapper = mapper;
        _mailService = mailService;

}

public LoginResponse Login(LoginRequest request)
    {
        //var user = _userRepository.GetAllUsers().FirstOrDefault(x => x.Email == request.Email);
        var user = _userRepository.GetUserByEmailWithRoles(request.Email);


        if (user == null)
        {
            throw new NotFoundException("Kullanıcı bulunamadı.");
        }


        if (!_authHelper.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new ValidationException("Şifre hatalı!");
        }
        var roles = user.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? new List<string>();


        var token = _authHelper.GenerateToken(user.Id, user.Email,roles);
        var response = _mapper.Map<LoginResponse>(user);
        response.Token = token;

        return response;
    }

    public void ForgotPassword(string email)
    {
        var user = _userRepository.GetByEmail(email);
        if(user == null)
        {
            throw new NotFoundException("Kullanıcı bulunamadı.");
        }

        user.PasswordResetToken= Guid.NewGuid().ToString();
        user.PasswordResetTokenExpires=DateTime.Now.AddHours(1);//sunuculara göre değişebilir (UTC NOW) ile çözülebilir.
        _userRepository.UpdateUser(user);
        _mailService.SendPasswordResetEmail(user.Email, user.PasswordResetToken);
    }

    public void ResetPassword(ResetPasswordRequest request)
    {
        if(request.NewPassword!=request.ConfirmPassword)
        {
            throw new Exception("Şifreler uyuşmuyor");
        }

        var user = _userRepository.GetByResetToken(request.Token);
        if(user == null || user.PasswordResetTokenExpires<DateTime.Now)
        {
            throw new Exception("Geçersiz veya süresi dolmuş token");
        }
        byte[] passwordHash, passwordSalt;
        _authHelper.CreatePasswordHash(request.NewPassword,out passwordHash,out passwordSalt);

        user.PasswordHash = Convert.ToBase64String(passwordHash);
        user.PasswordSalt = Convert.ToBase64String(passwordSalt);

        user.PasswordResetToken = null;
        user.PasswordResetTokenExpires = null;

        _userRepository.UpdateUser(user);

    }
    // verdiğim role curent userda var mı
}