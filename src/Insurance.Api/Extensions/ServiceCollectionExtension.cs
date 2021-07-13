using Insurance.Business.Service.Interfaces;
using Insurance.Business.Service.Services;
using Insurance.Data.Access.DBContext;
using Insurance.Data.Access.Repositories;
using Insurance.Data.Access.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Api.Extensions
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection ResolveDependencies(this IServiceCollection services)
		{
			services.AddScoped<InsuranceContext>();

			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<IProductTypeRepository, ProductTypeRepository>();

			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IProductTypeService, ProductTypeService>();
			services.AddScoped<ICalculationService, CalculationService>();

			return services;
		}
    }
}
