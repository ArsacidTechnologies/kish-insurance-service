using kish_insurance_service.DTOs;

namespace kish_insurance_service.Services
{
    public interface IInsuranceRequestService
    {
        Task<int> SubmitInsuranceRequestAsync(InsuranceRequestDTO requestDto);
    }
}
