using ESozluk.Domain.Constants;
using ESozluk.Domain.DTOs;
using ESozluk.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESozluk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IAuthService _authService;

        public UsersController(IUserService service, IAuthService authService)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var result = _service.GetAllUsers();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateUser(AddUserRequest request)
        {
            _service.AddUser(request);
            return Ok("Kullanıcı eklendi.");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteUser([FromRoute] int id, [FromBody] DeleteUserRequest request)
        {

            request.Id = id;
            var currentUserId = _authService.GetCurrentUserId();
            var isCurrentUserAdmin = User.IsInRole(Roles.Admin);

            _service.DeleteUser(request, currentUserId, isCurrentUserAdmin);
            return Ok("Kullanıcı silindi");

        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateUser([FromBody] UpdateUserRequest request)
        {

            var currentUserId = _authService.GetCurrentUserId();
            var isCurrentUserAdmin = User.IsInRole(Roles.Admin);

            _service.UpdateUser(request, currentUserId, isCurrentUserAdmin);
            return Ok(new { Message = "Kullanıcı başarıyla güncellendi." });

        }
    }
}
