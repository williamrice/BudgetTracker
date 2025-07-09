using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BudgetTracker.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BudgetTracker.Models;
using BudgetTracker.Data;
using BudgetTracker.Web.Services;

namespace BudgetTracker.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepository<IncomeSource> _incomeRepository;
    private readonly IRepository<BudgetedExpense> _expenseRepository;
    private readonly IRepository<ExpenseCategory> _categoryRepository;
    private readonly IRepository<Budget> _budgetRepository;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly DemoService _demoService;

    public HomeController(
        ILogger<HomeController> logger,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IRepository<IncomeSource> incomeRepository,
        IRepository<BudgetedExpense> expenseRepository,
        IRepository<ExpenseCategory> categoryRepository,
        IRepository<Budget> budgetRepository,
        DemoService demoService)
    {
        _logger = logger;
        _userManager = userManager;
        _incomeRepository = incomeRepository;
        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
        _budgetRepository = budgetRepository;
        _signInManager = signInManager;
        _demoService = demoService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(bool demo = false)
    {
        ApplicationUser? user = null;

        if (demo)
        {
            user = await _demoService.GetAndLoginDemoUserAsync();
        }
        else
        {
            // For non-demo access, require authentication
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return Challenge();
            }
            user = await _userManager.GetUserAsync(User);
        }

        if (user == null)
        {
            return BadRequest("User not found");
        }

        var incomes = await _incomeRepository.GetAllAsync();
        var expenses = await _expenseRepository.GetAllAsync();
        var categories = await _categoryRepository.GetAllAsync();

        var userIncomes = incomes.Where(i => i.UserId == user.Id && i.IsActive);
        var userExpenses = expenses.Where(e => e.UserId == user.Id && e.IsActive);
        var userCategories = categories.Where(c => c.UserId == user.Id);

        var expensesByCategory = userExpenses
            .GroupBy(e => userCategories.FirstOrDefault(c => c.Id == e.CategoryId)?.Name ?? "Uncategorized")
            .ToArray();

        var viewModel = new DashboardViewModel
        {
            TotalMonthlyIncome = userIncomes.Sum(i => i.Amount),
            TotalRecurringExpenses = userExpenses.Sum(e => e.BudgetedAmount),
            IncomeSourceCount = userIncomes.Count(),
            RecurringExpenseCount = userExpenses.Count(),
            IncomeSourcesOverview = userIncomes.OrderByDescending(i => i.Amount).Take(5),
            RecurringExpensesOverview = userExpenses.OrderByDescending(e => e.BudgetedAmount).Take(5),
            ExpensesByCategory = expensesByCategory,
            CurrentMonth = DateTime.Now.ToString("MMMM yyyy")
        };

        return View("Dashboard", viewModel);
    }

    [AllowAnonymous]
    public IActionResult Welcome()
    {
        return View("Login");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [AllowAnonymous]
    public async Task<IActionResult> ResetDemo()
    {
        try
        {
            await _demoService.ResetDemoDataAsync();
            TempData["Success"] = "Demo data has been reset successfully!";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while resetting demo data");
            TempData["Error"] = "An error occurred while resetting the demo data. Please try again.";
        }

        return RedirectToAction("Index", new { demo = true });
    }

    [AllowAnonymous]
    public IActionResult Demo()
    {
        return RedirectToAction("Index", new { demo = true });
    }

    [AllowAnonymous]
    public IActionResult DemoInfo()
    {
        return View();
    }
}
