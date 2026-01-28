using AutoMapper;
using Core.Extensions;
using ESozluk.Business.Utilities.Security;
using ESozluk.Domain.DTOs;
using ESozluk.Domain.Entities;
using ESozluk.Domain.Exceptions;
using ESozluk.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace ESozluk.Business.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthHelper _authHelper;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMailService _mailService;
    


    public AuthService(IUserRepository userRepository, IAuthHelper authHelper, IMapper mapper, IMailService mailService, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _authHelper = authHelper;
        _mapper = mapper;
        _mailService = mailService;
        _httpContextAccessor = httpContextAccessor;

    }

    public int GetCurrentUserId()
    {
        var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        string.IsNullOrEmpty(userIdString)
                        .IfTrueThrow(() => new AuthorizedAccessException(Resources.SharedResource.UserNotFound));
        return int.Parse(userIdString);
    }
    public User? GetCurrentUser()
    {
        var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;


        string.IsNullOrEmpty(userIdString)
            .IfTrueThrow(() => new AuthorizedAccessException(Resources.SharedResource.UserNotFound));
        //if (string.IsNullOrEmpty(userIdString))
        //{
        //    throw new AuthorizedAccessException("Kullanıcı oturumu bulunamadı.");
        //}
        return _userRepository.GetById(int.Parse(userIdString));
    }

    public LoginResponse Login(LoginRequest request)
    {
        //var user = _userRepository.GetAllUsers().FirstOrDefault(x => x.Email == request.Email);
        var user = _userRepository.GetUserByEmailWithRoles(request.Email);


        (user == null)
                .IfTrueThrow(() => new NotFoundException(Resources.SharedResource.UserNotFound));

        (!_authHelper.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            .IfTrueThrow(() => new ValidationException(Resources.SharedResource.PasswordError));


        
        var roles = user.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? new List<string>();


        var token = _authHelper.GenerateToken(user.Id, user.Email, roles);
        var response = _mapper.Map<LoginResponse>(user);
        response.Token = token;

        return response;
    }

    public void ForgotPassword(string email)
    {
        var user = _userRepository.GetByEmail(email);
        (user == null)
                .IfTrueThrow(() => new NotFoundException(Resources.SharedResource.UserNotFound));



        user.PasswordResetToken = Guid.NewGuid().ToString();
        user.PasswordResetTokenExpires = DateTime.Now.AddHours(1);//sunuculara göre değişebilir (UTC NOW) ile çözülebilir.
        _userRepository.UpdateUser(user);
        _mailService.SendPasswordResetEmail(user.Email, user.PasswordResetToken);
    }

    public void ResetPassword(ResetPasswordRequest request)
    {

        (request.NewPassword != request.ConfirmPassword)
            .IfTrueThrow(() => new Exception(Resources.SharedResource.ErrorPasswordsDoNotMatch));
        

        var user = _userRepository.GetByResetToken(request.Token);

        (user == null || user.PasswordResetTokenExpires < DateTime.Now)
            .IfTrueThrow(() => new Exception(Resources.SharedResource.ErrorInvalidOrExpiredToken));

        
        byte[] passwordHash, passwordSalt;
        _authHelper.CreatePasswordHash(request.NewPassword, out passwordHash, out passwordSalt);

        user.PasswordHash = Convert.ToBase64String(passwordHash);
        user.PasswordSalt = Convert.ToBase64String(passwordSalt);

        user.PasswordResetToken = null;
        user.PasswordResetTokenExpires = null;

        _userRepository.UpdateUser(user);

    }
    // verdiğim role curent userda var mı
}