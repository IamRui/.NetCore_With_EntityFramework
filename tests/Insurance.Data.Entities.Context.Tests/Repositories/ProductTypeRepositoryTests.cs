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
	public class ProductTypeRepositoryTests : RepositorySetUpBase, IDisposable
	{
		private DbConnection _connection;

		[Fact]
		public void GetAll_ShouldReturnAListOfProductTypesWithCorrectNumberOfProductTypes_WhenProductTypesExist()
		{
			//arrange
			var (_, productTypeRepository, expectedProductTypeTypes) = Init();

			//act
			var productTypeList = productTypeRepository.GetAll().ToList();

			//assert
			Assert.NotEmpty(productTypeList);
			Assert.IsType<List<ProductType>>(productTypeList);
			Assert.Equal(7 + expectedProductTypeTypes.Count, productTypeList.Count);
		}

		[Fact]
		public async void GetAll_ShouldReturnEmptyProductTypeList_WhenProductTypesDoNotExist()
		{
			//arrange
			var (db, productTypeRepository, _) = Init();
			await CleanDb(db);

			//act
			var productTypes = productTypeRepository.GetAll().ToList();

			//assert
			Assert.Empty(productTypes);
			Assert.IsType<List<ProductType>>(productTypes);
		}

		[Fact]
		public async void GetById_ShouldReturnNull_WhenProductTypeWithIdNotExist()
		{
			//arrange
			var (db, productTypeRepository, productTypes) = Init();
			await CleanDb(db);

			//act
			var productType = await productTypeRepository.GetById(productTypes[0].Id);

			//assert
			Assert.Null(productType);
		}

		[Fact]
		public async void GetById_ShouldReturnProductTypeWithCorrectValues_WhenProductTypeExist()
		{
			//arrange
			var (_, productTypeRepository, productTypes) = Init();
			var expectedProductType = productTypes[0];

			//act
			var product = await productTypeRepository.GetById(expectedProductType.Id);

			//assert
			Assert.NotNull(product);
			Assert.Equal(expectedProductType.Id, product.Id);
			Assert.Equal(expectedProductType.Name, product.Name);
			Assert.Equal(expectedProductType.CanBeInsured, product.CanBeInsured);
			Assert.Equal(expectedProductType.SurchargeRate, product.SurchargeRate);
		}

		[Fact]
		public async void UpdateProductType_WhenProductTypeIsValid_ShouldUpdateProductTypeWithCorrectValues()
		{
			//arrange
			var (db, productTypeRepository, _) = Init();
			var productTypeUpdate = new ProductType
			{
				Id = 32,
				SurchargeRate = 30
			};

			//act
			await productTypeRepository.Update(productTypeUpdate);

			//assert
			var updatedProductType = await db.ProductTypes.Where(pt => pt.Id == productTypeUpdate.Id).FirstOrDefaultAsync();
			Assert.NotNull(updatedProductType);
			Assert.Equal(productTypeUpdate.Id, updatedProductType.Id);
			Assert.Equal(productTypeUpdate.SurchargeRate, updatedProductType.SurchargeRate);
		}

		private (InsuranceContext db, ProductTypeRepository productTypeRepository, List<ProductType> productTypeList) Init()
		{
			_connection = DbTest.CreateInMemoryDatabase();

			var db = new InsuranceContext(new DbContextOptionsBuilder<InsuranceContext>()
				.UseSqlite(_connection)
				.Options);

			db.Database.EnsureCreated();

			var productTypeList = new List<ProductType>();

			var productTypeRepository = new ProductTypeRepository(db);

			var smartPhonesProductType = new ProductType()
			{
				Name = "Smartphones",
				CanBeInsured = true,
				SurchargeRate = 30
			};
			db.AddTestEntity(smartPhonesProductType);
			productTypeList.Add(smartPhonesProductType);

			var digitalCamerasProductType = new ProductType()
			{
				Name = "Digital cameras",
				CanBeInsured = false
			};
			
			db.AddTestEntity(digitalCamerasProductType);
			productTypeList.Add(digitalCamerasProductType);

			db.SaveChanges();
			return (db, productTypeRepository, productTypeList);
		}

		public void Dispose()
		{
			_connection.Dispose();
		}
	}
}
