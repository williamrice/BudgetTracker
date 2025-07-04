
using BudgetTracker.Models.ViewModels;

namespace BudgetTracker.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<CreateExpenseViewModel> PrepareCreateViewModelAsync();
        Task<EditExpenseViewModel> PrepareEditViewModelAsync(int id);
        Task<bool> CreateExpenseAsync(CreateExpenseViewModel model);
        Task<bool> UpdateExpenseAsync(EditExpenseViewModel model);
        Task<ExpenseListViewModel> GetExpenseListAsync(int page = 1, int pageSize = 10);
    }
}