using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Insurance.Data.Access.Entities.Base;

namespace Insurance.Data.Access.Repositories.Interfaces
{
	public interface IRepository<TEntity> : IDisposable where TEntity : BaseEntity
	{
		Task Add(TEntity entity);
		IQueryable<TEntity> GetAll();
		Task<TEntity> GetById(int id);
		Task Update(TEntity entity);
		Task Remove(TEntity entity);
		Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate);
		Task<int> SaveChanges();
	}
}
