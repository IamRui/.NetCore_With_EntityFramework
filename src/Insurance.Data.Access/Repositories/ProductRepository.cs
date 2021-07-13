using System.Linq;
using System.Threading.Tasks;
using Insurance.Data.Access.DBContext;
using Insurance.Data.Access.Entities;
using Insurance.Data.Access.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Data.Access.Repositories
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		public ProductRepository(InsuranceContext context) : base(context) { }

		public override IQueryable<Product> GetAll() => _Db.Set<Product>().AsNoTracking().Include(p => p.ProductType);

		public override async Task<Product> GetById(int id)
		{
			return await _Db.Set<Product>().AsNoTracking().Include(p => p.ProductType)
				.Where(p => p.Id == id)
				.FirstOrDefaultAsync();
		}
	}
}
