using System.Threading.Tasks;
using Insurance.Business.Service.Interfaces;
using Insurance.Data.Access.Entities;
using Insurance.Data.Access.Repositories.Interfaces;

namespace Insurance.Business.Service.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;

		public ProductService(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<Product> GetById(int id)
		{
			var productDetails =  await _productRepository.GetById(id);
			return productDetails ?? null;
		}
	}
}
