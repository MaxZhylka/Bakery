using AutoMapper;
using backend.Core.DTOs;
using backend.Core.Models;
using System.Globalization;

namespace backend.Core.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Маппинг для Order
            CreateMap<OrderDTO, OrderEntity>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt ?? DateTime.Now));

            CreateMap<OrderEntity, OrderDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => (DateTime?)src.CreatedAt));

            // Маппинг для Product
            CreateMap<ProductDTO, ProductEntity>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt ?? DateTime.Now));

            CreateMap<ProductEntity, ProductDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => (DateTime?)src.CreatedAt));
        }
    }
}

