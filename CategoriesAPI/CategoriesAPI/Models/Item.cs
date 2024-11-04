using System.ComponentModel.DataAnnotations;

namespace CategoriesAPI.Models
{
    public class Item
    {
        [Key] // Вказуємо, що це первинний ключ
        public int ItemId { get; set; }

        [Required] // Вимагайте, щоб поле не було порожнім
        public string ItemName { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        // Зв'язок з моделлю Category
        public Category Category { get; set; }
    }
}
