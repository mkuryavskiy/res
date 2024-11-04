using CategoriesAPI.Models;
using System.Threading.Tasks;

namespace CategoriesAPI.Services
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllItems();
        Task<Item> GetItemById(int id);
        Task AddItem(Item item);
        Task UpdateItem(Item item);
        Task DeleteItem(int id);
        Task<Category> GetCategory(int id); // Add this line
    }
}
