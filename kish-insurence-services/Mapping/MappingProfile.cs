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
        }
    }
}
