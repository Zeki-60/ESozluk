using ESozluk.Core.DTOs;
using ESozluk.Core.Interfaces;
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

        public TopicsController(ITopicService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult ListTopics()
        {
            var result = _service.GetAllTopics();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles ="Admin,Writer")]
        public IActionResult CreateTopic([FromBody] AddTopicRequest request)
        {
            _service.AddTopic(request);
            return Ok("Topic eklendi.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Writer")]
        public IActionResult DeleteTopic([FromRoute] int id, [FromBody] DeleteTopicRequest request)
        {
                request.Id = id;

                _service.DeleteTopic(request);
                return Ok("Topic silindi");
            
        }

        [HttpGet("{id}/Entry")]
        public IActionResult GetTopicWithEntries(int id, [FromQuery] string? sort)
        {
            var result = _service.GetTopicWithEntries(id,sort);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Writer")]
        public IActionResult UpdateTopic([FromBody] UpdateTopicRequest request)
        {
            
                _service.UpdateTopic(request);
                return Ok(new { Message = "Topic başarıyla güncellendi." });
            
        }
    }
}
