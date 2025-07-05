using BudgetTracker.Models;
using BudgetTracker.Models.ViewModels;
using BudgetTracker.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BudgetTracker.Services
{
    public class CategoryService : ICategoryService
    {
        public ExpenseCategory CreateCategoryFromViewModel(CreateCategoryViewModel model, IdentityUser user)
        {
            return new ExpenseCategory
            {
                Name = model.Name,
                Description = model.Description,
                UserId = user.Id
            };
        }
    }
}
