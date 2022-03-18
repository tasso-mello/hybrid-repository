namespace core.hybrid.repository.Contracts.Base
{
    using System;
    using System.Threading.Tasks;

	public interface IGenericReadBusiness
	{
		Task<object> Get(int skip, int take);
		Task<object> GetById(Guid id);
		Task<object> GetByString(string name);
	}
}
