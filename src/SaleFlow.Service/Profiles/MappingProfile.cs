using AutoMapper;
using SaleFlow.Domain.Entities;
using SaleFlow.Service.DTOs;

namespace SaleFlow.Service.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping from DTO to domain for Sale creation.
            CreateMap<SaleDto, Sale>()
                .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
                .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.SaleDate))
                // Map Customer as a new Customer instance using CustomerExternalId and CustomerName.
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src =>
                    new Customer(src.CustomerExternalId, src.CustomerName)))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch));

            CreateMap<SaleItemDto, SaleItem>()
                // You can use the constructor in SaleItem. Assume a parameterized mapping:
                .ConstructUsing(src => new SaleItem(src.ProductId, src.Quantity, src.UnitPrice))
                .ForMember(dest => dest.Discount, opt => opt.Ignore())
                .ForMember(dest => dest.TotalItemAmount, opt => opt.Ignore())
                .ForMember(dest => dest.IsCancelled, opt => opt.Ignore());

            // Mapping from domain to DTO (for queries)
            CreateMap<Sale, SaleDto>()
                .ForMember(dest => dest.CustomerExternalId, opt => opt.MapFrom(src => src.Customer.ExternalId))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
                .ForMember(dest => dest.SaleItems, opt => opt.MapFrom(src => src.SaleItems));
            CreateMap<SaleItem, SaleItemDto>();
        }
    }
}
