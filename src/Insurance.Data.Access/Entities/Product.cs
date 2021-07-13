using Insurance.Data.Access.Entities.Base;

namespace Insurance.Data.Access.Entities
{
	public class Product : BaseEntity
	{
		public string Name { get; set; }
		public decimal SalesPrice { get; set; }
		public int ProductTypeId { get; set; }

		public virtual ProductType ProductType { get; set; }
	}
}
