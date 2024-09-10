namespace kish_insurance_service.DTOs
{
    public class CoverageDTO
    {
        public int CoverageTypeId { get; set; }
        public decimal Capital { get; set; }
    }
    public class ReadCoverageDto
    {
        public int Id { get; set; }
        public decimal Capital { get; set; }
        public string CoverageType { get; set; }
    }


}
