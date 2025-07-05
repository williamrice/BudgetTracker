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
    [Authorize]
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

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateCategoryViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var category = new ExpenseCategory
            {
                Name = model.Name,
                Description = model.Description,
                MaxAmount = model.MaxAmount,
                Color = model.Color,
                UserId = user.Id,
                IsActive = true
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            TempData["SuccessMessage"] = "Category created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null || category.UserId != user.Id)
            {
                return NotFound();
            }

            var model = new EditCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                MaxAmount = category.MaxAmount,
                Color = category.Color,
                IsActive = category.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var category = await _categoryRepository.GetByIdAsync(model.Id);
            if (category == null || category.UserId != user.Id)
            {
                return NotFound();
            }

            category.Name = model.Name;
            category.Description = model.Description;
            category.MaxAmount = model.MaxAmount;
            category.Color = model.Color;
            category.IsActive = model.IsActive;

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            TempData["SuccessMessage"] = "Category updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null || category.UserId != user.Id)
            {
                return NotFound();
            }

            // Check if category has any expenses
            if (category.BudgetedExpenses.Any())
            {
                TempData["ErrorMessage"] = "Cannot delete category that has expenses assigned to it.";
                return RedirectToAction(nameof(Index));
            }

            _categoryRepository.Remove(category);
            await _categoryRepository.SaveChangesAsync();

            TempData["SuccessMessage"] = "Category deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null || category.UserId != user.Id)
            {
                return NotFound();
            }

            category.IsActive = !category.IsActive;
            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Category {(category.IsActive ? "activated" : "deactivated")} successfully!";
            return RedirectToAction(nameof(Index));
        }

        // Keep the existing AJAX create method for the modal
        [HttpPost]
        public async Task<IActionResult> CreateQuick([FromBody] CreateCategoryViewModel model)
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
                Color = category.Color ?? "#1e3c72"
            });
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Json(categories);
        }
    }
}
