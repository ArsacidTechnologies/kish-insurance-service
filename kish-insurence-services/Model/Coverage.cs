using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kish_insurance_service.Models
{
    public class Coverage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("CoverageType")]
        public int Type { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Capital must be greater than 0")]
        public decimal Capital { get; set; }

        // Foreign key to InsuranceRequest
        [ForeignKey("InsuranceRequest")]
        public int InsuranceRequestId { get; set; }

        // Navigation properties
        public InsuranceRequest InsuranceRequest { get; set; }
        public CoverageType CoverageType { get; set; }
    }
}
