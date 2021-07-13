using System.Collections.Generic;
using Insurance.Data.Access.Entities.Base;

namespace Insurance.Data.Access.Entities
{
	public class ProductType : BaseEntity
	{
		public string Name { get; set; }
		public bool CanBeInsured { get; set; }
		public decimal? SurchargeRate { get; set; } = null;

		public virtual ICollection<Product> Products { get; set; }
	}
}
