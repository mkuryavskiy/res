using MediatR;
using reviews.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace reviews.Application.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<int> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderById(request.Id);

            if (order == null)
            {
                // Тут можна повернути статус 404 або кинути відповідний виняток
                throw new KeyNotFoundException("Order not found");
            }

            await _orderRepository.DeleteOrder(order);

            return order.Id; // Можна повернути ідентифікатор видаленого замовлення
        }
    }
}