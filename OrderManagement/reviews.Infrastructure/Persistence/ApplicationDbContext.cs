using Microsoft.EntityFrameworkCore;
using reviews.Domain.Entities;

namespace reviews.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Вказати тип стовпця для UnitPrice
            modelBuilder.Entity<OrderItem>()
                .Property(o => o.UnitPrice)
                .HasColumnType("decimal(18,2)"); // Приклад: 18 цифр в цілому, з 2 знаками після коми

            // Інші конфігурації моделей, якщо є
        }
    }
}
