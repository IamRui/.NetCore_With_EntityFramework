using AutoMapper;
using Insurance.Api.Models;
using Insurance.Data.Access.Entities;

namespace Insurance.Api.Mappings
{
	public class ProductTypeMapping : Profile
	{
		public ProductTypeMapping()
		{
			CreateMap<ProductTypeDto, ProductType>()
				.ForMember(
					dest => dest.SurchargeRate,
					opt => opt.MapFrom(src => src.SurchargeRate)
				)
				.ForMember(
					dest => dest.Id,
					opt => opt.MapFrom(src => src.ProductTypeId)
				);
		}
	}
}
