using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Insurance.Data.Access.DBContext;
using Insurance.Data.Access.Entities;
using Insurance.Data.Access.Repositories;
using Insurance.Data.Access.Tests.Shared;
using Insurance.TestUtils;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Insurance.Data.Access.Tests.Repositories
{
	public class ProductRepositoryTests : RepositorySetUpBase, IDisposable
	{
		private DbConnection _connection;

		[Fact]
		public void GetAll_ShouldReturnAListOfProductsWithCorrectNumberOfProducts_WhenProductsExist()
		{
			//arrange
			var (_, productRepository, expectedProducts) = Init();

			//act
			var productList = productRepository.GetAll().ToList();

			//assert
			Assert.NotEmpty(productList);
			Assert.IsType<List<Product>>(productList);
			Assert.Equal(19 + expectedProducts.Count, productList.Count);
		}

		[Fact]
		public async void GetAll_ShouldReturnEmptyProductList_WhenProductsDoNotExist()
		{
			//arrange
			var (db, productRepository, _) = Init();
			await CleanDb(db);

			//act
			var productList = productRepository.GetAll().ToList();

			//assert
			Assert.Empty(productList);
			Assert.IsType<List<Product>>(productList);
		}

		[Fact]
		public async void GetById_ShouldReturnNull_WhenProductWithIdNotExist()
		{
			//arrange
			var (db, productRepository, products) = Init();
			await CleanDb(db);

			//act
			var product = await productRepository.GetById(products[0].Id);

			//assert
			Assert.Null(product);
		}

		[Fact]
		public async void GetById_ShouldReturnProductWithCorrectValues_WhenProductExist()
		{
			//arrange
			var (_, productRepository, products) = Init();
			var expectedProduct = products[0];

			//act
			var product = await productRepository.GetById(expectedProduct.Id);

			//assert
			Assert.NotNull(product);
			Assert.Equal(expectedProduct.Id, product.Id);
			Assert.Equal(expectedProduct.Name, product.Name);
			Assert.Equal(expectedProduct.SalesPrice, product.SalesPrice);
			Assert.Equal(expectedProduct.ProductTypeId, product.ProductTypeId);
			Assert.Equal(expectedProduct.ProductType.Name, product.ProductType.Name);
			Assert.Equal(expectedProduct.ProductType.CanBeInsured, product.ProductType.CanBeInsured);
		}

		private (InsuranceContext db, ProductRepository productRepository, List<Product> products) Init()
		{
			_connection = DbTest.CreateInMemoryDatabase();

			var db = new InsuranceContext(new DbContextOptionsBuilder<InsuranceContext>()
				.UseSqlite(_connection)
				.Options);

			db.Database.EnsureCreated();

			var products = new List<Product>();

			var productRepository = new ProductRepository(db);

			var smartPhonesProductTypeId = db.AddTestEntity(new ProductType()
			{
				Name = "Smartphones",
				CanBeInsured = true
			});

			var productIphone = new Product()
			{
				Name = "Iphone X",
				SalesPrice = 1000,
				ProductTypeId = smartPhonesProductTypeId
			};
			db.AddTestEntity(productIphone);
			products.Add(productIphone);

			var digitalCamerasProductTypeId = db.AddTestEntity(new ProductType()
			{
				Name = "Digital cameras",
				CanBeInsured = false
			});

			var productDigitalCamera = new Product()
			{
				Name = "Cannon G7X",
				SalesPrice = 200,
				ProductTypeId = digitalCamerasProductTypeId
			};
			db.AddTestEntity(productDigitalCamera);
			products.Add(productDigitalCamera);

			db.SaveChanges();
			return (db, productRepository, products);
		}

		public void Dispose()
		{
			_connection.Dispose();
		}
	}
}
