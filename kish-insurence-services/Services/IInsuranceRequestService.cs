using kish_insurance_service.DTOs;

namespace kish_insurance_service.Services
{
    public interface IInsuranceRequestService
    {
        Task<InsuranceRequestResponseDto> SubmitInsuranceRequestAsync(InsuranceRequestDTO requestDto);
        Task<ReadInsuranceRequestDto> GetInsuranceRequestByIdAsync(int id);
        Task<List<ReadInsuranceRequestDto>> GetAllInsuranceRequestsAsync();
        Task<PaginatedResultDto<ReadInsuranceRequestDto>> GetPaginatedInsuranceRequestsAsync(
            int pageNumber, int pageSize, string title = null, int? coverageTypeId = null);

    }
}
