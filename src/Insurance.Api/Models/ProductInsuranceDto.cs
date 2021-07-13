namespace Insurance.Api.Models
{
	public class ProductInsuranceDto 
	{
		public int ProductId { get; set; }
		public decimal? InsuranceValue { get; set; }
		public string ProductTypeName { get; set; }
		public bool ProductTypeHasInsurance { get; set; }
		public float SalesPrice { get; set; }
	}
}
