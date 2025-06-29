using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetTracker.Models
{
    public class Budget : BaseEntity
    {
        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalIncome { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBudgetedExpenses { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RemainingAmount => TotalIncome - TotalBudgetedExpenses;

        public bool IsActive { get; set; } = true;

        [MaxLength(500)]
        public string? Notes { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public virtual ApplicationUser User { get; set; } = null!;
    }
}