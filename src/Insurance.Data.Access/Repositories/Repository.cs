using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Insurance.Data.Access.DBContext;
using Insurance.Data.Access.Entities.Base;
using Insurance.Data.Access.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Data.Access.Repositories
{
	public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
	{
		protected readonly InsuranceContext _Db;

		protected readonly DbSet<TEntity> DbSet;

		protected Repository(InsuranceContext db)
		{
			_Db = db;
			DbSet = db.Set<TEntity>();
		}

		public virtual async Task Add(TEntity entity)
		{
			DbSet.Add(entity);
			await SaveChanges();
		}

		public virtual IQueryable<TEntity> GetAll()
		{
			return DbSet.AsNoTracking();
		}

		public virtual async Task<TEntity> GetById(int id)
		{
			return await DbSet.FindAsync(id);
		}

		public virtual async Task Update(TEntity entity)
		{
			DbSet.Update(entity);
			await SaveChanges();
		}

		public virtual async Task Remove(TEntity entity)
		{
			DbSet.Remove(entity);
			await SaveChanges();
		}

		public async Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
		{
			return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
		}

		public async Task<int> SaveChanges()
		{
			return await _Db.SaveChangesAsync();
		}

		public void Dispose()
		{
			_Db?.Dispose();
		}
	}
}
