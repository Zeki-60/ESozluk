using ESozluk.Core.DTOs;
using ESozluk.Core.Entities;
using ESozluk.Core.Interfaces;
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

        public EntriesController(IEntryService service)
        {
            _service = service;
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
            _service.AddEntry(request);
            return Ok("Entry eklendi.");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteEntry([FromRoute] int id, [FromBody] DeleteEntryRequest request)
        {
                request.Id = id;

                _service.DeleteEntry(request);
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
