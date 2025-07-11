using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Models.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Budget Limit")]
        public decimal? MaxAmount { get; set; }

        [Required]
        [Display(Name = "Color")]
        public string Color { get; set; } = "#1e3c72";
    }
}
