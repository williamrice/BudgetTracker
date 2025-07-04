using BudgetTracker.Data;
using BudgetTracker.Models;
using BudgetTracker.Models.ViewModels;
using BudgetTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BudgetTracker.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryController(ICategoryService categoryService, UserManager<ApplicationUser> userManager)
        {
            _categoryService = categoryService;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized("User not found.");
            var result = await _categoryService.CreateCategoryAsync(model, user);
            if (result == null)
                return StatusCode(500, "Failed to create category.");

            return Json(new CategoryDto
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                Color = result.Color ?? "#007bff"
            });
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Json(categories);
        }
    }
}
