using NUnit.Framework;
using Moq;
using ESozluk.Business.Services;
using ESozluk.Domain.Interfaces;
using AutoMapper;
using ESozluk.Domain.DTOs;
using ESozluk.Domain.Entities;
using System;
using Microsoft.Extensions.Localization; // Bu using ifadesini ekleyin
using Microsoft.AspNetCore.Http; // Bu using ifadesini ekleyin

namespace ESozluk.Business.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private AuthService _authService;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IAuthHelper> _mockAuthHelper;
        private Mock<IMapper> _mockMapper;
        private Mock<IMailService> _mockMailService;
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor; 

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockMailService = new Mock<IMailService>();
            _mockAuthHelper = new Mock<IAuthHelper>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            

            _authService = new AuthService(
                _mockUserRepository.Object,
                _mockAuthHelper.Object,
                _mockMapper.Object,
                _mockMailService.Object,
                _mockHttpContextAccessor.Object
            );
        }

        [Test]
        public void ResetPassword_WhenPasswordsDoNotMatch_ShouldThrowException()
        {
            var request = new ResetPasswordRequest
            {
                Token = "some-valid-token",
                NewPassword = "password123",
                ConfirmPassword = "password456"
            };

            

            var ex = Assert.Throws<Exception>(() => _authService.ResetPassword(request));
            Assert.That(ex.Message, Is.EqualTo("Şifreler uyuşmuyor."));
        }

        [Test]
        public void ResetPassword_WhenTokenIsInvalidOrExpired_ShouldThrowException()
        {
            var request = new ResetPasswordRequest
            {
                Token = "invalid-token",
                NewPassword = "password123",
                ConfirmPassword = "password123"
            };

            _mockUserRepository.Setup(repo => repo.GetByResetToken(request.Token))
                               .Returns((User)null);

            


            var ex = Assert.Throws<Exception>(() => _authService.ResetPassword(request));
            Assert.That(ex.Message, Is.EqualTo("Geçersiz veya süresi dolmuş token"));
        }

        [Test]
        public void ResetPassword_WhenRequestIsValid_ShouldUpdateUserPasswordAndClearToken()
        {
            var request = new ResetPasswordRequest
            {
                Token = "valid-token",
                NewPassword = "newStrongPassword",
                ConfirmPassword = "newStrongPassword"
            };

            var user = new User
            {
                Id = 1,
                PasswordResetToken = request.Token,
                PasswordResetTokenExpires = DateTime.Now.AddHours(1)
            };

            _mockUserRepository.Setup(repo => repo.GetByResetToken(request.Token))
                               .Returns(user);

            byte[] fakeHash = { 1, 2, 3 };
            byte[] fakeSalt = { 4, 5, 6 };
            _mockAuthHelper.Setup(helper => helper.CreatePasswordHash(It.IsAny<string>(), out fakeHash, out fakeSalt));

            _authService.ResetPassword(request);

            _mockUserRepository.Verify(repo => repo.UpdateUser(It.IsAny<User>()), Times.Once);

            Assert.That(user.PasswordResetToken, Is.Null);
            Assert.That(user.PasswordResetTokenExpires, Is.Null);
        }
    }
}