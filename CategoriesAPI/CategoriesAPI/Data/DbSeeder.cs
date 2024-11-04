using Bogus;
using CategoriesAPI.Models;

namespace CategoriesAPI.Data
{
    public class DbSeeder
    {
        private readonly CategoriesDbContext _context;

        public DbSeeder(CategoriesDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Categories.Any()) return; // Перевірка наявності даних

            // Faker для категорій
            var categoryFaker = new Faker<Category>()
                .RuleFor(c => c.CategoryName, f => f.Commerce.Categories(1).First())
                .RuleFor(c => c.Items, f => new List<Item>());

            // Faker для елементів
            var itemFaker = new Faker<Item>()
                .RuleFor(i => i.ItemName, f => f.Commerce.ProductName())
                .RuleFor(i => i.Description, f => f.Lorem.Sentence());

            // Генерація категорій і елементів
            var categories = categoryFaker.Generate(10);
            foreach (var category in categories)
            {
                var items = itemFaker.Generate(5);
                foreach (var item in items)
                {
                    item.Category = category;
                    category.Items.Add(item);
                }
                _context.Categories.Add(category);
            }

            _context.SaveChanges();
        }
    }
}
