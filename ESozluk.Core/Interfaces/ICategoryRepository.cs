using ESozluk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        void Add(Category category);
        Category? GetById ( int id );
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        Category GetCategoryWithTopics(int id);
    }
}
