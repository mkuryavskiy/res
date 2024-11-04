using reviews.Application.Interfaces;
using reviews.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace reviews.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // Метод для створення замовлення
        public async Task<int> CreateOrderAsync(Order order)
        {
            await _orderRepository.AddAsync(order);
            return order.Id;  // Повертає ID створеного замовлення
        }

        // Метод для отримання замовлення за його ID
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }

        // Метод для отримання всіх замовлень
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }
    }
}
