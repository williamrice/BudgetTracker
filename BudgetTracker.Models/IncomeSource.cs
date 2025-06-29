using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetTracker.Models
{
    public class IncomeSource : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public virtual ApplicationUser User { get; set; } = null!;
    }
}