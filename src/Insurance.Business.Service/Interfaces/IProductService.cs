using System.Threading.Tasks;
using Insurance.Data.Access.Entities;

namespace Insurance.Business.Service.Interfaces
{
	public interface IProductService
	{
		Task<Product> GetById(int id);
	}
}
