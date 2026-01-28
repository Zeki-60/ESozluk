using ESozluk.Domain.DTOs;
using ESozluk.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ESozluk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _service;
        private readonly IAuthService _authService;

        public LikesController(ILikeService service,IAuthService authService)
        {
            _service = service;
            _authService = authService;
        }

        

        [HttpPost("toggle/{entryId}")]
        public IActionResult ToggleLike(int entryId)
        {
            int currentUserId=_authService.GetCurrentUserId();
            string result = _service.ToggleLike(entryId,currentUserId);
            return Ok(result);
        }

        [HttpGet("my-likes")]
        public IActionResult GetMyLikes()
        {
            int currentUserId= _authService.GetCurrentUserId();
            var result = _service.GetMyLikedEntries(currentUserId);
            return Ok(result);
        }

        
    }
}
