using BudgetTracker.Models.ViewModels;
using BudgetTracker.Models;
using BudgetTracker.Data;
using BudgetTracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace BudgetTracker.Services
{

    public class ExpenseService : IExpenseService
    {

        public ExpenseListViewModel PrepareListViewModel(IEnumerable<BudgetedExpense> expenses, IEnumerable<ExpenseCategory> categories)
        {
            var expenseList = expenses.Select(e => new ExpenseItemViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Amount = e.BudgetedAmount,
                CategoryName = categories.FirstOrDefault(c => c.Id == e.CategoryId)?.Name ?? string.Empty
            }).ToList();
            return new ExpenseListViewModel
            {
                Expenses = expenseList
            };
        }

        public CreateExpenseViewModel PrepareCreateViewModel(IEnumerable<ExpenseCategory> categories)
        {
            return new CreateExpenseViewModel
            {
                Categories = categories?.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList() ?? new List<SelectListItem>(),
            };
        }

        public EditExpenseViewModel? PrepareEditViewModel(BudgetedExpense? expense, IEnumerable<ExpenseCategory> categories)
        {
            if (expense == null) return null;
            return new EditExpenseViewModel
            {
                Id = expense.Id,
                Name = expense.Name,
                Description = expense.Description,
                Amount = expense.BudgetedAmount,
                CategoryId = expense.CategoryId,
                Categories = categories?.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == expense.CategoryId
                }).ToList() ?? new List<SelectListItem>()
            };
        }
        public BudgetedExpense PrepareExpenseFromViewModel(CreateExpenseViewModel model, ApplicationUser user)
        {
            return new BudgetedExpense
            {
                Name = model.Name,
                Description = model.Description,
                BudgetedAmount = model.Amount,
                CategoryId = model.CategoryId,
                UserId = user.Id
            };
        }
    }
}


