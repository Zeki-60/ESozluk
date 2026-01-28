using AutoMapper;
using Core.Extensions;
using ESozluk.Domain.DTOs;
using ESozluk.Domain.Entities;
using ESozluk.Domain.Exceptions;
using ESozluk.Domain.Interfaces;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        public readonly IMapper _mapper;


        public CategoryService(ICategoryRepository repository, IMapper mapper )
        {
            _repository = repository;
            _mapper = mapper;
           
        }


        public void AddCategory(AddCategoryRequest request)
        {


            var categoryEntity = _mapper.Map<Category>(request);


            _repository.Add(categoryEntity);

        }
        public CategoryWithTopicsResponse GetCategoryWithTopics(int categoryId)
        {
            var category = _repository.GetCategoryWithTopics(categoryId);

            (category==null)
                .IfTrueThrow(() => new NotFoundException(Resources.SharedResource.ErrorCategoryNotFound));


            
            var response = new CategoryWithTopicsResponse
            {
                Id = category.Id,
                Name = category.Name,
                // Category entity'sinin içindeki Topics listesini mapliyoruz
                Topics = _mapper.Map<List<TopicResponse>>(category.Topics)
            };

            return response;
        }

        public List<CategoryResponse> GetAllCategories()
        {

            var category = _repository.GetAll();
            return _mapper.Map<List<CategoryResponse>>(category);
            
        }

        public void UpdateCategory(UpdateCategoryRequest request)
        {
            var category = _repository.GetById(request.Id);

            (category==null)
                .IfTrueThrow(() => new NotFoundException(Resources.SharedResource.ErrorCategoryNotFound));

            
            _mapper.Map(request, category);
            _repository.UpdateCategory(category);


        }


        public void DeleteCategory(DeleteCategoryRequest request)
        {
            var category = _repository.GetById(request.Id);

            (category==null)
                .IfTrueThrow(() => new NotFoundException(Resources.SharedResource.ErrorCategoryNotFound));

            
            _repository.DeleteCategory(category);
        }
    }
}
