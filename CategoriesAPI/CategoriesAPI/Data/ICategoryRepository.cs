using CategoriesAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CategoriesAPI.Data
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(bool includeItems = false);
        Task<Category> GetCategoryById(int id, bool includeItems = false);
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int id);
    }
}
