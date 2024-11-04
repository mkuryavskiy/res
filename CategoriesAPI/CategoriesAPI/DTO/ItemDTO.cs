namespace CategoriesAPI.DTOs
{
    public class ItemDTO
    {
        public int ItemId { get; set; } // Represents the unique ID of the item
        public string ItemName { get; set; } // Represents the name of the item
        public string Description { get; set; } // Description of the item
        public int CategoryId { get; set; } // Foreign key to the category

    }
}
