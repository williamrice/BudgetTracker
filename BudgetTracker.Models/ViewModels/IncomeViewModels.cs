using System.ComponentModel.DataAnnotations;
using BudgetTracker.Models;

namespace BudgetTracker.Models.ViewModels
{
    public class IncomeListViewModel
    {
        public IEnumerable<IncomeSource> Incomes { get; set; } = new List<IncomeSource>();
        public decimal TotalActiveIncome { get; set; }
    }

    public class CreateIncomeViewModel
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Income Source Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        [Display(Name = "Monthly Amount")]
        public decimal Amount { get; set; }

        [MaxLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }
    }

    public class EditIncomeViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Income Source Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        [Display(Name = "Monthly Amount")]
        public decimal Amount { get; set; }

        [MaxLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }
}
