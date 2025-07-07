// Create a dedicated seeding service
using BudgetTracker.Data;
using BudgetTracker.Models;
using Microsoft.AspNetCore.Identity;

public class DatabaseSeeder
{
    private readonly BudgetTrackerContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(BudgetTrackerContext context, UserManager<ApplicationUser> userManager, ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            // Ensure database is created
            await _context.Database.EnsureCreatedAsync();
            
            // Seed demo user
            await SeedDemoUser();
            
            // Seed financial data
            await SeedFinancialData();
            
            _logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task SeedDemoUser()
    {
        const string demoEmail = "demo@williamarice.com";
        const string demoPassword = "Password#123!!!";
        
        var demoUser = await _userManager.FindByEmailAsync(demoEmail);
        
        if (demoUser == null)
        {
            demoUser = new ApplicationUser
            {
                UserName = demoEmail,
                Email = demoEmail,
                EmailConfirmed = true,
                FirstName = "Demo",
                LastName = "User"
            };
            
            var result = await _userManager.CreateAsync(demoUser, demoPassword);
            
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create demo user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            
            _logger.LogInformation("Demo user created successfully.");
        }
        else
        {
            _logger.LogInformation("Demo user already exists.");
        }
    }

    private async Task SeedFinancialData()
    {
        var demoUser = await _userManager.FindByEmailAsync("demo@williamarice.com");
        if (demoUser == null) return;
        
        await SeedCategories(demoUser.Id);
        await SeedIncome(demoUser.Id);
        await SeedExpenses(demoUser.Id);
    }

    private async Task SeedCategories(string userId)
    {
        if (!_context.ExpenseCategories.Any(c => c.UserId == userId))
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
            
            _logger.LogInformation("Expense categories seeded successfully.");
        }
    }

    private async Task SeedIncome(string userId)
    {
        if (!_context.IncomeSources.Any(i => i.UserId == userId))
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
            
            _logger.LogInformation("Income sources seeded successfully.");
        }
    }

    private async Task SeedExpenses(string userId)
    {
        if (!_context.BudgetedExpenses.Any(e => e.UserId == userId))
        {
            var foodCategory = _context.ExpenseCategories.First(c => c.Name == "Food & Dining" && c.UserId == userId);
            var transportCategory = _context.ExpenseCategories.First(c => c.Name == "Transportation" && c.UserId == userId);
            var utilitiesCategory = _context.ExpenseCategories.First(c => c.Name == "Utilities" && c.UserId == userId);
            var entertainmentCategory = _context.ExpenseCategories.First(c => c.Name == "Entertainment" && c.UserId == userId);
            var healthcareCategory = _context.ExpenseCategories.First(c => c.Name == "Healthcare" && c.UserId == userId);
            var shoppingCategory = _context.ExpenseCategories.First(c => c.Name == "Shopping" && c.UserId == userId);
            
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
            
            _logger.LogInformation("Budgeted expenses seeded successfully.");
        }
    }
}