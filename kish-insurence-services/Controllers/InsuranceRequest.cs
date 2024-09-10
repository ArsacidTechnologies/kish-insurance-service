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
                // Return 201 Created with a location header pointing to the created resource
                return CreatedAtAction(nameof(GetInsuranceRequestById), new { id = requestId }, new { message = "Insurance request submitted successfully", requestId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
