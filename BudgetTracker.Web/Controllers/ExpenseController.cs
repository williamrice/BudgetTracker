using BudgetTracker.Data;
using BudgetTracker.Models;
using Microsoft.AspNetCore.Mvc;

public class ExpenseController : Controller
{
    private readonly IRepository<BudgetedExpense> _expenseRepository;
    private readonly IRepository<ExpenseCategory> _expenseCategoryRepository;

    public ExpenseController(IRepository<BudgetedExpense> expenseRepository, IRepository<ExpenseCategory> expenseCatRepository)
    {
        _expenseRepository = expenseRepository;
        _expenseCategoryRepository = expenseCatRepository;
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

    public IActionResult Create()
    {
        ViewBag.Categories = _expenseCategoryRepository.GetAllAsync().Result;
        if (ViewBag.Categories == null || !ViewBag.Categories.Any())
        {
            return View();
        }
        return View();
    }

    [HttpPost]
    public IActionResult Create(BudgetedExpense expense)
    {
        if (ModelState.IsValid)
        {
            _expenseRepository.AddAsync(expense).Wait();
            return RedirectToAction("Index");
        }
        return View(expense);
    }
}