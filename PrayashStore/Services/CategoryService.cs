using PrayashStore.Models;
using PrayashStore.Repositories.Interfaces;
using PrayashStore.Services.Interfaces;
using PrayashStore.UOW.Interfaces;
using System.Collections.Generic;

namespace PrayashStore.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _categoryRepository = UnitOfWork.GetRepository<Category>();
        }

        public IRepository<Category> CategoryRepository
        {
            get { return _categoryRepository; }
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return CategoryRepository.GetAll();
        }

        public Category GetCategoryById(int id)
        {
            return CategoryRepository.Get(id);
        }

        public void AddCategory(Category category)
        {
            CategoryRepository.Add(category);
            UnitOfWork.Complete();
        }

        public void EditCategory(Category category)
        {
            CategoryRepository.Edit(category, category.Id);
            UnitOfWork.Complete();
        }

        public void RemoveCategory(Category category)
        {
            var categoryToRemove = CategoryRepository.Get(category.Id);
            if (categoryToRemove != null)
            {
                CategoryRepository.Remove(categoryToRemove);
                UnitOfWork.Complete();
            }
        }
    }
}