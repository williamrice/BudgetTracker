using Microsoft.EntityFrameworkCore;
using BudgetTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BudgetTracker.Data
{
    public class BudgetTrackerContext : IdentityDbContext<ApplicationUser>
    {
        public BudgetTrackerContext(DbContextOptions<BudgetTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetedExpense> BudgetedExpenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<IncomeSource> IncomeSources { get; set; }
    }
}