using CategoriesAPI.DTOs; // Include DTO namespace
using CategoriesAPI.Models;
using CategoriesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoriesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemService.GetAllItems();

            // Convert domain models to DTOs
            var itemDtos = items.Select(i => new ItemDTO
            {
                ItemId = i.ItemId,
                ItemName = i.ItemName,
                Description = i.Description,
                CategoryId = i.CategoryId
            });

            return Ok(itemDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await _itemService.GetItemById(id);
            if (item == null)
                return NotFound();

            // Convert domain model to DTO
            var itemDto = new ItemDTO
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                Description = item.Description,
                CategoryId = item.CategoryId
            };

            return Ok(itemDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(ItemDTO itemDto)
        {
            // Ensure that the CategoryId exists before adding the item
            var category = await _itemService.GetCategory(itemDto.CategoryId); // Assuming a method to get a category
            if (category == null)
            {
                return BadRequest("Category does not exist.");
            }

            // Convert DTO to domain model
            var item = new Item
            {
                ItemName = itemDto.ItemName,
                Description = itemDto.Description,
                CategoryId = itemDto.CategoryId
            };

            await _itemService.AddItem(item);
            return CreatedAtAction(nameof(GetItemById), new { id = item.ItemId }, itemDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, ItemDTO itemDto)
        {
            if (id != itemDto.ItemId)
                return BadRequest("Item ID mismatch.");

            // Convert DTO to domain model
            var item = new Item
            {
                ItemId = itemDto.ItemId,
                ItemName = itemDto.ItemName,
                Description = itemDto.Description,
                CategoryId = itemDto.CategoryId
            };

            await _itemService.UpdateItem(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            await _itemService.DeleteItem(id);
            return NoContent();
        }
    }
}
