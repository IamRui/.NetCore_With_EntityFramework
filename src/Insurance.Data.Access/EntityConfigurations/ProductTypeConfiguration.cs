using Insurance.Data.Access.Entities;
using Insurance.Data.Access.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Data.Access.EntityConfigurations
{
	public class ProductTypeConfiguration : BaseEntityConfiguration<ProductType>, IEntityTypeConfiguration<ProductType>
	{
		public override void Configure(EntityTypeBuilder<ProductType> entity)
		{
			base.Configure(entity);

			entity.ToTable(name: "ProductType", schema: "dbo");

			entity.HasMany(x => x.Products)
				.WithOne(p => p.ProductType)
				.HasForeignKey(p => p.ProductTypeId);
		}
	}
}
