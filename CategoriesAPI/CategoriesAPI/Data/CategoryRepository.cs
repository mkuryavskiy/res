using CategoriesAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoriesAPI.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CategoriesDbContext _context;

        public CategoryRepository(CategoriesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool includeItems = false)
        {
            IQueryable<Category> query = _context.Categories;

            if (includeItems)
            {
                query = query.Include(c => c.Items);
            }

            return await query.ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id, bool includeItems = false)
        {
            IQueryable<Category> query = _context.Categories;

            if (includeItems)
            {
                query = query.Include(c => c.Items);
            }

            return await query.FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task AddCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
