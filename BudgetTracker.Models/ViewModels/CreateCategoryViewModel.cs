using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Models.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
