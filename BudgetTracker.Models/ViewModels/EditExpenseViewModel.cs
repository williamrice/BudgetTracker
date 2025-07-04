using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Models.ViewModels;

public class EditExpenseViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Expense name is required")]
    [StringLength(100, ErrorMessage = "Expense name cannot exceed 100 characters")]
    [Display(Name = "Expense Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    [Display(Name = "Amount")]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Expense date is required")]
    [Display(Name = "Expense Date")]
    [DataType(DataType.Date)]
    public DateTime ExpenseDate { get; set; }

    [Required(ErrorMessage = "Category is required")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Display(Name = "Description")]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    public List<SelectListItem> Categories { get; set; } = new();

    [Display(Name = "Created Date")]
    public DateTime? CreatedDate { get; set; }

    [Display(Name = "Last Modified")]
    public DateTime? ModifiedDate { get; set; }
}
