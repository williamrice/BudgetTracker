using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetTracker.Models
{
    public class ExpenseCategory : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxAmount { get; set; }

        [MaxLength(7)]
        public string Color { get; set; } = "#1e3c72";

        public bool IsActive { get; set; } = true;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<BudgetedExpense> BudgetedExpenses { get; set; } = new List<BudgetedExpense>();
    }
}