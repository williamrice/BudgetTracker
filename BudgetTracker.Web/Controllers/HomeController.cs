using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BudgetTracker.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BudgetTracker.Models;
using BudgetTracker.Data;

namespace BudgetTracker.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepository<IncomeSource> _incomeRepository;
    private readonly IRepository<BudgetedExpense> _expenseRepository;
    private readonly IRepository<Budget> _budgetRepository;

    public HomeController(
        ILogger<HomeController> logger,
        UserManager<ApplicationUser> userManager,
        IRepository<IncomeSource> incomeRepository,
        IRepository<BudgetedExpense> expenseRepository,
        IRepository<Budget> budgetRepository)
    {
        _logger = logger;
        _userManager = userManager;
        _incomeRepository = incomeRepository;
        _expenseRepository = expenseRepository;
        _budgetRepository = budgetRepository;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        var incomes = await _incomeRepository.GetAllAsync();
        var expenses = await _expenseRepository.GetAllAsync();
        var budgets = await _budgetRepository.GetAllAsync();

        var userIncomes = incomes.Where(i => i.UserId == user.Id && i.IsActive);
        var userExpenses = expenses.Where(e => e.UserId == user.Id && e.IsActive);
        var userBudgets = budgets.Where(b => b.UserId == user.Id && b.IsActive);

        var viewModel = new DashboardViewModel
        {
            TotalIncome = userIncomes.Sum(i => i.Amount),
            TotalExpenses = userExpenses.Sum(e => e.BudgetedAmount),
            IncomeCount = userIncomes.Count(),
            ExpenseCount = userExpenses.Count(),
            RecentIncomes = userIncomes.OrderByDescending(i => i.CreatedDate).Take(5),
            RecentExpenses = userExpenses.OrderByDescending(e => e.CreatedDate).Take(5),
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
}
