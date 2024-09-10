using kish_insurance_service.DTOs;
using kish_insurance_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace kish_insurance_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceRequestController : ControllerBase
    {
        private readonly IInsuranceRequestService _insuranceRequestService;

        public InsuranceRequestController(IInsuranceRequestService insuranceRequestService)
        {
            _insuranceRequestService = insuranceRequestService;
        }

        [HttpPost("submit-request")]
        public async Task<IActionResult> SubmitInsuranceRequest([FromBody] InsuranceRequestDTO requestDto)
        {
            try
            {
                var requestId = await _insuranceRequestService.SubmitInsuranceRequestAsync(requestDto);
                return Ok(new { message = "Insurance request submitted successfully", requestId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
