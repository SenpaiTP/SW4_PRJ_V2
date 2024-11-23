using AutoMapper;
using PRJ4.Models;
using PRJ4.DTOs;

namespace PRJ4.Mappings
{
    public class LogMappingProfile : Profile
    {
        public LogMappingProfile()
        {
            // Define the mapping from Log to LogDto
            CreateMap<Log, LogDto>()
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp))
                .ForMember(dest => dest.MessageTemplate, opt => opt.MapFrom(src => src.MessageTemplate))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Properties))
                .ForMember(dest => dest.Exception, opt => opt.MapFrom(src => src.Exception));
        }
    }
}
