// Services/InsuranceRequestService.cs
using AutoMapper;
using kish_insurance_service.DTOs;
using kish_insurance_service.Models;
using Microsoft.EntityFrameworkCore;

namespace kish_insurance_service.Services
{
    public class InsuranceRequestService : IInsuranceRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public InsuranceRequestService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //submit Insurance request
        public async Task<int> SubmitInsuranceRequestAsync(InsuranceRequestDTO requestDto)
        {
            // Create a new InsuranceRequest object using AutoMapper
            var insuranceRequest = _mapper.Map<InsuranceRequest>(requestDto);

            // Validate and process each coverage
            foreach (var coverage in insuranceRequest.Coverages)
            {

                var coverageType = await _context.CoverageTypes.FindAsync(coverage.CoverageTypeId);
                if (coverageType == null)
                {
                    throw new Exception($"CoverageType with ID {coverage.CoverageTypeId} not found.");
                }

                // Ensure capital is within the valid range
                if (coverage.Capital < coverageType.MinCapital || coverage.Capital > coverageType.MaxCapital)
                {
                    throw new Exception($"Capital for {coverageType.Name} should be between {coverageType.MinCapital} and {coverageType.MaxCapital}.");
                }
            }

            // Save the request to the database
            _context.InsuranceRequests.Add(insuranceRequest);
            await _context.SaveChangesAsync();

            return insuranceRequest.Id;
        }

        // Get all insurance requests
        public async Task<List<ReadInsuranceRequestDto>> GetAllInsuranceRequestsAsync()
        {
            var insuranceRequests = await _context.InsuranceRequests
                                                  .Include(r => r.Coverages)
                                                  .ThenInclude(c => c.CoverageType)
                                                  .ToListAsync();

            return _mapper.Map<List<ReadInsuranceRequestDto>>(insuranceRequests);
        }

        // Get insurance request by ID
        public async Task<ReadInsuranceRequestDto> GetInsuranceRequestByIdAsync(int id)
        {
            var insuranceRequest = await _context.InsuranceRequests
                                                 .Include(r => r.Coverages)
                                                 .ThenInclude(c => c.CoverageType)
                                                 .FirstOrDefaultAsync(r => r.Id == id);

            if (insuranceRequest == null) return null;

            return _mapper.Map<ReadInsuranceRequestDto>(insuranceRequest);
        }
    }
}
