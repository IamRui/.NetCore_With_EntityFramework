using Insurance.Business.Service.Const;
using Insurance.Business.Service.Services;
using Insurance.Data.Access.Entities;
using System.Collections.Generic;
using Xunit;

namespace Insurance.Business.Service.Tests.Services
{
	public class CalculationServiceTests
	{
		[Fact]
		public void GetInsuranceForProduct_GivenNoProduct_ShouldReturnNull()
		{
			//arrange
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(null);

			//assert
			Assert.Null(insuranceCost);
		}

		[Fact]
		public void GetInsuranceForProduct_GivenProductWithNoProductTypeName_ShouldReturnNull()
		{
			//arrange
			var product = WithProductDoesNotHaveProductTypeName(productTypeId: 12, canBeInsured: true, salesPrice: 500);
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Null(insuranceCost);
		}

		/*add for fixing the reported issue that when customers buy a laptop that costs less than € 500, insurance is not calculated, while it should be € 500*/
		[Fact]
		public void GetInsuranceForProduct_GivenLaptopSalesPriceLessThan500Euros_ShouldAdd500EurosToInsuranceCost()
		{
			//arrange
			const decimal expectedInsuranceValue = 500;
			var product = WithProduct(productTypeName: ProductTypeNameKey.Laptop, productTypeId: 12, canBeInsured: true, salesPrice: 200);
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Equal(expected: expectedInsuranceValue, actual: insuranceCost);
		}

		[Fact]
		public void GetInsuranceForProduct_GivenSmartPhoneSalesPriceLessThan500Euros_ShouldAdd500EurosToInsuranceCost()
		{
			//arrange
			const decimal expectedInsuranceValue = 500;
			var product = WithProduct(productTypeName: ProductTypeNameKey.SmartPhone, productTypeId: 12, canBeInsured: true, salesPrice: 200);
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Equal(expected: expectedInsuranceValue, actual: insuranceCost);
		}

		[Fact]
		public void GetInsuranceForProduct_GivenProductSalesPriceLessThan500Euros_ShouldAdd0EurosToInsuranceCost()
		{
			//arrange
			const decimal expectedInsuranceValue = 0;
			var product = WithProduct(productTypeName: ProductTypeNameKey.WashingMachine, productTypeId: 12, canBeInsured: true, salesPrice: 200);
			product.ProductType.SurchargeRate = 30;
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Equal(expected: expectedInsuranceValue, actual: insuranceCost);
		}

		[Fact]
		public void GetInsuranceForProduct_GivenProductSalesPriceBetween500And2000EurosWithSurcharge_ShouldAddSurchargeRateToInsuranceCost()
		{
			//arrange
			const decimal expectedInsuranceValue = 1030;
			var product = WithProduct(productTypeName: ProductTypeNameKey.WashingMachine, productTypeId: 12, canBeInsured: true, salesPrice: 500);
			product.ProductType.SurchargeRate = 30;
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Equal(expected: expectedInsuranceValue, actual: insuranceCost);
		}

		[Fact]
		public void GetInsuranceForProduct_GivenLaptopSalesPriceBetween500And2000EurosWithSurcharge_ShouldAddSurchargeRateToInsuranceCost()
		{
			//arrange
			const decimal expectedInsuranceValue = 1530;
			var product = WithProduct(productTypeName: ProductTypeNameKey.Laptop, productTypeId: 12, canBeInsured: true, salesPrice: 500);
			product.ProductType.SurchargeRate = 30;
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Equal(expected: expectedInsuranceValue, actual: insuranceCost);
		}

		[Fact]
		public void GetInsuranceForProduct_GivenProductSalesPriceMoreThan2000EurosWithSurcharge_ShouldAddSurchargeRateToInsuranceCost()
		{
			//arrange
			const decimal expectedInsuranceValue = 2030;
			var product = WithProduct(productTypeName: ProductTypeNameKey.WashingMachine, productTypeId: 12, canBeInsured: true, salesPrice: 6000);
			product.ProductType.SurchargeRate = 30;
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Equal(expected: expectedInsuranceValue, actual: insuranceCost);
		}

		[Fact]
		public void GetInsuranceForProduct_GivenSmartPhoneSalesPriceMoreThan2000EurosWithSurcharge_ShouldAddSurchargeRateToInsuranceCost()
		{
			//arrange
			const decimal expectedInsuranceValue = 2530;
			var product = WithProduct(productTypeName: ProductTypeNameKey.SmartPhone, productTypeId: 12, canBeInsured: true, salesPrice: 6000);
			product.ProductType.SurchargeRate = 30;
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Equal(expected: expectedInsuranceValue, actual: insuranceCost);
		}

