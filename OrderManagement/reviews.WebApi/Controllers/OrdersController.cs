using Microsoft.AspNetCore.Mvc;
using MediatR;
using reviews.Application.Orders.Queries.GetOrders;
using reviews.Application.Orders.Commands.CreateOrder;
using reviews.Application.Orders.Commands.DeleteOrder;
using reviews.Application.Orders.Commands.UpdateOrder; 
using reviews.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace reviews.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Отримати всі замовлення
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var orders = await _mediator.Send(new GetOrdersQuery());
            return Ok(orders);
        }

        // Створити нове замовлення
        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder([FromBody] CreateOrderCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOrders), new { id = orderId }, orderId);
        }

        // Видалити замовлення
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteOrder(int id)
        {
            var result = await _mediator.Send(new DeleteOrderCommand { Id = id });
            if (result == 0)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            return Ok(result);
        }

        // Оновити замовлення
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> UpdateOrder(int id, [FromBody] UpdateOrderCommand command)
        {
            if (id != command.OrderId)
            {
                return BadRequest("Order ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(command);
            if (result == 0)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            return Ok(result);
        }
    }
}
