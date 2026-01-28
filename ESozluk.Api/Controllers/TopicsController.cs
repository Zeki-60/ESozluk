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
    public class TopicsController : ControllerBase
    {
        private readonly ITopicService _service;
        private readonly IAuthService _authService;

        public TopicsController(ITopicService service,IAuthService authService)
        {
            _service = service;
            _authService= authService;
        }

        [HttpGet]
        public IActionResult ListTopics()
        {
            var result = _service.GetAllTopics();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles =$"{Roles.Admin},{Roles.Writer}")]
        public IActionResult CreateTopic([FromBody] AddTopicRequest request)
        {
            _service.AddTopic(request);
            return Ok("Topic eklendi.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Writer}")]
        public IActionResult DeleteTopic([FromRoute] int id, [FromBody] DeleteTopicRequest request)
        {
                request.Id = id;
                int currentUserId=_authService.GetCurrentUserId();
                _service.DeleteTopic(request,currentUserId);
                return Ok("Topic silindi");
            
        }

        [HttpGet("{id}/Entry")]
        public IActionResult GetTopicWithEntries(int id, [FromQuery] string? sort)
        {
            var result = _service.GetTopicWithEntries(id,sort);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Writer}")]
        public IActionResult UpdateTopic([FromBody] UpdateTopicRequest request)
        {
                int currentUserId=_authService.GetCurrentUserId();
                _service.UpdateTopic(request,currentUserId);
                return Ok(new { Message = "Topic başarıyla güncellendi." });
            
        }
    }
}
