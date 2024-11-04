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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories(string sortBy = "id", bool ascending = true)
        {
            var categories = await _categoryService.GetCategories();

            // Apply sorting based on the provided parameters
            categories = sortBy.ToLower() switch
            {
                "name" => ascending ? categories.OrderBy(c => c.CategoryName).ToList() : categories.OrderByDescending(c => c.CategoryName).ToList(),
                _ => ascending ? categories.OrderBy(c => c.CategoryId).ToList() : categories.OrderByDescending(c => c.CategoryId).ToList(),
            };

            // Convert domain models to DTOs
            var categoryDtos = categories.Select(c => new CategoryDTO
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Items = c.Items.Select(i => new ItemDTO
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    Description = i.Description,
                    CategoryId = i.CategoryId
                }).ToList()
            });

            return Ok(categoryDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategory(id);
            if (category == null)
                return NotFound();

            // Convert domain model to DTO
            var categoryDto = new CategoryDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Items = category.Items.Select(i => new ItemDTO
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    Description = i.Description,
                    CategoryId = i.CategoryId
                }).ToList()
            };

            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDTO categoryDto)
        {
            // Convert DTO to domain model
            var category = new Category
            {
                CategoryName = categoryDto.CategoryName,
                Items = categoryDto.Items.Select(i => new Item
                {
                    ItemName = i.ItemName,
                    Description = i.Description,
                    CategoryId = i.CategoryId
                }).ToList()
            };

            await _categoryService.CreateCategory(category);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryId }, categoryDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDTO categoryDto)
        {
            if (id != categoryDto.CategoryId)
                return BadRequest("Category ID mismatch.");

            // Convert DTO to domain model
            var category = new Category
            {
                CategoryId = categoryDto.CategoryId,
                CategoryName = categoryDto.CategoryName,
                Items = categoryDto.Items.Select(i => new Item
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    Description = i.Description,
                    CategoryId = i.CategoryId
                }).ToList()
            };

            await _categoryService.UpdateCategory(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategory(id);
            return NoContent();
        }
    }
}
