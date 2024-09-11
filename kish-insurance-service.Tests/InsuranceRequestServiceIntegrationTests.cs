using AutoMapper;
using kish_insurance_service;
using kish_insurance_service.DTOs;
using kish_insurance_service.Models;
using kish_insurance_service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace Tests;
public class InsuranceRequestServiceIntegrationTests : IClassFixture<DatabaseFixture>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly InsuranceRequestService _service;
    private readonly IDistributedCache _cache;

    public InsuranceRequestServiceIntegrationTests(DatabaseFixture fixture)
    {
        _context = fixture.Context; // Use context from DatabaseFixture

        // Mock the IDistributedCache
        var cacheMock = new Mock<IDistributedCache>();
        _cache = cacheMock.Object;

        // Configure AutoMapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<InsuranceRequestDTO, InsuranceRequest>();
            cfg.CreateMap<CoverageDTO, Coverage>();
        });
        _mapper = config.CreateMapper();

        _service = new InsuranceRequestService(_context, _mapper, _cache);

        /*SeedData(); */
    }

   /* private void SeedData()
    {
        if (!_context.CoverageTypes.Any())
        {
            // Seed data for CoverageTypes
            _context.CoverageTypes.AddRange(
                new CoverageType { MinCapital = 5000, MaxCapital = 100000, Name = "Health" },
                new CoverageType { MinCapital = 1000, MaxCapital = 20000, Name = "Vehicle" }
            );
            _context.SaveChanges(); // Ensure the data is saved
        }
    }*/

    [Fact]
    public async Task SubmitInsuranceRequest_ShouldThrowException_IfCoverageTypeNotFound()
    {
        // Arrange
        var insuranceRequestDto = new InsuranceRequestDTO
        {
            Coverages = new List<CoverageDTO>
            {
                new CoverageDTO { CoverageTypeId = 999, Capital = 10000 } // CoverageTypeId 999 doesn't exist
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.SubmitInsuranceRequestAsync(insuranceRequestDto));
    }

    [Fact]
    public async Task SubmitInsuranceRequest_ShouldSave_WhenValid()
    {
        // Arrange
        var insuranceRequestDto = new InsuranceRequestDTO
        {
            Title = "Test Request",
            Coverages = new List<CoverageDTO>
            {
                new CoverageDTO { CoverageTypeId = 1, Capital = 7000 } // CoverageTypeId 1 exists from seed data
            }
        };

        // Act
        var InsuranceRequestResponseDto = await _service.SubmitInsuranceRequestAsync(insuranceRequestDto);

        // Assert
        var savedRequest = await _context.InsuranceRequests.FindAsync(InsuranceRequestResponseDto.RequestId);
        Assert.NotNull(savedRequest);
        Assert.Equal("Test Request", savedRequest.Title);
        Assert.Single(savedRequest.Coverages);
    }
    [Fact]
    public async Task SubmitInsuranceRequest_ShouldError_OutOfRangeMin()
    {
        // Arrange
        var insuranceRequestDto = new InsuranceRequestDTO
        {
            Title = "Out of Range Request",
            Coverages = new List<CoverageDTO>
        {
            new CoverageDTO { CoverageTypeId = 1, Capital = 1000 } // Capital exceeds MaxCapital of 100000 for CoverageTypeId 1
        }
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.SubmitInsuranceRequestAsync(insuranceRequestDto));

        // Log the exception message for visibility
        Console.WriteLine(exception.Message);

        // Check if the exception message matches the expected out-of-range message
        Assert.Equal("Capital for Surgery should be between 5000.00 and 500000000.00.", exception.Message);
    }

    [Fact]
    public async Task SubmitInsuranceRequest_ShouldError_OutOfRangeMax()
    {
        // Arrange
        var insuranceRequestDto = new InsuranceRequestDTO
        {
            Title = "Out of Range Request",
            Coverages = new List<CoverageDTO>
        {
            new CoverageDTO { CoverageTypeId = 1, Capital = 100000000000000 } // Capital exceeds MaxCapital of 100000 for CoverageTypeId 1
        }
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.SubmitInsuranceRequestAsync(insuranceRequestDto));

        // Log the exception message for visibility
        Console.WriteLine(exception.Message);

        // Check if the exception message matches the expected out-of-range message
        Assert.Equal("Capital for Surgery should be between 5000.00 and 500000000.00.", exception.Message);
    }

}
