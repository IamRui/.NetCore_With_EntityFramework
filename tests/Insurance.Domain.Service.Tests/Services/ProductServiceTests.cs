using Insurance.Business.Service.Services;
using Insurance.Data.Access.Entities;
using Insurance.Data.Access.Repositories.Interfaces;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Insurance.Business.Service.Tests.Services
{
	public class ProductServiceTests
	{
		[Fact]
		public async void GetById_GivenValidProductId_ShouldReturnProduct()
		{
			//arrange
			var (productRepository, expectedProduct) = WithProductRepository();
			var productService = new ProductService(productRepository);

			//act
			var result = await productService.GetById(expectedProduct.Id);

			//assert
			Assert.NotNull(result);
			Assert.IsType<Product>(result);
		}

		[Fact]
		public async void GetById_WhenProductDoesNotExist_ShouldReturnNull()
		{
			//arrange
			var productRepository = WithProductRepositoryReturnNull();
			var productService = new ProductService(productRepository);

			//act
			var result = await productService.GetById(Arg.Any<int>());

			//assert
			Assert.Null(result);
		}

		private static IProductRepository WithProductRepositoryReturnNull()
		{
			var productRepository = Substitute.For<IProductRepository>();
			productRepository.GetById(Arg.Any<int>()).ReturnsNull();
			return productRepository;
		}

		private static (IProductRepository productRepository, Product product) WithProductRepository()
		{
			var productType = new ProductType
			{
				Id = 12,
				Name = "Iphone XS",
				CanBeInsured = true
			};
			var product = new Product
			{
				SalesPrice = 1200,
				ProductTypeId = productType.Id,
				ProductType = productType
			};
			var productRepository = Substitute.For<IProductRepository>();
			productRepository.GetById(Arg.Any<int>()).Returns(product);
			return (productRepository, product);
		}
	}
}
