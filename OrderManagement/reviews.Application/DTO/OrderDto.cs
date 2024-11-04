using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reviews.Application.DTO
{
    public class OrderDto
    {
        public int Id { get; set; } // Ідентифікатор замовлення
        public string CustomerName { get; set; } // Ім'я замовника
        public string CustomerEmail { get; set; } // Email замовника
        public DateTime OrderDate { get; set; } // Дата замовлення
        public List<OrderItemDto> OrderItems { get; set; } // Список товарів у замовленні
    }
}
