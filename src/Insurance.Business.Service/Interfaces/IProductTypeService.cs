using Insurance.Data.Access.Entities;
using System.Threading.Tasks;

namespace Insurance.Business.Service.Interfaces
{
	public interface IProductTypeService
	{
		Task<ProductType> Update(ProductType productType);
		Task<ProductType> GetById(int id);
	}
}
