
using BudgetTracker.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BudgetTracker.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<CreateExpenseViewModel> PrepareCreateViewModelAsync();
        Task<EditExpenseViewModel> PrepareEditViewModelAsync(int id);
        Task<bool> CreateExpenseAsync(CreateExpenseViewModel model, IdentityUser user);
        Task<bool> UpdateExpenseAsync(EditExpenseViewModel model);
        Task<ExpenseListViewModel> GetExpenseListAsync(int page = 1, int pageSize = 10);
    }
}