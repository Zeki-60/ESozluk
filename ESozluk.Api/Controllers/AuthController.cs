using ESozluk.Core.DTOs;
using ESozluk.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ESozluk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var result = _authService.Login(request);
            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgotPasswordRequest request)
        {
            _authService.ForgotPassword(request.Email);
            return Ok("Şifre sıfırlama talimatları gönderilmiştir.");
        }
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                _authService.ResetPassword(request);
                return Ok("Şifreniz başarıyla güncellendi. Yeni şifrenizle giriş yapabilirsiniz.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}