using ESozluk.Core.DTOs;
using ESozluk.Core.Interfaces;
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

        public LikesController(ILikeService service)
        {
            _service = service;
        }

        

        [HttpPost("toggle/{entryId}")]
        public IActionResult ToggleLike(int entryId)
        {
            string result = _service.ToggleLike(entryId);
            return Ok(result);
        }

        [HttpGet("my-likes")]
        public IActionResult GetMyLikes()
        {
            var result = _service.GetMyLikedEntries();
            return Ok(result);
        }

        
    }
}
