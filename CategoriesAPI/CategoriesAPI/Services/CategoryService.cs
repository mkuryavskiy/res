using CategoriesAPI.Models;
using CategoriesAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CategoriesAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            // Звертаємося до репозиторію для отримання категорій разом із завантаженими елементами
            return await _categoryRepository.GetAllCategoriesAsync(includeItems: true);
        }

        public async Task<Category> GetCategory(int id)
        {
            // Отримуємо категорію з елементами
            return await _categoryRepository.GetCategoryById(id, includeItems: true);
        }

        public async Task CreateCategory(Category category)
        {
            await _categoryRepository.AddCategory(category);
        }

        public async Task UpdateCategory(Category category)
        {
            await _categoryRepository.UpdateCategory(category);
        }

        public async Task DeleteCategory(int id)
        {
            await _categoryRepository.DeleteCategory(id);
        }
    }
}
