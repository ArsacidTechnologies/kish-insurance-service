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
        //POST: api/submit-request
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

        // GET: api/InsuranceRequest
        [HttpGet]
        public async Task<ActionResult<List<ReadInsuranceRequestDto>>> GetAllInsuranceRequests()
        {
            var insuranceRequests = await _insuranceRequestService.GetAllInsuranceRequestsAsync();
            return Ok(insuranceRequests);
        }

        // GET: api/insurance-request/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadInsuranceRequestDto>> GetInsuranceRequestById(int id)
        {
            var insuranceRequest = await _insuranceRequestService.GetInsuranceRequestByIdAsync(id);

            if (insuranceRequest == null)
            {
                return NotFound();
            }

            return Ok(insuranceRequest);
        }



    }
}
