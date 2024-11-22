using AutoMapper;
using PRJ4.Models;
using PRJ4.DTOs;

namespace PRJ4.Mappings
{
    public class FudgifterProfile : Profile
    {
        public FudgifterProfile()
        {
            // Map for creating new Fudgifter from nyFudgifterDTO
            CreateMap<nyFudgifterDTO, Fudgifter>()
                .ForMember(dest => dest.KategoriId, opt => opt.MapFrom(src => src.KategoriId))
                .ForMember(dest => dest.Tekst, opt => opt.MapFrom(src => src.Tekst))
                .ForMember(dest => dest.Pris, opt => opt.MapFrom(src => src.Pris))
                .ForMember(dest => dest.Dato, opt => opt.MapFrom(src => src.Dato));

            // Map from Fudgifter to FudgifterResponseDTO
            CreateMap<Fudgifter, FudgifterResponseDTO>()
                .ForMember(dest => dest.FudgiftId, opt => opt.MapFrom(src => src.FudgiftId))
                .ForMember(dest => dest.Pris, opt => opt.MapFrom(src => src.Pris))
                .ForMember(dest => dest.Tekst, opt => opt.MapFrom(src => src.Tekst))
                .ForMember(dest => dest.KategoriNavn, opt => opt.MapFrom(src => src.Kategori.Navn)) // Assuming Kategori has a 'Navn' property
                .ForMember(dest => dest.Dato, opt => opt.MapFrom(src => src.Dato));

            // Map for updating Fudgifter from FudgifterUpdateDTO (optional, for future updates)
            CreateMap<FudgifterUpdateDTO, Fudgifter>()
                .ForMember(dest => dest.Pris, opt => opt.MapFrom(src => src.Pris))
                .ForMember(dest => dest.Tekst, opt => opt.MapFrom(src => src.Tekst))
                .ForMember(dest => dest.Dato, opt => opt.MapFrom(src => src.Dato))
                .ForMember(dest => dest.KategoriId, opt => opt.MapFrom(src => src.KategoriId));

            CreateMap<Fudgifter, nyFudgifterDTO>()
                .ForMember(dest => dest.Pris, opt => opt.MapFrom(src => src.Pris))
                .ForMember(dest => dest.Tekst, opt => opt.MapFrom(src => src.Tekst))
                .ForMember(dest => dest.Dato, opt => opt.MapFrom(src => src.Dato))
                .ForMember(dest => dest.KategoriId, opt => opt.MapFrom(src => src.KategoriId));
        }
    }
}
