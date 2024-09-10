namespace kish_insurance_service.DTOs
{
    public class InsuranceRequestDTO
    {
        public string Title { get; set; }
        public List<CoverageDTO> Coverages { get; set; }
    }
}