using Insurance.Data.Access.Entities;
using Insurance.Data.Access.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Data.Access.EntityConfigurations
{
	public class ProductConfiguration : BaseEntityConfiguration<Product>, IEntityTypeConfiguration<Product>
	{
		public override void Configure(EntityTypeBuilder<Product> entity)
		{
			base.Configure(entity);

			entity.ToTable(name: "Product", schema: "dbo");
		}
	}
}
