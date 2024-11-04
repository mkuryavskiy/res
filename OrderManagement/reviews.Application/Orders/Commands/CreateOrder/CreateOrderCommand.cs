// CreateOrderCommand.cs
using MediatR;

public class CreateOrderCommand : IRequest<int>
{
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public DateTime OrderDate { get; set; }
    public List<CreateOrderItemDto> OrderItems { get; set; }

    public class CreateOrderItemDto
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
