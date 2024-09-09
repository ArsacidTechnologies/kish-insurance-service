using System.ComponentModel.DataAnnotations;

namespace kish_insurance_service.Models
{
    public class CoverageType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [Range(0.0001, 1.0000, ErrorMessage = "Premium rate must be between 0.0001 and 1.0000")]
        public decimal PremiumRate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Minimum capital must be greater than 0")]
        public decimal MinCapital { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Maximum capital must be greater than 0")]
        public decimal MaxCapital { get; set; }
    }
}
