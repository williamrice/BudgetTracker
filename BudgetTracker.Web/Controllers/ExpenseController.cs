using BudgetTracker.Data;
using BudgetTracker.Models;
using BudgetTracker.Models.ViewModels;
using BudgetTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class ExpenseController : Controller
{
    private readonly IRepository<BudgetedExpense> _expenseRepository;
    private readonly IRepository<ExpenseCategory> _expenseCategoryRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IExpenseService _expenseService;

    public ExpenseController(
        IRepository<BudgetedExpense> expenseRepository,
        IRepository<ExpenseCategory> expenseCatRepository,
        UserManager<ApplicationUser> userManager,
        IExpenseService expenseService)
    {
        _expenseRepository = expenseRepository;
        _expenseCategoryRepository = expenseCatRepository;
        _userManager = userManager;
        _expenseService = expenseService;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var allExpenses = await _expenseRepository.GetAllAsync();
        var userExpenses = allExpenses.Where(e => e.UserId == user.Id);
        
        var allCategories = await _expenseCategoryRepository.GetAllAsync();
        var userCategories = allCategories.Where(c => c.UserId == user.Id);
        
        var model = _expenseService.PrepareListViewModel(userExpenses, userCategories);
        return View(model);
    }

    public async Task<IActionResult> Create()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var allCategories = await _expenseCategoryRepository.GetAllAsync();
        var userCategories = allCategories.Where(c => c.UserId == user.Id);
        var model = _expenseService.PrepareCreateViewModel(userCategories);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExpenseViewModel model)
    {
        if (!ModelState.IsValid)
        {
            RedirectToAction("Create");
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }
        var expense = _expenseService.PrepareExpenseFromViewModel(model, user);

        await _expenseRepository.AddAsync(expense);
        await _expenseRepository.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        var categories = await _expenseCategoryRepository.GetAllAsync();
        var model = _expenseService.PrepareEditViewModel(expense, categories);
        if (model == null)
        {
            return NotFound();
        }
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(EditExpenseViewModel model)
    {
        if (!ModelState.IsValid)
        {
            RedirectToAction("Edit", new { id = model.Id });
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