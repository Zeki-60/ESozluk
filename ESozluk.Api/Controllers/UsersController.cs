using ESozluk.Core.DTOs;
using ESozluk.Core.Interfaces;
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

        public UsersController(IUserService service)
        {
            _service = service;
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

                _service.DeleteUser(request);
                return Ok("Kullanıcı silindi");
            
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateUser([FromBody]UpdateUserRequest request)
        {

                
                _service.UpdateUser(request);
                return Ok(new { Message = "Kullanıcı başarıyla güncellendi." });
            
        }
    }
}
