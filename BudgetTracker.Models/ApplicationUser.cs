using Microsoft.AspNetCore.Identity;

namespace BudgetTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<IncomeSource> IncomeSources { get; set; } = new List<IncomeSource>();
        public ICollection<ExpenseCategory> ExpenseCategories { get; set; } = new List<ExpenseCategory>();
        public ICollection<BudgetedExpense> BudgetedExpenses { get; set; } = new List<BudgetedExpense>();
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    }
}