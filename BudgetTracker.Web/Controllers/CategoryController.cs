using BudgetTracker.Data;
using BudgetTracker.Models;
using BudgetTracker.Models.ViewModels;
using BudgetTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetTracker.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IRepository<ExpenseCategory> _categoryRepository;

        public CategoryController(ICategoryService categoryService, UserManager<ApplicationUser> userManager, IRepository<ExpenseCategory> categoryRepository)
        {
            _categoryRepository = categoryRepository;
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
            
            var category = _categoryService.CreateCategoryFromViewModel(model, user);
            await _categoryRepository.AddAsync(category);
            var result = await _categoryRepository.SaveChangesAsync();
            
            if (result <= 0)
                return StatusCode(500, "Failed to create category.");

            return Json(new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Color = category.Color ?? "#007bff"
            });
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Json(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var allCategories = await _categoryRepository.GetAllAsync();
            var userCategories = allCategories.Where(c => c.UserId == user.Id).OrderByDescending(c => c.CreatedDate);
            
            return View(userCategories);
        }
    }
}
