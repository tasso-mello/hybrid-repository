namespace data.hybrid.repository.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<T> where T : class
	{
		Task Add(T entity, string idUser);
		Task Add(T entity);
		Task AddAll(IEnumerable<T> entities, string idUser);
		Task AddAll(IEnumerable<T> entities);
		void Update(T entity, string idUser);
		void Update(T entity);
		void UpdateAll(IEnumerable<T> entities, string idUser);
		void UpdateAll(IEnumerable<T> entities);
		void Delete(T entity);
		void Delete(Expression<Func<T, bool>> where);
		Task<bool> Exists(Expression<Func<T, bool>> where);
		Task<int> Count(Expression<Func<T, bool>> where);
		Task<int> Count();
		Task<int> Count(string query, object filter = null);
		Task<T> GetById(long id);
		Task<T> Get(Expression<Func<T, bool>> where, List<string> includes = null);
		Task<T> Get(string query, object filter = null);
		Task<IEnumerable<T>> GetAll(List<string> includes = null);
		Task<IEnumerable<T>> GetAll(string query, int page, int registers, object filter = null);
		Task<IEnumerable<T>> GetAll(string query, object filter = null);
		Task<IEnumerable<T>> GetAll(int skip, int take, List<string> includes = null);
		Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> orderby, int skip, int take, List<string> includes = null);
		Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> where, List<string> includes = null);
		Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> where, int skip, int take, List<string> includes = null);
		Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderby, int skip, int take, List<string> includes = null);
		Task SaveChanges();
	}
}
