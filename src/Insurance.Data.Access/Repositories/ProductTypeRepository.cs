using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Insurance.Data.Access.DBContext;
using Insurance.Data.Access.Entities;
using Insurance.Data.Access.Repositories.Interfaces;

namespace Insurance.Data.Access.Repositories
{
	public class ProductTypeRepository : Repository<ProductType>, IProductTypeRepository
	{
		public ProductTypeRepository(InsuranceContext context) : base(context) { }

		public override IQueryable<ProductType> GetAll() => _Db.Set<ProductType>().AsNoTracking();

		public override async Task<ProductType> GetById(int id)
		{
			return await _Db.Set<ProductType>().AsNoTracking()
				.Where(p => p.Id == id)
				.FirstOrDefaultAsync();
		}
	}
}
