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
    }
}
