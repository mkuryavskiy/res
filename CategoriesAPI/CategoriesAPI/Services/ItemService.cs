using CategoriesAPI.Data;
using CategoriesAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CategoriesAPI.Services
{
    public class ItemService : IItemService
    {
        private readonly CategoriesDbContext _context;

        public ItemService(CategoriesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAllItems()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item> GetItemById(int id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task AddItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItem(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItem(int id)
        {
            var item = await GetItemById(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Category> GetCategory(int id) // Implement this method
        {
            return await _context.Categories.FindAsync(id);
        }
    }
}
