using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetTracker.Models
{
    public class BudgetedExpense : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Expense Name")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Monthly Amount")]
        public decimal BudgetedAmount { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ExpenseCategory Category { get; set; } = null!;
    }
}