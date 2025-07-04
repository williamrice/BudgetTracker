using BudgetTracker.Data;
using BudgetTracker.Models;
using BudgetTracker.Models.ViewModels;
using BudgetTracker.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetTracker.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<ExpenseCategory> _categoryRepository;

        public CategoryService(IRepository<ExpenseCategory> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ExpenseCategory?> CreateCategoryAsync(CreateCategoryViewModel model, IdentityUser user)
        {
            var category = new ExpenseCategory
            {
                Name = model.Name,
                Description = model.Description,
                UserId = user.Id
            };
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<ExpenseCategory>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }
    }
}
