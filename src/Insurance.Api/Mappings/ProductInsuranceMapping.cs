using AutoMapper;
using Insurance.Api.Models;
using Insurance.Data.Access.Entities;

namespace Insurance.Api.Mappings
{
	public class ProductInsuranceMapping : Profile
	{
		public ProductInsuranceMapping()
		{
			CreateMap<Product, ProductInsuranceDto>()
				.ForMember(
					dest => dest.ProductTypeHasInsurance,
					opt => opt.MapFrom(src => src.ProductType.CanBeInsured)
				)
				.ForMember(
					dest => dest.ProductTypeName,
					opt => opt.MapFrom(src => src.ProductType.Name)
				)
				.ForMember(
					dest => dest.ProductId,
					opt => opt.MapFrom(src => src.Id)
				)
				.ForMember(
					dest => dest.SalesPrice,
					opt => opt.MapFrom(src => src.SalesPrice)
				);
		}
	}
}
