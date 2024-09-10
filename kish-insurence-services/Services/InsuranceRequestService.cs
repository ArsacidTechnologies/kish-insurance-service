// Services/InsuranceRequestService.cs
using AutoMapper;
using kish_insurance_service.DTOs;
using kish_insurance_service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

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
        public async Task<int> SubmitInsuranceRequestAsync(InsuranceRequestDTO requestDto)
        {
            // Create a new InsuranceRequest object using AutoMapper
            var insuranceRequest = _mapper.Map<InsuranceRequest>(requestDto);

            // Validate and process each coverage
            foreach (var coverage in insuranceRequest.Coverages)
            {
                // Check Redis cache first
                string cacheKey = $"CoverageType_{coverage.CoverageTypeId}";
                var cachedCoverageType = await _cache.GetStringAsync(cacheKey);

                CoverageType coverageType;
                if (!string.IsNullOrEmpty(cachedCoverageType))
                {
                    // Deserialize the cached data if found
                    coverageType = JsonConvert.DeserializeObject<CoverageType>(cachedCoverageType);
                }
                else
                {
                    // Fetch from the database if not found in cache
                    coverageType = await _context.CoverageTypes.FindAsync(coverage.CoverageTypeId);
                    if (coverageType == null)
                    {
                        throw new Exception($"CoverageType with ID {coverage.CoverageTypeId} not found.");
                    }

                    // Cache the retrieved data in Redis
                    await _cache.SetStringAsync(
                        cacheKey,
                        JsonConvert.SerializeObject(coverageType),
                        new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6) // Cache for 6 hours
                        });
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