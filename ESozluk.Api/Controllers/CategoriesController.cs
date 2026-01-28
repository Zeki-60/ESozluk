using ESozluk.Domain.DTOs;
using ESozluk.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESozluk.Domain.Constants; 


namespace ESozluk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult ListCategories()
        {
            var result = _service.GetAllCategories();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles =$"{Roles.Admin}")]
        public IActionResult CreateCategory(AddCategoryRequest request)
        {
            _service.AddCategory(request);
            return Ok("Kategori eklendi.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{Roles.Admin}")]
        public IActionResult DeleteCategory([FromRoute] int id, [FromBody] DeleteCategoryRequest request)
        {
            
                request.Id = id;

                _service.DeleteCategory(request);
                return Ok("Kategori silindi");
    
        }

        [HttpGet("{id}/Topic")]
        public IActionResult GetCategoryWithTopics(int id) 
        {
            var result = _service.GetCategoryWithTopics(id);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles =$"{Roles.Admin}")]
        public IActionResult UpdateCategory([FromBody] UpdateCategoryRequest request)
        {
            
                _service.UpdateCategory(request);
                return Ok(new { Message = "Kategori başarıyla güncellendi." });
            
            
        }
    }
}
