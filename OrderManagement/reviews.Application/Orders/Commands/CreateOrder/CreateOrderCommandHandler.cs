using MediatR;
using reviews.Application.Interfaces;
using reviews.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace reviews.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IOrderService _orderService;

        public CreateOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                OrderDate = request.OrderDate,
                OrderItems = request.OrderItems.Select(oi => new OrderItem
                {
                    ProductName = oi.ProductName,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };

            return await _orderService.CreateOrderAsync(order);
        }
    }
}
