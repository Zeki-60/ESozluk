using ESozluk.Business;
using ESozluk.Domain.Constants;
using ESozluk.Domain.DTOs;
using ESozluk.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ESozluk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        private readonly IComplaintService _service;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ComplaintsController(IComplaintService service, IStringLocalizer<SharedResource> localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        [HttpPost]
        public IActionResult Add(AddComplaintRequest request)
        {
            
                _service.AddComplaint(request);
                return Ok("Başarılı");
            
        }

        [HttpGet("my-complaints")]
        public IActionResult GetMyComplaints()
        {
            var result = _service.GetMyComplaints();
            return Ok(result);
        }

        [HttpGet("admin/all")]
        [Authorize(Roles = $"{Roles.Admin}")] 
        public IActionResult GetAll()
        {
            var result = _service.GetAllComplaints();
            return Ok(result);
        }
    }
}
