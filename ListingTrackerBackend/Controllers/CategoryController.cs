using ListingTracker.Classes;
using ListingTracker.DbEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ListingTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly qDbContext _context;

        public CategoryController(qDbContext context)
        {
            _context = context;
        }

        [HttpPost("/getCategoryList")]
        public async Task<IActionResult> GetCategoryList()
        {
            var categoryList = await _context.Categories.Where(s=>s.IsDeleted==false).ToListAsync();

            return Ok(new
            {
                IsSuccessful= true,
                Data = categoryList
            });
        }

        [HttpPost("/addCategory")]
        public async Task<IActionResult> AddCategory(CategoryType categoryType)
        {
            try
            {
                var maxCategoryLevel = await _context.Categories.MaxAsync(c => (int?)c.CategoryLevel);

                int nextCategoryLevel = maxCategoryLevel.HasValue ? maxCategoryLevel.Value + 1 : 1;

                var category = new Category()
                {
                    IsDeleted = false,
                    CategoryName = categoryType.CategoryName,
                    Id = categoryType.Id,
                    CategoryLevel = nextCategoryLevel
                };

                await _context.Categories.AddAsync(category);
                _context.SaveChanges();
                return Ok(new
                {
                    IsSuccessful = true,
                    Data = category
                });
            }
            catch (Exception ex)
            {
                return NoContent();
            }
           
        }
        [HttpPut("/updateCategory")]
        public async Task<IActionResult> UpdateCategory(CategoryType category)
        {
            var existingCategory = await _context.Categories.FindAsync(category.Id);

            if (existingCategory == null)
            {
                return NotFound("Category not found");
            }

            existingCategory.CategoryName = category.CategoryName; 
            existingCategory.CategoryLevel = 1;

            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                IsSuccessful = true,
                Data = existingCategory
            });
        }

        [HttpDelete("/deleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound("Category not found");
            }

            // Soft delete by setting IsDeleted to true
            category.IsDeleted = true;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                IsSuccessful = true,
                Message = "Category deleted successfully"
            });
        }


    }
}
