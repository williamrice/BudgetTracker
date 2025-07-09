using BudgetTracker.Data;
using BudgetTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Web.Services
{
    public class DemoService
    {
        private readonly BudgetTrackerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<DemoService> _logger;
        private readonly IConfiguration _configuration;

        public DemoService(
            BudgetTrackerContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DemoService> logger,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<ApplicationUser?> GetAndLoginDemoUserAsync()
        {
            const string demoEmail = "demo@williamarice.com";
            var demoUser = await _userManager.FindByEmailAsync(demoEmail);
            
            if (demoUser != null)
            {
                await _signInManager.SignInAsync(demoUser, isPersistent: false);
                
                SetDemoSessionTracking(demoUser.Id);
                
                _logger.LogInformation("Demo user logged in successfully");
                return demoUser;
            }
            
            _logger.LogWarning("Demo user not found in database");
            return null;
        }

        public async Task ResetDemoDataAsync()
        {
            const string demoEmail = "demo@williamarice.com";
            var demoUser = await _userManager.FindByEmailAsync(demoEmail);
            
            if (demoUser == null)
            {
                _logger.LogWarning("Demo user not found for reset");
                return;
            }

            try
            {
                // Start transaction
                using var transaction = await _context.Database.BeginTransactionAsync();

                // Clear existing demo data
                await ClearDemoUserDataAsync(demoUser.Id);

                // Re-seed the demo data
                await SeedDemoDataAsync(demoUser.Id);

                await transaction.CommitAsync();
                
                _logger.LogInformation("Demo data reset successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while resetting demo data");
                throw;
            }
        }

        private async Task ClearDemoUserDataAsync(string userId)
        {
            // Remove existing data for demo user
            var expenses = _context.BudgetedExpenses.Where(e => e.UserId == userId);
            var incomes = _context.IncomeSources.Where(i => i.UserId == userId);
            var categories = _context.ExpenseCategories.Where(c => c.UserId == userId);
            var budgets = _context.Budgets.Where(b => b.UserId == userId);

            _context.BudgetedExpenses.RemoveRange(expenses);
            _context.IncomeSources.RemoveRange(incomes);
            _context.ExpenseCategories.RemoveRange(categories);
            _context.Budgets.RemoveRange(budgets);

            await _context.SaveChangesAsync();
        }

        private async Task SeedDemoDataAsync(string userId)
        {
            await SeedCategoriesAsync(userId);
            await SeedIncomeAsync(userId);
            await SeedExpensesAsync(userId);
        }

        private async Task SeedCategoriesAsync(string userId)
        {
            var categories = new List<ExpenseCategory>
            {
                new ExpenseCategory 
                { 
                    Name = "Food & Dining", 
                    Description = "Groceries, restaurants, and dining out",
                    MaxAmount = 800m,
                    Color = "#FF6B6B",
                    UserId = userId 
                },
                new ExpenseCategory 
                { 
                    Name = "Transportation", 
                    Description = "Gas, car payments, public transit",
                    MaxAmount = 500m,
                    Color = "#4ECDC4",
                    UserId = userId 
                },
                new ExpenseCategory 
                { 
                    Name = "Utilities", 
                    Description = "Electric, water, gas, internet",
                    MaxAmount = 300m,
                    Color = "#45B7D1",
                    UserId = userId 
                },
                new ExpenseCategory 
                { 
                    Name = "Entertainment", 
                    Description = "Movies, games, subscriptions",
                    MaxAmount = 200m,
                    Color = "#96CEB4",
                    UserId = userId 
                },
                new ExpenseCategory 
                { 
                    Name = "Healthcare", 
                    Description = "Medical expenses, insurance",
                    MaxAmount = 400m,
                    Color = "#FFEAA7",
                    UserId = userId 
                },
                new ExpenseCategory 
                { 
                    Name = "Shopping", 
                    Description = "Clothing, household items",
                    MaxAmount = 300m,
                    Color = "#DDA0DD",
                    UserId = userId 
                }
            };
            
            _context.ExpenseCategories.AddRange(categories);
            await _context.SaveChangesAsync();
        }

        private async Task SeedIncomeAsync(string userId)
        {
            var incomeSources = new List<IncomeSource>
            {
                new IncomeSource 
                { 
                    Name = "Primary Job Salary", 
                    Amount = 5000m,
                    Description = "Monthly salary from full-time employment",
                    UserId = userId 
                },
                new IncomeSource 
                { 
                    Name = "Freelance Work", 
                    Amount = 1500m,
                    Description = "Monthly income from freelance projects",
                    UserId = userId 
                },
                new IncomeSource 
                { 
                    Name = "Investment Returns", 
                    Amount = 300m,
                    Description = "Monthly returns from investments and dividends",
                    UserId = userId 
                },
                new IncomeSource 
                { 
                    Name = "Side Business", 
                    Amount = 800m,
                    Description = "Monthly income from side business",
                    UserId = userId 
                }
            };
            
            _context.IncomeSources.AddRange(incomeSources);
            await _context.SaveChangesAsync();
        }

        private async Task SeedExpensesAsync(string userId)
        {
            var foodCategory = await _context.ExpenseCategories.FirstAsync(c => c.Name == "Food & Dining" && c.UserId == userId);
            var transportCategory = await _context.ExpenseCategories.FirstAsync(c => c.Name == "Transportation" && c.UserId == userId);
            var utilitiesCategory = await _context.ExpenseCategories.FirstAsync(c => c.Name == "Utilities" && c.UserId == userId);
            var entertainmentCategory = await _context.ExpenseCategories.FirstAsync(c => c.Name == "Entertainment" && c.UserId == userId);
            var healthcareCategory = await _context.ExpenseCategories.FirstAsync(c => c.Name == "Healthcare" && c.UserId == userId);
            var shoppingCategory = await _context.ExpenseCategories.FirstAsync(c => c.Name == "Shopping" && c.UserId == userId);
            
            var budgetedExpenses = new List<BudgetedExpense>
            {
                new BudgetedExpense 
                { 
                    Name = "Groceries", 
                    BudgetedAmount = 600m,
                    Description = "Monthly grocery shopping",
                    CategoryId = foodCategory.Id,
                    UserId = userId 
                },
                new BudgetedExpense 
                { 
                    Name = "Restaurants", 
                    BudgetedAmount = 200m,
                    Description = "Dining out and takeout",
                    CategoryId = foodCategory.Id,
                    UserId = userId 
                },
                new BudgetedExpense 
                { 
                    Name = "Gas", 
                    BudgetedAmount = 150m,
                    Description = "Monthly fuel expenses",
                    CategoryId = transportCategory.Id,
                    UserId = userId 
                },
                new BudgetedExpense 
                { 
                    Name = "Car Payment", 
                    BudgetedAmount = 350m,
                    Description = "Monthly car loan payment",
                    CategoryId = transportCategory.Id,
                    UserId = userId 
                },
                new BudgetedExpense 
                { 
                    Name = "Electric Bill", 
                    BudgetedAmount = 120m,
                    Description = "Monthly electricity bill",
                    CategoryId = utilitiesCategory.Id,
                    UserId = userId 
                },
                new BudgetedExpense 
                { 
                    Name = "Internet", 
                    BudgetedAmount = 80m,
                    Description = "Monthly internet service",
                    CategoryId = utilitiesCategory.Id,
                    UserId = userId 
                },
                new BudgetedExpense 
                { 
                    Name = "Water & Sewer", 
                    BudgetedAmount = 65m,
                    Description = "Monthly water and sewer bill",
                    CategoryId = utilitiesCategory.Id,
                    UserId = userId 
                },
                new BudgetedExpense 
                { 
                    Name = "Streaming Services", 
                    BudgetedAmount = 45m,
                    Description = "Netflix, Spotify, etc.",
                    CategoryId = entertainmentCategory.Id,
                    UserId = userId 
                },
                new BudgetedExpense 
                { 
                    Name = "Movies & Events", 
                    BudgetedAmount = 100m,
                    Description = "Monthly entertainment budget",
                    CategoryId = entertainmentCategory.Id,
                    UserId = userId 
                },
                new BudgetedExpense 
                { 
                    Name = "Health Insurance", 
                    BudgetedAmount = 250m,
                    Description = "Monthly health insurance premium",
                    CategoryId = healthcareCategory.Id,
                    UserId = userId 
                },
                new BudgetedExpense 
                { 
                    Name = "Clothing", 
                    BudgetedAmount = 150m,
                    Description = "Monthly clothing budget",
                    CategoryId = shoppingCategory.Id,
                    UserId = userId 
                }
            };
            
            _context.BudgetedExpenses.AddRange(budgetedExpenses);
            await _context.SaveChangesAsync();
        }

        private void SetDemoSessionTracking(string userId)
        {
            // This could be enhanced with a database table to track demo sessions
            // For now, we'll use a simple approach
            _logger.LogInformation($"Demo session started for user {userId} at {DateTime.UtcNow}");
        }
    }
}
