using AutoMapper;
using PRJ4.Models;
using PRJ4.DTOs;

namespace PRJ4.Mappings
{
    public class VudgifterProfile : Profile
    {
        public VudgifterProfile()
        {
            // Map for creating new Vudgifter from nyVudgifterDTO
            CreateMap<nyVudgifterDTO, Vudgifter>()
                .ForMember(dest => dest.KategoriId, opt => opt.MapFrom(src => src.KategoriId))
                .ForMember(dest => dest.Tekst, opt => opt.MapFrom(src => src.Tekst))
                .ForMember(dest => dest.Pris, opt => opt.MapFrom(src => src.Pris))
                .ForMember(dest => dest.Dato, opt => opt.MapFrom(src => src.Dato));

            // Map from Vudgifter to VudgifterResponseDTO
            CreateMap<Vudgifter, VudgifterResponseDTO>()
                .ForMember(dest => dest.VudgiftId, opt => opt.MapFrom(src => src.VudgiftId))
                .ForMember(dest => dest.Pris, opt => opt.MapFrom(src => src.Pris))
                .ForMember(dest => dest.Tekst, opt => opt.MapFrom(src => src.Tekst))
                .ForMember(dest => dest.KategoriNavn, opt => opt.MapFrom(src => src.Kategori.Navn)) // Assuming Kategori has a 'Navn' property
                .ForMember(dest => dest.Dato, opt => opt.MapFrom(src => src.Dato));

            // Map for updating Vudgifter from VudgifterUpdateDTO (optional, for future updates)
            CreateMap<VudgifterUpdateDTO, Vudgifter>()
                .ForMember(dest => dest.Pris, opt => opt.MapFrom(src => src.Pris))
                .ForMember(dest => dest.Tekst, opt => opt.MapFrom(src => src.Tekst))
                .ForMember(dest => dest.Dato, opt => opt.MapFrom(src => src.Dato))
                .ForMember(dest => dest.KategoriId, opt => opt.MapFrom(src => src.KategoriId));

            CreateMap<Vudgifter, nyVudgifterDTO>()
                .ForMember(dest => dest.Pris, opt => opt.MapFrom(src => src.Pris))
                .ForMember(dest => dest.Tekst, opt => opt.MapFrom(src => src.Tekst))
                .ForMember(dest => dest.Dato, opt => opt.MapFrom(src => src.Dato))
                .ForMember(dest => dest.KategoriId, opt => opt.MapFrom(src => src.KategoriId));
        }
    }
}
