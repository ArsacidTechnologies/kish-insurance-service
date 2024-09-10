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
        public int CoverageTypeId { get; set; } // change CoverageTypeId to clearly indicate the foreign key

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Capital must be greater than 0")]
        public decimal Capital { get; set; }

        [ForeignKey("InsuranceRequest")]
        public int InsuranceRequestId { get; set; }

        public InsuranceRequest InsuranceRequest { get; set; }
        public CoverageType CoverageType { get; set; }
    }
}