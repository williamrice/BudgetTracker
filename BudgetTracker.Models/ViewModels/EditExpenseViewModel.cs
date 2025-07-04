using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Models.ViewModels;

public class EditExpenseViewModel : ExpenseFormViewModelBase
{
    public int Id { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
