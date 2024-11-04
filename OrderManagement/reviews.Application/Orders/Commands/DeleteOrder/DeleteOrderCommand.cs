using MediatR;

namespace reviews.Application.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}