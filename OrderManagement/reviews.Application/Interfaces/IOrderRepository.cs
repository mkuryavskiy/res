// reviews.Application/Interfaces/IOrderRepository.cs
using reviews.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace reviews.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderById(int id);   // Існуючий метод
        Task<List<Order>> GetAllOrders();   // Існуючий метод
        Task<int> AddOrder(Order order);    // Існуючий метод
        Task UpdateOrder(Order order);      // Існуючий метод
        Task DeleteOrder(Order order);      // Існуючий метод

        // Додамо асинхронні методи
        Task<List<Order>> GetAllAsync();    // Новий асинхронний метод для отримання всіх замовлень
        Task<Order> GetByIdAsync(int id);   // Новий асинхронний метод для отримання замовлення за ID
        Task<int> AddAsync(Order order);    // Новий асинхронний метод для додавання замовлення
    }
}
