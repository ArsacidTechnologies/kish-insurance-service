using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using kish_insurance_service;
using kish_insurance_service.DTOs;
using System.Net.Http.Json;
using System.Net;

namespace ApiTests;
public class InsuranceRequestApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public InsuranceRequestApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task SubmitInsuranceRequest_ShouldReturnBadRequest_WhenCapitalOutOfRangeMin()
    {
        // Arrange
        var insuranceRequestDto = new InsuranceRequestDTO
        {
            Title = "Out of Range Request",
            Coverages = new List<CoverageDTO>
        {
            new CoverageDTO { CoverageTypeId = 1, Capital = 1 } // Capital exceeds MinCapital
        }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/InsuranceRequest/submit-request", insuranceRequestDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Capital for", content); // Check if the exception message is in the response
    }
    [Fact]
    public async Task SubmitInsuranceRequest_ShouldReturnBadRequest_WhenTypeNotFound()
    {
        // Arrange
        var insuranceRequestDto = new InsuranceRequestDTO
        {
            Title = "Out of Range Request",
            Coverages = new List<CoverageDTO>
        {
            new CoverageDTO { CoverageTypeId = 43, Capital = 32423 } // CoverageTypeId exceeds None Of Type
        }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/InsuranceRequest/submit-request", insuranceRequestDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Invalid coverage type", content); // Check if the exception message is in the response
    }
    [Fact]
    public async Task SubmitInsuranceRequest_ShouldReturnBadRequest_WhenCapitalOutOfRange()
    {
        // Arrange
        var insuranceRequestDto = new InsuranceRequestDTO
        {
            Title = "Out of Range Request",
            Coverages = new List<CoverageDTO>
        {
            new CoverageDTO { CoverageTypeId = 1, Capital = 1500000000000 } // Capital exceeds MaxCapital
        }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/InsuranceRequest/submit-request", insuranceRequestDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Capital for", content); // Check if the exception message is in the response
    }
    [Fact]
    public async Task SubmitInsuranceRequest_ShouldReturnBadRequest_When_should_save()
    {
        // Arrange
        var insuranceRequestDto = new InsuranceRequestDTO
        {
            Title = "in range for save",
            Coverages = new List<CoverageDTO>
        {
            new CoverageDTO { CoverageTypeId = 1, Capital = 54999 } // Capital exceeds MaxCapital
        }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/InsuranceRequest/submit-request", insuranceRequestDto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Insurance request submitted successfully", content); // Check if the exception message is in the response
    }
}
