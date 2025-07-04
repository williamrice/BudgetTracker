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

    public IActionResult Index()
    {
        var expenses = _expenseRepository.GetAllAsync().Result;
        if (expenses == null || !expenses.Any())
        {
            return View();
        }
        return View(expenses);
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
}