		[Fact]
		public void GetInsuranceForProduct_GivenProductSalesPriceEqualTo500Euros_ShouldAdd1000EurosToInsuranceCost()
		{
			//arrange
			const decimal expectedInsuranceValue = 1000;
			var product = WithProduct(productTypeName: ProductTypeNameKey.WashingMachine, productTypeId: 12, canBeInsured: true, salesPrice: 500);
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Equal(expected: expectedInsuranceValue, actual: insuranceCost);
		}

		[Fact]
		public void GetInsuranceForProduct_GivenProductSalesPriceBetween500And2000Euros_ShouldAdd1000EurosToInsuranceCost()
		{
			//arrange
			const decimal expectedInsuranceValue = 1000;
			var product = WithProduct(productTypeName: ProductTypeNameKey.WashingMachine, productTypeId: 12, canBeInsured: true, salesPrice: 1200);
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Equal(expected: expectedInsuranceValue, actual: insuranceCost);
		}

		[Fact]
		public void GetInsuranceForProduct_GivenSmartPhoneSalesPriceBetween500And2000Euros_ShouldAdd1500EurosToInsuranceCost()
		{
			//arrange
			const decimal expectedInsuranceValue = 1500;
			var product = WithProduct(productTypeName: ProductTypeNameKey.SmartPhone, productTypeId: 12, canBeInsured: true, salesPrice: 1200);
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Equal(expected: expectedInsuranceValue, actual: insuranceCost);
		}

		[Fact]
		public void GetInsuranceForProduct_GivenLaptopSalesPriceBetween500And2000Euros_ShouldAdd1500EurosToInsuranceCost()
		{
			//arrange
			const decimal expectedInsuranceValue = 1500;
			var product = WithProduct(productTypeName: ProductTypeNameKey.Laptop, productTypeId: 12, canBeInsured: true, salesPrice: 1000);
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Equal(expected: expectedInsuranceValue, actual: insuranceCost);
		}

		[Fact]
		public void GetInsuranceForProduct_GivenProductSalesPriceEqualTo2000Euros_ShouldAdd2000EurosToInsuranceCost()
		{
			//arrange
			const decimal expectedInsuranceValue = 2000;
			var product = WithProduct(productTypeName: ProductTypeNameKey.WashingMachine, productTypeId: 12, canBeInsured: true, salesPrice: 2000);
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Equal(expected: expectedInsuranceValue, actual: insuranceCost);
		}

		[Fact]
		public void GetInsuranceForProduct_GivenProductSalesPriceMoreThan2000Euros_ShouldAdd2000EurosToInsuranceCost()
		{
			//arrange
			const decimal expectedInsuranceValue = 2000;
			var product = WithProduct(productTypeName: ProductTypeNameKey.WashingMachine, productTypeId: 12, canBeInsured: true, salesPrice: 2500);
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Equal(expected: expectedInsuranceValue, actual: insuranceCost);
		}

		[Fact]
		public void GetInsuranceForProduct_GivenLaptopSalesPriceMoreThan2000Euros_ShouldAdd2500EurosToInsuranceCost()
		{
			//arrange
			const decimal expectedInsuranceValue = 2500;
			var product = WithProduct(productTypeName: ProductTypeNameKey.Laptop, productTypeId: 12, canBeInsured: true, salesPrice: 2700);
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Equal(expected: expectedInsuranceValue, actual: insuranceCost);
		}

		[Fact]
		public void GetInsuranceForProduct_GivenSmartPhoneSalesPriceMoreThan2000Euros_ShouldAdd2500EurosToInsuranceCost()
		{
			//arrange
			const decimal expectedInsuranceValue = 2500;
			var product = WithProduct(productTypeName: ProductTypeNameKey.SmartPhone, productTypeId: 12, canBeInsured: true, salesPrice: 2700);
			var calculationService = new CalculationService();

			//act
			var insuranceCost = calculationService.GetInsuranceForProduct(product);

			//assert
			Assert.Equal(expected: expectedInsuranceValue, actual: insuranceCost);
		}

