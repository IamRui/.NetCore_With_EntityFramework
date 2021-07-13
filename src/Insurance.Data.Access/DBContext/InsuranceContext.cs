using System.Collections.Generic;
using System.IO;
using Insurance.Data.Access.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Insurance.Data.Access.DBContext
{
	public class InsuranceContext : DbContext
    {
        public InsuranceContext(DbContextOptions<InsuranceContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
	        modelBuilder.Entity<ProductType>().ToTable("ProductType");
	        modelBuilder.Entity<Product>().ToTable("Product");

	        //seed data
			modelBuilder.Entity<ProductType>().HasData(SeedProductTypeData());
			modelBuilder.Entity<Product>().HasData(SeedProductData());


			base.OnModelCreating(modelBuilder);
        }
		
		private IEnumerable<Product> SeedProductData()
		{
			using var json = new StreamReader(@"product.json");
			return JsonConvert.DeserializeObject<List<Product>>(json.ReadToEnd());
		}

		private IEnumerable<ProductType> SeedProductTypeData()
		{
			using var json = new StreamReader(@"producttype.json");
			return JsonConvert.DeserializeObject<List<ProductType>>(json.ReadToEnd());
		}
	}
}
