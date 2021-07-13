using System.Collections.Generic;
using Insurance.Data.Access.Entities;

namespace Insurance.Business.Service.Interfaces
{
	public interface ICalculationService
	{
		public decimal? GetInsuranceForProduct(Product product);
		public decimal? GetInsuranceForOrder(IEnumerable<Product> productList);
	}
}
