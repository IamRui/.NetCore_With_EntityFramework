using AutoMapper;
using Insurance.Api.Controllers;
using Insurance.Api.Models;
using Insurance.Business.Service.Const;
using Insurance.Business.Service.Interfaces;
using Insurance.Data.Access.Entities;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Net;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Insurance.Api.Tests.Controllers
{
	public class ProductTypeControllerTests
	{
		[Fact]
		public void UploadSurchargeRate_GivenMismatchedProductTypeIdInRoute_ShouldReturnBadRequest()
		{
			//arrange
			var productTypeService = WithProductTypeService();
			var mapper = WithAutoMapper();
			var productTypeController = new ProductTypeController(mapper, WithLogger(), productTypeService);
			var productTypeUpdate = new ProductTypeDto() {ProductTypeId = 12, SurchargeRate = 0.03};

			//act
			var actionResult = productTypeController.UploadSurchargeRate(11, productTypeUpdate);

			//assert
			var statusCodeResult = actionResult.Result as StatusCodeResult;
			Assert.Equal((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);
		}

		[Fact]
		public void UploadSurchargeRate_GivenNotExistProductType_ShouldReturnNotFound()
		{
			//arrange
			var productTypeService = WithProductTypeServiceNotExistProductType();
			var mapper = WithAutoMapper();
			var productTypeController = new ProductTypeController(mapper, WithLogger(), productTypeService);
			var productTypeUpdate = new ProductTypeDto() { ProductTypeId = 12, SurchargeRate = 0.03 };

			//act
			var actionResult = productTypeController.UploadSurchargeRate(productTypeUpdate.ProductTypeId, productTypeUpdate);

			//assert
			var statusCodeResult = actionResult.Result as StatusCodeResult;
			Assert.Equal((int)HttpStatusCode.NotFound, statusCodeResult.StatusCode);
		}

		[Fact]
		public void UploadSurchargeRate_GivenValidProductType_ShouldReturnNoContent()
		{
			//arrange
			var productTypeService = WithProductTypeService();
			var mapper = WithAutoMapper();
			var productTypeController = new ProductTypeController(mapper, WithLogger(), productTypeService);
			var productTypeUpdate = new ProductTypeDto() { ProductTypeId = 12, SurchargeRate = 0.03 };

			//act
			var actionResult = productTypeController.UploadSurchargeRate(productTypeUpdate.ProductTypeId, productTypeUpdate);

			//assert
			var statusCodeResult = actionResult.Result as StatusCodeResult;
			Assert.Equal((int)HttpStatusCode.NoContent, statusCodeResult.StatusCode);
		}

		private static IMapper WithAutoMapper()
		{
			var mapper = Substitute.For<IMapper>();
			mapper.Map<ProductTypeDto, ProductType>(Arg.Any<ProductTypeDto>(), Arg.Any<ProductType>());
			return mapper;
		}
		private static ILogger<ProductTypeController> WithLogger()
		{
			var mapper = Substitute.For<ILogger<ProductTypeController>>();
			return mapper;
		}

		private static IProductTypeService WithProductTypeService()
		{
			var productType = new ProductType()
			{
				Id = 12,
				Name = ProductTypeNameKey.WashingMachine,
				CanBeInsured = true,
				SurchargeRate = 30
			};
			var productService = Substitute.For<IProductTypeService>();
			productService.GetById(Arg.Any<int>()).Returns(productType);
			return productService;
		}

		private static IProductTypeService WithProductTypeServiceNotExistProductType()
		{
			var productTypeService = Substitute.For<IProductTypeService>();
			productTypeService.GetById(Arg.Any<int>()).ReturnsNull();
			return productTypeService;
		}
	}
}
