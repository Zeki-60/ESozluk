using AutoMapper;
using Core.Extensions;
using ESozluk.Business.Utilities.Security;
using ESozluk.Domain.DTOs;
using ESozluk.Domain.Entities;
using ESozluk.Domain.Exceptions;
using ESozluk.Domain.Interfaces;
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
        private readonly IAuthHelper _authHelper;
        private readonly IMailService _mailService;
        private readonly IAuthService _authService;



        public UserService(IUserRepository repository, IMapper mapper,IHttpContextAccessor httpContextAccessor,IAuthHelper authHelper,IMailService mailService, IAuthService authService)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _authHelper = authHelper;
            _mailService = mailService;
            _authService= authService;

        }
        

        



        public void AddUser(AddUserRequest request)
        {
            _repository.GetAllUsers().Any(u => u.Email == request.Email)
                .IfTrueThrow(() => new ValidationException(Resources.SharedResource.ValidationEmailAlreadyExists));


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
        public void UpdateUser(UpdateUserRequest request, int currentUserId, bool isCurrentUserAdmin)
        {
            var user = _repository.GetById(request.Id);
            (user == null)
                .IfTrueThrow(() => new NotFoundException(Resources.SharedResource.UserNotFound));
            //var currentUser = _httpContextAccessor.HttpContext ?. User;
            //bool isAdmin = currentUser.IsInRole("Admin");

            (user.Id != currentUserId && !isCurrentUserAdmin)
                .IfTrueThrow(() => new AuthorizedAccessException(Resources.SharedResource.ErrorUnauthorizedAccess));



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


        public void DeleteUser(DeleteUserRequest request,int currentUserId, bool isCurrentUserAdmin)
        {
            var user = _repository.GetById(request.Id);

            (user == null)
                .IfTrueThrow(() => new NotFoundException(Resources.SharedResource.UserNotFound));

            //var currentUser = _httpContextAccessor.HttpContext?.User;
            //bool isAdmin = currentUser.IsInRole("Admin");



            (user.Id != currentUserId && !isCurrentUserAdmin)
                .IfTrueThrow(() => new AuthorizedAccessException(Resources.SharedResource.ErrorUnauthorizedAccess));


            _repository.DeleteUser(user);
        }
    }
}