using AutoMapper;
using Core.Extensions;
using ESozluk.Business.Utilities.Security;
using ESozluk.Core.DTOs;
using ESozluk.Core.Entities;
using ESozluk.Core.Exceptions;
using ESozluk.Core.Interfaces;
using ESozluk.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthHelper _authHelper;
        private readonly IMailService _mailService;


        public UserService(IUserRepository repository, IMapper mapper,IHttpContextAccessor httpContextAccessor,AuthHelper authHelper,IMailService mailService)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _authHelper = authHelper;
            _mailService = mailService;
        }
        public int GetCurrentUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                throw new AuthorizedAccessException("Kullanıcı oturumu bulunamadı.");
            }
            return int.Parse(userIdString);
        }

        public User? GetCurrentUser()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                throw new AuthorizedAccessException("Kullanıcı oturumu bulunamadı.");
            }
            return _repository.GetById(int.Parse(userIdString));
        }



        public void AddUser(AddUserRequest request)
        {

            _repository.GetAllUsers().Any(u => u.Email == request.Email)
                .IfTrueThrow(() => new ValidationException("Bu e-posta adresi zaten kullanılıyor."));


            byte[] passwordHash, passwordSalt;
            _authHelper.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);
            var userEntity = _mapper.Map<User>(request);
            userEntity.PasswordHash = Convert.ToBase64String(passwordHash); ;
            userEntity.PasswordSalt = Convert.ToBase64String(passwordSalt); ;

            userEntity.RegistrationDate = DateTime.Now;

            _repository.AddUser(userEntity);
            //mail gönderiyoruz kayıt olduğu için
            _mailService.SendWelcomeEmail(request.Email, request.FullName);



            
        }
        public void UpdateUser(UpdateUserRequest request)
        {
            var user = _repository.GetById(request.Id);
            (user == null)
                .IfTrueThrow(() => new NotFoundException("Kullanıcı bulunamadı."));
            var currentUser = _httpContextAccessor.HttpContext ?. User;
            bool isAdmin = currentUser.IsInRole("Admin");

            if (user.Id != GetCurrentUserId() && !isAdmin)
            {
                throw new AuthorizedAccessException("Bu kullancıyı güncelleme yetkiniz yok.");
            }

            byte[] passwordHash, passwordSalt;
            _authHelper.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);
            _mapper.Map(request, user);
            user.PasswordHash = Convert.ToBase64String(passwordHash);
            user.PasswordSalt = Convert.ToBase64String(passwordSalt);
            _repository.UpdateUser(user);     
        }
        public List<UserResponse> GetAllUsers()
        {
            var users = _repository.GetAllUsers();
            return _mapper.Map<List<UserResponse>>(users);

            
        }


        public void DeleteUser(DeleteUserRequest request)
        {
            var user = _repository.GetById(request.Id);
            if (user == null)
            {
                throw new NotFoundException("Kullanıcı bulunamadı.");
            }
            var currentUser = _httpContextAccessor.HttpContext?.User;
            bool isAdmin = currentUser.IsInRole("Admin");
            if (user.Id != GetCurrentUserId() && !isAdmin)
            {
                throw new AuthorizedAccessException("Bu kullanıcıyı silme yetkiniz yok.");
            }
            _repository.DeleteUser(user);
        }
    }
}