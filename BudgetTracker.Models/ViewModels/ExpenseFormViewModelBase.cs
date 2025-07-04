using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetTracker.Models.ViewModels
{
    public abstract class ExpenseFormViewModelBase
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; } = new();
    }
}
