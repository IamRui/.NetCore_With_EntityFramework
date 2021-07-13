using System.Threading.Tasks;
using Insurance.Data.Access.DBContext;

namespace Insurance.Data.Access.Tests.Shared
{
	public abstract class RepositorySetUpBase 
	{
		protected static async Task CleanDb(InsuranceContext context)
		{
			foreach (var contextProduct in context.Products)
			{
				context.Products.Remove(contextProduct);
			}

			foreach (var contextProductType in context.ProductTypes)
			{
				context.ProductTypes.Remove(contextProductType);
			}

			await context.SaveChangesAsync();
		}
	}
}
