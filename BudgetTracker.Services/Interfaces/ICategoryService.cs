using BudgetTracker.Models;
using BudgetTracker.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetTracker.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ExpenseCategory?> CreateCategoryAsync(CreateCategoryViewModel model, IdentityUser user);
        Task<IEnumerable<ExpenseCategory>> GetAllCategoriesAsync();
    }
}
