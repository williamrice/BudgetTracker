using Microsoft.EntityFrameworkCore;
using BudgetTracker.Models;

namespace BudgetTracker.Data
{
    public class BudgetTrackerContext : DbContext
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