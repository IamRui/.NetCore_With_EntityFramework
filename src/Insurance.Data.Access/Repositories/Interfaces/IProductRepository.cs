using System.Linq;
using System.Threading.Tasks;
using Insurance.Data.Access.Entities;

namespace Insurance.Data.Access.Repositories.Interfaces
{
	public interface IProductRepository : IRepository<Product>
	{
		new IQueryable<Product> GetAll();
		new Task<Product> GetById(int id);
	}
}
