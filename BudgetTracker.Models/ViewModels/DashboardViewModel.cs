using BudgetTracker.Models;

namespace BudgetTracker.Models.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalMonthlyIncome { get; set; }
        public decimal TotalRecurringExpenses { get; set; }
        public decimal AvailableForSavings => TotalMonthlyIncome - TotalRecurringExpenses;
        public decimal ExpenseRatio => TotalMonthlyIncome > 0 ? (TotalRecurringExpenses / TotalMonthlyIncome * 100) : 0;
        public int IncomeSourceCount { get; set; }
        public int RecurringExpenseCount { get; set; }
        public string CurrentMonth { get; set; } = string.Empty;
        public IEnumerable<IncomeSource> IncomeSourcesOverview { get; set; } = new List<IncomeSource>();
        public IEnumerable<BudgetedExpense> RecurringExpensesOverview { get; set; } = new List<BudgetedExpense>();
        public IGrouping<string, BudgetedExpense>[] ExpensesByCategory { get; set; } = Array.Empty<IGrouping<string, BudgetedExpense>>();
        public bool CanAffordNewExpense(decimal newExpenseAmount) => AvailableForSavings >= newExpenseAmount;
        public decimal RecommendedSavingsAmount => AvailableForSavings * 0.2m; // 20% of available amount
    }
}
