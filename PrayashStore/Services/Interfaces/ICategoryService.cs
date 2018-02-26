using PrayashStore.Models;
using System.Collections.Generic;

namespace PrayashStore.Services.Interfaces
{
    public interface ICategoryService
    {
        Category GetCategoryById(int id);
        IEnumerable<Category> GetAllCategories();
        void AddCategory(Category category);
        void EditCategory(Category category);
        void RemoveCategory(Category category);
    }
}
