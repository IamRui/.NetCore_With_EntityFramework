using System;
using System.Data.Common;
using Insurance.Data.Access.Entities.Base;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Insurance.TestUtils
{
	public static class DbTest
	{
		public static DbConnection CreateInMemoryDatabase()
		{
			var connection = new SqliteConnection("Filename=:memory:");
			connection.Open();
			return connection;
		}

		public static int AddTestEntity<TEntity>(this DbContext db, TEntity entity) where TEntity : BaseEntity
		{

			entity.Id = BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0); ;
			db.Set<TEntity>().Add(entity);
			return entity.Id;
		}
	}
}
