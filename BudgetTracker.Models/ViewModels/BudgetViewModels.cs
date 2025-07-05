using System.ComponentModel.DataAnnotations;
using BudgetTracker.Models;

namespace BudgetTracker.Models.ViewModels
{
    public class BudgetListViewModel
    {
        public IEnumerable<Budget> Budgets { get; set; } = new List<Budget>();
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal OverallBalance => TotalIncome - TotalExpenses;
        public string CurrentYear { get; set; } = DateTime.Now.Year.ToString();
    }

    public class CreateBudgetViewModel
    {
        [Required]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12")]
        [Display(Name = "Month")]
        public int Month { get; set; } = DateTime.Now.Month;

        [Required]
        [Range(2020, 2050, ErrorMessage = "Year must be between 2020 and 2050")]
        [Display(Name = "Year")]
        public int Year { get; set; } = DateTime.Now.Year;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total income must be a positive value")]
        [Display(Name = "Total Income")]
        [DataType(DataType.Currency)]
        public decimal TotalIncome { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total budgeted expenses must be a positive value")]
        [Display(Name = "Total Budgeted Expenses")]
        [DataType(DataType.Currency)]
        public decimal TotalBudgetedExpenses { get; set; }

        [MaxLength(500)]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // Helper properties
        public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM");
        public decimal RemainingAmount => TotalIncome - TotalBudgetedExpenses;
    }

    public class EditBudgetViewModel
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12")]
        [Display(Name = "Month")]
        public int Month { get; set; }

        [Required]
        [Range(2020, 2050, ErrorMessage = "Year must be between 2020 and 2050")]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total income must be a positive value")]
        [Display(Name = "Total Income")]
        [DataType(DataType.Currency)]
        public decimal TotalIncome { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total budgeted expenses must be a positive value")]
        [Display(Name = "Total Budgeted Expenses")]
        [DataType(DataType.Currency)]
        public decimal TotalBudgetedExpenses { get; set; }

        [MaxLength(500)]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        // Helper properties
        public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM");
        public decimal RemainingAmount => TotalIncome - TotalBudgetedExpenses;
    }

    public class BudgetSummaryViewModel
    {
        public int Id { get; set; }
        public string MonthYear { get; set; } = string.Empty;
        public decimal TotalIncome { get; set; }
        public decimal TotalBudgetedExpenses { get; set; }
        public decimal RemainingAmount { get; set; }
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; }

        // Helper properties
        public string FormattedIncome => TotalIncome.ToString("C");
        public string FormattedExpenses => TotalBudgetedExpenses.ToString("C");
        public string FormattedRemaining => RemainingAmount.ToString("C");
        public bool IsOverBudget => RemainingAmount < 0;
        public string StatusText => IsOverBudget ? "Over Budget" : "Within Budget";
        public string StatusClass => IsOverBudget ? "text-danger" : "text-success";
    }
}
