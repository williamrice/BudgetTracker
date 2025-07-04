using BudgetTracker.Data;
using BudgetTracker.Models;
using BudgetTracker.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class ExpenseController : Controller
{
    private readonly IRepository<BudgetedExpense> _expenseRepository;
    private readonly IRepository<ExpenseCategory> _expenseCategoryRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ExpenseController(IRepository<BudgetedExpense> expenseRepository, IRepository<ExpenseCategory> expenseCatRepository, UserManager<ApplicationUser> userManager)
    {
        _expenseRepository = expenseRepository;
        _expenseCategoryRepository = expenseCatRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var expenses = await _expenseRepository.GetAllAsync();
        var categories = await _expenseCategoryRepository.GetAllAsync();
        var expenseList = expenses.Select(e => new ExpenseItemViewModel
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            Amount = e.BudgetedAmount,
            CategoryName = categories.FirstOrDefault(c => c.Id == e.CategoryId)?.Name ?? "",
        }).ToList();
        var model = new ExpenseListViewModel
        {
            Expenses = expenseList
        };
        return View(model);
    }

    public async Task<IActionResult> Create()
    {
        var categories = await _expenseCategoryRepository.GetAllAsync();
        var model = new CreateExpenseViewModel
        {
            Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList(),
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExpenseViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _expenseCategoryRepository.GetAllAsync();
            model.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);

        var expense = new BudgetedExpense
        {
            Name = model.Name,
            Description = model.Description,
            BudgetedAmount = model.Amount,
            CategoryId = model.CategoryId,
            CreatedDate = DateTime.UtcNow,
            UserId = user.Id
        };

        await _expenseRepository.AddAsync(expense);
        await _expenseRepository.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        if (expense == null)
        {
            return NotFound();
        }

        var categories = await _expenseCategoryRepository.GetAllAsync();
        var model = new EditExpenseViewModel
        {
            Id = expense.Id,
            Name = expense.Name,
            Description = expense.Description,
            Amount = expense.BudgetedAmount,
            CategoryId = expense.CategoryId,
            Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList(),
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(EditExpenseViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _expenseCategoryRepository.GetAllAsync();
            model.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            return View(model);
        }

        var expense = await _expenseRepository.GetByIdAsync(model.Id);
        if (expense == null)
        {
            return NotFound();
        }

        expense.Name = model.Name;
        expense.Description = model.Description;
        expense.BudgetedAmount = model.Amount;
        expense.CategoryId = model.CategoryId;

        _expenseRepository.Update(expense);
        await _expenseRepository.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        if (expense == null)
        {
            return NotFound();
        }

        _expenseRepository.Remove(expense);
        await _expenseRepository.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}