using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;
using AutoMapper;
using kish_insurance_service;
using kish_insurance_service.DTOs;
using kish_insurance_service.Models;
using kish_insurance_service.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Tests
{
    public class InsuranceRequestServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly InsuranceRequestService _service;

        public InsuranceRequestServiceTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestInsuranceDB")
                .Options;

            _context = new ApplicationDbContext(options);

            // Mock the IDistributedCache
            _cacheMock = new Mock<IDistributedCache>();

            // Configure AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<InsuranceRequestDTO, InsuranceRequest>();
                cfg.CreateMap<CoverageDTO, Coverage>();
            });
            _mapper = config.CreateMapper();

            // Initialize the service with the context, mapper, and cache mock
            _service = new InsuranceRequestService(_context, _mapper, _cacheMock.Object);
        }



        [Fact]
        public async Task SubmitInsuranceRequest_ShouldThrowException_IfCoverageTypeNotFound()
        {
            // Arrange
            var insuranceRequestDto = new InsuranceRequestDTO
            {
                Coverages = new List<CoverageDTO>
                {
                    new CoverageDTO { CoverageTypeId = 999, Capital = 10000 }
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.SubmitInsuranceRequestAsync(insuranceRequestDto));
        }
    }
}
