using AutoMapper;
using kish_insurance_service.DTOs;
using kish_insurance_service.Models;

namespace kish_insurance_service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map InsuranceRequestDTO to InsuranceRequest
            CreateMap<InsuranceRequestDTO, InsuranceRequest>()
                .ForMember(dest => dest.Coverages, opt => opt.MapFrom(src => src.Coverages));

            // Map CoverageDTO to Coverage
            CreateMap<CoverageDTO, Coverage>();

            // Map InsuranceRequest to ReadInsuranceRequestDto
            CreateMap<InsuranceRequest, ReadInsuranceRequestDto>()
                .ForMember(dest => dest.Coverages, opt => opt.MapFrom(src => src.Coverages));

            // Map Coverage to ReadCoverageDto
            CreateMap<Coverage, ReadCoverageDto>()
                .ForMember(dest => dest.CoverageType, opt => opt.MapFrom(src => src.CoverageType.Name));

            // Ensure this map is present
            CreateMap<Coverage, CoverageDTO>();
        }
    }
}
