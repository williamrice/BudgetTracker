namespace BudgetTracker.Models.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalBudgetedExpenses { get; set; }
        public decimal RemainingAmount { get; set; }
        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }

        public List<ExpenseCategoryViewModel> CategorySummaries { get; set; } = new();
        public List<IncomeSource> IncomeSources { get; set; } = new();
    }

    public class ExpenseCategoryViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal TotalBudgeted { get; set; }
        public decimal? MaxAmount { get; set; }
        public int ExpenseCount { get; set; }
        public decimal UtilizationPercentage => MaxAmount > 0 ? (TotalBudgeted / MaxAmount.Value) * 100 : 0;
    }
}