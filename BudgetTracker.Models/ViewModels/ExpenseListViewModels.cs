// BudgetTracker.Web/Models/ViewModels/ExpenseViewModels.cs
using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Models.ViewModels;

public class ExpenseListViewModel
{
    public List<ExpenseItemViewModel> Expenses { get; set; } = new();

    // Pagination properties
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }

    // Filtering properties
    [Display(Name = "Search")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Category")]
    public int? CategoryFilter { get; set; }

    [Display(Name = "From Date")]
    [DataType(DataType.Date)]
    public DateTime? FromDate { get; set; }

    [Display(Name = "To Date")]
    [DataType(DataType.Date)]
    public DateTime? ToDate { get; set; }

    // Sorting properties
    public string SortBy { get; set; } = "ExpenseDate";
    public string SortDirection { get; set; } = "desc";

    // Summary properties
    [Display(Name = "Total Amount")]
    [DataType(DataType.Currency)]
    public decimal TotalAmount { get; set; }

    [Display(Name = "Average Amount")]
    [DataType(DataType.Currency)]
    public decimal AverageAmount { get; set; }

    // Helper properties for pagination
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    public int PreviousPageNumber => CurrentPage - 1;
    public int NextPageNumber => CurrentPage + 1;

    // Helper method to get page numbers for pagination UI
    public List<int> GetPageNumbers()
    {
        var pages = new List<int>();
        var start = Math.Max(1, CurrentPage - 2);
        var end = Math.Min(TotalPages, CurrentPage + 2);

        for (int i = start; i <= end; i++)
        {
            pages.Add(i);
        }

        return pages;
    }
}

public class ExpenseItemViewModel
{
    public int Id { get; set; }

    [Display(Name = "Expense Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Amount")]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }

    [Display(Name = "Date")]
    [DataType(DataType.Date)]
    public DateTime ExpenseDate { get; set; }

    [Display(Name = "Category")]
    public string CategoryName { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Created")]
    [DataType(DataType.DateTime)]
    public DateTime CreatedDate { get; set; }

    // Helper property for display
    public string FormattedAmount => Amount.ToString("C");
    public string FormattedDate => ExpenseDate.ToString("MM/dd/yyyy");
}