using MediatR;
using reviews.Domain.Entities;
using System.Collections.Generic;

namespace reviews.Application.Orders.Queries.GetOrders
{
    public class GetOrdersQuery : IRequest<List<Order>>
    {
        // Додаткові параметри запиту можуть бути додані тут, якщо потрібно
    }
}
