using CategoriesAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CategoriesAPI.Data
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAllItemsAsync(); // Додати цей метод
        Task<Item> GetItemById(int id);
        Task AddItem(Item item);
        Task UpdateItem(Item item);
        Task DeleteItem(int id);
    }
}
