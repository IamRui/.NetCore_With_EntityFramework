using System.Collections.Generic;
using Insurance.Business.Service.Const;
using Insurance.Business.Service.Interfaces;
using Insurance.Data.Access.Entities;

namespace Insurance.Business.Service.Services
{
	public class CalculationService : ICalculationService
	{
		public CalculationService() {
		}

		public decimal? GetInsuranceForProduct(Product product)
		{
			var insuranceValue = 0m;

			if (product?.ProductType.Name == null) return null;

			var productTypeHasInsurance = product.ProductType.CanBeInsured;
			var surchargeRate = product.ProductType.SurchargeRate;
			var salesPrice = product.SalesPrice;

			if (!productTypeHasInsurance) return insuranceValue;

			if (surchargeRate != null && salesPrice >= 500)
			{
				insuranceValue = (decimal) surchargeRate;
			}

			if (salesPrice >= 500 && salesPrice < 2000) {
				insuranceValue += 1000;
			}

			if (salesPrice >= 2000) {
				insuranceValue += 2000;
			}

			if (product.ProductType.Name == ProductTypeNameKey.Laptop || product.ProductType.Name == ProductTypeNameKey.SmartPhone)
			{
				insuranceValue += 500;
			}

			return insuranceValue;
		}

		public decimal? GetInsuranceForOrder(IEnumerable<Product> productList)
		{
			decimal? orderInsuranceValue = 0m;
			var  digitalCameraQuantity = 0;

			foreach (var product in productList)
			{
				var productInsuranceCost = GetInsuranceForProduct(product);

				if (productInsuranceCost == null) return null;

				orderInsuranceValue += productInsuranceCost;

				if (product.SalesPrice >= 500 && product.ProductType.CanBeInsured && product.ProductType.Name == ProductTypeNameKey.DigitalCamera) {
					digitalCameraQuantity += 1;
				}
			}

			if (digitalCameraQuantity > 0)
			{
				orderInsuranceValue += 500;
			}

			return orderInsuranceValue;
		}
	}
}
