using Insurance.Business.Service.Services;
using Insurance.Data.Access.Entities;
using Insurance.Data.Access.Repositories.Interfaces;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Insurance.Business.Service.Tests.Services
{
	public class ProductTypeServiceTests
	{
		[Fact]
		public async void GetById_GivenValidProductTypeId_ShouldReturnProductType()
		{
			//arrange
			var (productTypeRepository, productType) = WithProductRepositoryGetById();
			var productTypeService = new ProductTypeService(productTypeRepository);

			//act
			var result = await productTypeService.GetById(productType.Id);

			//assert
			Assert.NotNull(result);
			Assert.IsType<ProductType>(result);
		}

		[Fact]
		public async void GetById_WhenProductTypeDoesNotExist_ShouldReturnNull()
		{
			//arrange
			var productTypeRepository = WithProductTypeRepositoryGetByIdReturnNull();
			var productTypeService = new ProductTypeService(productTypeRepository);

			//act
			var result = await productTypeService.GetById(Arg.Any<int>());

			//assert
			Assert.Null(result);
		}

		[Fact]
		public async void Update_WhenProductTypeExists_ShouldUpdate()
		{
			//arrange
			var productType = new ProductType
			{
				Id = 12,
				Name = "Iphone XS",
				CanBeInsured = true,
				SurchargeRate = 30
			};
			var productTypeRepository = Substitute.For<IProductTypeRepository>();
			var productTypeService = new ProductTypeService(productTypeRepository);

			//act
			await productTypeService.Update(productType);

			//assert
			productTypeRepository.Received(1);
		}

		[Fact]
		public async void Update_WhenProductTypeDoesNotExist_ShouldReturnNull()
		{
			//arrange
			var productType = new ProductType
			{
				Id = 7788,
				Name = "Iphone XS",
				CanBeInsured = true,
				SurchargeRate = 30
			};
			var productTypeRepository = Substitute.For<IProductTypeRepository>();
			productTypeRepository.GetById(productType.Id).ReturnsNull();
			var productTypeService = new ProductTypeService(productTypeRepository);

			//act
			var result = await productTypeService.Update(productType);

			//assert
			Assert.Null(result);
			productTypeRepository.Received(0);
		}

		private static IProductTypeRepository WithProductTypeRepositoryGetByIdReturnNull()
		{
			var productTypeRepository = Substitute.For<IProductTypeRepository>();
			productTypeRepository.GetById(Arg.Any<int>()).ReturnsNull();
			return productTypeRepository;
		}

		private static (IProductTypeRepository productTypeRepository, ProductType productType)
			WithProductRepositoryGetById()
		{
			var productType = new ProductType
			{
				Id = 12,
				Name = "Iphone XS",
				CanBeInsured = true,
				SurchargeRate = 30
			};

			var productTypeRepository = Substitute.For<IProductTypeRepository>();
			productTypeRepository.GetById(Arg.Any<int>()).Returns(productType);

			return (productTypeRepository, productType);
		}

	}
}
