using AutoMapper;
using Insurance.Api.Controllers;
using Insurance.Api.Models;
using Insurance.Business.Service.Const;
using Insurance.Business.Service.Interfaces;
using Insurance.Data.Access.Entities;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Insurance.Api.Tests.Controllers
{
	public class InsuranceControllerTests
	{
		[Fact]
		public void CalculateProductInsurance_GivenNotExistProductId_ShouldReturnNotFound()
		{
			//arrange
			var productService = WithProductServiceNotExistProduct();
			var calculationService = WithCalculationServiceGetProductInsurance(2000);
			var mapper = WithAutoMapper(new ProductInsuranceDto() { ProductId = 1 });
			var logger = WithLogger();
			var insuranceController = new InsuranceController(mapper, logger, productService, calculationService);

			//act
			var actionResult = insuranceController.CalculateProductInsurance(Arg.Any<int>());

			//assert
			var statusCodeResult = actionResult.Result as StatusCodeResult;
			Assert.Equal((int)HttpStatusCode.NotFound, statusCodeResult.StatusCode);
		}

		[Fact]
		public void CalculateProductInsurance_WithInvalidCalculation_ShouldReturnBadRequest()
		{
			//arrange
			var (productService, product) = WithProductService();
			var calculationService = WithCalculationServiceGetProductInsuranceReturnsNull();
			var mapper = WithAutoMapper(new ProductInsuranceDto() { ProductId = 1 });
			var logger = WithLogger();
			var insuranceController = new InsuranceController(mapper, logger, productService, calculationService);

			//act
			var actionResult = insuranceController.CalculateProductInsurance(product.Id);

			//assert
			var statusCodeResult = actionResult.Result as StatusCodeResult;
			Assert.Equal((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);
		}

		[Fact]
		public void CalculateProductInsurance_GivenValidProduct_ShouldReturnOk()
		{
			//arrange
			var (productService, product) = WithProductService();
			var calculationService = WithCalculationServiceGetProductInsurance(1500);
			var mapper = WithAutoMapper(new ProductInsuranceDto() { ProductId = 123, SalesPrice = 1000, ProductTypeHasInsurance = true, ProductTypeName = "laptops" });
			var insuranceController = new InsuranceController(mapper, WithLogger(), productService, calculationService);

			//act
			var actionResult = insuranceController.CalculateProductInsurance(product.Id);

			//assert
			var statusCodeResult = (OkObjectResult)actionResult.Result;
			Assert.Equal((int)HttpStatusCode.OK, statusCodeResult.StatusCode);
		}

		[Fact]
		public void CalculateOrderInsurance_GivenNotExistProductInProductList_ShouldReturnNotFound()
		{
			//arrange
			var washingMachine = WithProduct(productTypeName: ProductTypeNameKey.WashingMachine, productTypeId: 13, canBeInsured: true, salesPrice: 2000);
			var productList = new List<Product>(){
				washingMachine
			};
			var productService = WithProductServiceNotExistProduct();
			var calculationService = WithCalculationServiceGetOrderInsurance(productList);
			var mapper = WithAutoMapper(new ProductInsuranceDto() { ProductId = 1 });
			var insuranceController = new InsuranceController(mapper, WithLogger(), productService, calculationService);
			var productIdsList = new List<int>() {1, 2, 3};

			//act
			var actionResult = insuranceController.CalculateOrderInsurance(productIdsList);

			//assert
			var statusCodeResult = actionResult.Result as StatusCodeResult;
			Assert.Equal((int)HttpStatusCode.NotFound, statusCodeResult.StatusCode);
		}

		[Fact]
		public void CalculateOrderInsurance_WithInvalidOrderInsuranceCalculation_ShouldReturnBadRequest()
		{
			//arrange
			var (productService, product) = WithProductService();
			var productList = new List<Product>() {
				product
			};
			var calculationService = WithCalculationServiceGetOrderInsuranceReturnsNull(productList);
			var mapper = WithAutoMapper(new ProductInsuranceDto() { ProductId = 1 });
			var insuranceController = new InsuranceController(mapper, WithLogger(), productService, calculationService);
			var productIdsList = new List<int>() { product.Id };

			//act
			var actionResult = insuranceController.CalculateOrderInsurance(productIdsList);

			//assert
			var statusCodeResult = actionResult.Result as StatusCodeResult;
			Assert.Equal((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);
		}

		[Fact]
		public void CalculateOrderInsurance_WithValidProductList_ShouldReturnOk()
		{
			//arrange
			var (productService, product) = WithProductService();
			var productList = new List<Product>() {
				product
			};
			var calculationService = WithCalculationServiceGetOrderInsurance(productList);
			var mapper = WithAutoMapper(new ProductInsuranceDto() { ProductId = 123, SalesPrice = 1000, ProductTypeHasInsurance = true, ProductTypeName = ProductTypeNameKey.WashingMachine });
			var insuranceController = new InsuranceController(mapper, WithLogger(), productService, calculationService);
			var productIdsList = new List<int>() { product.Id };

			//act
			var actionResult = insuranceController.CalculateOrderInsurance(productIdsList);

			//assert
			var statusCodeResult = (OkObjectResult)actionResult.Result;
			Assert.Equal((int)HttpStatusCode.OK, statusCodeResult.StatusCode);
		}

		private static Product WithProduct(string productTypeName, int productTypeId, bool canBeInsured, decimal salesPrice)
		{
			var productType = new ProductType
			{
				Id = productTypeId,
				Name = productTypeName,
				CanBeInsured = canBeInsured
			};
			return new Product
			{
				SalesPrice = salesPrice,
				ProductTypeId = productType.Id,
				ProductType = productType
			};
		}

		private static IMapper WithAutoMapper(ProductInsuranceDto product)
		{
			var mapper = Substitute.For<IMapper>();
			mapper.Map<ProductInsuranceDto>(Arg.Any<Product>()).Returns(product);
			return mapper;
		}

		private static ILogger<InsuranceController> WithLogger()
		{
			var mapper = Substitute.For<ILogger<InsuranceController>>();
			return mapper;
		}

		private static (IProductService productService, Product product) WithProductService()
		{
			var pt = new ProductType()
			{
				Id = 12,
				Name = ProductTypeNameKey.WashingMachine,
				CanBeInsured = true
			};
			var product = new Product()
			{
				Id = 123,
				SalesPrice = 1000,
				ProductType = pt
			};
			var productService = Substitute.For<IProductService>();
			productService.GetById(Arg.Any<int>()).Returns(product);
			return (productService, product);
		}
		private static IProductService WithProductServiceNotExistProduct()
		{
			var productService = Substitute.For<IProductService>();
			productService.GetById(Arg.Any<int>()).ReturnsNull();
			return productService;
		}

		private static ICalculationService WithCalculationServiceGetProductInsuranceReturnsNull()
		{
			var calculationService = Substitute.For<ICalculationService>();
			calculationService.GetInsuranceForProduct(Arg.Any<Product>()).Returns(i => null);
			return calculationService;
		}

		private static ICalculationService WithCalculationServiceGetOrderInsuranceReturnsNull(List<Product> productList)
		{
			var calculationService = Substitute.For<ICalculationService>();
			calculationService.GetInsuranceForOrder(productList).Returns(i => null);
			return calculationService;
		}

		private static ICalculationService WithCalculationServiceGetProductInsurance(decimal insurance)
		{
			var calculationService = Substitute.For<ICalculationService>();
			calculationService.GetInsuranceForProduct(Arg.Any<Product>()).Returns(insurance);
			return calculationService;
		}

		private static ICalculationService WithCalculationServiceGetOrderInsurance(IEnumerable<Product> productList)
		{
			var calculationService = Substitute.For<ICalculationService>();
			calculationService.GetInsuranceForOrder(productList).ReturnsForAnyArgs(2000);
			return calculationService;
		}
	}
}
