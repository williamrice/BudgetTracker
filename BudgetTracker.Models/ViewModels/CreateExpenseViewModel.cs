using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetTracker.Models.ViewModels;

public class CreateExpenseViewModel
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, 1000000, ErrorMessage = "Amount must be greater than 0.")]
    public decimal Amount { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime ExpenseDate { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public List<SelectListItem> Categories { get; set; } = new();
}