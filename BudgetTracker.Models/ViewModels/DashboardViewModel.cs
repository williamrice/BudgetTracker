using BudgetTracker.Models;

namespace BudgetTracker.Models.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetAmount => TotalIncome - TotalExpenses;
        public int IncomeCount { get; set; }
        public int ExpenseCount { get; set; }
        public string CurrentMonth { get; set; } = string.Empty;
        public IEnumerable<IncomeSource> RecentIncomes { get; set; } = new List<IncomeSource>();
        public IEnumerable<BudgetedExpense> RecentExpenses { get; set; } = new List<BudgetedExpense>();
    }
}
