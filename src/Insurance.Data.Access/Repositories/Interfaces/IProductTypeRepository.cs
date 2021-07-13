using System.Linq;
using System.Threading.Tasks;
using Insurance.Data.Access.Entities;

namespace Insurance.Data.Access.Repositories.Interfaces
{
	public interface IProductTypeRepository : IRepository<ProductType>
	{
		new IQueryable<ProductType> GetAll();
		new Task<ProductType> GetById(int id);
	}
}
