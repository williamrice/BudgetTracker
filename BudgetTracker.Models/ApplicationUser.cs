using Microsoft.AspNetCore.Identity;

namespace BudgetTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual ICollection<IncomeSource> IncomeSources { get; set; } = new List<IncomeSource>();
        public virtual ICollection<ExpenseCategory> ExpenseCategories { get; set; } = new List<ExpenseCategory>();
        public virtual ICollection<BudgetedExpense> BudgetedExpenses { get; set; } = new List<BudgetedExpense>();
        public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    }
}