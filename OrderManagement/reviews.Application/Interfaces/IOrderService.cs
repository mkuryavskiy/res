using reviews.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace reviews.Application.Interfaces
{
    public interface IOrderService
    {
        // Метод для створення нового замовлення
        Task<int> CreateOrderAsync(Order order);

        // Метод для отримання замовлення за його ID
        Task<Order?> GetOrderByIdAsync(int orderId);

        // Метод для отримання всіх замовлень
        Task<List<Order>> GetAllOrdersAsync();
    }
}
