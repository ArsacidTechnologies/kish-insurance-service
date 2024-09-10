using kish_insurance_service.DTOs;

namespace kish_insurance_service.Services
{
    public interface IInsuranceRequestService
    {
        Task<int> SubmitInsuranceRequestAsync(InsuranceRequestDTO requestDto);
        Task<ReadInsuranceRequestDto> GetInsuranceRequestByIdAsync(int id);
        Task<List<ReadInsuranceRequestDto>> GetAllInsuranceRequestsAsync();

    }
}
