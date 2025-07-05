using BudgetTracker.Data;
using BudgetTracker.Models;
using BudgetTracker.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Web.Controllers
{
    [Authorize]
    public class IncomeController : Controller
    {
        private readonly IRepository<IncomeSource> _incomeRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public IncomeController(
            IRepository<IncomeSource> incomeRepository,
            UserManager<ApplicationUser> userManager)
        {
            _incomeRepository = incomeRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var allIncomes = await _incomeRepository.GetAllAsync();
            var userIncomes = allIncomes.Where(i => i.UserId == user.Id).OrderByDescending(i => i.CreatedDate);
            
            var viewModel = new IncomeListViewModel
            {
                Incomes = userIncomes,
                TotalActiveIncome = userIncomes.Where(i => i.IsActive).Sum(i => i.Amount)
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View(new CreateIncomeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateIncomeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var income = new IncomeSource
            {
                Name = model.Name,
                Amount = model.Amount,
                Description = model.Description,
                IsActive = true,
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow
            };

            await _incomeRepository.AddAsync(income);
            await _incomeRepository.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var income = await _incomeRepository.GetByIdAsync(id);
            if (income == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || income.UserId != user.Id)
            {
                return Forbid();
            }

            var viewModel = new EditIncomeViewModel
            {
                Id = income.Id,
                Name = income.Name,
                Amount = income.Amount,
                Description = income.Description,
                IsActive = income.IsActive
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditIncomeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var income = await _incomeRepository.GetByIdAsync(model.Id);
            if (income == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || income.UserId != user.Id)
            {
                return Forbid();
            }

            income.Name = model.Name;
            income.Amount = model.Amount;
            income.Description = model.Description;
            income.IsActive = model.IsActive;

            _incomeRepository.Update(income);
            await _incomeRepository.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var income = await _incomeRepository.GetByIdAsync(id);
            if (income == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || income.UserId != user.Id)
            {
                return Forbid();
            }

            _incomeRepository.Remove(income);
            await _incomeRepository.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var income = await _incomeRepository.GetByIdAsync(id);
            if (income == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || income.UserId != user.Id)
            {
                return Forbid();
            }

            income.IsActive = !income.IsActive;
            _incomeRepository.Update(income);
            await _incomeRepository.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
