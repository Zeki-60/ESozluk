using ESozluk.Business.Services;
using ESozluk.Domain.DTOs;
using ESozluk.Domain.Entities;
using ESozluk.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESozluk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntriesController : ControllerBase
    {
        private readonly IEntryService _service;
        private readonly IAuthService _authService;

        public EntriesController(IEntryService service, IAuthService authService)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string? sort, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)  
        {
           
                var result = _service.GetAllEntries(sort, page, pageSize);
                return Ok(result);
            
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateEntry(AddEntryRequest request)
        {
            var currentUserId = _authService.GetCurrentUserId();
            _service.AddEntry(request, currentUserId);
            return Ok("Entry eklendi.");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteEntry([FromRoute] int id, [FromBody] DeleteEntryRequest request)
        {
                request.Id = id;
                int currentUserId= _authService.GetCurrentUserId();
                bool isCurrentUserModerator= User.IsInRole("Moderator");

            _service.DeleteEntry(request,currentUserId,isCurrentUserModerator);
                return Ok("Entry silindi");
            
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateEntry([FromBody] UpdateEntryRequest request)
        {
            
                _service.UpdateEntry(request);
                return Ok(new { Message = "Entry başarıyla güncellendi." });
            
        }

        
    }
}
