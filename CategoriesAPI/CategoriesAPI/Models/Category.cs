using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CategoriesAPI.Models
{
    public class Category
    {
        [Key] // Вказуємо, що це первинний ключ
        public int CategoryId { get; set; }

        [Required] // Вимагайте, щоб поле не було порожнім
        public string CategoryName { get; set; }

        // Список елементів, пов'язаних з категорією
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
