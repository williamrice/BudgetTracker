using BudgetTracker.Models;
using BudgetTracker.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BudgetTracker.Services.Interfaces
{
    public interface ICategoryService
    {
        ExpenseCategory CreateCategoryFromViewModel(CreateCategoryViewModel model, IdentityUser user);
        // CRUD methods removed. Use repository directly for Add, Update, Delete, and Get operations.
    }
}
