using BudgetTracker.Models.ViewModels;
using BudgetTracker.Models;
using BudgetTracker.Data;
using BudgetTracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetTracker.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IRepository<BudgetedExpense> _expenseRepository;
        private readonly IRepository<ExpenseCategory> _categoryRepository;

        public ExpenseService(
            IRepository<BudgetedExpense> expenseRepository,
            IRepository<ExpenseCategory> categoryRepository)
        {
            _expenseRepository = expenseRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<CreateExpenseViewModel> PrepareCreateViewModelAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return new CreateExpenseViewModel
            {
                Categories = categories?.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList() ?? new List<SelectListItem>(),
                ExpenseDate = DateTime.Today
            };
        }

        public async Task<EditExpenseViewModel> PrepareEditViewModelAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null) return null;

            var categories = await _categoryRepository.GetAllAsync();

            return new EditExpenseViewModel
            {
                Id = expense.Id,
                Name = expense.Name,
                Amount = expense.BudgetedAmount,
                ExpenseDate = expense.CreatedDate,
                CategoryId = expense.CategoryId,
                Categories = categories?.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == expense.CategoryId
                }).ToList() ?? new List<SelectListItem>()
            };
        }

        public async Task<bool> CreateExpenseAsync(CreateExpenseViewModel model)
        {
            var expense = new BudgetedExpense
            {
                Name = model.Name,
                BudgetedAmount = model.Amount,
                Description = model.Description,
                CategoryId = model.CategoryId,
                UserId = model.UserId,
                IsActive = true
            };

            await _expenseRepository.AddAsync(expense);
            var result = await _expenseRepository.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateExpenseAsync(EditExpenseViewModel model)
        {
            var expense = await _expenseRepository.GetByIdAsync(model.Id);
            if (expense == null) return false;

            expense.Name = model.Name;
            expense.BudgetedAmount = model.Amount;
            expense.Description = model.Description;
            expense.CategoryId = model.CategoryId;

            _expenseRepository.Update(expense);
            var result = await _expenseRepository.SaveChangesAsync();
            return result > 0;
        }

        public async Task<ExpenseListViewModel> GetExpenseListAsync(int page = 1, int pageSize = 10)
        {
            var expenses = await _expenseRepository.GetPagedAsync(page, pageSize);
            var totalCount = await _expenseRepository.GetCountAsync();

            return new ExpenseListViewModel
            {
                Expenses = expenses.Select(e => new ExpenseItemViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Amount = e.Amount,
                    ExpenseDate = e.ExpenseDate,
                    CategoryName = e.Category?.Name
                }).ToList(),
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }
    }
}