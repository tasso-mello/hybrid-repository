namespace data.hybrid.repository.Infrastructure
{
    using Dapper;
    using data.hybrid.repository.Context;
    using data.hybrid.repository.Entities.Base;
    using data.hybrid.repository.Service;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;

    public class RepositoryBase<T> : IRepository<T> where T : class
	{
		#region Properties

		private readonly ApplicationDbContext _dataContext;
		private readonly DbSet<T> _dbSet;
		private readonly IDapperDbService _conn;

		#endregion

		protected RepositoryBase(ApplicationDbContext dbContext)
		{
			_dataContext = dbContext;
			_dbSet = _dataContext.Set<T>();
		}
		protected RepositoryBase(ApplicationDbContext dbContext, IDapperDbService dapperDbService)
		{
			_dataContext = dbContext;
			_dbSet = _dataContext.Set<T>();
			_conn = dapperDbService;
		}

		#region Implementation

		public virtual async Task Add(T entity, string idUser)
		{
			await _dbSet.AddAsync(entity);
		}

		public virtual async Task Add(T entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public virtual async Task AddAll(IEnumerable<T> entities, string idUser)
		{
			await _dbSet.AddRangeAsync(entities);
		}

		public virtual async Task AddAll(IEnumerable<T> entities)
		{
			await _dbSet.AddRangeAsync(entities);
		}

		public virtual void Update(T entity, string idUser)
		{
			_dbSet.Attach(entity);
			_dataContext.Entry(entity).State = EntityState.Modified;
		}

		public virtual void Update(T entity)
		{
			_dbSet.Attach(entity);
			_dataContext.Entry(entity).State = EntityState.Modified;
		}

		public virtual void UpdateAll(IEnumerable<T> entities, string idUser)
		{
			_dbSet.AttachRange(entities);
			foreach (var entity in entities)
				_dataContext.Entry(entity).State = EntityState.Modified;
		}

		public virtual void UpdateAll(IEnumerable<T> entities)
		{
			_dbSet.AttachRange(entities);
			foreach (var entity in entities)
				_dataContext.Entry(entity).State = EntityState.Modified;
		}

		public virtual void Delete(T entity)
		{
			_dbSet.Remove(entity);
			_dataContext.Entry(entity).State = EntityState.Deleted;
		}

		public virtual void Delete(Expression<Func<T, bool>> where)
		{
			IEnumerable<T> objects = _dbSet.Where<T>(where).AsEnumerable();
			foreach (T obj in objects)
			{
				Delete(obj);
			}
		}

		public virtual async Task<bool> Exists(Expression<Func<T, bool>> where)
			=> await _dbSet.AnyAsync(where);

		public virtual async Task<int> Count()
			=> await _dbSet.CountAsync();

		public virtual async Task<int> Count(Expression<Func<T, bool>> where)
			 => await _dbSet.Where(where).CountAsync();

		public async Task<int> Count(string query, object filter = null)
		{
			if (filter == null)
				return await _conn.GetConnection().QueryFirstAsync<int>(query);
			else
				return await _conn.GetConnection().QueryFirstAsync<int>(query, GetInstanceFilterObject(filter));
		}

		public virtual async Task<T> GetById(long id)
		{
			return await _dbSet.FindAsync(id);
		}

		public virtual async Task<IEnumerable<T>> GetAll(List<string> includes = null)
		{
			var query = _dbSet.Select(c => c);
			if (includes != null)
			{
				includes.ForEach(include => query = query.Include(include));
			}
			return await query.ToListAsync();
		}

		public virtual async Task<IEnumerable<T>> GetAll(string query, int page, int registers, object filter = null)
		{
			if (filter == null)
				return await _conn.GetConnection().QueryAsync<T>(query, new { page = ((page - 1) * registers), registers = registers });
			else
			{
				return await _conn.GetConnection().QueryAsync<T>(query, GetInstanceFilterObject(filter, page, registers));
			}
		}

		public virtual async Task<IEnumerable<T>> GetAll(string query, object filter = null)
		{
			if (filter == null)
				return await _conn.GetConnection().QueryAsync<T>(query);
			else
			{
				return await _conn.GetConnection().QueryAsync<T>(query, filter);
			}
		}

		public virtual async Task<IEnumerable<T>> GetAll(int skip, int take, List<string> includes = null)
		{
			var query = _dbSet.Select(c => c).Skip((skip - 1) * take).Take(take);

			if (includes != null)
				includes.ForEach(include => query = query.Include(include));

			return await query.ToListAsync();
		}

		public virtual async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> orderby, int skip, int take, List<string> includes = null)
		{
			var query = _dbSet.OrderBy(orderby).Select(c => c).Skip((skip - 1) * take).Take(take);

			if (includes != null)
			{
				includes.ForEach(include => query = query.Include(include));
			}
			return await query.ToListAsync();
		}

		public virtual async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> where, List<string> includes = null)
		{
			var query = _dbSet.Select(c => c).Where(where);

			if (includes != null)
				includes.ForEach(include => query = query.Include(include));

			return await query.ToListAsync();
		}

		public virtual async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> where, int skip, int take, List<string> includes = null)
		{
			var query = _dbSet.Select(c => c).Where(where).Skip((skip - 1) * take).Take(take);

			if (includes != null)
			{
				includes.ForEach(include => query = query.Include(include));
			}
			return await query.ToListAsync();
		}

		public async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderby, int skip, int take, List<string> includes = null)
		{
			var query = _dbSet.Select(c => c).Where(where).OrderBy(orderby).Skip((skip - 1) * take).Take(take);

			if (includes != null)
			{
				includes.ForEach(include => query = query.Include(include));
			}
			return await query.ToListAsync();
		}

		public virtual async Task<T> Get(Expression<Func<T, bool>> where, List<string> includes = null)
		{
			var query = _dbSet.Select(c => c).Where(where);
			if (includes != null)
			{
				includes.ForEach(include => query = query.Include(include));
			}

			return await query.FirstOrDefaultAsync<T>();
		}

		public virtual async Task<T> Get(string query, object filter = null)
		{
			if (filter == null)
				return await _conn.GetConnection().QueryFirstAsync<T>(query);
			else
				return await _conn.GetConnection().QueryFirstAsync<T>(query, filter);
		}

		public virtual async Task SaveChanges()
		{
			await _dataContext.SaveChangesAsync();
		}

		private object GetInstanceFilterObject(object obj)
		{
			var property = new ExpandoObject() as IDictionary<string, object>;

			foreach (PropertyInfo currentProperty in obj.GetType().GetProperties())
				if (currentProperty.GetValue(obj, null) != null)
					property.Add(currentProperty.Name, currentProperty.GetValue(obj));

			return property.Count == 0 ? null : property;
		}

		private object GetInstanceFilterObject(object obj, int page, int registers)
		{
			var property = new ExpandoObject() as IDictionary<string, object>;

			foreach (PropertyInfo currentProperty in obj.GetType().GetProperties())
				property.Add(currentProperty.Name, currentProperty.GetValue(obj));

			property.Add("page", ((page - 1) * registers));
			property.Add("registers", registers);

			return property;
		}

		#endregion
	}
}
