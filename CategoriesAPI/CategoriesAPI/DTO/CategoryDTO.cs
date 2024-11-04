namespace CategoriesAPI.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; } // Represents the unique ID of the category
        public string CategoryName { get; set; } // Represents the name of the category
        public List<ItemDTO> Items { get; set; } = new List<ItemDTO>(); // Represents the list of items in the category

    }
}