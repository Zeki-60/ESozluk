using ESozluk.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.Interfaces
{
    public interface ICategoryService
    {
        List<CategoryResponse> GetAllCategories();
        void AddCategory(AddCategoryRequest request);
        void DeleteCategory(DeleteCategoryRequest request);
        void UpdateCategory(UpdateCategoryRequest request);
        CategoryWithTopicsResponse GetCategoryWithTopics(int categoryId);

    }
}