		[Fact]
		public void GetInsuranceForOrder_GivenInValidProductList_ShouldReturnNull()
		{
			//arrange
			var productWithoutProductTypeName = WithProductDoesNotHaveProductTypeName(productTypeId: 12, canBeInsured: true, salesPrice: 1000);
			var smartPhone = WithProduct(productTypeName: ProductTypeNameKey.SmartPhone, productTypeId: 12, canBeInsured: true, salesPrice: 1000);
			var washingMachine = WithProduct(productTypeName: ProductTypeNameKey.WashingMachine, productTypeId: 13, canBeInsured: true, salesPrice: 2000);
			var productList = new List<Product>()
			{
				smartPhone,
				washingMachine,
				productWithoutProductTypeName

			};
			var calculationService = new CalculationService();

			//act
			var orderInsuranceCost = calculationService.GetInsuranceForOrder(productList);

			//assert
			Assert.Null(orderInsuranceCost);
		}

		[Fact]
		public void GetInsuranceForOrder_GivenDigitalCameraWithPriceUnder500Euros_ShouldAdd0EurosToOrderInsuranceCost()
		{
			//arrange
			const decimal expectedOrderInsuranceValue = 0;
			var digitalCamera = WithProduct(productTypeName: ProductTypeNameKey.DigitalCamera, productTypeId: 13, canBeInsured: true, salesPrice: 200);
			var productList = new List<Product>() {
				digitalCamera
			};
			var calculationService = new CalculationService();

			//act
			var orderInsuranceCost = calculationService.GetInsuranceForOrder(productList);

			//assert
			Assert.Equal(expected: expectedOrderInsuranceValue, actual: orderInsuranceCost);
		}

		[Fact]
		public void GetInsuranceForOrder_GivenValidProductList_ShouldAddCorrectOrderInsuranceCost()
		{
			//arrange
			const decimal expectedOrderInsuranceValue = 3500;
			var smartPhone = WithProduct(productTypeName: ProductTypeNameKey.SmartPhone, productTypeId: 12, canBeInsured: true, salesPrice: 1000);
			var washingMachine = WithProduct(productTypeName: ProductTypeNameKey.WashingMachine, productTypeId: 13, canBeInsured: true, salesPrice: 2000);
			var productList = new List<Product>()
			{
				smartPhone,
				washingMachine
			};
			var calculationService = new CalculationService();

			//act
			var orderInsuranceCost = calculationService.GetInsuranceForOrder(productList);

			//assert
			Assert.Equal(expected: expectedOrderInsuranceValue, actual: orderInsuranceCost);
		}

		[Fact]
		public void GetInsuranceForOrder_GivenValidProductListWithDigitalCamera_ShouldAddExtra500EurosToOrderInsuranceCost()
		{
			//arrange
			const decimal expectedOrderInsuranceValue = 6000;
			var smartPhone = WithProduct(productTypeName: ProductTypeNameKey.SmartPhone, productTypeId: 12, canBeInsured: true, salesPrice: 1000);
			var digitalCamera = WithProduct(productTypeName: ProductTypeNameKey.DigitalCamera, productTypeId: 12, canBeInsured: true, salesPrice: 2000);
			var washingMachine = WithProduct(productTypeName: ProductTypeNameKey.WashingMachine, productTypeId: 13, canBeInsured: true, salesPrice: 2000);
			var productList = new List<Product>()
			{
				smartPhone,
				washingMachine,
				digitalCamera
			};
			var calculationService = new CalculationService();

			//act
			var orderInsuranceCost = calculationService.GetInsuranceForOrder(productList);

			//assert
			Assert.Equal(expected: expectedOrderInsuranceValue, actual: orderInsuranceCost);
		}

		private static Product WithProduct(string productTypeName, int productTypeId, bool canBeInsured, decimal salesPrice)
		{
			var productType = new ProductType {
				Id = productTypeId,
				Name = productTypeName,
				CanBeInsured = canBeInsured
			};
			return new Product {
				SalesPrice = salesPrice,
				ProductTypeId = productType.Id,
				ProductType = productType
			};
		}

		private static Product WithProductDoesNotHaveProductTypeName(int productTypeId, bool canBeInsured, decimal salesPrice)
		{
			var productType = new ProductType
			{
				Id = productTypeId,
				CanBeInsured = canBeInsured
			};
			return new Product
			{
				SalesPrice = salesPrice,
				ProductTypeId = productType.Id,
				ProductType = productType
			};
		}
	}
}
