using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace kish_insurance_service.Models
{
    public class InsuranceRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        // Navigation property for the related Coverages
        public ICollection<Coverage> Coverages { get; set; }
    }
}
