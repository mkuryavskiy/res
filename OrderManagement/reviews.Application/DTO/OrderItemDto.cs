using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reviews.Application.DTO
{
    public class OrderItemDto
    {
        public int Id { get; set; } // Ідентифікатор товару
        public string ProductName { get; set; } // Назва товару
        public decimal UnitPrice { get; set; } // Ціна одиниці товару
        public int Quantity { get; set; } // Кількість товару
    }
}
