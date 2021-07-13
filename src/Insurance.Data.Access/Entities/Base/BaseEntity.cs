using Insurance.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Data.Access.Entities.Base
{
	public abstract class BaseEntity : IId
	{
		public int Id { get; set; }
	}

	public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
		where TEntity : BaseEntity
	{
		public virtual void Configure(EntityTypeBuilder<TEntity> entity)
		{
			entity.HasKey(e => e.Id)
				.IsClustered(false);

			entity.Property(e => e.Id).ValueGeneratedNever();
		}
	}
}
