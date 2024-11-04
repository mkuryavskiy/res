using CategoriesAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CategoriesAPI.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories(); // Метод для отримання всіх категорій
        Task<Category> GetCategory(int id); // Метод для отримання категорії за ID
        Task CreateCategory(Category category); // Метод для створення категорії
        Task UpdateCategory(Category category); // Метод для оновлення категорії
        Task DeleteCategory(int id); // Метод для видалення категорії
    }
}
