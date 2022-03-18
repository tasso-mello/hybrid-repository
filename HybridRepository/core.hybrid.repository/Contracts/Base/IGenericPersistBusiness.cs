namespace core.hybrid.repository.Contracts.Base
{
    using System;
    using System.Threading.Tasks;

	public interface IGenericPersistBusiness<TEntity> where TEntity : class
	{
		Task<object> Save(TEntity obj);
		Task<object> Update(TEntity obj);
		Task<object> Delete(Guid id);
	}
}
