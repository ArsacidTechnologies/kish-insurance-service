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

        // POST: api/InsuranceRequest/submit-request
        [HttpPost("submit-request")]
        public async Task<IActionResult> SubmitInsuranceRequest([FromBody] InsuranceRequestDTO requestDto)
        {
            try
            {
                // Call the service to submit the insurance request
                var response = await _insuranceRequestService.SubmitInsuranceRequestAsync(requestDto);

                // Return a 201 Created response with the RequestId, TotalPremium, and UserMessage
                return StatusCode(201, new
                {
                    message = "Insurance request submitted successfully",
                    response.RequestId,
                    response.TotalPremium,
                    UserMessage = $"درخواست شما با موفقیت ثبت شد  میزان پوشش {response.TotalPremium}"
                });
            }
            catch (Exception ex)
            {
                // If any error occurs, return a 400 Bad Request with the error message
                return BadRequest(new { message = ex.Message });
            }
        }
        // GET: api/InsuranceRequest/requests
        [HttpGet("requests")]
        public async Task<IActionResult> GetInsuranceRequests(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string title = null,
            [FromQuery] int? coverageTypeId = null)
        {
            var result = await _insuranceRequestService.GetPaginatedInsuranceRequestsAsync(pageNumber, pageSize, title, coverageTypeId);
            return Ok(result);
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

        // GET: api/InsuranceRequest/all
        [HttpGet("all")]
        public async Task<ActionResult<List<ReadInsuranceRequestDto>>> GetAllInsuranceRequests()
        {
            var insuranceRequests = await _insuranceRequestService.GetAllInsuranceRequestsAsync();
            return Ok(insuranceRequests);
        }

    }
}
