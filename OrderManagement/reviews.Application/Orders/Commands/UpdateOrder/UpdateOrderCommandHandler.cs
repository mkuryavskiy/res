using MediatR;
using reviews.Application.Interfaces;
using reviews.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace reviews.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<int> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            // Отримуємо замовлення за ID
            var order = await _orderRepository.GetOrderById(request.OrderId);

            // Якщо замовлення не знайдено, кидаємо виняток
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {request.OrderId} was not found.");
            }

            // Оновлення інформації про замовлення
            order.CustomerName = request.CustomerName;
            order.CustomerEmail = request.CustomerEmail;
            order.OrderDate = request.OrderDate;

            // Логіка оновлення товарів замовлення
            // Наприклад, видалення старих і додавання нових товарів
            order.OrderItems.Clear();  // Видаляємо старі товари
            foreach (var item in request.OrderItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            // Оновлюємо замовлення у базі даних
            await _orderRepository.UpdateOrder(order);

            // Повертаємо ідентифікатор оновленого замовлення
            return order.Id;
        }
    }
}