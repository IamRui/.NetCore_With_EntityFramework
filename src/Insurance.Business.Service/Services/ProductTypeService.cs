using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.Business.Service.Interfaces;
using Insurance.Data.Access.Entities;
using Insurance.Data.Access.Repositories.Interfaces;

namespace Insurance.Business.Service.Services
{
	public class ProductTypeService : IProductTypeService
	{
		private readonly IProductTypeRepository _productTypeRepository;

		public ProductTypeService(IProductTypeRepository productTypeRepository)
		{
			_productTypeRepository = productTypeRepository;
		}

		public async Task<ProductType> Update(ProductType productType)
		{
			var productTypeDetails = await _productTypeRepository.GetById(productType.Id);
			if (productTypeDetails == null) return null;

			await _productTypeRepository.Update(productType);
			return productType;
		}

		public async Task<ProductType> GetById(int id)
		{
			var productDetails = await _productTypeRepository.GetById(id);
			return productDetails ?? null;
		}
	}
}
