namespace kish_insurance_service.DTOs
{
    public class InsuranceRequestDTO
    {
        public string Title { get; set; }
        public List<CoverageDTO> Coverages { get; set; }
    }
    public class ReadInsuranceRequestDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<CoverageDTO> Coverages { get; set; }
    }
}