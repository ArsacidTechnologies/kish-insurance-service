// Services/InsuranceRequestService.cs
using AutoMapper;
using kish_insurance_service.DTOs;
using kish_insurance_service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Storage;

namespace kish_insurance_service.Services
{
    public class InsuranceRequestService : IInsuranceRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private const string CacheKeyPrefix = "InsuranceRequest_";
        public InsuranceRequestService(ApplicationDbContext context, IMapper mapper, IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;

        }
        //submit Insurance request
        public async Task<InsuranceRequestResponseDto> SubmitInsuranceRequestAsync(InsuranceRequestDTO requestDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create a new insurance request
                var insuranceRequest = new InsuranceRequest
                {
                    Title = requestDto.Title,
                    Coverages = new List<Coverage>()
                };

                decimal totalPremium = 0;

                // Loop through the coverages and calculate the premium for each one
                foreach (var coverageDto in requestDto.Coverages)
                {
                    var coverageType = await _context.CoverageTypes.FindAsync(coverageDto.CoverageTypeId);
                    if (coverageType == null)
                    {
                        throw new Exception("Invalid coverage type");
                    }

                    // Validate the capital is within the allowed range
                    if (coverageDto.Capital < coverageType.MinCapital || coverageDto.Capital > coverageType.MaxCapital)
                    {
                        throw new Exception($"Capital for {coverageType.Name} should be between {coverageType.MinCapital} and {coverageType.MaxCapital}.");
                    }

                    // Calculate premium: Premium = Capital * PremiumRate
                    var premium = coverageDto.Capital * coverageType.PremiumRate;
                    totalPremium += premium;

                    // Add the coverage with the calculated premium
                    var coverage = new Coverage
                    {
                        CoverageTypeId = coverageDto.CoverageTypeId,
                        Capital = coverageDto.Capital,
                        Premium = premium, // Storing calculated premium
                    };

                    insuranceRequest.Coverages.Add(coverage);
                }

                // Save the insurance request and its coverages
                _context.InsuranceRequests.Add(insuranceRequest);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Create the response DTO
                var response = new InsuranceRequestResponseDto
                {
                    RequestId = insuranceRequest.Id,
                    TotalPremium = totalPremium
                };

                return response;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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

        // Get paginated insurance requests
        public async Task<PaginatedResultDto<ReadInsuranceRequestDto>> GetPaginatedInsuranceRequestsAsync(
            int pageNumber, int pageSize, string title = null, int? coverageTypeId = null)
        {
            var query = _context.InsuranceRequests
                                .Include(r => r.Coverages)
                                .ThenInclude(c => c.CoverageType)
                                .AsQueryable();

            // Filtering by Title
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(r => r.Title.Contains(title));
            }

            // Filtering by CoverageTypeId
            if (coverageTypeId.HasValue)
            {
                query = query.Where(r => r.Coverages.Any(c => c.CoverageTypeId == coverageTypeId));
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Pagination
            var insuranceRequests = await query
                                          .Skip((pageNumber - 1) * pageSize)
                                          .Take(pageSize)
                                          .ToListAsync();

            // Map to DTO
            var insuranceRequestDtos = _mapper.Map<List<ReadInsuranceRequestDto>>(insuranceRequests);

            return new PaginatedResultDto<ReadInsuranceRequestDto>(insuranceRequestDtos, totalCount, pageNumber, pageSize);
        }
    }
}
