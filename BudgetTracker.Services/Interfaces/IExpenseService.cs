
using BudgetTracker.Models.ViewModels;
using BudgetTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace BudgetTracker.Services.Interfaces
{
    public interface IExpenseService
    {
        CreateExpenseViewModel PrepareCreateViewModel(IEnumerable<ExpenseCategory> categories);
        EditExpenseViewModel? PrepareEditViewModel(BudgetedExpense? expense, IEnumerable<ExpenseCategory> categories);
        ExpenseListViewModel PrepareListViewModel(IEnumerable<BudgetedExpense> expenses, IEnumerable<ExpenseCategory> categories);
        BudgetedExpense PrepareExpenseFromViewModel(CreateExpenseViewModel model, ApplicationUser user);
    }
}