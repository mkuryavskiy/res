using CategoriesAPI.Models;
using Microsoft.EntityFrameworkCore; // Необхідно для DbContext
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CategoriesAPI.Data
{
    public class ItemRepository : IItemRepository
    {
        private readonly CategoriesDbContext _context;

        public ItemRepository(CategoriesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _context.Items.ToListAsync(); // Отримуємо всі товари асинхронно
        }

        public async Task<Item> GetItemById(int id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task AddItem(Item item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItem(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
